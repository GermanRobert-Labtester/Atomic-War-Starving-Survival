using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Event_EuthanasiaPactState
    {
        public string eventId = "euthanasia_pact";
        public float warningHours = 12f;
        public float hoursRemaining = 0f;
        public string survivor1Id = string.Empty;
        public string survivor2Id = string.Empty;
        public bool isPactActive = false;
        public bool isExecuted = false;
    }

    public class Event_EuthanasiaPact
    {
        public Event_EuthanasiaPactState State { get; private set; }

        public event Action<string, string, float> OnPactFormed;
        public event Action<string, string, float> OnPactTick;
        public event Action<string, string> OnPactExecuted;
        public event Action<string, string> OnPactCancelled;

        public Event_EuthanasiaPact()
        {
            State = new Event_EuthanasiaPactState();
        }

        public Event_EuthanasiaPact(Event_EuthanasiaPactState state)
        {
            State = state ?? new Event_EuthanasiaPactState();
        }

        public void FormPact(string id1, string id2)
        {
            if (string.IsNullOrEmpty(id1) || string.IsNullOrEmpty(id2))
            {
                return;
            }

            if (State.isPactActive)
            {
                return;
            }

            State.survivor1Id = id1;
            State.survivor2Id = id2;
            State.isPactActive = true;
            State.isExecuted = false;
            State.hoursRemaining = State.warningHours;

            OnPactFormed?.Invoke(id1, id2, State.warningHours);
        }

        public void TickHour(int currentFood)
        {
            if (!State.isPactActive || State.isExecuted)
            {
                return;
            }

            State.hoursRemaining--;

            OnPactTick?.Invoke(State.survivor1Id, State.survivor2Id, State.hoursRemaining);

            if (currentFood > 0)
            {
                CancelPact();
                return;
            }

            if (State.hoursRemaining <= 0f)
            {
                ExecutePact();
            }
        }

        public void ExecutePact()
        {
            if (!State.isPactActive || State.isExecuted)
            {
                return;
            }

            State.isExecuted = true;
            State.isPactActive = false;
            State.hoursRemaining = 0f;

            OnPactExecuted?.Invoke(State.survivor1Id, State.survivor2Id);
        }

        public void CancelPact()
        {
            if (!State.isPactActive)
            {
                return;
            }

            State.isPactActive = false;
            State.hoursRemaining = 0f;

            OnPactCancelled?.Invoke(State.survivor1Id, State.survivor2Id);
        }

        public bool IsPactActive()
        {
            return State.isPactActive;
        }

        public float GetHoursRemaining()
        {
            return State.hoursRemaining;
        }

        public bool IsExecuted()
        {
            return State.isExecuted;
        }

        public Event_EuthanasiaPactState CaptureState()
        {
            return State;
        }

        public void RestoreState(Event_EuthanasiaPactState saved)
        {
            State = saved ?? new Event_EuthanasiaPactState();
        }
    }
}
