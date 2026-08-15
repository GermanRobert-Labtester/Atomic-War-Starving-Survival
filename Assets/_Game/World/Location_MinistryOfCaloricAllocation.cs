using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion VIII — Location: Ministry of Caloric Allocation. Where the
    /// "Caloric Triage Protocols" were drafted. They didn't starve the outer
    /// districts by accident; they did it via spreadsheet. The archives contain
    /// proof of war crimes. The sub-basement holds the Director's Reserve.
    /// </summary>
    public class Location_MinistryOfCaloricAllocation
    {
        public const string LocationId = "location_ministry_of_caloric_allocation";
        public const string DisplayName = "Ministry of Caloric Allocation";
        public const int TravelHours = 3;
        public const int DangerLevel = 6;
        public const float BaseRads = 22f;

        public const string Item_LedgerDistrict9 = "ledger_district9";
        public const string Item_StampMinistry = "stamp_ministry_official";
        public const string Item_RationCardBlank = "ration_card_blank";
        public const string NPC_Archivist = "npc_the_archivist";

        public event Action<string> OnArchivistEncounter;
        public event Action<string> OnLedgerFound;
        public event Action<string> OnStampFound;

        private readonly System.Random _rng;
        private bool _ledgerFound;
        private bool _stampFound;
        private bool _archivistEncountered;
        private readonly HashSet<string> _searchedCabinets = new HashSet<string>();

        public bool IsLedgerFound => _ledgerFound;
        public bool IsStampFound => _stampFound;

        public Location_MinistryOfCaloricAllocation(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(11000);
        }

        public List<string> SearchFilingCabinet(string cabinetId, string survivorId)
        {
            if (_searchedCabinets.Contains(cabinetId)) return null;
            _searchedCabinets.Add(cabinetId);

            var loot = new List<string>();

            if (!_ledgerFound && _rng.NextDouble() < 0.15f)
            {
                _ledgerFound = true;
                loot.Add(Item_LedgerDistrict9);
                OnLedgerFound?.Invoke(survivorId);
            }

            if (_rng.NextDouble() < 0.40f)
                loot.Add(Item_RationCardBlank);

            return loot;
        }

        public ArchivistResult EncounterArchivist(string survivorId, bool canRead)
        {
            if (_archivistEncountered)
                return new ArchivistResult { AlreadyEncountered = true };

            _archivistEncountered = true;
            OnArchivistEncounter?.Invoke(survivorId);

            return new ArchivistResult
            {
                Success = true,
                CanTrade = canRead,
                Message = "A blind man sits among the files. His fingers trace the edges of folders. " +
                    "He knows the filing system by touch. He will trade the Ministry Stamp " +
                    "for you reading the retraction orders that never arrived."
            };
        }

        public bool TradeWithArchivist(string survivorId, bool hasReadRetraction)
        {
            if (!hasReadRetraction) return false;
            _stampFound = true;
            OnStampFound?.Invoke(survivorId);
            return true;
        }

        public MinistrySave CaptureState()
        {
            var cabinets = new string[_searchedCabinets.Count];
            _searchedCabinets.CopyTo(cabinets);
            return new MinistrySave
            {
                LedgerFound = _ledgerFound,
                StampFound = _stampFound,
                ArchivistEncountered = _archivistEncountered,
                SearchedCabinets = cabinets
            };
        }

        public void RestoreState(MinistrySave save)
        {
            _searchedCabinets.Clear();
            _ledgerFound = false;
            _stampFound = false;
            _archivistEncountered = false;
            if (save == null) return;
            _ledgerFound = save.LedgerFound;
            _stampFound = save.StampFound;
            _archivistEncountered = save.ArchivistEncountered;
            if (save.SearchedCabinets != null)
                for (int i = 0; i < save.SearchedCabinets.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedCabinets[i]))
                        _searchedCabinets.Add(save.SearchedCabinets[i]);
        }
    }

    [Serializable]
    public class ArchivistResult
    {
        public bool Success;
        public bool AlreadyEncountered;
        public bool CanTrade;
        public string Message;
    }

    [Serializable]
    public class MinistrySave
    {
        public bool LedgerFound;
        public bool StampFound;
        public bool ArchivistEncountered;
        public string[] SearchedCabinets;
    }
}
