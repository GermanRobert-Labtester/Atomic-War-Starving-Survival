using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Dental Decay / Scurvy (Prompt #57). Tracks VitaminC intake per survivor.
    /// 30 days without fruit/veg triggers Scurvy: healed wounds reopen (old
    /// afflictions re-inflict), Toothache caps Sleep Quality at 50%.
    /// Pre-war vitamins are incredibly valuable. Save/load safe. Plain C#.
    /// </summary>
    public class ScurvySystem
    {
        /// <summary>Days without VitaminC before Scurvy onset.</summary>
        public const int ScurvyOnsetDays = 30;

        /// <summary>VitaminC units restored by one pre-war vitamin pill.</summary>
        public const float VitaminPillRestore = 30f;

        /// <summary>VitaminC units restored by mutated fungus (partial).</summary>
        public const float FungusRestore = 5f;

        /// <summary>VitaminC units restored by canned fruit (rare).</summary>
        public const float CannedFruitRestore = 20f;

        /// <summary>Sleep quality cap while Toothache is active (0..1).</summary>
        public const float ToothacheSleepQualityCap = 0.5f;

        /// <summary>Daily morale drain while Scurvy is active.</summary>
        public const float ScurvyMoraleDrainPerDay = 1.5f;

        /// <summary>Reopened wounds health cost when Scurvy triggers.</summary>
        public const float ReopenedWoundHealthCost = 15f;

        /// <summary>Days without VitaminC per survivor.</summary>
        private readonly Dictionary<string, float> _daysWithoutC = new Dictionary<string, float>();
        private readonly HashSet<string> _hasScurvy = new HashSet<string>();
        private readonly HashSet<string> _hasToothache = new HashSet<string>();

        private Func<string, Survivors.Survivor> _findSurvivor;
        private Action<Survivors.Survivor, string> _inflictAffliction;
        private Func<Survivors.Survivor, bool> _hasHealedAfflictions;

        // -- Events --
        public event Action<Survivors.Survivor> OnScurvyOnset;
        public event Action<Survivors.Survivor> OnToothacheStarted;
        public event Action<Survivors.Survivor> OnWoundsReopened;

        public ScurvySystem() { }

        public void Bind(
            Func<string, Survivors.Survivor> findSurvivor,
            Action<Survivors.Survivor, string> inflictAffliction,
            Func<Survivors.Survivor, bool> hasHealedAfflictions = null)
        {
            _findSurvivor = findSurvivor;
            _inflictAffliction = inflictAffliction;
            _hasHealedAfflictions = hasHealedAfflictions;
        }

        /// <summary>Days without VitaminC for a survivor.</summary>
        public float GetDaysWithoutC(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _daysWithoutC.TryGetValue(survivorId, out float d) ? d : 0f;
        }

        public bool HasScurvy(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _hasScurvy.Contains(survivorId);
        }

        public bool HasToothache(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _hasToothache.Contains(survivorId);
        }

        /// <summary>Consume VitaminC from food/medicine. Resets deficiency counter.</summary>
        public void ConsumeVitaminC(string survivorId, float amount)
        {
            if (string.IsNullOrEmpty(survivorId) || amount <= 0f) return;

            _daysWithoutC[survivorId] = Mathf.Max(0f, GetDaysWithoutC(survivorId) - amount);
            if (_daysWithoutC[survivorId] <= 0f)
            {
                _daysWithoutC.Remove(survivorId);
                // Scurvy and Toothache cure when vitamin levels restore.
                if (_hasScurvy.Remove(survivorId) || _hasToothache.Remove(survivorId))
                {
                    // Cured — no event needed but state is restored.
                }
            }
        }

        /// <summary>
        /// Daily tick. Advances VitaminC deficiency, triggers Scurvy, Toothache,
        /// and wound reopening.
        /// </summary>
        public void TickDaily(IReadOnlyList<Survivors.Survivor> survivors)
        {
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // Advance deficiency.
                float days = GetDaysWithoutC(sv.Id) + 1f;
                _daysWithoutC[sv.Id] = days;

                // Scurvy onset.
                if (days >= ScurvyOnsetDays && !_hasScurvy.Contains(sv.Id))
                {
                    _hasScurvy.Add(sv.Id);
                    _inflictAffliction?.Invoke(sv, AfflictionSO.Ids.Scurvy);
                    OnScurvyOnset?.Invoke(sv);

                    // Reopen healed wounds.
                    if (_hasHealedAfflictions != null && _hasHealedAfflictions(sv))
                    {
                        SurvivorNeedWrite.AdjustHealth(sv, -ReopenedWoundHealthCost);
                        // Re-inflict a bacterial infection from old wound sites.
                        _inflictAffliction?.Invoke(sv, AfflictionSO.Ids.BacterialInfection);
                        OnWoundsReopened?.Invoke(sv);
                    }
                }

                // Toothache onset (slightly after scurvy, same root cause).
                if (days >= ScurvyOnsetDays + 5 && !_hasToothache.Contains(sv.Id))
                {
                    _hasToothache.Add(sv.Id);
                    _inflictAffliction?.Invoke(sv, AfflictionSO.Ids.Toothache);
                    OnToothacheStarted?.Invoke(sv);
                }

                // Scurvy morale drain.
                if (_hasScurvy.Contains(sv.Id))
                {
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale - ScurvyMoraleDrainPerDay, 0f, 100f);
                }
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public ScurvySave CaptureState()
        {
            var keys = new string[_daysWithoutC.Count];
            var values = new float[_daysWithoutC.Count];
            int i = 0;
            foreach (var kv in _daysWithoutC) { keys[i] = kv.Key; values[i] = kv.Value; i++; }
            var scurvy = new string[_hasScurvy.Count]; _hasScurvy.CopyTo(scurvy);
            var tooth = new string[_hasToothache.Count]; _hasToothache.CopyTo(tooth);
            return new ScurvySave
            {
                DaysWithoutCKeys = keys,
                DaysWithoutCValues = values,
                HasScurvyIds = scurvy,
                HasToothacheIds = tooth
            };
        }

        public void RestoreState(ScurvySave save)
        {
            _daysWithoutC.Clear();
            _hasScurvy.Clear();
            _hasToothache.Clear();
            if (save == null) return;
            if (save.DaysWithoutCKeys != null)
            {
                for (int i = 0; i < save.DaysWithoutCKeys.Length; i++)
                {
                    if (string.IsNullOrEmpty(save.DaysWithoutCKeys[i])) continue;
                    float v = save.DaysWithoutCValues != null && i < save.DaysWithoutCValues.Length
                        ? save.DaysWithoutCValues[i] : 0f;
                    _daysWithoutC[save.DaysWithoutCKeys[i]] = v;
                }
            }
            if (save.HasScurvyIds != null)
                for (int i = 0; i < save.HasScurvyIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.HasScurvyIds[i])) _hasScurvy.Add(save.HasScurvyIds[i]);
            if (save.HasToothacheIds != null)
                for (int i = 0; i < save.HasToothacheIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.HasToothacheIds[i])) _hasToothache.Add(save.HasToothacheIds[i]);
        }
    }

    [Serializable]
    public class ScurvySave
    {
        public string[] DaysWithoutCKeys;
        public float[] DaysWithoutCValues;
        public string[] HasScurvyIds;
        public string[] HasToothacheIds;
    }
}
