using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: The Grain Exchange (Upland Militia HQ).
    /// Danger Level 5, Travel 2.5h, Base Rads 18 mSv/h.
    ///
    /// Once Tessarat's weekly livestock and produce market, now a fortified
    /// militia compound. Commander Voss, a former agronomist, does the math:
    /// 400 fighters need 960,000 calories. The villages produce 1,200,000.
    /// The margin is survival. Voss protects the margin.
    /// </summary>
    [Serializable]
    public class MilitiaGrainExchangeState
    {
        public string locationId = "militia_grain_exchange";
        public string displayName = "The Grain Exchange (Upland Militia HQ)";
        public float dangerLevel = 5f;
        public float travelHours = 2.5f;
        public float baseRadsPerHour = 18f;

        // Building state
        public bool grainStorageLooted = false;
        public bool agShedLooted = false;
        public bool vossOfficeUnlocked = false;
        public bool vossDeskDrawerOpened = false;
        public bool medicalLockboxLooted = false;
        public bool guardTowerLooted = false;
        public bool storageCellarLooted = false;
        public bool supplyRoomLooted = false;

        // Quest
        public bool charterTaken = false;
        public bool ledgerTaken = false;
        public bool martaReceiptTaken = false;
        public bool childrenPresent = true; // Tomas and Lena are in the youth program
        public bool childrenExtracted = false;

        // Youth program
        public int childrenCount = 12;
    }

    /// <summary>
    /// The Grain Exchange. "Every Thursday, farmers brought grain, vegetables,
    /// eggs, and livestock. The militia used to meet here, informally, over
    /// coffee and bread. They discussed crop yields, irrigation schedules,
    /// and the central government's tax demands."
    /// </summary>
    /// <summary>ASHDEEP-Location — The Militia Grain Exchange.</summary>
    public class Location_MilitiaGrainExchange
    {
        private MilitiaGrainExchangeState _state = new MilitiaGrainExchangeState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> GrainStorageLoot = new List<string>
        {
            "wheat_flour", "wheat_flour", "wheat_flour", "wheat_flour",
            "wheat_flour", "wheat_flour", "wheat_flour", "wheat_flour"
        };

        public static readonly List<string> AgShedLoot = new List<string>
        {
            "fertilizer", "fertilizer", "fertilizer", "fertilizer",
            "fertilizer", "fertilizer", "fertilizer", "fertilizer",
            "fertilizer", "fertilizer", "fertilizer", "fertilizer"
        };

        public static readonly List<string> VossDeskLoot = new List<string>
        {
            "seed_envelope_wheat", "seed_envelope_wheat", "seed_envelope_wheat",
            "seed_envelope_wheat", "seed_envelope_wheat", "seed_envelope_wheat"
        };

        public static readonly List<string> MedicalLockboxLoot = new List<string>
        {
            "antibiotics_bottle_20", "antibiotics_bottle_20"
        };

        public static readonly List<string> GuardTowerLoot = new List<string>
        {
            "hunting_rifle_bolt"
        };

        public static readonly List<string> StorageCellarLoot = new List<string>
        {
            "canned_beans", "canned_beans", "canned_beans", "canned_beans",
            "canned_beans", "canned_beans", "canned_beans", "canned_beans",
            "canned_beans", "canned_beans"
        };

        public static readonly List<string> SupplyRoomLoot = new List<string>
        {
            "militia_uniform_patch", "militia_uniform_patch",
            "militia_uniform_patch", "militia_uniform_patch",
            "militia_uniform_patch"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<MilitiaGrainExchangeState, List<string>> OnLootCollected;
        public event Action<MilitiaGrainExchangeState> OnVossOfficeUnlocked;
        public event Action<MilitiaGrainExchangeState> OnCharterFound;
        public event Action<MilitiaGrainExchangeState> OnLedgerFound;

        public MilitiaGrainExchangeState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The Grain Exchange is now a fortified compound. Sandbag walls. A guard tower " +
                "on the weighing platform. The fountain — dry since Year 3 of the drought — " +
                "is a watch post. The bulletin board that once posted surplus notices now lists " +
                "what each village owes, updated weekly.";

            public const string FountainDescription =
                "The fountain basin is filled with ash. In the center, where the water jet was, " +
                "someone has planted a single wheat stalk. It's dead. It's been dead for weeks. " +
                "Nobody removes it.";

            public const string WeighingPlatformDescription =
                "The weighing platform has scratch marks. Not from weapons. From fingernails. " +
                "Someone was dragged across it. The marks are old.";

            public const string BulletinBoardDescription =
                "A new notice, over the old ones: 'CONTRIBUTION SCHEDULE — WEEK 9. ALL VILLAGES " +
                "REPORT BY THURSDAY. FAILURE TO COMPLY WILL RESULT IN PATROL WITHDRAWAL.' " +
                "Underneath, in pencil, barely visible: 'Marta was here.'";

            public const string VossOfficeDescription =
                "Commander Voss's office. He still wears his farmer's overalls. He still smiles. " +
                "He still asks how the harvest is going. The ledger on his desk has 340 entries. " +
                "A coffee mug says 'WORLD'S BEST DAD.' The coffee is cold.";

            public const string YouthProgramDescription =
                "Twelve children, ages 6-14, in the back of the market square. They are fed. " +
                "They are trained. They carry wooden rifles. They drill in the courtyard. " +
                "They do not play. Tomas and Lena are among them.";

            public const string CharterDescription =
                "The founding charter, framed on Voss's wall: 'We protect the land. We protect " +
                "the people on the land. No one takes what we grew.' Written on a feed sack. " +
                "The ink is faded. The words are still true. The words are also a lie. Both are true.";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public bool UnlockVossOffice(string itemId)
        {
            if (_state.vossOfficeUnlocked) return true;
            if (itemId == "lockpick" || itemId == "crowbar")
            {
                _state.vossOfficeUnlocked = true;
                OnVossOfficeUnlocked?.Invoke(_state);
                return true;
            }
            return false;
        }

        public bool OpenVossDeskDrawer(string itemId)
        {
            if (_state.vossDeskDrawerOpened) return true;
            if (itemId == "lockpick")
            {
                _state.vossDeskDrawerOpened = true;
                return true;
            }
            return false;
        }

        public List<string> LootGrainStorage()
        {
            if (_state.grainStorageLooted) return new List<string>();
            _state.grainStorageLooted = true;
            var loot = new List<string>(GrainStorageLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootAgShed()
        {
            if (_state.agShedLooted) return new List<string>();
            _state.agShedLooted = true;
            var loot = new List<string>(AgShedLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootVossDesk()
        {
            if (!_state.vossDeskDrawerOpened) return new List<string>();
            var loot = new List<string>(VossDeskLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootMedicalLockbox()
        {
            if (_state.medicalLockboxLooted) return new List<string>();
            _state.medicalLockboxLooted = true;
            var loot = new List<string>(MedicalLockboxLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootGuardTower()
        {
            if (_state.guardTowerLooted) return new List<string>();
            _state.guardTowerLooted = true;
            var loot = new List<string>(GuardTowerLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootStorageCellar()
        {
            if (_state.storageCellarLooted) return new List<string>();
            _state.storageCellarLooted = true;
            var loot = new List<string>(StorageCellarLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootSupplyRoom()
        {
            if (_state.supplyRoomLooted) return new List<string>();
            _state.supplyRoomLooted = true;
            var loot = new List<string>(SupplyRoomLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public string TakeCharter()
        {
            if (_state.charterTaken) return null;
            _state.charterTaken = true;
            OnCharterFound?.Invoke(_state);
            return "militia_charter_feed_sack";
        }

        public string TakeLedger()
        {
            if (_state.ledgerTaken) return null;
            _state.ledgerTaken = true;
            OnLedgerFound?.Invoke(_state);
            return "requisition_ledger";
        }

        public string TakeMartaReceipt()
        {
            if (_state.martaReceiptTaken) return null;
            _state.martaReceiptTaken = true;
            return "marta_receipt";
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootGrainStorage());
            allLoot.AddRange(LootAgShed());
            allLoot.AddRange(LootMedicalLockbox());
            allLoot.AddRange(LootGuardTower());
            allLoot.AddRange(LootStorageCellar());
            allLoot.AddRange(LootSupplyRoom());
            var ledger = TakeLedger();
            if (ledger != null) allLoot.Add(ledger);
            var receipt = TakeMartaReceipt();
            if (receipt != null) allLoot.Add(receipt);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public MilitiaGrainExchangeState CaptureState() => _state;
        public void RestoreState(MilitiaGrainExchangeState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
