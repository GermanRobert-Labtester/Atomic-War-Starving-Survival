using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Per-survivor balance stats recorded during a headless simulation run.
    /// </summary>
    public class BalanceRecord
    {
        public string SurvivorId;
        public float DaysSurvived;
        public string CauseOfDeath;
        public float TotalRadiationExposure;
        public float PeakRadiation;
        public int HoursStarving;
        public int HoursDehydrated;
        public int HoursFreezing;
        public bool DevelopedChronicIllness;
        public bool DevelopedARS;
    }

    /// <summary>
    /// Tracks survivor outcomes across a multi-day smoke simulation.
    /// </summary>
    public class BalanceTracker
    {
        public readonly List<BalanceRecord> Records = new List<BalanceRecord>();
        private readonly Dictionary<string, BalanceRecord> _active = new Dictionary<string, BalanceRecord>();

        public void Register(Survivor sv)
        {
            _active[sv.Id] = new BalanceRecord
            {
                SurvivorId = sv.Id,
                DaysSurvived = 0f,
                CauseOfDeath = "alive",
                TotalRadiationExposure = sv.LifetimeRadiationExposure
            };
        }

        public void Tick(float gameHours, float currentDay, IReadOnlyList<Survivor> survivors)
        {
            foreach (var sv in survivors)
            {
                if (!_active.TryGetValue(sv.Id, out var rec)) continue;
                UpdateLivingMetrics(rec, sv);
                TryRecordDeath(rec, sv, currentDay);
            }
        }

        public void FinalizeAlive(float finalDay, IReadOnlyList<Survivor> survivors)
        {
            foreach (var sv in survivors)
            {
                if (!_active.TryGetValue(sv.Id, out var rec)) continue;
                if (rec.CauseOfDeath != "alive") continue;
                rec.DaysSurvived = finalDay;
                Records.Add(rec);
            }
        }

        public void LogReport()
        {
            Debug.Log("=== BALANCE REPORT ===");
            Debug.Log($"{"ID",-20} {"Days",6} {"Cause",-18} {"AvgRad",8} {"PeakRad",8} {"Starv",6} {"Dehyd",6} {"Freez",6} {"CI",4} {"ARS",5}");
            foreach (var r in Records)
            {
                float avgRad = r.DaysSurvived > 0 ? r.TotalRadiationExposure / (r.DaysSurvived * 24f) : 0f;
                Debug.Log($"{r.SurvivorId,-20} {r.DaysSurvived,6:F1} {r.CauseOfDeath,-18} {avgRad,8:F2} {r.PeakRadiation,8:F1} {r.HoursStarving,6} {r.HoursDehydrated,6} {r.HoursFreezing,6} {r.DevelopedChronicIllness,4} {r.DevelopedARS,5}");
            }
            Debug.Log("======================");
        }

        private static void UpdateLivingMetrics(BalanceRecord rec, Survivor sv)
        {
            rec.TotalRadiationExposure = sv.LifetimeRadiationExposure;
            rec.PeakRadiation = Mathf.Max(rec.PeakRadiation, sv.RadiationDose);

            if (sv.Needs.Hunger >= 100f) rec.HoursStarving++;
            if (sv.Needs.Thirst >= 100f) rec.HoursDehydrated++;
            if (sv.Needs.Warmth <= 10f) rec.HoursFreezing++;

            if (sv.HasChronicIllness) rec.DevelopedChronicIllness = true;
            if (sv.HasAcuteRadiationSickness) rec.DevelopedARS = true;
        }

        private void TryRecordDeath(BalanceRecord rec, Survivor sv, float currentDay)
        {
            if (sv.State != SurvivorState.Dead || rec.CauseOfDeath != "alive") return;
            rec.DaysSurvived = currentDay;
            rec.CauseOfDeath = DetermineCause(sv);
            Records.Add(rec);
        }

        private static string DetermineCause(Survivor sv)
        {
            if (sv.RadiationDose >= 100f)
                return "radiation_overdose";
            if (sv.Needs.Health > 0f)
                return "unknown";
            return DetermineHealthDepletedCause(sv);
        }

        private static string DetermineHealthDepletedCause(Survivor sv)
        {
            bool starved = sv.Needs.Hunger >= 100f;
            bool dehydrated = sv.Needs.Thirst >= 100f;
            if (starved && dehydrated)
                return "starvation+dehydration";
            if (starved)
                return "starvation";
            if (dehydrated)
                return "dehydration";
            return SecondaryHealthCause(sv);
        }

        private static string SecondaryHealthCause(Survivor sv)
        {
            if (sv.Needs.Warmth <= 10f)
                return "hypothermia";
            if (sv.HasAcuteRadiationSickness)
                return "acute_radiation";
            return "health_depleted";
        }
    }
}
