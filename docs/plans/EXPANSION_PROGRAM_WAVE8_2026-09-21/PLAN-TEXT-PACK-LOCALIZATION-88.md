# PLAN-TEXT-PACK-LOCALIZATION-88 — Data-Driven Content Keys, Pseudo-Locale & Orphan-Key Gate

**Wave 8 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-LOCALIZATION-READINESS-52, PLAN-DATA-AUTHORITY-14, PLAN-NARRATIVE-GRAPH-18.
**Implementation scaffold:** [`PLAN-TEXT-PACK-LOCALIZATION-88_APPENDIX-A_SCAFFOLD.md`](PLAN-TEXT-PACK-LOCALIZATION-88_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-LOCALIZATION-READINESS-52` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no machine translation service, no runtime text generation, no
duplicate string tables beside the JSON authority.

## 1. Outcome
Plan 52 covers the **UI extraction pilot** (472 hardcoded literals in 214 UI
files per `artifacts/l10n-inventory.json`). The larger untracked surface is
**data-driven content**: 703 JSON files under `Assets/StreamingAssets/Data`
carry display text (journal entries, radio logs, quests, ceremonies,
broadsheets). Nothing today freezes content keys, detects orphan keys, or
proves a layout survives a longer locale.

| Deliverable | Detail |
|---|---|
| Content key census | display-string fields per top narrative catalog with a stable key derived from the record id + field |
| Key stability rule | display text is addressed by key; ids are frozen once referenced by save/flag/quilt state |
| Pseudo-locale round | a generated pseudo-locale (expanded length, bracketed) rendered through the UI without layout escape |
| Orphan/missing gate | a key referenced by any consumer but absent from the pack fails; a pack key with no consumer is reported (not silently kept) |
| Authoring guide | one page for writers: which fields are localizable, how keys are formed, what must not be inlined |

## 2. Evidence
- 703 JSON files under `Assets/StreamingAssets/Data` (content authority).
- `artifacts/l10n-inventory.json`: 535 records · 472 hardcoded literals · 63 localized lookups · 214 UI files; pilot panels `ResearchPanel`, `OnboardingHintPanel`.
- Narrative files are already consumed by systems (`YearOfAsh/QuestlineSystem`, `Narrative/LetterDeliverySystem`), so keys must not break save-referenced ids.
- Plan 52 owns UI extraction; this plan does not restate it.

## 3. Packages
- **LTC-88A** content key census: emit per-catalog key tables from data + consumer scan.
- **LTC-88B** stability rule + frozen-id list checked by a gate (id change without alias fails).
- **LTC-88C** pseudo-locale generator (script, not runtime) + one full panel pass under it.
- **LTC-88D** orphan/missing key gate: consumer↔pack comparison with per-row failure output.
- **LTC-88E** authoring guide `docs/l10n/CONTENT_KEYS.md` + one worked example per content type.

## 4. Acceptance & verification
- Census covers the top narrative catalogs with counts; ids in save-referenced
  catalogs verified frozen.
- Pseudo-locale run: zero clipped/overflowing labels in the sampled panels.
- Gate: injecting one orphan lookup fails with the catalog+record named.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/` plus the focused UI render check.

## 5. Risks
Key proliferation → keys are derived, never hand-invented; the census is
generated. Pseudo-locale false alarms → only text containers that already
declare wrapping participate.

---

## 6. Expanded census (2 files · 629 lines)

Scope: `Assets/Ashfall.Core/Localization/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LocalizationService.cs` | 595 | Support | — | 0 | 0 | 0 |
| `WildlifeTrappingLocalization.cs` | 34 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Localization/` |
| Test references | 6 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-LOCALIZATION-READINESS-52` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `LTC-88A` | no name match — resolve at claim time |
| `LTC-88B` | no name match — resolve at claim time |
| `LTC-88C` | no name match — resolve at claim time |
| `LTC-88D` | no name match — resolve at claim time |
| `LTC-88E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Localization/AshfallLocalization.cs`, `src/Main.ShelterSocial.cs`, `src/UI/TutorialPanel.cs`, `src/UI/WildlifeTrappingPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs`, `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs`, `Ashfall.Core.Tests/Localization/MicroLocationLocalizationTests.cs` |
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

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPackMigrated` | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

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

Host files (`src/`) whose names share a domain token: **6**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/WildlifeEcosystemHostSession.cs` |
| `src/Host/WildlifeEcosystemSaveStore.cs` |
| `src/Host/WildlifeTrappingHostSession.cs` |
| `src/Host/WildlifeTrappingSaveStore.cs` |
| `src/Localization/AshfallLocalization.cs` |
| `src/UI/WildlifeTrappingPanel.cs` |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 2 (laddered 0) · RNG streams 4 · host files 10 · catalogs 7 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TEXT-PACK-LOCALIZATION-88
wave: 8
status: PROPOSED — foreman claim required
packages: LTC-88A, LTC-88B, LTC-88C, LTC-88D, LTC-88E
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
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
