// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RumorNetworkHostSession
// Core System  : Ashfall.Core.InformationFlow.RumorSystem
// Host Caller  : Main.RumorNetwork
// Purpose      : Plan 203 / 131 — Coordinates wasteland rumors, information hubs,
//                intelligence propagation, and radio intercepts.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.InformationFlow;

namespace AtomicWar.GodotApp
{
    public sealed class RumorNetworkHostSession
    {
        private readonly RumorSystem _system;

        public RumorSystem System => _system;

        public event Action? StateChanged;

        public int TotalRumorCount => _system.TotalRumorCount;
        public int InterceptedRumorCount => _system.InterceptedRumorCount;
        public int HubCount => _system.HubCount;
        public IReadOnlyList<WastelandRumor> Rumors => _system.Rumors;
        public IReadOnlyList<InformationHub> Hubs => _system.Hubs;

        public RumorNetworkHostSession(RumorSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            // Initialize standard hubs if network is fresh
            if (_system.HubCount == 0)
            {
                _system.RegisterHub("hub_crossroads", "Crossroads Trading Post", "loc_crossroads", 0.85f, "mercantile");
                _system.RegisterHub("hub_rust_oasis", "Rust Oasis Waystation", "loc_oasis", 0.75f, "settler");
                _system.RegisterHub("hub_radio_relay", "North Peak Radio Relay", "loc_relay", 0.90f, "scientific");
                _system.RegisterHub("hub_old_railhead", "Old Railhead Terminus", "loc_railhead", 0.70f, "scavenger");
            }

            _system.OnRumorGenerated += _ => StateChanged?.Invoke();
            _system.OnRumorPropagated += (_, _) => StateChanged?.Invoke();
            _system.OnRumorIntercepted += _ => StateChanged?.Invoke();
            _system.OnStateChanged += () => StateChanged?.Invoke();
        }

        public InformationHub RegisterHub(string hubId, string name, string locationId, float credibility = 0.8f, string bias = "neutral")
        {
            var hub = _system.RegisterHub(hubId, name, locationId, credibility, bias);
            StateChanged?.Invoke();
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
            var rumor = _system.GenerateRumor(originLocationId, subjectType, subjectId, headline, description, truthfulness, currentDay);
            StateChanged?.Invoke();
            return rumor;
        }

        public bool PropagateRumorToHub(string rumorId, string hubId)
        {
            bool ok = _system.PropagateRumorToHub(rumorId, hubId);
            if (ok) StateChanged?.Invoke();
            return ok;
        }

        public bool InterceptRumor(string rumorId)
        {
            bool ok = _system.InterceptRumor(rumorId);
            if (ok) StateChanged?.Invoke();
            return ok;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            StateChanged?.Invoke();
        }

        public IReadOnlyList<WastelandRumor> GetRumorsAtLocation(string locationId)
        {
            return _system.GetRumorsAtLocation(locationId);
        }

        public IReadOnlyList<WastelandRumor> GetInterceptedRumors()
        {
            return _system.GetInterceptedRumors();
        }

        public RumorNetworkState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(RumorNetworkState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
