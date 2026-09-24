// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Events
{
    [Serializable]
    public sealed class HolidayDef
    {
        [JsonPropertyName("holiday_id")]
        public string HolidayId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("trigger_day")]
        public int TriggerDay { get; set; } = 1;

        [JsonPropertyName("base_morale_boost")]
        public float BaseMoraleBoost { get; set; } = 10.0f;

        [JsonPropertyName("food_cost")]
        public int FoodCost { get; set; } = 5;

        [JsonPropertyName("fuel_cost")]
        public int FuelCost { get; set; } = 2;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("allowed_activities")]
        public List<string> AllowedActivities { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class AnniversaryTypeDef
    {
        [JsonPropertyName("type_id")]
        public string TypeId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("base_morale_boost")]
        public float BaseMoraleBoost { get; set; } = 5.0f;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class CelebrationScaleDef
    {
        [JsonPropertyName("scale_id")]
        public string ScaleId { get; set; } = "small";

        [JsonPropertyName("cost_multiplier")]
        public float CostMultiplier { get; set; } = 1.0f;

        [JsonPropertyName("morale_multiplier")]
        public float MoraleMultiplier { get; set; } = 1.0f;

        [JsonPropertyName("is_memorable")]
        public bool IsMemorable { get; set; }
    }

    [Serializable]
    public sealed class CelebrationCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("holidays")]
        public List<HolidayDef> Holidays { get; set; } = new List<HolidayDef>();

        [JsonPropertyName("anniversary_types")]
        public List<AnniversaryTypeDef> AnniversaryTypes { get; set; } = new List<AnniversaryTypeDef>();

        [JsonPropertyName("scales")]
        public List<CelebrationScaleDef> Scales { get; set; } = new List<CelebrationScaleDef>();
    }

    [Serializable]
    public sealed class CelebrationRecord
    {
        public string CelebrationId { get; set; } = string.Empty;
        public string HolidayId { get; set; } = string.Empty;
        public int Day { get; set; }
        public string Scale { get; set; } = "small";
        public float MoraleGained { get; set; }
        public int FoodSpent { get; set; }
        public int FuelSpent { get; set; }
        public bool IsMemorable { get; set; }
        public string Summary { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class CelebrationSaveState
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("current_day")]
        public int CurrentDay { get; set; } = 1;

        [JsonPropertyName("tradition_streak")]
        public int TraditionStreak { get; set; }

        [JsonPropertyName("celebration_history")]
        public List<CelebrationRecord> CelebrationHistory { get; set; } = new List<CelebrationRecord>();

        [JsonPropertyName("recorded_anniversaries")]
        public List<string> RecordedAnniversaries { get; set; } = new List<string>();

        [JsonPropertyName("held_holiday_occurrences")]
        public List<string> HeldHolidayOccurrences { get; set; } = new List<string>();

        [JsonPropertyName("skipped_holiday_occurrences")]
        public List<string> SkippedHolidayOccurrences { get; set; } = new List<string>();
    }

    /// <summary>
    /// Plan 170 / DEC-162: Seasonal Events & Celebrations System.
    /// Manages communal holiday observances, anniversary remembrances, celebration scaling,
    /// tradition streaks, and morale rewards in the shelter.
    /// </summary>
    public sealed class SeasonalCelebrationSystem
    {
        private readonly Dictionary<string, HolidayDef> _holidays = new Dictionary<string, HolidayDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, AnniversaryTypeDef> _anniversaries = new Dictionary<string, AnniversaryTypeDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, CelebrationScaleDef> _scales = new Dictionary<string, CelebrationScaleDef>(StringComparer.OrdinalIgnoreCase);

        private readonly List<CelebrationRecord> _history = new List<CelebrationRecord>();
        private readonly List<string> _recordedAnniversaries = new List<string>();
        private readonly HashSet<string> _heldHolidayOccurrences = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _skippedHolidayOccurrences = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private int _currentDay = 1;
        private int _traditionStreak;

        public IReadOnlyDictionary<string, HolidayDef> Holidays => _holidays;
        public IReadOnlyDictionary<string, AnniversaryTypeDef> Anniversaries => _anniversaries;
        public IReadOnlyDictionary<string, CelebrationScaleDef> Scales => _scales;
        public IReadOnlyList<CelebrationRecord> History => _history;
        public IReadOnlyList<string> RecordedAnniversaries => _recordedAnniversaries;
        public int CurrentDay => _currentDay;
        public int TraditionStreak => _traditionStreak;
        public IReadOnlyCollection<string> HeldHolidayOccurrences => _heldHolidayOccurrences;
        public IReadOnlyCollection<string> SkippedHolidayOccurrences => _skippedHolidayOccurrences;

        public Action<CelebrationRecord>? OnCelebrationHeldSeam { get; set; }
        public Action<string, int>? OnHolidaySkippedSeam { get; set; }
        public Action<string, string, float>? OnAnniversaryCommemoratedSeam { get; set; }

        public SeasonalCelebrationSystem()
        {
            LoadFallbackCatalog();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<CelebrationCatalogData>(json, options);
                if (data != null)
                {
                    if (data.Holidays != null && data.Holidays.Count > 0)
                    {
                        _holidays.Clear();
                        foreach (var h in data.Holidays)
                            if (!string.IsNullOrEmpty(h.HolidayId)) _holidays[h.HolidayId] = h;
                    }

                    if (data.AnniversaryTypes != null && data.AnniversaryTypes.Count > 0)
                    {
                        _anniversaries.Clear();
                        foreach (var a in data.AnniversaryTypes)
                            if (!string.IsNullOrEmpty(a.TypeId)) _anniversaries[a.TypeId] = a;
                    }

                    if (data.Scales != null && data.Scales.Count > 0)
                    {
                        _scales.Clear();
                        foreach (var s in data.Scales)
                            if (!string.IsNullOrEmpty(s.ScaleId)) _scales[s.ScaleId] = s;
                    }
                }
            }
            catch (JsonException)
            {
                // Catalog parse failure; fallback retained
            }
        }

        public HolidayDef? CheckHolidayForDay(int day)
        {
            _currentDay = day;
            int cycleDay = (Math.Max(1, day) - 1) % 360 + 1;
            return _holidays.Values.FirstOrDefault(h => h.TriggerDay == cycleDay);
        }

        public bool IsHolidayOccurrenceResolved(string holidayId, int day)
        {
            if (string.IsNullOrWhiteSpace(holidayId)) return false;
            string key = OccurrenceKey(holidayId, day);
            return _heldHolidayOccurrences.Contains(key) || _skippedHolidayOccurrences.Contains(key);
        }

        public bool WasHolidayOccurrenceHeld(string holidayId, int day)
            => !string.IsNullOrWhiteSpace(holidayId) && _heldHolidayOccurrences.Contains(OccurrenceKey(holidayId, day));

        public bool WasHolidayOccurrenceSkipped(string holidayId, int day)
            => !string.IsNullOrWhiteSpace(holidayId) && _skippedHolidayOccurrences.Contains(OccurrenceKey(holidayId, day));

        public bool TryHoldCelebration(
            string holidayId,
            string scaleId,
            int participantCount,
            int day,
            string foodItemId,
            string fuelItemId,
            IPlayerInventoryPort? inventory,
            ISeededRng? rng,
            out CelebrationRecord? record)
        {
            record = null;
            if (!_holidays.TryGetValue(holidayId ?? string.Empty, out var holiday)) return false;
            if (!_scales.TryGetValue(scaleId ?? string.Empty, out var scale)) return false;
            if (!string.Equals(CheckHolidayForDay(day)?.HolidayId, holiday.HolidayId, StringComparison.OrdinalIgnoreCase)
                || IsHolidayOccurrenceResolved(holidayId, day)
                || inventory == null)
                return false;
            if ((holiday.FoodCost > 0 && string.IsNullOrWhiteSpace(foodItemId))
                || (holiday.FuelCost > 0 && string.IsNullOrWhiteSpace(fuelItemId)))
                return false;

            var bill = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            AddCost(bill, foodItemId, (int)Math.Ceiling(holiday.FoodCost * scale.CostMultiplier));
            AddCost(bill, fuelItemId, (int)Math.Ceiling(holiday.FuelCost * scale.CostMultiplier));
            string occurrence = OccurrenceKey(holidayId, day);
            CelebrationRecord? committedRecord = null;
            bool committed = inventory.TryConsumeBill(bill, () =>
            {
                _currentDay = day;
                committedRecord = CreateCelebrationRecord(holidayId, holiday, scale, participantCount, rng);
                _heldHolidayOccurrences.Add(occurrence);
            });
            if (!committed || committedRecord == null) return false;

            record = committedRecord;
            OnCelebrationHeldSeam?.Invoke(committedRecord);
            return true;
        }

        public bool TrySkipHoliday(string holidayId, int day, out float moralePenalty)
        {
            moralePenalty = 0f;
            if (string.IsNullOrWhiteSpace(holidayId)
                || !_holidays.TryGetValue(holidayId, out var holiday)
                || !string.Equals(CheckHolidayForDay(day)?.HolidayId, holiday.HolidayId, StringComparison.OrdinalIgnoreCase)
                || IsHolidayOccurrenceResolved(holidayId, day))
                return false;

            _currentDay = day;
            _traditionStreak = 0;
            _skippedHolidayOccurrences.Add(OccurrenceKey(holidayId, day));
            moralePenalty = -2.0f;
            OnHolidaySkippedSeam?.Invoke(holidayId, day);
            return true;
        }

        public CelebrationRecord HoldCelebration(
            string holidayId,
            string scaleId = "small",
            int participantCount = 1,
            ISeededRng? rng = null)
        {
            if (!_holidays.TryGetValue(holidayId, out var holiday))
            {
                holiday = new HolidayDef
                {
                    HolidayId = holidayId,
                    Name = holidayId,
                    BaseMoraleBoost = 5.0f,
                    FoodCost = 2,
                    FuelCost = 1
                };
            }

            if (!_scales.TryGetValue(scaleId, out var scale))
            {
                scale = new CelebrationScaleDef
                {
                    ScaleId = scaleId,
                    CostMultiplier = 1.0f,
                    MoraleMultiplier = 1.0f
                };
            }

            var record = CreateCelebrationRecord(holidayId, holiday, scale, participantCount, rng);
            OnCelebrationHeldSeam?.Invoke(record);
            return record;
        }

        public float SkipHoliday(string holidayId)
        {
            float moralePenalty = -2.0f;
            _traditionStreak = 0;
            if (!string.IsNullOrWhiteSpace(holidayId))
                _skippedHolidayOccurrences.Add(OccurrenceKey(holidayId, _currentDay));
            OnHolidaySkippedSeam?.Invoke(holidayId, _currentDay);
            return moralePenalty;
        }

        private CelebrationRecord CreateCelebrationRecord(
            string holidayId,
            HolidayDef holiday,
            CelebrationScaleDef scale,
            int participantCount,
            ISeededRng? rng)
        {
            int foodSpent = (int)Math.Ceiling(holiday.FoodCost * scale.CostMultiplier);
            int fuelSpent = (int)Math.Ceiling(holiday.FuelCost * scale.CostMultiplier);
            float participantBonus = 1.0f + Math.Min(0.5f, Math.Max(0, participantCount - 1) * 0.05f);
            float streakBonus = Math.Min(5.0f, _traditionStreak * 0.5f);
            float moraleGain = (holiday.BaseMoraleBoost * scale.MoraleMultiplier * participantBonus) + streakBonus;
            if (rng != null)
                moraleGain += (float)(rng.NextDouble() * 2.0 - 1.0);
            moraleGain = (float)Math.Round(moraleGain, 1);
            _traditionStreak++;

            var record = new CelebrationRecord
            {
                CelebrationId = $"cel_{holidayId}_{_currentDay}_{_history.Count}",
                HolidayId = holidayId,
                Day = _currentDay,
                Scale = scale.ScaleId,
                MoraleGained = moraleGain,
                FoodSpent = foodSpent,
                FuelSpent = fuelSpent,
                IsMemorable = scale.IsMemorable,
                Summary = $"Observed {holiday.Name} ({scale.ScaleId}) on Day {_currentDay}: +{moraleGain} morale (Streak: {_traditionStreak})."
            };
            _history.Add(record);
            return record;
        }

        private static void AddCost(Dictionary<string, int> bill, string itemId, int amount)
        {
            if (amount <= 0) return;
            bill.TryGetValue(itemId, out int existing);
            bill[itemId] = checked(existing + amount);
        }

        private static string OccurrenceKey(string holidayId, int day)
        {
            int occurrence = Math.Max(1, day) - 1;
            int cycle = occurrence / 360;
            return $"{cycle}:{holidayId}";
        }

        public float CommemorateAnniversary(string typeId, string entityName, int day)
        {
            _currentDay = day;
            float boost = 5.0f;
            if (_anniversaries.TryGetValue(typeId, out var def))
                boost = def.BaseMoraleBoost;

            string recordText = $"Day {day}: Commemorated {def?.Name ?? typeId} for '{entityName}'.";
            _recordedAnniversaries.Add(recordText);
            OnAnniversaryCommemoratedSeam?.Invoke(typeId, entityName, boost);
            return boost;
        }

        public CelebrationSaveState CaptureState()
        {
            var heldOccurrences = new List<string>(_heldHolidayOccurrences);
            heldOccurrences.Sort(StringComparer.Ordinal);
            var skippedOccurrences = new List<string>(_skippedHolidayOccurrences);
            skippedOccurrences.Sort(StringComparer.Ordinal);
            return new CelebrationSaveState
            {
                SchemaVersion = 1,
                CurrentDay = _currentDay,
                TraditionStreak = _traditionStreak,
                CelebrationHistory = new List<CelebrationRecord>(_history),
                RecordedAnniversaries = new List<string>(_recordedAnniversaries),
                HeldHolidayOccurrences = heldOccurrences,
                SkippedHolidayOccurrences = skippedOccurrences
            };
        }

        public void RestoreState(CelebrationSaveState? state)
        {
            if (state == null)
                return;

            _currentDay = state.CurrentDay > 0 ? state.CurrentDay : 1;
            _traditionStreak = state.TraditionStreak;
            _history.Clear();
            if (state.CelebrationHistory != null)
                _history.AddRange(state.CelebrationHistory);

            _recordedAnniversaries.Clear();
            if (state.RecordedAnniversaries != null)
                _recordedAnniversaries.AddRange(state.RecordedAnniversaries);
            _heldHolidayOccurrences.Clear();
            if (state.HeldHolidayOccurrences != null)
                foreach (string occurrence in state.HeldHolidayOccurrences)
                    if (!string.IsNullOrWhiteSpace(occurrence)) _heldHolidayOccurrences.Add(occurrence);
            _skippedHolidayOccurrences.Clear();
            if (state.SkippedHolidayOccurrences != null)
                foreach (string occurrence in state.SkippedHolidayOccurrences)
                    if (!string.IsNullOrWhiteSpace(occurrence)) _skippedHolidayOccurrences.Add(occurrence);
        }

        private void LoadFallbackCatalog()
        {
            _holidays.Clear();
            _holidays["hol_new_year"] = new HolidayDef
            {
                HolidayId = "hol_new_year",
                Name = "New Year Dawn",
                TriggerDay = 1,
                BaseMoraleBoost = 8.0f,
                FoodCost = 4,
                FuelCost = 2
            };
            _holidays["hol_midsummer_day"] = new HolidayDef
            {
                HolidayId = "hol_midsummer_day",
                Name = "Midsummer Long Day",
                TriggerDay = 180,
                BaseMoraleBoost = 12.0f,
                FoodCost = 6,
                FuelCost = 2
            };

            _anniversaries.Clear();
            _anniversaries["anniv_founding"] = new AnniversaryTypeDef
            {
                TypeId = "anniv_founding",
                Name = "Shelter Founding Day",
                BaseMoraleBoost = 10.0f
            };
            _anniversaries["anniv_fallen"] = new AnniversaryTypeDef
            {
                TypeId = "anniv_fallen",
                Name = "Remembrance of the Fallen",
                BaseMoraleBoost = 3.0f
            };

            _scales.Clear();
            _scales["small"] = new CelebrationScaleDef
            {
                ScaleId = "small",
                CostMultiplier = 1.0f,
                MoraleMultiplier = 1.0f,
                IsMemorable = false
            };
            _scales["large"] = new CelebrationScaleDef
            {
                ScaleId = "large",
                CostMultiplier = 3.0f,
                MoraleMultiplier = 3.0f,
                IsMemorable = true
            };
        }
    }
}
