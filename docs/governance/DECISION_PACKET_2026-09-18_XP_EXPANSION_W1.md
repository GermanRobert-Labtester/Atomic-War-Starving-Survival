# XP Expansion W1 Decision Record

**Date:** 2026-09-18
**Authority:** user authorization: “Allright i authorise implementation!”
**Source plans:**
[`Part 1`](../../Seal-steps/ashfall-feature-expansion-proposal-part-1-pillars-xp-01-%E2%80%A6-xp-10.md)
and
[`Part 2`](../../Seal-steps/ashfall-expansion-integration-plan-part-2-xp-01-%E2%80%A6-xp-10-implementation.md)

## D1 — difficulty authority

**Approved:** `difficulty_presets.json` is the sole authored authority for
campaign difficulty. A Core `DifficultyDirector` resolves the immutable
preset ID into typed scalars. The proposed consumer list remains closed until
each live calculation site has been premise-checked.

**Current implementation boundary:** W1 first creates and validates the
catalog/director. Campaign binding, persistence, scalar consumers, and the
read-only chronicle projection are separate follow-on slices because the
current source contains no campaign header and the sealed completion history
is owned by the active Wave 11 claim.

## D2 — SOFC fuel owner

**Premise correction:** XP-05's proposed `FuelConsumer = units => true`
placeholder is absent at the current source revision. `SetupSofcPower` binds
`FuelConsumer` to `ConsumeSofcFuel`; that method consumes canonical inventory
items in quality order and then the existing power-grid fuel reserve. The
current `sofc_power_catalog.json` already owns fuel quality profiles.

**Decision:** retain the existing inventory/grid owners and existing SOFC save
section. W1 will verify the binding instead of adding `fuel_grades.json`, a
fuel buffer, or a second fuel save section.

## Deferred source correction

The existing completion history stores completed-run observations only. It
does not store campaign starts or a difficulty ID, so a projection containing
`runs_started` by difficulty cannot be a pure read model over it. The XP-01
chronicle scope remains deferred until the Wave 11 completion-history owner
transfers its claimed paths and records an append-only compatible data shape.
