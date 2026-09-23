# PLAN-CODEX-SURFACE-TRUTH-110 — Every Codex Entry Reachable, Accurate & Current

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DATA-AUTHORITY-14, PLAN-DATA-CONSUMER-22, PLAN-REFERENCE-INTEGRITY-34.
**Non-goals:** no prose rewrite, no new lore, no second content store; codex
entries are presented from existing catalogs/data.

## 1. Outcome
Plan 14 classified the catalog baseline and found **279 CODEX_ONLY** catalogs
among 538 — files consumed (if at all) only through codex-style presentation.
Plan 22 finds consumer gaps; Plan 34 validates references. The missing check is
player-facing: for each codex-eligible entry, is there a **reachable surface**
(codex screen, journal, field guide) and does it render the current authored
text rather than a stale copy or a placeholder?

| Deliverable | Detail |
|---|---|
| Eligibility table | per catalog: codex-eligible entries, surface (codex/journal/field guide/none), unlock source |
| Reachability proof | each surface row resolves to a route/panel and an entry id that renders; `none` rows are explicit decisions |
| Freshness | rendered text equals the authored record (no duplicated prose in panels); a changed record shows through without a rebuild inventing text |
| Unlock truth | an entry appears only via its documented unlock source (discovery, quest, research); no entry is visible by default |
| Placeholder scan | entries containing TODO/TBD/empty strings are listed and routed to their content owner |

## 2. Evidence
- Plan 14 Appendix A: 538 catalogs classified (GAMEPLAY_CONSUMED 162 · CODEX_ONLY 279 · OPTIONAL 24 · UNRESOLVED 73).
- `World/FieldGuideCatalog.cs` exists as a presentation catalog; journal routes exist in `Main.PlayerSurfaces.cs` (192 route ids per Plan 15 Appendix A).
- Plan 34 Appendix A lists the id families the codex surfaces reference.
- Plan 55 owns onboarding; this plan owns the reference surface behind it.

## 3. Packages
- **CST-110A** eligibility + surface table generated from catalogs and route registry; `--check` mode.
- **CST-110B** reachability probe: render each surface row's entry in a focused UI check.
- **CST-110C** freshness check: compare rendered string to authored record for a sampled set; mismatch fails.
- **CST-110D** unlock audit: default-hidden assertion + one unlock path per source class.
- **CST-110E** placeholder report to content owners (no rewrite in this plan).

## 4. Acceptance & verification
- Table covers the codex-classified catalogs; every `none` row is an explicit decision recorded.
- Sampled entries render current text; a deliberate record edit changes the surface without a duplicated literal.
- Hidden-by-default assertion passes for a fresh campaign.
- `bash scripts/run_test.sh` on the UI/data regions touched + the generated table `--check`.

## 5. Risks
Scope creep into content editing → placeholders are reported, not rewritten.
Stale route knowledge → the table is generated from the route registry at check time.

---

## 6. Expanded census (3 files · 760 lines)

Scope: `Assets/Ashfall.Core/UI/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CollectiblePresentationModel.cs` | 178 | Support | — | 0 | 0 | 0 |
| `CrisisPresentationCoordinator.cs` | 461 | System | — | 0 | 0 | 0 |
| `CrisisPresentationSnapshot.cs` | 121 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `codex_entries.json` | array[63] |
| `bunker_blueprints_codex.json` | object[3 keys] |
| `bunker_court_verdicts_codex.json` | object[3 keys] |
| `culinary_ration_codex.json` | object[3 keys] |
| `oral_lore_codex.json` | object[3 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/UI/` |
| Test references | 5 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 3. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-UI-CONTRACT-FAMILY-TRUTH-277` | 3 |
| `PLAN-CRISIS-DISASTER-RESPONSE-80` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CST-110A` | no name match — resolve at claim time |
| `CST-110B` | no name match — resolve at claim time |
| `CST-110C` | no name match — resolve at claim time |
| `CST-110D` | no name match — resolve at claim time |
| `CST-110E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.UiPanels.cs`, `src/UI/EmergencyResponseHud.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/CollectibleCardAccessibilityTests.cs`, `Ashfall.Core.Tests/CollectibleItemPresentationTests.cs`, `Ashfall.Core.Tests/CrisisPresentationCoordinatorTests.cs`, `Ashfall.Core.Tests/Plan194CrisisProducerWireTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `CrisisPresentationCoordinator` | `CrisisPresentationSnapshot` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `mental_health_crisis` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--ui-snapshot-regenerate` |
| `--ui-snapshot-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/codex_entries.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json` |
| `Assets/StreamingAssets/Data/narrative/culinary_ration_codex.json` |
| `Assets/StreamingAssets/Data/narrative/oral_lore_codex.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (3 files, 34 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Codex` | 2 | 29 |
| `Presentation` | 1 | 5 |

**Verdict:** 34 cases sit under matching regions — run those first (`Codex`, `Presentation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **17**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CodexHostSession.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Journal/JournalCodex.cs` |
| `src/Main.BriefingCrisis.cs` |
| `src/Main.Codex.cs` |
| `src/Settings/AccessibilityPresentation.cs` |
| `src/UI/BlackMarketSnapshotFixture.cs` |
| `src/UI/DesperationCrisisPanel.cs` |
| `src/UI/EconomyMarketSnapshotFixture.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 4, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `codex_entries.json` | UNRESOLVED |
| `narrative/bunker_blueprints_codex.json` | CODEX_ONLY |
| `narrative/bunker_court_verdicts_codex.json` | CODEX_ONLY |
| `narrative/culinary_ration_codex.json` | CODEX_ONLY |
| `narrative/oral_lore_codex.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 10 · test regions 2 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CODEX-SURFACE-TRUTH-110
wave: 9
status: PROPOSED — foreman claim required
packages: CST-110A, CST-110B, CST-110C, CST-110D, CST-110E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CodexHostSession.cs  # §19 candidate host surface
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/CollectibleEffectDispatcher.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/codex_entries.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Codex/
  - godot --headless --path . -- --ui-snapshot-regenerate
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
