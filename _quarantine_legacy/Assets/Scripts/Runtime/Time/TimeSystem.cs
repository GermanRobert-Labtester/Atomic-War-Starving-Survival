using System;
using AtomicWar.Core.Events;
using AtomicWar.Core.Save;
using AtomicWar.Runtime.GameState;
using UnityEngine;

namespace AtomicWar.Runtime.Time
{
    public struct HourTickEvent
    {
        public int CurrentHour;
        public int DayNumber;
        public DayCyclePhase CurrentPhase;
    }

    public struct TimePauseChangedEvent
    {
        public bool IsPaused;
    }

    /// <summary>
    /// Pure C# Time System controlling the day/night clock, phase progression, pausing, and save persistence.
    /// </summary>
    public class TimeSystem : ISavable
    {
        public const int HoursPerDay = 24;

        // Hour schedule thresholds
        public const int MorningStartHour = 6;
        public const int DayStartHour = 8;
        public const int EveningStartHour = 18;
        public const int NightStartHour = 21;

        private readonly GameStateSystem _gameStateSystem;

        public float CurrentHour { get; private set; } = MorningStartHour;
        public float TimeScale { get; set; } = 1.0f;
        public bool IsPaused { get; private set; } = false;

        private float _secondsPerGameHour = 4.0f; // 4 real seconds = 1 in-game hour
        private float _hourProgress = 0f;

        public string SaveKey => "TimeSystem";

        public TimeSystem(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
            EvaluatePhaseForHour((int)CurrentHour);
        }

        public void Tick(float deltaTime)
        {
            if (IsPaused || _gameStateSystem?.CurrentPhase == DayCyclePhase.GameOver) return;

            _hourProgress += (deltaTime * TimeScale) / _secondsPerGameHour;
            if (_hourProgress >= 1.0f)
            {
                _hourProgress -= 1.0f;
                AdvanceHour();
            }
        }

        public void AdvanceHour()
        {
            int oldHour = (int)CurrentHour;
            CurrentHour = (CurrentHour + 1) % HoursPerDay;
            int newHour = (int)CurrentHour;

            // Day rollover at Morning start (06:00)
            if (newHour == MorningStartHour && oldHour != MorningStartHour)
            {
                _gameStateSystem?.AdvanceDay();
            }

            EvaluatePhaseForHour(newHour);

            EventBus.Raise(new HourTickEvent
            {
                CurrentHour = newHour,
                DayNumber = _gameStateSystem?.DayNumber ?? 1,
                CurrentPhase = _gameStateSystem?.CurrentPhase ?? DayCyclePhase.Day
            });
        }

        public void EvaluatePhaseForHour(int hour)
        {
            if (_gameStateSystem == null) return;

            DayCyclePhase targetPhase;
            if (hour >= MorningStartHour && hour < DayStartHour)
            {
                targetPhase = DayCyclePhase.Morning;
            }
            else if (hour >= DayStartHour && hour < EveningStartHour)
            {
                targetPhase = DayCyclePhase.Day;
            }
            else if (hour >= EveningStartHour && hour < NightStartHour)
            {
                targetPhase = DayCyclePhase.Evening;
            }
            else
            {
                targetPhase = DayCyclePhase.Night;
            }

            _gameStateSystem.SetPhase(targetPhase, hour);
        }

        // Pause Controls
        public void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
                EventBus.Raise(new TimePauseChangedEvent { IsPaused = true });
                Debug.Log("[TimeSystem] Time PAUSED.");
            }
        }

        public void Resume()
        {
            if (IsPaused)
            {
                IsPaused = false;
                EventBus.Raise(new TimePauseChangedEvent { IsPaused = false });
                Debug.Log("[TimeSystem] Time RESUMED.");
            }
        }

        public void TogglePause()
        {
            if (IsPaused) Resume();
            else Pause();
        }

        // ISavable implementation
        public object CaptureState()
        {
            return new DayCycleSaveData
            {
                DayNumber = _gameStateSystem?.DayNumber ?? 1,
                CurrentPhase = _gameStateSystem?.CurrentPhase.ToString(),
                CurrentHour = CurrentHour,
                IsPaused = IsPaused,
                TimeScale = TimeScale
            };
        }

        public void RestoreState(object state)
        {
            if (state is DayCycleSaveData saveData)
            {
                CurrentHour = saveData.CurrentHour;
                IsPaused = saveData.IsPaused;
                TimeScale = saveData.TimeScale;
                EvaluatePhaseForHour((int)CurrentHour);
                Debug.Log($"[TimeSystem] Restored Time state: Hour {CurrentHour:00}:00, Paused: {IsPaused}");
            }
        }
    }
}
