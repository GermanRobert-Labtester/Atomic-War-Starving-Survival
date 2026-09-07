# Patrol Recognition System

Patrol encounters use the existing chain-stage architecture and a patrol history ledger to create persistent faction memory. Factions remember how the player treated them across encounters and across save/load cycles.

## Architecture

### Chain Stages (Encounter-level gating)

The `chain_id`, `chain_stage`, `prereq_chain_stage`, and `advances_chain_stage` fields on `TravelEncounterDefinition` and `TravelEncounterChoice` control encounter eligibility and progression.

- `chain_id` — globally unique identifier scoped to one faction relationship (e.g., `patrol_garrison_trust`)
- `prereq_chain_stage` — the chain stage that must be reached before this encounter/choice is eligible
- `advances_chain_stage` — when a choice is resolved, the chain advances to this absolute stage value (not increment)
- `-1` for `prereq_chain_stage` means "any stage" — used for recurring base encounters

### Patrol History (Choice-level memory)

`PatrolHistoryRecord` tracks every resolved patrol choice per faction, per encounter, per choice. This enables recognition labels, weighted follow-up behavior, and history-tag-gated choices.

- `history_tags` on choices — tags recorded when the choice resolves (e.g., `paid_toll`, `hostile`, `cooperative`)
- `required_history_tags` / `forbidden_history_tags` on choices — gate availability on prior history
- `required_chain_stage` / `max_chain_stage` on choices — gate availability on chain progression

### Recognition Context

`PatrolRecognitionContext` is a read-only projection built by `TravelEncounterSystem.GetRecognitionContext()`. It aggregates:
- `PriorChoiceIds` — all choice IDs resolved for this faction
- `EncountersWithFaction` — total encounter count
- `PaidTollCount` / `FoughtPatrolCount` — semantic counters
- `CurrentChainStage` — current patrol chain stage
- `CurrentStanding` — canonical faction standing
- `RecognitionTags` — accumulated history tags

## Garrison Trust Chain

```
chain_id: patrol_garrison_trust
```

### Stage 0 — Initial Checkpoint

Encounter: `enc_patrol_garrison_checkpoint` (+ v2, v3 variants)
- `prereq_chain_stage: -1` (always eligible)
- `cooldown_group: patrol_garrison_checkpoint` (shared 3-day cooldown)

Choices:
| Choice | Effect | Advances |
|--------|--------|----------|
| `choice_pay_garrison_toll` | -2 canned_food, +1 standing | → stage 1 |
| `choice_show_garrison_pass` | Requires document, +2 standing | no advance |
| `choice_negotiate_garrison` | -1 morale, no standing | no advance |
| `choice_avoid_garrison` | -2 morale, bypass | no advance |

### Stage 1 — Recognized Payer

When chain stage reaches 1, new choices become available on the same encounter:
| Choice | Effect | Requires | Advances |
|--------|--------|----------|----------|
| `choice_garrison_reduced_toll` | -1 canned_food, +1 standing, +1 morale | `required_chain_stage: 1` | → stage 2 |

### Stage 2 — Trusted Regular

When chain stage reaches 2:
| Choice | Effect | Requires |
|--------|--------|----------|
| `choice_garrison_free_passage` | +2 morale, +1 standing | `required_chain_stage: 2` |

Recognition label progression:
- Stage 0, no history: "No prior contact"
- Stage 0, has history: "Prior contact"
- Stage 1: "Recognized regular"
- Stage 2: "Trusted regular"
- Hostile tag: "Hostile history"

State flow:

```text
stage 0 / no prior contact
        │ pay initial toll
        ▼
stage 1 / recognized regular
        │ accept reduced toll
        ▼
stage 2 / trusted regular
        ├─ free passage
        └─ faction trade opportunity
```

## Warlord Hostility Memory

Warlord encounters use patrol history tags rather than chain stages for recognition. Fighting a warlord raid records a `hostile` tag, which increases the effective weight of future warlord raid encounters by 25%.

```
faction: warlords_sector_4
trigger: choice_warlord_fight (standing_delta: -15, history_tags: [hostile])
effect: GetEffectiveWeight multiplies by 1.25 when hostile tag present
```

This is faction-scoped: fighting warlords does not affect garrison or other faction encounters.

## Save/Restore Behavior

`TravelEncounterState` persists:
- `ChainStages` — `Dictionary<string, int>` keyed by chain_id
- `PatrolChainStages` — `Dictionary<string, int>` keyed by `canonicalFaction::chainId`
- `EncounterAvailableDay` — `Dictionary<string, int>` keyed by cooldown group/encounter ID
- `PatrolHistory` — `List<PatrolHistoryRecord>` with faction, encounter, choice, day, tags

All state survives capture → serialize → deserialize → restore with exact value fidelity. Legacy saves without patrol chain/history data initialize to empty (stage 0, no history).

## Authoring Rules

1. **Chain IDs must be globally unique and faction-scoped.** Use `patrol_<faction>_<relationship>` (e.g., `patrol_garrison_trust`, `patrol_warlords_sector_4_tribute`).

2. **Advancement is absolute, not incremental.** `advances_chain_stage: 2` sets the chain to stage 2 regardless of current stage.

3. **Use `required_chain_stage` on choices for same-encounter gating.** This lets one encounter definition serve all stages through conditional choice availability.

4. **Use `prereq_chain_stage` on definitions for separate-stage encounters.** This creates distinct encounter entries that appear only at specific chain stages.

5. **`prereq_chain_stage: -1` means "any stage."** Use for base encounters that should always be eligible.

6. **History tags are additive.** Once recorded, they persist forever. Design gating accordingly.

7. **Faction scoping is enforced.** `GetPatrolChainKey` uses `canonicalFaction::chainId`. Cross-faction contamination is structurally impossible.

## Presentation Model

`BuildPatrolPresentation()` in `TravelEncounterSystem` produces a read-only `PatrolEncounterPresentation` with:
- `FactionId` — canonical systems ID
- `DisplayFactionId` — authored lore ID for the faction name and emblem; it remains separate so an Iron Garrison patrol is not presented with the Central Garrison's later-era display name
- `TerritoryState` — controlled/contested/border
- `PatrolArchetype` — checkpoint/raid_party/etc.
- `RecognitionLabel` — human-readable recognition state
- `Choices` — each with availability, reason codes, costs, deltas

Hosts (UI) should consume this projection rather than reimplementing eligibility logic.

## Testing

- `PatrolF5F8LifecycleTests` — full chain lifecycle, save/restore, recognition, atomicity
- `TravelEncounterCooldownGroupTests` — cooldown groups, per-archetype durations
- `PatrolEncounterIntegrationTests` — faction standing, costs, requirements, expedition bridge
- `TravelEncounterBalanceSimulatorTests` — deterministic frequency simulation
- `TravelEncounterPatrolBalanceTests` — broader patrol regression and audit bands
