# D1 — Unblock Verification Truth — Premise Evidence

**Task:** Wave 8 Part 2, TASK D1 (`Seal-steps/847219_ASHFALL_WAVE8_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`)
**Status:** EXECUTED
**Date:** 2026-09-17
**Base commit:** `033df2b7` (working tree carries the D1 changes below)
**Package:** `D1-VERIFICATION-TRUTH`
**Claims:** none (test fixtures/comments/docs only; no production behavior change)

## 1. Plan premise re-checked against HEAD

The plan's verified starting premises:

1. *"Five full-suite failures were inherited from the 210–213 handoff."* —
   **PARTIALLY STALE.** A sanctioned diagnostic full-suite run at HEAD
   (`dotnet test Ashfall.Core.Tests`, 11,697 total) found **8** standing
   failures, not 5. Four of the five named in the old reconnaissance note
   (`PLANS_66_69_RECONNAISSANCE.md`) had already been fixed; the real set had
   grown because later plans added mandatory catalog checks and content/data.
   Exact identities are recorded below.
2. *"A Plan 126–129 source header incorrectly says completed plans are
   pending."* — **NOT PROVEN.** `src/Main.Plans126_129.cs` says Plans 127–129
   (tethered recon drone, continuous steel casting, atmospheric lidar) "land in
   follow-up waves". No implementation of those three subsystems exists
   repo-wide by any name; the plan-number closeout docs present in the tree
   (`PLAN127_IMPLEMENTATION_LOG.md`, `PLAN128_COMPLETION_REPORT.md`,
   `PLAN_129_FOUNDRY_PRODUCTION_CLOSEOUT.md`) document **different subjects**
   (holdfast verdict ladder, faction identity, foundry production) — i.e.
   plan-number drift, not a completed drone/caster/lidar wave. Per D1 Phase 2.4
   ("no authoritative completion log ⇒ leave unchanged and record ambiguity"),
   the header was **left unchanged**.
3. *"Architecture map contains dozens of stale GAP rows."* — The generator
   `--check` is green at HEAD (193 subsystems, 100% end-to-end verified); any
   residual GAP rows are already accounted for by the generator inputs. No
   hand-editing was performed (D1 Gate 6).
4. *"Plan 24's 14/15 needs characterization issue is handled in Part 1/A4."* —
   Confirmed; it was repaired to 15/15 under the concurrent Part 1 wave and is
   not re-opened here.

## 2. Standing-failure baseline (Phase 0)

Command (the one sanctioned diagnostic full-suite run — `TEST_POLICY.md`):

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

Result before D1: **Failed 8 / Passed 11,689 / Total 11,697** (~42 s).

| # | Failure | Message | Classification |
|---|---|---|---|
| 1 | `MarketSystemTests.CorruptState_OldVersionMigratesPredictably` | expected version 2, actual 3 | STALE CONTRACT — Plan 14A raised `MarketState.Version` to 3 (nested embargo decay state); restore stamps the current version |
| 2 | `Plan56EconomyGoodsTests.Catalog_reaches_the_48_good_breadth_target` | expected 48, actual 50 | STALE CONTRACT — Wave 8 Plan 22 trade parity added `cloth` + `item_air_filter_hepa` |
| 3 | `Plan56FollowUpTests.Baseline40_AreUntouchedByTheCategoryFill` | expected 48, actual 50 | STALE CONTRACT — same 2 added goods |
| 4 | `VersionReportContractTests.AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes` | expected 186, actual 187 | STALE CONTRACT — live save registry advanced |
| 5 | `VersionReportContractTests.FormatPersistenceInventory_RendersSummaryAndEntries` | expected 192 sections, actual 193 | STALE CONTRACT — same registry |
| 6 | `MainTriadDriftGateTests.SetupWithoutSave_IsAllowlistedOrHasSaveTwin` | `Cascade`, `FitnessForDuty` lack a Save twin | STALE GATE ALLOWLIST — both are derived/read-only by design |
| 7 | `Save.B5B8Phase0SaveFixtureTests.SumpFlooding_Phase0Fixture_RoundTrips` | serialized shape drifted | DELIBERATE ADDITIVE FIELD — `SumpNode.lastNetLevelChangeCmPerDay` (C2[6] 23B); fixture needed re-capture |
| 8 | `DocLinkValidationGateTests.AuthorityDocs_RelativeLinksResolveToExistingFiles` | broken `docs/INDEX.md` links | STALE GENERATED INDEX — plan files moved (`531842` → `Seal-steps/Completed/`; Wave 12 → `Seal-steps/Unblocking-tasks/`) |
| 9 | `Host.Phase0EffectsBridgeTests.TradeSpecialty_CraftingItems_AdvancesTierAndMasters` | narrativeFired expected 1, actual 4 (intermittent) | TEST ISOLATION — shared static `TradeSpecialtySystem.ProfessionInfo` populated by parallel test classes |

Additional root cause not in the plan's list (discovered during triage): the
earlier-recorded catalog-scratch failures (`CatalogIntegrityValidatorTests` 4,
`MilitaryBranchCatalogTests` 1, `WildlifeTrappingCatalogIntegrityTests` 7,
`Plan144MoralChoiceStubClosureTests` 2 = 14) were a separate first tranche of
the same class: the validator gained whole-directory mandatory-catalog checks
(Plan 14A/14B/24A) after the isolated scratch tests were written. These were
fixed in the same sweep.

## 3. Exact paths / ownership

| Area | Paths | Claim state |
|---|---|---|
| Test fixtures/assertions | `Ashfall.Core.Tests/{CatalogIntegrityValidatorTests,MilitaryBranchCatalogTests,WildlifeTrappingCatalogIntegrityTests,Plan56EconomyGoodsTests,Plan56FollowUpTests,VersionReportContractTests,EconomySystemTests,Host/Phase0EffectsBridgeTests,World/CatalogIntegrityWeatherGateTests,MoralChoice/Plan144MoralChoiceStubClosureTests}.cs`, `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs`, `Ashfall.Core.Tests/IntegrityScratchFixture.cs` (new), `Ashfall.Core.Tests/Fixtures/B5B8_Phase0/sump_flooding_phase0.json` | unclaimed |
| Generated docs | `docs/INDEX.md` (regenerated) | unclaimed |
| Truth docs | `docs/plans/flagship_b5_b8/B5_B8_COMPLETION_REPORT.md`, `KNOWN_DEBT.md` | unclaimed |

No `Assets/Ashfall.Core/`, `src/`, or `Assets/StreamingAssets/Data/` file was
edited by D1.
