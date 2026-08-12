using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion III — Mega-Location: The Drowned Town.
    /// A hydroelectric dam whose reservoir froze solid after the EMP.
    /// The submerged town of Oakhaven protrudes from the ice — church steeple,
    /// school roof, supermarket skylight. Survivors walk on thin ice near the
    /// spillgates and lower someone through the skylight to scavenge pristine
    /// loot from the black, irradiated water below.
    /// </summary>
    public class Location_HydroelectricDam
    {
        // ── Identity ──────────────────────────────────────────────────
        public const string LocationId = "location_hydroelectric_dam";
        public const string DisplayName = "The Drowned Town";
        public const int TravelHours = 14;
        public const int DangerLevel = 8;
        public const float BaseRadsAmbient = 15f;   // mSv/h on the ice
        public const float BaseRadsWater = 150f;     // mSv/h in the water

        // ── Required gear ─────────────────────────────────────────────
        public const string RequiredGear_Snowshoes = "snowshoes";
        public const string RequiredGear_Rope = "rope_2m_of_2m";

        // ── Unique loot ids ───────────────────────────────────────────
        public const string Loot_TurbineBearing = "hydroelectric_turbine_bearing";
        public const string Loot_SafeDepositBox = "submerged_safe_deposit_box";

        // ── Actions ───────────────────────────────────────────────────
        public const string Action_Crawlspace = "action_crawlspace";
        public const string Action_WalkOnIce = "action_walk_on_ice";
        public const string Action_SubmergeSearch = "action_submerge_search";

        // ── Hazard constants ──────────────────────────────────────────
        public const float ThinIceBreakChance = 0.15f;       // per step near spillgates
        public const float SubmergeDeathMinutes = 4f;         // minutes to die if fallen in
        public const float FrostbiteRate = 25f;               // health/min in freezing water
        public const float RadiationDoseWater = 50f;          // mSv per minute submerged

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnIceBreak;               // survivorId
        public event Action<string> OnSurvivorSubmerged;      // survivorId
        public event Action<string> OnLootRecovered;          // itemId
        public event Action OnTurbineBearingFound;
        public event Action OnSafeDepositBoxFound;

        private readonly System.Random _rng;
        private bool _turbineBearingRecovered;
        private bool _safeDepositBoxRecovered;
        private bool _skylightEntered;
        private readonly HashSet<string> _searchedAreas = new HashSet<string>();

        public bool IsTurbineBearingRecovered => _turbineBearingRecovered;
        public bool IsSafeDepositBoxRecovered => _safeDepositBoxRecovered;

        public Location_HydroelectricDam(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(7777);
        }

        /// <summary>
        /// Attempt to walk on the ice. Near spillgates, ice may break.
        /// Returns true if the survivor made it across safely.
        /// </summary>
        public bool AttemptIceTraversal(string survivorId, bool nearSpillgate)
        {
            if (nearSpillgate && _rng.NextDouble() < ThinIceBreakChance)
            {
                OnIceBreak?.Invoke(survivorId);
                OnSurvivorSubmerged?.Invoke(survivorId);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lower a survivor through the drowned supermarket skylight.
        /// Requires rope. Returns items found in the submerged store.
        /// </summary>
        public List<string> EnterThroughSkylight(string survivorId, bool hasRope)
        {
            if (!hasRope) return null;
            _skylightEntered = true;

            var loot = new List<string>();

            // The supermarket is sealed in the dark — loot is pristine
            if (!_searchedAreas.Contains("supermarket") && _rng.NextDouble() < 0.70f)
            {
                _searchedAreas.Add("supermarket");
                loot.Add("canned_food_pristine");
                loot.Add("medical_supplies_sealed");
            }

            return loot;
        }

        /// <summary>
        /// Search submerged buildings through the ice. Very dangerous.
        /// Returns unique loot if found.
        /// </summary>
        public List<string> SearchSubmerged(string survivorId, float diveMinutes)
        {
            var loot = new List<string>();

            // Turbine bearing — required for Project_DeepWell
            if (!_turbineBearingRecovered && _searchedAreas.Count >= 2
                && _rng.NextDouble() < 0.40f)
            {
                _turbineBearingRecovered = true;
                loot.Add(Loot_TurbineBearing);
                OnTurbineBearingFound?.Invoke();
                OnLootRecovered?.Invoke(Loot_TurbineBearing);
            }

            // Safe deposit box — requires lockpick + 2 hours underwater
            if (!_safeDepositBoxRecovered && diveMinutes >= 120f
                && _rng.NextDouble() < 0.30f)
            {
                _safeDepositBoxRecovered = true;
                loot.Add(Loot_SafeDepositBox);
                OnSafeDepositBoxFound?.Invoke();
                OnLootRecovered?.Invoke(Loot_SafeDepositBox);
            }

            return loot;
        }

        /// <summary>
        /// Apply radiation and cold damage to a submerged survivor.
        /// Called per-minute while underwater.
        /// </summary>
        public SubmergeDamage GetSubmergeDamage(float minutesSubmerged)
        {
            return new SubmergeDamage
            {
                HealthLoss = FrostbiteRate * minutesSubmerged,
                RadiationDose = RadiationDoseWater * minutesSubmerged,
                IsLethal = minutesSubmerged >= SubmergeDeathMinutes
            };
        }

        // ── Save / Load ───────────────────────────────────────────────

        public HydroDamSave CaptureState()
        {
            var areas = new string[_searchedAreas.Count];
            _searchedAreas.CopyTo(areas);
            return new HydroDamSave
            {
                TurbineBearingRecovered = _turbineBearingRecovered,
                SafeDepositBoxRecovered = _safeDepositBoxRecovered,
                SkylightEntered = _skylightEntered,
                SearchedAreas = areas
            };
        }

        public void RestoreState(HydroDamSave save)
        {
            _searchedAreas.Clear();
            _turbineBearingRecovered = false;
            _safeDepositBoxRecovered = false;
            _skylightEntered = false;
            if (save == null) return;
            _turbineBearingRecovered = save.TurbineBearingRecovered;
            _safeDepositBoxRecovered = save.SafeDepositBoxRecovered;
            _skylightEntered = save.SkylightEntered;
            if (save.SearchedAreas != null)
                for (int i = 0; i < save.SearchedAreas.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedAreas[i]))
                        _searchedAreas.Add(save.SearchedAreas[i]);
        }
    }

    [Serializable]
    public struct SubmergeDamage
    {
        public float HealthLoss;
        public float RadiationDose;
        public bool IsLethal;
    }

    [Serializable]
    public class HydroDamSave
    {
        public bool TurbineBearingRecovered;
        public bool SafeDepositBoxRecovered;
        public bool SkylightEntered;
        public string[] SearchedAreas;
    }
}
