using System;
using System.Collections.Generic;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #20 — The Lifeboat Transmission: late-game two-way contact with a
    /// sealed facility that can take exactly one more person. Player chooses who
    /// leaves; the rest die; campaign ends as bittersweet partial victory
    /// (<see cref="EndgameConditionKind.LifeboatPartialExtraction"/>).
    /// Mutually exclusive with full <see cref="EndgameConditionKind.RescueExtractionSuccess"/>.
    /// </summary>
    public class LifeboatTransmissionSystem
    {
        /// <summary>Earliest day the contact can fire.</summary>
        public const int MinContactDay = 80;

        public const string EventId = "evt_lifeboat_transmission";
        public const string ChoiceIdPrefix = "send_";
        public const string FlagContacted = "lifeboat_contacted";
        public const string FlagResolved = "lifeboat_resolved";

        private Func<int> _getDay;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<bool> _isCampaignTerminal;
        private EndgameEngine _endgame;
        private VictoryProjectManager _victory;

        private bool _contacted;
        private bool _offered;
        private bool _resolved;
        private string _extractedSurvivorId = string.Empty;
        private string _extractedSurvivorName = string.Empty;
        private readonly List<string> _leftBehindIds = new List<string>();
        private readonly List<string> _leftBehindNames = new List<string>();

        public bool HasContacted => _contacted;
        public bool HasOffered => _offered;
        public bool IsResolved => _resolved;
        public string ExtractedSurvivorId => _extractedSurvivorId;
        public string ExtractedSurvivorName => _extractedSurvivorName;
        public IReadOnlyList<string> LeftBehindIds => _leftBehindIds;
        public IReadOnlyList<string> LeftBehindNames => _leftBehindNames;

        public event Action<GameEvent> OnContactOffered;
        public event Action<string, IReadOnlyList<string>> OnLifeboatResolved;
        public event Action OnStateChanged;
        /// <summary>day, description, survivorName — for MoralChronicleBridge.</summary>
        public event Action<int, string, string> OnMoralRecord;

        public void Bind(
            Func<int> getDay = null,
            Func<IReadOnlyList<Survivor>> getSurvivors = null,
            Func<bool> isCampaignTerminal = null,
            EndgameEngine endgame = null,
            VictoryProjectManager victory = null)
        {
            _getDay = getDay ?? (() => 0);
            _getSurvivors = getSurvivors;
            _isCampaignTerminal = isCampaignTerminal ?? (() => false);
            _endgame = endgame;
            _victory = victory;
        }

        public int CurrentDay => _getDay != null ? _getDay() : 0;

        /// <summary>
        /// True when day ≥ <see cref="MinContactDay"/>, at least one living survivor,
        /// not yet offered/resolved, and campaign is not already terminal.
        /// </summary>
        public bool CanOfferContact(int day = -1, IReadOnlyList<Survivor> survivors = null)
        {
            if (_resolved || _offered) return false;
            if (_isCampaignTerminal != null && _isCampaignTerminal()) return false;
            if (_endgame != null && _endgame.Result.IsTerminal) return false;
            if (_victory != null && _victory.IsTerminal) return false;

            int d = day >= 0 ? day : CurrentDay;
            if (d < MinContactDay) return false;

            survivors = survivors ?? _getSurvivors?.Invoke();
            return CountLiving(survivors) >= 1;
        }

        /// <summary>
        /// Daily tick: offer the lifeboat contact once when eligible.
        /// </summary>
        public GameEvent TickDay(int day, IReadOnlyList<Survivor> survivors = null)
        {
            if (!CanOfferContact(day, survivors)) return null;
            return OfferContact(survivors, day);
        }

        /// <summary>Force-offer the contact event (tests / scripted). Respects terminal gates.</summary>
        public GameEvent OfferContact(IReadOnlyList<Survivor> survivors = null, int day = -1)
        {
            if (_resolved || _offered) return null;
            if (_isCampaignTerminal != null && _isCampaignTerminal()) return null;
            if (_endgame != null && _endgame.Result.IsTerminal) return null;
            if (_victory != null && _victory.IsTerminal) return null;

            survivors = survivors ?? _getSurvivors?.Invoke();
            if (CountLiving(survivors) < 1) return null;

            int d = day >= 0 ? day : CurrentDay;
            var ev = CreateContactEvent(survivors, d);
            if (ev == null) return null;

            _offered = true;
            _contacted = true;
            OnContactOffered?.Invoke(ev);
            OnStateChanged?.Invoke();
            return ev;
        }

        public GameEvent CreateContactEvent(IReadOnlyList<Survivor> survivors, int day = -1)
        {
            if (survivors == null) return null;
            var living = new List<Survivor>();
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive)
                    living.Add(survivors[i]);
            }
            if (living.Count == 0) return null;

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EventId;
            ev.title = "The Lifeboat";
            ev.bodyText =
                "Two-way contact. A sealed facility — scrubbers, rations, a living ecosystem — " +
                "has room for one more body. One seat. The rest of the bunker is not invited. " +
                "Someone walks into the ash. Everyone else stays under concrete.";
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = MinContactDay };
            ev.choices = new List<EventChoice>(living.Count);
            for (int i = 0; i < living.Count; i++)
            {
                var s = living[i];
                string name = string.IsNullOrEmpty(s.DisplayName) ? s.Id : s.DisplayName;
                ev.choices.Add(new EventChoice
                {
                    ChoiceId = ChoiceIdPrefix + s.Id,
                    Text = $"Send {name}.",
                    MoraleDelta = -8f
                });
            }
            return ev;
        }

        public static string ExtractSurvivorIdFromChoice(string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) return string.Empty;
            if (!choiceId.StartsWith(ChoiceIdPrefix, StringComparison.Ordinal)) return string.Empty;
            return choiceId.Substring(ChoiceIdPrefix.Length);
        }

        public bool ApplyChoiceFromEvent(GameEvent gameEvent, EventChoice choice, EventContext flags = null)
        {
            if (gameEvent == null || choice == null) return false;
            if (!string.Equals(gameEvent.id, EventId, StringComparison.Ordinal)) return false;
            return ResolveSend(ExtractSurvivorIdFromChoice(choice.ChoiceId), flags);
        }

        /// <summary>
        /// Send <paramref name="survivorId"/> out; mark all other living survivors dead;
        /// fire Lifeboat endgame on engine + victory project.
        /// </summary>
        public bool ResolveSend(string survivorId, EventContext flags = null)
        {
            if (_resolved) return false;
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (_isCampaignTerminal != null && _isCampaignTerminal()) return false;
            if (_endgame != null && _endgame.Result.IsTerminal) return false;
            if (_victory != null && _victory.IsTerminal) return false;

            var survivors = _getSurvivors?.Invoke();
            if (survivors == null) return false;

            Survivor chosen = null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (string.Equals(s.Id, survivorId, StringComparison.Ordinal))
                {
                    chosen = s;
                    break;
                }
            }
            if (chosen == null) return false;

            _leftBehindIds.Clear();
            _leftBehindNames.Clear();
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (ReferenceEquals(s, chosen)) continue;
                s.State = SurvivorState.Dead;
                _leftBehindIds.Add(s.Id ?? string.Empty);
                _leftBehindNames.Add(string.IsNullOrEmpty(s.DisplayName) ? s.Id : s.DisplayName);
            }

            _extractedSurvivorId = chosen.Id ?? string.Empty;
            _extractedSurvivorName = string.IsNullOrEmpty(chosen.DisplayName)
                ? chosen.Id
                : chosen.DisplayName;
            // Chosen remains alive — they left, they lived.
            chosen.State = SurvivorState.Idle;

            _resolved = true;
            _contacted = true;
            _offered = true;

            flags?.SetEventFlag(FlagContacted, true);
            flags?.SetEventFlag(FlagResolved, true);

            int day = CurrentDay;
            int left = _leftBehindIds.Count;

            _endgame?.ApplyLifeboatPartialExtraction(day, _extractedSurvivorName, left);
            _victory?.ApplyLifeboat(_extractedSurvivorName, left, day, survivors);

            string chronicle =
                left <= 0
                    ? $"We sent {_extractedSurvivorName} into the ash. The channel closed."
                    : $"We sent {_extractedSurvivorName}. We left {left} behind.";
            OnMoralRecord?.Invoke(day, chronicle, _extractedSurvivorName);
            for (int i = 0; i < _leftBehindNames.Count; i++)
            {
                OnMoralRecord?.Invoke(
                    day,
                    $"{_leftBehindNames[i]} stayed under concrete.",
                    _leftBehindNames[i]);
            }

            OnLifeboatResolved?.Invoke(_extractedSurvivorId, _leftBehindIds);
            OnStateChanged?.Invoke();
            return true;
        }

        public LifeboatTransmissionSave CaptureState()
        {
            return new LifeboatTransmissionSave
            {
                Contacted = _contacted,
                Offered = _offered,
                Resolved = _resolved,
                ExtractedSurvivorId = _extractedSurvivorId,
                ExtractedSurvivorName = _extractedSurvivorName,
                LeftBehindIds = _leftBehindIds.ToArray(),
                LeftBehindNames = _leftBehindNames.ToArray()
            };
        }

        public void RestoreState(LifeboatTransmissionSave save)
        {
            _contacted = false;
            _offered = false;
            _resolved = false;
            _extractedSurvivorId = string.Empty;
            _extractedSurvivorName = string.Empty;
            _leftBehindIds.Clear();
            _leftBehindNames.Clear();
            if (save == null) return;
            _contacted = save.Contacted;
            _offered = save.Offered;
            _resolved = save.Resolved;
            _extractedSurvivorId = save.ExtractedSurvivorId ?? string.Empty;
            _extractedSurvivorName = save.ExtractedSurvivorName ?? string.Empty;
            if (save.LeftBehindIds != null)
            {
                for (int i = 0; i < save.LeftBehindIds.Length; i++)
                    _leftBehindIds.Add(save.LeftBehindIds[i] ?? string.Empty);
            }
            if (save.LeftBehindNames != null)
            {
                for (int i = 0; i < save.LeftBehindNames.Length; i++)
                    _leftBehindNames.Add(save.LeftBehindNames[i] ?? string.Empty);
            }
        }

        public void Clear()
        {
            RestoreState(null);
        }

        public static int CountLiving(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 0;
            int n = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive) n++;
            }
            return n;
        }
    }

    [Serializable]
    public class LifeboatTransmissionSave
    {
        public bool Contacted;
        public bool Offered;
        public bool Resolved;
        public string ExtractedSurvivorId;
        public string ExtractedSurvivorName;
        public string[] LeftBehindIds;
        public string[] LeftBehindNames;
    }
}
