using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Shelter Degradation (Spec #2 of Section VIII). The bunker is a dying
    /// machine. Concrete cracks admit radon; hatch seals lose 2 % per day
    /// after Day 20; wiring shorts rise after Day 40; pipes corrode into
    /// pinhole leaks. Each subsystem has a 0..1 integrity value that the
    /// host UI subscribes to.
    /// </summary>
    public class ShelterDegradationSystem
    {
        public enum Subsystem { Concrete, HatchSeal, Wiring, Pipes }

        [Serializable]
        public class State
        {
            public float ConcreteIntegrity = 1f;
            public float HatchSealIntegrity = 1f;
            public float WiringIntegrity = 1f;
            public float PipesIntegrity = 1f;
            public int LastSimulatedDay = 0;
            public int ActivePipeLeaks = 0;
            public int AccumulatedShortCircuits = 0;
        }

        private State _state = new State();

        public State Current => _state;
        public event Action<Subsystem, float> OnIntegrityChanged;
        public event Action<Subsystem, string> OnFailureEvent;
        public event Action<Subsystem> OnRepaired;

        public Func<float> GetDay;
        public Func<float> GetShelterNoiseLevel;
        public Action<string, float> RequestConsumeItem;
        public Action<string, float> AddWaterLossPerDay;
        public Action<string, float, float> QueueFireRisk;
        public System.Random Rng;

        public void Tick()
        {
            if (GetDay == null) return;
            int day = Mathf.FloorToInt(GetDay());
            if (day == _state.LastSimulatedDay) return;
            int delta = Mathf.Max(1, day - _state.LastSimulatedDay);
            _state.LastSimulatedDay = day;

            if (day >= 30)
            {
                float crackRate = 0.005f * delta;
                _state.ConcreteIntegrity = Mathf.Clamp01(_state.ConcreteIntegrity - crackRate);
                if (_state.ConcreteIntegrity < 0.95f) OnIntegrityChanged?.Invoke(Subsystem.Concrete, _state.ConcreteIntegrity);
                if (_state.ConcreteIntegrity < 0.40f) OnFailureEvent?.Invoke(Subsystem.Concrete, "evt_radon_intrusion");
            }
            if (day >= 20)
            {
                float decay = 0.02f * delta;
                _state.HatchSealIntegrity = Mathf.Clamp01(_state.HatchSealIntegrity - decay);
                OnIntegrityChanged?.Invoke(Subsystem.HatchSeal, _state.HatchSealIntegrity);
                if (_state.HatchSealIntegrity <= 0.50f && _state.HatchSealIntegrity > 0.45f)
                    OnFailureEvent?.Invoke(Subsystem.HatchSeal, "evt_hatch_whistle");
                else if (_state.HatchSealIntegrity <= 0.30f && _state.HatchSealIntegrity > 0.25f)
                    OnFailureEvent?.Invoke(Subsystem.HatchSeal, "evt_fallout_air_leak");
                else if (_state.HatchSealIntegrity <= 0.10f)
                    OnFailureEvent?.Invoke(Subsystem.HatchSeal, "evt_seal_failed");
            }
            if (day >= 40)
            {
                int weeks = (day - 40) / 7;
                float shortChance = 0.05f + 0.05f * weeks;
                if (Roll(shortChance * delta))
                {
                    _state.AccumulatedShortCircuits++;
                    OnFailureEvent?.Invoke(Subsystem.Wiring, "evt_short_circuit");
                    QueueFireRisk?.Invoke("wiring_short", 0.10f, 1f + weeks * 0.05f);
                }
                _state.WiringIntegrity = Mathf.Clamp01(1f - 0.01f * (day - 40));
                OnIntegrityChanged?.Invoke(Subsystem.Wiring, _state.WiringIntegrity);
            }
            if (day >= 15)
            {
                int ageDays = day - 15;
                int expectedLeaks = ageDays / 20;
                while (_state.ActivePipeLeaks < expectedLeaks)
                {
                    _state.ActivePipeLeaks++;
                    AddWaterLossPerDay?.Invoke("pipe_leak", 0.5f);
                }
                _state.PipesIntegrity = Mathf.Clamp01(1f - 0.02f * ageDays);
                OnIntegrityChanged?.Invoke(Subsystem.Pipes, _state.PipesIntegrity);
            }
        }

        public bool RepairConcrete()
        {
            if (_state.ConcreteIntegrity >= 0.99f) return false;
            if (!ConsumeItem("concrete_patch_mix", 1)) return false;
            _state.ConcreteIntegrity = Mathf.Clamp01(_state.ConcreteIntegrity + 0.25f);
            OnRepaired?.Invoke(Subsystem.Concrete);
            OnIntegrityChanged?.Invoke(Subsystem.Concrete, _state.ConcreteIntegrity);
            return true;
        }

        public bool RepairHatchSeal()
        {
            if (_state.HatchSealIntegrity >= 0.99f) return false;
            if (!ConsumeItem("rubber_gasket", 1)) return false;
            _state.HatchSealIntegrity = Mathf.Clamp01(_state.HatchSealIntegrity + 0.20f);
            OnRepaired?.Invoke(Subsystem.HatchSeal);
            OnIntegrityChanged?.Invoke(Subsystem.HatchSeal, _state.HatchSealIntegrity);
            return true;
        }

        /// <summary>
        /// Effect-driven hatch seal patch for the repair_gasket recipe: the
        /// recipe already consumed its own gasket ingredients at craft start
        /// and produces no item, so its hatch_seal_integrity effect lands here
        /// without charging the inventory a second time.
        /// </summary>
        public bool ApplyHatchSealPatch(float amount)
        {
            if (amount <= 0f || _state.HatchSealIntegrity >= 0.99f) return false;
            _state.HatchSealIntegrity = Mathf.Clamp01(_state.HatchSealIntegrity + amount);
            OnRepaired?.Invoke(Subsystem.HatchSeal);
            OnIntegrityChanged?.Invoke(Subsystem.HatchSeal, _state.HatchSealIntegrity);
            return true;
        }

        public bool RepairPipe()
        {
            if (_state.ActivePipeLeaks <= 0) return false;
            if (!ConsumeItem("concrete_patch_mix", 1)) return false;
            _state.ActivePipeLeaks = Mathf.Max(0, _state.ActivePipeLeaks - 1);
            AddWaterLossPerDay?.Invoke("pipe_leak", -0.5f);
            _state.PipesIntegrity = Mathf.Clamp01(_state.PipesIntegrity + 0.10f);
            OnRepaired?.Invoke(Subsystem.Pipes);
            OnIntegrityChanged?.Invoke(Subsystem.Pipes, _state.PipesIntegrity);
            return true;
        }

        public bool RepairWiring()
        {
            if (_state.WiringIntegrity >= 0.99f) return false;
            if (!ConsumeItem("insulation_tape", 1)) return false;
            _state.WiringIntegrity = Mathf.Clamp01(_state.WiringIntegrity + 0.15f);
            OnRepaired?.Invoke(Subsystem.Wiring);
            OnIntegrityChanged?.Invoke(Subsystem.Wiring, _state.WiringIntegrity);
            return true;
        }

        public State CaptureState() => _state;

        public void RestoreState(State s)
        {
            _state = s ?? new State();
        }

        private bool ConsumeItem(string itemId, float count)
        {
            if (RequestConsumeItem == null) return false;
            RequestConsumeItem.Invoke(itemId, count);
            return true;
        }

        private bool Roll(float chance)
        {
            if (chance <= 0f) return false;
            if (Rng == null) Rng = new System.Random();
            return Rng.NextDouble() < chance;
        }
    }
}
