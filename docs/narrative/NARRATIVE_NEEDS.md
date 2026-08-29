# Narrative Needs — Faction War Arc (Days 480–600+)

Code requirements surfaced while authoring the six `faction_war_*.json` catalogs
and the three new `locations.json` entries for the Day 480–600 "Faction War"
narrative pass. Nothing below has been implemented. All six catalogs are
content-only and pass the data-integrity gate on their own; none of them are
loaded or surfaced by any running code yet.

**Second pass addendum:** a follow-up expansion added a new faction
(`faction_forward_roster`), three more locations, eight more item-lore
entries, three more event chains (days 570/583/605), branching aftermath
stages for the day-541/545 plaza chain, and further radio/journal/
communiqué/dialogue entries.

**Third pass addendum:** a further expansion, authored by six parallel
subagent writers each scoped to a single catalog (to avoid concurrent-edit
conflicts), added two more event chains (days 488/522), four more radio
broadcasts, three more journal entries (including two new named civilian
voices — a gardener at `loc_the_allotments`, an Exchange weigher), four more
dialogue snippets, four more item-lore entries, two more `ambient_addendum`
location overrides, and two more communiqués. All content was cross-checked
for id collisions and reference validity before merging, and the full data-
integrity gate was re-run afterward (PASS, 0 errors). Nothing in the third
pass changes the schema or the code requirements below — it's the same six
catalogs, more densely filled in. Current content totals: 22 event chains /
45 stages / 93 choices, 29 radio broadcasts, 22 journal entries, 18
communiqués, 18 dialogue snippets, 9 location overrides, 105 locations, 499
items.

## 1. Host-session wiring for the new catalogs

None of the following files are read by any C# code today (confirmed by
grepping `src/` and `Assets/Ashfall.Core` for each filename literal):

- `faction_war_events.json`
- `faction_war_radio.json`
- `faction_war_journal.json`
- `faction_war_communiques.json`
- `faction_war_dialogue.json`
- `faction_war_location_overrides.json`

**Requirement:** a `FactionWarHostSession` (or equivalent), modeled on the
existing `YearOfAshHostSession` pattern, that loads all six catalogs at
startup and exposes day-gated queries the Godot host can poll each simulated
day (e.g. "which radio broadcasts are due", "which journal entries have
unlocked", "which location override is active for `locationId` X today").

## 2. New event-chain schema — `faction_war_events.json`

**This is a new schema, not a reuse of `events.json` or
`narrative_arc_events.json`.** Flagging explicitly per the no-schema-changes-
without-flagging rule. `events.json` is a flat weighted random-draw pool
(`id/title/bodyText/weight/minDay`) with no branching and no chains, and
`narrative_arc_events.json` is background-item trigger tables — neither fits
"linked multi-day chains with trigger conditions and branching outcomes."
The new file introduces:

```
{ "chains": [ { "chainId", "band", "title", "factionsInvolved": [faction ids],
    "locationId", "stages": [ { "stageId", "minDay", "triggerCondition"
    (free text — see below), "title", "bodyText",
    "choices": [ { "choiceId", "text", "moraleDelta", "leadsToStageId" } ] } ] } ] }
```

**Requirement:** a `FactionWarChainRunner` that:
- Tracks which `stageId` is "current" per chain (starts at each chain's first
  stage, advances via a stage's chosen `leadsToStageId`, or automatically
  after `minDay` when a stage has no choices / a single non-branching choice).
- Evaluates `triggerCondition` — currently authored as human-readable prose
  ("Fires automatically once the player has visited X", "Fires N days after
  stage Y"), not a machine condition. **Needs a real boolean grammar**
  (day offset from a prior stage; player-visited-location flag; prior-chain-
  resolved flag) before this can drive anything other than a human reading
  the JSON. The prose was deliberately kept close to what such a grammar
  would need to express, to minimize rework.
- Applies `moraleDelta` on choice selection (mirrors the existing
  `narrative_encounters.json` choice/moraleDelta pattern already wired to
  bunker morale — reuse that path if it exists).

## 3. Location-description override mechanism

**Requirement:** a `LocationOverrideResolver` that, given a `locationId` and
the current simulated day, checks `faction_war_location_overrides.json` for
an entry where `activeFromDay <= day` (and `day <= activeUntilDay` when that
field is present) and substitutes `displayName`/`description` in place of the
base `locations.json` entry for display purposes only — the base entry must
remain the source of truth for `dangerLevel`/`travelHours`/`baseRadsPerHour`
unless a future pass explicitly adds rad/danger deltas for post-strike states
(none are included in this pass; the prose implies higher local rads
post-strike but the mechanical fields were deliberately left in the base
`locations.json` entries only, to avoid balancing changes outside this
narrative pass's scope).

Three `overrideType` values are used and need distinct handling:
- `pre_strike` — bounded window (`activeFromDay`..`activeUntilDay`), foreshadowing text only.
- `post_strike` — open-ended (`activeFromDay` onward, no end), permanent aftermath text.
- `ambient_addendum` — open-ended, a minor flavor addition rather than a full description replacement (currently authored as a full replacement string for simplicity; the resolver could instead append it to the base description if that reads better in practice).

This is also the mechanism a future "shelled" world-state flag should hang
off of: raising a flag like `flag_ration_plaza_struck` on the day a strike
chain resolves, with the override resolver keying off the flag rather than a
raw day comparison, would let player agency (e.g. preventing a strike) change
which override applies. No such flag currently exists or is set by anything.

## 4. Evacuation-window trigger type — RESOLVED (content-side)

`evt_d541_evacuation_window_plaza` now has real per-choice branching: each of
its three `s1` choices (warn / loot / stay clear) sets a distinct
`leadsToStageId` pointing at its own consequence stage —
`evt_d541_evacuation_window_plaza_s2_warned`,  `_s2_looted`, and `_s2_silent`
respectively — each with prose specific to that path, at `minDay: 543`. All
three are terminal (empty `choices` array) and converge back into the shared
`evt_d545_ration_plaza_strike` chain, whose `s1` `triggerCondition` was
updated to state explicitly that it fires regardless of which `s2` variant
resolved (the strike itself is not preventable by the day-541 choice; only
the player's foreknowledge/guilt framing differs). This is option (a) from
the original note: the existing chain-runner schema (per-choice
`leadsToStageId` fan-out) was reused rather than adding a second event type.
**Still required on the code side:** the `FactionWarChainRunner` from item 2
must correctly resolve a chain whose stages fan out to different next-stage
ids per choice rather than a single linear `leadsToStageId` — the JSON now
exercises that fan-out shape, so it's a good first test case once the runner
exists.

## 5. `FactionWarSystem.cs` day range and roster

`Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` is explicitly commented
"Days 180 to 360" but `SimulateDailyFriction(int day)` has no coded upper
bound — it will keep clashing `faction_central_garrison` vs
`faction_rebuilders` every 15 days indefinitely, which happens to still be
directionally correct for this pass (those two are the arc's primary
belligerents) but was not verified against the day-480-600 beat sheet this
pass authored, and its `totalArtilleryStrikesLogged`/
`territorialControlPercent` drift is not connected to any of the six new
narrative catalogs or the shelling timeline below.

**Open design questions for whoever wires this up:**
- Should `faction_ash_sign` (now defined in `faction_lore.json`, previously
  only a bare string in this file's default roster) get simulated behavior
  tied to the shrine-strike anomaly (`evt_d578_shrine_strike_anomaly`), or
  stay a narrative-only faction with no `FactionWarSystem` participation?
- **RESOLVED:** the unnamed Rebuilders splinter introduced in
  `evt_d552_rebuilders_fracture` (day 552) now has a real `faction_id`:
  `faction_forward_roster`, added to `faction_lore.json` with a full entry
  (origin story, dialogue style, relationships, tribute/tech fields). It has
  its own location (`loc_forward_roster_camp`), its own radio identity
  ("Forward Roster Checkpoint Wire", 71.500 MHz), and three event chains —
  `evt_d570_forward_roster_first_action` (first independent toll action),
  `evt_d583_d9_reassessment` (D/9 debates whether the Roster meets its denial
  threshold), and `evt_d605_post_ceasefire_forward_roster` (post-ceasefire
  status still unresolved — a Garrison patrol and the checkpoint reach an
  informal, un-treatied stand-off). **Still needs a roster slot in
  `FactionWarSystem.cs`** if it should participate in the simulated daily
  friction loop rather than staying event-chain-only; nothing in this pass
  added it to that system's C# roster.
- `faction_hydro_barons` is already defined in `currents.json` but scoped to
  a different region (`home_region: "the_coast"`, the Currents/Expansion 06
  module). This pass deliberately avoided using that id for the Terrace
  Pumphouse water-leverage plot (`evt_d565_hydro_leverage_break`) to prevent
  a cross-region contradiction, routing the plot through a named NPC
  (Barrow Fennick) and the Rebuilders/Garrison factions instead. If a
  district-local water faction is wanted for future passes, it needs its own
  `faction_id` distinct from the Currents module's hydro-barons.

## 6. Shelling timeline (for the code owner's reference)

| Day | Location | Type |
|-----|----------|------|
| 495 | Open ground near `loc_railway_span_44_alpha` (unnamed, not the span itself) | first unexplained/"clean" strike |
| 517 | `loc_st_brigids_almshouse` | first shelling of a minor location |
| 545 | `loc_ration_queue_plaza` | known-location strike (the mandated player-recognizes-this-place beat) |
| 578 | `loc_ash_sign_shrine` | pattern-breaking strike (mystery partial-reveal engine) |

Only these four are backed by `faction_war_location_overrides.json` entries.
