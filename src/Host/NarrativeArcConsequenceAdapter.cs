using System;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Small Godot-host adapter for Plan 143. Main supplies lazy delegates to
    /// the existing authorities; this class contains no gameplay rules.
    /// </summary>
    public sealed class NarrativeArcConsequenceAdapter : INarrativeArcConsequencePort
    {
        public Func<string, int, bool, (bool ok, string reason)> MoralePreflight { get; set; } =
            (_, _, _) => (false, "morale authority is not bound");
        public Action<string, int, bool> MoraleCommit { get; set; } = (_, _, _) => { };

        public Func<string, (bool ok, string reason)> IntelPreflight { get; set; } =
            _ => (false, "journal knowledge authority is not bound");
        public Action<string> IntelCommit { get; set; } = _ => { };

        public Func<string, (bool ok, string reason)> ExpeditionPreflight { get; set; } =
            _ => (false, "expedition authority is not bound");
        public Action<string> ExpeditionCommit { get; set; } = _ => { };

        public Func<string, int, (bool ok, string reason)> StandingPreflight { get; set; } =
            (_, _) => (false, "faction standing authority is not bound");
        public Action<string, int> StandingCommit { get; set; } = (_, _) => { };

        public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
        {
            var check = MoralePreflight(survivorId, delta, shelterWide);
            reason = check.reason;
            return check.ok;
        }

        public void ApplyMorale(string survivorId, int delta, bool shelterWide)
            => MoraleCommit(survivorId, delta, shelterWide);

        public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
        {
            var check = IntelPreflight(canonicalFactionId);
            reason = check.reason;
            return check.ok;
        }

        public void GrantFactionIntel(string canonicalFactionId) => IntelCommit(canonicalFactionId);

        public bool CanOfferExpedition(string locationId, out string reason)
        {
            var check = ExpeditionPreflight(locationId);
            reason = check.reason;
            return check.ok;
        }

        public void OfferExpedition(string locationId) => ExpeditionCommit(locationId);

        public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
        {
            var check = StandingPreflight(canonicalFactionId, delta);
            reason = check.reason;
            return check.ok;
        }

        public void ApplyFactionStanding(string canonicalFactionId, int delta)
            => StandingCommit(canonicalFactionId, delta);
    }
}
