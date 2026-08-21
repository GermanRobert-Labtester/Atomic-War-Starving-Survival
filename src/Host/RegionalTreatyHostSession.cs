using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for RegionalTreatySystem.
    /// Manages diplomatic accords, scrap ratification costs, compliance checks, and violation penalties.
    /// </summary>
    public sealed class RegionalTreatyHostSession
    {
        public RegionalTreatySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public RegionalTreatyHostSession(RegionalTreatySystem system)
        {
            System = system ?? new RegionalTreatySystem(new GodotLog());

            System.OnTreatyStatusChanged += treaty =>
            {
                LastEvent = $"[Treaty] {treaty.treatyId} status changed to {treaty.status} (Score: {treaty.complianceScore:P0})";
                StateChanged?.Invoke();
            };
        }

        public ActionResult ProposeTreaty(string treatyId, int currentDay)
        {
            var res = System.Propose(treatyId);
            if (res.IsSuccess)
            {
                LastEvent = $"Proposed regional treaty: {treatyId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult RatifyTreaty(string treatyId, int scrapCost)
        {
            var res = System.Ratify(treatyId, scrapCost);
            if (res.IsSuccess)
            {
                LastEvent = $"Ratified regional treaty: {treatyId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
