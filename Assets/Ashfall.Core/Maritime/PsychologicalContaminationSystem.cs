using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — psychological contamination.
    /// Survivors carry trauma from horrific locations. Contamination manifests
    /// as work refusal, mental breaks, and behavioral changes. Engine-agnostic,
    /// deterministic, save/load safe.
    /// </summary>
    public class PsychologicalContaminationSystem
    {
        public const string Contam_ThousandYardStare = "contam_thousand_yard_stare";
        public const string Contam_DisgustCascade = "contam_disgust_cascade";
        public const string Contam_PhantomSmell = "contam_phantom_smell";
        public const string Contam_ChildCotTrauma = "contam_child_cot_trauma";

        public static readonly Dictionary<string, string[]> LocationContaminations =
            new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            { "location_stadium_evacuation_center", new[] { Contam_ThousandYardStare } },
            { "location_automated_abattoir", new[] { Contam_DisgustCascade, Contam_PhantomSmell } },
            { "location_sunshine_daycare", new[] { Contam_ChildCotTrauma, Contam_ThousandYardStare } },
            { "location_quarantine_mile", new[] { Contam_ThousandYardStare } },
            { "location_regional_blood_bank", new[] { Contam_DisgustCascade, Contam_PhantomSmell } }
        };

        public const int StareDurationDays = 3;
        public const int DisgustDurationDays = 2;
        public const int PhantomSmellDurationDays = 5;
        public const int ChildCotDurationDays = 4;

        public static readonly string[] StareBlockedActions = { "action_teach_child", "action_tell_stories" };
        public static readonly string[] DisgustBlockedActions = { "action_cook", "action_tend_hydroponics" };
        public static readonly string[] ChildCotBlockedActions = { "action_teach_child", "action_comfort_child" };

        public event Action<string, string> OnContaminationApplied;
        public event Action<string, string> OnContaminationExpired;
        public event Action<string> OnMentalBreakFromContamination;
        public event Action<string, string> OnMoralChronicleEntry;

        private readonly Dictionary<string, List<ContaminationEntry>> _bySurvivor =
            new Dictionary<string, List<ContaminationEntry>>(StringComparer.Ordinal);

        public void ApplyContamination(string survivorId, string locationId,
float moraleAtVisit, string? survivorArchetype = null)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(locationId)) return;
            if (!LocationContaminations.TryGetValue(locationId, out var types)) return;

            for (int i = 0; i < types.Length; i++)
            {
                string type = types[i];
                int duration = GetDuration(type);

                if (!_bySurvivor.TryGetValue(survivorId, out var list))
                {
                    list = new List<ContaminationEntry>();
                    _bySurvivor[survivorId] = list;
                }

                bool alreadyHas = false;
                for (int j = 0; j < list.Count; j++)
                    if (list[j].Type == type) { alreadyHas = true; break; }
                if (alreadyHas) continue;

                list.Add(new ContaminationEntry
                {
                    Type = type,
                    LocationId = locationId,
                    DaysRemaining = duration,
                    MoraleAtExposure = moraleAtVisit
                });
                OnContaminationApplied?.Invoke(survivorId, type);

                if (locationId == "location_sunshine_daycare" && type == Contam_ChildCotTrauma)
                    OnMoralChronicleEntry?.Invoke(survivorId,
                        "They came back from the daycare. They haven't spoken. " +
                        "They just sit by the heater, folding and unfolding a child's red coat.");
                else if (locationId == "location_stadium_evacuation_center")
                    OnMoralChronicleEntry?.Invoke(survivorId,
                        "Elena came back from the stadium. She hasn't spoken. " +
                        "We need the cloth. We don't need the coat.");
            }
        }

        private static int GetDuration(string type)
        {
            switch (type)
            {
                case Contam_ThousandYardStare: return StareDurationDays;
                case Contam_DisgustCascade: return DisgustDurationDays;
                case Contam_PhantomSmell: return PhantomSmellDurationDays;
                case Contam_ChildCotTrauma: return ChildCotDurationDays;
                default: return 3;
            }
        }

        public bool HasContamination(string survivorId, string type)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var list)) return false;
            for (int i = 0; i < list.Count; i++)
                if (list[i].Type == type) return true;
            return false;
        }

        public bool IsActionBlocked(string survivorId, string actionId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var list)) return false;
            for (int i = 0; i < list.Count; i++)
            {
                string[] blocked = GetBlockedActions(list[i].Type)!;
                if (blocked != null)
                    for (int j = 0; j < blocked.Length; j++)
                        if (blocked[j] == actionId) return true;
            }
            return false;
        }

        private static string[]? GetBlockedActions(string type)
        {
            switch (type)
            {
                case Contam_ThousandYardStare: return StareBlockedActions;
                case Contam_DisgustCascade: return DisgustBlockedActions;
                case Contam_ChildCotTrauma: return ChildCotBlockedActions;
                default: return null;
            }
        }

        public IReadOnlyList<ContaminationEntry>? GetContaminations(string survivorId)
            => _bySurvivor.TryGetValue(survivorId, out var list) ? list : null;

        public void Tick(float gameDays, string survivorId, float currentMorale, string currentAssignment)
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

                if (c.Type == Contam_ThousandYardStare
                    && (currentAssignment == "shelter_module_autopsy"
                        || currentAssignment == "shelter_module_bio_latrine"))
                    OnMentalBreakFromContamination?.Invoke(survivorId);
            }
        }

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
        public string Type = string.Empty;
        public string LocationId = string.Empty;
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
        public string SurvivorId = string.Empty;
        public ContaminationEntrySave[] Entries;
    }

    [Serializable]
    public class ContaminationEntrySave
    {
        public string Type = string.Empty;
        public string LocationId = string.Empty;
        public float DaysRemaining;
        public float MoraleAtExposure;
    }
}
