using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Simulation
{
    /// <summary>Prompt #172 — Antibiotic resistance: using expired meds builds resistance, eventually only pristine meds work.</summary>
    public class AntibioticResistanceSystem
    {
        public const float ExpiredFailureChancePerResistance = 0.1f;
        public const float MaxResistance = 5f;
        private readonly Dictionary<string, float> _resistance = new Dictionary<string, float>();
        public float GetResistance(string id) => _resistance.TryGetValue(id, out float r) ? r : 0f;
        public bool TryUseExpired(string id, System.Random rng)
        {
            float r = GetResistance(id);
            if (rng.NextDouble() < r * ExpiredFailureChancePerResistance) return false;
            _resistance[id] = Mathf.Min(MaxResistance, r + 1f); return true;
        }
        public AntibioticResistSave CaptureState()
        {
            var k = new string[_resistance.Count]; var v = new float[_resistance.Count]; int i = 0;
            foreach (var kv in _resistance) { k[i] = kv.Key; v[i] = kv.Value; i++; }
            return new AntibioticResistSave { Keys = k, Values = v };
        }
        public void RestoreState(AntibioticResistSave s) { _resistance.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) _resistance[s.Keys[i]] = s.Values != null && i < s.Values.Length ? s.Values[i] : 0f; }
    }

    [Serializable] public class AntibioticResistSave { public string[] Keys; public float[] Values; }

    /// <summary>Prompt #173 — Item encumbrance: loot dumped in airlock, survivors must haul to storage via Utility AI.</summary>
    public class InternalHaulingSystem
    {
        public const float HaulFatiguePerKg = 0.5f;
        public const string AirlockRoomId = "airlock";
        private float _airlockDumpedWeight;
        public float AirlockDumpedWeight => _airlockDumpedWeight;
        public event Action<float> OnLootDumped;
        public void DumpLootInAirlock(float weightKg) { _airlockDumpedWeight += weightKg; OnLootDumped?.Invoke(weightKg); }
        public float HaulFromAirlock(Survivors.Survivor hauler, float hours)
        {
            if (hauler == null || _airlockDumpedWeight <= 0f) return 0f;
            float capacity = 20f * hours;
            float moved = Mathf.Min(_airlockDumpedWeight, capacity);
            _airlockDumpedWeight -= moved;
            hauler.Needs.Fatigue = Mathf.Clamp(hauler.Needs.Fatigue + moved * HaulFatiguePerKg, 0f, 100f);
            return moved;
        }
        public HaulingSave CaptureState() => new HaulingSave { AirlockDumpedWeight = _airlockDumpedWeight };
        public void RestoreState(HaulingSave s) => _airlockDumpedWeight = s?.AirlockDumpedWeight ?? 0f;
    }

    [Serializable] public class HaulingSave { public float AirlockDumpedWeight; }

    /// <summary>Prompt #174 — Weapon maintenance: firing degrades, humidity rusts, <50% durability = jam during defense.
    /// Prompt #182 — jam clear ticks (default 5; Tap-Rack-Bang reduces to 1).</summary>
    public class WeaponMaintenanceSystem
    {
        public const float FireDegradePerShot = 2f;
        public const float RustPerHourInHumidity = 0.5f;
        public const float JamThreshold = 50f;
        public const float MaxDurability = 100f;
        public const float GunOilRepair = 30f;
        public const int DefaultJamClearTicks = 5;
        public const float DefaultJamChanceWhenEligible = 0.5f;

        private readonly Dictionary<string, float> _weaponDurability = new Dictionary<string, float>();
        /// <summary>Active jams: weaponId → ticks remaining until clear.</summary>
        private readonly Dictionary<string, int> _jamTicksRemaining = new Dictionary<string, int>();

        public float GetDurability(string weaponId) => _weaponDurability.TryGetValue(weaponId, out float d) ? d : MaxDurability;
        public void Fire(string weaponId) { float d = GetDurability(weaponId); _weaponDurability[weaponId] = Mathf.Max(0f, d - FireDegradePerShot); }
        public bool CanJam(string weaponId) => GetDurability(weaponId) < JamThreshold;
        public bool IsJammed(string weaponId) =>
            !string.IsNullOrEmpty(weaponId)
            && _jamTicksRemaining.TryGetValue(weaponId, out int t)
            && t > 0;
        public int GetJamTicksRemaining(string weaponId) =>
            _jamTicksRemaining.TryGetValue(weaponId, out int t) ? Mathf.Max(0, t) : 0;

        /// <summary>
        /// Start a jam on this weapon. <paramref name="clearTicks"/> is how long the
        /// vulnerability window lasts (1 with Tap-Rack-Bang, else 5).
        /// </summary>
        public void StartJam(string weaponId, int clearTicks = DefaultJamClearTicks)
        {
            if (string.IsNullOrEmpty(weaponId)) return;
            _jamTicksRemaining[weaponId] = Mathf.Max(1, clearTicks);
        }

        /// <summary>
        /// Roll a jam if durability is below threshold. Returns true if jam started.
        /// </summary>
        public bool TryJam(
            string weaponId,
            System.Random rng = null,
            int clearTicks = DefaultJamClearTicks,
            float chanceWhenEligible = DefaultJamChanceWhenEligible)
        {
            if (string.IsNullOrEmpty(weaponId) || IsJammed(weaponId)) return false;
            if (!CanJam(weaponId)) return false;
            rng ??= new System.Random();
            if (rng.NextDouble() >= chanceWhenEligible) return false;
            StartJam(weaponId, clearTicks);
            return true;
        }

        /// <summary>
        /// Advance jam clear by one tick. Returns true when the jam fully clears
        /// this tick (weapon is free again).
        /// </summary>
        public bool TickJamClear(string weaponId)
        {
            if (string.IsNullOrEmpty(weaponId)) return false;
            if (!_jamTicksRemaining.TryGetValue(weaponId, out int t) || t <= 0) return false;
            t--;
            if (t <= 0)
            {
                _jamTicksRemaining.Remove(weaponId);
                return true;
            }
            _jamTicksRemaining[weaponId] = t;
            return false;
        }

        public void ClearJam(string weaponId)
        {
            if (string.IsNullOrEmpty(weaponId)) return;
            _jamTicksRemaining.Remove(weaponId);
        }

        public void TickRust(string weaponId, float gameHours, float humidity)
        {
            if (humidity <= 0.5f) return;
            float d = GetDurability(weaponId);
            _weaponDurability[weaponId] = Mathf.Max(0f, d - RustPerHourInHumidity * gameHours);
        }
        public void OilWeapon(string weaponId)
        {
            _weaponDurability[weaponId] = MaxDurability;
            ClearJam(weaponId);
        }
        public WeaponMaintSave CaptureState()
        {
            var k = new string[_weaponDurability.Count]; var v = new float[_weaponDurability.Count]; int i = 0;
            foreach (var kv in _weaponDurability) { k[i] = kv.Key; v[i] = kv.Value; i++; }
            var jk = new string[_jamTicksRemaining.Count]; var jv = new int[_jamTicksRemaining.Count]; int j = 0;
            foreach (var kv in _jamTicksRemaining) { jk[j] = kv.Key; jv[j] = kv.Value; j++; }
            return new WeaponMaintSave { Keys = k, Values = v, JamKeys = jk, JamTicks = jv };
        }
        public void RestoreState(WeaponMaintSave s)
        {
            _weaponDurability.Clear();
            _jamTicksRemaining.Clear();
            if (s?.Keys == null) return;
            for (int i = 0; i < s.Keys.Length; i++)
                _weaponDurability[s.Keys[i]] = s.Values != null && i < s.Values.Length ? s.Values[i] : MaxDurability;
            if (s.JamKeys == null) return;
            for (int i = 0; i < s.JamKeys.Length; i++)
            {
                if (string.IsNullOrEmpty(s.JamKeys[i])) continue;
                int ticks = s.JamTicks != null && i < s.JamTicks.Length ? s.JamTicks[i] : 0;
                if (ticks > 0) _jamTicksRemaining[s.JamKeys[i]] = ticks;
            }
        }
    }

    [Serializable]
    public class WeaponMaintSave
    {
        public string[] Keys;
        public float[] Values;
        public string[] JamKeys;
        public int[] JamTicks;
    }

    /// <summary>Prompt #175 — Room aesthetics: light/temp/hygiene/decor score → morale aura, survivors pick best rooms.</summary>
    public class RoomAestheticsSystem
    {
        public const float LightingWeight = 0.3f;
        public const float TempWeight = 0.25f;
        public const float HygieneWeight = 0.25f;
        public const float DecorWeight = 0.2f;
        public const float MaxAuraMoralePerHour = 2f;
        public const float MinAuraMoralePerHour = -1.5f;
        private readonly Dictionary<string, float> _roomDecor = new Dictionary<string, float>();
        public void SetDecor(string roomId, float value) { _roomDecor[roomId] = Mathf.Clamp01(value); }
        public float GetDecor(string roomId) => _roomDecor.TryGetValue(roomId, out float d) ? d : 0f;
        public float CalculateScore(float lighting, float temp, float hygiene, string roomId)
        {
            float decor = GetDecor(roomId);
            float tempScore = Mathf.Clamp01(1f - Mathf.Abs(temp - 20f) / 30f);
            return lighting * LightingWeight + tempScore * TempWeight + hygiene * HygieneWeight + decor * DecorWeight;
        }
        public float GetMoraleAura(float score) => Mathf.Lerp(MinAuraMoralePerHour, MaxAuraMoralePerHour, score);
        public AestheticsSave CaptureState()
        {
            var k = new string[_roomDecor.Count]; var v = new float[_roomDecor.Count]; int i = 0;
            foreach (var kv in _roomDecor) { k[i] = kv.Key; v[i] = kv.Value; i++; }
            return new AestheticsSave { Keys = k, Values = v };
        }
        public void RestoreState(AestheticsSave s) { _roomDecor.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) _roomDecor[s.Keys[i]] = s.Values != null && i < s.Values.Length ? s.Values[i] : 0f; }
    }

    [Serializable] public class AestheticsSave { public string[] Keys; public float[] Values; }

    /// <summary>Prompt #176 — Ham Radio endgame: broadcast 20 days via tower, contact carrier, need 100 explosives for LZ.</summary>
    public class HamRadioSystem
    {
        public const float BroadcastDaysRequired = 20f;
        public const int ExplosivesForLZ = 100;
        private float _broadcastDays;
        private bool _carrierContacted;
        private bool _lzCleared;
        public bool CarrierContacted => _carrierContacted;
        public bool LZCleared => _lzCleared;
        public bool VictoryReady => _carrierContacted && _lzCleared;
        public event Action OnCarrierContacted;
        public event Action OnVictoryReady;
        public void TickBroadcast(float gameHours, bool radioTowerActive)
        {
            if (!radioTowerActive || _carrierContacted) return;
            _broadcastDays += gameHours / 24f;
            if (_broadcastDays >= BroadcastDaysRequired && !_carrierContacted) { _carrierContacted = true; OnCarrierContacted?.Invoke(); }
        }
        public bool ClearLZ(int explosivesSpent)
        {
            if (_lzCleared || !_carrierContacted) return false;
            _lzCleared = explosivesSpent >= ExplosivesForLZ;
            if (_lzCleared) OnVictoryReady?.Invoke();
            return _lzCleared;
        }
        public HamRadioSave CaptureState() => new HamRadioSave { BroadcastDays = _broadcastDays, CarrierContacted = _carrierContacted, LZCleared = _lzCleared };
        public void RestoreState(HamRadioSave s) { _broadcastDays = s?.BroadcastDays ?? 0f; _carrierContacted = s?.CarrierContacted ?? false; _lzCleared = s?.LZCleared ?? false; }
    }

    [Serializable] public class HamRadioSave { public float BroadcastDays; public bool CarrierContacted; public bool LZCleared; }

}
