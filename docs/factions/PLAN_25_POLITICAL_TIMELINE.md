# Plan 25 Political Timeline (Plan 25 · 25C.19)

> Anchored to **repository campaign pacing** (plan §11: "Do not hardcode example dates from this plan").
> Verified 2026-09-01. The campaign calendar (`Assets/Ashfall.Core/Campaign/CampaignCalendar.cs`) has no day cap; day 360 is the epilogue-matrix view, not a gate.

## 1. Canonical anchors (immutable, other plans own them)

| Anchor | Day | Owner |
|---|---:|---|
| Muster questlines open (rate card, cold count, unsigned order, long walk, second winter, color ledger) | 180–200 | `MusterSystem.PopulateFoundingCatalog` |
| Faction-war daily friction begins (`SimulateDailyFriction` no-op ≤ 240) | 240 | `FactionWarSystem.cs:87` |
| THE MUSTER opens (`MusterOpeningDay`) | **260** | `MusterSystem.cs:222` (canon, untouched) |
| Coalition camp can form | 260 | `CoalitionCampSystem.Form` gate |
| Year of Ash phases end (Great Thaw) | 360 | `YearOfAshTimelineSystem` |
| Epilogue matrix view | 360 | `MusterHostSession`/`muster_epilogues.json` |
| Faction-war chain: cold_war band | 480–498 | `faction_war_events.json` |
| open_conflict band | 503–528 | 〃 |
| the_offensive band | 533–560 | 〃 |
| culmination band + ceasefire by exhaustion | 565–605 (ceasefire 588) | 〃 |

**Key continuity fact:** the authored hot-war arc (480–605) postdates the Muster opening (260). Plan 25's "war → weariness → Muster" narrative order is therefore re-sequenced to: *grievances → escalation → Muster (260) → hot war (480+) → ceasefire → testimony/epilogue consequence*. Recorded here per plan §13.10 (continuity outranks content count).

## 2. Plan 25 event placement

| Event | Phase | Earliest | Latest | Treaty state | War state | Conflicts / notes |
|---|---|---:|---:|---|---|---|
| A1 Guild Salvage Claim | peacetime | 60 | — | — | peace (<240 tension ramp) | none |
| A2 Guild Arbitration | peacetime | 80 | — | — | peace | after A1 preferred, not required |
| A3 Hydro Purification Toll | peacetime | 70 | — | — | peace | none |
| A4 Hydro Emergency Water Appeal | peacetime | 100 | — | any | peace | cooler after A3 resolved |
| A5 Raider Parley | peacetime | 90 | — | — | peace | not while raid in progress |
| A6 Raider Passage Demand | escalation | 200 | 470 | any | peace/cold_war | repeatable w/ cooldown |
| A7 Coalition Mediation Request | escalation | 210 | — | intact/strained | peace | none |
| A8 Coalition Shared-Supply Appeal | escalation | 220 | — | any | peace | strengthens Muster attendance flag |
| E-P1 The Marked Ruin | escalation | 200 | 320 | intact/strained | peace | consumes guild grievance |
| E-P2 The Stopped Convoy | escalation | 220 | 340 | strained | peace | after any grievance |
| E-P3 Bitter Water | escalation | 230 | 350 | strained | peace | truth uncertain by design |
| E-P4 The Empty Chair | escalation | 240 | 300 | strained/breached | peace, tension >20 | after ≥1 grievance |
| E-P5 Cistern Toll | escalation | 210 | 330 | strained | peace | consumes hydro grievance |
| E-P6 Prisoner at the Gate | escalation | 220 | 340 | any | peace | consumes raider grievance |
| E-W1 Refugees From the Line | war | 503 | — | any | open_conflict, after `evt_d509_border_clash_span44` | chain-gated |
| E-W2 Requisition | war | 509 | — | any | open_conflict | chain-gated |
| E-W3 Broken Route | war | 522 | — | any | open_conflict | after `evt_d522_switchback_toll` |
| E-W4 Field Hospital Overflow | war | 533 | — | any | the_offensive | after `evt_d533_*` |
| E-W5 Deserter Column | war | 545 | — | any | the_offensive | none |
| E-W6 Retaliation | war | 552 | — | any | the_offensive, after `evt_d552_rebuilders_fracture` | none |
| E-R1 No More Volunteers | weariness | 565 | — | any | culmination | none |
| E-R2 Bread Before Bullets | weariness | 570 | — | any | culmination | after ≥1 E-W resolved |
| E-R3 The Quiet Faction | weariness | 576 | — | any | culmination | produces `flag_peace_faction_forms` |
| E-R4 Refusal at Dawn | weariness | 583 | — | any | culmination, pre-ceasefire | latest 587 (ceasefire 588 owns the end) |
| Muster invite / gathering | endgame | 260 | — | varies | pre-hot-war (usually) | path evaluated here; re-evaluated post-588 in extended campaigns |

Day windows above are authored as chain `minDay` values (no maxDay exists in the schema — sequencing via trigger prerequisites and band order).

## 3. Rules

1. Weariness events never resolve the war; `evt_d588_ceasefire_by_exhaustion` stays the sole war terminator (06C canon).
2. Escalation events produce grievance/escalation flags only; they never set war outcomes (plan §13.2).
3. Faction-alive requirements: events referencing a faction check its Muster system state (guild/hydro/raider active flags; camp formed) before surfacing.
4. Mid-war events reference real 06C stages via `ChainResolvedTrigger`; if a referenced battle chain is absent from a campaign, the mid-war event never fires (by design, not dead data — the trigger is the consumer).
