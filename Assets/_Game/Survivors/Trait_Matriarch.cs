using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class MatriarchState
    {
        public string traitId = "trait_matriarch";
        public int descendantsRequired = 2;
        public List<string> matriarchIds = new List<string>();
    }

    public class Trait_Matriarch
    {
        public event Action<string> OnMatriarchGained;
        public event Action<string, string> OnDescendantSided;

        private readonly MatriarchState _state;

        public Trait_Matriarch()
        {
            _state = new MatriarchState();
        }

        public bool CheckTrait(string survivorId, int descendantCount)
        {
            if (descendantCount < _state.descendantsRequired)
                return false;

            if (!_state.matriarchIds.Contains(survivorId))
            {
                _state.matriarchIds.Add(survivorId);
                OnMatriarchGained?.Invoke(survivorId);
            }

            return true;
        }

        public bool WillDescendantSide(string descendantId, string matriarchId)
        {
            if (!_state.matriarchIds.Contains(matriarchId))
                return false;

            OnDescendantSided?.Invoke(descendantId, matriarchId);
            return true;
        }

        public bool HasTrait(string survivorId)
        {
            return _state.matriarchIds.Contains(survivorId);
        }

        public MatriarchState CaptureState() => _state;

        public void RestoreState(MatriarchState state)
        {
            _state.traitId = state.traitId;
            _state.descendantsRequired = state.descendantsRequired;
            _state.matriarchIds = new List<string>(state.matriarchIds);
        }
    }
}
