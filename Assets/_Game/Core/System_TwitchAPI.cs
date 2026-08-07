// System_TwitchAPI.cs — Twitch Integration (Prompt #865)
// Hooks into Twitch chat. Viewers vote on weather, spawn raids, drop SupplyCrates.
// Internet = cruel god. Offline-safe stubs (no network) for EditMode + single-player.
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
            poll_id = id ?? string.Empty;
            remaining_seconds = duration;
            total_votes = 0;
            options = new List<string>();
            votes = new List<int>();
            if (opts == null) return;
            for (int i = 0; i < opts.Length; i++)
            {
                options.Add(opts[i] ?? string.Empty);
                votes.Add(0);
            }
        }
    }

    /// <summary>
    /// Twitch Integration system (Prompt #865).
    /// Polls last 60 seconds. Chat commands: !weather blizzard/heatwave, !raid, !supply.
    /// 5-minute cooldown between events. Requires OAuth token (stub accepted offline).
    /// Graceful disconnect if stream ends. No live network in this build — host uses
    /// <see cref="ReceiveVote"/> / <see cref="ProcessChatCommand"/> for simulation.
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
        public const int DefaultPollDurationSeconds = 60;
        public const float EventCooldownSeconds = 300f; // 5 minutes
        public const string OfflineToken = "offline_stub_token";

        public const string OptBlizzard = "blizzard";
        public const string OptHeatwave = "heatwave";
        public const string OptRaid = "raid";
        public const string OptSupply = "supply";

        // ── State ──────────────────────────────────────────────────────
        private TwitchApiState _state = new TwitchApiState();
        private float _secondAccum;

        // ── Public accessors ───────────────────────────────────────────

        public string SystemId =>
            string.IsNullOrEmpty(_state.system_id) ? "system_twitch_api" : _state.system_id;

        public bool IsConnected => _state.is_connected;
        public string ChannelName => _state.channel_name ?? string.Empty;
        public float CooldownSeconds => _state.cooldown_seconds;
        public int ViewerCount => _state.viewer_count;
        public int ActivePollCount =>
            _state.active_polls != null ? _state.active_polls.Count : 0;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Connect to a Twitch channel with an OAuth token.
        /// Offline tests may pass <see cref="OfflineToken"/>.
        /// </summary>
        public void Connect(string channel, string oauthToken)
        {
            if (string.IsNullOrEmpty(channel) || string.IsNullOrEmpty(oauthToken))
                return;

            EnsureLists();
            _state.channel_name = channel;
            _state.is_connected = true;
            _state.cooldown_seconds = 0f;
            if (string.IsNullOrEmpty(_state.system_id))
                _state.system_id = "system_twitch_api";
            OnConnected?.Invoke(channel);
        }

        /// <summary>Convenience offline connect (no real network).</summary>
        public void ConnectOffline(string channel = "ashfall_stream")
        {
            Connect(channel, OfflineToken);
        }

        /// <summary>
        /// Disconnect from Twitch chat. Called gracefully when stream ends.
        /// </summary>
        public void Disconnect()
        {
            if (!_state.is_connected)
                return;

            _state.is_connected = false;
            EnsureLists();
            _state.active_polls.Clear();
            _secondAccum = 0f;
            OnDisconnected?.Invoke();
        }

        public void SetViewerCount(int count)
        {
            _state.viewer_count = count < 0 ? 0 : count;
        }

        /// <summary>
        /// Start a viewer poll. Chat commands like !weather blizzard/heatwave
        /// map to poll options. Polls last 60 seconds by default.
        /// </summary>
        public void StartPoll(string pollId, string[] options, int durationSeconds = 0)
        {
            if (!_state.is_connected)
                return;
            if (string.IsNullOrEmpty(pollId) || options == null || options.Length == 0)
                return;
            if (_state.cooldown_seconds > 0f)
                return;
            if (FindPoll(pollId) != null)
                return;

            EnsureLists();
            int dur = durationSeconds > 0 ? durationSeconds : DefaultPollDurationSeconds;
            _state.active_polls.Add(new TwitchPoll(pollId, options, dur));
        }

        /// <summary>
        /// Tally votes for a specific poll and return the winning option index.
        /// </summary>
        public int TallyVotes(string pollId)
        {
            var poll = FindPoll(pollId);
            if (poll == null || poll.votes == null || poll.votes.Count == 0)
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
            string winningOption = (winnerIndex >= 0 && poll.options != null &&
                                    winnerIndex < poll.options.Count)
                ? poll.options[winnerIndex]
                : string.Empty;

            OnPollClosed?.Invoke(pollId, winningOption, poll.total_votes);

            if (winningOption == OptBlizzard || winningOption == OptHeatwave)
                OnEventSpawned?.Invoke("weather_" + winningOption);
            else if (winningOption == OptRaid)
                OnEventSpawned?.Invoke(OptRaid);
            else if (winningOption == OptSupply)
                OnSupplyDrop?.Invoke(pollId + "_crate");

            EnsureLists();
            _state.active_polls.Remove(poll);
            _state.cooldown_seconds = EventCooldownSeconds;
        }

        public IReadOnlyList<TwitchPoll> GetActivePolls()
        {
            EnsureLists();
            return _state.active_polls.AsReadOnly();
        }

        /// <summary>
        /// Called each second to tick down poll timers and cooldowns.
        /// </summary>
        public void TickSecond()
        {
            if (!_state.is_connected)
                return;

            if (_state.cooldown_seconds > 0f)
            {
                _state.cooldown_seconds -= 1f;
                if (_state.cooldown_seconds < 0f)
                    _state.cooldown_seconds = 0f;
            }

            EnsureLists();
            for (int i = _state.active_polls.Count - 1; i >= 0; i--)
            {
                var poll = _state.active_polls[i];
                if (poll == null)
                {
                    _state.active_polls.RemoveAt(i);
                    continue;
                }

                poll.remaining_seconds -= 1f;
                if (poll.remaining_seconds <= 0f)
                    ExecuteResult(poll.poll_id);
            }
        }

        /// <summary>
        /// Real-time tick: accumulates delta and fires <see cref="TickSecond"/> each second.
        /// Host should call from Update with unscaled delta while connected.
        /// </summary>
        public void Tick(float deltaSeconds)
        {
            if (!_state.is_connected || deltaSeconds <= 0f)
                return;

            _secondAccum += deltaSeconds;
            while (_secondAccum >= 1f)
            {
                _secondAccum -= 1f;
                TickSecond();
            }
        }

        /// <summary>
        /// Simulate receiving a chat vote from a viewer.
        /// </summary>
        public void ReceiveVote(string viewerName, string pollId, string option)
        {
            var poll = FindPoll(pollId);
            if (poll == null || poll.options == null || poll.votes == null)
                return;

            int optIndex = poll.options.IndexOf(option);
            if (optIndex < 0 || optIndex >= poll.votes.Count)
                return;

            poll.votes[optIndex]++;
            poll.total_votes++;
            OnVoteReceived?.Invoke(viewerName ?? string.Empty, pollId, option);
        }

        /// <summary>
        /// Offline-safe chat command parser for host/tests.
        /// Supports: !weather blizzard|heatwave, !raid, !supply.
        /// Starts a short poll if needed and casts the viewer's vote.
        /// </summary>
        public bool ProcessChatCommand(string viewerName, string command, string arg = null)
        {
            if (!_state.is_connected || string.IsNullOrEmpty(command))
                return false;
            if (_state.cooldown_seconds > 0f)
                return false;

            string cmd = command.Trim().ToLowerInvariant();
            if (cmd.StartsWith("!"))
                cmd = cmd.Substring(1);

            if (cmd == "weather")
            {
                string opt = (arg ?? string.Empty).Trim().ToLowerInvariant();
                if (opt != OptBlizzard && opt != OptHeatwave)
                    return false;
                EnsurePoll("weather", new[] { OptBlizzard, OptHeatwave }, 10);
                ReceiveVote(viewerName, "weather", opt);
                return true;
            }

            if (cmd == "raid")
            {
                EnsurePoll("raid", new[] { OptRaid, "pass" }, 10);
                ReceiveVote(viewerName, "raid", OptRaid);
                return true;
            }

            if (cmd == "supply")
            {
                EnsurePoll("supply", new[] { OptSupply, "pass" }, 10);
                ReceiveVote(viewerName, "supply", OptSupply);
                return true;
            }

            return false;
        }

        // ── Internals ──────────────────────────────────────────────────

        private void EnsurePoll(string pollId, string[] options, int durationSeconds)
        {
            if (FindPoll(pollId) != null)
                return;
            StartPoll(pollId, options, durationSeconds);
        }

        private TwitchPoll FindPoll(string pollId)
        {
            if (string.IsNullOrEmpty(pollId) || _state.active_polls == null)
                return null;
            for (int i = 0; i < _state.active_polls.Count; i++)
            {
                var p = _state.active_polls[i];
                if (p != null && p.poll_id == pollId)
                    return p;
            }
            return null;
        }

        private void EnsureLists()
        {
            if (_state.active_polls == null)
                _state.active_polls = new List<TwitchPoll>();
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>Deep snapshot (polls cloned; does not share live lists).</summary>
        public TwitchApiState CaptureState()
        {
            return CloneState(_state);
        }

        public void RestoreState(TwitchApiState state)
        {
            _state = state == null ? new TwitchApiState() : CloneState(state);
            if (string.IsNullOrEmpty(_state.system_id))
                _state.system_id = "system_twitch_api";
            EnsureLists();
            if (_state.cooldown_seconds < 0f)
                _state.cooldown_seconds = 0f;
            if (_state.viewer_count < 0)
                _state.viewer_count = 0;
            _secondAccum = 0f;
        }

        private static TwitchApiState CloneState(TwitchApiState src)
        {
            var cap = new TwitchApiState
            {
                system_id = string.IsNullOrEmpty(src.system_id) ? "system_twitch_api" : src.system_id,
                is_connected = src.is_connected,
                channel_name = src.channel_name ?? string.Empty,
                cooldown_seconds = src.cooldown_seconds < 0f ? 0f : src.cooldown_seconds,
                viewer_count = src.viewer_count < 0 ? 0 : src.viewer_count,
                active_polls = new List<TwitchPoll>()
            };

            if (src.active_polls != null)
            {
                for (int i = 0; i < src.active_polls.Count; i++)
                {
                    if (src.active_polls[i] != null)
                        cap.active_polls.Add(ClonePoll(src.active_polls[i]));
                }
            }

            return cap;
        }

        private static TwitchPoll ClonePoll(TwitchPoll src)
        {
            var p = new TwitchPoll
            {
                poll_id = src.poll_id ?? string.Empty,
                remaining_seconds = src.remaining_seconds,
                total_votes = src.total_votes,
                options = new List<string>(),
                votes = new List<int>()
            };

            if (src.options != null)
            {
                for (int i = 0; i < src.options.Count; i++)
                    p.options.Add(src.options[i] ?? string.Empty);
            }

            if (src.votes != null)
            {
                for (int i = 0; i < src.votes.Count; i++)
                    p.votes.Add(src.votes[i]);
            }

            // Keep lists aligned.
            while (p.votes.Count < p.options.Count)
                p.votes.Add(0);
            while (p.options.Count < p.votes.Count)
                p.options.Add(string.Empty);

            return p;
        }
    }
}
