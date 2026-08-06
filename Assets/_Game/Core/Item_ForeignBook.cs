using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ForeignBookState
    {
        public string itemId = "item_foreign_book";
        public string displayName = "Foreign Language Book";
        public bool isTranslated = false;
        public int translationDaysRequired = 10;
        public int daysSpentTranslating = 0;
        public float intelligenceThreshold = 60f;
        public int intelNodesYielded = 5;
    }

    /// <summary>
    /// Prompt #614: Item: Foreign Language Book.
    /// Found on dead foreign paratroopers. Unreadable initially.
    /// High Intelligence survivor spends 10 days translating. Yields massive IntelNodes.
    /// </summary>
    public class Item_ForeignBook
    {
        private ForeignBookState _state = new ForeignBookState();

        public event Action<ForeignBookState, float> OnTranslationStarted;
        public event Action<ForeignBookState> OnTranslationCompleted;
        public event Action<ForeignBookState, int> OnIntelExtracted;

        public ForeignBookState State => _state;

        public bool StartTranslation(float survivorIntelligence)
        {
            if (survivorIntelligence < _state.intelligenceThreshold || _state.isTranslated)
                return false;

            OnTranslationStarted?.Invoke(_state, survivorIntelligence);
            return true;
        }

        public void TickDay()
        {
            if (_state.isTranslated)
                return;

            _state.daysSpentTranslating++;

            if (_state.daysSpentTranslating >= _state.translationDaysRequired)
            {
                _state.isTranslated = true;

                OnTranslationCompleted?.Invoke(_state);
                OnIntelExtracted?.Invoke(_state, _state.intelNodesYielded);
            }
        }

        public bool IsTranslated()
        {
            return _state.isTranslated;
        }

        public int GetIntelYield()
        {
            return _state.isTranslated ? _state.intelNodesYielded : 0;
        }
    }
}
