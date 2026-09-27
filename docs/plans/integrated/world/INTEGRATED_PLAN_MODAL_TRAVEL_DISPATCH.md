# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
> **Not committed** (per user directive). See §6 for the closeout evidence.

---

# PLAN-MODAL-TRAVEL-DISPATCH — Multi-Modal Wasteland Travel Dispatch Read Model

> **STATUS: APPROVED BY USER**
> **Package:** `MODAL-TRAVEL-DISPATCH`
> **Category:** world / expeditions
> **Plan type:** host integration of an unhosted pure engine as a derived read model.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/World/ModalTravelDispatchEngine.cs` (L-P32R /
UNBLOCK-04 §2.14 / §5.12) so a player can evaluate whether a wasteland route is
crossable by foot, ground convoy, amphibious rig, or aerial recon **before**
committing an expedition — with the transit duration, fuel requirement, and
attrition risk the live map, vehicle, inventory, and weather owners imply.

**Bounded outcome:** a derived read model over `WastelandMapSystem.Routes`.
No mutation, no save section, no second route list.

**Non-goals:** no new route pathfinding (the map owner owns the graph), no new
vehicle condition authority, no new weather forecast authority, no new fuel
ledger, no change to expedition dispatch itself.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `grep -rn "ModalTravelDispatchEngine\|TravelModality" src/` → no matches |
| Unit-tested only | `Ashfall.Core.Tests/World/ModalTravelDispatchEngineTests.cs` |
| Live route owner | `WastelandMapSystem.Routes` → `IReadOnlyList<MapRoute>` (DistanceKm, WeatherHazard, Tags) |
| Live blocked-route seam | `ExpeditionHostSession.GetBlockReason` / `ExtraGateBlock` / `GetWeatherGateBlock` |
| Live vehicle condition | vehicle/garage condition permille surfaces in `src/Host/` |
| Live fuel | `Inventory` item counts; `PowerGridHostSession` fuel-unit conventions |
| Live weather | weather owner's forecast/hazard surfaces |
| No save section needed | the engine is `static` and returns immutable results |

The engine is engine-free, integer/permille deterministic, and complete.

## 3. Files

### New — Host
- `src/Host/ModalTravelDispatchHostSession.cs`
- `src/Host/HostCli.ModalTravelDispatch.cs`
- `src/Main.ModalTravelDispatch.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--modal-travel-dispatch-selftest`

### Modified — Host
- `src/Host/HostCli.cs`, `src/Main.Application.cs`

### Modified — Tests
- `Ashfall.Core.Tests/World/PlanModalTravelDispatchHostIntegrationTests.cs` (new)
- `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` — no new heartbeat (no daily
  state); a projection row is recorded instead

## 4. Acceptance

1. `--modal-travel-dispatch-selftest` ≥ 10/10 headless: each modality's terrain
   gate, weather grounding, fuel refusal, critical-condition refusal, duration and
   attrition math, and null-route refusal.
2. Focused xUnit suite green; the read model is proven to be a pure projection of
   the live map routes (mutating the map route changes the projection, and the
   projection never writes back).
3. Deterministic: identical inputs → identical integers across repeated calls.
4. Adjacent gates green: `SaveSectionRegistryTests` (unchanged count),
   `HostCliActionParityGate`, `HostCliHelpContract`, `MainTriadDriftGate`
   (`SetupWithoutSave`), `PortContractGate`.

## 5. Deferred with named reasons

- No save section: a derived read model has no state to persist; adding one
   would be fabricated state.
- No map-panel route overlay: `src/UI/MapPanel.cs` is a shared surface; the
   additive readout rides the expedition status line instead.

## 6. Closeout evidence (2026-09-26)

**Claim:** `claim-quad-f-warlord-response-patrolradio-modaltravel-rationconflict-2026-09-26`
**Branch:** `integration/all-latest-2026-09-24` · **Not committed** (user directive).

| Item | Evidence |
| --- | --- |
| Headless probe | `--modal-travel-dispatch-selftest` **11/11** (registered in the Core + host CLI registries, the selftest manifest, and the CLI command catalog) |
| Focused xUnit | `PlanModalTravelDispatchHostIntegrationTests` **9** |
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
