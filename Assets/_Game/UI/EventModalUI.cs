using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Event modal presentation dialog driven by EventRunner. Displays event title,
    /// body text (trust-aware), and dynamic choice buttons that react to crew traits,
    /// faction trust, and eventFlags. Invokes choice resolution on user click.
    /// Event-driven only.
    /// </summary>
    public class EventModalUI : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public GameEvent ActiveEvent { get; private set; }
        public EventContext ActiveContext { get; private set; }

        /// <summary>Resolved body (may swap to threatening copy under low trust).</summary>
        public string DisplayBodyText { get; private set; }

        /// <summary>Visible rows only: available + grayed. Hidden gate failures omitted.</summary>
        public IReadOnlyList<PresentedEventChoice> VisibleChoices { get; private set; }
            = Array.Empty<PresentedEventChoice>();

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
            DisplayBodyText = gameEvent.ResolveBodyText(context);
            VisibleChoices = EventRunner.GetVisibleChoices(gameEvent, context);
            IsOpen = true;
        }

        /// <summary>
        /// Select by index into <see cref="VisibleChoices"/> (not raw event.choices).
        /// Grayed-out rows are ignored.
        /// </summary>
        public void SelectChoice(int visibleIndex, EventRunner runner = null)
        {
            if (!IsOpen || ActiveEvent == null || VisibleChoices == null) return;
            if (visibleIndex < 0 || visibleIndex >= VisibleChoices.Count) return;

            var presented = VisibleChoices[visibleIndex];
            if (presented == null || presented.Choice == null) return;
            if (!presented.IsAvailable || presented.IsGrayedOut || presented.IsHidden)
                return;

            var choice = presented.Choice;
            if (runner != null && ActiveContext != null)
            {
                runner.ApplyChoice(ActiveEvent, choice, ActiveContext);
            }

            OnChoiceSelected?.Invoke(ActiveEvent, choice);
            Close();
        }

        /// <summary>Select by choice id among currently available (non-grayed) options.</summary>
        public void SelectChoiceById(string choiceId, EventRunner runner = null)
        {
            if (!IsOpen || ActiveEvent == null || string.IsNullOrEmpty(choiceId)) return;
            var choice = EventRunner.FindAvailableChoice(ActiveEvent, ActiveContext, choiceId);
            if (choice == null) return;

            if (runner != null && ActiveContext != null)
                runner.ApplyChoice(ActiveEvent, choice, ActiveContext);

            OnChoiceSelected?.Invoke(ActiveEvent, choice);
            Close();
        }

        public void Close()
        {
            IsOpen = false;
            ActiveEvent = null;
            ActiveContext = null;
            DisplayBodyText = null;
            VisibleChoices = Array.Empty<PresentedEventChoice>();
        }
    }
}
