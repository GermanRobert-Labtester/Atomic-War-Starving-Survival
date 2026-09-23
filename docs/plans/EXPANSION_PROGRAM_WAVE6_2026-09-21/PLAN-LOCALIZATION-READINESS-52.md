# PLAN-LOCALIZATION-READINESS-52 — String Extraction, Freeze Policy & Pseudo-Locale

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-UNBLOCK-03 U3 (freeze), PLAN-NARRATIVE-GRAPH-18.
**Expanded appendix:** [`PLAN-LOCALIZATION-READINESS-52_APPENDIX-A_L10N_INVENTORY.md`](PLAN-LOCALIZATION-READINESS-52_APPENDIX-A_L10N_INVENTORY.md)
— the l10n inventory dump: **535 records**, 472 hardcoded literals by panel (LZ-52A conversion queue).

**Non-goals:** no shipping translations; DEC-13 stays deferred until its
condition is met; no machine-translated prose.

## Outcome
`DEC-13` is deferred "until UI string extraction and string freeze". The tree
already has `Localization/LocalizationService.cs`, `StringFreezePolicy.cs`,
`WildlifeTrappingLocalization.cs`, `extract_l10n_inventory.py`, and an
`artifacts/l10n-inventory.json`. This plan completes the *readiness* half so
the decision's condition becomes provably met.

| Deliverable | Detail |
|---|---|
| Extraction truth | every user-facing string reachable from catalogs/UI/CLI; no concatenated sentences; format holes named |
| Freeze policy | `StringFreezePolicy` enforced by a gate: post-freeze changes need a dated exception |
| Pseudo-locale | `qps`-style generated pseudo text (length +40%) used in a snapshot pass to catch overflow/truncation |
| Font coverage | the Barlow Condensed family plus a documented fallback chain; missing-glyph test for the pseudo-locale |
| Resource pipeline | one locator; keys stable; context strings for translators; plural rules per key |

## Evidence
- `Assets/Ashfall.Core/Localization/` (3 files incl. `StringFreezePolicy`).
- `scripts/ci/extract_l10n_inventory.py`; artifact `artifacts/l10n-inventory.json`.
- `assets/fonts/BarlowCondensed-*.ttf` (3+ weights).
- UI text sourced from catalogs and panel constants; snapshot corpus of 69 panels.
- Register rows: `DEC-11` (VO) / `DEC-13` (localization) deferred with conditions.

## Packages
- **LZ-52A** inventory completion: extraction covers narrative catalogs, UI constants, CLI help, and item/medical text; report per-file counts.
- **LZ-52B** freeze gate: `StringFreezePolicy` reads the freeze date; new hardcoded strings outside catalogs fail; exceptions recorded.
- **LZ-52C** pseudo-locale: generator + snapshot run; every overflow/truncation is a finding with panel + key.
- **LZ-52D** font/glyph test: pseudo-locale renders with the fallback chain; missing glyphs fail.
- **LZ-52E** translator context: key + source + context + length limit table export.

## Acceptance & verification
- Inventory delta reported; zero post-freeze hardcoded strings; pseudo-locale snapshot clean.
- `python3 scripts/ci/extract_l10n_inventory.py --check`; `godot --headless --path . -- --ui-a11y-selftest`; snapshot diff run.

## Risks
Pseudo-locale churn → run once per release; findings triaged, not mass-fixed.

---

## 6. Expanded census (bespoke: localization inventory)

This plan's subject is the string surface, so the census uses the l10n
inventory artifact rather than source filenames.

| Metric | Value |
|---|---:|
| Inventory records | 535 |
| Hardcoded literals | 472 |
| Localized lookups | 63 |
| UI files scanned | 214 |
| Pilot panels | ResearchPanel, OnboardingHintPanel |

**Top panels by hardcoded literals:**

| Panel | Literals |
|---|---:|
| `ElectrostaticScrubberPanel` | 18 |
| `RailwayTerminalPanel` | 15 |
| `SlurryDewateringSumpPanel` | 11 |
| `GeigerCalibrationPanel` | 10 |
| `AquiferTreatyConcessionPanel` | 9 |
| `BasalRadonMigrationPanel` | 9 |
| `ClandestineInsurgencyPanel` | 9 |
| `CrossingSafeConductVouchPanel` | 9 |
| `CryogenicPermafrostCorePanel` | 9 |
| `FungalProteinFermenterPanel` | 9 |

## 7. Expanded surface: extraction contract

| Rule | Detail |
|---|---|
| Keys | stable keys derived from panel + element; no literals left in panels |
| Coverage | UI extraction is complete when the inventory's hardcoded count is zero for player-facing surfaces |
| Pseudo-locale | longer locales must not clip controls (Plan 88's round) |
| Gates | a new hardcoded literal in a converted panel fails the check |

## 8. Expanded verification

| Check | Baseline: the inventory artifact regenerated after each conversion batch |
|---|---|
| Inventory delta | hardcoded count strictly decreases per batch |
| Key round-trip | a key renders its string in every registered locale |
| Pseudo-locale | sampled panels pass the expansion round |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Re-generate the inventory and freeze the baseline counts.
2. Convert the top panels by literal count.
3. Add the regression gate for converted panels.
4. Pseudo-locale round on the pilot set.
5. Regression: inventory delta recorded per batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Panel conversion | inventory literal count drops; zero for the panel |
| Key stability | keys unchanged across conversion |
| Gate | literal reintroduced in a converted panel fails |
| Locale round | string renders in every registered locale |

**Non-goals unchanged:** this expansion adds census and verification detail; no new localization framework.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 7. Other plans referencing them: **5**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-TEXT-PACK-LOCALIZATION-88` | 3 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-NARRATIVE-GRAPH-18` | 1 |
| `PLAN-CONTENT-PIPELINE-QA-77` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Localization/LocalizationService.cs` |
| `PLAN-LOCALIZATION-READINESS-52_APPENDIX-A_L10N_INVENTORY.md` |
| `StringFreezePolicy.cs` |
| `WildlifeTrappingLocalization.cs` |
| `artifacts/l10n-inventory.json` |
| `extract_l10n_inventory.py` |
| `scripts/ci/extract_l10n_inventory.py` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `LZ-52A` | `PLAN-LOCALIZATION-READINESS-52_APPENDIX-A_L10N_INVENTORY.md`, `artifacts/l10n-inventory.json`, `extract_l10n_inventory.py` |
| `LZ-52B` | `StringFreezePolicy.cs` |
| `LZ-52C` | no name match — resolve at claim time |
| `LZ-52D` | no name match — resolve at claim time |
| `LZ-52E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **4** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Localization/AshfallLocalization.cs`, `src/Main.ShelterSocial.cs`, `src/UI/TutorialPanel.cs`, `src/UI/WildlifeTrappingPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs`, `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs`, `Ashfall.Core.Tests/Localization/MicroLocationLocalizationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **24** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `expanded_shelter` |
| `inventory` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **25** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--journal-weather-panel-selftest` |
| `--narrative-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnFreezeAlarmTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/wildlife_ecosystem.json` |
| `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (9 files, 91 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Localization` | 4 | 21 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 91 cases sit under matching regions — run those first (`Localization`, `WildlifeTrapping`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **8**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/WildlifeEcosystemHostSession.cs` |
| `src/Host/WildlifeEcosystemSaveStore.cs` |
| `src/Host/WildlifeTrappingHostSession.cs` |
| `src/Host/WildlifeTrappingSaveStore.cs` |
| `src/Localization/AshfallLocalization.cs` |
| `src/UI/AshfallFocusPolicy.cs` |
| `src/UI/WildlifeTrappingPanel.cs` |
| `src/UI/WinterFreezePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `wildlife_ecosystem` | no |
| `wildlife_trapping` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `narrative/wasteland_wildlife_bestiary.json` | CODEX_ONLY |
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |
| `wildlife_trapping_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 2 (laddered 0) · RNG streams 4 · host files 12 · catalogs 7 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LOCALIZATION-READINESS-52
wave: 6
status: PROPOSED — foreman claim required
packages: LZ-52A, LZ-52B, LZ-52C, LZ-52D, LZ-52E
claim paths:
  - src/Host/WildlifeEcosystemHostSession.cs  # §19 candidate host surface
  - src/Host/WildlifeEcosystemSaveStore.cs  # §19 candidate host surface
  - src/Host/WildlifeTrappingHostSession.cs  # §19 candidate host surface
  - src/Host/WildlifeTrappingSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Localization/
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
