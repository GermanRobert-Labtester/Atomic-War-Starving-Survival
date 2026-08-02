using System;
using AtomicWar.Core.Events;
using AtomicWar.Core.Save;
using UnityEngine;

namespace AtomicWar.Runtime.GameState
{
    public enum DayCyclePhase
    {
        Morning,  // 06:00 - 08:00: Report & morning events
        Day,      // 08:00 - 18:00: Shelter crafting & building
        Evening,  // 18:00 - 21:00: Night preparation & assignments
        Night,    // 21:00 - 06:00: Scavenging runs & shelter raids
        GameOver  // Terminal phase
    }

    public struct PhaseChangedEvent
    {
        public DayCyclePhase PreviousPhase;
        public DayCyclePhase NewPhase;
        public int DayNumber;
        public int Hour;
    }

    [Serializable]
    public class DayCycleSaveData
    {
        public int DayNumber;
        public string CurrentPhase;
        public float CurrentHour;
        public bool IsPaused;
        public float TimeScale;
    }

    /// <summary>
    /// Pure C# system managing day cycle phases (Morning, Day, Evening, Night), phase events, and state persistence.
    /// </summary>
    public class GameStateSystem : ISavable
    {
        public DayCyclePhase CurrentPhase { get; private set; } = DayCyclePhase.Morning;
        public int DayNumber { get; private set; } = 1;

        public string SaveKey => "GameStateSystem";

        public GameStateSystem()
        {
            Debug.Log("[GameStateSystem] Initialized with Day/Night cycle.");
        }

        public void SetPhase(DayCyclePhase newPhase, int hour = 0)
        {
            if (CurrentPhase == newPhase) return;

            var oldPhase = CurrentPhase;
            CurrentPhase = newPhase;

            EventBus.Raise(new PhaseChangedEvent
            {
                PreviousPhase = oldPhase,
                NewPhase = CurrentPhase,
                DayNumber = DayNumber,
                Hour = hour
            });

            Debug.Log($"[GameStateSystem] Phase Transition: {oldPhase} -> {CurrentPhase} (Day {DayNumber}, Hour {hour:00}:00)");
        }

        public void AdvanceDay()
        {
            DayNumber++;
            Debug.Log($"[GameStateSystem] Advanced to Day {DayNumber}");
        }

        public void TriggerGameOver(string reason)
        {
            var oldPhase = CurrentPhase;
            CurrentPhase = DayCyclePhase.GameOver;
            Debug.LogWarning($"[GameStateSystem] GAME OVER: {reason}");

            EventBus.Raise(new PhaseChangedEvent
            {
                PreviousPhase = oldPhase,
                NewPhase = DayCyclePhase.GameOver,
                DayNumber = DayNumber,
                Hour = 0
            });
        }

        // ISavable implementation
        public object CaptureState()
        {
            return new DayCycleSaveData
            {
                DayNumber = DayNumber,
                CurrentPhase = CurrentPhase.ToString()
            };
        }

        public void RestoreState(object state)
        {
            if (state is DayCycleSaveData saveData)
            {
                DayNumber = saveData.DayNumber;
                if (Enum.TryParse<DayCyclePhase>(saveData.CurrentPhase, out var phase))
                {
                    CurrentPhase = phase;
                }
                Debug.Log($"[GameStateSystem] Restored State: Day {DayNumber}, Phase {CurrentPhase}");
            }
        }
    }
}
