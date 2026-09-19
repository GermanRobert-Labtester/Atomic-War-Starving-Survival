// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.InformationFlow
{
    public enum RumorSubjectType
    {
        Faction = 0,
        Location = 1,
        Event = 2,
        Economy = 3
    }

    [Serializable]
    public sealed class WastelandRumor
    {
        public string RumorId { get; set; } = string.Empty;
        public string OriginLocationId { get; set; } = string.Empty;
        public int OriginDay { get; set; } = 1;
        public RumorSubjectType SubjectType { get; set; } = RumorSubjectType.Faction;
        public string SubjectId { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Truthfulness { get; set; } = 0.85f; // 0.0 (false) to 1.0 (verified)
        public float DecayRate { get; set; } = 0.05f;
        public int PropagationSpeed { get; set; } = 1; // hubs per day
        public bool IsIntercepted { get; set; } = false;
        public List<string> ReachedHubIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class InformationHub
    {
        public string HubId { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public float Credibility { get; set; } = 0.8f;
        public string Bias { get; set; } = "neutral";
    }

    [Serializable]
    public sealed class RumorNetworkState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<WastelandRumor> Rumors { get; set; } = new List<WastelandRumor>();
        public List<InformationHub> Hubs { get; set; } = new List<InformationHub>();
    }

    /// <summary>
    /// Plan 131 — Wasteland Information & Rumor Network.
    /// Simulates information flow and rumor propagation between settlements, caravans,
    /// and radio intercepts, tracking truthfulness degradation, bias, and intelligence value.
    /// </summary>
    public sealed class RumorSystem
    {
        private readonly RumorNetworkState _state;

        public event Action<WastelandRumor>? OnRumorGenerated;
        public event Action<WastelandRumor, string>? OnRumorPropagated;
        public event Action<WastelandRumor>? OnRumorIntercepted;

        public int TotalRumorCount => _state.Rumors.Count;
        public int InterceptedRumorCount => _state.Rumors.Count(r => r.IsIntercepted);
        public int HubCount => _state.Hubs.Count;

        public RumorSystem(RumorNetworkState? state = null)
        {
            _state = state ?? new RumorNetworkState();
        }

        public InformationHub RegisterHub(
            string hubId,
            string name,
            string locationId,
            float credibility = 0.8f,
            string bias = "neutral")
        {
            if (string.IsNullOrWhiteSpace(hubId)) throw new ArgumentNullException(nameof(hubId));

            var hub = _state.Hubs.FirstOrDefault(h => string.Equals(h.HubId, hubId, StringComparison.OrdinalIgnoreCase));
            if (hub == null)
            {
                hub = new InformationHub
                {
                    HubId = hubId.Trim(),
                    HubName = string.IsNullOrWhiteSpace(name) ? hubId : name.Trim(),
                    LocationId = locationId ?? string.Empty,
                    Credibility = Math.Clamp(credibility, 0f, 1f),
                    Bias = bias ?? "neutral"
                };
                _state.Hubs.Add(hub);
            }

            return hub;
        }

        public WastelandRumor GenerateRumor(
            string originLocationId,
            RumorSubjectType subjectType,
            string subjectId,
            string headline,
            string description,
            float truthfulness = 0.8f,
            int currentDay = 1)
        {
            var rumor = new WastelandRumor
            {
                RumorId = $"rmr_{_state.NextSequence++}",
                OriginLocationId = originLocationId ?? string.Empty,
                OriginDay = Math.Max(1, currentDay),
                SubjectType = subjectType,
                SubjectId = subjectId ?? string.Empty,
                Headline = string.IsNullOrWhiteSpace(headline) ? "Wasteland Whisper" : headline.Trim(),
                Description = description ?? string.Empty,
                Truthfulness = Math.Clamp(truthfulness, 0f, 1f),
                DecayRate = 0.05f,
                PropagationSpeed = 1,
                IsIntercepted = false,
                ReachedHubIds = new List<string>()
            };

            // Origin hub immediately reaches if hub exists at origin location
            var originHub = _state.Hubs.FirstOrDefault(h => string.Equals(h.LocationId, originLocationId, StringComparison.OrdinalIgnoreCase));
            if (originHub != null)
            {
                rumor.ReachedHubIds.Add(originHub.HubId);
            }

            _state.Rumors.Add(rumor);
            OnRumorGenerated?.Invoke(rumor);
            return rumor;
        }

        public bool PropagateRumorToHub(string rumorId, string hubId)
        {
            var rumor = _state.Rumors.FirstOrDefault(r => string.Equals(r.RumorId, rumorId, StringComparison.OrdinalIgnoreCase));
            if (rumor == null) return false;

            var hub = _state.Hubs.FirstOrDefault(h => string.Equals(h.HubId, hubId, StringComparison.OrdinalIgnoreCase));
            if (hub == null) return false;

            if (!rumor.ReachedHubIds.Contains(hub.HubId))
            {
                rumor.ReachedHubIds.Add(hub.HubId);

                // Rumor slightly mutates / loses truthfulness as it spreads
                rumor.Truthfulness = Math.Clamp(rumor.Truthfulness * (hub.Credibility * 0.95f), 0.1f, 1f);

                OnRumorPropagated?.Invoke(rumor, hub.HubId);
                return true;
            }

            return false;
        }

        public bool InterceptRumor(string rumorId)
        {
            var rumor = _state.Rumors.FirstOrDefault(r => string.Equals(r.RumorId, rumorId, StringComparison.OrdinalIgnoreCase));
            if (rumor == null) return false;

            if (!rumor.IsIntercepted)
            {
                rumor.IsIntercepted = true;
                OnRumorIntercepted?.Invoke(rumor);
            }

            return true;
        }

        public void TickDay(int currentDay)
        {
            for (int i = _state.Rumors.Count - 1; i >= 0; i--)
            {
                var r = _state.Rumors[i];
                r.Truthfulness = Math.Max(0f, r.Truthfulness - r.DecayRate);

                // Expire stale rumors after 30 days or if truth drops to 0
                if (r.Truthfulness <= 0f || (currentDay - r.OriginDay > 30))
                {
                    _state.Rumors.RemoveAt(i);
                }
            }
        }

        public IReadOnlyList<WastelandRumor> GetRumorsAtLocation(string locationId)
        {
            var hub = _state.Hubs.FirstOrDefault(h => string.Equals(h.LocationId, locationId, StringComparison.OrdinalIgnoreCase));
            if (hub == null) return Array.Empty<WastelandRumor>();

            return _state.Rumors.Where(r => r.ReachedHubIds.Contains(hub.HubId)).ToList();
        }

        public IReadOnlyList<WastelandRumor> GetInterceptedRumors()
        {
            return _state.Rumors.Where(r => r.IsIntercepted).ToList();
        }

        public RumorNetworkState CaptureState()
        {
            var state = new RumorNetworkState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Rumors = new List<WastelandRumor>(_state.Rumors.Count),
                Hubs = new List<InformationHub>(_state.Hubs.Count)
            };

            foreach (var r in _state.Rumors)
            {
                state.Rumors.Add(new WastelandRumor
                {
                    RumorId = r.RumorId,
                    OriginLocationId = r.OriginLocationId,
                    OriginDay = r.OriginDay,
                    SubjectType = r.SubjectType,
                    SubjectId = r.SubjectId,
                    Headline = r.Headline,
                    Description = r.Description,
                    Truthfulness = r.Truthfulness,
                    DecayRate = r.DecayRate,
                    PropagationSpeed = r.PropagationSpeed,
                    IsIntercepted = r.IsIntercepted,
                    ReachedHubIds = new List<string>(r.ReachedHubIds)
                });
            }

            foreach (var h in _state.Hubs)
            {
                state.Hubs.Add(new InformationHub
                {
                    HubId = h.HubId,
                    HubName = h.HubName,
                    LocationId = h.LocationId,
                    Credibility = h.Credibility,
                    Bias = h.Bias
                });
            }

            return state;
        }

        public void RestoreState(RumorNetworkState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Rumors.Clear();
            _state.Hubs.Clear();

            if (state.Rumors != null)
            {
                foreach (var r in state.Rumors)
                {
                    _state.Rumors.Add(new WastelandRumor
                    {
                        RumorId = r.RumorId,
                        OriginLocationId = r.OriginLocationId,
                        OriginDay = r.OriginDay,
                        SubjectType = r.SubjectType,
                        SubjectId = r.SubjectId,
                        Headline = r.Headline,
                        Description = r.Description,
                        Truthfulness = r.Truthfulness,
                        DecayRate = r.DecayRate,
                        PropagationSpeed = r.PropagationSpeed,
                        IsIntercepted = r.IsIntercepted,
                        ReachedHubIds = new List<string>(r.ReachedHubIds ?? Enumerable.Empty<string>())
                    });
                }
            }

            if (state.Hubs != null)
            {
                foreach (var h in state.Hubs)
                {
                    _state.Hubs.Add(new InformationHub
                    {
                        HubId = h.HubId,
                        HubName = h.HubName,
                        LocationId = h.LocationId,
                        Credibility = h.Credibility,
                        Bias = h.Bias
                    });
                }
            }
        }
    }
}
