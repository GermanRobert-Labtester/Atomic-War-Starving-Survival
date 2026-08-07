using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// L-9: Create a salted Random from the world seed + a string context.
        /// Uses HashCode.Combine so different contexts never produce coincident
        /// streams for the same world seed, even if added offsets collide.
        /// Example: CreateSaltedRng(42, "ai") != CreateSaltedRng(42, "mental_break").
        /// </summary>
        private static System.Random CreateSaltedRng(int worldSeed, string context)
        {
            int salted = HashCode.Combine(worldSeed, context);
            return new System.Random(salted);
        }

        /// <summary>
        /// Held while Awake restores a save requested from the main menu, so the
        /// OnPhaseChanged -> AutoSave hook wired in InitSaveAndExpeditions does
        /// not fire during the restore. See <see cref="Awake"/>.
        /// </summary>
        private bool _suppressAutoSave;

        private void Awake()
        {
            InitializeSystems();
            WireHUD();
            ApplyPendingGameLoad();

            // Only flip to Running if the restore did not already land us
            // somewhere meaningful. The IsGameOver guard preserves LoadGame's
            // terminal-state branch -- without it, continuing into a finished
            // run would resurrect it and immediately autosave over the ending.
            if (GameState.Phase != GamePhase.Running && !IsGameOver)
            {
                GameState.Phase = GamePhase.Running;
            }
        }

        /// <summary>
        /// Consume the main menu's "Continue" request, if any, and restore that
        /// slot.
        ///
        /// Ordering is load-bearing: this must run BEFORE Phase becomes Running.
        /// Setting Phase = Running fires the autosave hook, which would
        /// overwrite save_autosave.json with this freshly-initialised, empty
        /// world -- destroying the very save the player asked to continue before
        /// it could be read.
        ///
        /// Entering play mode directly on the gameplay scene leaves no pending
        /// slot, so this is a no-op and the fresh-game path is unchanged.
        /// </summary>
        private void ApplyPendingGameLoad()
        {
            string slotId = PendingGameLoad.ConsumeSlotId();
            if (string.IsNullOrEmpty(slotId)) return;

            _suppressAutoSave = true;
            try
            {
                if (!LoadGame(slotId))
                {
                    Debug.LogWarning(
                        $"[GameBootstrap] Continue requested slot '{slotId}', but it could not be " +
                        "loaded (missing or corrupt). Starting a fresh game instead.");
                }
            }
            finally
            {
                _suppressAutoSave = false;
            }
        }

        /// <summary>
        /// Release every subscription, event handler, and disposable that
        /// the bootstrap accumulated during InitializeSystems. Called by
        /// Unity when the GameObject is destroyed (e.g. scene unload,
        /// "new game" flow, or test teardown). Idempotent.
        /// </summary>
        private void OnDestroy()
        {
            // EventBus subscription registered in InitializeSystems.
            EventBus.Unsubscribe<FlashpointEmptiedDevices>(OnFlashpointEmp_UnlockGhosts);

            // Class-level static events accumulated by InitializeSystems.
            // The unsubscribe pattern must match the subscribe pattern
            // (same delegate instance) — we kept a reference to the lambdas
            // in OnDestroy-cached fields.
            if (_onWorldPhaseChanged != null) WorldPhaseSystem.OnPhaseChanged -= _onWorldPhaseChanged;
            if (_onGameStateChanged != null) GameState.OnPhaseChanged -= _onGameStateChanged;
            if (_onNeedsDied != null) NeedsSystem.OnDied -= _onNeedsDied;
            if (_onNeedChanged != null) NeedsSystem.OnNeedChanged -= _onNeedChanged;

            // Companion-system cleanup.
            SaveSystem?.Dispose();
            ExpeditionSystem?.UnsubscribeAll();
            // Prompt #864 — drop process-wide Active so importers do not
            // keep a destroyed bootstrap's mod loader.
            ModLoader?.ClearActiveIfSelf();
            // Note: AudioEventBus is a process-wide service, not owned by
            // the bootstrap. Its lifetime is managed by the gameplay scene
            // (or by tests). Skip Teardown here to avoid breaking other
            // systems that share the bus.

            // Note: EventBus.Clear() is NOT called here — the EventBus is
            // process-wide and may have subscribers from other systems
            // (MoralChronicleBridge, AudioEventBus). Per-system cleanup
            // is the right granularity; clearing the whole bus would
            // break other systems on the next raise.
        }

        private void Update()
        {
            if (GameState.Phase != GamePhase.Running) return;
            if (IsGameOver) return;

            float dt = Time.unscaledDeltaTime;

            // Day-30 Flashpoint choreography runs in real time (the flash is a
            // visual event, not a game-time event). Tick before the rest of
            // the systems so the EMP step's side effects (radiation pause,
            // weather force) are visible to the same frame's HUD push.
            FlashpointChoreographer?.Tick(dt);

            // Prompt #865 — Twitch poll timers / cooldowns are real-time while connected.
            TickTwitchAPI(dt);
            // Batch — speedrun real-time seconds.
            TickBatchSystemsRealtime(dt);
            // Victory paths — airlift defense real-time timer when active.
            TickVictoryPathsRealtime(dt);

            // Fast-forward-safe clock: TimeScale (1x / 3x) scales the simulated
            // delta, and the accumulated game-time is consumed in sub-steps of
            // at most TimeSystem.MaxGameHoursPerStep so systems + AI see every
            // hour chunk — large deltas (3x, hitches) never skip ticks.
            TickFrame(Time.unscaledDeltaTime);
            CheckWinLose();

            // Push environment data to HUD every frame
            if (_hud != null)
            {
                string weatherName = GetWeatherDisplayName(WeatherSystem.Current);
                string seasonName = TemperatureSystem.CurrentSeason?.displayName ?? "Nuclear Winter";
                _hud.Tick(TimeSystem.CurrentDay, TimeSystem.CurrentHourFloat, weatherName, seasonName, TimeSystem.TimeScale);
                _hud.OnShelterUpdated(Shelter);
                // Live radio hardware (signal + tuned label) on the intercept strip.
                PushRadioLiveStateToHud();
                // Internal Horror status strip (corpses / fire / coma / rust).
                RefreshInternalHorrorHud();
            }
        }

        private WeatherKind _cachedWeatherKind;
        private string _cachedWeatherName;

        /// <summary>
        /// Enum-to-label lookup for the HUD's weather strip.
        /// <c>Enum.ToString()</c> allocates a fresh string on every call, and
        /// this sits in <see cref="Update"/>, so it was producing one dead
        /// string per rendered frame. Weather changes at most a few times per
        /// in-game day, so caching the last result removes the allocation
        /// entirely without changing what the player sees.
        /// </summary>
        private string GetWeatherDisplayName(WeatherKind kind)
        {
            if (_cachedWeatherName == null || _cachedWeatherKind != kind)
            {
                _cachedWeatherKind = kind;
                _cachedWeatherName = kind.ToString();
            }
            return _cachedWeatherName;
        }

        /// <summary>
        /// Drive one frame's worth of game-time. Public so PlayMode tests
        /// can call it with a controlled dt and exercise the substep
        /// watchdog directly. The Unity Update() loop also calls this with
        /// the unscaled real-time delta. The substep loop is bounded by
        /// <see cref="MaxSubstepsPerFrame"/>; if the per-frame budget is
        /// exhausted, the leftover rolls into the next frame's budget (no
        /// time is lost). The watchdog counters are updated on overflow.
        /// </summary>
        public void TickFrame(float dt)
        {
            if (GameState.Phase != GamePhase.Running) return;
            if (IsGameOver) return;

            _pendingGameHours += dt * TimeSystem.TimeScale / TimeSystem.SecondsPerGameHour;
            int steps = 0;
            while (_pendingGameHours > 0f && steps < MaxSubstepsPerFrame)
            {
                float step = Mathf.Min(_pendingGameHours, TimeSystem.MaxGameHoursPerStep);
                _pendingGameHours -= step;
                TimeSystem.TickHours(step);
                TickSystems(step);
                steps++;
            }

            // H-1: TimeSystem substep watchdog.
            if (steps == MaxSubstepsPerFrame && _pendingGameHours > 0f)
            {
                DropEventCount++;
                LastFrameDroppedGameHours = _pendingGameHours;
                TotalDroppedGameHours += _pendingGameHours;
                // Throttled log: every Nth event (avoids log spam if the
                // player spends a long time on a menu then returns at 3x).
                if (DropEventCount == 1 || DropEventCount % WatchdogLogEveryNEvents == 0)
                {
                    Debug.LogWarning(
                        $"[TimeSystemWatchdog] Frame exceeded {MaxSubstepsPerFrame} substeps. " +
                        $"Pending={_pendingGameHours:F2} game-hours carried into next frame. " +
                        $"TotalDroppedHours={TotalDroppedGameHours:F2}. " +
                        $"Increase TimeSystem.MaxGameHoursPerStep to reduce carry, " +
                        $"or lower the fast-forward scale, if this happens often.");
                }
            }
            else
            {
                LastFrameDroppedGameHours = 0f;
            }
            if (steps > PeakSubstepsInOneFrame)
            {
                PeakSubstepsInOneFrame = steps;
            }
        }

        /// <summary>
        /// H-5: Diagnostic check run at the end of InitializeSystems.
        /// Uses reflection to find all public non-null system properties and
        /// verifies each appears in the registry. Logs a warning for any
        /// that are missing — indicating a C-1 class bug.
        /// </summary>
        private void VerifyAllSystemsRegistered()
        {
            if (_registry == null) return;
            var unticked = GetUntickedSystemNames();
            if (unticked.Count > 0)
            {
                Debug.LogWarning(
                    $"[SystemRegistry] {unticked.Count} system(s) constructed but not registered " +
                    $"in any tick category (C-1 class bug): {string.Join(", ", unticked)}");
            }
        }

        /// <summary>
        /// L-1: Warn on startup if critical ScriptableObject assets are
        /// unassigned in the inspector. Missing SOs cause silent failures
        /// (empty catalogs, no events, missing recipes) rather than hard
        /// crashes, so the warning gives designers a clear signal.
        /// </summary>
        private void VerifyCriticalScriptableObjects()
        {
            if (_needsProfile == null)
                Debug.LogWarning("[GameBootstrap] NeedsProfile is unassigned. Needs decay will use C# defaults (no ScriptableObject override).");
            if (_itemCatalog == null)
                Debug.LogWarning("[GameBootstrap] ItemCatalogSO is unassigned. No items will be available in inventory/loot.");
            if (_recipeCatalog == null)
                Debug.LogWarning("[GameBootstrap] RecipeCatalogSO is unassigned. Crafting will have no recipes.");
            if (_eventCatalog == null)
                Debug.LogWarning("[GameBootstrap] GameEventCatalogSO is unassigned. Event pool will only contain factory events (no authored events).");
            if (_locationCatalog == null)
                Debug.LogWarning("[GameBootstrap] LocationCatalogSO is unassigned. Expeditions will have no map nodes.");
            if (_radioCatalog == null)
                Debug.LogWarning("[GameBootstrap] RadioCatalogSO is unassigned. Radio tuner will have no broadcasts.");
            if (_worldPhaseConfig == null)
                Debug.LogWarning("[GameBootstrap] WorldPhaseConfigSO is unassigned. World phase transitions will not fire.");
            if (_mentalBreakCatalog == null)
                Debug.LogWarning("[GameBootstrap] MentalBreakCatalogSO is unassigned. Mental breaks will be disabled.");
            if (_lootTable == null)
                Debug.LogWarning("[GameBootstrap] LootTableSO is unassigned. Scavenging will produce no loot.");
        }
    }
}
