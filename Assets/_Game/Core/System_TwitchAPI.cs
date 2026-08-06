// System_TwitchAPI.cs — Twitch Integration (Prompt #865)
// Hooks into Twitch chat. Viewers vote on weather, spawn raids, drop SupplyCrates.
// Internet = cruel god.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Twitch API system (Prompt #865).
    /// Tracks connection, active polls, cooldowns, and viewer count.
    /// </summary>
    [Serializable]
    public class TwitchApiState
    {
        public string system_id = "system_twitch_api";
        public bool is_connected;
        public string channel_name = string.Empty;
        public List<TwitchPoll> active_polls = new List<TwitchPoll>();
        public float cooldown_seconds;
        public int viewer_count;
    }

    /// <summary>
    /// A single Twitch chat poll with options and vote tallies.
    /// </summary>
    [Serializable]
    public class TwitchPoll
    {
        public string poll_id;
        public List<string> options = new List<string>();
        public List<int> votes = new List<int>();
        public float remaining_seconds;
        public int total_votes;

        public TwitchPoll() { }

        public TwitchPoll(string id, string[] opts, int duration)
        {
            poll_id = id;
            remaining_seconds = duration;
            total_votes = 0;
            for (int i = 0; i < opts.Length; i++)
            {
                options.Add(opts[i]);
                votes.Add(0);
            }
        }
    }

    /// <summary>
    /// Twitch Integration system (Prompt #865).
    /// Polls last 60 seconds. Chat commands: !weather blizzard/heatwave, !raid, !supply.
    /// 5-minute cooldown between events. Requires OAuth token.
    /// Graceful disconnect if stream ends.
    /// </summary>
    public class System_TwitchAPI
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnConnected;
        public event Action OnDisconnected;
        public event Action<string, string, string> OnVoteReceived;
        public event Action<string, string, int> OnPollClosed;
        public event Action<string> OnEventSpawned;
        public event Action<string> OnSupplyDrop;

        // ── Constants ──────────────────────────────────────────────────
        private const int DefaultPollDuration = 60;
        private const float EventCooldown = 300f; // 5 minutes

        // ── State ──────────────────────────────────────────────────────
        private TwitchApiState _state = new TwitchApiState();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Connect to a Twitch channel with an OAuth token.
        /// </summary>
        public void Connect(string channel, string oauthToken)
        {
            if (string.IsNullOrEmpty(channel) || string.IsNullOrEmpty(oauthToken))
                return;

            _state.channel_name = channel;
            _state.is_connected = true;
            _state.cooldown_seconds = 0f;
            OnConnected?.Invoke(channel);
        }

        /// <summary>
        /// Disconnect from Twitch chat. Called gracefully when stream ends.
        /// </summary>
        public void Disconnect()
        {
            if (!_state.is_connected)
                return;

            _state.is_connected = false;
            _state.active_polls.Clear();
            OnDisconnected?.Invoke();
        }

        /// <summary>
        /// Start a viewer poll. Chat commands like !weather blizzard/heatwave
        /// map to poll options. Polls last 60 seconds by default.
        /// </summary>
        public void StartPoll(string pollId, string[] options, int durationSeconds)
        {
            if (!_state.is_connected)
                return;

            if (_state.cooldown_seconds > 0f)
                return;

            int dur = durationSeconds > 0 ? durationSeconds : DefaultPollDuration;
            var poll = new TwitchPoll(pollId, options, dur);
            _state.active_polls.Add(poll);
        }

        /// <summary>
        /// Tally votes for a specific poll and return the winning option index.
        /// </summary>
        public int TallyVotes(string pollId)
        {
            var poll = FindPoll(pollId);
            if (poll == null || poll.votes.Count == 0)
                return -1;

            int bestIndex = 0;
            for (int i = 1; i < poll.votes.Count; i++)
            {
                if (poll.votes[i] > poll.votes[bestIndex])
                    bestIndex = i;
            }
            return bestIndex;
        }

        /// <summary>
        /// Close a poll, fire events, and execute the winning result.
        /// Applies cooldown after execution.
        /// </summary>
        public void ExecuteResult(string pollId)
        {
            var poll = FindPoll(pollId);
            if (poll == null)
                return;

            int winnerIndex = TallyVotes(pollId);
            string winningOption = (winnerIndex >= 0 && winnerIndex < poll.options.Count)
                ? poll.options[winnerIndex]
                : string.Empty;

            OnPollClosed?.Invoke(pollId, winningOption, poll.total_votes);

            // Spawn event based on winning option
            if (winningOption == "blizzard" || winningOption == "heatwave")
            {
                OnEventSpawned?.Invoke("weather_" + winningOption);
            }
            else if (winningOption == "raid")
            {
                OnEventSpawned?.Invoke("raid");
            }
            else if (winningOption == "supply")
            {
                OnSupplyDrop?.Invoke(pollId + "_crate");
            }

            // Remove poll and start cooldown
            _state.active_polls.Remove(poll);
            _state.cooldown_seconds = EventCooldown;
        }

        /// <summary>
        /// Returns all currently active polls.
        /// </summary>
        public IReadOnlyList<TwitchPoll> GetActivePolls()
        {
            return _state.active_polls.AsReadOnly();
        }

        /// <summary>
        /// Called each second to tick down poll timers and cooldowns.
        /// </summary>
        public void TickSecond()
        {
            if (!_state.is_connected)
                return;

            // Tick cooldown
            if (_state.cooldown_seconds > 0f)
                _state.cooldown_seconds -= 1f;

            // Tick polls
            for (int i = _state.active_polls.Count - 1; i >= 0; i--)
            {
                _state.active_polls[i].remaining_seconds -= 1f;
                if (_state.active_polls[i].remaining_seconds <= 0f)
                {
                    ExecuteResult(_state.active_polls[i].poll_id);
                }
            }
        }

        /// <summary>
        /// Simulate receiving a chat vote from a viewer.
        /// </summary>
        public void ReceiveVote(string viewerName, string pollId, string option)
        {
            var poll = FindPoll(pollId);
            if (poll == null)
                return;

            int optIndex = poll.options.IndexOf(option);
            if (optIndex < 0)
                return;

            poll.votes[optIndex]++;
            poll.total_votes++;
            OnVoteReceived?.Invoke(viewerName, pollId, option);
        }

        // ── Internals ──────────────────────────────────────────────────

        private TwitchPoll FindPoll(string pollId)
        {
            for (int i = 0; i < _state.active_polls.Count; i++)
            {
                if (_state.active_polls[i].poll_id == pollId)
                    return _state.active_polls[i];
            }
            return null;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TwitchApiState CaptureState()
        {
            return _state;
        }

        public void RestoreState(TwitchApiState state)
        {
            _state = state ?? new TwitchApiState();
        }
    }
}
