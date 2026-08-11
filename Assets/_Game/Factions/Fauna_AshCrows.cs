using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class Fauna_AshCrowsState
    {
        public string id = "fauna_ashcrows";
        public string displayName = "AshCrows";
        public bool isPresent = false;
        public int count = 0;
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — Fauna
    /// archetype `Fauna_AshCrows`. Behaviour is host-injected; the data
    /// (presence + count) is persisted here.
    /// </summary>
    public class Fauna_AshCrows
    {
        private Fauna_AshCrowsState _state = new Fauna_AshCrowsState();

        public event Action<Fauna_AshCrowsState> OnSighted;
        public event Action<Fauna_AshCrowsState> OnCountChanged;

        public Fauna_AshCrowsState State => _state;

        public void Sighted(int count) { _state.isPresent = true; _state.count = count; OnSighted?.Invoke(_state); }
        public void SetCount(int count) { _state.count = count; OnCountChanged?.Invoke(_state); }
        public void Clear() { _state.isPresent = false; _state.count = 0; }

        public Fauna_AshCrowsState CaptureState() => _state;
        public void RestoreState(Fauna_AshCrowsState saved) { _state = saved ?? new Fauna_AshCrowsState(); }
    }
}
