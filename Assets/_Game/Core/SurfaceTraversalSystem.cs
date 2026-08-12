using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — The Logistics of the Ash. Surface traversal is not fast travel.
    /// It is a grueling, physics-based endurance event. The ash is pulverized concrete,
    /// glass, and topsoil. It shifts. It swallows. Weight determines everything.
    /// Integrates with ExpeditionSystem to modify stamina drain, travel time, and
    /// collapse risk based on ash conditions, gear, and weather.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class SurfaceTraversalSystem
    {
        // ── Ash depth constants ───────────────────────────────────────
        public const float BaseAshDepthStreets = 0.5f;    // meters
        public const float BaseAshDepthSwamp = 2.0f;      // Biome_AshSwamp
        public const float SinkingWeightThreshold = 15f;  // kg without snowshoes
        public const float SinkingFatigueMultiplier = 3f; // 300% fatigue decay
        public const float SinkingTimeMultiplier = 3f;    // 4hr trip → 12hr caloric burn

        // ── Collapse thresholds ───────────────────────────────────────
        public const float CollapseStaminaThreshold = 0f;  // Stamina hits 0 → collapse
        public const float LootDropOnCollapse = 0.50f;     // Drop 50% loot to crawl back

        // ── Sled constants ────────────────────────────────────────────
        public const string SledItemId = "sled_improvised";
        public const float SledCarryCapacity = 60f;        // kg
        public const float SledTrailDurationHours = 48f;   // Warlords can track for 48h

        // ── Beast of burden constants ─────────────────────────────────
        public const string AshGoatItemId = "ash_goat";
        public const string PackMuleItemId = "pack_mule";
        public const float AshGoatCarryCapacity = 20f;     // kg
        public const float AshGoatFoodPerExpedition = 0.5f; // kg roots/scrap_wood
        public const float AshGoatPanicChanceGlassStorm = 0.60f;

        // ── Tether constants ──────────────────────────────────────────
        public const string TetherItemId = "tether_rope_5m";
        public const float WhiteoutVisibilityMeters = 0.5f;
        public const float WhiteoutDeliriumMoraleThreshold = 20f;

        // ── Whiteout weather id ───────────────────────────────────────
        public const string WeatherWhiteoutId = "weather_whiteout";
        public const int WhiteoutStartDay = 60;

        // ── Affliction ids ────────────────────────────────────────────
        public const string Affliction_AshDelirium = "affliction_ash_delirium";
        public const string Affliction_AshExhaustion = "affliction_ash_exhaustion";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnSurvivorSinks;              // survivorId
        public event Action<string> OnExpeditionCollapse;         // expeditionId
        public event Action<string, float> OnSledTrailCreated;    // survivorId, hours
        public event Action<string> OnAshGoatPanicked;            // survivorId
        public event Action<string> OnWhiteoutDelirium;           // survivorId
        public event Action<string, string> OnTetherBroken;       // survivorId1, survivorId2
        public event Action<string> OnSurvivorLostInWhiteout;     // survivorId

        private readonly System.Random _rng;
        private float _currentAshDepth;
        private bool _isWhiteout;
        private readonly List<SledTrail> _activeTrails = new List<SledTrail>();

        public float CurrentAshDepth => _currentAshDepth;
        public bool IsWhiteout => _isWhiteout;
        public IReadOnlyList<SledTrail> ActiveTrails => _activeTrails;

        public SurfaceTraversalSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(6000);
        }

        /// <summary>Set current ash depth (updated by AshDriftSystem).</summary>
        public void SetAshDepth(float depth) => _currentAshDepth = Mathf.Max(0f, depth);

        /// <summary>Set whiteout state (updated by WeatherSystem).</summary>
        public void SetWhiteout(bool active) => _isWhiteout = active;

        // ── Traversal Toll Calculation ────────────────────────────────

        /// <summary>
        /// Calculate the traversal toll for an expedition. Returns fatigue
        /// multiplier, time multiplier, and whether the survivor sinks.
        /// </summary>
        public TraversalToll CalculateToll(
            float carriedWeightKg,
            bool hasSnowshoes,
            bool hasSled,
            bool isAshSwamp,
            bool hasTether,
            float survivorMorale)
        {
            var toll = new TraversalToll
            {
                FatigueMultiplier = 1f,
                TimeMultiplier = 1f,
                IsSinking = false,
                IsWhiteoutDelirium = false
            };

            float ashDepth = isAshSwamp ? BaseAshDepthSwamp : _currentAshDepth;

            // Sinking check: >15kg without snowshoes
            if (carriedWeightKg > SinkingWeightThreshold && !hasSnowshoes && !hasSled)
            {
                toll.IsSinking = true;
                toll.FatigueMultiplier = SinkingFatigueMultiplier;
                toll.TimeMultiplier = SinkingTimeMultiplier;
                OnSurvivorSinks?.Invoke("expedition_survivor");
            }

            // Sled allows heavy carry without sinking, but leaves trail
            if (hasSled && carriedWeightKg <= SledCarryCapacity)
            {
                toll.IsSinking = false;
                toll.FatigueMultiplier = 1.2f; // Slight drag penalty
                toll.TimeMultiplier = 1.1f;
                toll.LeavesSledTrail = true;
            }

            // Whiteout: visibility drops, delirium risk
            if (_isWhiteout)
            {
                toll.TimeMultiplier *= 1.5f;
                toll.FatigueMultiplier *= 1.3f;
                toll.VisibilityMeters = WhiteoutVisibilityMeters;

                if (survivorMorale < WhiteoutDeliriumMoraleThreshold)
                {
                    toll.IsWhiteoutDelirium = true;
                    OnWhiteoutDelirium?.Invoke("expedition_survivor");
                }
            }

            return toll;
        }

        // ── Sled Trail Tracking ───────────────────────────────────────

        /// <summary>Record a sled trail for warlord tracking.</summary>
        public void RecordSledTrail(string survivorId, string locationId)
        {
            _activeTrails.Add(new SledTrail
            {
                SurvivorId = survivorId,
                LocationId = locationId,
                HoursRemaining = SledTrailDurationHours
            });
            OnSledTrailCreated?.Invoke(survivorId, SledTrailDurationHours);
        }

        /// <summary>Check if any active sled trail leads to a location.</summary>
        public bool HasTrailTo(string locationId)
        {
            for (int i = 0; i < _activeTrails.Count; i++)
                if (_activeTrails[i].LocationId == locationId && _activeTrails[i].HoursRemaining > 0f)
                    return true;
            return false;
        }

        /// <summary>
        /// Process an expedition collapse. Survivor drops 50% loot and crawls back.
        /// </summary>
        public ExpeditionCollapseResult ProcessCollapse(ExpeditionState expedition)
        {
            if (expedition == null) return null;

            var result = new ExpeditionCollapseResult
            {
                ExpeditionId = expedition.ExpeditionId,
                SurvivorId = expedition.SurvivorId,
                LootDropped = expedition.DropLoot(LootDropOnCollapse),
                HoursDelayed = expedition.TotalDistanceTicks - expedition.TravelTicksCompleted
            };

            OnExpeditionCollapse?.Invoke(expedition.ExpeditionId);
            return result;
        }

        /// <summary>
        /// Process whiteout tether break. If one survivor's morale is too low,
        /// they untether and wander into the grey.
        /// </summary>
        public bool ProcessTetherBreak(string survivor1Id, string survivor2Id,
            float survivor1Morale, float survivor2Morale)
        {
            // The one with lower morale breaks free
            string wanderer;
            if (survivor1Morale < survivor2Morale)
                wanderer = survivor1Id;
            else
                wanderer = survivor2Id;

            if (_rng.NextDouble() < 0.30f) // 30% chance of permanent loss
            {
                OnSurvivorLostInWhiteout?.Invoke(wanderer);
                OnTetherBroken?.Invoke(survivor1Id, survivor2Id);
                return true; // Lost
            }
            return false; // Found in time
        }

        /// <summary>
        /// Process ash goat panic during glass storm. Goat may flee with loot.
        /// </summary>
        public bool ProcessAshGoatPanic(string ownerId)
        {
            if (_rng.NextDouble() < AshGoatPanicChanceGlassStorm)
            {
                OnAshGoatPanicked?.Invoke(ownerId);
                return true; // Goat fled with loot
            }
            return false;
        }

        // ── Tick ──────────────────────────────────────────────────────

        /// <summary>Advance trail timers. Called per game-hour.</summary>
        public void Tick(float gameHours)
        {
            for (int i = _activeTrails.Count - 1; i >= 0; i--)
            {
                _activeTrails[i].HoursRemaining -= gameHours;
                if (_activeTrails[i].HoursRemaining <= 0f)
                    _activeTrails.RemoveAt(i);
            }
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SurfaceTraversalSave CaptureState()
        {
            var trails = new SledTrailSave[_activeTrails.Count];
            for (int i = 0; i < _activeTrails.Count; i++)
            {
                trails[i] = new SledTrailSave
                {
                    SurvivorId = _activeTrails[i].SurvivorId,
                    LocationId = _activeTrails[i].LocationId,
                    HoursRemaining = _activeTrails[i].HoursRemaining
                };
            }
            return new SurfaceTraversalSave
            {
                CurrentAshDepth = _currentAshDepth,
                IsWhiteout = _isWhiteout,
                ActiveTrails = trails
            };
        }

        public void RestoreState(SurfaceTraversalSave save)
        {
            _activeTrails.Clear();
            _currentAshDepth = 0f;
            _isWhiteout = false;
            if (save == null) return;
            _currentAshDepth = save.CurrentAshDepth;
            _isWhiteout = save.IsWhiteout;
            if (save.ActiveTrails != null)
            {
                for (int i = 0; i < save.ActiveTrails.Length; i++)
                {
                    var t = save.ActiveTrails[i];
                    if (t == null) continue;
                    _activeTrails.Add(new SledTrail
                    {
                        SurvivorId = t.SurvivorId,
                        LocationId = t.LocationId,
                        HoursRemaining = t.HoursRemaining
                    });
                }
            }
        }
    }

    [Serializable]
    public class TraversalToll
    {
        public float FatigueMultiplier = 1f;
        public float TimeMultiplier = 1f;
        public bool IsSinking;
        public bool IsWhiteoutDelirium;
        public bool LeavesSledTrail;
        public float VisibilityMeters = 100f;
    }

    [Serializable]
    public class ExpeditionCollapseResult
    {
        public string ExpeditionId;
        public string SurvivorId;
        public List<ItemDefinition> LootDropped;
        public int HoursDelayed;
    }

    public class SledTrail
    {
        public string SurvivorId;
        public string LocationId;
        public float HoursRemaining;
    }

    [Serializable]
    public class SurfaceTraversalSave
    {
        public float CurrentAshDepth;
        public bool IsWhiteout;
        public SledTrailSave[] ActiveTrails;
    }

    [Serializable]
    public class SledTrailSave
    {
        public string SurvivorId;
        public string LocationId;
        public float HoursRemaining;
    }
}
