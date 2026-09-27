# PLAN-GENEALOGY-FAMILY-NAMES — Authored Family-Name Catalog Reaches the Live Lineage Owner
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — authored data bound · existing owner seam used · live composition site wired · CLI probe green · focused tests green · no parallel authority · no new save section or day event.**

## Integrated evidence

Authored `family_name_templates.json` now binds to the live lineage owner (`GenealogyHostSession.Lineage`) inside `SetupGenealogy()`. The extension reads `_familyNameCatalog` at three sites (archetype selection, template generation), but only `LoadFamilyNameCatalog` writes it and nothing ever called it — so authored cultural naming could not influence surnames at all.

Probe `--genealogy-family-names-selftest`: 9/9. Pre-existing `Plan217GenealogyIntegrationTests` (10 tests) remains green; no duplicate suite was created.

FINDING (Core owner semantics, deliberately NOT changed): a wrong `schema_version` is swallowed — the loader logs and sets the catalog to `null` instead of surfacing the authoring error. Re-binding restores authored behaviour, which the probe pins as the safe contract.

## Original plan body (preserved for the record)

> **Package:** `survivors`
> **Category:** GENEALOGY-FAMILY-NAMES
> **Plan type:** bind authored game data to the existing, designed seam on its live owner.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`
> **Claim:** `claim-quad-i-survivors-2026-09-26`

## 1. Objective
Make one already-designed Core bind seam actually receive its authored data, so
JSON remains authoritative (AGENTS.md rule 3) without creating a second owner.

## 2. Current evidence (verified in source, this session)
- `GenerationalLineageExtension.LoadFamilyNameCatalog(string)` (Assets/Ashfall.Core/GenerationalLineageExtension.cs:97) has **zero callers**.
- It is the **only** writer of `_familyNameCatalog` (:86), which is **read** at :162-:164 (cultural archetype selection) and :178 (name templates) — so the field is permanently `null` and every read falls through to built-in behaviour.
- `Assets/StreamingAssets/Data/family_name_templates.json` is authored with `schema_version: 1`, `cultural_archetypes`, and `templates` — the exact shape the loader validates (:105-:106 rejects any other schema).
- The lineage owner **is** live: `src/Host/GenealogyHostSession.cs:90,99` constructs and exposes `GenerationalLineageExtension Lineage`, and `src/Main.Genealogy.cs:42` hosts that session over the succession engine (`_expansions?.Generational`) — never a second engine.

## 3. Design decision
Bind the authored JSON into the existing session's own `Lineage` during `SetupGenealogy()`, after the succession engine is resolved. The restore path is untouched: the catalog is configuration, not save state, and lineage records already persist through `GenealogySaveStore`.

## 4. Non-goals
* No new save section, no new day-event heartbeat, no new Core system, no new catalog.
* No gameplay-rule invention: only data that already exists in `Assets/StreamingAssets/Data/`.
* No UI surface is added unless the command already exists on the owner.

## 5. Implementation surface
- `src/Main.PackageIBindings.cs` — `BindAuthoredFamilyNames()`, called from `SetupGenealogy()` in `src/Main.Genealogy.cs`.

## 6. Verification (bounded)
* One headless host probe: `--genealogy-family-names-selftest`
* One focused engine-free Core suite: PLAN_GENEALOGY_FAMILY_NAMES_HOST_INTEGRATION.md0
* Adjacent gates only where this package can move them.

## 7. Acceptance
Authored rows are reachable through the live owner; the probe and suite pass; the
existing owner remains the single authority; no duplicate ledger/cache is created.
