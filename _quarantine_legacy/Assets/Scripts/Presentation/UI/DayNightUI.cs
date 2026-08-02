using AtomicWar.Core.Events;
using AtomicWar.Core.Services;
using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Time;
using UnityEngine;

namespace AtomicWar.Presentation.UI
{
    /// <summary>
    /// Example lightweight UI binding MonoBehaviour.
    /// Subscribes to pure C# domain events via EventBus.
    /// </summary>
    public class DayNightUI : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus.Subscribe<PhaseChangedEvent>(OnPhaseChanged);
            EventBus.Subscribe<HourTickEvent>(OnHourTick);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PhaseChangedEvent>(OnPhaseChanged);
            EventBus.Unsubscribe<HourTickEvent>(OnHourTick);
        }

        private void OnPhaseChanged(PhaseChangedEvent e)
        {
            Debug.Log($"[UI View] Updated display: Day {e.DayNumber} | Phase: {e.NewPhase}");
        }

        private void OnHourTick(HourTickEvent e)
        {
            var timeSystem = ServiceLocator.Get<TimeSystem>();
            string timeOfDay = timeSystem.IsDaytime ? "Day" : "Night";
            Debug.Log($"[UI View] Time Update: {e.CurrentHour:00}:00 ({timeOfDay})");
        }
    }
}
