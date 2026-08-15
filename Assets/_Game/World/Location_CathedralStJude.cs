using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion V — Mega-Location: Cathedral St. Jude (The Epicenter).
    /// The epicenter of the Glow. Stained glass melted into cobblestones.
    /// The Cult believes the radiation is the "breath of god." The air shimmers
    /// with Cherenkov radiation. 60 mSv/h — lethal in 14 days.
    /// </summary>
    public class Location_CathedralStJude
    {
        public const string LocationId = "location_cathedral_st_jude";
        public const string DisplayName = "Cathedral St. Jude";
        public const int TravelHours = 8;
        public const int DangerLevel = 8;
        public const float BaseRads = 60f;

        public const string Loot_SchematicChemSynthesis = "sch_chem_synthesis";
        public const string Loot_SawnOffDoubleBarrel = "weapon_sawn_off_double_barrel";
        public const string Loot_RelicChalice = "item_relic_pre_war_chalice";
        public const string Loot_SchematicAmmoReloader = "sch_ammo_reloader";

        public const string OutpostModuleId = "sanctuary_crypt";
        public const float OutpostLethalDoseDays = 14f;

        public event Action<string> OnCultZealotEncounter;
        public event Action<string> OnSpecialLootFound;
        public event Action<string> OnSanctuaryEstablished;

        private readonly System.Random _rng;
        private readonly HashSet<string> _searchedAreas = new HashSet<string>();
        private bool _chaliceFound;
        private bool _schematicFound;
        private bool _weaponFound;
        private bool _sanctuaryEstablished;

        public bool IsChaliceFound => _chaliceFound;
        public bool IsSanctuaryEstablished => _sanctuaryEstablished;

        public Location_CathedralStJude(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(9000);
        }

        public List<string> SearchArea(string areaId, string survivorId,
            bool isDisguised, bool hasAshGhillie)
        {
            if (_searchedAreas.Contains(areaId)) return null;
            _searchedAreas.Add(areaId);

            var loot = new List<string>();

            // Cult zealot encounter (skip if disguised)
            if (!isDisguised || !hasAshGhillie)
            {
                if (_rng.NextDouble() < 0.50f)
                    OnCultZealotEncounter?.Invoke(survivorId);
            }

            switch (areaId)
            {
                case "pews":
                    loot.Add("scrap_wood");
                    loot.Add("nails");
                    loot.Add("cloth");
                    break;

                case "melted_stained_glass":
                    loot.Add("glass_shards");
                    break;

                case "altar":
                    if (!_chaliceFound)
                    {
                        _chaliceFound = true;
                        loot.Add(Loot_RelicChalice);
                        OnSpecialLootFound?.Invoke(Loot_RelicChalice);
                    }
                    break;
            }

            if (!_schematicFound && _rng.NextDouble() < 0.25f)
            {
                _schematicFound = true;
                loot.Add(Loot_SchematicChemSynthesis);
                OnSpecialLootFound?.Invoke(Loot_SchematicChemSynthesis);
            }

            return loot;
        }

        public bool OpenAltarCache(string survivorId, bool hasChalice)
        {
            if (!hasChalice) return false;
            var loot = new List<string>();
            for (int i = 0; i < 10; i++) loot.Add("morphine_ampoule");
            for (int i = 0; i < 5; i++) loot.Add("iodine_pills_bottle_10_of_10");
            return true;
        }

        public bool FindSawnOff(string survivorId)
        {
            if (_weaponFound) return false;
            if (_rng.NextDouble() < 0.20f)
            {
                _weaponFound = true;
                OnSpecialLootFound?.Invoke(Loot_SawnOffDoubleBarrel);
                return true;
            }
            return false;
        }

        public bool EstablishSanctuary(string survivorId)
        {
            if (_sanctuaryEstablished) return false;
            _sanctuaryEstablished = true;
            OnSanctuaryEstablished?.Invoke(survivorId);
            return true;
        }

        public CathedralSave CaptureState()
        {
            var areas = new string[_searchedAreas.Count];
            _searchedAreas.CopyTo(areas);
            return new CathedralSave
            {
                SearchedAreas = areas,
                ChaliceFound = _chaliceFound,
                SchematicFound = _schematicFound,
                WeaponFound = _weaponFound,
                SanctuaryEstablished = _sanctuaryEstablished
            };
        }

        public void RestoreState(CathedralSave save)
        {
            _searchedAreas.Clear();
            _chaliceFound = false;
            _schematicFound = false;
            _weaponFound = false;
            _sanctuaryEstablished = false;
            if (save == null) return;
            if (save.SearchedAreas != null)
                for (int i = 0; i < save.SearchedAreas.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedAreas[i]))
                        _searchedAreas.Add(save.SearchedAreas[i]);
            _chaliceFound = save.ChaliceFound;
            _schematicFound = save.SchematicFound;
            _weaponFound = save.WeaponFound;
            _sanctuaryEstablished = save.SanctuaryEstablished;
        }
    }

    [Serializable]
    public class CathedralSave
    {
        public string[] SearchedAreas;
        public bool ChaliceFound;
        public bool SchematicFound;
        public bool WeaponFound;
        public bool SanctuaryEstablished;
    }
}
