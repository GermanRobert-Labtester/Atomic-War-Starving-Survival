using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phase 0 — Foundation wiring for the 40-system Massive Expansion.
    /// Constructs and registers 8 new systems, wires them into the tick
    /// loop and save/load via SystemRegistry + ISaveable.
    ///
    /// All systems are plain C# leaf assemblies; host hooks inject the
    /// cross-assembly dependencies (Inventory, Shelter, AI, etc.).
    /// </summary>
    public partial class GameBootstrap
    {
        // ── Phase 0 system accessors ──────────────────────────────────

        public RadiationPhaseProgression RadiationPhaseProgression { get; private set; }
        public PhantomMemorySystem PhantomMemorySystem { get; private set; }
        public GuiltInsomniaSystem GuiltInsomniaSystem { get; private set; }
        public CombatTraumaSystem CombatTraumaSystem { get; private set; }
        public SomaticFlashbackSystem SomaticFlashbackSystem { get; private set; }
        public MoralBranchingSystem MoralBranchingSystem { get; private set; }
        public ChemicalDependencySystem ChemicalDependencySystem { get; private set; }
        public TradeSpecialtySystem TradeSpecialtySystem { get; private set; }
        public FinalWishSystem FinalWishSystem { get; private set; }
        public RespiratoryDegenerationSystem RespiratoryDegenerationSystem { get; private set; }

        /// <summary>
        /// Call at the end of InitFoundation(), after all pre-existing
        /// systems are constructed and wired.
        /// </summary>
        private void InitPhase0Expansion()
        {
            // ── 1. Radiation Phase Progression ─────────────────────────
            RadiationPhaseProgression = new RadiationPhaseProgression
            {
                ApplyHealthDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Health, delta);
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                GrantChronicIllness = (sv, id) =>
                {
                    if (sv != null && MedicalSystem != null)
                        MedicalSystem.Inflict(sv, id);
                },
                MarkChronicFibrosis = sv =>
                {
                    if (sv != null && !sv.HasDisability("scarred_lungs"))
                    {
                        sv.DisabilityIds.Add("scarred_lungs");
                        sv.HasPermanentLungDamage = true;
                    }
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 17)
            };
            _registry.RegisterPerSubstep("radiation_phase_progression",
                h => TickPhase0RadiationSystems(h));
            _registry.Register<RadiationPhaseProgression>(RadiationPhaseProgression);

            // ── 2. Phantom Memory System ───────────────────────────────
            PhantomMemorySystem = new PhantomMemorySystem
            {
                GetItemCategory = sv =>
                {
                    if (sv?.PhantomBackgroundId == null) return null;
                    // Look up from item catalog; fall back to prefix extraction
                    // Host-aware: only returns non-null for triggerable items
                    return sv.PhantomBackgroundId;
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                SetWorkEfficiencyMultiplier = (sv, mult) =>
                {
                    // Placeholder — will wire into AI work speed in Phase 11
                },
                SetWorkRefusalHours = (sv, hours) =>
                {
                    // Placeholder — will wire into AI action blocking in Phase 11
                },
                Rng = new System.Random(_worldSeed + 19)
            };
            _registry.RegisterPerSubstep("phantom_memory",
                h => TickPhantomMemorySurvivors(h));
            _registry.Register<PhantomMemorySystem>(PhantomMemorySystem);

            // ── 3. Guilt Insomnia System ───────────────────────────────
            GuiltInsomniaSystem = new GuiltInsomniaSystem();
            GuiltInsomniaSystem.SetNeedsSystem(NeedsSystem);
            GuiltInsomniaSystem.GetDay = () => TimeSystem?.CurrentDay ?? 1;
            GuiltInsomniaSystem.Rng = new System.Random(_worldSeed + 21);
            _registry.RegisterPerSubstep("guilt_insomnia",
                h => TickGuiltInsomniaSurvivors(h));
            _registry.Register<GuiltInsomniaSystem>(GuiltInsomniaSystem);

            // ── 4. Combat Trauma System ────────────────────────────────
            CombatTraumaSystem = new CombatTraumaSystem();
            CombatTraumaSystem.SetNeedsSystem(NeedsSystem);
            CombatTraumaSystem.ApplyMoraleDelta = (sv, delta) =>
            {
                if (sv?.Needs != null && NeedsSystem != null)
                    NeedsSystem.Modify(sv, NeedKind.Morale, delta);
            };
            CombatTraumaSystem.GetDay = () => TimeSystem?.CurrentDay ?? 1;
            CombatTraumaSystem.GetSurvivors = () => Survivors;
            CombatTraumaSystem.Rng = new System.Random(_worldSeed + 23);
            _registry.RegisterPerSubstep("combat_trauma",
                h => TickCombatTraumaSurvivors(h));
            _registry.Register<CombatTraumaSystem>(CombatTraumaSystem);

            // ── 5. Somatic Flashback System ────────────────────────────
            SomaticFlashbackSystem = new SomaticFlashbackSystem
            {
                IsCompanionInSameRoom = (sv, other) =>
                {
                    if (sv == null || other == null) return false;
                    return !string.IsNullOrEmpty(sv.CurrentRoomId) &&
                        string.Equals(sv.CurrentRoomId, other.CurrentRoomId,
                            StringComparison.Ordinal);
                },
                SetWorkEfficiencyPenalty = (sv, penalty) =>
                {
                    // Placeholder — wired in Phase 11
                },
                GetSurvivors = () => Survivors,
                Rng = new System.Random(_worldSeed + 25)
            };
            _registry.RegisterPerSubstep("somatic_flashback",
                h => TickSomaticFlashbackSurvivors(h));
            _registry.Register<SomaticFlashbackSystem>(SomaticFlashbackSystem);

            // ── 6. Moral Branching System ──────────────────────────────
            MoralBranchingSystem = new MoralBranchingSystem
            {
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                ApplyShelterMoraleDelta = delta =>
                {
                    if (Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        var s = Survivors[i];
                        if (s?.Needs != null && NeedsSystem != null)
                            NeedsSystem.Modify(s, NeedKind.Morale, delta);
                    }
                }
            };
            // Event-driven only — no per-substep tick
            _registry.RegisterEventDriven("moral_branching");
            _registry.Register<MoralBranchingSystem>(MoralBranchingSystem);

            // ── 7. Chemical Dependency System ──────────────────────────
            ChemicalDependencySystem = new ChemicalDependencySystem
            {
                ApplyCraftingPenalty = (sv, penalty) =>
                {
                    // Placeholder — wired into CraftingSystem in Phase 11
                },
                ApplyCombatPenalty = (sv, penalty) =>
                {
                    // Placeholder — wired in Phase 11
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 27)
            };
            _registry.RegisterPerSubstep("chemical_dependency",
                h => TickChemicalDependencySurvivors(h));
            _registry.Register<ChemicalDependencySystem>(ChemicalDependencySystem);

            // ── 8. Trade Specialty System ──────────────────────────────
            TradeSpecialtySystem = new TradeSpecialtySystem
            {
                GrantSkillBonus = (sv, professionId, bonus) =>
                {
                    if (sv == null) return;
                    switch (professionId)
                    {
                        case "electrician":
                        case "machinist":
                            sv.CraftingSkill = Math.Min(1f, sv.CraftingSkill + bonus);
                            break;
                        case "nurse":
                            sv.MedicalSkill = Math.Min(1f, sv.MedicalSkill + bonus);
                            break;
                        case "teacher":
                            sv.ScienceSkill = Math.Min(1f, sv.ScienceSkill + bonus);
                            break;
                    }
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                GetNarrativeEventId = professionId => $"narrative_trade_mastery_{professionId}",
                FireNarrativeEvent = (narrativeId, sv) =>
                {
                    // Placeholder — wired into EventRunner in Phase 11
                }
            };
            _registry.RegisterEventDriven("trade_specialty");
            _registry.Register<TradeSpecialtySystem>(TradeSpecialtySystem);

            // ── 9. Final Wish System ───────────────────────────────────
            FinalWishSystem = new FinalWishSystem
            {
                ApplyPermanentShelterMoraleBuff = delta =>
                {
                    // Permanent morale buff — stored on shelter state
                    if (Shelter != null)
                    {
                        // Placeholder — wired in Phase 11
                    }
                    // Apply to all current survivors
                    if (Survivors != null && NeedsSystem != null)
                    {
                        for (int i = 0; i < Survivors.Count; i++)
                        {
                            var s = Survivors[i];
                            if (s?.Needs != null)
                                NeedsSystem.Modify(s, NeedKind.Morale, delta);
                        }
                    }
                },
                GetWishNarrativeText = wishId => wishId,
                Rng = new System.Random(_worldSeed + 29)
            };
            _registry.RegisterPerSubstep("final_wish",
                h => TickFinalWishSurvivors(h));
            _registry.Register<FinalWishSystem>(FinalWishSystem);

            // ── 10. Respiratory Degeneration System ────────────────────
            RespiratoryDegenerationSystem = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () =>
                {
                    var filterModule = Shelter?.GetModule("air_filtration");
                    return filterModule?.FilterHealth ?? 100f;
                },
                IsInFalloutStorm = () =>
                {
                    return WeatherSystem?.Current == WeatherKind.FalloutStorm;
                },
                IsInAshZone = () =>
                {
                    // Placeholder — check expedition zone for ash
                    return false;
                },
                ApplyStaminaPenalty = (sv, penalty) =>
                {
                    // Placeholder — wired into max stamina calc in Phase 11
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 31)
            };
            _registry.RegisterPerSubstep("respiratory_degeneration",
                h => TickRespiratoryDegenerationSurvivors(h));
            _registry.Register<RespiratoryDegenerationSystem>(RespiratoryDegenerationSystem);
        }

        // ── Per-system tick helpers (called from SystemRegistry) ──────

        private void TickPhase0RadiationSystems(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                RadiationPhaseProgression?.Tick(sv, gameHours);
            }
        }

        private void TickPhantomMemorySurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                PhantomMemorySystem?.Tick(sv, gameHours);
            }
        }

        private void TickGuiltInsomniaSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            int day = TimeSystem?.CurrentDay ?? 1;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                GuiltInsomniaSystem?.Tick(sv, gameHours, day);
            }
        }

        private void TickCombatTraumaSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            bool isNight = PhotoperiodSystem != null &&
                PhotoperiodSystem.EffectiveDaylightHours < 2f;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                CombatTraumaSystem?.Tick(sv, gameHours, isNight);
            }
        }

        private void TickSomaticFlashbackSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                SomaticFlashbackSystem?.Tick(sv, gameHours);
            }
        }

        private void TickChemicalDependencySurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                ChemicalDependencySystem?.Tick(sv, gameHours);
            }
        }

        private void TickFinalWishSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                FinalWishSystem?.Tick(sv, gameHours);
            }
        }

        private void TickRespiratoryDegenerationSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                RespiratoryDegenerationSystem?.Tick(sv, gameHours);
            }
        }
    }
}
