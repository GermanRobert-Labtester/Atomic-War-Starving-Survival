# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
> **Not committed** (per user directive). See §6 for the closeout evidence.

---

# PLAN-RATION-CONFLICT — Survivor Ration Resentment Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `RATION-CONFLICT`
> **Category:** survivors / economy
> **Plan type:** host integration of an unhosted Core authority. No second rationing ledger.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` so unequal ration
allocations produce resentment between survivors, escalating to a confrontation
or (deterministically) a theft, with the morale and relationship consequences
routed through the canonical owners. The class has **zero `src/` references**
(verified by `grep -rl --include=*.cs RationConflictSystem src/` → no matches) and
no port-contract seam.

**Bounded outcome:** the per-survivor allocation the live rationing owner already
publishes feeds `SetAllocation`; the day owner ticks the authority; confusion and
theft apply morale exactly once through `NeedsSystem`, and the resentment target
is recorded through `SurvivorRelationsSystem.ModifyAffinity`. All resentment state
persists in a new `ration_conflict` section.

**Non-goals:** no new ration tier, no new allocation authority, no new morale
authority, no new relationship ledger, no UI panel.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `grep -rl --include=*.cs RationConflictSystem src/` → no matches |
| Live rationing owner | `ResourceRationingSystem` (`Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`) with `SurvivorRationAssignment.PriorityBonus` and `GetAllocationMultiplier(resourceId, survivorId)` |
| Live morale owner | `NeedsSystem.Modify(survivorId, NeedKind.Morale, delta)` |
| Live relationship owner | `SurvivorRelationsSystem.ModifyAffinity(a, b, delta)` — the **only** place pair affinity is produced |
| Stateful | `CaptureState()` / `RestoreState(RationConflictSaveState)` |
| Deterministic | one `ISeededRng` draw per theft attempt |
| Authored thresholds | `FairnessDeviationThreshold 0.20`, `ResentmentGainPerDay 0.10`, `ConfrontationThreshold 0.70`, `TheftThreshold 0.85`, `ConfrontationMoraleHit -10`, `TheftMoraleHit -15` |
| No save section | `SaveSectionRegistry` has no `ration_conflict` |

The engine itself is complete and already declares its boundary: it emits
`OnMoraleDelta(… , source)` where the source strings
(`ration.confrontation` / `ration.theft`) are attribution for the owner that
actually owns morale.

## 3. Files

### New — Host
- `src/Host/RationConflictHostSession.cs` (+ `RationConflictSaveStore`)
- `src/Host/HostCli.RationConflict.cs`
- `src/Main.RationConflict.cs`

### Modified — Core
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `ration_conflict` section
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — `ration_conflict_ticked`
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--ration-conflict-selftest`

### Modified — Host
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `src/Main.CampaignOwners.cs` (phase-5 day owner)
- `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`

### Modified — Tests
- `Ashfall.Core.Tests/Survivors/PlanRationConflictHostIntegrationTests.cs` (new)
- `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (section pin)
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (1 row)
- `scripts/ci/generate-architecture-map.py` (1 entry)

## 4. Acceptance

1. `--ration-conflict-selftest` ≥ 10/10 headless: allocation projection from the
   live rationing owner, fairness computation, resentment accrual below/above the
   deviation threshold, confrontation morale exactly once, deterministic theft
   roll, save/restore round-trip, reset.
2. Focused xUnit suite green; morale applied exactly once per event through
   `NeedsSystem`, and the resentment target recorded through
   `SurvivorRelationsSystem.ModifyAffinity` (never a local affinity copy).
3. No `System.Random`; the injected campaign-forked stream is the only randomness.
4. Adjacent gates green: `SaveSectionRegistryTests`, `PersistentFilenameRegistry`,
   `DayEventVocabulary`/`DayEventParitySourceGate`, `MainTriadDriftGate`
   (`SetupWithoutSave`), `SaveStoreMatrixGate`, `PortContractGate`.

## 5. Deferred with named reasons

- No UI panel: the survivors/social surfaces are shared; a resentment row belongs
  with the integrator's panel budget.
- No theft inventory transfer: the engine only emits the fact. Moving rations
  between survivors is an inventory-authority decision that needs a signed
  producer seam, exactly as the trauma-bond shared-hazard producer was left
  unbound.

## 6. Closeout evidence (2026-09-26)

**Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
**Branch:** `integration/all-latest-2026-09-24` · **Not committed** (user directive).

| Item | Evidence |
| --- | --- |
| Headless probe | `--ration-conflict-selftest` **11/11** (registered in the Core + host CLI registries, the selftest manifest, and the CLI command catalog) |
| Focused xUnit | `PlanRationConflictHostIntegrationTests` **9** |
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
