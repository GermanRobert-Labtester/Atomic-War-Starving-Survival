using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion V — Mega-Location: The Iron Arteries (Railyard Graveyard).
    /// Three refugee trains and one military coal train stopped when the EMP hit.
    /// Doors locked from outside to "maintain quarantine." Labyrinth of rusted
    /// iron and frozen corpses. Homeless descendants live in coal cars.
    /// </summary>
    public class Location_RailyardGraveyard
    {
        public const string LocationId = "location_railyard_graveyard";
        public const string DisplayName = "The Railyard Graveyard";
        public const int TravelHours = 6;
        public const int DangerLevel = 7;
        public const float BaseRads = 15f;

        public const string Loot_SchematicUVCycling = "sch_uv_cycling";
        public const string Loot_WinstonGregun = "weapon_winston_gregun";
        public const string Loot_FuelDepotC4 = "item_c4";

        public event Action<string> OnHomelessEncounter;
        public event Action<string> OnRebelAmbush;
        public event Action<string> OnSpecialLootFound;

        private readonly System.Random _rng;
        private readonly HashSet<string> _searchedContainers = new HashSet<string>();
        private bool _schematicFound;
        private bool _weaponFound;
        private bool _fuelDepotSabotaged;

        public bool IsSchematicFound => _schematicFound;
        public bool IsWeaponFound => _weaponFound;
        public bool IsFuelDepotSabotaged => _fuelDepotSabotaged;

        public Location_RailyardGraveyard(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(8000);
        }

        public List<string> SearchFreightContainer(string containerId, string survivorId,
            bool hasAngleGrinder, bool hasLockpick)
        {
            if (_searchedContainers.Contains(containerId)) return null;
            _searchedContainers.Add(containerId);

            var loot = new List<string>();

            if (_rng.NextDouble() < 0.50f) loot.Add("canned_food");
            if (_rng.NextDouble() < 0.30f) loot.Add("winter_coat");
            if (_rng.NextDouble() < 0.10f) loot.Add("weapon_hmg");

            if (!_schematicFound && _rng.NextDouble() < 0.20f)
            {
                _schematicFound = true;
                loot.Add(Loot_SchematicUVCycling);
                OnSpecialLootFound?.Invoke(Loot_SchematicUVCycling);
            }

            if (_rng.NextDouble() < 0.25f) OnHomelessEncounter?.Invoke(survivorId);
            if (_rng.NextDouble() < 0.15f) OnRebelAmbush?.Invoke(survivorId);

            return loot;
        }

        public int ShovelCoal(string survivorId, int shovelHours)
        {
            return shovelHours * 2; // 2 coal per hour
        }

        public bool FindWinstonGregun(string survivorId)
        {
            if (_weaponFound) return false;
            if (_rng.NextDouble() < 0.15f)
            {
                _weaponFound = true;
                OnSpecialLootFound?.Invoke(Loot_WinstonGregun);
                return true;
            }
            return false;
        }

        public bool SabotageFuelDepot(string survivorId)
        {
            if (_fuelDepotSabotaged) return false;
            _fuelDepotSabotaged = true;
            return true;
        }

        public RailyardSave CaptureState()
        {
            var containers = new string[_searchedContainers.Count];
            _searchedContainers.CopyTo(containers);
            return new RailyardSave
            {
                SearchedContainers = containers,
                SchematicFound = _schematicFound,
                WeaponFound = _weaponFound,
                FuelDepotSabotaged = _fuelDepotSabotaged
            };
        }

        public void RestoreState(RailyardSave save)
        {
            _searchedContainers.Clear();
            _schematicFound = false;
            _weaponFound = false;
            _fuelDepotSabotaged = false;
            if (save == null) return;
            if (save.SearchedContainers != null)
                for (int i = 0; i < save.SearchedContainers.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedContainers[i]))
                        _searchedContainers.Add(save.SearchedContainers[i]);
            _schematicFound = save.SchematicFound;
            _weaponFound = save.WeaponFound;
            _fuelDepotSabotaged = save.FuelDepotSabotaged;
        }
    }

    [Serializable]
    public class RailyardSave
    {
        public string[] SearchedContainers;
        public bool SchematicFound;
        public bool WeaponFound;
        public bool FuelDepotSabotaged;
    }
}
