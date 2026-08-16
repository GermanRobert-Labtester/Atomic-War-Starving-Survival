using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side aggregate view of one survivor's Phase-0 effect state.
    /// </summary>
    public class Phase0SurvivorEffects
    {
        public string survivorId = string.Empty;
        /// <summary>Combined work-efficiency factor from phantom motivation and flashback penalty.</summary>
        public float workEfficiencyMultiplier = 1f;
        /// <summary>Hours the survivor refuses to work (phantom breakdown).</summary>
        public float workRefusalHours = 0f;
        /// <summary>Stamina multiplier (respiratory degeneration).</summary>
        public float staminaMultiplier = 1f;
    }

    /// <summary>Serialized Phase-0 effects envelope.</summary>
    public class Phase0EffectsSaveState
    {
        public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
        public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
        public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
        public FinalWishSaveState finalWishes = new FinalWishSaveState();
        public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
        public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
        public float permanentShelterMoraleBuff = 0f;
    }

    /// <summary>
    /// Thin Godot-host session for the Phase-0 psychological/medical effects:
    /// phantom memory (work efficiency + refusal), somatic flashbacks (work
    /// penalty), trade specialty (skill/morale/narrative), final wishes
    /// (permanent shelter morale buff), and respiratory degeneration (stamina
    /// penalty + ash-zone exposure). Chemical-dependency crafting/combat
    /// penalties are owned by MedicalHostSession, not duplicated here. All
    /// rules live in Ashfall.Core; this session only wires the effect events
    /// into per-survivor host state and persists it.
    /// </summary>
    public sealed class Phase0HostSession
    {
        public const int DefaultSeed = 808;

        public PhantomMemoryEngine Phantom { get; }
        public SomaticFlashbackSystem Flashbacks { get; }
        public TradeSpecialtySystem TradeSpecialty { get; }
        public FinalWishSystem FinalWish { get; }
        public RespiratoryDegenerationSystem Respiratory { get; }

        /// <summary>Accumulated permanent shelter-wide morale buff from completed final wishes.</summary>
        public float PermanentShelterMoraleBuff { get; private set; }

        /// <summary>Set by the host from the current expedition/zone (real ash-zone signal).</summary>
        public bool IsInAshZone { get; set; }

        /// <summary>Air-filtration health 0..100, injected by the shelter host.</summary>
        public Func<float> GetFilterHealth;

        public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;

        /// <summary>Public accessor for the derived host view of one survivor.</summary>
        public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);

        public string LastEvent { get; private set; } = string.Empty;
        public event Action StateChanged;

        private readonly List<Phase0SurvivorEffects> _effects = new List<Phase0SurvivorEffects>();
        private readonly List<string> _aliveSurvivorIds = new List<string>();
        private readonly ISeededRng _rng;

        public Phase0HostSession(int seed = DefaultSeed)
        {
            _rng = new CoreSeededRng(seed);
            Phantom = new PhantomMemoryEngine();
            Flashbacks = new SomaticFlashbackSystem
            {
                Rng = new CoreSeededRng(seed + 1),
                GetAliveSurvivorIds = () => _aliveSurvivorIds,
                IsCompanionInSameRoom = (a, b) => false
            };
            TradeSpecialty = new TradeSpecialtySystem
            {
                GrantSkillBonus = (sv, prof, bonus) =>
                {
                    LastEvent = $"Specialty: {sv} ({prof}) skill +{bonus:F2}.";
                    StateChanged?.Invoke();
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    LastEvent = $"Specialty: {sv} morale {delta:+#.##;-#.##;0}.";
                    StateChanged?.Invoke();
                },
                GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}",
                FireNarrativeEvent = (narrativeId, sv) =>
                {
                    LastEvent = $"Narrative event fired: {narrativeId} for {sv}.";
                    StateChanged?.Invoke();
                }
            };
            FinalWish = new FinalWishSystem
            {
                Rng = new CoreSeededRng(seed + 2),
                ApplyPermanentShelterMoraleBuff = delta =>
                {
                    PermanentShelterMoraleBuff += delta;
                    LastEvent = $"Permanent shelter morale {(delta >= 0 ? "+" : "")}{delta:F0} (total {PermanentShelterMoraleBuff:F0}).";
                    StateChanged?.Invoke();
                }
            };
            Respiratory = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => GetFilterHealth?.Invoke() ?? 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => IsInAshZone
            };

            // ── Effect wiring ─────────────────────────────────────────
            // The host view is DERIVED from the core systems on every relevant
            // change (RecomputeSurvivorEffects). Event handlers must not write
            // the aggregate fields directly — competing handlers would clobber
            // each other (e.g. a phantom boost erasing a flashback penalty, or
            // a flashback ending resetting an active boost).
            Flashbacks.OnFlashbackTriggered += (sv, duration) =>
            {
                RecomputeSurvivorEffects(sv);
                LastEvent = $"Flashback: {sv} — {duration:F1}h, work ×{fx(sv).workEfficiencyMultiplier:F2}.";
                StateChanged?.Invoke();
            };
            Flashbacks.OnFlashbackEnded += sv =>
            {
                RecomputeSurvivorEffects(sv);
                LastEvent = $"Flashback ended: {sv}.";
                StateChanged?.Invoke();
            };

            Phantom.OnPhantomTriggered += (sv, item, isMotivation) =>
            {
                RecomputeSurvivorEffects(sv);
                LastEvent = isMotivation
                    ? $"Phantom motivation: {sv} work ×{fx(sv).workEfficiencyMultiplier:F2}."
                    : $"Phantom breakdown: {sv} refuses work for {fx(sv).workRefusalHours:F0}h.";
                StateChanged?.Invoke();
            };

            Respiratory.OnStaminaPenaltyRequested += (sv, factor) =>
            {
                RecomputeSurvivorEffects(sv);
                StateChanged?.Invoke();
            };

            TradeSpecialty.OnSpecialtyMastered += (sv, prof) =>
            {
                LastEvent = $"Trade mastered: {sv} ({prof}).";
                StateChanged?.Invoke();
            };

            Phantom.OnStateChanged += _ => RecomputeAllEffects();
            Flashbacks.OnStateChanged += () => RecomputeAllEffects();
            TradeSpecialty.OnStateChanged += () => StateChanged?.Invoke();
            FinalWish.OnStateChanged += () => StateChanged?.Invoke();
            Respiratory.OnStateChanged += () => RecomputeAllEffects();
        }

        // ── Roster wiring ─────────────────────────────────────────────

        /// <summary>Load the phantom_triggers.json catalog into the engine (the authority).</summary>
        public void LoadPhantomRules(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string path = System.IO.Path.Combine(dataDir, "phantom_triggers.json");
                if (!files.FileExists(path)) return;

                var entries = json.Deserialize<List<PhantomTriggerJsonEntry>>(files.ReadAllText(path));
                if (entries == null) return;

                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    if (entry == null || string.IsNullOrEmpty(entry.background_id)) continue;
                    if (entry.triggers == null) continue;
                    for (int j = 0; j < entry.triggers.Count; j++)
                    {
                        var t = entry.triggers[j];
                        if (t == null) continue;
                        Phantom.RegisterRule(
                            entry.background_id,
                            t.item_category,
                            t.motivation_chance,
                            t.description,
                            t.motivation_text,
                            t.breakdown_text);
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Phase0] Failed to load phantom rules: {ex.Message}");
            }
        }

        /// <summary>Built-in fallback phantom rules (host demo convenience).</summary>
        public void RegisterDefaultRules()
        {
            Phantom.RegisterRule("former_soldier", "military", 0.40f, "desc",
                "{name} pockets the tags. 'I'll remember them,' they say. Their posture straightens.",
                "{name} reads the name on the tag and goes pale. They knew this person.");
            Phantom.RegisterRule("nurse", "medical", 0.50f, "desc",
                "{name} taps the bell of the stethoscope. 'Still works,' they say.",
                "{name} listens to their own heartbeat through the stethoscope.");
            Phantom.RegisterRule("teacher", "correspondence", 0.50f, "desc",
                "{name} finds a blank page and writes a new lesson at the top.",
                "{name} reads a name written in clumsy letters on the cover.");
            Phantom.RegisterRule("generic", "photograph", 0.50f, "desc",
                "{name} props the photograph against the wall. 'They'd want us to keep going.'",
                "{name} can't stop looking at the photograph. 'These people had lives.'");
        }

        /// <summary>Register the alive survivor ids (the host's roster authority).</summary>
        public void RegisterSurvivors(IEnumerable<string> ids)
        {
            _aliveSurvivorIds.Clear();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    _aliveSurvivorIds.Add(id);
                    GetOrCreateEffects(id);
                }
            }
            StateChanged?.Invoke();
        }

        /// <summary>Seed a small demo roster (host demo convenience).</summary>
        public void SeedDemoRoster()
        {
            RegisterSurvivors(new[] { "survivor_dr_sarah_chen", "survivor_gunner_mikhail", "elena_vasquez" });
        }

        // ── Demo actions ──────────────────────────────────────────────

        public string ScavengeItem(string survivorId, string itemId)
        {
            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = survivorId,
                displayName = survivorId,
                backgroundId = InferBackground(survivorId),
                isAlive = true
            };
            var outcome = Phantom.OnItemScavenged(sv, itemId, _rng);
            LastEvent = outcome != TriggerOutcome.None
                ? $"Phantom: {survivorId} {(outcome == TriggerOutcome.Motivation ? "motivated" : "broke down")} on {itemId}."
                : $"Phantom: no memory triggered for {survivorId} ({itemId}).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string RaiseNoise(string survivorId)
        {
            Flashbacks.OnAudioEvent("siren", 1f);
            LastEvent = $"Noise event raised (flashbacks checked).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string CraftItem(string survivorId, string professionId, string itemId)
        {
            TradeSpecialty.OnItemCrafted(survivorId, professionId, itemId);
            int tier = TradeSpecialty.GetMasteryTier(survivorId);
            LastEvent = $"{survivorId} crafted {itemId}: specialty tier {tier}/3.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string TickHour(float gameHours = 1f)
        {
            foreach (var id in _aliveSurvivorIds)
            {
                Phantom.TickHour(id, gameHours);
                Flashbacks.Tick(id, gameHours);
                Respiratory.TickHours(id, gameHours);
            }
            RecomputeAllEffects();
            LastEvent = $"Phase-0 effects ticked {gameHours:F0}h.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("PHASE-0 — PSYCHOLOGICAL & MEDICAL EFFECTS\n");
            sb.Append("Permanent shelter morale buff: ").Append(PermanentShelterMoraleBuff.ToString("F0")).Append('\n');
            for (int i = 0; i < _effects.Count; i++)
            {
                var fx = _effects[i];
                if (fx == null) continue;
                sb.Append(fx.survivorId)
                  .Append(": work ×").Append(fx.workEfficiencyMultiplier.ToString("F2"))
                  .Append(" · refusal ").Append(fx.workRefusalHours.ToString("F1")).Append("h")
                  .Append(" · stamina ×").Append(fx.staminaMultiplier.ToString("F2"))
                  .Append(IsInAshZone ? " · ASH ZONE" : "")
                  .Append('\n');
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public Phase0EffectsSaveState CaptureSave()
        {
            var save = new Phase0EffectsSaveState
            {
                phantom = Phantom.CaptureState(),
                flashbacks = Flashbacks.CaptureState(),
                tradeSpecialty = TradeSpecialty.CaptureState(),
                finalWishes = FinalWish.CaptureState(),
                respiratory = Respiratory.CaptureState(),
                permanentShelterMoraleBuff = PermanentShelterMoraleBuff
            };
            for (int i = 0; i < _effects.Count; i++)
            {
                var e = _effects[i];
                if (e == null) continue;
                save.effects.Add(new Phase0SurvivorEffects
                {
                    survivorId = e.survivorId,
                    workEfficiencyMultiplier = e.workEfficiencyMultiplier,
                    workRefusalHours = e.workRefusalHours,
                    staminaMultiplier = e.staminaMultiplier
                });
            }
            return save;
        }

        public void RestoreSave(Phase0EffectsSaveState save)
        {
            if (save == null) return;
            Phantom.RestoreState(save.phantom);
            Flashbacks.RestoreState(save.flashbacks);
            TradeSpecialty.RestoreState(save.tradeSpecialty);
            FinalWish.RestoreState(save.finalWishes);
            Respiratory.RestoreState(save.respiratory);
            PermanentShelterMoraleBuff = save.permanentShelterMoraleBuff;
            _effects.Clear();
            if (save.effects != null)
            {
                for (int i = 0; i < save.effects.Count; i++)
                {
                    var e = save.effects[i];
                    if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
                    _effects.Add(new Phase0SurvivorEffects
                    {
                        survivorId = e.survivorId,
                        workEfficiencyMultiplier = e.workEfficiencyMultiplier,
                        workRefusalHours = e.workRefusalHours,
                        staminaMultiplier = e.staminaMultiplier
                    });
                    if (!_aliveSurvivorIds.Contains(e.survivorId))
                        _aliveSurvivorIds.Add(e.survivorId);
                }
            }
            StateChanged?.Invoke();
        }

        // ── Helpers ────────────────────────────────────────────────────

        private Phase0SurvivorEffects fx(string survivorId) => GetOrCreateEffects(survivorId);

        /// <summary>
        /// Derive the host view of one survivor from the core systems. The
        /// aggregate fields are a pure function of core state — never written
        /// directly by event handlers (see constructor wiring notes).
        /// </summary>
        private void RecomputeSurvivorEffects(string survivorId)
        {
            var fx = GetOrCreateEffects(survivorId);
            // Composition: a phantom motivation boost multiplies the flashback
            // penalty factor (e.g. 1.20 × 0.40 = 0.48 effective), rather than
            // one effect erasing the other.
            fx.workEfficiencyMultiplier =
                Phantom.GetWorkEfficiencyMultiplier(survivorId)
                * (1f - Flashbacks.GetWorkEfficiencyPenalty(survivorId));
            fx.workRefusalHours = Phantom.GetWorkRefusalHours(survivorId);
            fx.staminaMultiplier = Respiratory.GetStaminaMultiplier(survivorId);
        }

        private void RecomputeAllEffects()
        {
            for (int i = 0; i < _aliveSurvivorIds.Count; i++)
                RecomputeSurvivorEffects(_aliveSurvivorIds[i]);
        }

        private Phase0SurvivorEffects GetOrCreateEffects(string survivorId)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                var e = _effects[i];
                if (e != null && e.survivorId == survivorId) return e;
            }
            var fx = new Phase0SurvivorEffects { survivorId = survivorId };
            _effects.Add(fx);
            return fx;
        }

        private static string InferBackground(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return "generic";
            if (survivorId.Contains("gunner") || survivorId.Contains("soldier")) return "former_soldier";
            if (survivorId.Contains("sarah") || survivorId.Contains("nurse")) return "nurse";
            if (survivorId.Contains("teacher")) return "teacher";
            return "generic";
        }

        /// <summary>Deterministic ISeededRng adapter delegating to the core SeededRng.</summary>
        private sealed class CoreSeededRng : ISeededRng
        {
            private readonly SeededRng _rng;
            public int Seed { get; }
            public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => _rng.NextFloat();
            public double NextDouble() => _rng.NextDouble();
        }

        // ── JSON DTOs (phantom_triggers.json) ─────────────────────────

        private sealed class PhantomTriggerJsonEntry
        {
            public string background_id;
            public List<PhantomTriggerRuleJson> triggers;
        }

        private sealed class PhantomTriggerRuleJson
        {
            public string item_category;
            public float motivation_chance;
            public string description;
            public string motivation_text;
            public string breakdown_text;
        }
    }
}
