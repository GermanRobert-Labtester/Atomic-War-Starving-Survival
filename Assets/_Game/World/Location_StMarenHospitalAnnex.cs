using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: St. Maren's Regional Hospital — East Annex.
    /// Danger Level 6, Travel 2.0h, Base Rads 28 mSv/h.
    ///
    /// Forty beds for elderly patients, dementia care, and chronic illness
    /// management. The main hospital took a direct hit. The Annex survived
    /// because it was behind a concrete hill. The staff stayed for 19 days.
    /// The last patient, Agnes, died holding a nurse's hand. The nurse was 22.
    /// </summary>
    [Serializable]
    public class StMarenHospitalAnnexState
    {
        public string locationId = "st_maren_hospital_annex";
        public string displayName = "St. Maren's Regional Hospital — East Annex";
        public float dangerLevel = 6f;
        public float travelHours = 2.0f;
        public float baseRadsPerHour = 28f;

        // Building state
        public bool pharmacyUnlocked = false;
        public bool pharmacyLooted = false;
        public bool supplyClosetLooted = false;
        public bool nursesStationSearched = false;
        public bool headNurseOfficeSearched = false;
        public bool combinationCardTaken = false;
        public bool patientRecordsRead = false;
        public bool kitchenLooted = false;
        public bool patientRoomsSearched = false;
        public bool physioRoomLooted = false;
        public bool gardenVisited = false;
        public bool gardenSeedPlanted = false;

        // Special items
        public bool agnesLetterTaken = false;
        public int wheelchairsTaken = 0;

        // Echo
        public bool mirrorMessageSeen = false;
    }

    /// <summary>
    /// St. Maren's East Annex. "The Annex is intact but cold. The beds are made.
    /// The sheets are clean. The pharmacy is empty. The garden courtyard is buried
    /// in ash. The shift schedule is still posted. The last entry is Day 19. The
    /// nurse's name is Lena. There was no one to relieve her."
    /// </summary>
    /// <summary>ASHDEEP-Location — St. Maren's Hospital East Annex.</summary>
    public class Location_StMarenHospitalAnnex
    {
        private StMarenHospitalAnnexState _state = new StMarenHospitalAnnexState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> PharmacyLoot = new List<string>
        {
            "morphine_ampoule", "morphine_ampoule", "morphine_ampoule", "morphine_ampoule",
            "morphine_ampoule", "morphine_ampoule", "morphine_ampoule", "morphine_ampoule",
            "antibiotics_bottle_20", "antibiotics_bottle_20", "antibiotics_bottle_20",
            "saline_drip_bag", "saline_drip_bag", "saline_drip_bag", "saline_drip_bag", "saline_drip_bag"
        };

        public static readonly List<string> SupplyClosetLoot = new List<string>
        {
            "bandage_roll", "bandage_roll", "bandage_roll", "bandage_roll",
            "bandage_roll", "bandage_roll", "bandage_roll", "bandage_roll",
            "bandage_roll", "bandage_roll", "bandage_roll", "bandage_roll",
            "bandage_roll", "bandage_roll", "bandage_roll"
        };

        public static readonly List<string> KitchenLoot = new List<string>
        {
            "canned_food", "canned_food", "canned_food", "canned_food"
            // Last meals. Expired but sealed.
        };

        public static readonly List<string> PatientRoomsLoot = new List<string>
        {
            "wool_blanket", "wool_blanket", "wool_blanket", "wool_blanket",
            "wool_blanket", "wool_blanket", "wool_blanket", "wool_blanket"
        };

        public static readonly List<string> PhysioRoomLoot = new List<string>
        {
            "physiotherapy_bands", "physiotherapy_bands",
            "physiotherapy_bands", "physiotherapy_bands",
            "physiotherapy_bands", "physiotherapy_bands"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<StMarenHospitalAnnexState, List<string>> OnLootCollected;
        public event Action<StMarenHospitalAnnexState> OnPharmacyUnlocked;
        public event Action<StMarenHospitalAnnexState> OnCombinationFound;
        public event Action<StMarenHospitalAnnexState> OnAgnesLetterFound;
        public event Action<StMarenHospitalAnnexState> OnRecordsRead;
        public event Action<StMarenHospitalAnnexState> OnGardenVisited;

        public StMarenHospitalAnnexState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The East Annex survived because it was behind a concrete hill. The main hospital " +
                "took a direct hit. The Annex's windows are broken but the walls held. Inside, " +
                "the beds are made. The sheets are clean. It's been two months. Nobody changed the sheets.";

            public const string NursesStationDescription =
                "The nurses' station. A whiteboard with the shift schedule still posted. " +
                "Last entry: Day 19, 'Lena. 0600-1800. All patients. All rooms. All night.' " +
                "There was no one to relieve her.";

            public const string Room12Description =
                "Room 12. Flowers on the bedside table. Dead for two months. The water in the " +
                "vase is black. The card reads: 'For Agnes. Get well soon. — Staff.' Agnes did not " +
                "get well. She was 84. She died holding a nurse's hand. She said, 'Thank you for staying.'";

            public const string PharmacyDescription =
                "The locked pharmacy. A combination lock. The combination is in the head nurse's " +
                "office, in a drawer, on a card: 'IN CASE OF EMERGENCY.' The emergency was Day 0. " +
                "The card was never used. The pharmacy is still locked.";

            public const string GardenCourtyardDescription =
                "The garden courtyard. Buried in ash three inches deep. A bench faces east toward " +
                "the sunrise. The ash is compressed in the shape of a body. Someone sat here every " +
                "morning. The body is gone. The shape remains.";

            public const string PhysioMirrorDescription =
                "The physiotherapy room mirror. Someone has written in lipstick: 'They walked. " +
                "All of them. They walked out and they didn't look back.' The lipstick is red. " +
                "The handwriting is steady. The writer was not a patient.";

            public const string AgnesLetterDescription =
                "A letter in Room 12's bedside table. Dated Day -2. 'Dear Mum, do you need " +
                "anything from the market? I'm going Thursday. Love, Elena.' Agnes never answered. " +
                "The market is a crater.";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public bool FindCombinationCard()
        {
            if (_state.combinationCardTaken) return false;
            _state.headNurseOfficeSearched = true;
            _state.combinationCardTaken = true;
            OnCombinationFound?.Invoke(_state);
            return true;
        }

        public bool UnlockPharmacy(string combinationCode)
        {
            if (_state.pharmacyUnlocked) return true;
            // The combination is "7-23-41"
            if (combinationCode == "7-23-41")
            {
                _state.pharmacyUnlocked = true;
                OnPharmacyUnlocked?.Invoke(_state);
                return true;
            }
            return false;
        }

        public List<string> LootPharmacy()
        {
            if (!_state.pharmacyUnlocked || _state.pharmacyLooted) return new List<string>();
            _state.pharmacyLooted = true;
            var loot = new List<string>(PharmacyLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootSupplyCloset()
        {
            if (_state.supplyClosetLooted) return new List<string>();
            _state.supplyClosetLooted = true;
            var loot = new List<string>(SupplyClosetLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootKitchen()
        {
            if (_state.kitchenLooted) return new List<string>();
            _state.kitchenLooted = true;
            var loot = new List<string>(KitchenLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootPatientRooms()
        {
            if (_state.patientRoomsSearched) return new List<string>();
            _state.patientRoomsSearched = true;
            var loot = new List<string>(PatientRoomsLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public string TakeAgnesLetter()
        {
            if (_state.agnesLetterTaken) return null;
            _state.agnesLetterTaken = true;
            OnAgnesLetterFound?.Invoke(_state);
            return "agnes_letter";
        }

        public int TakeWheelchair()
        {
            if (_state.wheelchairsTaken >= 2) return 0;
            _state.wheelchairsTaken++;
            return 1;
        }

        public List<string> ReadPatientRecords()
        {
            if (_state.patientRecordsRead) return new List<string>();
            _state.patientRecordsRead = true;
            OnRecordsRead?.Invoke(_state);
            return new List<string> { "patient_records_annex" };
        }

        public List<string> LootPhysioRoom()
        {
            if (_state.physioRoomLooted) return new List<string>();
            _state.physioRoomLooted = true;
            var loot = new List<string>(PhysioRoomLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public void VisitGardenCourtyard()
        {
            if (_state.gardenVisited) return;
            _state.gardenVisited = true;
            OnGardenVisited?.Invoke(_state);
        }

        public bool PlantSeed()
        {
            if (_state.gardenSeedPlanted || !_state.gardenVisited) return false;
            _state.gardenSeedPlanted = true;
            return true;
        }

        public List<string> GetAll38Names()
        {
            return new List<string>
            {
                "Agnes", "Harold", "Margaret", "Thomas", "Ruth",
                "George", "Eleanor", "Frank", "Dorothy", "Albert",
                "Helen", "Walter", "Virginia", "Edward", "Martha",
                "Henry", "Florence", "Arthur", "Louise", "Herbert",
                "Mabel", "Clarence", "Esther", "Oscar", "Pearl",
                "Eugene", "Mildred", "Ernest", "Frances", "Stanley",
                "Beatrice", "Raymond", "Edith", "Howard", "Gertrude",
                "Leonard", "Evelyn", "Ida"
            };
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootSupplyCloset());
            allLoot.AddRange(LootKitchen());
            allLoot.AddRange(LootPatientRooms());
            allLoot.AddRange(LootPhysioRoom());
            allLoot.AddRange(ReadPatientRecords());
            var letter = TakeAgnesLetter();
            if (letter != null) allLoot.Add(letter);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public StMarenHospitalAnnexState CaptureState() => _state;
        public void RestoreState(StMarenHospitalAnnexState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
