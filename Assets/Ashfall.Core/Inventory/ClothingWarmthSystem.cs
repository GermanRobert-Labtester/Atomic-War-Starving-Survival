// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    public enum ClothingLayer
    {
        Underwear = 0,
        Middle = 1,
        Outer = 2,
        Accessory = 3
    }

    [Serializable]
    public sealed class ClothingItemProfile
    {
        public string item_id = string.Empty;
        public string display_name = string.Empty;
        public int warmth_value = 10;
        public int cold_mitigation_bp = 1500; // basis points (1500 = 15%)
        public ClothingLayer layer = ClothingLayer.Outer;
        public bool is_waterproof;
    }

    [Serializable]
    public sealed class EquippedClothingInstance
    {
        public string item_id = string.Empty;
        public float condition = 1.0f; // 0.0 to 1.0
    }

    [Serializable]
    public sealed class SurvivorClothingRecord
    {
        public string survivor_id = string.Empty;
        public float wetness; // 0.0 to 1.0 (0% to 100% soaked)
        public List<EquippedClothingInstance> equipped = new List<EquippedClothingInstance>();
    }

    [Serializable]
    public sealed class ClothingWarmthSaveState
    {
        public int schema_version = 1;
        public Dictionary<string, SurvivorClothingRecord> survivors =
            new Dictionary<string, SurvivorClothingRecord>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Plan 142 / C2[29] / DEC-109: Clothing & Warmth Gear Progression System.
    /// Governs equipped clothing insulation, multi-layer cold protection,
    /// condition degradation, wetness thermal penalties, and cold mitigation for NeedsSystem.
    /// Completely engine-free pure domain logic.
    /// </summary>
    public sealed class ClothingWarmthSystem
    {
        public const string SystemId = "clothing_warmth";
        public const float MaxColdMitigation = 0.85f; // Hard 85% reduction cap

        private readonly Dictionary<string, ClothingItemProfile> _profiles =
            new Dictionary<string, ClothingItemProfile>(StringComparer.OrdinalIgnoreCase);

        private readonly ClothingWarmthSaveState _state = new ClothingWarmthSaveState();

        public event Action<string, string>? OnClothingEquipped; // survivorId, itemId
        public event Action<string, string>? OnClothingUnequipped; // survivorId, itemId
        public event Action<string, float>? OnWetnessChanged; // survivorId, newWetness

        public IReadOnlyDictionary<string, ClothingItemProfile> Profiles => _profiles;
        public ClothingWarmthSaveState State => _state;

        public ClothingWarmthSystem()
        {
            RegisterDefaultProfiles();
        }

        public void RegisterProfile(ClothingItemProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.item_id)) return;
            _profiles[profile.item_id] = profile;
        }

        private void RegisterDefaultProfiles()
        {
            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_ragged_coat",
                display_name = "Ragged Scavenger Coat",
                warmth_value = 5,
                cold_mitigation_bp = 1000,
                layer = ClothingLayer.Outer,
                is_waterproof = false
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_wool_scarf",
                display_name = "Knit Wool Scarf",
                warmth_value = 5,
                cold_mitigation_bp = 600,
                layer = ClothingLayer.Accessory,
                is_waterproof = false
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_thermal_underwear",
                display_name = "Thermal Flannel Underwear",
                warmth_value = 10,
                cold_mitigation_bp = 1500,
                layer = ClothingLayer.Underwear,
                is_waterproof = false
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_winter_coat",
                display_name = "Insulated Winter Parka",
                warmth_value = 20,
                cold_mitigation_bp = 2500,
                layer = ClothingLayer.Outer,
                is_waterproof = false
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_fur_boots",
                display_name = "Fur-Lined Expedition Boots",
                warmth_value = 10,
                cold_mitigation_bp = 1200,
                layer = ClothingLayer.Accessory,
                is_waterproof = false
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_hazmat_cold_suit",
                display_name = "Hazmat Sealed Cold-Suit",
                warmth_value = 30,
                cold_mitigation_bp = 4000,
                layer = ClothingLayer.Outer,
                is_waterproof = true
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_arctic_survival_suit",
                display_name = "Full Arctic Survival Exosuit",
                warmth_value = 45,
                cold_mitigation_bp = 6000,
                layer = ClothingLayer.Outer,
                is_waterproof = true
            });

            RegisterProfile(new ClothingItemProfile
            {
                item_id = "item_thermal_balaclava",
                display_name = "Windproof Thermal Balaclava",
                warmth_value = 12,
                cold_mitigation_bp = 1200,
                layer = ClothingLayer.Accessory,
                is_waterproof = true
            });
        }

        private SurvivorClothingRecord GetOrCreateRecord(string survivorId)
        {
            if (!_state.survivors.TryGetValue(survivorId, out var rec))
            {
                rec = new SurvivorClothingRecord
                {
                    survivor_id = survivorId,
                    wetness = 0f,
                    equipped = new List<EquippedClothingInstance>()
                };
                _state.survivors[survivorId] = rec;
            }
            return rec;
        }

        // ── Equipment Management ──────────────────────────────────

        public bool EquipClothing(string survivorId, string itemId, float condition = 1.0f)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return false;
            if (!_profiles.TryGetValue(itemId, out var profile)) return false;

            var rec = GetOrCreateRecord(survivorId);

            // If same layer already has an item, replace it
            for (int i = rec.equipped.Count - 1; i >= 0; i--)
            {
                var eq = rec.equipped[i];
                if (_profiles.TryGetValue(eq.item_id, out var eqProf) && eqProf.layer == profile.layer)
                {
                    rec.equipped.RemoveAt(i);
                    OnClothingUnequipped?.Invoke(survivorId, eq.item_id);
                }
            }

            rec.equipped.Add(new EquippedClothingInstance
            {
                item_id = itemId,
                condition = Math.Max(0.1f, Math.Min(1.0f, condition))
            });

            OnClothingEquipped?.Invoke(survivorId, itemId);
            return true;
        }

        public bool UnequipClothing(string survivorId, string itemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return false;
            if (!_state.survivors.TryGetValue(survivorId, out var rec)) return false;

            int idx = rec.equipped.FindIndex(e => string.Equals(e.item_id, itemId, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                rec.equipped.RemoveAt(idx);
                OnClothingUnequipped?.Invoke(survivorId, itemId);
                return true;
            }
            return false;
        }

        public IReadOnlyList<EquippedClothingInstance> GetEquipped(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return Array.Empty<EquippedClothingInstance>();
            return _state.survivors.TryGetValue(survivorId, out var rec)
                ? rec.equipped
                : (IReadOnlyList<EquippedClothingInstance>)Array.Empty<EquippedClothingInstance>();
        }

        // ── Wetness and Environmental Exposure ────────────────────

        public void ApplyWetness(string survivorId, float delta)
        {
            if (string.IsNullOrEmpty(survivorId) || delta <= 0f) return;
            var rec = GetOrCreateRecord(survivorId);

            // Waterproof outer gear reduces wetness absorption by 80%
            bool hasWaterproofOuter = false;
            foreach (var eq in rec.equipped)
            {
                if (_profiles.TryGetValue(eq.item_id, out var p) && p.layer == ClothingLayer.Outer && p.is_waterproof)
                {
                    hasWaterproofOuter = true;
                    break;
                }
            }

            float effectiveDelta = hasWaterproofOuter ? delta * 0.2f : delta;
            float prev = rec.wetness;
            rec.wetness = Math.Max(0f, Math.Min(1.0f, rec.wetness + effectiveDelta));

            if (Math.Abs(prev - rec.wetness) > 0.001f)
            {
                OnWetnessChanged?.Invoke(survivorId, rec.wetness);
            }
        }

        public void DryClothing(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId) || hours <= 0f) return;
            if (!_state.survivors.TryGetValue(survivorId, out var rec)) return;
            if (rec.wetness <= 0f) return;

            float dryRatePerHour = 0.35f; // Dries completely in ~3 hours near heat
            float prev = rec.wetness;
            rec.wetness = Math.Max(0f, rec.wetness - dryRatePerHour * hours);

            if (Math.Abs(prev - rec.wetness) > 0.001f)
            {
                OnWetnessChanged?.Invoke(survivorId, rec.wetness);
            }
        }

        public void DegradeCondition(string survivorId, float wearHours)
        {
            if (string.IsNullOrEmpty(survivorId) || wearHours <= 0f) return;
            if (!_state.survivors.TryGetValue(survivorId, out var rec)) return;

            float wearPer24Hours = 0.05f; // 5% wear per full day of expedition/wear
            float delta = (wearPer24Hours * wearHours) / 24f;

            foreach (var eq in rec.equipped)
            {
                eq.condition = Math.Max(0.1f, eq.condition - delta);
            }
        }

        // ── Cold Mitigation Calculation ───────────────────────────

        /// <summary>
        /// Calculates the net fraction [0.0..0.85] of cold loss reduction provided by equipped clothing.
        /// Consumed directly by NeedsSystem.ClothingWarmthReductionProvider.
        /// </summary>
        public float CalculateColdLossReduction(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return 0f;
            if (!_state.survivors.TryGetValue(survivorId, out var rec) || rec.equipped.Count == 0)
                return 0f;

            int totalMitigationBp = 0;

            foreach (var eq in rec.equipped)
            {
                if (!_profiles.TryGetValue(eq.item_id, out var prof)) continue;

                // Base mitigation scaled by condition
                float itemMitigation = prof.cold_mitigation_bp * eq.condition;

                // Wetness penalty: non-waterproof clothes lose up to 50% effectiveness when soaked
                if (!prof.is_waterproof && rec.wetness > 0f)
                {
                    itemMitigation *= (1.0f - (rec.wetness * 0.5f));
                }

                totalMitigationBp += (int)itemMitigation;
            }

            float rawFraction = totalMitigationBp / 10000f;
            return Math.Max(0f, Math.Min(MaxColdMitigation, rawFraction));
        }

        // ── Save / Load ───────────────────────────────────────────

        public ClothingWarmthSaveState CaptureState()
        {
            var copy = new ClothingWarmthSaveState
            {
                schema_version = _state.schema_version,
                survivors = new Dictionary<string, SurvivorClothingRecord>(StringComparer.OrdinalIgnoreCase)
            };

            foreach (var kvp in _state.survivors)
            {
                var rec = new SurvivorClothingRecord
                {
                    survivor_id = kvp.Value.survivor_id,
                    wetness = kvp.Value.wetness,
                    equipped = new List<EquippedClothingInstance>()
                };
                foreach (var eq in kvp.Value.equipped)
                {
                    rec.equipped.Add(new EquippedClothingInstance
                    {
                        item_id = eq.item_id,
                        condition = eq.condition
                    });
                }
                copy.survivors[kvp.Key] = rec;
            }
            return copy;
        }

        public void RestoreState(ClothingWarmthSaveState? saved)
        {
            _state.survivors.Clear();
            if (saved == null) return;

            _state.schema_version = saved.schema_version;
            if (saved.survivors != null)
            {
                foreach (var kvp in saved.survivors)
                {
                    var rec = new SurvivorClothingRecord
                    {
                        survivor_id = kvp.Value.survivor_id,
                        wetness = kvp.Value.wetness,
                        equipped = new List<EquippedClothingInstance>()
                    };
                    if (kvp.Value.equipped != null)
                    {
                        foreach (var eq in kvp.Value.equipped)
                        {
                            rec.equipped.Add(new EquippedClothingInstance
                            {
                                item_id = eq.item_id,
                                condition = eq.condition
                            });
                        }
                    }
                    _state.survivors[kvp.Key] = rec;
                }
            }
        }
    }
}
