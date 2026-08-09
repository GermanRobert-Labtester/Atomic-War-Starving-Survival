using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Simulation
{
    /// <summary>Prompt #166 — Mental resilience: surviving trauma builds calluses, reducing future morale penalties.</summary>
    public class ResilienceSystem
    {
        private readonly Dictionary<string, float> _resilience = new Dictionary<string, float>();
        public const float ResiliencePerTrauma = 5f;
        public const float MaxResilience = 50f;
        public float GetResilience(string id) => _resilience.TryGetValue(id, out float r) ? r : 0f;
        public float ApplyMoraleReduction(string id, float penalty)
        {
            float r = GetResilience(id);
            return -Mathf.Max(0f, Mathf.Abs(penalty) - r);
        }
        public void OnTraumaSurvived(string id)
        {
            _resilience.TryGetValue(id, out float r);
            _resilience[id] = Mathf.Min(MaxResilience, r + ResiliencePerTrauma);
        }
        public ResilienceSave CaptureState()
        {
            var k = new string[_resilience.Count]; var v = new float[_resilience.Count]; int i = 0;
            foreach (var kv in _resilience) { k[i] = kv.Key; v[i] = kv.Value; i++; }
            return new ResilienceSave { Keys = k, Values = v };
        }
        public void RestoreState(ResilienceSave s) { _resilience.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) if (!string.IsNullOrEmpty(s.Keys[i])) _resilience[s.Keys[i]] = s.Values != null && i < s.Values.Length ? s.Values[i] : 0f; }
    }

    [Serializable] public class ResilienceSave { public string[] Keys; public float[] Values; }

    /// <summary>Prompt #167 — Compost bin: waste+spoiled meat → fertilizer, generates heat+mold in room.</summary>
    public class CompostSystem
    {
        public const string CompostModuleId = "compost_bin";
        public const float HeatGeneratedPerHour = 0.5f;
        public const float MoldGrowthPerHour = 0.03f;
        public const float WasteToFertilizerRatio = 5f; // 5 waste → 1 fertilizer
        /// <summary>Daily waste contribution per survivor (food scraps, etc.).</summary>
        public const float DailyWastePerSurvivor = 0.2f;
        private float _compostProgress;
        private float _fertilizerReady;
        public float FertilizerReady => _fertilizerReady;
        public float CompostProgress => _compostProgress;
        public event Action<float> OnFertilizerHarvested;
        public void AddWaste(float units) { _compostProgress += units; }
        /// <summary>Called once per day from SystemWiring. Adds per-survivor waste.</summary>
        public void DailyWasteFromSurvivors(int survivorCount)
        {
            if (survivorCount <= 0) return;
            _compostProgress += DailyWastePerSurvivor * survivorCount;
        }
        public void Tick(float gameHours, Shelter.ShelterRoom room)
        {
            if (room == null || _compostProgress <= 0f) return;
            float converted = Mathf.Min(_compostProgress, gameHours * 2f);
            _compostProgress -= converted;
            _fertilizerReady += converted / WasteToFertilizerRatio;
            room.Humidity = Mathf.Clamp01(room.Humidity + HeatGeneratedPerHour * gameHours * 0.1f);
            room.HasMold = true;
            room.MoldLevel = Mathf.Clamp01(room.MoldLevel + MoldGrowthPerHour * gameHours);
        }
        public float HarvestFertilizer() { float f = _fertilizerReady; _fertilizerReady = 0f; OnFertilizerHarvested?.Invoke(f); return f; }
        public CompostSave CaptureState() => new CompostSave { CompostProgress = _compostProgress, FertilizerReady = _fertilizerReady };
        public void RestoreState(CompostSave s) { _compostProgress = s?.CompostProgress ?? 0f; _fertilizerReady = s?.FertilizerReady ?? 0f; }
    }

    [Serializable] public class CompostSave { public float CompostProgress; public float FertilizerReady; }

    /// <summary>Prompt #168 — Scrap weaponry: cheap pipe guns, 25% misfire → destroyed + ShrapnelWound.</summary>
    public class ScrapWeaponSystem
    {
        public const float MisfireChance = 0.25f;
        public const float MisfireHealthDamage = 25f;
        public const string ShrapnelWoundId = "shrapnel_wound";
        public bool TryFireWeapon(Survivors.Survivor user, System.Random rng, Action<Survivors.Survivor, string> inflictAffliction)
        {
            if (user == null || rng == null) return true;
            if (rng.NextDouble() < MisfireChance)
            {
                SurvivorNeedWrite.SetHealth(user, user.Needs.Health - MisfireHealthDamage);
                inflictAffliction?.Invoke(user, ShrapnelWoundId);
                return false; // weapon destroyed
            }
            return true;
        }
    }

    /// <summary>Prompt #169 — Surgical sterilization: dirty tools guarantee Sepsis; must boil at Stove (water+fuel).</summary>
    public class SterilizationSystem
    {
        public const float BoilWaterCost = 2f;
        public const float BoilFuelCost = 1f;
        private bool _toolsSterile = true;
        public bool ToolsSterile => _toolsSterile;
        public event Action OnToolsSterilized;
        public void UseTools() { _toolsSterile = false; }
        public bool BoilTools(Func<string, float, float> consumeWater, Func<float, bool> consumeFuel)
        {
            if (_toolsSterile) return true;
            if (consumeWater == null || consumeFuel == null) return false;
            if (consumeWater("clean_water", BoilWaterCost) < BoilWaterCost) return false;
            if (!consumeFuel(BoilFuelCost)) return false;
            _toolsSterile = true; OnToolsSterilized?.Invoke(); return true;
        }
        public SterilizationSave CaptureState() => new SterilizationSave { ToolsSterile = _toolsSterile };
        public void RestoreState(SterilizationSave s) => _toolsSterile = s?.ToolsSterile ?? true;
    }

    [Serializable] public class SterilizationSave { public bool ToolsSterile = true; }

    /// <summary>Prompt #170 — Chelation therapy: only way to lower LifetimeRads; 5-day coma, constant IV, interrupt = kidney failure.</summary>
    public class ChelationSystem
    {
        public const float ChelationComaDays = 5f;
        public const float RadReduction = 500f;
        public const float DailyWaterIVCost = 3f;
        public const float DailyNutritionCost = 2f;
        private readonly Dictionary<string, float> _activeChelations = new Dictionary<string, float>();
        public bool IsUndergoingChelation(string survivorId) => _activeChelations.ContainsKey(survivorId);
        public void BeginChelation(string survivorId) { _activeChelations[survivorId] = ChelationComaDays * 24f; }
        /// <summary>Called once per day from SystemWiring. Advances timer; if elapsed, the chelation is complete.</summary>
        public bool AdvanceDay(string survivorId)
        {
            if (!_activeChelations.TryGetValue(survivorId, out float remaining)) return false;
            remaining -= 24f;
            if (remaining <= 0f)
            {
                _activeChelations.Remove(survivorId);
                return true; // completed
            }
            _activeChelations[survivorId] = remaining;
            return false;
        }
        public float GetRemainingHours(string survivorId) =>
            _activeChelations.TryGetValue(survivorId, out float r) ? r : 0f;
        public bool TickChelation(string survivorId, float gameHours, Func<string, float, bool> consumeWater, Func<string, float, bool> consumeFood, out bool completed)
        {
            completed = false;
            if (!_activeChelations.TryGetValue(survivorId, out float remaining)) return true;
            remaining -= gameHours;
            if (remaining <= 0f) { _activeChelations.Remove(survivorId); completed = true; return true; }
            _activeChelations[survivorId] = remaining;
            float days = gameHours / 24f;
            if (!consumeWater("clean_water", DailyWaterIVCost * days) || !consumeFood("canned_food", DailyNutritionCost * days))
            { _activeChelations.Remove(survivorId); return false; } // kidney failure
            return true;
        }
        public ChelationSave CaptureState()
        {
            var k = new string[_activeChelations.Count]; var v = new float[_activeChelations.Count]; int i = 0;
            foreach (var kv in _activeChelations) { k[i] = kv.Key; v[i] = kv.Value; i++; }
            return new ChelationSave { Keys = k, Values = v };
        }
        public void RestoreState(ChelationSave s) { _activeChelations.Clear(); if (s?.Keys == null) return; for (int i = 0; i < s.Keys.Length; i++) _activeChelations[s.Keys[i]] = s.Values != null && i < s.Values.Length ? s.Values[i] : 0f; }
    }

    [Serializable] public class ChelationSave { public string[] Keys; public float[] Values; }

    /// <summary>Prompt #171 — Overworld wind turbine: free power, 100% hatch visibility, needs perimeter defense.</summary>
    public class WindTurbineSystem
    {
        public const string TurbineModuleId = "wind_turbine";
        public const float BasePowerPerWindLevel = 5f;
        public const float MaxHatchVisibility = 1f;
        private bool _turbineBuilt;
        public bool IsBuilt => _turbineBuilt;
        public event Action OnTurbineBuilt;
        public void Build() { if (!_turbineBuilt) { _turbineBuilt = true; OnTurbineBuilt?.Invoke(); } }
        public float GetPowerOutput(float windSpeed) => _turbineBuilt ? BasePowerPerWindLevel * windSpeed : 0f;
        public WindTurbineSave CaptureState() => new WindTurbineSave { TurbineBuilt = _turbineBuilt };
        public void RestoreState(WindTurbineSave s) => _turbineBuilt = s?.TurbineBuilt ?? false;
    }

    [Serializable] public class WindTurbineSave { public bool TurbineBuilt; }

}
