using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    /// <summary>
    /// Survivor Narrative Arc System — tracks each named survivor's
    /// progression through their 4-stage narrative arc (Discovery →
    /// Investigation → Crisis → Resolution) with branching choices
    /// at the crisis point.
    ///
    /// Owns: Survivor.NarrativeArcMilestone, .NarrativeArcBranchId,
    /// .IsNarrativeArcComplete, .NarrativeStressAccumulation.
    ///
    /// Plain C#, save-safe. Integrates with PersonalQuestSystem,
    /// MoralBranchingSystem, and EventRunner.
    /// </summary>
    [Serializable]
    public class NarrativeArcSaveState
    {
        public List<string> CompletedArcSurvivorIds = new List<string>();
        public Dictionary<string, int> ArcMilestones = new Dictionary<string, int>();
        public Dictionary<string, string> ArcBranches = new Dictionary<string, string>();
    }

    public class SurvivorNarrativeArcSystem
    {
        // ── Milestone constants ────────────────────────────────────────
        public const int MilestoneDiscovery = 0;
        public const int MilestoneTrigger = 1;
        public const int MilestoneCrisis = 2;
        public const int MilestoneResolution = 3;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, int, string> OnArcMilestoneReached;
        // sv, milestoneIndex, branchId (empty if not crisis)
        public event Action<Survivor, string, string> OnArcBranchChosen;
        // sv, branchId, traitGranted
        public event Action<Survivor> OnArcCompleted;
        public event Action<Survivor, float> OnArcStressChanged;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, string> GrantTrait;
        public Action<Survivor, string> GrantLatentTrait;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<Survivor, float> ApplyStressDelta;
        public Action<string, string> FireNarrativeEvent;
        // eventId, context
        public Func<string, Survivor> FindSurvivor;
        public Func<int> GetDay;
        public Func<IReadOnlyList<Survivor>> GetSurvivors;
        public System.Random Rng;

        // ── Character arc definitions ──────────────────────────────────
        public static readonly Dictionary<string, string[]> CharacterArcStages =
            new Dictionary<string, string[]>
            {
                { "aris_thorne", new[] {
                    "The Cracked Floor: A structural crack appears in the bunker sub-floor. Aris identifies the damage and the risk.",
                    "The Overwork: Aris insists on working 24-hour shifts to repair the crack. His health deteriorates.",
                    "The Crisis: Force Aris to rest and accept imperfect repairs, or let him push through at any cost?",
                    "Resolution: Aris completes the Deep Aquifer — or collapses during a desperate repair, leaving his blueprints behind."
                }},
                { "maya_lin", new[] {
                    "The Signal: Maya intercepts a repeating 142.5 MHz distress beacon. The pattern is military — but the voice sounds civilian.",
                    "The Investigation: Send an expedition to the Communications Array to trace the signal. The transmitter is still running on backup power.",
                    "The Crisis: The signal is an automated death loop from Maya's brother's unit. Channel grief into saving others, or withdraw into silence?",
                    "Resolution: Maya leads the Unification Protocol, uniting fragmented survivor enclaves across the region through radio broadcast."
                }},
                { "victor_vance", new[] {
                    "The Refugee Mass: A family of desperate civilians arrives at the hatch during a fallout storm. Vance recommends turning them away.",
                    "The Command Decision: Vance's authority is tested. The bunker's survival vs. its humanity hangs in the balance.",
                    "The Crisis: Override Vance's recommendation and admit the refugees, or follow his cold tactical logic?",
                    "Resolution: Vance negotiates a historic peace treaty between the Garrison and Militia — or leads a preemptive strike against Sector 4."
                }},
                { "elena_rostov", new[] {
                    "The ARS Diagnosis: A survivor contracts terminal Acute Radiation Syndrome. Elena calculates the treatment cost: 80% of remaining medical supplies.",
                    "The Triage: Elena presents the cold math. Save one at enormous cost, or let them pass peacefully with sedatives.",
                    "The Crisis: Override Elena's triage logic and save the patient regardless, or trust her clinical judgment?",
                    "Resolution: Elena synthesizes the Ash Rot Remedy — or sacrifices herself during a biological containment breach to save the shelter."
                }}
            };

        // ── Branch definitions ─────────────────────────────────────────
        public static readonly Dictionary<string, (string branchA, string branchB, string traitA, string traitB)> CharacterBranches =
            new Dictionary<string, (string, string, string, string)>
            {
                { "aris_thorne", (
                    "pragmatic_acceptance", "obsessive_strain",
                    "trait_resilient_builder", "trait_severe_tremors"
                )},
                { "maya_lin", (
                    "the_beacon", "static_collapse",
                    "trait_voice_of_hope", "trait_catatonic_depression"
                )},
                { "victor_vance", (
                    "restored_humanity", "cold_efficiency",
                    "trait_guardian_captain", "trait_iron_sentinel"
                )},
                { "elena_rostov", (
                    "oath_restored", "triage_logic",
                    "trait_healers_soul", "trait_cold_triage"
                )}
            };

        // ── Milestone trigger checks ───────────────────────────────────

        /// <summary>Check if Aris should advance from discovery to trigger.</summary>
        public void CheckArisMilestone(Survivor sv, string eventId)
        {
            if (sv == null || sv.Id != "aris_thorne") return;
            if (sv.NarrativeArcMilestone != MilestoneDiscovery) return;
            if (eventId == "shelter_structural_damage" || eventId.Contains("crack"))
            {
                AdvanceMilestone(sv, MilestoneTrigger);
            }
        }

        /// <summary>Check if Maya should advance from discovery to trigger.</summary>
        public void CheckMayaMilestone(Survivor sv, string frequencyId)
        {
            if (sv == null || sv.Id != "maya_lin") return;
            if (sv.NarrativeArcMilestone != MilestoneDiscovery) return;
            if (frequencyId == "freq_distress_217_4" || frequencyId.Contains("142.5"))
            {
                AdvanceMilestone(sv, MilestoneTrigger);
            }
        }

        /// <summary>Check if Vance should advance.</summary>
        public void CheckVanceMilestone(Survivor sv, string eventId)
        {
            if (sv == null || sv.Id != "victor_vance") return;
            if (sv.NarrativeArcMilestone != MilestoneDiscovery) return;
            if (eventId == "refugee_arrival" || eventId.Contains("refugee"))
            {
                AdvanceMilestone(sv, MilestoneTrigger);
            }
        }

        /// <summary>Check if Elena should advance.</summary>
        public void CheckElenaMilestone(Survivor sv, string afflictionId)
        {
            if (sv == null || sv.Id != "elena_rostov") return;
            if (sv.NarrativeArcMilestone != MilestoneDiscovery) return;
            if (afflictionId == "acute_radiation_syndrome" ||
                afflictionId.Contains("ars") || afflictionId.Contains("terminal"))
            {
                AdvanceMilestone(sv, MilestoneTrigger);
            }
        }

        // ── Core progression ───────────────────────────────────────────

        public void AdvanceMilestone(Survivor sv, int newMilestone)
        {
            if (sv == null || !sv.IsAlive) return;
            if (sv.IsNarrativeArcComplete) return;
            if (newMilestone <= sv.NarrativeArcMilestone) return;

            sv.NarrativeArcMilestone = newMilestone;

            // Fire narrative event for this stage
            string eventId = $"narrative_{sv.Id}_stage_{newMilestone}";
            FireNarrativeEvent?.Invoke(eventId, sv.Id);

            if (newMilestone >= MilestoneResolution)
            {
                sv.IsNarrativeArcComplete = true;
                OnArcCompleted?.Invoke(sv);
            }

            string branchId = sv.NarrativeArcBranchId ?? "";
            OnArcMilestoneReached?.Invoke(sv, newMilestone, branchId);
        }

        /// <summary>Apply a branch choice at the crisis point (milestone 2→3).</summary>
        public bool ChooseBranch(Survivor sv, string branchId)
        {
            if (sv == null || sv.NarrativeArcMilestone != MilestoneCrisis) return false;
            if (!string.IsNullOrEmpty(sv.NarrativeArcBranchId)) return false;

            sv.NarrativeArcBranchId = branchId;
            sv.NarrativeStressAccumulation += 25f;

            // Grant the appropriate trait
            if (CharacterBranches.TryGetValue(sv.Id, out var branches))
            {
                string traitId = branchId == "a" ? branches.traitA : branches.traitB;
                GrantTrait?.Invoke(sv, traitId);
            }

            OnArcBranchChosen?.Invoke(sv, branchId,
                branchId == "a" ? "Branch A — Empathy" : "Branch B — Pragmatism");
            AdvanceMilestone(sv, MilestoneResolution);
            return true;
        }

        /// <summary>Apply narrative stress accumulation.</summary>
        public void AccumulateStress(Survivor sv, float amount)
        {
            if (sv == null) return;
            sv.NarrativeStressAccumulation = Mathf.Min(100f,
                sv.NarrativeStressAccumulation + amount);
            OnArcStressChanged?.Invoke(sv, sv.NarrativeStressAccumulation);
        }

        /// <summary>Tick — decay narrative stress slowly.</summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || sv.NarrativeStressAccumulation <= 0f) return;
            sv.NarrativeStressAccumulation = Mathf.Max(0f,
                sv.NarrativeStressAccumulation - 1f * (gameHours / 24f));
        }

        // ── Save/Load ─────────────────────────────────────────────────

        public NarrativeArcSaveState CaptureState(IReadOnlyList<Survivor> survivors)
        {
            var state = new NarrativeArcSaveState();
            if (survivors == null) return state;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null) continue;
                state.ArcMilestones[sv.Id] = sv.NarrativeArcMilestone;
                if (!string.IsNullOrEmpty(sv.NarrativeArcBranchId))
                    state.ArcBranches[sv.Id] = sv.NarrativeArcBranchId;
                if (sv.IsNarrativeArcComplete)
                    state.CompletedArcSurvivorIds.Add(sv.Id);
            }
            return state;
        }

        public void RestoreState(NarrativeArcSaveState state,
            IReadOnlyList<Survivor> survivors)
        {
            if (state == null || survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null) continue;
                if (state.ArcMilestones.TryGetValue(sv.Id, out int m))
                    sv.NarrativeArcMilestone = m;
                if (state.ArcBranches.TryGetValue(sv.Id, out string b))
                    sv.NarrativeArcBranchId = b;
                sv.IsNarrativeArcComplete =
                    state.CompletedArcSurvivorIds.Contains(sv.Id);
            }
        }
    }
}
