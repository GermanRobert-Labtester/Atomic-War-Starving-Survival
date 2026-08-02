using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Event modal presentation dialog driven by EventRunner. Displays event title,
    /// body text, and dynamic choice buttons. Invokes choice resolution on user click.
    /// Event-driven only.
    /// </summary>
    public class EventModalUI : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public GameEvent ActiveEvent { get; private set; }
        public EventContext ActiveContext { get; private set; }

        public event Action<GameEvent, EventChoice> OnChoiceSelected;

        public void Bind(EventRunner runner)
        {
            if (runner != null)
            {
                runner.OnEventTriggered += ShowEvent;
            }
        }

        public void ShowEvent(GameEvent gameEvent, EventContext context)
        {
            if (gameEvent == null) return;
            ActiveEvent = gameEvent;
            ActiveContext = context;
            IsOpen = true;
        }

        public void SelectChoice(int choiceIndex, EventRunner runner = null)
        {
            if (!IsOpen || ActiveEvent == null || ActiveEvent.choices == null) return;
            if (choiceIndex < 0 || choiceIndex >= ActiveEvent.choices.Count) return;

            var choice = ActiveEvent.choices[choiceIndex];
            if (runner != null && ActiveContext != null)
            {
                runner.ApplyChoice(ActiveEvent, choice, ActiveContext);
            }

            OnChoiceSelected?.Invoke(ActiveEvent, choice);
            Close();
        }

        public void Close()
        {
            IsOpen = false;
            ActiveEvent = null;
            ActiveContext = null;
        }
    }
}
