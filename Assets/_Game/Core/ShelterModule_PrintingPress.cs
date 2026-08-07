using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PrintingPressState
    {
        public string moduleId = "module_printing_press";
        public bool isBuilt = false;
        public float forgeryDetectionChance = 0.05f;
        public int useCount = 0;
        public bool bloodFeudTriggered = false;
    }

    /// <summary>
    /// Prompt #659: Shelter Module: Printing Press.
    /// Forge PreWarMoney. Warlords accept at first. Hidden 5% increasing detection
    /// chance each use. Detection → execute trader + BloodFeud.
    /// </summary>
    public class ShelterModule_PrintingPress
    {
        private PrintingPressState _state = new PrintingPressState();

        public event Action<PrintingPressState> OnPressBuilt;
        public event Action<PrintingPressState, int> OnMoneyForged;
        public event Action<PrintingPressState> OnForgeryDetected;
        public event Action<PrintingPressState> OnBloodFeudTriggered;

        public PrintingPressState State => _state;

        public bool Build()
        {
            if (_state.isBuilt) return false;

            _state.isBuilt = true;
            OnPressBuilt?.Invoke(_state);
            return true;
        }

        public (int money, bool detected) Forge(int amount, System.Random rng)
        {
            if (!_state.isBuilt || _state.bloodFeudTriggered)
                return (0, false);

            _state.useCount++;
            float currentDetectionChance = _state.forgeryDetectionChance * _state.useCount;

            bool detected = rng != null && (float)rng.NextDouble() < currentDetectionChance;

            if (detected)
            {
                _state.bloodFeudTriggered = true;
                OnForgeryDetected?.Invoke(_state);
                OnBloodFeudTriggered?.Invoke(_state);
                return (0, true);
            }

            OnMoneyForged?.Invoke(_state, amount);
            return (amount, false);
        }
    
        public PrintingPressState CaptureState()
        {
            return _state;
        }

        public void RestoreState(PrintingPressState saved)
        {
            _state = saved ?? new PrintingPressState();
        }
    }
}

