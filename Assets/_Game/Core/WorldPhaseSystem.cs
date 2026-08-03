using System;
using AtomicWar._Game.Data;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Drives the campaign's top-level WorldPhase from TimeSystem.OnDayTick. The
    /// Civil War -> Flashpoint edge fires OnNuclearExchange exactly once, guarded by
    /// HasTriggeredExchange for save/load safety (no replay on restore). Flashpoint
    /// and NuclearWinter side effects are collapsed into that single cascade — the
    /// day after, CurrentPhase advances to NuclearWinter as a pure label change.
    /// </summary>
    public class WorldPhaseSystem
    {
        private const int DefaultFlashpointDay = 30;
        private const float DefaultExchangeMoraleHit = 25f;

        public WorldPhase CurrentPhase { get; private set; } = WorldPhase.CivilWar;
        public bool HasTriggeredExchange { get; private set; }

        public int FlashpointDay { get; }
        public float ExchangeMoraleHit { get; }

        /// <summary>Fired whenever CurrentPhase changes.</summary>
        public event Action<WorldPhase> OnPhaseChanged;

        /// <summary>Fired exactly once, on the day the atomic exchange happens.</summary>
        public event Action OnNuclearExchange;

        public WorldPhaseSystem(WorldPhaseConfigSO config = null)
        {
            FlashpointDay = config != null ? config.flashpointDay : DefaultFlashpointDay;
            ExchangeMoraleHit = config != null ? config.exchangeMoraleHit : DefaultExchangeMoraleHit;
        }

        /// <summary>Subscribe to TimeSystem.OnDayTick.</summary>
        public void OnDayTick(int day)
        {
            WorldPhase next;
            if (day < FlashpointDay)
            {
                next = WorldPhase.CivilWar;
            }
            else if (day == FlashpointDay)
            {
                next = WorldPhase.Flashpoint;
            }
            else
            {
                next = WorldPhase.NuclearWinter;
            }

            SetPhase(next);

            if (day >= FlashpointDay && !HasTriggeredExchange)
            {
                HasTriggeredExchange = true;
                OnNuclearExchange?.Invoke();
            }
        }

        private void SetPhase(WorldPhase next)
        {
            if (next == CurrentPhase) return;
            CurrentPhase = next;
            OnPhaseChanged?.Invoke(next);
        }

        public WorldPhaseSave CaptureState()
        {
            return new WorldPhaseSave
            {
                CurrentPhase = CurrentPhase,
                HasTriggeredExchange = HasTriggeredExchange
            };
        }

        /// <summary>Restores state directly; does not replay OnNuclearExchange.</summary>
        public void RestoreState(WorldPhaseSave save)
        {
            if (save == null) return;
            CurrentPhase = save.CurrentPhase;
            HasTriggeredExchange = save.HasTriggeredExchange;
        }
    }

    [Serializable]
    public class WorldPhaseSave
    {
        public WorldPhase CurrentPhase;
        public bool HasTriggeredExchange;
    }
}
