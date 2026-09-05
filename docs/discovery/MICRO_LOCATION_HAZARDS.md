# Micro-Location Hazards — Disease & Contamination Integration (F17)

Flagship plan §6 deliverable. Proven by `Ashfall.Core.Tests.MicroLocationHazardIntegrationTests` (13 tests) and `MicroLocationIntegrationDeterminismTests`.

## Authoritative hazard systems

| Domain | Authority | Notes |
|---|---|---|
| Biological contamination | `Ashfall.Core.Disease.DiseaseSystem` (`Infect(survivorId, diseaseId, day)`) | Deterministic, no-op when the survivor is already infected with the same disease. No parallel contamination counter exists or was created. |
| World-flag channel | `CampaignConsequenceLedger` (`IFlagLedger`) | The authored `setWorldFlag` effect; committed by the host consequence applier. |
| Hazard routing | `Ashfall.Core.Narrative.MicroLocationHazardRegistry` | Pure Core coordinator: flag → consequence mapping + exactly-once application. The disease system never sees encounter IDs. |
| Radiation | `RadiationSystem` / `NeedsSystem.ExposeToZone` | Untouched by F17 (see deferred hooks). |

## Current hazard locations (from `micro_locations.json`)

| Location | Choice | Effects |
|---|---|---|
| `micro_dead_livestock` | `scavenge_livestock` | `cloth` ×2, `setWorldFlag: micro_contamination_exposure`, depletes |
| `micro_dead_livestock` | `inspect_livestock_tags` | journal `micro_dead_livestock_tags` (non-depleting) |
| `micro_dead_livestock` | `avoid_livestock` | nothing |
| `micro_shell_crater` | `inspect_crater` | `scrap_metal` ×2, depletes — **no contamination** |
| `micro_shell_crater` | `salvage_crater_harness` | `mechanical_parts` ×1, depletes |
| `micro_collapsed_bridge` | `search_bridge_vehicle` | `fuel` ×2, depletes — **no contamination** |
| `micro_collapsed_bridge` | `inspect_bridge_structure` | `scrap_metal` ×3, depletes |

## `micro_contamination_exposure` semantics

- **Producer:** exactly one — `micro_dead_livestock / scavenge_livestock` (pinned by `F17_07_ShellCraterAndBridge_NeverAuthorContaminationFlag`).
- **Consumer:** `MicroLocationHazardRegistry` maps the flag to `disease_zoonotic_flu` and routes it through `DiseaseSystem.Infect`. The mapping is **data-authored, not invented**: the disease catalog's own `source_note` for `disease_zoonotic_flu` reads *"Carried in from scavenged bedding and dead livestock."*
- **Host wiring:** `ExpeditionHostSession.ApplyDisease` (lazy delegate, same pattern as `WildlifeTrappingHostSession.ApplyDisease`), wired in `Main.Expeditions.cs` to `_disease?.Engine?.Infect(...)`. The survivor exposed is the same deterministic grant-survivor rule used for loot (`ResolveGrantSurvivorId`).
- The flag is a **persistent world fact**, not a consumed trigger. Re-processing cannot re-infect: the hazard only fires on the ledger's unset→set transition (`flagWasAlreadySet` verdict), and `Infect` is itself a no-op for an already-infected survivor. Two independent exactly-once gates close the §14.4 flag-replay exploit.

## Deterministic exposure behavior

No RNG enters the resolution or hazard path. Given the same fixture, seed, choice, and subsystem state, the contamination outcome (infected survivor, disease id, `infected_day`) is identical — pinned by `F17_12_Deterministic_SameSeedSameChoice_IdenticalContaminationState` across 8 seeds.

## Stacking policy

The canonical authority refuses additive stacking for the same disease (`Infect` is a no-op when already infected). The discovery layer does not emulate stacking — a second exposure is a no-op, matching `DiseaseSystemTests` semantics.

## Save/load behavior

`DiseaseSystem.CaptureState/RestoreState`, the flag ledger, and the narrative state round-trip through the production `SystemTextJsonSerializer` wire. After restore: contamination state identical, first post-load hazard pass reports `AlreadyKnown`, and the depleted site can never re-surface through the production selector (`F17_11`).

## Idempotence rule

`micro-location-id + choice-id` resolution commits exactly once through Core depletion; the hazard additionally requires a fresh flag transition. Revisiting, re-rendering, event replay, and save/reload all apply the exposure exactly once.

## Shell crater UXO status

**Investigated — deferred.** No unexploded-ordnance / explosive-trap / injury authority exists in the current tree that a crater hazard could delegate to without inventing a new subsystem (§6.9 explicitly forbids that). `inspect_crater` / `salvage_crater_harness` remain structural-salvage choices with no biological contamination. When an injury/structural-hazard authority lands, the same `MicroLocationHazardRegistry` pattern can route a crater flag into it with zero disease coupling.

## Collapsed bridge status

**Investigated — no contamination, by authored design.** The bridge's authored effects are loot-only (fuel/scrap). No route-gating or traversal-injury authority reads bridge-specific state today; forcing contamination onto it would violate the authored data. Deferred alongside the crater hooks.

## Tests

- `MicroLocationHazardIntegrationTests` — positive path, avoidance path, tag-inspection path, shell-crater no-contamination, flag-replay protection, registry contract, save/reload parity, determinism, orphan-consumer gate.
- `MicroLocationWorldFlagTests` / `WorldFlagConsumerIntegrationTests` — flag ledger idempotence and cross-system reads (pre-existing, unchanged).

## Known deferred extensions

UXO injuries (crater), structural traversal hazards (bridge), radiation exposure at hot micro-sites, weather-sensitive hazard severity, equipment-based contamination protection (gas mask already the authored countermeasure item for zoonotic flu), disease incubation after scavenging.
