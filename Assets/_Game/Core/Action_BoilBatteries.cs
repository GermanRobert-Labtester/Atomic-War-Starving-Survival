using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BoilBatteriesSave
    {
        public List<string> reconditionedBatteryIds = new List<string>();
    }

    [Serializable]
    public class BoilBatteriesState
    {
        public string actionId = "action_boil_batteries";
        public float chargeRestored = 0.2f;
        public bool canOnlyDoOnce = true;
    }

    public class Action_BoilBatteries
    {
        public event Action<string, float> OnBatteryReconditioned;
        public event Action<string> OnToxicGasGenerated;

        private BoilBatteriesState _state;
        private Dictionary<string, bool> _reconditionedBatteries = new Dictionary<string, bool>();

        public Action_BoilBatteries()
        {
            _state = new BoilBatteriesState();
        }

        public Action_BoilBatteries(BoilBatteriesState state)
        {
            _state = state ?? new BoilBatteriesState();
        }

        public BoilBatteriesState CaptureState() => _state;

        public void RestoreState(BoilBatteriesState state)
        {
            _state = state ?? new BoilBatteriesState();
        }

        public BoilBatteriesSave CaptureSave()
        {
            var save = new BoilBatteriesSave();
            foreach (var kvp in _reconditionedBatteries)
            {
                if (kvp.Value)
                    save.reconditionedBatteryIds.Add(kvp.Key);
            }
            return save;
        }

        public void RestoreSave(BoilBatteriesSave save)
        {
            _reconditionedBatteries.Clear();
            if (save == null) return;
            foreach (var id in save.reconditionedBatteryIds)
            {
                _reconditionedBatteries[id] = true;
            }
        }

        public bool BoilBattery(string survivorId, string batteryId, string roomId)
        {
            if (_state.canOnlyDoOnce && _reconditionedBatteries.ContainsKey(batteryId) && _reconditionedBatteries[batteryId])
                return false;

            _reconditionedBatteries[batteryId] = true;

            OnBatteryReconditioned?.Invoke(batteryId, _state.chargeRestored);
            OnToxicGasGenerated?.Invoke(roomId);

            return true;
        }
    }
}
