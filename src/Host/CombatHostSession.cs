// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the combat core (Expansion 6). Presents the
    /// engine, wires host ports (inventory ammo/scrap/loot, survivor health/
    /// morale/injury) when available, persists to user:// via CombatSaveStore,
    /// and exposes typed snapshots + real player actions for the Combat panels.
    /// No gameplay rules live here — hosts only present and wire.
    /// </summary>
    public sealed class CombatHostSession : HostSessionBase, IWiringReporter
    {
        public const int DemoSeed = 4242;

        private readonly ICampaignRngManager? _campaignRng;
        private readonly CombatFactionStandingBridge _factionStandingBridge = new();
        private Action<string, int>? _applyFactionStanding;

        public TacticalCombatSystem Engine { get; }
        public event Action<CombatFactionConsequence>? FactionConsequenceApplied;

        /// <summary>Optional real inventory backing for ammo / scrap / loot.</summary>
        public InventoryHostSession Inventory { get; set; }

        /// <summary>Optional real survivor backing for health / morale / injury.</summary>
        public SurvivorsHostSession Survivors { get; set; }

        /// <summary>
        /// Optional equipment-condition authority. When set, the default weapon
        /// loadout is projected from its Weapon-family instances (condition
        /// 0–100 → combat 0–1) and combat wear is written back through the
        /// authority when the encounter ends — one persisted condition per
        /// weapon, no duplicate durability. Unset: legacy demo literals.
        /// </summary>
        public Ashfall.Core.EquipmentConditionSystem? Equipment { get; set; }

        /// <summary>
        /// Optional bounded ballistics projection. When set, persisted
        /// calibration/optic state is applied to each combat weapon token at
        /// encounter start.
        /// </summary>
        public BallisticsWorkbenchSystem? Ballistics { get; set; }

        /// <summary>
        /// Optional CBRN authority. When set, <see cref="ActionEndTurn"/> evaluates
        /// toxic exposure for living combatants in active hazard lanes.
        /// </summary>
        public ChemWarfareSystem? ChemWarfare { get; set; }

        /// <summary>Optional stealth authority. Fired weapons add registered noise.</summary>
        public StealthSystem? Stealth { get; set; }

        /// <summary>Expedition id used when applying weapon noise. Defaults to the live combat encounter.</summary>
        public string StealthExpeditionId { get; set; } = "combat_active";

        /// <summary>Condition-at-start of bridge-bound weapons, for the post-combat write-back.</summary>
        private readonly Dictionary<string, float> _boundWeaponConditionAtStart = new();
        private string _boundWeaponsSyncedForResolution = string.Empty;

        public string LastEvent { get; private set; } = string.Empty;
        public CombatHostSession(TacticalCombatSystem engine = null!, CombatHostPorts ports = null!, ICampaignRngManager? campaignRng = null)
        {
            _campaignRng = campaignRng;
            Engine = engine ?? new TacticalCombatSystem(null!, ports ?? CombatHostPorts.NoOp());
            Engine.OnStateChanged += _ => RaiseStateChanged();
            Engine.OnCombatEvent += (s, e) => { LastEvent = e.Detail; RaiseStateChanged(); };
            _factionStandingBridge.OnConsequenceApplied += consequence =>
                FactionConsequenceApplied?.Invoke(consequence);
            Engine.OnEncounterEnded += s =>
            {
                LastEvent = "Combat ended: " + s.OutcomeText;
                SyncBoundWeaponsAfterCombat(s);
                int factionConsequences = ApplyFactionConsequences(s);
                if (factionConsequences > 0)
                    LastEvent += $" · {factionConsequences} faction consequence(s) recorded";
                RaiseStateChanged();
            };
        }

        /// <summary>
        /// Bind the one canonical faction-standing mutation path. If a resolved
        /// encounter was restored with pending consequences, binding retries it;
        /// persisted incident markers prevent replay after a completed apply.
        /// </summary>
        public void ConfigureFactionStanding(Action<string, int>? applyStanding)
        {
            _applyFactionStanding = applyStanding;
            if (Engine.State.Resolved)
            {
                int applied = ApplyFactionConsequences(Engine.State);
                if (applied > 0) RaiseStateChanged();
            }
        }

        private int ApplyFactionConsequences(CombatState state)
        {
            if (state == null) return 0;
            var consequences = CombatFactionStandingBridge.EvaluateConsequences(
                state, state.IsSelfDefense);
            int applied = _factionStandingBridge.ApplyConsequences(
                consequences,
                _applyFactionStanding,
                state.AppliedFactionConsequenceIds);
            state.FactionConsequences = TacticalCombatSystem.CloneFactionConsequences(
                new List<CombatFactionConsequence>(consequences));
            return applied;
        }

        /// <summary>
        /// Single post-combat write-back point: bridge-bound weapons sync their
        /// condition delta into the equipment authority. Runs once per
        /// encounter end; the snapshot is cleared after syncing.
        /// </summary>
        private void SyncBoundWeaponsAfterCombat(CombatState state)
        {
            if (Equipment == null || state?.Weapons == null)
            {
                _boundWeaponConditionAtStart.Clear();
                return;
            }

            if (!string.IsNullOrEmpty(state.ResolutionId) &&
                string.Equals(_boundWeaponsSyncedForResolution, state.ResolutionId, StringComparison.Ordinal))
            {
                return; // already synced for this resolution (exactly-once guard)
            }
            _boundWeaponsSyncedForResolution = state.ResolutionId ?? string.Empty;

            foreach (var weapon in state.Weapons)
            {
                if (weapon == null || string.IsNullOrEmpty(weapon.InstanceId)) continue;
                float start = state.GetBoundWeaponStartCondition(weapon.InstanceId, -1f);
                if (start < 0f && _boundWeaponConditionAtStart.TryGetValue(weapon.InstanceId, out float cached))
                    start = cached;

                if (start >= 0f)
                    Ashfall.Core.Combat.WeaponEquipmentBridge.SyncAfterCombat(Equipment, weapon, start);
            }
            _boundWeaponConditionAtStart.Clear();
        }

        /// <summary>
        /// Wire the engine's host ports to real inventory + survivor sessions
        /// when present. <paramref name="markCombatSurvived"/> is an explicit
        /// callback rather than a new session-reference property.
        /// <paramref name="onSurvivorDeath"/> routes lethal casualties to the canonical
        /// death authority (SurvivorFateSystem).
        /// </summary>
        public void WireRealState(
            Action<string>? markCombatSurvived = null,
            Action<string, string>? onSurvivorDeath = null)
        {
            var prior = Engine.Ports ?? CombatHostPorts.NoOp();

            Func<string, int, int>? consumeAmmo = null;
            Func<string, int, bool>? consumeItem = null;
            Action<CombatLootEntry>? grantLoot = null;
            if (Inventory != null)
            {
                consumeAmmo = (ammoId, n) =>
                {
                    if (Inventory.Inventory.CountById(ammoId) >= n)
                    {
                        Inventory.Remove(ammoId, n);
                        return 999; // consumed; report ample stock remaining
                    }
                    return -1; // cannot afford -> action refused
                };
                consumeItem = (itemId, n) =>
                {
                    if (Inventory.Inventory.CountById(itemId) >= n)
                    {
                        Inventory.Remove(itemId, n);
                        return true;
                    }
                    return false;
                };
                grantLoot = l => Inventory.Add(l.itemId, l.quantity);
            }

            Func<string, float, float>? damageSurvivor = null;
            Func<string, float, float>? healSurvivor = null;
            Action<string, float>? applyMoraleDelta = null;
            if (Survivors != null)
            {
                damageSurvivor = (id, d) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return d;
                    bool wasAlive = s.IsAlive;
                    s.Health = MathfCompat.Max(0f, s.Health - d);
                    if (s.Health <= 0f)
                    {
                        s.IsAlive = false;
                        s.IsDead = true;
                        if (wasAlive)
                        {
                            onSurvivorDeath?.Invoke(id, "tactical_combat");
                        }
                    }
                    return s.Health;
                };
                healSurvivor = (id, h) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return h;
                    s.Health = MathfCompat.Min(s.MaxHealthCap, s.Health + h);
                    return s.Health;
                };
                applyMoraleDelta = (id, m) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return;
                    s.Morale = MathfCompat.Clamp(s.Morale + m, 0f, 100f);
                };
            }

            Engine.Ports = new CombatHostPorts(
                damageSurvivor,
                healSurvivor,
                applyMoraleDelta,
                consumeAmmo,
                consumeItem,
                prior.RaiseTrauma,
                grantLoot,
                markCombatSurvived ?? prior.MarkCombatSurvived,
                prior.EmitBreachNoise,
                prior.ApplyBreachToolWear);
        }

        /// <summary>
        /// Logs any production-required combat effects still unbound after
        /// <see cref="WireRealState"/>. An empty list means every health, morale,
        /// inventory, and progression effect reaches a real consumer.
        /// </summary>
        public void ValidatePorts()
        {
            var unbound = Engine.Ports.UnboundRequiredEffects;
            if (unbound.Count > 0)
            {
                GD.PrintErr("[Ashfall Godot] Combat host ports unbound: "
                    + string.Join(", ", unbound)
                    + ". Effects will silently no-op/fallback in production.");
            }
        }

        public static CombatHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
        {
            // The weapon/ammo/material catalog is the data authority
            // (combat_catalog.json). Without loading it here, every real
            // PlayerFire() call in production silently returns "Unknown
            // weapon" (CombatCatalog.GetWeapon returns null) and the
            // encounter can only ever resolve by the enemy attacking an
            // unarmed-in-effect player — combat was never actually being
            // fought. Clear+reload is safe to call every time a session is
            // created: the catalog is a static registry shared by the whole
            // process, and the data authority never changes mid-session.
            if (!string.IsNullOrEmpty(dataDir))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                try
                {
                    CombatCatalogLoader.Load(dataDir, files, json);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Combat] combat_catalog.json load failed, falling back to defaults: {ex.Message}");
                }
            }
            if (CombatCatalog.GetWeapon("weapon_assault_rifle") == null)
                CombatCatalog.SeedDefaults();

            var session = new CombatHostSession(campaignRng: campaignRng);
            if (!string.IsNullOrEmpty(dataDir))
            {
                try
                {
                    var files = new FileSystemIO();
                    var json = new SystemTextJsonSerializer();
                    var breachCatalog = BreachingCatalogLoader.Load(dataDir, files, json);
                    BreachingCatalogLoader.Validate(breachCatalog);
                    session.Engine.LoadBreachingCatalog(breachCatalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Combat] breaching_equipment_catalog.json load failed: {ex.Message}");
                }
            }

            var save = CombatSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Combat state restored from save.";
            }
            return session;
        }

        /// <summary>
        /// Plan B86 — bind expedition inventory + vehicle availability so
        /// breaching tools cannot clear for free when logistics are unbound.
        /// </summary>
        public void ConfigureBreachingLogistics(bool vehicleAvailable = false)
        {
            Engine.ConfigureBreachingLogistics(Inventory?.Inventory, vehicleAvailable);
        }

        // ── Production Combat Entry Point ────────────────────────────────

        /// <summary>Default enemy count for ambush encounters without an explicit count (Plan 45 handoff shares it).</summary>
        public const int DefaultAmbushEnemyCount = 3;

        /// <summary>
        /// Start a tactical combat encounter at a location, sourcing survivors and
        /// weapons from live state when not explicitly provided.
        /// When <paramref name="enemyCombatantIds"/> is supplied, enemies spawn
        /// from the combat catalog (Plan 45): catalog base_health is honored
        /// unless the caller forces <paramref name="enemyHealth"/>, and unknown
        /// ids fall back to the legacy enemy block inside BeginEncounter.
        /// </summary>
        public string StartCombat(
            string locationId,
            string locationName,
            IReadOnlyList<CombatantState>? roster = null,
            IReadOnlyList<WeaponInstanceState>? weapons = null,
            int enemyCount = 0,
            int enemyHealth = 0,
            int? seed = null,
            IReadOnlyList<string>? enemyCombatantIds = null,
            bool isSelfDefense = false)
        {
            if (!Engine.State.Resolved && !string.IsNullOrEmpty(Engine.State.EncounterId)
                && Engine.State.Phase != (int)CombatPhase.Setup)
                return "Combat already active — finish or retreat first.";

            var players = new List<CombatantState>();
            if (roster != null && roster.Count > 0)
            {
                players.AddRange(roster);
            }
            else if (Survivors != null && Survivors.RosterState.Count > 0)
            {
                foreach (var s in Survivors.RosterState)
                {
                    if (s == null || !s.IsAlive) continue;
                    players.Add(new CombatantState
                    {
                        Id = "p_" + s.Id.Replace("survivor_", ""),
                        Name = s.Id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant(),
                        SurvivorId = s.Id,
                        IsPlayer = true,
                        Health = (int)Math.Max(1f, s.Health),
                        MaxHealth = (int)Math.Max(1f, s.MaxHealthCap),
                        ArmorRating = 0.4f,
                        CoverRating = 0.3f
                    });
                    if (players.Count >= 4) break;
                }
            }

            if (players.Count == 0)
            {
                players.Add(new CombatantState { Id = "p_yuki", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f });
                players.Add(new CombatantState { Id = "p_mikhail", Name = "Gunner Mikhail", SurvivorId = "survivor_gunner_mikhail", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f });
            }

            var weaponList = new List<WeaponInstanceState>();
            if (weapons != null && weapons.Count > 0)
            {
                weaponList.AddRange(weapons);
            }
            else
            {
                // The primary tracked weapon must go to the survivor the
                // ENGINE will actually shoot with. TacticalCombatSystem resolves
                // its shooter over LivingPlayers(), which SORTS by ordinal
                // combatant Id — so it is NOT players[0] (roster order). Assigning
                // the assault rifle to players[0] left the bound equipment
                // token on a survivor who never fires, which made
                // WeaponEquipmentBridge's post-combat write-back unreachable for
                // the tracked instance (a real weapon could be carried for a
                // whole encounter and never lose condition).
                int primaryIndex = 0;
                string? ordinalFirstId = null;
                for (int i = 0; i < players.Count; i++)
                {
                    if (ordinalFirstId == null || string.CompareOrdinal(players[i].Id, ordinalFirstId) < 0)
                    {
                        ordinalFirstId = players[i].Id;
                        primaryIndex = i;
                    }
                }

                for (int i = 0; i < players.Count; i++)
                {
                    var p = players[i];
                    string wId = i == primaryIndex ? "weapon_assault_rifle" : "weapon_pipe_rifle";
                    string aId = i == primaryIndex ? "ammo_556" : "ammo_357";
                    // Project the persisted equipment authority when it tracks
                    // this weapon; otherwise fall back to the demo literal.
                    var token = Ashfall.Core.Combat.WeaponEquipmentBridge.ToCombatInstance(
                        Equipment, wId, p.SurvivorId);
                    bool bound = !string.IsNullOrEmpty(token.InstanceId);
                    weaponList.Add(new WeaponInstanceState
                    {
                        InstanceId = bound ? token.InstanceId : "w_" + p.Id,
                        WeaponId = wId,
                        OwnerSurvivorId = p.SurvivorId,
                        ConditionPct = bound ? token.ConditionPct : 0.9f,
                        AmmoId = aId,
                        AmmoRemaining = 50
                    });
                    if (bound)
                        _boundWeaponConditionAtStart[token.InstanceId] = token.ConditionPct;
                }
            }

            if (Ballistics != null)
            {
                for (int i = 0; i < weaponList.Count; i++)
                    Ballistics.ApplyToCombatWeapon(weaponList[i]);
            }

            int finalEnemyCount = enemyCount > 0 ? enemyCount : DefaultAmbushEnemyCount;
            bool useCatalogEnemies = enemyCombatantIds != null && enemyCombatantIds.Count > 0;
            // Catalog ids carry their own base_health — only an explicit
            // caller-provided health may override it (Core honors 0 as "no
            // override"). The legacy template keeps its 45 HP default.
            int finalEnemyHealth = enemyHealth > 0
                ? enemyHealth
                : (useCatalogEnemies ? 0 : 45);

            bool ok = Engine.BeginEncounter(
                "enc_" + locationId + "_" + ScheduleDay(),
                "exp_" + locationId,
                locationId,
                locationName ?? locationId,
                ScheduleDay(),
                seed ?? CombatEncounterSeed(),
                players,
                weaponList,
                enemyCount: finalEnemyCount,
                enemyHealth: finalEnemyHealth,
                enemyCombatantIds: useCatalogEnemies ? enemyCombatantIds : null);

            if (ok && weaponList.Count > 0)
            {
                for (int i = 0; i < weaponList.Count; i++)
                {
                    var w = weaponList[i];
                    if (!string.IsNullOrEmpty(w.InstanceId))
                        Engine.SetBoundWeaponStartCondition(w.InstanceId, w.ConditionPct);
                }
            }

            if (ok) Engine.State.IsSelfDefense = isSelfDefense;

            // DEC-358: arm fixed-tick realtime clock after a successful engage.
            if (ok)
                TryEnableRealtime(CombatArenaCatalog.DefaultArenaId);

            return ok ? "Combat engaged at " + (locationName ?? locationId) + "." : "Could not start combat.";
        }

        public string StartDemoCombat(string locationId, string locationName)
            => StartCombat(locationId, locationName);

        public int ScheduleDay()
        {
            // Deterministic day from the engine state (host may override with the real clock).
            return Engine.State.Day > 0 ? Engine.State.Day : 1;
        }

        // ── Player actions (return the human message + refresh UI) ─────

        public string ActionStance(string stanceId)
        {
            if (TacticalCombatSystem.TryParseStance(stanceId, out var s))
            {
                var r = Engine.SetStance(s);
                return r.Message;
            }
            return "Unknown stance: " + stanceId;
        }

        public string ActionFire(string targetId)
        {
            var r = Engine.PlayerFire(targetId, new SeededRng(RollSeed()));
            if (r.Success && Stealth != null)
            {
                string expeditionId = string.IsNullOrEmpty(StealthExpeditionId)
                    ? "combat_active"
                    : StealthExpeditionId;
                string weaponId = Engine.State?.Weapons != null && Engine.State.Weapons.Count > 0
                    ? Engine.State.Weapons[0].WeaponId
                    : "weapon_assault_rifle";
                Stealth.ApplyWeaponNoise(expeditionId, weaponId, StealthSystem.WeaponNoiseKind.Fired);
            }
            return r.Message;
        }

        public string ActionSuppress()
        {
            var r = Engine.PlayerSuppress(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionClearJam(string subjectId)
        {
            var r = Engine.PlayerClearJam(subjectId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionReload(string subjectId)
        {
            var r = Engine.PlayerReload(subjectId);
            return r.Message;
        }

        public CommandResult ActionRepair(string subjectId)
        {
            var r = Engine.ExecutePlayerFieldRepair(subjectId, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (r.IsSuccess)
            {
                LastEvent = r.FailureCode == string.Empty ? "Field repair completed." : $"Field repair: {r.FailureCode}";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Field repair refused: {r.FailureCode}.";
            }
            return r;
        }

        public string ActionMoveLane(string subjectId, CombatLane lane)
        {
            var r = Engine.PlayerMoveLane(subjectId, lane, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionDeployTrap()
        {
            var r = Engine.PlayerDeployTrap(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionDecontaminate()
        {
            var r = Engine.PlayerDecontaminate(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionBandage(string rescuerId, string downedId)
        {
            var r = Engine.PlayerBandage(rescuerId, downedId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionRetreat()
        {
            if (Engine.State != null && Engine.State.RealtimeActive)
            {
                var flee = Engine.RequestFlee();
                return flee.Message;
            }

            var r = Engine.PlayerRetreat(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionLastStand(string subjectId)
        {
            var r = Engine.PlayerLastStand(subjectId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionEndTurn()
        {
            if (Engine.State != null && Engine.State.RealtimeActive)
                return "Realtime combat is active — use PumpRealtime instead of End Turn.";

            var r = Engine.EndTurn(new SeededRng(RollSeed()));
            EvaluateToxicExposureAfterTurn();
            return r.Message;
        }

        // ── DEC-358 realtime pump ─────────────────────────────────────────

        private CombatInputFrame _realtimeInput = CombatInputFrame.Empty;
        private float _realtimeAccum;
        private bool _realtimePumpEnabled;

        public void SetRealtimePumpEnabled(bool enabled) => _realtimePumpEnabled = enabled;

        public void SetInputFrame(CombatInputFrame frame) =>
            _realtimeInput = frame ?? CombatInputFrame.Empty;

        public bool TryEnableRealtime(string? arenaId = null)
        {
            bool ok = Engine.EnableRealtime(arenaId, new SeededRng(CombatEncounterSeed()));
            if (ok) _realtimePumpEnabled = true;
            return ok;
        }

        /// <summary>
        /// Accumulator pump: converts wall/frame dt into fixed Core ticks.
        /// </summary>
        public int PumpRealtime(float wallDt)
        {
            if (!_realtimePumpEnabled || Engine.State == null || !Engine.State.RealtimeActive)
                return 0;

            if (wallDt < 0f) wallDt = 0f;
            _realtimeAccum += wallDt;
            int pumps = 0;
            const int maxPumps = 5;
            while (_realtimeAccum >= TacticalCombatSystem.RealtimeSimDt && pumps < maxPumps)
            {
                Engine.TickRealtime(TacticalCombatSystem.RealtimeSimDt, _realtimeInput, new SeededRng(RollSeed()));
                _realtimeAccum -= TacticalCombatSystem.RealtimeSimDt;
                pumps++;
            }

            if (pumps > 0) RaiseStateChanged();
            return pumps;
        }

        /// <summary>
        /// After a combat turn resolves, evaluate CBRN lane exposure for living
        /// combatants. Raises <see cref="ChemWarfareSystem.OnToxicExposureResolved"/>
        /// (host-subscribed for HP/journal) and applies gas-mask filter wear.
        /// </summary>
        private void EvaluateToxicExposureAfterTurn()
        {
            if (ChemWarfare == null) return;
            var hazards = ChemWarfare.State?.ActiveHazards;
            if (hazards == null || hazards.Count == 0) return;
            var combatants = Engine?.State?.Combatants;
            if (combatants == null || combatants.Count == 0) return;

            EquippedItem? mask = null;
            float mask01 = 0f;
            var inv = Inventory?.Inventory;
            if (inv != null)
            {
                mask = inv.GetEquipped(EquipSlot.Face);
                if (mask?.Item != null
                    && (string.Equals(mask.Item.id, "gas_mask", StringComparison.Ordinal)
                        || string.Equals(mask.Item.id, "item_gas_mask", StringComparison.Ordinal)))
                {
                    float max = mask.Item.durability > 0f ? mask.Item.durability : 100f;
                    float current = mask.CurrentDurability >= 0f ? mask.CurrentDurability : max;
                    mask01 = Math.Clamp(current / max, 0f, 1f);
                }
                else
                {
                    mask = null;
                }
            }

            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                if (c == null || c.IsDowned || c.HasFled) continue;
                string actorId = !string.IsNullOrEmpty(c.SurvivorId) ? c.SurvivorId : c.Id;
                float actorMask = c.IsPlayer ? mask01 : 0f;
                ChemWarfare.EvaluateActorExposure(actorId, c.Lane, actorMask, out float wear);
                if (wear > 0f && c.IsPlayer && mask != null && inv != null)
                {
                    float max = mask.Item != null && mask.Item.durability > 0f
                        ? mask.Item.durability
                        : 100f;
                    inv.RecordWear(mask, wear * max, "chem_filter");
                }
            }
        }

        public string ActionEnvironmental(float severity)
        {
            var r = Engine.TickEnvironmental(severity, new SeededRng(RollSeed()));
            return r.Message;
        }

        // ── Action Preflight / Legality Queries ──────────────────────────

        public ActionPreflight EvaluateFire(string targetId) => Engine.EvaluateFire(targetId);
        public ActionPreflight EvaluateSuppress() => Engine.EvaluateSuppress();
        public ActionPreflight EvaluateClearJam(string subjectId) => Engine.EvaluateClearJam(subjectId);
        public ActionPreflight EvaluateReload(string subjectId) => Engine.EvaluateReload(subjectId);
        public ActionPreflight EvaluateRepair(string subjectId) => Engine.EvaluateRepair(subjectId);
        public ActionPreflight EvaluateRetreat() => Engine.EvaluateRetreat();
        public ActionPreflight EvaluateEndTurn() => Engine.EvaluateEndTurn();

        /// <summary>Deterministic roll seed for this action (host owns seeding).</summary>
        private int RollSeed()
        {
            unchecked
            {
                int day = Engine.State.Day;
                int turn = Engine.State.Turn;
                return (CombatBaseSeed() * 31) + (day * 7) + (turn * 13);
            }
        }

        private int CombatBaseSeed()
        {
            return _campaignRng != null
                ? _campaignRng.GetStream(CampaignStreamIds.Combat).DerivedBaseSeed
                : DemoSeed;
        }

        private int CombatEncounterSeed()
        {
            return _campaignRng != null
                ? _campaignRng.GetStream(CampaignStreamIds.Combat).ForkSeed(ScheduleDay())
                : DemoSeed;
        }

        /// <summary>First living hostile id for HUD Fire when no target picker is wired.</summary>
        public string DefaultHostileTargetId()
        {
            var combatants = Engine?.State?.Combatants;
            if (combatants == null) return string.Empty;
            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                if (c == null || c.IsPlayer || c.IsDowned || c.HasFled) continue;
                return c.Id ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>First living player combatant id for HUD Clear Jam.</summary>
        public string DefaultPlayerSubjectId()
        {
            var combatants = Engine?.State?.Combatants;
            if (combatants == null) return string.Empty;
            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                if (c == null || !c.IsPlayer || c.IsDowned || c.HasFled) continue;
                return c.Id ?? string.Empty;
            }
            return string.Empty;
        }

        // ── Snapshot / status ───────────────────────────────────────────

        public CombatSnapshot Snapshot() => Engine.BuildSnapshot();

        public string StatusLine()
        {
            if (string.IsNullOrEmpty(Engine.State.EncounterId) ||
                (Engine.State.Resolved && Engine.State.Phase == (int)CombatPhase.Setup))
                return "Combat: none active.";

            var snap = Engine.BuildSnapshot();
            var sb = new System.Text.StringBuilder();
            sb.Append("Combat [").Append(snap.LocationName).Append("] phase ").Append(snap.Phase)
              .Append(" · turn ").Append(snap.Turn).Append(" · stance ").Append(snap.StanceId);
            foreach (var c in snap.Combatants)
            {
                sb.Append('\n').Append(c.IsPlayer ? "  ▶ " : "  ● ")
                  .Append(c.Name).Append(" (").Append(c.Lane).Append(") HP ")
                  .Append(c.Health).Append('/').Append(c.MaxHealth)
                  .Append(c.IsDowned ? " [DOWNED]" : "")
                  .Append(c.IsPinned ? " [PINNED]" : "")
                  .Append(" · ").Append(c.WeaponName).Append(" ").Append(c.WeaponConditionPct).Append("%")
                  .Append(c.WeaponJammed ? " [JAM]" : "");
            }
            if (snap.Resolved) sb.Append('\n').Append("OUTCOME: ").Append(snap.OutcomeText);
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public CombatState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(CombatState state) => Engine.RestoreState(state);

        public bool TryPersist() => CombatSaveStore.TrySave(Engine.CaptureState());
        public CombatState? TryRestorePersisted() => CombatSaveStore.TryLoad();

        // ── Wiring Reporter (Plan 36B) ──────────────────────────────────

        public WiringReport GetWiringReport()
        {
            var reqCollabs = new List<string> { "Inventory", "Survivors" };
            var boundCollabs = new List<string>();
            var missingCollabs = new List<string>();

            if (Inventory != null) boundCollabs.Add("Inventory");
            else missingCollabs.Add("Inventory");

            if (Survivors != null) boundCollabs.Add("Survivors");
            else missingCollabs.Add("Survivors");

            var allPorts = new[] { "DamageSurvivor", "HealSurvivor", "ApplyMoraleDelta", "ConsumeAmmo", "ConsumeItem", "GrantLoot", "MarkCombatSurvived" };
            var unbound = Engine?.Ports?.UnboundRequiredEffects ?? Array.Empty<string>();
            var boundPorts = allPorts.Where(p => !unbound.Contains(p)).ToList();

            var fallbacks = new List<NamedFallback>();
            if (unbound.Count > 0)
            {
                fallbacks.Add(new NamedFallback("CombatHostPorts.NoOp", "combat", AllowedEnvironment.TestOnly, "No-op essential adapters used when full host ports are not injected"));
            }

            return new WiringReport
            {
                SessionId = "CombatHostSession",
                RequiredCollaborators = reqCollabs,
                BoundCollaborators = boundCollabs,
                MissingCollaborators = missingCollabs,
                RequiredPorts = allPorts,
                BoundPorts = boundPorts,
                MissingPorts = unbound.ToList(),
                ActiveFallbacks = fallbacks
            };
        }
    }
}
