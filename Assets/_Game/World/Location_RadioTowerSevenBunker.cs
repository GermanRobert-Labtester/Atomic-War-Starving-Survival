using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: Broadcast Tower 7 — Sub-Level Bunker.
    /// Danger Level 7, Travel 4.0h, Base Rads 45 mSv/h.
    ///
    /// An 80-metre military communications relay on the northern ridge. On Day 0,
    /// the six operators in the sub-level bunker transmitted the order that killed
    /// Tessarat. They transmitted it because the order came from the chain of command.
    /// They transmitted it because that was their job. Then they sat in the bunker
    /// and listened to the static that followed.
    /// </summary>
    [Serializable]
    public class RadioTowerSevenBunkerState
    {
        public string locationId = "radio_tower_seven_bunker";
        public string displayName = "Broadcast Tower 7 — Sub-Level Bunker";
        public float dangerLevel = 7f;
        public float travelHours = 4.0f;
        public float baseRadsPerHour = 45f;

        // Building state
        public bool baseStationSearched = false;
        public bool baseStationConsoleActive = false;
        public bool frequencyLogTaken = false;
        public bool frequencyManualTaken = false;
        public bool bunkerDoorOpen = false;
        public bool bunkerEntered = false;
        public bool bunkerSupplyCacheLooted = false;
        public bool bunkerArmoryLooted = false;
        public bool recordingStationSearched = false;

        // Base station
        public bool radiosTaken = false;
        public bool batteriesTaken = false;

        // Bunker access
        public bool bunkerCodeKnown = false;
        public bool keycardObtained = false;
        public bool broadcastEquipmentRepaired = false;
        public bool broadcastEquipmentTaken = false;
        public bool backupGeneratorPartsTaken = false;

        // Final log
        public bool operatorFinalLogTaken = false;

        // Tower
        public bool towerDown = true;
        public bool plaqueSeen = false;
    }

    /// <summary>
    /// Broadcast Tower 7. "The tower is down — snapped at the third guy-wire.
    /// The base station is intact but stripped. The sub-level bunker is sealed.
    /// Captain Venn ordered it sealed on Day 0 and it hasn't opened since. The
    /// ceasefire has not been confirmed. Captain Venn's final log entry: 'Day 35.
    /// The frequencies are quiet.' He signed off. The recording is still in there."
    /// </summary>
    /// <summary>ASHDEEP-Location — Broadcast Tower 7 Sub-Level Bunker.</summary>
    public class Location_RadioTowerSevenBunker
    {
        private RadioTowerSevenBunkerState _state = new RadioTowerSevenBunkerState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> BaseStationDeskLoot = new List<string>
        {
            "handheld_radio", "handheld_radio"
        };

        public static readonly List<string> BaseStationStorageLoot = new List<string>
        {
            "battery", "battery", "battery", "battery", "battery", "battery"
        };

        public static readonly List<string> BunkerSupplyCacheLoot = new List<string>
        {
            "mre_military", "mre_military", "mre_military", "mre_military",
            "mre_military", "mre_military", "mre_military", "mre_military",
            "mre_military", "mre_military", "mre_military", "mre_military"
        };

        public static readonly List<string> BunkerArmoryLoot = new List<string>
        {
            "ammo_762x51_exi", "ammo_762x51_exi", "ammo_762x51_exi",
            "ammo_762x51_exi", "ammo_762x51_exi", "ammo_762x51_exi",
            "ammo_762x51_exi", "ammo_762x51_exi", "ammo_762x51_exi",
            "ammo_762x51_exi", "ammo_762x51_exi", "ammo_762x51_exi",
            "ammo_762x51_exi", "ammo_762x51_exi", "ammo_762x51_exi"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<RadioTowerSevenBunkerState, List<string>> OnLootCollected;
        public event Action<RadioTowerSevenBunkerState> OnBaseStationSearched;
        public event Action<RadioTowerSevenBunkerState> OnBunkerDoorOpened;
        public event Action<RadioTowerSevenBunkerState> OnBunkerEntered;
        public event Action<RadioTowerSevenBunkerState> OnFinalLogFound;
        public event Action<RadioTowerSevenBunkerState> OnConsoleActive;

        public RadioTowerSevenBunkerState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The tower is down — an 80-metre spire snapped at the third guy-wire. " +
                "The base station building is intact but stripped. A plaque on the tower base: " +
                "'BROADCAST TOWER 7. ERECTED 1978. SERVED THE PEOPLE OF TESSARAT.' " +
                "The plaque is intact. The tower is not.";

            public const string BaseStationDescription =
                "The base station's main room. A coffee mug on the console, evaporated. " +
                "It reads: 'WORLD'S BEST OPERATOR.' A name is scratched into the bottom: " +
                "'YURI.' Yuri is the same name on the bench in the Glow Chapel. " +
                "Yuri was here first. Yuri left. Yuri walked into the Glow.";

            public const string FrequencyLogDescription =
                "The frequency log has a page torn out. The missing page is dated Day 0. " +
                "The entry that's still visible on the torn edge reads: '...ORDER 7741. " +
                "CONFIRMED. GOD FORGIVE US.' The tear is fresh. Someone visited recently.";

            public const string SealedBunkerDescription =
                "The sub-level bunker airlock. Military-grade. Requires a keycard and a " +
                "four-digit code. Through the small window: a desk, a chair, a microphone. " +
                "The microphone is off. The chair is empty. A photograph of six operators " +
                "is on the desk, face-down. The photograph is dated Day -1.";

            public const string BunkerInteriorDescription =
                "The bunker is cold. 4°C. The operators wore their coats. Six bunks, " +
                "a recording station, an armory, a supply cache. Captain Venn's body is " +
                "in the commander's chair. His keycard is still in his pocket.";

            public const string FinalLogDescription =
                "Captain Venn's final recording. 'Day 35. The ceasefire has not been " +
                "confirmed. The bunker will remain sealed. The operators are alive. " +
                "I am alive. The frequencies are quiet.' He signed off. The recording ends. " +
                "The silence after is longer than the recording.";

            public const string SurvivorFrequencyDescription =
                "The survivor frequency has a single voice, repeating: 'Is anyone there? " +
                "Is anyone there? Is anyone there?' The voice has been repeating for thirty days. " +
                "Nobody answers. With the broadcast equipment, you could answer.";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public void SearchBaseStation()
        {
            if (_state.baseStationSearched) return;
            _state.baseStationSearched = true;
            OnBaseStationSearched?.Invoke(_state);
        }

        public bool ActivateConsole(string itemId)
        {
            if (_state.baseStationConsoleActive) return true;
            if (itemId == "battery")
            {
                _state.baseStationConsoleActive = true;
                OnConsoleActive?.Invoke(_state);
                return true;
            }
            return false;
        }

        public string TakeFrequencyLog()
        {
            if (_state.frequencyLogTaken) return null;
            _state.frequencyLogTaken = true;
            _state.bunkerCodeKnown = true;
            return "frequency_log_sealed";
        }

        public string TakeFrequencyManual()
        {
            if (_state.frequencyManualTaken) return null;
            _state.frequencyManualTaken = true;
            return "broadcast_frequency_manual";
        }

        public List<string> TakeRadios()
        {
            if (_state.radiosTaken) return new List<string>();
            _state.radiosTaken = true;
            var loot = new List<string>(BaseStationDeskLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> TakeBatteries()
        {
            if (_state.batteriesTaken) return new List<string>();
            _state.batteriesTaken = true;
            var loot = new List<string>(BaseStationStorageLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public bool TryOpenBunkerDoor(string code, bool hasKeycard)
        {
            if (_state.bunkerDoorOpen) return true;
            // The code is 7741 (the order number)
            if (code == "7741" && hasKeycard)
            {
                _state.bunkerDoorOpen = true;
                _state.keycardObtained = true;
                OnBunkerDoorOpened?.Invoke(_state);
                return true;
            }
            return false;
        }

        public void EnterBunker()
        {
            if (_state.bunkerEntered || !_state.bunkerDoorOpen) return;
            _state.bunkerEntered = true;
            OnBunkerEntered?.Invoke(_state);
        }

        public string TakeCaptainVennKeycard()
        {
            if (_state.keycardObtained) return null;
            _state.keycardObtained = true;
            return "captain_venn_keycard";
        }

        public List<string> LootSupplyCache()
        {
            if (_state.bunkerSupplyCacheLooted) return new List<string>();
            _state.bunkerSupplyCacheLooted = true;
            var loot = new List<string>(BunkerSupplyCacheLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootArmory()
        {
            if (_state.bunkerArmoryLooted) return new List<string>();
            _state.bunkerArmoryLooted = true;
            var loot = new List<string>(BunkerArmoryLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public string TakeOperatorFinalLog()
        {
            if (_state.operatorFinalLogTaken || !_state.bunkerEntered) return null;
            _state.operatorFinalLogTaken = true;
            _state.recordingStationSearched = true;
            OnFinalLogFound?.Invoke(_state);
            return "operator_final_log";
        }

        public string GetBroadcastEquipment()
        {
            if (_state.broadcastEquipmentTaken) return null;
            _state.broadcastEquipmentTaken = true;
            return "radio_broadcast_equipment";
        }

        public string GetBackupGeneratorParts()
        {
            if (_state.backupGeneratorPartsTaken) return null;
            _state.backupGeneratorPartsTaken = true;
            return "backup_generator_parts";
        }

        /// <summary>Repair broadcast equipment (returns true if successful).</summary>
        public bool RepairBroadcastEquipment(int electronicScrapAvailable)
        {
            if (_state.broadcastEquipmentRepaired) return true;
            if (electronicScrapAvailable >= 3)
            {
                _state.broadcastEquipmentRepaired = true;
                return true;
            }
            return false;
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(TakeRadios());
            allLoot.AddRange(TakeBatteries());
            var log = TakeFrequencyLog();
            if (log != null) allLoot.Add(log);
            var manual = TakeFrequencyManual();
            if (manual != null) allLoot.Add(manual);
            var equip = GetBroadcastEquipment();
            if (equip != null) allLoot.Add(equip);
            var parts = GetBackupGeneratorParts();
            if (parts != null) allLoot.Add(parts);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public RadioTowerSevenBunkerState CaptureState() => _state;
        public void RestoreState(RadioTowerSevenBunkerState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
