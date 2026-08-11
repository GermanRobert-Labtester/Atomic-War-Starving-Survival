using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class Fauna_BloatedCattleState
    {
        public string id = "fauna_bloatedcattle";
        public string displayName = "BloatedCattle";
        public bool isPresent = false;
        public int count = 0;
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — Fauna
    /// archetype `Fauna_BloatedCattle`. Behaviour is host-injected; the data
    /// (presence + count) is persisted here.
    /// </summary>
    public class Fauna_BloatedCattle
    {
        private Fauna_BloatedCattleState _state = new Fauna_BloatedCattleState();

        public event Action<Fauna_BloatedCattleState> OnSighted;
        public event Action<Fauna_BloatedCattleState> OnCountChanged;

        public Fauna_BloatedCattleState State => _state;

        public void Sighted(int count) { _state.isPresent = true; _state.count = count; OnSighted?.Invoke(_state); }
        public void SetCount(int count) { _state.count = count; OnCountChanged?.Invoke(_state); }
        public void Clear() { _state.isPresent = false; _state.count = 0; }

        public Fauna_BloatedCattleState CaptureState() => _state;
        public void RestoreState(Fauna_BloatedCattleState saved) { _state = saved ?? new Fauna_BloatedCattleState(); }
    }
}
