// GameBootstrap.BatchSystems.cs — boot/wire remaining CaptureState systems (batch wiring).
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.UI;

using AtomicWar._Game.Endgame;

using AtomicWar._Game.Encounters;

using AtomicWar._Game.Factions;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private float _speedrunSecondAccum;
        private float _batchHourAccum;

        /// <summary>
        /// Construct every previously-unwired CaptureState system (disease, siege, mode, UI, victory).
        /// Host hooks are offline-safe; sieges wait for explicit Start* calls.
        /// </summary>
        private void BootBatchSystems()
        {
            DiseaseExpansion = new DiseaseSystem_Expansion();
            Scapegoat = new Dynamic_Scapegoat();
            IronMan = new Mode_IronMan();
            AndroidNpcs = new NPC_Android();
            Sheriff = new Role_Sheriff();
            ScenarioGen = new UI_ScenarioGen();
            SpeedrunTimer = new UI_SpeedrunTimer();
            TrueEnding = new Victory_TrueEnding();
            SiegeArtillery = new Siege_Artillery();
            SiegeBiowarfare = new Siege_Biowarfare();
            SiegeBlockade = new Siege_Blockade();
            SiegeHostageShield = new Siege_HostageShield();
            SiegeNightRaid = new Siege_NightRaid();
            SiegeSappers = new Siege_Sappers();
            SiegeSmokeOut = new Siege_SmokeOut();
            SiegeVehicleRam = new Siege_VehicleRam();

            WireBatchSystems();
            GameLog.Log("[GameBootstrap] Batch systems ready (disease/scapegoat/ironman/android/sheriff/scenario/speedrun/true_ending/8 sieges).");
        }

        private void WireBatchSystems()
        {
            if (IronMan != null)
            {
                IronMan.OnLastSurvivorDied += id =>
                    GameLog.Log($"[GameBootstrap] IRONMAN: last survivor '{id}' died — save marked for deletion.");
                IronMan.OnSaveDeleted += path =>
                    GameLog.Log($"[GameBootstrap] IRONMAN: save deleted at '{path}'");
            }

            if (Scapegoat != null)
            {
                Scapegoat.OnScapegoatSelected += id =>
                    GameLog.Log($"[GameBootstrap] SCAPEGOAT: bunker blames '{id}'");
                Scapegoat.OnMoraleDrained += id =>
                {
                    // Host: drain scapegoat morale when blamed (NeedsSystem 0–100 scale).
                    if (NeedsSystem == null || Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        var sv = Survivors[i];
                        if (sv == null || sv.Id != id) continue;
                        NeedsSystem.Modify(sv, NeedKind.Morale, -50f);
                        break;
                    }
                };
            }

            if (TrueEnding != null)
            {
                TrueEnding.OnBlueSky += day =>
                    GameLog.Log($"[GameBootstrap] TRUE ENDING: blue sky on day {day}");
                TrueEnding.OnTerraformerHacked += () =>
                    GameLog.Log("[GameBootstrap] TRUE ENDING: terraformer hacked");
            }

            if (DiseaseExpansion != null)
            {
                DiseaseExpansion.OnOutbreakDeclared += diseaseId =>
                    GameLog.Log($"[GameBootstrap] DISEASE: outbreak '{diseaseId}'");
                DiseaseExpansion.OnInfection += (sv, disease) =>
                    GameLog.Log($"[GameBootstrap] DISEASE: '{sv}' infected with '{disease}'");
            }

            if (Sheriff != null)
            {
                Sheriff.OnSheriffAssigned += id =>
                    GameLog.Log($"[GameBootstrap] SHERIFF: '{id}' wears the star");
            }

            if (SpeedrunTimer != null)
            {
                SpeedrunTimer.OnSplitRecorded += (name, rt, day) =>
                    GameLog.Log($"[GameBootstrap] SPEEDRUN: split '{name}' @ {rt:F1}s day {day}");
            }
        }

        /// <summary>Real-time: speedrun timer seconds.</summary>
        private void TickBatchSystemsRealtime(float unscaledDelta)
        {
            if (SpeedrunTimer == null) return;
            _speedrunSecondAccum += unscaledDelta;
            while (_speedrunSecondAccum >= 1f)
            {
                _speedrunSecondAccum -= 1f;
                // inGameDayDelta 0 here; day advances on day-tick path.
                SpeedrunTimer.TickSecond(1f, 0f);
            }
        }

        /// <summary>Accumulate game hours and fire whole-hour ticks for disease/sheriff/true-ending/biowarfare.</summary>
        private void TickBatchSystemsHourly(float gameHours)
        {
            if (gameHours <= 0f) return;
            _batchHourAccum += gameHours;
            while (_batchHourAccum >= 1f)
            {
                _batchHourAccum -= 1f;
                DiseaseExpansion?.TickHour();

                if (Sheriff != null && Survivors != null)
                {
                    var ids = new List<string>(Survivors.Count);
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        if (Survivors[i] != null && !string.IsNullOrEmpty(Survivors[i].Id))
                            ids.Add(Survivors[i].Id);
                    }
                    Sheriff.TickHour(ids);
                }

                if (TrueEnding != null)
                {
                    int watts = PowerNetwork != null ? Mathf.RoundToInt(PowerNetwork.TotalGeneration) : 0;
                    TrueEnding.UpdatePower(watts);
                    TrueEnding.TickHour();
                }

                SiegeBiowarfare?.TickHour();
            }
        }

        /// <summary>Per game-day: scapegoat selection, speedrun day counter, true-ending day stamp, blockade.</summary>
        private void TickBatchSystemsDaily(int currentDay)
        {
            TrueEnding?.SetCurrentDay(currentDay);
            SpeedrunTimer?.TickSecond(0f, 1f); // advance in-game day counter without real time

            if (Scapegoat != null && Survivors != null && NeedsSystem != null && Survivors.Count > 0)
            {
                float sum = 0f;
                int n = 0;
                var roster = new List<(string id, float skill, float strength)>();
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || string.IsNullOrEmpty(sv.Id)) continue;
                    float morale = (sv.Needs != null ? sv.Needs.Morale : 50f) / 100f;
                    sum += morale;
                    n++;
                    // Skill/strength proxies from health/fatigue residual (no full skill sheet required).
                    float skill = (sv.Needs != null ? sv.Needs.Health : 50f) / 100f;
                    float strength = 1f - ((sv.Needs != null ? sv.Needs.Fatigue : 0f) / 100f);
                    roster.Add((sv.Id, skill, strength));
                }
                if (n > 0)
                    Scapegoat.SelectScapegoat(roster, sum / n);
            }

            // Blockade tick only when active (host StartBlockade elsewhere).
            if (SiegeBlockade != null && SiegeBlockade.IsExpeditionLocked())
            {
                float food = Inventory != null ? Inventory.FoodFillRatio() : 0f;
                float water = WaterStorage != null ? WaterStorage.CleanWater : 0f;
                SiegeBlockade.TickDay(food, water, food + water);
            }
        }

        /// <summary>Iron Man: notify death and delete save when last living survivor falls.</summary>
        private void NotifyIronManSurvivorDeath(Survivor deceased)
        {
            if (IronMan == null || !IronMan.IsIronManActive() || deceased == null)
                return;

            int remaining = 0;
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || sv == deceased) continue;
                    // Treat missing health need as living; ForceDeath removes via OnDied.
                    remaining++;
                }
            }

            IronMan.OnSurvivorDeath(deceased.Id ?? "unknown", remaining);
            if (IronMan.ShouldDeleteSave())
                IronMan.DeleteSave();
        }
    }
}
