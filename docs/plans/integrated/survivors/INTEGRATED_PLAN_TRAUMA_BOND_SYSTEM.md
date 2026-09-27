# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-e-expansion21-expansion13-traumabond-xp08f6-2026-09-26`
> **Not committed** (per user direction). See §5 for the closeout evidence.
>
> **Independent seal-sweep verification (2026-09-26, no-commit seal session):** `TraumaBondSystem` is fully wired: own `trauma_bond` section, `TraumaBondHostSession` + save store, phase-5 day owner, co-shift bonus composed into the duty-roster work-speed seam, `PlanTraumaBondHostIntegrationTests` 8/8 and `--trauma-bond-selftest` 11/11 verified 2026-09-26. See `INTEGRATION_PLANS.md` for the full evidence trail.

# PLAN-TRAUMA-BOND-SYSTEM — Survivor Trauma Bond Host Integration

> **Package:** `TRAUMA-BOND-SYSTEM` (Wave-3 psychology/health/social track — trauma-bond owner)
> **Category:** survivors / social
> **Plan type:** host integration of an unhosted Core authority. Extends the existing relationship owner; no second social ledger.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

`TraumaBondSystem` is a complete, self-contained Core authority (bond formation
from shared hazards, daily decay, capture/restore, affinity and co-shift hooks)
with **zero host references**. Survivors who endure a hazard together never form
a bond, bonds never decay, and the authored co-shift efficiency bonus never
reaches the duty roster.

**Bounded outcome:**

1. `TraumaBondHostSession` binds the authority and persists through its own
   bounded `trauma_bond` checksummed save section.
2. Its three host hooks route to the **existing canonical owners**:
   `AdjustAffinity` → `SurvivorRelationsSystem.ModifyAffinity`,
   `AreOnSameShift` → the duty roster's real shift assignment,
   `GetDay` → the canonical campaign day clock. No affinity, shift, or clock copy.
3. `GetCoShiftEfficiencyBonus` is consumed by the duty roster through its
   existing `DutyRosterSystem.WorkSpeedMultiplierLookup` seam, **composed with**
   the needs-performance multiplier rather than replacing it.
4. Bond decay ticks once per canonical day in a phase-5 day owner with pre-day
   snapshot rollback.
5. `--trauma-bond-selftest` proves hook routing to the real owners, exactly-once
   affinity application, bond decay and expiry, co-shift bonus gating, save
   round-trip, and reset.

**Non-goals (hard boundaries):** no second affinity store; no second roster; no
new RNG stream (bond strength changes are deterministic and integer/fixed-point
free of wall-clock); no UI panel in this package (a survivor-detail row is a
follow-on); no Unity.

---

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
|---|---|
| Authority is complete with save state | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` — `TraumaBondSaveState`, `CaptureState`, `RestoreState`, `OnSharedHazardEndured`, `Tick`, `GetCoShiftEfficiencyBonus` |
| Authority has **zero** host references | `grep -rn TraumaBondSystem src/` → no hits |
| Two Core test suites exist and pass | `Ashfall.Core.Tests/TraumaBondSystemTests.cs`, `Ashfall.Core.Tests/Shelter/Plan12_27SocialAutopsyIntegrationTests.cs` |
| Affinity owner exists and is hosted | `SurvivorRelationsSystem.ModifyAffinity(a, b, delta)` via `src/Host/SurvivorRelationsHostSession.cs` |
| Shift owner exists | `DutyRosterSystem` (`duty_roster` section, hosted) |
| Work-speed composition seam exists | `DutyRosterSystem.WorkSpeedMultiplierLookup` (already composed by `src/Main.DutyRoster.cs:47`) |
| Campaign day clock exists | `Main.CampaignDay` / `_campaignDay.Rng` |

---

## 3. Files

**New:** `src/Host/TraumaBondHostSession.cs`, `src/Host/TraumaBondSaveStore.cs`,
`src/Host/HostCli.TraumaBond.cs`, `src/Main.TraumaBond.cs`,
`Ashfall.Core.Tests/Survivors/PlanTraumaBondHostIntegrationTests.cs`

**Edited:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (`trauma_bond`
section + filename, section-count pin bump),
`Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`,
`src/Main.Application.cs`, `src/Main.CampaignOwners.cs` (phase-5 day owner with
pre-day snapshot rollback), `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`,
`src/Main.DutyRoster.cs` (compose the co-shift bonus into the work-speed lookup),
`docs/architecture/ARCHITECTURE_TEST_MAP.md`.

---

## 4. Acceptance

Host + Core test builds 0 errors / 0 warnings; `--trauma-bond-selftest` green;
`TraumaBondSystemTests` and `Plan12_27SocialAutopsyIntegrationTests` still green;
section-count pin updated with a ledger comment.


---

## 5. Closeout evidence (2026-09-26)

The Core authority named in this plan is now bound to the live canonical owners
and verified headless. Exact commands and results:

1. **Host seam:** `src/Host/TraumaBondHostSession.cs` + `TraumaBondSaveStore`
   (section `trauma_bond`, `trauma_bond_save.json`) bind the authority. All three of
   its host hooks route to existing canonical owners: `AdjustAffinity` ->
   `SurvivorRelationsSystem.ModifyAffinity`, `AreOnSameShift` -> the duty roster's
   assignment entries, `GetDay` -> the canonical campaign day clock.
2. **Exactly one authority per concern:** no affinity copy, no roster copy, no clock
   copy. `GetCoShiftEfficiencyBonus` is composed into the duty roster's existing
   `DutyRosterSystem.WorkSpeedMultiplierLookup` seam — it **multiplies onto** the
   needs-performance composition in `src/Main.DutyRoster.cs` instead of replacing it,
   so neither authority is shadowed. The shared-shift gate is enforced by the host
   session against the canonical shift owner (the Core authority exposes
   `AreOnSameShift` as a host seam and gates only on bond strength — asserted in
   `ShiftHook_IsAHostSeam_TheAuthorityDoesNotConsultItItself`).
3. **CLI probe:** `--trauma-bond-selftest` (**11/11 PASS** headless) covering hook
   routing to the real affinity owner, exactly-once affinity per pair, bond-strength
   scaling, co-shift gating (shared shift only), daily decay, full expiry without a
   permanent ledger, save round-trip, deterministic double restore, and reset that
   leaves the canonical affinity owner untouched.
4. **Tests:** `PlanTraumaBondHostIntegrationTests` **8/8 PASS**; pre-existing
   `TraumaBondSystemTests` and `Plan12_27SocialAutopsyIntegrationTests` still green.
5. **Lifecycle:** registered save section `trauma_bond` (part of the 302 -> 305 pin
   bump), phase-5 day owner with `IPreDaySnapshotRestore` pre-day rollback, setup on
   both campaign paths, save mirror, lifecycle reset.
6. **Generated artifacts:** selftest manifest, CLI catalog, save-store matrix, port
   contract, and the `ARCHITECTURE_GRAPH` entry for `trauma_bond`.

**Deferred with named reasons:** no shared-hazard producer is bound yet — no canonical
"the whole shelter endured this hazard together" fact exists today (disaster response
and faction-war raid are separate owners), so the seam stays unbound rather than
wired to a fabricated producer, exactly as Plan 42's `visitor_arrived` was left
unbound. A survivor-detail bond row is a presentation follow-on.
