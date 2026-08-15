using System;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    [Serializable]
    public struct SurvivorData
    {
        public string id;
        public string name;
        public string roleTag;
        public float health;      // 0 - 100
        public float morale;      // 0 - 100
        public float fatigue;     // 0 - 100
        public float radiation;   // 0 - 100
        public string statusLabel; // HEALTHY, STRESSED, ILL, CRITICAL
        public string bloodType;  // O-, A+, B+, AB-, etc.

        public SurvivorData(string id, string name, string roleTag, float health, float morale, float fatigue, float radiation, string statusLabel, string bloodType)
        {
            this.id = id;
            this.name = name;
            this.roleTag = roleTag;
            this.health = Mathf.Clamp(health, 0f, 100f);
            this.morale = Mathf.Clamp(morale, 0f, 100f);
            this.fatigue = Mathf.Clamp(fatigue, 0f, 100f);
            this.radiation = Mathf.Clamp(radiation, 0f, 100f);
            this.statusLabel = statusLabel ?? "HEALTHY";
            this.bloodType = bloodType ?? "O-";
        }
    }
}
