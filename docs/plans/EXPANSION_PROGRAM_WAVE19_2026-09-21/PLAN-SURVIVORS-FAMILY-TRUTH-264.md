# PLAN-SURVIVORS-FAMILY-TRUTH-264 — Consent, Fitness & Store Architecture

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SURVIVOR-ROSTER-TRUTH-244, PLAN-VOLUNTARY-REGISTER-TRUTH-253, PLAN-CAREGIVING-TRUTH-203.
**Non-goals:** no new survivor systems; the family is audited for store
architecture and gate integrity.

## 1. Outcome
**29 `Survivors/` files** are referenced by no plan, including
`ISurvivorComponentStore`, `CrewConsentVerdict`, `FitnessForDutyModel`,
`GenealogyBridge`, `FinalWishCatalog`, `GuiltSourceCatalog`. The component-store
interface is architectural: if stores disagree, every survivor view can drift.

| Deliverable | Detail |
|---|---|
| Store map | component-store implementations and their owners; one authority per component |
| Fitness/consent gates | `FitnessForDutyModel` and `CrewConsentVerdict` inputs documented and routed to Plans 101/253 |
| Genealogy bridge | the bridge to family records (Plan 43) tested with a fixture |
| Catalog coverage | FinalWish/Guilt catalogs resolve through loaders (Plans 200/246) |
| Test presence | family files without fixtures get one |

## 2. Evidence
- 29 `Survivors/` basenames absent from every plan body (Wave 19 file-level audit).
- Plans 244/253/203 own the registry, consent, and care; this plan wires the shared architecture beneath them.
- Plan 141's ledger and Plan 43's relations are bridge counterparts.

## 3. Packages
- **SFT-264A** store map + single-authority proof.
- **SFT-264B** fitness/consent gate tests.
- **SFT-264C** genealogy bridge fixture.
- **SFT-264D** catalog→loader resolution test.
- **SFT-264E** fixture pass.

## 4. Acceptance & verification
- No component has two stores; gates produce documented verdicts; catalogs resolve.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Store duplication → single-authority proof is the guard.
Gate opacity → verdict inputs/outputs are documented and tested.

---

## 6. Expanded census (72 files in scope · 25,313 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Survivors/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
System 40 · Support 20 · Catalog 5 · Loader 4 · Save 1 · DTO/Type 1 · Demo 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `AgingSystem.cs` | 304 | System | 0 | 1 | 2 | since-mentioned |
| `AntenatalMaternalHealthEngine.cs` | 321 | System | 0 | 0 | 0 | since-mentioned |
| `BackstorySystem.cs` | 364 | System | 0 | 1 | 2 | since-mentioned |
| `CaregivingSystem.cs` | 393 | System | 0 | 0 | 2 | since-mentioned |
| `ChildDevelopmentSystem.cs` | 354 | System | 0 | 1 | 2 | since-mentioned |
| `CombatTraumaSystem.cs` | 255 | System | 0 | 0 | 3 | since-mentioned |
| `CrewConsentVerdict.cs` | 28 | Support | 0 | 0 | 0 | since-mentioned |
| `DesperationSystem.cs` | 289 | System | 0 | 0 | 2 | since-mentioned |
| `DreamSystem.cs` | 300 | System | 0 | 0 | 2 | since-mentioned |
| `ExerciseSystem.cs` | 416 | System | 0 | 0 | 2 | since-mentioned |
| `FinalWishCatalog.cs` | 115 | Catalog | 0 | 0 | 0 | since-mentioned |
| `FinalWishCatalogLoader.cs` | 67 | Loader | 0 | 0 | 0 | since-mentioned |
| `FinalWishSystem.cs` | 399 | System | 0 | 0 | 3 | since-mentioned |
| `FitnessForDutyModel.cs` | 910 | Support | 0 | 0 | 0 | since-mentioned |
| `GenealogyBridge.cs` | 183 | Support | 0 | 0 | 0 | since-mentioned |
| `GenerationalSystem.cs` | 456 | System | 0 | 0 | 2 | since-mentioned |
| `GuiltInsomniaSystem.cs` | 225 | System | 0 | 0 | 2 | since-mentioned |
| `GuiltSourceCatalog.cs` | 107 | Catalog | 0 | 0 | 0 | since-mentioned |
| `HiddenAgendaSystem.cs` | 441 | System | 0 | 0 | 2 | since-mentioned |
| `HobbySystem.cs` | 344 | System | 0 | 0 | 2 | since-mentioned |
| `ISurvivorComponentStore.cs` | 93 | Support | 0 | 0 | 0 | since-mentioned |
| `IdeologicalFrictionEvents.cs` | 653 | Support | 0 | 0 | 2 | since-mentioned |
| `IdeologicalFrictionSystem.cs` | 164 | System | 0 | 0 | 2 | since-mentioned |
| `InterpersonalConflictSystem.cs` | 657 | System | 0 | 0 | 2 | since-mentioned |
| `LaborProductivity.cs` | 206 | Support | 0 | 0 | 0 | since-mentioned |
| `LatentExpertAwakeningSystem.cs` | 241 | System | 0 | 0 | 2 | since-mentioned |
| `LeadershipSystem.cs` | 663 | System | 0 | 0 | 2 | since-mentioned |
| `MemorialComponentAdapter.cs` | 139 | Support | 0 | 0 | 1 | since-mentioned |
| `MemorialComponentParity.cs` | 388 | Support | 0 | 0 | 0 | since-mentioned |
| `MemorialComponentStore.cs` | 301 | Support | 0 | 0 | 5 | since-mentioned |
| `MoralBranchingSystem.cs` | 295 | System | 0 | 0 | 3 | since-mentioned |
| `MoraleContagionCatalog.cs` | 65 | Catalog | 0 | 0 | 0 | since-mentioned |
| `MoraleContagionSave.cs` | 199 | Save | 0 | 0 | 0 | since-mentioned |
| `MoraleContagionSystem.cs` | 770 | System | 0 | 0 | 4 | since-mentioned |
| `NeedsComponentParity.cs` | 340 | Support | 0 | 0 | 0 | since-mentioned |
| `NeedsComponentStore.cs` | 322 | Support | 0 | 0 | 5 | since-mentioned |
| `NeedsModifierStack.cs` | 239 | Support | 0 | 0 | 0 | since-mentioned |
| `NeedsPerformanceBridge.cs` | 476 | Support | 0 | 0 | 0 | since-mentioned |
| `NeedsSystem.cs` | 441 | System | 0 | 0 | 1 | since-mentioned |
| `PersonalBelongingsSystem.cs` | 584 | System | 0 | 0 | 2 | since-mentioned |
| `PsychologicalArcSystem.cs` | 560 | System | 0 | 0 | 2 | since-mentioned |
| `RationConflictSystem.cs` | 196 | System | 0 | 0 | 2 | since-mentioned |
| `RecruitmentSystem.cs` | 367 | System | 0 | 0 | 2 | since-mentioned |
| `RelationshipDecaySystem.cs` | 428 | System | 0 | 0 | 2 | since-mentioned |
| `RomanceFamilySystem.cs` | 655 | System | 0 | 0 | 2 | since-mentioned |

… and 27 more files in scope.

**Census totals:** 3 banned nondeterministic references · 5 empty-catch sites · 46 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `starting_survivors.json` | object[2 keys] |
| `year_of_ash_survivors.json` | object[2 keys] |
| `survivors.json` | object[2 keys] |

**State surfaces (capture/restore present):**

- `AgingSystem.cs`
- `BackstorySystem.cs`
- `CaregivingSystem.cs`
- `ChildDevelopmentSystem.cs`
- `CombatTraumaSystem.cs`
- `DesperationSystem.cs`
- `DreamSystem.cs`
- `ExerciseSystem.cs`
- `FinalWishSystem.cs`
- `GenerationalSystem.cs`
- `GuiltInsomniaSystem.cs`
- `HiddenAgendaSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Files referenced by tests | 403 name references across the test tree |
| Determinism scan | 3 banned references to fix or justify |
| Failure scan | 5 empty-catch sites to route through Plan 35's rules |
| Drift | 72 of 72 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

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
(45 files). Other plans referencing those names: **28**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 8 |
| `PLAN-FAMILY-DYNASTY-43` | 6 |
| `EVIDENCE` | 5 |
| `PLAN-RECREATION-MORALE-50` | 4 |
| `PLAN-MUTATION-HEREDITY-81` | 4 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 4 |
| `PLAN-SILENT-FAILURE-35` | 3 |
| `PLAN-MORALE-CONTAGION-TRUTH-162` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SFT-264A` | `ISurvivorComponentStore.cs`, `MemorialComponentStore.cs`, `NeedsComponentStore.cs` |
| `SFT-264B` | `CrewConsentVerdict.cs`, `FitnessForDutyModel.cs` |
| `SFT-264C` | `GenealogyBridge.cs`, `NeedsPerformanceBridge.cs` |
| `SFT-264D` | `FinalWishCatalogLoader.cs`, `FinalWishCatalog.cs`, `GuiltSourceCatalog.cs` |
| `SFT-264E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 45; intra-domain edges: **23**; isolated files:
**22**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AntenatalMaternalHealthEngine` | `CaregivingSystem` |
| `AntenatalMaternalHealthEngine` | `ChildDevelopmentSystem` |
| `ChildDevelopmentSystem` | `GenerationalSystem` |
| `DesperationSystem` | `NeedsSystem` |
| `FinalWishCatalog` | `FinalWishCatalogLoader` |
| `FinalWishCatalog` | `FinalWishSystem` |
| `FinalWishCatalogLoader` | `FinalWishCatalog` |
| `FitnessForDutyModel` | `ExerciseSystem` |
| `GenerationalSystem` | `ChildDevelopmentSystem` |
| `GenerationalSystem` | `NeedsSystem` |
| `IdeologicalFrictionEvents` | `IdeologicalFrictionSystem` |
| `MemorialComponentAdapter` | `MemorialComponentStore` |
| `MemorialComponentParity` | `MemorialComponentStore` |
| `MemorialComponentStore` | `ISurvivorComponentStore` |
| `MoraleContagionCatalog` | `MoraleContagionSystem` |
| `MoraleContagionSystem` | `NeedsSystem` |
| `NeedsComponentParity` | `NeedsComponentStore` |
| `NeedsComponentParity` | `NeedsSystem` |
| `NeedsComponentStore` | `ISurvivorComponentStore` |
| `NeedsComponentStore` | `NeedsSystem` |
| `NeedsModifierStack` | `NeedsSystem` |
| `NeedsSystem` | `NeedsModifierStack` |
| `PsychologicalArcSystem` | `NeedsSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `NeedsSystem` | 7 |
| `ChildDevelopmentSystem` | 2 |
| `ISurvivorComponentStore` | 2 |
| `MemorialComponentStore` | 2 |
| `CaregivingSystem` | 1 |
| `ExerciseSystem` | 1 |
| `FinalWishCatalog` | 1 |
| `FinalWishCatalogLoader` | 1 |
| `FinalWishSystem` | 1 |
| `GenerationalSystem` | 1 |

**Class split:** hub 9 · sink 5 · source 9 · isolated 22.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 45. Host files: **51** · Test files: **118** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 51 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/CaregivingHostSession.cs`, `src/Host/CollectibleEffectDispatcher.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 118 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/BalanceRestRadiationTests.cs`, `Ashfall.Core.Tests/BalanceTelemetryHarnessTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/slice_seven_days.json`, `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **24** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caregiving` |
| `child_development` |
| `combat` |
| `desperation` |
| `duty_roster` |
| `events` |
| `forced_labor` |
| `hidden_agenda` |
| `memorial` |
| `mental_health_crisis` |
| `moral_choice` |
| `morale` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--hidden-agenda-selftest` |
| `--hidden-agendas-selftest` |
| `--memorial-wall-selftest` |
| `--moral-choice-selftest` |
| `--performance-selftest` |
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **41**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnCaregivingBondDeepened` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingDialogueUnlocked` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingEnded` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingStarted` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnChildBooked` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnConflictStarted` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/desperation_events.json` |
| `Assets/StreamingAssets/Data/development_traits.json` |
| `Assets/StreamingAssets/Data/dream_templates.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (104 files, 810 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `DutyRoster` | 5 | 49 |
| `Events` | 1 | 6 |
| `Memorial` | 6 | 67 |
| `MoralChoice` | 4 | 33 |
| `Needs` | 4 | 21 |
| `Performance` | 10 | 47 |
| `Survivors` | 57 | 453 |
| `Verdict` | 7 | 50 |

**Verdict:** 810 cases sit under matching regions — run those first (`Combat`, `DutyRoster`, `Events`, `Memorial`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **257**
(24 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AutopsySaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **24**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caregiving` | no |
| `child_development` | no |
| `combat` | no |
| `desperation` | no |
| `duty_roster` | no |
| `events` | no |
| `forced_labor` | no |
| `hidden_agenda` | no |
| `memorial` | no |
| `mental_health_crisis` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `combat` |
| `duty_roster` |
| `events` |
| `moral_choice` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **56**
(CODEX_ONLY 11, GAMEPLAY_CONSUMED 29, OPTIONAL 6, UNRESOLVED 10).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `desperation_events.json` | GAMEPLAY_CONSUMED |
| `development_traits.json` | GAMEPLAY_CONSUMED |
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |

**Verdict:** 10 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 28
**Surface:** save sections 24 (laddered 0) · RNG streams 4 · host files 17 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SURVIVORS-FAMILY-TRUTH-264
wave: 19
status: PROPOSED — foreman claim required
packages: SFT-264A, SFT-264B, SFT-264C, SFT-264D, SFT-264E
claim paths:
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecuritySaveStore.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/AmputationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/backstory_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --combat-breaching-selftest
dependencies:
  - coordinate: 28 other plan(s) name these artifacts (§12)
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
