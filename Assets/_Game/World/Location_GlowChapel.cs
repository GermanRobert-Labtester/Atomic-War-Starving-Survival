using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: The Glow Chapel (Irradiated Zone East).
    /// Danger Level 6, Travel 3.5h, Base Rads 55 mSv/h.
    ///
    /// Once a water pumping station, now a functioning commune of 80 converts
    /// who worship the radiation itself. Brother Orin walked here through the
    /// fallout on Day 21 and said the humming of the dead turbines sounded like
    /// a choir. The Cup has been in the fallout zone. The water clicks.
    /// They drink. They smile.
    /// </summary>
    [Serializable]
    public class GlowChapelState
    {
        public string locationId = "the_glow_chapel";
        public string displayName = "The Glow Chapel (Irradiated Zone East)";
        public float dangerLevel = 6f;
        public float travelHours = 3.5f;
        public float baseRadsPerHour = 55f;

        // Building state
        public bool sanctuaryLooted = false;
        public bool gardenVisited = false;
        public bool storageRoomBreached = false;
        public bool dryingRacksLooted = false;
        public bool sleepingQuartersSearched = false;
        public bool handprintPaintCollected = false;

        // Items
        public bool cupTaken = false;
        public bool orinJournalTaken = false;
        public bool iodinePillsTaken = false;

        // Cult state
        public int convertCount = 83;
        public int childHandprints = 3;
        public bool cultHostile = false;
        public bool ceremonyActive = false;
    }

    /// <summary>
    /// The Glow Chapel. "The turbine room is the sanctuary — candles on the pipes,
    /// a cloth altar, a wooden bowl. The upper floors are sleeping quarters.
    /// The basement is the 'Garden of Light' — a room where the most devoted sit
    /// in silence for hours, absorbing ambient radiation. They call it Communion.
    /// Their dosimeters read 30 mSv/h in the Garden. They come out smiling."
    /// </summary>
    /// <summary>ASHDEEP-Location — The Glow Chapel.</summary>
    public class Location_GlowChapel
    {
        private GlowChapelState _state = new GlowChapelState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> SanctuaryLoot = new List<string>
        {
            "candle_tallow", "candle_tallow", "candle_tallow", "candle_tallow",
            "candle_tallow", "candle_tallow", "candle_tallow", "candle_tallow",
            "candle_tallow", "candle_tallow", "candle_tallow", "candle_tallow",
            "candle_tallow", "candle_tallow", "candle_tallow", "candle_tallow",
            "candle_tallow", "candle_tallow", "candle_tallow", "candle_tallow"
        };

        public static readonly List<string> GardenOfLightLoot = new List<string>
        {
            "water_bottle_1l_full", "water_bottle_1l_full",
            "water_bottle_1l_full", "water_bottle_1l_full",
            "water_bottle_1l_full", "water_bottle_1l_full"
            // Contaminated at 15 mSv/L — noted in item description
        };

        public static readonly List<string> DryingRacksLoot = new List<string>
        {
            "herbs", "herbs", "herbs", "herbs", "herbs",
            "herbs", "herbs", "herbs", "herbs", "herbs",
            "herbs", "herbs", "herbs", "herbs", "herbs"
        };

        public static readonly List<string> HandprintPaintLoot = new List<string>
        {
            "convert_handprint_paint", "convert_handprint_paint",
            "convert_handprint_paint", "convert_handprint_paint",
            "convert_handprint_paint", "convert_handprint_paint",
            "convert_handprint_paint", "convert_handprint_paint"
        };

        public static readonly List<string> StorageRoomLoot = new List<string>
        {
            "decontamination_soap_5_of_5", "decontamination_soap_5_of_5"
        };

        public static readonly List<string> HiddenIodinePillsLoot = new List<string>
        {
            "iodine_pills", "iodine_pills", "iodine_pills", "iodine_pills"
        };

        public static readonly List<string> SleepingQuartersLoot = new List<string>
        {
            "family_photograph", "family_photograph", "family_photograph"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<GlowChapelState, List<string>> OnLootCollected;
        public event Action<GlowChapelState> OnGardenEntered;
        public event Action<GlowChapelState> OnCupTaken;
        public event Action<GlowChapelState> OnOrinJournalFound;
        public event Action<GlowChapelState> OnStorageRoomBreached;

        public GlowChapelState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The Glow Chapel was once a water pumping station. Concrete walls, steel pipes, " +
                "a turbine room in the basement. Now the walls are covered in grey handprints — " +
                "83 of them, painted in ash and animal fat. The pipes hum with nothing. " +
                "Candles on the turbine blades make the shadows turn.";

            public const string SanctuaryDescription =
                "The turbine room is the sanctuary. Candles on the pipes. A cloth altar. " +
                "A wooden bowl — the Cup — in the center. The candles are tallow. " +
                "The singing is off-key. The radiation is 55 mSv/h. Nobody seems to mind.";

            public const string GardenOfLightDescription =
                "The Garden of Light. A room in the basement where the most devoted sit " +
                "in silence for six-hour sessions. The bench is worn smooth. A name is scratched " +
                "into the wood: 'YURI.' Yuri is dead. The bench is still warm when you find it.";

            public const string HandprintsDescription =
                "Eighty-three grey handprints on the sanctuary walls. Three are small — " +
                "at child height. They are near the door. They look like the walls are reaching out.";

            public const string BrotherOrinDescription =
                "Brother Orin leads services twice daily. His voice is calm. His hands are scarred " +
                "from walking through the fallout on Day 21. He speaks of the Glow as a presence, " +
                "a forgiveness, a home. He does not speak of the garrison, the militia, or the warlords. " +
                "He speaks of the light. The light is enough.";

            public const string StorageRoomDescription =
                "The storage room has a lock on the outside. The lock is from the inside. " +
                "Someone locked themselves in. The iodine pills are behind the lock. " +
                "The Cult rejects iodine pills. The person who locked them is gone.";

            public const string JournalDescription =
                "Brother Orin's journal, 60 pages, hidden under his mattress. He writes: " +
                "'Every institution is a machine that grinds people into fuel. The garrison " +
                "grinds them into numbers. The militia grinds them into grain. The warlords " +
                "grind them into tribute. I offer a machine that grinds them into light. " +
                "The light is not real. But the grinding stops. Is that not enough?'";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public void EnterGardenOfLight()
        {
            if (_state.gardenVisited) return;
            _state.gardenVisited = true;
            OnGardenEntered?.Invoke(_state);
        }

        public string TakeTheCup()
        {
            if (_state.cupTaken) return null;
            _state.cupTaken = true;
            OnCupTaken?.Invoke(_state);
            return "glow_cup";
        }

        public string TakeOrinJournal()
        {
            if (_state.orinJournalTaken) return null;
            _state.orinJournalTaken = true;
            OnOrinJournalFound?.Invoke(_state);
            return "brother_orin_journal";
        }

        public List<string> LootSanctuaryCandles()
        {
            if (_state.sanctuaryLooted) return new List<string>();
            _state.sanctuaryLooted = true;
            var loot = new List<string>(SanctuaryLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> CollectGardenWater()
        {
            if (!_state.gardenVisited) return new List<string>();
            var loot = new List<string>(GardenOfLightLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootDryingRacks()
        {
            if (_state.dryingRacksLooted) return new List<string>();
            _state.dryingRacksLooted = true;
            var loot = new List<string>(DryingRacksLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> CollectHandprintPaint()
        {
            if (_state.handprintPaintCollected) return new List<string>();
            _state.handprintPaintCollected = true;
            return new List<string>(HandprintPaintLoot);
        }

        public bool BreachStorageRoom(string itemId)
        {
            if (_state.storageRoomBreached) return true;
            if (itemId == "crowbar")
            {
                _state.storageRoomBreached = true;
                OnStorageRoomBreached?.Invoke(_state);
                return true;
            }
            return false;
        }

        public List<string> LootStorageRoom()
        {
            if (!_state.storageRoomBreached) return new List<string>();
            var loot = new List<string>(StorageRoomLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> TakeIodinePills()
        {
            if (_state.iodinePillsTaken) return new List<string>();
            _state.iodinePillsTaken = true;
            var loot = new List<string>(HiddenIodinePillsLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootSleepingQuarters()
        {
            if (_state.sleepingQuartersSearched) return new List<string>();
            _state.sleepingQuartersSearched = true;
            var loot = new List<string>(SleepingQuartersLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootSanctuaryCandles());
            EnterGardenOfLight();
            allLoot.AddRange(CollectGardenWater());
            allLoot.AddRange(LootDryingRacks());
            allLoot.AddRange(CollectHandprintPaint());
            allLoot.AddRange(LootSleepingQuarters());
            var journal = TakeOrinJournal();
            if (journal != null) allLoot.Add(journal);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public GlowChapelState CaptureState() => _state;
        public void RestoreState(GlowChapelState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
