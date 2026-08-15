using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: Checkpoint Kilo (The Queue).
    /// Danger Level 7, Travel 3.0h, Base Rads 40 mSv/h.
    ///
    /// A military vehicle inspection post on Highway 9. On Day 0, the garrison
    /// sealed the underground bunker. 340 civilians gathered at the gate. By morning,
    /// 89 were dead. Private Maren loaded them into a fuel truck and buried them
    /// 400 metres east. He filed the report. His transfer was denied. He deserted
    /// on Day 9.
    /// </summary>
    [Serializable]
    public class CheckpointKiloMemorialState
    {
        public string locationId = "checkpoint_kilo_memorial";
        public string displayName = "Checkpoint Kilo (The Queue)";
        public float dangerLevel = 7f;
        public float travelHours = 3.0f;
        public float baseRadsPerHour = 40f;

        // Building state
        public bool guardBoothLooted = false;
        public bool bunkerEntranceLooted = false;
        public bool bunkerSupplyCacheBreached = false;
        public bool fuelTruckSiphoned = false;

        // Intercom
        public bool intercomBatteryConnected = false;

        // Trench
        public bool trenchVisited = false;
        public bool marenJournalTaken = false;
        public bool memorialPlaced = false;
        public int stonesPlaced = 0;
        public bool shoeTaken = false;
        public bool protocolDocumentTaken = false;

        // Bunker
        public bool bunkerSealed = true;
    }

    /// <summary>
    /// Checkpoint Kilo. "This facility is at capacity. Proceed to your assigned
    /// district shelter." The intercom played Protocol Nine while 600 people
    /// stood in the cold. The fence still stands. The bullet hole is visible.
    /// The children's shoe is still caught in the chain-link.
    /// </summary>
    /// <summary>ASHDEEP-Location — Checkpoint Kilo Memorial.</summary>
    public class Location_CheckpointKiloMemorial
    {
        private CheckpointKiloMemorialState _state = new CheckpointKiloMemorialState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> GuardBoothLoot = new List<string>
        {
            "flare_red", "flare_red", "flare_red",
            "garrison_requisition_forms"
        };

        public static readonly List<string> GuardBoothLockedCabinetLoot = new List<string>
        {
            "ammo_762x39_fmj" // 20 military rounds - needs lockpick
        };

        public static readonly List<string> BunkerEntranceLoot = new List<string>
        {
            "body_armour_military" // On a body, damaged durability 40
        };

        public static readonly List<string> BunkerSupplyCacheLoot = new List<string>
        {
            "mre_military", "mre_military", "mre_military",
            "mre_military", "mre_military", "mre_military"
        };

        public static readonly List<string> FuelTruckLoot = new List<string>
        {
            "fuel_1l", "fuel_1l", "fuel_1l", "fuel_1l"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<CheckpointKiloMemorialState, List<string>> OnLootCollected;
        public event Action<CheckpointKiloMemorialState> OnIntercomPlayed;
        public event Action<CheckpointKiloMemorialState> OnTrenchVisited;
        public event Action<CheckpointKiloMemorialState, int> OnMemorialPlaced;

        public CheckpointKiloMemorialState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The checkpoint is a ruin. Sandbags frozen solid. Boom barrier up and rusted. " +
                "The ash doesn't cover the stains. It pools around them. The underground bunker " +
                "is sealed — the garrison locked it from inside and never came out.";

            public const string QueueBarrierDescription =
                "The queue barrier isn't made of rope anymore. The ash has formed ridges " +
                "where people stood for hours, pressed together. The ground is compressed " +
                "in a human shape.";

            public const string FenceDescription =
                "The fence still stands. The bullet hole in the post is visible. " +
                "The children's shoe is still caught in the chain-link. They didn't make it over.";

            public const string IntercomDescription =
                "The intercom speaker hangs from the guard booth. If you connect a battery, " +
                "it will play the last three seconds of the loop: 'Remain calm. Help is—' " +
                "Then static. Then nothing.";

            public const string TrenchDescription =
                "Eighty-nine mounds. No headstones. A helmet on a stick. But someone has placed " +
                "a small stone on each mound. 89 stones. Recently placed. Someone still comes here.";

            public const string PrivateMarenJournalDescription =
                "A handwritten journal, 40 pages, buried under the helmet. Maren's account of " +
                "loading 89 bodies, the woman at the fence, the transfer request denied. " +
                "'I loaded them. I drove. I buried them. I filed the report. They denied my transfer.'";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public bool ConnectBattery()
        {
            if (_state.intercomBatteryConnected) return false;
            _state.intercomBatteryConnected = true;
            OnIntercomPlayed?.Invoke(_state);
            return true;
        }

        public List<string> LootGuardBooth()
        {
            if (_state.guardBoothLooted) return new List<string>();
            _state.guardBoothLooted = true;
            var loot = new List<string>(GuardBoothLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootLockedCabinet(string itemId)
        {
            if (itemId != "lockpick") return new List<string>();
            var loot = new List<string>(GuardBoothLockedCabinetLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootBunkerEntrance()
        {
            if (_state.bunkerEntranceLooted) return new List<string>();
            _state.bunkerEntranceLooted = true;
            var loot = new List<string>(BunkerEntranceLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public bool BreachBunkerSupplyCache(bool hasCrowbar, float hoursSpent)
        {
            if (_state.bunkerSupplyCacheBreached) return false;
            if (hoursSpent >= 2f && hasCrowbar)
            {
                _state.bunkerSupplyCacheBreached = true;
                return true;
            }
            return false;
        }

        public List<string> LootSupplyCache()
        {
            if (!_state.bunkerSupplyCacheBreached) return new List<string>();
            var loot = new List<string>(BunkerSupplyCacheLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> SiphonFuelTruck()
        {
            if (_state.fuelTruckSiphoned) return new List<string>();
            _state.fuelTruckSiphoned = true;
            var loot = new List<string>(FuelTruckLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public string TakeIntercomDocument()
        {
            if (_state.protocolDocumentTaken) return null;
            _state.protocolDocumentTaken = true;
            return "protocol_nine_document";
        }

        public string TakeMarenJournal()
        {
            if (_state.marenJournalTaken) return null;
            _state.marenJournalTaken = true;
            _state.trenchVisited = true;
            OnTrenchVisited?.Invoke(_state);
            return "private_maren_journal";
        }

        public string TakeChildShoe()
        {
            if (_state.shoeTaken) return null;
            _state.shoeTaken = true;
            return "child_shoe_single";
        }

        public bool PlaceMemorialStone()
        {
            _state.stonesPlaced++;
            OnMemorialPlaced?.Invoke(_state, _state.stonesPlaced);
            if (_state.stonesPlaced >= 89)
            {
                _state.memorialPlaced = true;
                return true;
            }
            return false;
        }

        /// <summary>Full loot sweep including environmental interactions.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootGuardBooth());
            allLoot.AddRange(LootBunkerEntrance());
            allLoot.AddRange(SiphonFuelTruck());
            var journal = TakeMarenJournal();
            if (journal != null) allLoot.Add(journal);
            var shoe = TakeChildShoe();
            if (shoe != null) allLoot.Add(shoe);
            var doc = TakeIntercomDocument();
            if (doc != null) allLoot.Add(doc);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public CheckpointKiloMemorialState CaptureState() => _state;
        public void RestoreState(CheckpointKiloMemorialState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
