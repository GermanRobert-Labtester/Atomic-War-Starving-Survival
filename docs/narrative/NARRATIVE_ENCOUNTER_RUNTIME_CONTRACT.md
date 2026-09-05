# Narrative Encounter Runtime Contract (Plan 58)

Verified against `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`,
`EncounterCatalog.cs`, `TravelEncounterSystem.cs` (separate travel system),
and `src/Host/NarrativeHostSession.cs`.

## Encounter DTO (`EncounterDefinition`)

| Field | Type | Semantics |
|---|---|---|
| `id` | string | stable id; runtime registry silently **drops duplicates** (`RegisterEncounter` no-ops on existing id) |
| `title` | string | display |
| `description` | string | scene text |
| `category` | string | **free-form label** — no enum validation anywhere; base uses `Discovery`/`Social`, expansion uses Trade/Misinformation/Structural/Radiation/Weather/Fear/Faction/Mystery/Ethical/Observation |
| `baseWeight` | float | selection weight (both catalogs use 1.5–3.0) |
| `stealthWeightMultiplier` | float | multiplier when stance == `Stealth` |
| `speedWeightMultiplier` | float | multiplier when stance == `Speed` |
| `minDangerLevel` | float | exclusive-below gate: `dangerLevel < minDangerLevel` → weight 0 (expansion uses 0.0/1.0/2.0) |
| `requiredLocationId` | string | empty = anywhere; else **exact ordinal match** against the passed `locationId` — single ID, no lists/tags |
| `forceOnArrival` | bool | present in data; enforced by the host surface layer, not `SelectEncounter` |
| `npcId` | string | Plan 52 data-linkage only (arc resolver never reads it) |
| `choices` | list | see below |

## Weight formula (`GetEffectiveWeight`)

```text
weight = baseWeight
if stance == "Stealth": weight *= stealthWeightMultiplier
elif stance == "Speed": weight *= speedWeightMultiplier
weight = max(0, weight)
eligibility: dangerLevel >= minDangerLevel AND (requiredLocationId empty OR exact match)
```

So: **stealth < 1 means sneaking makes the encounter less likely**; speed < 1
means rushing makes it less likely. Multipliers of 0 remove the encounter
entirely for that stance. Negative is impossible after the `max(0, …)` clamp.

## Selection (`SelectEncounter`)

Two-pass weighted roll over eligible, non-weather-gated encounters; all rolls
through the caller's `ISeededRng` → deterministic per seed. Returns null when
no encounter qualifies (no forced pick).

## Choices (`EncounterChoiceDefinition`)

Supported fields:

- `choiceId`, `text`;
- `moraleDelta`, `guiltDelta` (ints — applied to cumulative state and the
  resolution record);
- Plan 49 extensions: `grantItemId` + `grantItemQuantity`, `setWorldFlag`,
  `journalUnlockId`, `discoverLocationId`, `depletesOnResolve`;
- Plan 52 extensions: `completesQuestId`, `completesQuestChoiceId` (routed
  through `ExpansionQuestSystem` when `QuestLink` is bound).

The 3 baseline encounters use only `moraleDelta`/`guiltDelta` — the grammar
Plan 58 authored against.

## Resolution & persistence

`Resolve()` appends an `EncounterResolutionRecord`
(`encounterId, choiceId, locationId, day, moraleDelta, guiltDelta`) to
`NarrativeEncounterState.history`, bumps `totalResolved`/cumulative
morale/guilt, and bridges quest-linked choices. `CaptureState/RestoreState`
sorts history deterministically (day → encounterId → choiceId) and preserves
the pending surfaced queue. No new save fields exist; new ids conform to the
existing resolved-id model.

## Catalog merge behavior (verified)

`NarrativeEncounterCatalogLoader.Load` loads:
1. `narrative_encounters.json` (base);
2. `narrative_encounters_npc_arcs.json` (Plan 52 arcs, loaded after —
   duplicate ids dropped first-wins).

**`narrative_encounters_expansion.json` is NOT loaded through this path** —
the content-utilization scanner maps it to `NarrativeEncounterSystem` as its
expected consumer, but `NarrativeHostSession.Create` registers only base +
arcs. The live selection pool is therefore base + arcs; the 29 expansion
entries are content siblings for dedup purposes (and a flagged utilization
follow-up).

## Test surfaces (existing, no new tests per §2.4)

Loading, deterministic selection, stance weighting, choice effects, and save
round-trip are covered by existing narrative/encounter suites
(16 narrative-encounter tests green post-Plan-58; full suite 6,580/6,580).
