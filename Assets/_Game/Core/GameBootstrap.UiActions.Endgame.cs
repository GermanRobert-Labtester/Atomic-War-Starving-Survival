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
            // Halt TimeSystem by not ticking (Update already gates on Phase/IsGameOver).
            PushEndgameSummaryToHud(summary);
            Debug.Log($"[GameBootstrap] ENDGAME ({summary.State}): {summary.OutcomeTitle} — {summary.Reason}");
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
