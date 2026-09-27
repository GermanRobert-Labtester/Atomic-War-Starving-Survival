# PLAN-RELATIONSHIP-BANDS — Authored Affinity Bands Reach the Live Relations Owner
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — authored data bound · existing owner seam used · live composition site wired · CLI probe green · focused tests green · no parallel authority · no new save section or day event.**

## Integrated evidence

Authored `relationship_bands.json` (5 bands) now replaces the relations owner's built-in band table through `SurvivorRelationsSystem.LoadBandsCatalog`, bound where the single live `SurvivorRelationsSystem` is composed in `src/Main.ShelterSocial.cs`.

Bands are live gameplay authority: the owner's band loop turns affinity into caregiving/training/morale modifiers, and until now those thresholds existed only as C# literals while the JSON sat unconsumed. Snake_case mapping is proven non-zero (bonded +0.25, hostile −0.20) and empty/invalid payloads cannot strip the owner's table.

Probe `--relationship-bands-selftest`: 9/9. Pre-existing `Plan44SurvivorRelationsIntegrationTests` (11 tests) remains green.

## Original plan body (preserved for the record)

> **Package:** `relationships`
> **Category:** RELATIONSHIP-BANDS
> **Plan type:** bind authored game data to the existing, designed seam on its live owner.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`
> **Claim:** `claim-quad-i-relationships-2026-09-26`

## 1. Objective
Make one already-designed Core bind seam actually receive its authored data, so
JSON remains authoritative (AGENTS.md rule 3) without creating a second owner.

## 2. Current evidence (verified in source, this session)
- `SurvivorRelationsState.LoadBandsCatalog(RelationshipBandsCatalog)` (Assets/Ashfall.Core/SurvivorRelationsSystem.cs:184) has **zero callers** anywhere in `src/` or `Assets/`.
- The only other writer of `_bands` is `InitializeDefaultBands()` (:104-:167), which hardcodes the five bands.
- `_bands` **is** consumed: the band loop at :238 turns affinity into caregiving/training/morale modifiers, so band data is live gameplay authority.
- `Assets/StreamingAssets/Data/relationship_bands.json` authors the same five bands (`hostile`, `strained`, `cordial`, `close`, `bonded`) with thresholds and modifiers, and `RelationshipBandsCatalog`/`RelationshipBandDefinition` already carry the `[JsonPropertyName]` snake_case mapping (Relations/RelationshipEffect.cs:55-:32).

## 3. Design decision
Read the authored file and hand it to the owner's existing `LoadBandsCatalog` seam at composition time (`src/Main.ShelterSocial.cs:51` creates the single live `SurvivorRelationsSystem`). Authored thresholds become the truth and future data edits take effect; no band table is duplicated in the host.

## 4. Non-goals
* No new save section, no new day-event heartbeat, no new Core system, no new catalog.
* No gameplay-rule invention: only data that already exists in `Assets/StreamingAssets/Data/`.
* No UI surface is added unless the command already exists on the owner.

## 5. Implementation surface
- `src/Main.PackageIBindings.cs` — `BindAuthoredRelationshipBands(state)`, called where `srSys` is constructed.

## 6. Verification (bounded)
* One headless host probe: `--relationship-bands-selftest`
* One focused engine-free Core suite: PLAN_RELATIONSHIP_BANDS_HOST_INTEGRATION.md0
* Adjacent gates only where this package can move them.

## 7. Acceptance
Authored rows are reachable through the live owner; the probe and suite pass; the
existing owner remains the single authority; no duplicate ledger/cache is created.
