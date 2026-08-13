# ASHFALL — Lore Bible

> Status: brainstorm for review. Nothing here is in the game yet.
> Every entry carries a snake_case id and a target JSON file so it can be
> converted to data on approval.

## What this is

`docs/superpowers/specs/2026-08-12-ashfall-massive-content-expansion-design.md`
set targets — *+50 locations, +32 survivors, +50 dilemmas* — but contained no
actual content, and named factions that do not exist in this repo. This bible
supplies the substance and writes against real canon.

**Corrections carried in from that spec:**

| Spec claimed | Reality |
|---|---|
| `events.json` uses `consequence_text` | Real schema: `choices[]` with `choiceId`, `text`, `moraleDelta`, `effects[]` |
| Faction names | **Not wrong — a different namespace.** See below. |

### The two faction id namespaces

This repo has **two parallel sets of faction ids for the same factions**, and
the expansion spec was quoting the second one:

| Lore / UI namespace | Systems namespace | Faction |
|---|---|---|
| `iron_garrison` | `faction_central_garrison` | The Iron Garrison |
| `ash_militia` | `faction_upland_militia` | The Ash Militia |
| `cult_of_ash_sign` | `faction_cult_of_the_glow` | The Cult of the Ash Sign |
| `warlords_sector_4` | `faction_scavenger_warlords` | The Warlords of Sector 4 |
| *(uses systems id)* | `faction_rebuilders` | The Rebuilders — see `06` |
| *(uses systems id)* | `faction_black_ops` | D/9 — see `06` |

- **Lore/UI**: `faction_lore.json`, read by `FactionLoreCatalogLoader` into
  `LoreCodexPanel`.
- **Systems**: `WorldStateConsequenceSystem.cs` (hegemony), `FactionLockoutEngine`,
  `BureaucraticFrictionSystem`, `DoctrineSystem`, `Quest_MilitiaGrainWar`.

**This is a live defect, not just untidiness.** `Quest_MilitiaGrainWar.cs`
awards trust to `faction_upland_militia`; the Lore Codex shows trust for
`ash_militia`. They are the same faction and the two will never agree.

Two further systems ids had no `faction_lore.json` entry at all, so the player
could be starved by one and killed by the other without the game ever naming
either: `faction_rebuilders` (hegemony-tracked, owns the medical supply) and
`faction_black_ops` (traps, hostile to everyone). Both are now written in
`06_REBUILDERS_AND_BLACK_OPS.md`, using the **systems id as the lore id** —
neither had a lore-side twin yet, so matching the code is the only choice that
does not create a second instance of the militia bug.

Reconciling the namespaces is a code task, out of scope for this bible, but
**no new content should pick a side until it is done.** New factions in
`05_FACTIONS.md` therefore avoid both namespaces deliberately — see the
Powers/Currents split there.

## The rules this bible writes to

From `AGENTS.md`:

- snake_case ids everywhere
- No magic, no fantasy, no real countries/wars/people, no glorified violence
- **Tone: cold, exhausted, human, restrained. Show, don't preach.**

The house voice is already established in `echoes.json` and is the standard
every line here is held to. It works by specificity, never by adjectives:

> Caught in a chain-link fence, a small red winter coat, zipped halfway. The
> hood is up. There's no one inside it… The stitching is intact. **The label
> reads age 6.**

No line in this bible tells the player how to feel. The coat is a size. The
postman's letter has no return address. That is the whole technique.

## The two mechanics this expansion is built on

Both already exist and are barely used.

**1. Trust-reactive prose.** `events.json` supports `threateningBodyText`,
`threateningFactionId` and `threateningTrustBelow` — the same scene renders
differently when a faction has stopped trusting you. Presently used almost
nowhere. Used properly it means the world does not have a fixed narrator: a
checkpoint is a formality or a threat depending on what you have done, and the
player learns their reputation by reading tone rather than a number.

**2. Located knowledge.** `world_history.json` entries carry
`discovery_location_id`, `discovery_trigger` and `knowledge_key` — history is
*found in a specific place*, not granted. Every location in this bible is
authored with the question "what does standing here teach you that a menu
could not?"

## Structure

| File | Contents |
|---|---|
| `00_OVERVIEW.md` | this file |
| `01_GAZETTEER.md` | Sector 4 as a real place — 5 sub-regions |
| `02_THE_LIST.md` | the spine mystery + new `world_history` beats |
| `03_LOCATIONS.md` | new locations, with history and hooks |
| `04_ENCOUNTERS.md` | character encounters and situational encounters |
| `05_FACTIONS.md` | Powers vs Currents; 8 non-territorial factions, peaceful and dangerous |
| `06_REBUILDERS_AND_BLACK_OPS.md` | the two faction ids already live in code with no lore — written to the code, not around it |

## The spine, in one paragraph

Three canon beats — `The Bunker Boom` (Exchange−3Y), `The Quiet Evacuation`
(Exchange−1M), `The Final Broadcasts` (Exchange−1W) — mean that **a list
existed**, and that a month before the warheads flew, the people on it were
quietly moved into shelters prepared for them.

Your bunker was built for someone on that list.

You are not them.

Everything in this bible hangs off that sentence. It is political, not
supernatural. It makes the player's own home the last room they understand. It
gives `location_ministry_of_truth_bunker` and `location_the_memory_vault` a
reason to exist, and it promotes the orphaned `faction_archivists` id — already
sitting unused in `AshGetsDeeperNpcIds.cs` — into the people who kept the list.

The payoff, seeded from Day 1 and landing after Day 200: **the allocated party
arrives.** Frostbitten, escorted, and carrying the paperwork. They are not
raiders. They are polite, and by the law of a country that no longer exists,
they are correct.

See `02_THE_LIST.md`.

## A note on what this expansion deliberately does not add

- **No fifth territorial power.** The four in `faction_lore.json` hold the
  ground and that map stays closed. Additional factions are **Currents** —
  they cross territory rather than holding it. See `05_FACTIONS.md`.
  The two in `06` do not breach this: the Rebuilders sit on one uncontested
  floodplain and their hegemony gates a *market* rather than ground — their
  only map mutation is `Mutation_MedicalSupplyGone` — and D/9 is
  anti-territorial by doctrine, denying fixed infrastructure instead of
  holding it.
- **No new victory paths.** 15 `Victory_*.cs` already exist.
- **No new afflictions.** 27 `Affliction_*.cs` already exist and the pathology
  space is genuinely crowded.
- **No supernatural explanation for anything.** The Cult believes the glow is
  divine. The glow is not divine. Both facts stay true simultaneously and the
  game never adjudicates.
