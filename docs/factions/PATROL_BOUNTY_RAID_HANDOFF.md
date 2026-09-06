# ASHFALL — Patrol Violation to Faction Bounty Handoff Contract

**Milestone:** Flagship Integration Plan VII (F10)
**Authority:** Engine-agnostic Core (`Assets/Ashfall.Core/Factions/FactionBountySystem.cs`, `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`)
**Status:** Integrated & Verified

---

## 1. Executive Summary

When survivors interact with armed faction patrols across the wasteland, choices resulting in severe disobedience, combat engagement, or breach of territorial mandates trigger immediate formal bounty enforcement through `FactionBountySystem`.

The handoff occurs deterministically within `TravelEncounterSystem.ResolveChoice` whenever an encounter choice specifies an authored standing penalty of `-10` or more severe (`selectedChoice.FactionStandingDelta <= -10`).

---

## 2. Severity Mapping & Thresholds

Bounty severity is calculated strictly from the authored standing impact of the action, independent of standing clamps (such as reaching the minimum standing floor of -100):

| Authored Standing Delta | Severity Tier (`FactionBountySeverity`) | Game Consequence Profile |
|---|---|---|
| `> -10` (e.g., `-1` to `-9`) | `None` | Ordinary faction friction; no formal bounty warrant issued. |
| `-10` to `-14` | `Moderate` | Intermittent scouting harassment, increased border tolls, minor retaliatory patrols. |
| `-15` to `-19` | `Severe` | Active pursuit patrols, trade embargoes, hostile shoot-on-sight posture in faction territory. |
| `<= -20` | `Extreme` | Faction hit squads, coordinated raids on shelter expeditions, immediate inter-faction hostility escalation. |

---

## 3. Provenance & Deduplication Contract

Every bounty record maintains an immutable provenance trail linking the enforcement warrant back to its narrative root:

```csharp
[Serializable]
public sealed class FactionBountyProvenance
{
    public string SourceType { get; set; } = "patrol_violation";
    public string EncounterId { get; set; } = string.Empty;
    public string ChoiceId { get; set; } = string.Empty;
    public string SourceResolutionId { get; set; } = string.Empty; // Format: "{EncounterId}:{ChoiceId}:{Day}"
    public int Day { get; set; }
}
```

### Deduplication Invariant
- **Rule:** Multiple identical resolutions of the same encounter choice on the same day cannot spawn duplicate bounty entries.
- **Mechanism:** `FactionBountySystem.IssuePatrolBounty` checks `_state.Bounties` for an existing entry matching `SourceResolutionId`. If present, the existing record is returned and no duplicate event is dispatched.

---

## 4. Faction Identity & Canonical Alignment

All bounties resolve and store the canonical system faction ID via `FactionStandingIdResolver.ToSystemsId`:
- Authoring ID `warlords_sector_4` maps to canonical `faction_scavenger_warlords`.
- Authoring ID `iron_garrison` maps to canonical `faction_iron_garrison`.
- Inquiries through `HasActiveBounty(factionId)` and `ClearBountiesForFaction(factionId)` accept both display/data aliases and canonical IDs interchangeably.

---

## 5. Lifecycle & Clearance APIs

Bounties transition through two discrete states: `FactionBountyState.Active` and `FactionBountyState.Resolved`.

- `ResolveBounty(string bountyId, int day)`: Resolves a single warrant by unique ID (e.g., upon completing an atonement contract, paying a ransom, or surviving a retribution encounter).
- `ClearBountiesForFaction(string factionId, int day)`: Resolves all active bounties for the specified faction simultaneously (e.g., following a diplomatic summit, peace accord, or major treaty concession).
- `HasActiveBounty(string factionId)`: Queries whether any active warrant exists for the given faction.
- `GetActiveBounties()`: Returns a snapshot list of currently active bounties for UI rendering, radio broadcast alerts, and encounter generation hooks.

---

## 6. Persistence & Determinism

- **Zero Host Coupling:** Built with pure C# DTOs and `System.Text.Json` compatibility.
- **Idempotency:** State capture (`CaptureState`) and restoration (`RestoreState`) preserve all active warrants, historical resolved entries, severity tiers, and provenance trails byte-for-byte.
- **Replay Determinism:** Tested via xUnit seed replay suites (`PatrolBountyHandoffTests`, `PatrolCampaignCrossSystemSmokeTests`).
