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
    : HostSessionBase{
        public RegionalTreatySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public RegionalTreatyHostSession(RegionalTreatySystem system)
        {
            System = system ?? new RegionalTreatySystem(new GodotLog());

            System.OnTreatyStatusChanged += treaty =>
            {
                LastEvent = $"[Treaty] {treaty.treatyId} status changed to {treaty.status} (Score: {treaty.complianceScore:P0})";
                RaiseStateChanged();
            };
        }

        public ActionResult ProposeTreaty(string treatyId, int currentDay)
        {
            var res = System.Propose(treatyId);
            if (res.IsSuccess)
            {
                LastEvent = $"Proposed regional treaty: {treatyId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult RatifyTreaty(string treatyId, int scrapCost)
        {
            var res = System.Ratify(treatyId, scrapCost);
            if (res.IsSuccess)
            {
                LastEvent = $"Ratified regional treaty: {treatyId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }
    }
}
