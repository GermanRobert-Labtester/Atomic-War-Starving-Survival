using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class Fauna_IrradiatedDogsState
    {
        public string id = "fauna_irradiated_dogs";
        public string displayName = "IrradiatedDogs";
        public bool isPresent = false;
        public int count = 0;
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — Fauna
    /// archetype `Fauna_IrradiatedDogs`. Behaviour is host-injected; the data
    /// (presence + count) is persisted here.
    /// </summary>
    public class Fauna_IrradiatedDogs
    {
        private Fauna_IrradiatedDogsState _state = new Fauna_IrradiatedDogsState();

        public event Action<Fauna_IrradiatedDogsState> OnSighted;
        public event Action<Fauna_IrradiatedDogsState> OnCountChanged;

        public Fauna_IrradiatedDogsState State => _state;

        public void Sighted(int count) { _state.isPresent = true; _state.count = count; OnSighted?.Invoke(_state); }
        public void SetCount(int count) { _state.count = count; OnCountChanged?.Invoke(_state); }
        public void Clear() { _state.isPresent = false; _state.count = 0; }

        public Fauna_IrradiatedDogsState CaptureState() => _state;
        public void RestoreState(Fauna_IrradiatedDogsState saved) { _state = saved ?? new Fauna_IrradiatedDogsState(); }
    }
}
