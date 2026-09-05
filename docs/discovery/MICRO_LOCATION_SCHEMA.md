# Plan 49 — Micro-Location Schema

## EncounterDefinition (used for micro-locations)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable micro-location ID (prefix: `micro_`) |
| title | string | yes | Player-facing title |
| description | string | yes | Environmental story (1-2 sentences) |
| category | string | yes | "Discovery", "Hazard", or "Social" |
| baseWeight | float | yes | Selection weight (0.1-0.8) |
| stealthWeightMultiplier | float | yes | Weight multiplier for Stealth stance |
| speedWeightMultiplier | float | yes | Weight multiplier for Speed stance |
| minDangerLevel | float | yes | Minimum danger for eligibility |
| requiredLocationId | string | no | Location-specific encounter (empty = any) |
| forceOnArrival | bool | no | Force on arrival at destination |
| choices | list | yes | Player choices (2-3) |

## EncounterChoiceDefinition (extended for micro-locations)

| Field | Type | Required | Description |
|---|---|---|---|
| choiceId | string | yes | Stable choice ID |
| text | string | yes | Player-facing choice text |
| moraleDelta | int | yes | Morale impact |
| guiltDelta | int | yes | Guilt impact |
| grantItemId | string | no | Item to grant on resolution |
| grantItemQuantity | int | no | Quantity of granted item |
| setWorldFlag | string | no | World flag to set |
| journalUnlockId | string | no | Journal/codex knowledge key |
| discoverLocationId | string | no | Location to discover |
| depletesOnResolve | bool | no | Whether choice depletes the micro-location |

## Rarity Tiers

| Tier | Weight | Examples |
|---|---|---|
| Common | 0.6-0.8 | memorial, grave, pipe, barricade, tent, shrine |
| Uncommon | 0.4-0.5 | truck, bus, bridge, greenhouse, clinic, radio tower |
| Rare | 0.1-0.3 | emergency cache, observation post, drone, fuel cache, supply drop |

## Category Distribution

| Category | Count | Purpose |
|---|---|---|
| Discovery | 20 | Environmental storytelling, loot, information |
| Hazard | 3 | Risk/reward decisions, contamination |
| Social | 2 | Ethical decisions, moral texture |

---

# F1–F4 Runtime Consequence Pipeline (Flagship Integration)

Narrative Core decides what a choice means (`NarrativeEncounterSystem.TryResolve`
returns a `NarrativeEncounterResolutionResult`); the Host applies each effect
through the subsystem that owns it. Core never mutates expedition loot, shelter
inventory, journal state, or the discovery ledger.

## Depletion (`depletesOnResolve`)

- **Semantic:** per encounter ID. Resolving any choice with
  `depletesOnResolve: true` marks the WHOLE encounter exhausted — including
  every other choice on it.
- **Non-depleting revisits:** choices without the flag never deplete; the
  encounter stays eligible and can be surfaced again.
- **Selection:** depleted encounters are filtered before weighted selection,
  so they neither distort the weight sum nor consume deterministic RNG rolls.
- **Capacity independence:** a depleting choice depletes even when the loot
  grant is later rejected by capacity — the site was still searched. Loot is
  never re-farmable because the pack was full.
- **Persistence:** `NarrativeEncounterState.depletedEncounterIds` (ordinal
  sorted on capture). Saves predating the field (null list) are migrated by
  reconstructing the set from resolution history; a present list (even empty)
  is authoritative and is never re-derived.

## Item delta (`grantItemId` + `grantItemQuantity`)

- **Signed quantity:** `> 0` grants, `< 0` removes (offerings), `0` = no item
  effect. A non-zero quantity with an empty item id fails data integrity.
- **Positive grants** go to the ACTIVE expedition sortie's loot list (routed
  to the surfacing survivor, else the ordinal-first active expedition at the
  resolution's location) — never directly to shelter inventory; the normal
  expedition return flow unloads loot.
- **Capacity:** the grant is checked whole against `maxLootCapacityKg` (item
  weight from the item catalog, 1 kg fallback). An overweight grant is
  rejected whole — no partial stacks. Narrative resolution, depletion,
  journal, and location effects still apply; the UI reports the cargo was
  left behind.
- **Negative grants (offerings)** consume from the shelter inventory through
  the canonical `Inventory.TryConsume` transaction. Never underflows: an
  unaffordable offering is rejected (`RejectedInsufficientItems`) and
  inventory is unchanged.
- **Exactly once:** item deltas are not naturally idempotent — the Host
  applies them only from a committed `TryResolve` payload. Restore never
  replays historical resolutions.
- **Migration policy (historical saves):** item effects are NOT reconstructed
  from history — retroactive grants/removals cannot be reconciled safely.

## Journal unlock (`journalUnlockId`)

- **Canonical key:** `micro_` knowledge namespace (validated by tests).
- **Application:** the Host calls `JournalSystem.TryDiscoverKnowledge(key,
  author, day)` — one atomic path that writes the journal entry AND fires
  `OnCodexUnlocked` exactly once per key through a single KnowledgeBase dedup
  gate. (Calling `TryDiscover` + `AddKnowledgeEvidence` separately cannot
  guarantee this — whichever runs second finds the key already known.)
- **Author/time:** author id `expedition`; campaign day from the resolution;
  no wall-clock. Category routing comes from the journal catalog, not from
  micro-location code.
- **Dedup:** re-resolving a journal choice (or re-calling the API) is safe —
  no duplicate entry, no second event.

## Location discovery (`discoverLocationId`)

- **Canonical authority:** the expedition destination ledger on
  `ExpeditionSystem` (`IsLocationKnown` / `DiscoverLocation`), persisted in
  the expedition aggregate (`knownLocationIds`). It is NOT the radio
  triangulation candidate set (radio domain, `triangulated_*` ids) nor the
  wasteland map graph's node discovery (`loc_*` map nodes).
- **Dispatch gating:** opt-in per destination. An `ExpeditionDefinition` with
  `requiresDiscovery: true` stays undispatchable (`GetBlockReason` reports
  "Location unidentified") until discovered. Destinations without the flag
  dispatch as before. `rural_gas_station` and `government_bunker` are the
  authored clue-gated fixtures.
- **Semantics:** discovery means the player learns the destination exists. It
  does not reveal unrelated locations, clear route hazards, or bypass weather
  gates, crossing gates, route topology, or any other dispatch requirement.
- **Idempotence:** `DiscoverLocation` is idempotent (already-known returns
  true, no second event). Unknown destination ids are rejected.
- **Migration policy (historical saves):** a legacy aggregate (null
  `knownLocationIds`) reconstructs discoveries from the narrative resolution
  history's `discoverLocationId` effects — deterministic, catalog-looked-up,
  never guessed.

## Multi-effect choices

- Effects combine freely: item + journal + location + flag + depletion can
  coexist on one choice (the observation post combines journal + location).
- **Application order:** item delta → journal → location → world flag.
- **Failure semantics:** each effect reports an independent typed status
  (`ExpeditionHostSession.EncounterApplicationResult`); a capacity or
  affordability rejection never blocks the other effects or the narrative
  commit.
- **Exactly-once events:** one resolution produces at most one journal entry,
  one codex event, one discovery event, and one loot change. Save/load
  replays none of them.

## Ownership matrix

| State | Owner |
|---|---|
| Resolution history + depleted set | NarrativeEncounterState ("narrative" save section) |
| Expedition loot | expedition save ("expedition" section aggregate) |
| Shelter inventory | inventory save |
| Journal keys/entries | JournalSystem ("journal" section) |
| Destination discovery ledger | expedition aggregate (`knownLocationIds`) |
| World flags | CampaignConsequenceLedger |
| Map node discovery / routes | WastelandMapSystem (unchanged, separate domain) |
