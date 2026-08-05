using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Simulation
{
    /// <summary>Prompt #177 — Triage priority: assign medication permissions per survivor to prevent AI wasting rare meds.</summary>
    public class TriageBoardSystem
    {
        public enum TriageLevel { None, Basic, Full }
        private readonly Dictionary<string, TriageLevel> _permissions = new Dictionary<string, TriageLevel>();
        public TriageLevel GetPermission(string survivorId) => _permissions.TryGetValue(survivorId, out var t) ? t : TriageLevel.Full;
        public void SetPermission(string survivorId, TriageLevel level) { _permissions[survivorId] = level; }
        public bool CanReceiveMedication(string survivorId, string medTier)
        {
            var p = GetPermission(survivorId);
            if (p == TriageLevel.None) return false;
            if (p == TriageLevel.Basic && medTier == "advanced") return false;
            return true;
        }
        public TriageSave CaptureState()
        {
            var k = new string[_permissions.Count]; var v = new int[_permissions.Count]; int i = 0;
            foreach (var kv in _permissions) { k[i] = kv.Key; v[i] = (int)kv.Value; i++; }
            return new TriageSave { Keys = k, Values = v };
        }
        public void RestoreState(TriageSave s) { _permissions.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) _permissions[s.Keys[i]] = (TriageLevel)(s.Values != null && i < s.Values.Length ? s.Values[i] : 2); }
    }

    [Serializable] public class TriageSave { public string[] Keys; public int[] Values; }

    /// <summary>Prompt #178 — Polypharmacy: iodine+antibiotics+morphine in 12h window = ToxicOverdose affliction.</summary>
    public class PolypharmacySystem
    {
        public const float InteractionWindowHours = 12f;
        public const string ToxicOverdoseId = "toxic_overdose";
        private readonly Dictionary<string, List<float>> _recentDoses = new Dictionary<string, List<float>>();
        public void RecordDose(string survivorId, string drugId, float gameHour)
        {
            if (!_recentDoses.TryGetValue(survivorId, out var list)) { list = new List<float>(); _recentDoses[survivorId] = list; }
            list.Add(gameHour);
            PruneOld(survivorId, gameHour);
            if (CountRecent(survivorId, gameHour) >= 3) TriggerOverdose(survivorId);
        }
        private int CountRecent(string sid, float now)
        {
            if (!_recentDoses.TryGetValue(sid, out var list)) return 0;
            int c = 0; for (int i = 0; i < list.Count; i++) if (now - list[i] <= InteractionWindowHours) c++; return c;
        }
        private void PruneOld(string sid, float now)
        {
            if (!_recentDoses.TryGetValue(sid, out var list)) return;
            for (int i = list.Count - 1; i >= 0; i--) if (now - list[i] > InteractionWindowHours) list.RemoveAt(i);
        }
        /// <summary>Public wrapper around <see cref="PruneOld"/> for SystemWiring.</summary>
        public void PruneStaleDoses(string survivorId, float nowGameHour) => PruneOld(survivorId, nowGameHour);
        public int RecentDoseCount(string survivorId, float nowGameHour) => CountRecent(survivorId, nowGameHour);
        public event Action<string> OnToxicOverdose;
        private void TriggerOverdose(string sid) { OnToxicOverdose?.Invoke(sid); }
        public PolypharmSave CaptureState()
        {
            var k = new string[_recentDoses.Count]; var v = new float[_recentDoses.Count][]; int i = 0;
            foreach (var kv in _recentDoses) { k[i] = kv.Key; v[i] = kv.Value.ToArray(); i++; }
            return new PolypharmSave { Keys = k, ValuesJagged = v };
        }
        public void RestoreState(PolypharmSave s) { _recentDoses.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) if (!string.IsNullOrEmpty(s.Keys[i]) && s.ValuesJagged != null && i < s.ValuesJagged.Length) _recentDoses[s.Keys[i]] = new List<float>(s.ValuesJagged[i] ?? Array.Empty<float>()); }
    }

    [Serializable] public class PolypharmSave { public string[] Keys; public float[][] ValuesJagged; }

}
