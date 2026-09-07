# Plan 202 — Subterranean Plastic Pyrolysis — Closeout

**Flagship:** Plans 202–205 (`docs/plans/PLANS_202_205_RECONNAISSANCE.md`). **Status: IMPLEMENTED & VERIFIED** (commit `1e65f6b8`, Wave F hardening in the flagship integration log).

## Contracts

- **Core:** `Ashfall.Core.Shelter.PlasticPyrolysisSystem` — follows the
  `KineticStorageSystem` engine conventions (catalog-driven, inventory-port
  delegates, `ISeededRng`, capture/restore). The foundry was deliberately not
  extended: it is a domain-specific smelter-bay (heats/casts/treaties), and the
  house pattern for new industrial machines is a focused engine class.
- **Mass balance:** each profile carries explicit kg accounting
  (`input_mass_kg ≥ liquid+light+carbon`, losses 5–50%) — gated by
  `Catalog_MassBalance_IsBounded_NoCreatedMass`.
- **Energy:** batch draws the shelter's real surplus grid power (host projects
  `PowerGridSystem.NetWatts`); a deficit **stalls** the batch cold (no
  progress, no hazard roll). Offgas credit returns to the grid capped at 40%
  of batch cost — never net-positive (§5.6).
- **Hazards:** day-derived fresh-seed rolls (Wave F house pattern — split-run
  safe); incident table: quality loss / machine damage / fire (typed
  `OnFireIncident` → hazard authority) / gas release (registered
  `VentilationSource` — single air authority).
- **Skill:** host-injected `OperatorSkillProvider` (0–1) reduces risk by the
  profile's bounded share; duty-roster projection is a follow-up seam.
- **Fuel compatibility (Trap B):** outputs are canonical `Fuel`-type items
  (`synthetic_fuel_canister`, `fuel_1l`) — every fuel consumer path that
  accepts fuel items accepts them; no per-consumer Plan 202 branches.
- **Byproduct consumer (§5.13):** `carbon_black_powder` → new
  `craft_filter_pack_carbon` recipe (filter_pack, workbench) + trade value.
- **Save:** checksummed envelope (`plastic_pyrolysis` campaign section);
  old-save default = no machine, no free fuel, no free batches.

## Failure-state matrix (§15, verified by test)

| Failure | Behavior |
|---|---|
| Invalid feedstock | `pyro.feedstock_invalid` — batch rejected |
| Power unavailable | batch stalls (`pyro.batch_stalled`), resumes on recovery |
| Condition < 25 | `pyro.maintenance_required` |
| Buffer full | `pyro.storage_unavailable` — claim first |
| Fire | hazard authority handoff; machine may be destroyed → offline |
| Save/load | no output duplication (claim idempotence gated) |

## Verification

15 engine tests + cross-plan Wave F replay (`Plans202To205CampaignIntegrationTests`):
same-seed replay, day-15 and multi-point save/load split convergence against
30 continuous days, real-output and no-negative-inventory assertions.
Suite: 9166 total / 9157 passed at closeout (sole failures = concurrent
pharma stream's `room_pharma_lab` id + its INDEX.md reorganization).

## Abstraction boundary

No temperatures, catalysts, or condensation-hardware procedures — profiles are
abstract operating bands; the UI shows bands and outcomes only.
