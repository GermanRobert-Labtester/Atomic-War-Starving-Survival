# Plan 115 Crisis Coverage Matrix

## Baseline reconciliation

The plan brief stated five existing crises, which is correct. The encounter
portion of the same brief was stale: the live repository had 14 existing
encounters rather than 10. All five existing crises remain unchanged.

## Existing crises

| Crisis | Main issue | Existing phase grammar | Resolution prose | Runtime owner |
|---|---|---|---|---|
| `crisis_the_forfeit` | pledged granary debt | Notice → Terms Read → Broker Attempt → Resolution | debt honoured or renegotiated through arbitration | catalog prose plus related quest/arbitration systems |
| `crisis_the_vote` | Draft Four ratification | Call → Canvas → Interference → Count | clean or contested majority ratification | catalog prose plus related quest path |
| `crisis_the_standing_contested` | challenged arbitration | Call → Backer Recruitment → Rival Recruitment → Verdict | ruling holds or is overturned through verified weights | `CrossingArbitrationSystem` for related topics |
| `crisis_the_charter_found` | recovered founding pages | Request → Read → Verify → Decide | truth revealed to the three blocs | catalog prose plus charter quest |
| `crisis_who_holds_the_ledger` | final institutional control | Summons → Final Backing → Quorum Ruling → Aftermath | one canonical faction resolution | related endgame quest and arbitration |

## Added crises

| Crisis | Main issue | Phases | Existing route named in data |
|---|---|---|---|
| `crisis_the_water_claim` | common water versus maintenance ownership | Claim → Counter-Petition → Arbitration → Ruling | arbitration |
| `crisis_the_grain_riot` | shortage, hoarding, and distribution | Shortage → Hoarding Accusation → Distribution → Calm or Collapse | vote / forfeit |
| `crisis_the_charter_amendment` | narrowing charter protection | Proposal → Debate → Vote → Enactment | vote |
| `crisis_the_debt_forgiveness` | collective debt relief versus future credit | Demand → Counteroffer → Assembly → Verdict | vote / arbitration |
| `crisis_the_refugee_admission` | admission versus capacity | Arrival → Vetting → Resource Test → Admission or Refusal | vote / arbitration |
| `crisis_the_arbitrator_bribe` | legitimacy of neutral judgment | Accusation → Evidence → Hearing → Replacement or Vindication | arbitration / backers |
| `crisis_the_quarantine_break` | containment versus civic membership | Break → Exposure Trace → Containment → Reconciliation | existing political path |

## Semantics boundary

Every added crisis has 4 ordered labels, matching the existing content
grammar. They are authored escalation descriptions, not new phase tokens.
`CrossingSession` has no crisis state or phase-advance API, and the
`resolution` field is prose rather than a condition or action identifier.
No crisis adds a faction delta, flag, item result, treaty state, disease ID,
or ending state.
