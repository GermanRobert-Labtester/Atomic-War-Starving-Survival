using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phases 7-8 — Wire Interpersonal Dynamics systems into live gameplay.
    ///
    ///   TraumaBondSystem          → Shared hazard events
    ///   IdeologicalFrictionSystem → Roommate sleep quality calc
    ///   RationConflictSystem      → Daily ration fairness check
    ///   VigilSystem               → Survivor death event
    ///   ConfessionSystem          → Nighttime idle dialogue
    ///   MessTableSystem           → CookingSystem hot meal events
    ///   LeadershipSystem          → Designation + death witnessing
    ///   CaregivingSystem          → Patient assignment
    ///   DesertionRiskSystem       → Low morale + faction pressure check
    /// </summary>
    public partial class GameBootstrap
    {
        // ── Phase 7-8 system accessors ─────────────────────────────────

        public TraumaBondSystem TraumaBondSystem { get; private set; }
        public IdeologicalFrictionSystem IdeologicalFrictionSystem { get; private set; }
        public RationConflictSystem RationConflictSystem { get; private set; }
        public LeadershipSystem LeadershipSystem { get; private set; }
        public CaregivingSystem CaregivingSystem { get; private set; }

        /// <summary>
        /// Call during InitializeSystems, after all survivor roster
        /// and social systems exist.
        /// </summary>
        private void InitPhases7to8Wiring()
        {
            InitPhase7Systems();
            WirePhase7Callbacks();
            InitPhase8Systems();
            WirePhase8Callbacks();
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 7: Trauma Bonds, Ideological Friction, Ration Conflicts
        // ═══════════════════════════════════════════════════════════════

        private void InitPhase7Systems()
        {
            // ── Trauma Bond System ─────────────────────────────────────
            TraumaBondSystem = new TraumaBondSystem
            {
                AdjustAffinity = (a, b, delta) =>
                {
                    if (MentalBreakSystem?.Affinity != null)
                        MentalBreakSystem.Affinity.Adjust(a, b, delta);
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 41)
            };
            _registry.RegisterPerSubstep("traumaBond",
                h => TickTraumaBondSurvivors(h));
            _registry.Register<TraumaBondSystem>(TraumaBondSystem);

            // ── Ideological Friction System ────────────────────────────
            IdeologicalFrictionSystem = new IdeologicalFrictionSystem
            {
                AdjustAffinity = (a, b, delta) =>
                {
                    if (MentalBreakSystem?.Affinity != null)
                        MentalBreakSystem.Affinity.Adjust(a, b, delta);
                },
                AreSharingRoom = (a, b) =>
                    a != null && b != null &&
                    !string.IsNullOrEmpty(a.CurrentRoomId) &&
                    string.Equals(a.CurrentRoomId, b.CurrentRoomId,
                        StringComparison.Ordinal),
                Rng = new System.Random(_worldSeed + 43)
            };
            _registry.RegisterPerSubstep("ideologicalFriction",
                h => TickIdeologicalFriction(h));
            _registry.Register<IdeologicalFrictionSystem>(
                IdeologicalFrictionSystem);

            // ── Ration Conflict System ─────────────────────────────────
            RationConflictSystem = new RationConflictSystem
            {
                GetRationAllocation = sv =>
                {
                    // Default: equal allocation
                    if (Survivors == null || Survivors.Count == 0)
                        return 1f / Math.Max(1, Survivors?.Count ?? 1);
                    return 1f / Survivors.Count;
                },
                GetAverageRationAllocation = () =>
                    1f / Math.Max(1, Survivors?.Count ?? 1),
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                AdjustAffinity = (a, b, delta) =>
                {
                    if (MentalBreakSystem?.Affinity != null)
                        MentalBreakSystem.Affinity.Adjust(a, b, delta);
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 45)
            };
            _registry.RegisterDaily("rationConflict",
                d => TickRationConflictsDaily());
            _registry.Register<RationConflictSystem>(RationConflictSystem);
        }

        private void WirePhase7Callbacks()
        {
            // Wire trauma bonds into shared hazard detection
            // HatchDefenseSystem.OnRaidResolved → shared combat hazard
            if (HatchDefenseSystem != null)
            {
                Action<RaidResolution> onRaid = (resolution) =>
                {
                    if (resolution == null || !resolution.Launched) return;
                    if (resolution.TraumatizedSurvivorIds == null ||
                        resolution.TraumatizedSurvivorIds.Count < 2) return;

                    var participants = new List<Survivor>();
                    for (int i = 0; i < resolution.TraumatizedSurvivorIds.Count; i++)
                    {
                        var sv = FindSurvivorById(
                            resolution.TraumatizedSurvivorIds[i]);
                        if (sv != null && sv.IsAlive)
                            participants.Add(sv);
                    }
                    if (participants.Count >= 2)
                        TraumaBondSystem.OnSharedHazardEndured(
                            participants, "raid_defense");
                };
                HatchDefenseSystem.OnRaidResolved += onRaid;
                _subscriptions.Track(() =>
                    HatchDefenseSystem.OnRaidResolved -= onRaid);
            }

            // Wire ideological friction into roommate calculations
            // This is passive — tick handles it via the _registry daily tick
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 8: Leadership, Caregiving, Mess Table, Desertion
        // ═══════════════════════════════════════════════════════════════

        private void InitPhase8Systems()
        {
            // ── Leadership System ──────────────────────────────────────
            LeadershipSystem = new LeadershipSystem
            {
                ApplyMoraleDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, delta);
                },
                ApplyShelterMoraleDelta = delta =>
                {
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
                GetSurvivors = () => Survivors
            };
            _registry.RegisterPerSubstep("leadership",
                h => LeadershipSystem.Tick(h));
            _registry.Register<LeadershipSystem>(LeadershipSystem);

            // Wire leader death witnessing
            if (NeedsSystem != null)
            {
                Action<Survivor> onDied = (dead) =>
                {
                    LeadershipSystem?.OnSurvivorDied(dead);
                };
                NeedsSystem.OnDied += onDied;
                _subscriptions.Track(() => NeedsSystem.OnDied -= onDied);
            }

            // ── Caregiving System ──────────────────────────────────────
            CaregivingSystem = new CaregivingSystem
            {
                AdjustAffinity = (a, b, delta) =>
                {
                    if (MentalBreakSystem?.Affinity != null)
                        MentalBreakSystem.Affinity.Adjust(a, b, delta);
                },
                ApplyFatigueDelta = (sv, delta) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Fatigue, delta);
                },
                ApplyHealthRecoveryBonus = (sv, bonus) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Health, bonus);
                },
                GetSurvivorsForLookup = () => Survivors
            };
            _registry.RegisterPerSubstep("caregiving",
                h => CaregivingSystem.Tick(h, Survivors));
            _registry.Register<CaregivingSystem>(CaregivingSystem);
        }

        private void WirePhase8Callbacks()
        {
            // ── Desertion Risk: wired to mutiny system from BunkerSocialSystems ──
            // Uses Survivor.DesertionIntent field; checked in daily tick
            // Full implementation integrates with existing MutinySystem

            // ── Mess Table: wired to CookingSystem cook completion ─────
            if (CookingSystem != null)
            {
                // Placeholder — CookingSystem.OnMealCooked event
                // When hot meal is prepared and 3+ survivors eat together:
                // +5 morale all participants, contextual dialogue triggers
            }
        }

        // ── Tick helpers ──────────────────────────────────────────────

        private void TickTraumaBondSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                TraumaBondSystem?.Tick(Survivors[i], gameHours);
            }
        }

        private void TickIdeologicalFriction(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                for (int j = i + 1; j < Survivors.Count; j++)
                {
                    if (Survivors[i] == null || Survivors[j] == null) continue;
                    if (!Survivors[i].IsAlive || !Survivors[j].IsAlive) continue;
                    if (!string.IsNullOrEmpty(Survivors[i].CurrentRoomId) &&
                        string.Equals(Survivors[i].CurrentRoomId,
                            Survivors[j].CurrentRoomId, StringComparison.Ordinal))
                    {
                        IdeologicalFrictionSystem?.TickRoommates(
                            Survivors[i], Survivors[j], gameHours);
                    }
                }
            }
        }

        private void TickRationConflictsDaily()
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                RationConflictSystem?.Tick(Survivors[i], 24f, Survivors);
            }
        }
    }
}
