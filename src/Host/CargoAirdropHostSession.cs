// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot adapter for the Core cargo-airdrop authority (Plan 205).
    /// Presentation only — scheduling, tick, and crate collection all route
    /// through Core commands; feedback mirrors Core results.
    /// </summary>
    public sealed class CargoAirdropHostSession : HostSessionBase
    {
        public CargoAirdropSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public CargoAirdropHostSession(CargoAirdropSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnDropScheduled += ev =>
            {
                LastEvent = $"Airdrop {ev.event_id} scheduled — descending.";
                RaiseStateChanged();
            };
            System.OnDropLanded += ev =>
            {
                LastEvent = $"Airdrop {ev.event_id} landed at grid ({ev.landing_x},{ev.landing_y}) — beacon active.";
                RaiseStateChanged();
            };
            System.OnDropRecovered += ev =>
            {
                LastEvent = $"Airdrop {ev.event_id} recovered.";
                RaiseStateChanged();
            };
            System.OnDropIntercepted += ev =>
            {
                LastEvent = $"Airdrop {ev.event_id} LOST — hostiles reached the crate first.";
                RaiseStateChanged();
            };
            System.OnDropExpired += ev =>
            {
                LastEvent = $"Airdrop {ev.event_id} expired — the crate was buried by the elements.";
                RaiseStateChanged();
            };
            System.OnCrateCollected += (ev, itemId, qty) =>
            {
                LastEvent = $"Collected {qty}× cargo from {ev.event_id}.";
                RaiseStateChanged();
            };
        }

        public ActionResult Schedule(string profileId, string signalId, int targetX, int targetY)
        {
            var res = System.ScheduleDrop(profileId, signalId, targetX, targetY);
            if (res.IsFailure) LastEvent = "Drop blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public int Collect(string eventId, Func<string, float, int, bool> tryGrantItem)
        {
            var collected = System.CollectCrate(eventId, tryGrantItem);
            LastEvent = collected > 0 ? $"Claimed {collected} units of cargo." : "Nothing collected — check capacity.";
            RaiseStateChanged();
            return collected;
        }

        public ActionResult ReactivateBeacon(string eventId)
        {
            var res = System.ReactivateBeacon(eventId);
            if (res.IsFailure) LastEvent = "Beacon blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }
    }
}
