# PLAN-UI-CONTRACT-FAMILY-TRUTH-277 — Panel Registry, Surface Contracts & Modal Stack

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-UI-SURFACE-15, PLAN-HOST-COMPOSITION-GOVERNANCE-71, PLAN-ACCESSIBILITY-CLOSURE-51.
**Non-goals:** no panel redesign; the family is audited as the UI's own
plumbing.

## 1. Outcome
**10 `UI/` files** are referenced by no plan: `PanelRegistry`,
`PlayerSurfaceContract`, `PlayerSurfaceManifest`, `ModalStackController`,
`ConfirmationFlowGate`, `CrisisPresentationSnapshot`, `FactionIconCatalog`,
`CollectiblePresentationModel`. Registry/manifest/contract trios drift easily —
a panel can exist without a manifest row or a contract check.

| Deliverable | Detail |
|---|---|
| Registry truth | `PanelRegistry` ↔ `PlayerSurfaceManifest` ↔ route ids (Plan 15's inventory) agree; drift test |
| Contract enforcement | `PlayerSurfaceContract` rules are checked (or the check is wired) |
| Modal stack | stack discipline (push/pop/close) tested against Plan 51's focus rules |
| Confirmation gate | destructive actions pass through the gate; bypass report |
| Snapshot/model binding | presentation types read authority state only |

## 2. Evidence
- 10 `UI/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 15's route inventory (192 routes) is the registry counterpart.
- Plan 71's composition gates and Plan 51's focus behaviors are adjacent.

## 3. Packages
- **UCF-277A** registry/manifest/route drift test.
- **UCF-277B** contract check wiring.
- **UCF-277C** modal stack discipline tests.
- **UCF-277D** confirmation gate bypass report.
- **UCF-277E** presentation binding audit.

## 4. Acceptance & verification
- Zero registry/manifest/route drift; modal discipline holds; bypasses are reported.
- `bash scripts/run_test.sh` on the UI test region.

## 5. Risks
Silent panel drift → drift test.
Focus regression → modal tests reuse Plan 51 fixtures.

---

## 6. Expanded census (12 family files · 2,205 lines)

Scope: files under `Assets/Ashfall.Core/UI/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 10 · System 1 · Catalog 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `CollectiblePresentationModel.cs` | 178 | Support | 0 | 0 | 0 |
| `ConfirmationFlowGate.cs` | 58 | Support | 0 | 0 | 0 |
| `CrisisPresentationCoordinator.cs` | 461 | System | 0 | 0 | 0 |
| `CrisisPresentationSnapshot.cs` | 121 | Support | 0 | 0 | 0 |
| `FactionIconCatalog.cs` | 138 | Catalog | 0 | 0 | 0 |
| `ModalStackController.cs` | 160 | Support | 0 | 0 | 0 |
| `PanelRegistry.cs` | 283 | Support | 0 | 0 | 0 |
| `PanelRegistryBootstrap.cs` | 274 | Support | 0 | 0 | 0 |
| `PlayerSurfaceContract.cs` | 72 | Support | 0 | 0 | 0 |
| `PlayerSurfaceManifest.cs` | 205 | Support | 0 | 0 | 0 |
| `Theme.cs` | 192 | Support | 0 | 0 | 0 |
| `UiAssetManifest.cs` | 63 | Support | 0 | 0 | 0 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 0 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `field_guide.json` | array[38] |
| `breaching_equipment_catalog.json` | object[4 keys] |
| `fluid_infrastructure.json` | object[4 keys] |
| `microfluidic_diagnostic_catalog.json` | object[4 keys] |
| `rerailing_equipment_catalog.json` | object[2 keys] |
| `guilt_sources.json` | array[40] |

**State surfaces (capture/restore present):**

None — no save work is implied by this family.

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/UI/` |
| Family files referenced by tests | 53 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(12 files). Other plans referencing those names: **4**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-UI-SURFACE-15` | 4 |
| `PLAN-CODEX-SURFACE-TRUTH-110` | 3 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-CRISIS-DISASTER-RESPONSE-80` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `UCF-277A` | `PanelRegistry.cs`, `PanelRegistryBootstrap.cs`, `PlayerSurfaceManifest.cs` |
| `UCF-277B` | `PlayerSurfaceContract.cs` |
| `UCF-277C` | `ModalStackController.cs` |
| `UCF-277D` | `ConfirmationFlowGate.cs` |
| `UCF-277E` | `CollectiblePresentationModel.cs`, `CrisisPresentationCoordinator.cs`, `CrisisPresentationSnapshot.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 12. Host files: **251** · Test files: **27** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 251 | `src/Dose/DoseRegisterSurface.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 27 | `Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs`, `Ashfall.Core.Tests/CollectibleCardAccessibilityTests.cs`, `Ashfall.Core.Tests/CollectibleItemPresentationTests.cs`, `Ashfall.Core.Tests/CrisisPresentationCoordinatorTests.cs`, `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `faction_espionage` |
| `mental_health_crisis` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--port-contract-selftest` |
| `--selftest-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **9**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (4 files, 24 cases).

| Region | Files | Cases |
|---|---:|---:|
| `InformationFlow` | 3 | 19 |
| `Presentation` | 1 | 5 |

**Verdict:** 24 cases sit under matching regions — run those first (`InformationFlow`, `Presentation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **267**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/SurfaceAmbienceController.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/AssetRegistry.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `faction_espionage` | no |
| `mental_health_crisis` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **21**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 13, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 14 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-UI-CONTRACT-FAMILY-TRUTH-277
wave: 19
status: PROPOSED — foreman claim required
packages: UCF-277A, UCF-277B, UCF-277C, UCF-277D, UCF-277E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/SurfaceAmbienceController.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/asset_registry.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/InformationFlow/
  - godot --headless --path . -- --asset-coverage-report
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
