using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: Marta's Farmhouse (Eastern Lowlands).
    /// Danger Level 3, Travel 1.5h, Base Rads 10 mSv/h.
    ///
    /// A two-room stone house with a tin roof, a root cellar, and a potato field.
    /// Marta Kowalski paid the militia's tax for six weeks — 10%, 15%, 20%, 25%,
    /// 30%, 35%. On Day 45 she said no. The militia took the potatoes. They left
    /// a receipt. On Day 52 Marta walked north. Her children, Tomas (8) and
    /// Lena (5), went south in a militia truck.
    /// </summary>
    [Serializable]
    public class MartaFarmhouseState
    {
        public string locationId = "marta_farmhouse";
        public string displayName = "Marta's Farmhouse (Eastern Lowlands)";
        public float dangerLevel = 3f;
        public float travelHours = 1.5f;
        public float baseRadsPerHour = 10f;

        // Building state
        public bool kitchenSearched = false;
        public bool bedroomSearched = false;
        public bool rootCellarLooted = false;
        public bool woodShedLooted = false;
        public bool gardenInspected = false;

        // Items
        public bool receiptTaken = false;
        public bool schoolbookTaken = false;
        public bool rabbitTaken = false;
        public bool photographTaken = false;
        public bool flyerTaken = false;
        public bool sewingKitTaken = false;
        public bool candlesTaken = false;
        public bool saltTaken = false;

        // Echo
        public bool porchLaundrySeen = false;
        public bool cellarScratchesSeen = false;
        public bool halfHarvestedRowSeen = false;
    }

    /// <summary>
    /// Marta's Farmhouse. "The door is unlocked. The stove is cold. The children's
    /// beds are made. Lena's stuffed rabbit is on the pillow. Tomas's schoolbook
    /// is open to a page about photosynthesis: 'Plants need sunlight to grow.'
    /// There is no sunlight. There hasn't been for two months."
    /// </summary>
    /// <summary>ASHDEEP-Location — Marta's Farmhouse.</summary>
    public class Location_MartaFarmhouse
    {
        private MartaFarmhouseState _state = new MartaFarmhouseState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> RootCellarLoot = new List<string>
        {
            "vegetable_potato", "vegetable_potato", "vegetable_potato"
            // Last potatoes. The militia took the rest.
        };

        public static readonly List<string> WoodShedLoot = new List<string>
        {
            "wood_block", "wood_block", "wood_block",
            "wood_block", "wood_block", "wood_block"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<MartaFarmhouseState, List<string>> OnLootCollected;
        public event Action<MartaFarmhouseState> OnKitchenSearched;
        public event Action<MartaFarmhouseState> OnBedroomSearched;
        public event Action<MartaFarmhouseState> OnRootCellarEntered;
        public event Action<MartaFarmhouseState> OnGardenInspected;

        public MartaFarmhouseState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "Marta's farmhouse sits alone in the eastern lowlands. Stone walls, tin roof, " +
                "half an acre of potato field behind it. The door is unlocked. The laundry line " +
                "on the porch still has clothes — Marta's apron, Tomas's shirt, Lena's dress. " +
                "The dress is small. The dress is blue. The dress will never be worn again.";

            public const string KitchenDescription =
                "The kitchen. Four chairs at the table. One chair is pulled out, as if someone " +
                "just left. A half-eaten potato on a plate. The fork beside it. The meal was " +
                "interrupted. The interruption was the militia.";

            public const string ReceiptDescription =
                "The militia's receipt, on the kitchen table: 'RECEIVED: 40 kg potatoes, 12 eggs. " +
                "FROM: Marta K., Eastern Lowlands. REASON: Outstanding contribution. " +
                "SIGNED: Cmdr. Voss.' The paper is thin. The ink is still black.";

            public const string BedroomDescription =
                "The bedroom. Two beds — one for Marta, one for the children. Lena's stuffed " +
                "rabbit on the pillow, one ear torn. Tomas's schoolbook open to photosynthesis: " +
                "'Plants need sunlight to grow.' There is no sunlight.";

            public const string RootCellarDescription =
                "The root cellar. Three potatoes left. Scratch marks on the inside of the door. " +
                "Not from an animal. From a child. Tomas scratched his name into the wood. " +
                "He was eight. The letters are uneven.";

            public const string GardenDescription =
                "The potato field. A row half-harvested. The militia took the rest. " +
                "Three plants remain, dead. The soil is cold. Nothing will grow here until " +
                "the ash clears. The ash will not clear.";

            public const string MilitiaFlyerDescription =
                "A printed flyer on the kitchen table, under the receipt: 'YOUR CHILD'S FUTURE " +
                "IS SECURE. The Upland Militia Youth Program provides nutrition, education, and " +
                "vocational training. Enrollment is mandatory for all non-compliant families.'";

            public const string FamilyPhotographDescription =
                "A photograph on the kitchen shelf. Marta, Tomas, Lena. The husband is in uniform. " +
                "He was conscripted on Day -20. He did not come back. The photograph was taken in " +
                "summer. Everyone is smiling. The sun is in their eyes.";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public void SearchKitchen()
        {
            if (_state.kitchenSearched) return;
            _state.kitchenSearched = true;
            OnKitchenSearched?.Invoke(_state);
        }

        public string TakeReceipt()
        {
            if (_state.receiptTaken) return null;
            _state.receiptTaken = true;
            return "marta_receipt";
        }

        public string TakeMilitiaFlyer()
        {
            if (_state.flyerTaken) return null;
            _state.flyerTaken = true;
            return "militia_youth_program_flyer";
        }

        public string TakeFamilyPhotograph()
        {
            if (_state.photographTaken) return null;
            _state.photographTaken = true;
            return "family_photograph";
        }

        public string TakeCandles()
        {
            if (_state.candlesTaken) return null;
            _state.candlesTaken = true;
            return "candle_tallow";
        }

        public string TakeSalt()
        {
            if (_state.saltTaken) return null;
            _state.saltTaken = true;
            return "salt";
        }

        public string TakeSewingKit()
        {
            if (_state.sewingKitTaken) return null;
            _state.sewingKitTaken = true;
            return "sewing_kit_10_of_10";
        }

        public void SearchBedroom()
        {
            if (_state.bedroomSearched) return;
            _state.bedroomSearched = true;
            OnBedroomSearched?.Invoke(_state);
        }

        public string TakeSchoolbook()
        {
            if (_state.schoolbookTaken) return null;
            _state.schoolbookTaken = true;
            return "tomas_schoolbook";
        }

        public string TakeLenaRabbit()
        {
            if (_state.rabbitTaken) return null;
            _state.rabbitTaken = true;
            return "lena_stuffed_rabbit";
        }

        public void EnterRootCellar()
        {
            if (_state.cellarScratchesSeen) return;
            _state.cellarScratchesSeen = true;
            OnRootCellarEntered?.Invoke(_state);
        }

        public List<string> LootRootCellar()
        {
            if (_state.rootCellarLooted) return new List<string>();
            _state.rootCellarLooted = true;
            var loot = new List<string>(RootCellarLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootWoodShed()
        {
            if (_state.woodShedLooted) return new List<string>();
            _state.woodShedLooted = true;
            var loot = new List<string>(WoodShedLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public void InspectGarden()
        {
            if (_state.gardenInspected) return;
            _state.gardenInspected = true;
            OnGardenInspected?.Invoke(_state);
        }

        public string GetPorchLaundryDescription()
        {
            _state.porchLaundrySeen = true;
            return "Marta's apron. Tomas's shirt. Lena's dress. The dress is small. " +
                   "The dress is blue. The dress has a flower pattern. The flowers are faded. " +
                   "The dress will never be worn again.";
        }

        public string GetCellarScratchesDescription()
        {
            _state.cellarScratchesSeen = true;
            return "Tomas scratched his name into the wood of the root cellar door. " +
                   "The letters are uneven. He was five when he learned to write. " +
                   "He was eight when he scratched the door. The door is closed. " +
                   "The scratches are on the inside.";
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            SearchKitchen();
            var receipt = TakeReceipt();
            if (receipt != null) allLoot.Add(receipt);
            var flyer = TakeMilitiaFlyer();
            if (flyer != null) allLoot.Add(flyer);
            var photo = TakeFamilyPhotograph();
            if (photo != null) allLoot.Add(photo);
            var candles = TakeCandles();
            if (candles != null) allLoot.Add(candles);
            var salt = TakeSalt();
            if (salt != null) allLoot.Add(salt);
            var sewing = TakeSewingKit();
            if (sewing != null) allLoot.Add(sewing);
            SearchBedroom();
            var book = TakeSchoolbook();
            if (book != null) allLoot.Add(book);
            var rabbit = TakeLenaRabbit();
            if (rabbit != null) allLoot.Add(rabbit);
            EnterRootCellar();
            allLoot.AddRange(LootRootCellar());
            allLoot.AddRange(LootWoodShed());
            InspectGarden();
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public MartaFarmhouseState CaptureState() => _state;
        public void RestoreState(MartaFarmhouseState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
