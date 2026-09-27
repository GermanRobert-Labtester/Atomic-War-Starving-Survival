# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
> **Not committed** (per user directive). See §6 for the closeout evidence.

---

# PLAN-PATROL-RADIO-HOOKS — Patrol Encounter → Radio Broadcast Bridge Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `PATROL-RADIO-HOOKS`
> **Category:** radio / expeditions
> **Plan type:** host integration of an unhosted Core bridge between two live owners.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs` so that resolving a patrol
encounter on the wasteland graph queues the corresponding faction radio
broadcast, and the queued broadcast is delivered through the **canonical radio
owner's** intercept log. The authored patrol radio traffic exists in the corpus
but is unreachable, because nothing subscribes the travel-encounter resolver.

**Bounded outcome:** `TravelEncounterSystem.OnChoiceResolved` → one-shot queued
broadcast id → `RadioHostSession` intercept history. No duplicate broadcast for
the same signal, and the queue survives save/load.

**Non-goals:** no second radio log, no new faction capability table, no new
encounter→broadcast mapping (the authored map in the Core bridge stays the
authority), no audio cue additions.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `grep -rn "PatrolRadioHooks" src/` → no matches |
| Live travel-encounter owner | `TravelEncounterSystem` held by `src/Host/ExpeditionHostSession.cs` (`TravelEngine`, line 314) |
| Live radio owner | `src/Host/RadioHostSession.cs` — owns `_history` (32-cap), `LastIntercept`, `BroadcastIntercepted` |
| Canonical inject precedent | `RadioHostSession.BroadcastBeacon(...)` shows the exact append + event + `LastEvent` shape |
| Broadcast corpus | `RadioBroadcastCatalog.GetById(broadcastId)` → `UnifiedRadioBroadcast` |
| Authored signal ids | `PatrolRadioHooks.EncounterToRadioMap` maps 11 patrol encounter ids → 9 broadcast ids |
| Capability table | `RadioCapableFactions` / `NonRadioCapableFactions` / `IsFactionRadioCapable` |
| One-shot semantics | `QueueSignal` dedupes pending + consumed; `TickRadio()` drains and consumes |
| No save section | `SaveSectionRegistry` has no `patrol_radio_hooks` |

## 3. Files

### New — Host
- `src/Host/PatrolRadioHostSession.cs` (+ `PatrolRadioSaveStore`)
- `src/Host/HostCli.PatrolRadio.cs`
- `src/Main.PatrolRadio.cs`

### Modified — Core
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `patrol_radio_hooks` section
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — `patrol_radio_hooks_ticked`
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--patrol-radio-selftest`

### Modified — Host
- `src/Host/RadioHostSession.cs` — one additive canonical method
  `PlayFactionBroadcast(string broadcastId, int day)` mirroring `BroadcastBeacon`
- `src/Host/ExpeditionHostSession.cs` — nothing (subscription happens in the host
  wiring layer, not inside the session)
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `src/Main.CampaignOwners.cs` (phase-5 day owner: drain + deliver)
- `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`

### Modified — Tests
- `Ashfall.Core.Tests/Radio/PlanPatrolRadioHostIntegrationTests.cs` (new)
- `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (section pin)
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (1 row)

## 4. Acceptance

1. `--patrol-radio-selftest` ≥ 10/10 headless: encounter→signal mapping,
   capability table, duplicate suppression, one-shot drain, unknown-broadcast
   refusal, subscription/unsubscription, save/restore, delivery into the radio
   intercept log.
2. Focused xUnit suite green; the radio intercept log gains exactly one entry
   per queued signal and no entry for unmapped encounters.
3. The radio owner remains the sole writer of intercept history; this bridge
   only produces ids.
4. Adjacent gates green (same set as the railway package).

## 5. Deferred with named reasons

- No UI queue readout: the radio panel is a shared surface; a pending-signal
   count row is a follow-up needing the integrator's panel budget.
- No patrol-specific audio cues: `AudioCueCatalog` additions are audio-lane work.

## 6. Closeout evidence (2026-09-26)

**Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
**Branch:** `integration/all-latest-2026-09-24` · **Not committed** (user directive).

| Item | Evidence |
| --- | --- |
| Headless probe | `--patrol-radio-selftest` **11/11** (registered in the Core + host CLI registries, the selftest manifest, and the CLI command catalog) |
| Focused xUnit | `PlanPatrolRadioHostIntegrationTests` **7** |
| Host build | `dotnet build Ashfall.csproj` — 0 errors |
| Core test build | `dotnet build Ashfall.Core.Tests` — 0 errors |
| Save section pin | `ComprehensiveSaveStoreCorruptionAndMigrationTests` **308** sections |
| Adjacent gates green | `SaveSectionRegistryTests` 5/5, `PersistentFilenameRegistry`, `DayEventVocabulary`, `DayEventParitySourceGate`, `HostCliActionParityGate`, `MainTriadDriftGate.SetupWithoutSave`, `SaveStoreMatrixGate`, `PortContractGate` |
| Regenerated artifacts | selftest manifest **273** (271 headless) · CLI catalog **333 entries / 539 flags** · save-store matrix **310 stores** · port contract **307 seams** |

### Premise audit (Rule 7) — rejected candidates with current evidence

| Candidate | Why rejected |
| --- | --- |
| `DraisineRerailingSystem` | **Already fully hosted** by the Plan 130–133 lane (`src/Host/Plans130To133HostSessions.cs` `DraisineRerailingHostSession`, `src/Main.Plans130_133.cs`, `src/UI/Plans130To133Panel.cs`, `DraisineRerailingSaveStore`, and the `draisine_recovery` registry row). Integrating it would have created a duplicate host and a duplicate save section; the partial work was reverted in full. |
| `BallisticsSystem` | The live `TacticalCombatSystem` + `BallisticsWorkbenchSystem` own shot resolution. |
| `EncounterChoiceEffectDispatcher` | The live `ExpeditionHostSession` already applies `SetWorldFlagId` to the flag ledger. |
| `RadioTuner` | Would be a second tuning authority over `RadioHostSession.CurrentFrequency`. |
| `FoodTypeSystem` | `FoodPreservationSystem` is the signed spoilage truth and `food_preservation.json` already carries the per-item food-type mapping. |
| `RestockAllocationEngine` / `ProstheticConditionWearEngine` | F13 / F14 — decision-blocked (and concurrently in flight). |
| `GarmentLayeringThermalEngine` | Concurrent Plan 142 lane. |

### Deferred with named reasons

See §5 above; every deferral names the owner whose signature is required.
