# PLAN 124 — CVD Diamond Closeout (Phase 12)

**Verb:** `--cvd-diamond-selftest` (alias) · **Tool economy:** `docs/shelter/PLAN_124_DIAMOND_TOOL_ECONOMY.md` · **Authority map:** `docs/shelter/PLAN_124_DIAMOND_AUTHORITY_MAP.md`

## 1. Files changed

| Layer | Files |
|---|---|
| Data | `cvd_diamond_catalog.json` (schema_version 1) |
| Core | `CvdDiamondCatalog.cs`, `CvdDiamondSynthesisEngine.cs` (growth/defect/grade/consumer engine, Capture/Restore, `ActionResult<T>` typed result) |
| Host | `CvdDiamondHostSession.cs` (typed consumer-wear seam), `CvdDiamondSaveStore.cs`, Main wiring, harness/soak |
| UI | `CvdDiamondPanel.cs` |
| Save | `cvd_diamond` row (owner `shelter`) + `cvd_diamond_save.json` |
| Tests | `Plan124CvdDiamondCatalogTests` (10), `Plan124CvdDiamondSynthesisEngineTests` (17), persistence subset |

## 2. Catalog entries

4 growth grades (rejected sentinel / utility 7000 / industrial 4000 / master 2500 + certification), 5 defect profiles, 1 reactor profile, 2 feed profiles, 2 substrate profiles, 3 tool components, 2 consumer mappings, 1 maintenance profile. Validators: dup-ID + rank-ordering (sentinel wear-0 rule), non-circular component→grade / consumer→component FKs, **no consumer-less outputs** (outputs need registered consumers), item FKs validator-walked.

## 3. Tests added (27 focused cases)

Catalog: structural validation, grade ordering, **no-zero-wear-to-production invariant**, consumer-required outputs, item-FK presence, dup-ID + rank collision, determinism. Engine: input gating (feedstock/substrate/power/cooling typed failures), degraded-equipment start gate, growth completion + wear, purity-outgrades, **defect penalties with never-zero risk**, same-seed parity, **master-grade certification gate** (`CertificationUnavailable` without metrology), double-consume idempotency (no reward duplication), rejected batches produce no output, registered-consumer wear reduction inside the 2×–3.5× band and never zero, unregistered consumer gets nothing, lathe rejects utility inserts, plasma-collapse fault (batch lost + chamber damage), maintenance restore, 120-day soak + replay parity.

## 4. Selftests run

41/41 (consumer wear registry gates), 23/23 (75-day stage D/G), 24/24 (120-day tool economy: 10 started/9 consumed, grade distribution master 2/industrial 3/utility 4), data-integrity 317/317.

## 5. Save/migration matrix

| Save point | Reload | Gate |
|---|---|---|
| missing section | fresh reactor | `Cvd_null_save_is_clean_migration` |
| active growth batch | progress + equipment state preserved | `Cvd_roundtrip_preserves_midgrowth_batch` + `cvd_save_roundtrip` (75-day harness) |
| degraded reactor | condition preserved | soak bounds |
| failed batch | rejected-without-output preserved | rejection gate |
| certified output | certification preserved; **no reward duplication** | `Consuming_twice_yields_no_second_reward` |
| unknown profile | guarded (clamps) | RestoreState |

## 6. Determinism evidence

Same-seed parity over 3-batch sequences; 120-day soak replay parity; 75-day harness `stageH_diamond_hash`. Growth-chain over-division bug (progress collapsing to ~1/tick) was caught by the batch-completion tests and fixed to a single-round float factor.

## 7. Content-utilization evidence

Reactor install/repair items (`item_high_vacuum_pump`, `item_vacuum_pump_oil`, `item_ebpvd_vacuum_pump_seal`), feedstock (`item_biofuel_high/generator_grade`), substrates (`item_cast_borosilicate_glass_blank`, `item_superalloy_turbine_blade_blank`), and tool blanks (`item_foundry_drill_blanks`) all resolve in `items.json` (validator-walked). Consumer registry: `consumer_deep_excavation_cutter` + `consumer_precision_lathe_insert` registered at composition; **unregistered consumers receive no benefit** (typed seam, test-gated). Optical-window consumer: none live → deferred per plan §6.10 (not authored).

## 8. Scene-binding evidence

Route `cvd_diamond` registered + routed; PanelRouteGate 20/20; lifecycle 16/16; a11y PASS; scene lint 30/0. Panel shows active batch, plasma stability, expected-grade context, degraded equipment — no tuning controls (plan §15.3).

## 9. Known limitations

- Excavation downtime accounting (cutter-replacement savings in `ExcavationSystem` terms) applies through the typed wear seam at the condition sink; scenario-level effect is demonstrated in the 75-day harness, not a standalone downtime ledger.
- Optical-window consumer deliberately deferred (no live consumer).
- **Second-tool review (§21) pending.**

## 10. Follow-ups

Wire the workshop/excavation consumers to consume produced inserts through inventory; certification flow UI hook to `PrecisionMetrologySystem` grades; second-tool review (PERFORMED 2026-09-13, PASSED — `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md`).
