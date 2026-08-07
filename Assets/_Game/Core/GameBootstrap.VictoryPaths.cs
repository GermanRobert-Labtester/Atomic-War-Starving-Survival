// GameBootstrap.VictoryPaths.cs — boot/wire remaining Victory_* endgame paths.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private float _victoryBroadcastMinuteAccum;

        /// <summary>
        /// Construct every Victory_* path except TrueEnding (already in BootBatchSystems).
        /// Host hooks are offline-safe ending logs; active realtime paths only tick when started.
        /// </summary>
        private void BootVictoryPaths()
        {
            VictoryAirlift = new Victory_Airlift();
            VictoryAscendancy = new Victory_Ascendancy();
            VictoryBuriedAlive = new Victory_BuriedAlive();
            VictoryCannibalKing = new Victory_CannibalKing();
            VictoryDefection = new Victory_Defection();
            VictoryIcebreaker = new Victory_Icebreaker();
            VictoryLoneSurvivor = new Victory_LoneSurvivor();
            VictoryMAD = new Victory_MAD();
            VictoryMigration = new Victory_Migration();
            VictoryTheBroadcast = new Victory_TheBroadcast();
            VictoryTheCure = new Victory_TheCure();
            VictoryTheMartian = new Victory_TheMartian();
            VictoryUndergroundCity = new Victory_UndergroundCity();
            VictoryUnifier = new Victory_Unifier();

            WireVictoryPaths();
            Debug.Log("[GameBootstrap] Victory paths ready (14 endings; TrueEnding remains in batch).");
        }

        private void WireVictoryPaths()
        {
            if (VictoryAirlift != null)
            {
                VictoryAirlift.OnAirliftExtracted += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Airlift extraction complete");
                VictoryAirlift.OnDefenseFailed += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Airlift defense failed");
            }

            if (VictoryAscendancy != null)
                VictoryAscendancy.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: Ascendancy (Homo-Radiata)");

            if (VictoryBuriedAlive != null)
                VictoryBuriedAlive.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: Buried Alive");

            if (VictoryCannibalKing != null)
                VictoryCannibalKing.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: Cannibal King");

            if (VictoryDefection != null)
                VictoryDefection.OnGameOver += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Defection (bad ending)");

            if (VictoryIcebreaker != null)
            {
                VictoryIcebreaker.OnIcebreakerExtracted += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Icebreaker extraction");
                VictoryIcebreaker.OnExtractionFailed += (_, reason) =>
                    Debug.Log($"[GameBootstrap] VICTORY: Icebreaker failed — {reason}");
            }

            if (VictoryLoneSurvivor != null)
                VictoryLoneSurvivor.OnEndingTriggered += id =>
                    Debug.Log($"[GameBootstrap] VICTORY: Lone Survivor ({id})");

            if (VictoryMAD != null)
                VictoryMAD.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: MAD — flash of white");

            if (VictoryMigration != null)
                VictoryMigration.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: Migration (Trail of Tears)");

            if (VictoryTheBroadcast != null)
            {
                VictoryTheBroadcast.OnUploadComplete += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Broadcast upload complete");
                VictoryTheBroadcast.OnUploadFailed += (_, reason) =>
                    Debug.Log($"[GameBootstrap] VICTORY: Broadcast failed — {reason}");
            }

            if (VictoryTheCure != null)
                VictoryTheCure.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: The Cure");

            if (VictoryTheMartian != null)
                VictoryTheMartian.OnEndingTriggered += n =>
                    Debug.Log($"[GameBootstrap] VICTORY: The Martian — {n} launched");

            if (VictoryUndergroundCity != null)
            {
                VictoryUndergroundCity.OnSelfSustainingReached += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Underground City self-sustaining");
                VictoryUndergroundCity.OnHatchSealed += _ =>
                    Debug.Log("[GameBootstrap] VICTORY: Underground City hatch sealed");
            }

            if (VictoryUnifier != null)
                VictoryUnifier.OnEndingTriggered += () =>
                    Debug.Log("[GameBootstrap] VICTORY: Unifier — peace treaty signed");
        }

        /// <summary>
        /// Real-time: airlift defense timer (only while active). Broadcast uses game-minute path.
        /// </summary>
        private void TickVictoryPathsRealtime(float unscaledDelta)
        {
            if (VictoryAirlift != null && VictoryAirlift.State != null && VictoryAirlift.State.isActive)
                VictoryAirlift.TickRealTime(unscaledDelta);
        }

        /// <summary>
        /// Accumulate game hours into whole-minute ticks for The Broadcast upload (when active).
        /// Defense power is stubbed at 50 until hatch/combat host can supply a real value.
        /// </summary>
        private void TickVictoryPathsHourly(float gameHours)
        {
            if (gameHours <= 0f) return;
            if (VictoryTheBroadcast == null || VictoryTheBroadcast.State == null) return;
            if (!VictoryTheBroadcast.State.isActive) return;

            // 1 game-hour ≈ 1 broadcast "minute" step for upload progress (design: uploadSpeedPerMinute).
            _victoryBroadcastMinuteAccum += gameHours;
            while (_victoryBroadcastMinuteAccum >= 1f)
            {
                _victoryBroadcastMinuteAccum -= 1f;
                // Stub defense until combat host can supply hatch/weapon power.
                float defense = HatchDefenseSystem != null
                    ? HatchDefenseSystem.GetShelterSecurity()
                    : 50f;
                VictoryTheBroadcast.TickMinute(defense, null);
            }
        }

        /// <summary>
        /// Per game-day: lone-survivor check when host can supply day + alive count.
        /// Other endings remain player/action-driven (SealAndDetonate, FireAtOwnCoordinates, etc.).
        /// </summary>
        private void TickVictoryPathsDaily(int currentDay)
        {
            if (VictoryLoneSurvivor == null || Survivors == null) return;

            int alive = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null)
                    alive++;
            }

            VictoryLoneSurvivor.CheckVictory(currentDay, alive);
        }
    }
}
