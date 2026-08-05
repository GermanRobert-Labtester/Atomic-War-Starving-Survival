using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// House-to-Bunker Transition Engine (Prompt #79). The player starts in a
    /// normal basement on Day 1 of the Civil War. During Day 1-29, artillery
    /// randomly damages the house above. On Day 30, the house collapses. The
    /// debris adds shielding but blocks the hatch until cleared. The overworld
    /// structure evolves from house → collapsed rubble → reinforced bunker.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class HouseToBunkerSystem
    {
        /// <summary>Hours of debris that collapse onto the hatch.</summary>
        public const float DefaultCollapseDebrisHours = 24f;

        /// <summary>Shielding bonus from collapsed debris.</summary>
        public const float DefaultDebrisShieldingBonus = 0.2f;

        /// <summary>Artillery damage per strike to the house (Day 1-29).</summary>
        public const float ArtilleryDamagePerStrike = 15f;

        /// <summary>Minimum house durability before collapse auto-triggers.</summary>
        public const float MinHouseDurabilityForCollapse = 5f;

        /// <summary>Chance per day of artillery strike during Civil War.</summary>
        public const float DailyArtilleryChance = 0.35f;

        /// <summary>Event id for house collapse.</summary>
        public const string HouseCollapseEventId = "house_collapse";

        // -- State --
        private float _houseDurability = 100f;
        private bool _houseDestroyed;
        private bool _debrisCleared;
        private float _debrisClearHoursRemaining;
        private float _debrisClearHoursTotal;
        private float _debrisShieldingActive;
        private float _inherentShielding;

        private DebrisType _debrisType = DebrisType.WoodRubble;
        private readonly List<string> _activeAnomalies = new List<string>();
        private readonly System.Random _rng;

        // Layout reference.
        private Shelter.ShelterMapSO _layout;

        // -- Public state --
        public float HouseDurability => _houseDurability;
        public bool HouseDestroyed => _houseDestroyed;
        public bool DebrisCleared => _debrisCleared;
        public bool HatchBlocked => _houseDestroyed && !_debrisCleared;
        public float DebrisClearProgress => _debrisClearHoursTotal > 0f
            ? 1f - (_debrisClearHoursRemaining / _debrisClearHoursTotal) : 0f;
        public float DebrisShieldingActive => _debrisShieldingActive;
        public float InherentShielding => _inherentShielding;
        public DebrisType DebrisType => _debrisType;
        public IReadOnlyList<string> Anomalies => _activeAnomalies;

        public bool HasAnomaly(string anomaly) => _activeAnomalies.Contains(anomaly);

        // -- Events --
        public event Action<float> OnHouseDamaged;       // damageAmount
        public event Action OnHouseCollapsed;
        public event Action OnDebrisCleared;
        public event Action<float> OnShieldingChanged;

        public HouseToBunkerSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(79);
        }

        /// <summary>
        /// Initialize from a shelter layout. Sets up house durability, inherent
        /// shielding, anomalies, and debris type.
        /// </summary>
        public void InitializeFromLayout(Shelter.ShelterMapSO layout)
        {
            _layout = layout;
            if (layout == null) return;

            _houseDurability = layout.startingHouseDurability;
            _inherentShielding = layout.inherentShielding;
            _debrisType = layout.debrisType;
            _debrisShieldingActive = 0f;
            _houseDestroyed = false;
            _debrisCleared = true; // Not collapsed yet, so not blocked.

            _activeAnomalies.Clear();
            if (layout.anomalies != null)
                _activeAnomalies.AddRange(layout.anomalies);
        }

        /// <summary>
        /// Apply artillery damage to the house. Called during Day 1-29 Civil War.
        /// </summary>
        public bool ApplyArtilleryDamage()
        {
            if (_houseDestroyed) return false;
            if (_rng.NextDouble() >= DailyArtilleryChance) return false;

            float damage = ArtilleryDamagePerStrike * (0.5f + (float)_rng.NextDouble());
            _houseDurability = Mathf.Max(0f, _houseDurability - damage);
            OnHouseDamaged?.Invoke(damage);

            // Auto-collapse if durability is critically low.
            if (_houseDurability <= MinHouseDurabilityForCollapse && !_houseDestroyed)
            {
                CollapseHouse();
            }

            return true;
        }

        /// <summary>
        /// Force the house to collapse (Day 30 Flashpoint or scripted).
        /// </summary>
        public void CollapseHouse()
        {
            if (_houseDestroyed) return;
            _houseDestroyed = true;
            _houseDurability = 0f;

            // Calculate debris.
            float baseDebris = DefaultCollapseDebrisHours;
            if (_layout != null)
            {
                baseDebris = _layout.startingDebrisHours;
                _debrisShieldingActive = Mathf.Clamp01(
                    _inherentShielding + _layout.debrisShieldingBonus);
            }
            else
            {
                _debrisShieldingActive = Mathf.Clamp01(
                    _inherentShielding + DefaultDebrisShieldingBonus);
            }

            _debrisClearHoursRemaining = baseDebris;
            _debrisClearHoursTotal = baseDebris;
            _debrisCleared = false;

            OnHouseCollapsed?.Invoke();
            OnShieldingChanged?.Invoke(_debrisShieldingActive);
        }

        /// <summary>
        /// Clear debris from the hatch (work-hours). Returns true when fully cleared.
        /// </summary>
        public bool ClearDebris(float workHours)
        {
            if (!_houseDestroyed || _debrisCleared) return false;
            if (workHours <= 0f) return false;

            _debrisClearHoursRemaining = Mathf.Max(0f, _debrisClearHoursRemaining - workHours);
            if (_debrisClearHoursRemaining <= 0f)
            {
                _debrisCleared = true;
                // Shielding from debris remains (it's packed around the hatch).
                OnDebrisCleared?.Invoke();
            }
            return _debrisCleared;
        }

        /// <summary>
        /// Effective radiation shielding including debris bonus.
        /// </summary>
        public float GetEffectiveShielding()
        {
            return _houseDestroyed ? _debrisShieldingActive : _inherentShielding;
        }

        /// <summary>
        /// Whether expeditions are possible (hatch not blocked by uncleared debris).
        /// </summary>
        public bool CanLeaveShelter => !HatchBlocked;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public HouseToBunkerSave CaptureState()
        {
            return new HouseToBunkerSave
            {
                HouseDurability = _houseDurability,
                HouseDestroyed = _houseDestroyed,
                DebrisCleared = _debrisCleared,
                DebrisClearHoursRemaining = _debrisClearHoursRemaining,
                DebrisClearHoursTotal = _debrisClearHoursTotal,
                DebrisShieldingActive = _debrisShieldingActive,
                InherentShielding = _inherentShielding,
                DebrisType = (int)_debrisType,
                ActiveAnomalies = _activeAnomalies.ToArray()
            };
        }

        public void RestoreState(HouseToBunkerSave save)
        {
            if (save == null)
            {
                _houseDurability = 100f;
                _houseDestroyed = false;
                _debrisCleared = true;
                _debrisClearHoursRemaining = 0f;
                _debrisClearHoursTotal = 0f;
                _debrisShieldingActive = 0f;
                _inherentShielding = 0f;
                _activeAnomalies.Clear();
                return;
            }
            _houseDurability = save.HouseDurability;
            _houseDestroyed = save.HouseDestroyed;
            _debrisCleared = save.DebrisCleared;
            _debrisClearHoursRemaining = save.DebrisClearHoursRemaining;
            _debrisClearHoursTotal = save.DebrisClearHoursTotal;
            _debrisShieldingActive = save.DebrisShieldingActive;
            _inherentShielding = save.InherentShielding;
            _debrisType = (DebrisType)save.DebrisType;
            _activeAnomalies.Clear();
            if (save.ActiveAnomalies != null)
                _activeAnomalies.AddRange(save.ActiveAnomalies);
        }
    }

    [Serializable]
    public class HouseToBunkerSave
    {
        public float HouseDurability = 100f;
        public bool HouseDestroyed;
        public bool DebrisCleared = true;
        public float DebrisClearHoursRemaining;
        public float DebrisClearHoursTotal;
        public float DebrisShieldingActive;
        public float InherentShielding;
        public int DebrisType;
        public string[] ActiveAnomalies;
    }
}
