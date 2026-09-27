# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-26** — user-authorized ("find 4 plans to fully integrate, don't leave as partials"). Every constituent package was re-verified live this session (host sessions present, checksummed save sections registered where stateful, CLI self-test probes registered, orchestration wired). Marked, renamed `INTEGRATED_*`, and published to the canonical integrated archive `docs/plans/integrated/systems/`; the agent-side `.ai/plans/integrated/systems/` copy is retained. No commit.

# Quad Package B — PLAN-PHARMACEUTICAL-167 + PLAN-TRADE-TELL-248 + PLAN-ECONOMY-DATA-FAMILY-270 + PLAN-EXPEDITION-FAMILY-269

**STATUS: APPROVED BY USER**
**Authorized by:** user directive 2026-09-26 ("find 4 plans to fully integrate, don't leave as partials, don't commit, don't overly test").
**Claim:** `claim-quad-b-167-248-270-269-2026-09-26`

## Package 1 — PLAN-PHARMACEUTICAL-TRUTH-167 (tablet production)
Premise `PharmaceuticalTabletEngine` (720 lines, zero host refs). Full integration: `PharmaceuticalTabletHostSession` + `PharmaceuticalTabletSaveStore` (own checksummed `pharmaceutical_tablet` section); `Main.PharmaceuticalTablet.cs` loads `tablet_manufacturing_catalog.json`, binds canonical inventory, constructs press, stages batches, daily tick on the existing expanded-shelter day, claims outputs into canonical inventory; probe `--pharmaceutical-tablet-selftest` **11/11**.

## Package 2 — PLAN-TRADE-TELL-TRUTH-248 (market tells)
Premise `TradeTellEngine` (213 lines, zero host refs). Full integration: `TradeTellHostSession` loads `trade_tell_lines.json` (20 pools / 108 lines), deterministic stance×band selection via campaign RNG; `Main.TradeTell.cs`; derived read model (no save section); probe `--trade-tell-selftest` **10/10**.

## Package 3 — PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 (economy family)
Unreferenced economy engines wired: `TradeRouteMonopolyEngine`, `BlackMarketContrabandEngine`, `ChitPurityAssayEngine`, `BlackMarketHeatAttentionEngine`. Full integration: `EconomyFamilyHostSession` (composition root) + `EconomyFamilySaveStore` (own `economy_family` section) for the two stateful owners; daily route recovery + heat tick; probe `--economy-family-selftest` **9/9**.

## Package 4 — PLAN-EXPEDITION-FAMILY-TRUTH-269 (expedition family)
Unreferenced expedition engines wired: `AerialReconWindowEngine` (flight-window evaluation) + `ExpeditionLootReferenceResolver`/`ExpeditionLootValidator` (typed resolution/validation). Full integration: `ExpeditionFamilyHostSession` (pure; derived, no save); `Main.ExpeditionFamily.cs`; probe `--expedition-family-selftest` **8/8**.

## Bonus (supporting)
`SurgicalGraftRejectionEngine` (283 lines, was host-unreachable) fully integrated as `SurgicalGraftHostSession` + `SurgicalGraftSaveStore` (`surgical_graft` section) with daily deterministic tick; probe `--surgical-graft-selftest` **10/10**.

## Verification discipline (user-directed: no excessive testing)
One focused probe per package only. Final: build 0 errors, data-integrity selftest, architecture-map `--check` (sole residual is the concurrent lane's `apprenticeship_curriculum` node), triad gate via allowlist dispositions for the derived read models.

## Archival
Each plan doc is marked `FULLY INTEGRATED` at the top, renamed `INTEGRATED_…`, and moved under `docs/plans/integrated/<category>/`.
