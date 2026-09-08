# Crossing Faction and Crisis Matrix

## Faction authority

`Assets/StreamingAssets/Data/crossing_factions.json` currently defines three
active Crossing factions. No new factions are added by Plan 115.

| Faction ID | Display name | Alignment | Wants | Offers | Plan 115 use |
|---|---|---|---|---|---|
| `faction_the_scale` | The Scale | conditional | `trade_goods` | `stallrow_trade_access`, `verification` | implied institutional authority in manifests, tolls, and records |
| `faction_the_underwrite` | The Underwrite | conditional | `pledged_goods` | `seed_stock`, `covered_loss`, `favour_bank` | implied debt, collateral, and concession pressure |
| `faction_the_compact` | The Compact | peaceful | `signatories` | `charter_draft`, `ratification` | implied charter, petition, sponsorship, and admission pressure |

The encounter and crisis DTOs contain no faction ID field, standing delta,
alignment effect, or faction outcome field. Faction references therefore
remain prose-level institutional context only.

## Existing political mechanics

- `CrossingArbitrationSystem` owns Standing topics, backers, honest/rigged
  rulings, bribes, overturns, and save state.
- `CrossingQuestSystem` owns Crossing quest choice flags and quest progress.
- `VouchAccessSystem` owns access to `loc_crossing_*` travel.
- The encounter/crisis catalog is loaded by `CrossingCatalog`; it is not
  dispatched into those systems by `CrossingSession`.

## Crisis coverage

| Crisis ID | Institutional question | Phases | Intended existing route | Direct data hook |
|---|---|---|---|---|
| `crisis_the_forfeit` | Can pledged grain be seized? | Notice → Terms Read → Broker Attempt → Resolution | forfeit / arbitration | none |
| `crisis_the_vote` | Can the draft be ratified? | Call → Canvas → Interference → Count | vote/quest path | none |
| `crisis_the_standing_contested` | Can a ruling be challenged? | Call → Backer Recruitment → Rival Recruitment → Verdict | arbitration | none |
| `crisis_the_charter_found` | What does the founding record say? | Request → Read → Verify → Decide | quest/charter path | none |
| `crisis_who_holds_the_ledger` | Who speaks for the Crossing? | Summons → Final Backing → Quorum Ruling → Aftermath | arbitration/ending quest | none |
| `crisis_the_water_claim` | Can maintenance become ownership? | Claim → Counter-Petition → Arbitration → Ruling | arbitration | none |
| `crisis_the_grain_riot` | Who controls shortage distribution? | Shortage → Hoarding Accusation → Distribution → Calm or Collapse | vote/forfeit | none |
| `crisis_the_charter_amendment` | Who receives charter protection? | Proposal → Debate → Vote → Enactment | vote | none |
| `crisis_the_debt_forgiveness` | Can emergency debt be erased? | Demand → Counteroffer → Assembly → Verdict | vote/arbitration | none |
| `crisis_the_refugee_admission` | Who may become a resident? | Arrival → Vetting → Resource Test → Admission or Refusal | vote/arbitration | none |
| `crisis_the_arbitrator_bribe` | Can the judge remain trusted? | Accusation → Evidence → Hearing → Replacement or Vindication | arbitration/backers | none |
| `crisis_the_quarantine_break` | Can rights survive containment? | Break → Exposure Trace → Containment → Reconciliation | existing political path | none |

## Deferred cross-plan integrations

Plans 98, 76, 102, 112, and 89 have no representable direct field in the
active DTO. This pass does not invent standing, expedition reveal, treaty,
disease, or epilogue outcome fields.
