using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {

        private void CheckWinLose()
        {
            if (VictoryProject == null || VictoryProject.IsTerminal) return;
            if (Survivors == null) return;

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // Loss: all survivors dead → death-screen by cause (rads / hunger / breakdowns).
            VictoryProject.EvaluateLoss(Survivors, day);

            if (EndgameEngine != null && !EndgameEngine.Result.IsTerminal)
            {
                bool isExtractionUnlocked = VictoryProject != null && VictoryProject.ExtractionUnlocked;
                bool isHydroponicsWorking = Shelter != null && Shelter.IsGrowLightActive;
                int deadCount = 0;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && !Survivors[i].IsAlive) deadCount++;
                }

                EndgameEngine.Evaluate(
                    day,
                    Survivors,
                    Shelter,
                    isExtractionUnlocked,
                    isHydroponicsWorking,
                    deadCount);
            }
        }

        public void RecordMoralChoice()
        {
            VictoryProject?.RecordMoralChoice();
        }

        private void TickClothing(float gameHours)
        {
            if (ClothingSystem == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                float humidity = 0.5f;
                if (!string.IsNullOrEmpty(sv.CurrentRoomId) && Shelter != null && Shelter.Rooms != null)
                {
                    for (int r = 0; r < Shelter.Rooms.Count; r++)
                    {
                        var room = Shelter.Rooms[r];
                        if (room != null && string.Equals(room.RoomId, sv.CurrentRoomId, System.StringComparison.Ordinal))
                        {
                            humidity = room.Humidity;
                            break;
                        }
                    }
                }
                ClothingSystem.Tick(sv, gameHours, humidity);
            }
        }

        private void ApplyEndgame(EndgameSummaryData summary)
        {
            if (summary == null) return;
            IsGameOver = true;
            GameOverReason = summary.Reason ?? summary.OutcomeTitle;
            if (GameState != null)
            {
                GameState.IsPaused = true;
                GameState.Phase = GamePhase.GameOver;
            }
            TryRecordLastWillGrave(summary);
            TryGenerateEpilogue(summary);
            // Halt TimeSystem by not ticking (Update already gates on Phase/IsGameOver).
            PushEndgameSummaryToHud(summary);
            Debug.Log($"[GameBootstrap] ENDGAME ({summary.State}): {summary.OutcomeTitle} — {summary.Reason}");
        }

        /// <summary>
        /// Prompt #768 — freeze empty-bunker epilogue (meals, bullets, death rooms, journals).
        /// </summary>
        private void TryGenerateEpilogue(EndgameSummaryData summary)
        {
            if (EpilogueStats == null || summary == null) return;

            var dead = new List<string>();
            var deathRooms = new List<string>();
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null) continue;
                    if (sv.IsAlive) continue;
                    string name = string.IsNullOrEmpty(sv.DisplayName) ? sv.Id : sv.DisplayName;
                    dead.Add(name);
                    if (!string.IsNullOrEmpty(sv.CurrentRoomId))
                        deathRooms.Add(sv.CurrentRoomId);
                }
            }

            var journals = new List<string>();
            if (JournalSystem != null && JournalSystem.Entries != null)
            {
                int n = Mathf.Min(3, JournalSystem.EntryCount);
                for (int i = 0; i < n; i++)
                {
                    var e = JournalSystem.Entries[i];
                    if (e != null && !string.IsNullOrEmpty(e.Text))
                        journals.Add(e.Text);
                }
            }
            if (journals.Count == 0 && !string.IsNullOrEmpty(summary.Reason))
                journals.Add(summary.Reason);

            var record = EpilogueStats.GenerateEpilogue(
                mealsCooked: -1, // use live counters
                bulletsFired: -1,
                daysSurvived: summary.DaysSurvived,
                deadSurvivors: dead,
                deathRoomIds: deathRooms,
                finalJournalEntries: journals);

            string narrative = EpilogueStats.GetNarrativeSummary(record);
            if (!string.IsNullOrEmpty(narrative))
                Debug.Log($"[GameBootstrap] EPILOGUE: {narrative}");
        }

        /// <summary>
        /// Prompt #829 — bag transfusion. Incompatible bag → hemolytic shock (80% death).
        /// Returns true if compatible. Person-to-person stays on <see cref="BloodTransfusion"/>.
        /// </summary>
        public bool TryTransfuseBloodBag(string recipientId, string bagType)
        {
            if (BloodTypes == null || string.IsNullOrEmpty(recipientId) || string.IsNullOrEmpty(bagType))
                return false;

            bool compatible = BloodTypes.TryTransfuseBag(recipientId, bagType, out bool died);
            if (!compatible && died && Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || sv.Id != recipientId || !sv.IsAlive) continue;
                    NeedsSystem?.ForceDeath(sv);
                    break;
                }
            }
            return compatible;
        }

        /// <summary>
        /// On wipe (all survivors dead), snapshot a grave site for rogue-lite discovery.
        /// Wins do not generate a grave.
        /// </summary>
        private void TryRecordLastWillGrave(EndgameSummaryData summary)
        {
            if (LastWill == null || summary == null) return;
            if (summary.State != EndgameState.Starved && summary.State != EndgameState.Irradiated)
                return;
            if (LastWill.HasGraveSite) return;

            var names = new List<string>();
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null) continue;
                    names.Add(string.IsNullOrEmpty(sv.DisplayName) ? sv.Id : sv.DisplayName);
                }
            }

            var loot = new List<string>();
            if (Inventory?.Slots != null)
            {
                for (int i = 0; i < Inventory.Slots.Count && loot.Count < 12; i++)
                {
                    var slot = Inventory.Slots[i];
                    if (slot?.Item == null || string.IsNullOrEmpty(slot.Item.id)) continue;
                    if (!loot.Contains(slot.Item.id))
                        loot.Add(slot.Item.id);
                }
            }

            var diaries = new List<string>();
            if (!string.IsNullOrEmpty(summary.Reason))
                diaries.Add(summary.Reason);

            string cause = summary.DeathScreen != DeathScreenKind.None
                ? summary.DeathScreen.ToString().ToLowerInvariant()
                : (summary.State.ToString().ToLowerInvariant());

            LastWill.GenerateGraveSite(
                locationId: "grave_player_bunker",
                originalSeed: _worldSeed,
                dayOfDeath: summary.DaysSurvived > 0
                    ? summary.DaysSurvived
                    : (TimeSystem != null ? TimeSystem.CurrentDay : 1),
                deadSurvivorNames: names,
                diaryEntries: diaries,
                remainingLootIds: loot,
                causeOfDeath: cause);

            // Prompt #859 — seed legacy-start payload so the next run can choose ruined bunker.
            PrepareLegacyStartFromLastWill();
        }

        /// <summary>
        /// Seed <see cref="LegacyStart"/> from the current Last Will grave using bunker room ids.
        /// Does not activate the legacy run — call <see cref="TryBeginLegacyStart"/> for that.
        /// </summary>
        public bool PrepareLegacyStartFromLastWill()
        {
            if (LegacyStart == null || LastWill == null || !LastWill.HasGraveSite)
                return false;

            IList<string> rooms = null;
            if (Shelter != null)
            {
                var ids = Shelter.GetRoomIds();
                if (ids != null && ids.Count > 0)
                    rooms = ids;
            }

            return LegacyStart.PrepareFromLastWill(
                LastWill,
                rooms,
                previousSaveId: LastWill.CurrentGraveSite?.locationId ?? "grave_player_bunker");
        }

        /// <summary>
        /// Activate Prompt #859 legacy start: flood ruined rooms, grant leftover loot,
        /// and fire discovery events. Returns true when a legacy run was begun.
        /// </summary>
        public bool TryBeginLegacyStart()
        {
            if (LegacyStart == null) return false;

            if (!LegacyStart.IsPrepared && LastWill != null && LastWill.HasGraveSite)
                PrepareLegacyStartFromLastWill();

            if (!LegacyStart.CheckAvailability())
                return false;
            if (LegacyStart.IsLegacyRunActive)
                return true;

            var lootIds = LegacyStart.BeginLegacyRun();

            // Apply floods into the live flooding system.
            if (FloodingSystem != null)
            {
                var ruined = LegacyStart.GetRuinedRooms();
                for (int i = 0; i < ruined.Count; i++)
                    FloodingSystem.ForceFlood(ruined[i]);
            }

            // Starting bonus: one of each remaining loot id from the prior wipe.
            if (Inventory != null && lootIds != null && _itemCatalog != null)
            {
                for (int i = 0; i < lootIds.Count; i++)
                {
                    string id = lootIds[i];
                    if (string.IsNullOrEmpty(id)) continue;
                    var item = _itemCatalog.GetById(id);
                    if (item != null)
                        Inventory.Add(item, 1);
                }
            }

            Debug.Log(
                $"[GameBootstrap] LEGACY START active (prior day {LegacyStart.PriorDayOfDeath}, " +
                $"cause={LegacyStart.CauseOfDeath}, rooms={LegacyStart.GetRuinedRooms().Count}, " +
                $"corpses={LegacyStart.GetCorpseLocations().Count}).");
            return true;
        }

        private void PushEndgameSummaryToHud(EndgameSummaryData summary)
        {
            if (_hud == null || summary == null) return;
            var ui = _hud.EnsureEndgameSummary();
            if (ui == null) return;
            ui.Show(
                summary.State.ToString(),
                summary.OutcomeTitle,
                summary.OutcomeBody,
                summary.DeathScreen == DeathScreenKind.None ? string.Empty : summary.DeathScreen.ToString(),
                summary.DaysSurvived,
                summary.TotalRadiationAbsorbed,
                summary.MoralChoicesMade,
                summary.MilitaryIntelDecrypted,
                summary.ExtractionUnlocked,
                summary.VehicleEscapeUsed);
        }

        private static ItemDefinition MakeRuntimeItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.stackMax = id == VictoryProjectManager.EngineItemId ? 1 : 99;
            item.weight = 0.1f;
            if (id == VictoryProjectManager.EngineItemId)
            {
                item.type = ItemType.Tool;
                item.durability = 100f;
            }
            else if (id == VictoryProjectManager.FuelItemId)
            {
                item.type = ItemType.Fuel;
            }
            else
            {
                item.type = ItemType.Material;
            }
            return item;
        }

    }
}
