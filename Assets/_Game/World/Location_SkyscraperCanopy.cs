using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion III — Mega-Location: The Roofs of Tessarat.
    /// The ash in the city center is 30 feet deep. Streets are gone.
    /// Survivors walk across skyscraper roofs using climbing gear and planks
    /// to bridge alleys. The "ground" is now the 4th floor. High-altitude
    /// winds carry glass shards from blown-out windows.
    /// </summary>
    public class Location_SkyscraperCanopy
    {
        public const string LocationId = "location_skyscraper_canopy";
        public const string DisplayName = "The Roofs of Tessarat";
        public const int TravelHours = 8;
        public const int DangerLevel = 6;
        public const float BaseRads = 25f; // mSv/h

        // ── Required gear ─────────────────────────────────────────────
        public const string RequiredGear_ClimbingGear = "climbing_gear";
        public const string RequiredGear_ProtectiveGoggles = "protective_goggles";
        public const string RequiredGear_WinterCoat = "winter_coat";

        // ── Unique loot ───────────────────────────────────────────────
        public const string Loot_ServerRackBlade = "server_rack_blade";
        public const string Loot_ExecutiveKeycard = "executive_bunker_keycard";
        public const string Loot_ParachuteSilk = "parachute_silk";

        // ── Hazard constants ──────────────────────────────────────────
        public const float GlassStormDamage = 15f;
        public const float FallDamage = 80f;
        public const float AshWidowEncounterChance = 0.30f;
        public const float FeralDogEncounterChance = 0.25f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnGlassStormHit;
        public event Action<string> OnFallFromRoof;
        public event Action<string, string> OnNpcEncounter;   // (survivorId, npcType)
        public event Action<string> OnLootRecovered;

        private readonly System.Random _rng;
        private readonly HashSet<string> _searchedBuildings = new HashSet<string>();
        private bool _serverRackRecovered;
        private bool _keycardRecovered;
        private bool _parachuteSilkRecovered;

        public bool IsServerRackRecovered => _serverRackRecovered;
        public bool IsKeycardRecovered => _keycardRecovered;
        public bool IsParachuteSilkRecovered => _parachuteSilkRecovered;

        public Location_SkyscraperCanopy(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(8888);
        }

        /// <summary>
        /// Attempt to cross between two roofs via a plank bridge.
        /// High wind can trigger a glass storm.
        /// </summary>
        public bool AttemptRoofCrossing(string survivorId, bool hasGoggles, bool hasWinterCoat)
        {
            // Glass storm check
            if (_rng.NextDouble() < 0.35f)
            {
                if (!hasGoggles)
                {
                    OnGlassStormHit?.Invoke(survivorId);
                    return false; // Takes damage, may need to retreat
                }
                // Goggles protect from glass but not from the wind
            }

            // Fall check (no climbing gear = high risk)
            if (_rng.NextDouble() < 0.08f)
            {
                OnFallFromRoof?.Invoke(survivorId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Search a building's roof/upper floors. Returns loot found.
        /// </summary>
        public List<string> SearchBuilding(string buildingId, string survivorId)
        {
            if (string.IsNullOrEmpty(buildingId) || _searchedBuildings.Contains(buildingId))
                return null;

            _searchedBuildings.Add(buildingId);
            var loot = new List<string>();

            // NPC encounters
            if (_rng.NextDouble() < AshWidowEncounterChance)
                OnNpcEncounter?.Invoke(survivorId, "npc_ash_widows");
            else if (_rng.NextDouble() < FeralDogEncounterChance)
                OnNpcEncounter?.Invoke(survivorId, "feral_dog_pack");

            // Server rack blade — high electronic scrap yield
            if (!_serverRackRecovered && _rng.NextDouble() < 0.35f)
            {
                _serverRackRecovered = true;
                loot.Add(Loot_ServerRackBlade);
                OnLootRecovered?.Invoke(Loot_ServerRackBlade);
            }

            // Executive keycard — opens penthouse panic room
            if (!_keycardRecovered && _rng.NextDouble() < 0.20f)
            {
                _keycardRecovered = true;
                loot.Add(Loot_ExecutiveKeycard);
                OnLootRecovered?.Invoke(Loot_ExecutiveKeycard);
            }

            // Parachute silk — high-tier cloth for hazmat patching
            if (!_parachuteSilkRecovered && _rng.NextDouble() < 0.25f)
            {
                _parachuteSilkRecovered = true;
                loot.Add(Loot_ParachuteSilk);
                OnLootRecovered?.Invoke(Loot_ParachuteSilk);
            }

            return loot;
        }

        /// <summary>
        /// Open the penthouse panic room with the executive keycard.
        /// Returns premium loot.
        /// </summary>
        public List<string> OpenPanicRoom(string survivorId)
        {
            if (!_keycardRecovered) return null;
            var loot = new List<string>
            {
                "emergency_rations_premium",
                "bottled_water_import",
                "first_aid_kit_military"
            };
            return loot;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SkyscraperCanopySave CaptureState()
        {
            var buildings = new string[_searchedBuildings.Count];
            _searchedBuildings.CopyTo(buildings);
            return new SkyscraperCanopySave
            {
                ServerRackRecovered = _serverRackRecovered,
                KeycardRecovered = _keycardRecovered,
                ParachuteSilkRecovered = _parachuteSilkRecovered,
                SearchedBuildings = buildings
            };
        }

        public void RestoreState(SkyscraperCanopySave save)
        {
            _searchedBuildings.Clear();
            _serverRackRecovered = false;
            _keycardRecovered = false;
            _parachuteSilkRecovered = false;
            if (save == null) return;
            _serverRackRecovered = save.ServerRackRecovered;
            _keycardRecovered = save.KeycardRecovered;
            _parachuteSilkRecovered = save.ParachuteSilkRecovered;
            if (save.SearchedBuildings != null)
                for (int i = 0; i < save.SearchedBuildings.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedBuildings[i]))
                        _searchedBuildings.Add(save.SearchedBuildings[i]);
        }
    }

    [Serializable]
    public class SkyscraperCanopySave
    {
        public bool ServerRackRecovered;
        public bool KeycardRecovered;
        public bool ParachuteSilkRecovered;
        public string[] SearchedBuildings;
    }
}
