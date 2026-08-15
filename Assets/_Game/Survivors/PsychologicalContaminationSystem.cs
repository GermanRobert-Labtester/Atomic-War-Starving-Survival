using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion IX/X — Psychological Contamination System. Survivors do not just
    /// carry radiation home; they carry trauma. Visiting mass graves, abattoirs,
    /// and daycare centers tags survivors with psychological contamination that
    /// manifests as work refusal, mental breaks, and permanent behavioral changes.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class PsychologicalContaminationSystem
    {
        // ── Contamination types ───────────────────────────────────────
        public const string Contam_ThousandYardStare = "contam_thousand_yard_stare";
        public const string Contam_DisgustCascade = "contam_disgust_cascade";
        public const string Contam_PhantomSmell = "contam_phantom_smell";
        public const string Contam_ChildCotTrauma = "contam_child_cot_trauma";

        // ── Location contamination profiles ───────────────────────────
        public static readonly Dictionary<string, string[]> LocationContaminations = new Dictionary<string, string[]>
        {
            { "location_stadium_evacuation_center", new[] { Contam_ThousandYardStare } },
            { "location_automated_abattoir", new[] { Contam_DisgustCascade, Contam_PhantomSmell } },
            { "location_sunshine_daycare", new[] { Contam_ChildCotTrauma, Contam_ThousandYardStare } },
            { "location_quarantine_mile", new[] { Contam_ThousandYardStare } },
            { "location_regional_blood_bank", new[] { Contam_DisgustCascade, Contam_PhantomSmell } }
        };

        // ── Duration constants ────────────────────────────────────────
        public const int StareDurationDays = 3;
        public const int DisgustDurationDays = 2;
        public const int PhantomSmellDurationDays = 5;
        public const int ChildCotDurationDays = 4;

        // ── Work restrictions ─────────────────────────────────────────
        public static readonly string[] StareBlockedActions = { "action_teach_child", "action_tell_stories" };
        public static readonly string[] DisgustBlockedActions = { "action_cook", "action_tend_hydroponics" };
        public static readonly string[] ChildCotBlockedActions = { "action_teach_child", "action_comfort_child" };

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, string> OnContaminationApplied;
        public event Action<string, string> OnContaminationExpired;
        public event Action<string> OnMentalBreakFromContamination;
        public event Action<string, string> OnMoralChronicleEntry;

        private readonly Dictionary<string, List<ContaminationEntry>> _bySurvivor =
            new Dictionary<string, List<ContaminationEntry>>();

        // ── Application ───────────────────────────────────────────────

        /// <summary>
        /// Apply psychological contamination from a location visit.
        /// </summary>
        public void ApplyContamination(string survivorId, string locationId,
            float moraleAtVisit, string survivorArchetype = null)
        {
            if (!LocationContaminations.TryGetValue(locationId, out var types)) return;

            for (int i = 0; i < types.Length; i++)
            {
                string type = types[i];
                int duration = type switch
                {
                    Contam_ThousandYardStare => StareDurationDays,
                    Contam_DisgustCascade => DisgustDurationDays,
                    Contam_PhantomSmell => PhantomSmellDurationDays,
                    Contam_ChildCotTrauma => ChildCotDurationDays,
                    _ => 3
                };

                var entry = new ContaminationEntry
                {
                    Type = type,
                    LocationId = locationId,
                    DaysRemaining = duration,
                    MoraleAtExposure = moraleAtVisit
                };

                if (!_bySurvivor.TryGetValue(survivorId, out var list))
                {
                    list = new List<ContaminationEntry>();
                    _bySurvivor[survivorId] = list;
                }

                // Don't stack same type
                bool alreadyHas = false;
                for (int j = 0; j < list.Count; j++)
                    if (list[j].Type == type) { alreadyHas = true; break; }
                if (alreadyHas) continue;

                list.Add(entry);
                OnContaminationApplied?.Invoke(survivorId, type);

                // Moral chronicle entry for specific locations
                if (locationId == "location_sunshine_daycare" && type == Contam_ChildCotTrauma)
                {
                    OnMoralChronicleEntry?.Invoke(survivorId,
                        "They came back from the daycare. They haven't spoken. " +
                        "They just sit by the heater, folding and unfolding a child's red coat.");
                }
                else if (locationId == "location_stadium_evacuation_center")
                {
                    OnMoralChronicleEntry?.Invoke(survivorId,
                        "Elena came back from the stadium. She hasn't spoken. " +
                        "We need the cloth. We don't need the coat.");
                }
            }
        }

        // ── Queries ───────────────────────────────────────────────────

        /// <summary>Check if a survivor has a specific contamination.</summary>
        public bool HasContamination(string survivorId, string type)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var list)) return false;
            for (int i = 0; i < list.Count; i++)
                if (list[i].Type == type) return true;
            return false;
        }

        /// <summary>Check if a survivor is blocked from a specific action.</summary>
        public bool IsActionBlocked(string survivorId, string actionId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var list)) return false;
            for (int i = 0; i < list.Count; i++)
            {
                var c = list[i];
                string[] blocked = c.Type switch
                {
                    Contam_ThousandYardStare => StareBlockedActions,
                    Contam_DisgustCascade => DisgustBlockedActions,
                    Contam_ChildCotTrauma => ChildCotBlockedActions,
                    _ => null
                };
                if (blocked != null)
                    for (int j = 0; j < blocked.Length; j++)
                        if (blocked[j] == actionId) return true;
            }
            return false;
        }

        /// <summary>Get all active contaminations for a survivor.</summary>
        public IReadOnlyList<ContaminationEntry> GetContaminations(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var list) ? list : null;
        }

        // ── Tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Advance contamination timers. Check for mental break triggers
        /// when contaminations overlap with low morale or bad assignments.
        /// </summary>
        public void Tick(float gameDays, string survivorId, float currentMorale,
            string currentAssignment)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var list)) return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var c = list[i];
                c.DaysRemaining -= gameDays;

                if (c.DaysRemaining <= 0f)
                {
                    OnContaminationExpired?.Invoke(survivorId, c.Type);
                    list.RemoveAt(i);
                    continue;
                }

                // Mental break check: contamination + bad assignment
                if (c.Type == Contam_ThousandYardStare
                    && (currentAssignment == "shelter_module_autopsy"
                        || currentAssignment == "shelter_module_bio_latrine"))
                {
                    OnMentalBreakFromContamination?.Invoke(survivorId);
                }
            }
        }

        // ── Save / Load ───────────────────────────────────────────────

        public PsychContaminationSave CaptureState()
        {
            var entries = new ContaminationSurvivorSave[_bySurvivor.Count];
            int i = 0;
            foreach (var kv in _bySurvivor)
            {
                var list = kv.Value;
                var saveList = new ContaminationEntrySave[list.Count];
                for (int j = 0; j < list.Count; j++)
                    saveList[j] = new ContaminationEntrySave
                    {
                        Type = list[j].Type,
                        LocationId = list[j].LocationId,
                        DaysRemaining = list[j].DaysRemaining,
                        MoraleAtExposure = list[j].MoraleAtExposure
                    };
                entries[i++] = new ContaminationSurvivorSave { SurvivorId = kv.Key, Entries = saveList };
            }
            return new PsychContaminationSave { Survivors = entries };
        }

        public void RestoreState(PsychContaminationSave save)
        {
            _bySurvivor.Clear();
            if (save?.Survivors == null) return;
            for (int i = 0; i < save.Survivors.Length; i++)
            {
                var sv = save.Survivors[i];
                if (sv == null || string.IsNullOrEmpty(sv.SurvivorId)) continue;
                var list = new List<ContaminationEntry>();
                if (sv.Entries != null)
                    for (int j = 0; j < sv.Entries.Length; j++)
                        if (sv.Entries[j] != null)
                            list.Add(new ContaminationEntry
                            {
                                Type = sv.Entries[j].Type,
                                LocationId = sv.Entries[j].LocationId,
                                DaysRemaining = sv.Entries[j].DaysRemaining,
                                MoraleAtExposure = sv.Entries[j].MoraleAtExposure
                            });
                if (list.Count > 0) _bySurvivor[sv.SurvivorId] = list;
            }
        }
    }

    public class ContaminationEntry
    {
        public string Type;
        public string LocationId;
        public float DaysRemaining;
        public float MoraleAtExposure;
    }

    [Serializable]
    public class PsychContaminationSave
    {
        public ContaminationSurvivorSave[] Survivors;
    }

    [Serializable]
    public class ContaminationSurvivorSave
    {
        public string SurvivorId;
        public ContaminationEntrySave[] Entries;
    }

    [Serializable]
    public class ContaminationEntrySave
    {
        public string Type;
        public string LocationId;
        public float DaysRemaining;
        public float MoraleAtExposure;
    }
}
