# PLAN-UNBLOCK-03 — Decision-Unblock Programme: Signed-but-Unexecuted, Stale Ledgers & Audit-Pending Work

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required; ledger edits are
foreman-only.
**Owner role on execution:** Foreman/Integrator (ledger + register truth) with
Builders for the host-pending dispositions.
**Depends on:** PLAN-INTEGRATION-KIT-02 (reachability + ledger-truth gates)
for the enforcement half.
**Feeds:** PLAN-ORPHAN-SEAL-01 (the wiring waves), PLAN-LAUNCH-FACE-06
(Plan 37/48/53 execution).

---

## 1. Outcome

The queue is not blocked by missing decisions anymore. The decision register
contains **301 rows**; only **two** are non-terminal:

| Row | Subject | Condition | Unblock action |
|---|---|---|---|
| `DEC-11` | Voice-Over Audio Pipeline | full VO deferred until dialogue string freeze + bus loudness calibration | execute the string freeze (U3) and the audio calibration gate; then re-verdict |
| `DEC-13` | Localization Pipeline Expansion | multi-language catalog deferred until UI string extraction + string freeze | run the l10n inventory (`scripts/ci/extract_l10n_inventory.py`), freeze, then re-verdict |

Everything named "decision-blocked" in `AGENTS.md` (D11, D21, F13, F14,
EN-01…EN-08, Plan 49, C3 HOLDs 174/175/192/199, D22) now has a **terminal
disposition** in `docs/governance/DECISION_REGISTER.md` — mostly `SIGNED`
between DEC-21 and DEC-44. The real blocker is execution, not signatures: the
signed artifacts are frequently Core-only orphans (see PLAN-ORPHAN-SEAL-01).

**Outcome:** (a) ledger/rulebook truth repaired so agents stop obeying a stale
blocked list; (b) every signed-but-unexecuted disposition either integrated or
explicitly re-scoped; (c) the two genuinely open decisions driven to a verdict
with evidenced conditions; (d) an audited answer for the 111 `AUDIT-PENDING`
census rows; (e) the three "available" head plans (C2[15]/37, C2[21]/48,
E1/53) premise-audited and either started or returned `STALE_PLAN`.

**Expanded appendix:** [`PLAN-UNBLOCK-03_APPENDIX-A_REGISTER_INVENTORY.md`](PLAN-UNBLOCK-03_APPENDIX-A_REGISTER_INVENTORY.md)
— the full decision-register dump: **301 DEC rows** with subject, area, and verdict (the U-phase work list).

**Non-goals:** no new architecture decisions authored by a builder; no
decision reversal without foreman/user signature; no full-suite runs.

---

## 2. Premise evidence (2026-09-21)

### 2.1 Register vs rulebook drift
`AGENTS.md` (2026-09-19) still lists as decision-blocked: semantic-kind
re-grouping (D11), quarantine drain (D21), XP-04 economy legs (F13), XP-06
body-integrity schema (F14), EN-01…EN-08, Plan 49, C3 HOLDs 174/175/192/199,
and the string freeze (D22). Current register evidence:

| Item | Register row | Verdict |
|---|---|---|
| D11 semantic-kind | `DEC-23` | `SIGNED` — routing live in `DailyBriefingReportBuilder`, fallback preserved (`DayEventVocabularyTests` 8/8) |
| D21 quarantine | `DEC-24` | `SIGNED` — quarantine reconciled; ghost `Compile Remove` entries removed; `QuarantineManifestGateTests` requires real files |
| F13 / XP-04 | `DEC-22`, `DEC-26`, `DEC-37`, `DEC-39` | `SIGNED` — multi-currency policy, `FundsLedger`, `RestockAllocationEngine`, `HoldfastTradeSession` funds legs |
| F14 / XP-06 | `DEC-21`, `DEC-35`, `DEC-38`, `DEC-41`, `DEC-42` | `SIGNED` — limb schema, `SurvivorBodyState`, rehabilitation engine + slate, phantom-pain sleep variant |
| EN-01 | `DEC-40` | `SIGNED` — `DifficultyConsequenceWeave` read model |
| EN-02 | `DEC-36` | `SIGNED` — `LivingMapRouteProjection` |
| EN-03 | `DEC-43` | `SIGNED` — `UndergroundEconomyPressure` read model |
| EN-04 | `DEC-42` | `SIGNED` — `RehabilitationSlateProjection` |
| EN-06 | `DEC-44` | `SIGNED` — `BootstrapLifecycleGate` |
| EN-07 | `DEC-31` | `SIGNED` — `CompletionHistorySummary` read contract |
| C3-174 | `DEC-32` | `SIGNED` — `SurvivorOriginModifier` |
| C3-175 | `DEC-33` | `SIGNED` — `CrossRunProfileStore` |
| Plan 192 | `DEC-30` | `SIGNED` — `TradeRouteContract` |
| Plan 199 | `DEC-34` | `SIGNED` — `SeasonalHumanMigrationEngine` |
| D22 string freeze | part of `DEC-23` scope | `SIGNED` for semantic routing; **string freeze itself has no evidenced completion report** — treat as execution-pending, not decision-blocked |
| D19a/b/c, D3, D4, D13, D16 | `DEC-28`, `DEC-09`, `DEC-15`, `DEC-27`, `DEC-29` | `SIGNED` |
| Plan 49 | `DEC-127` (Plan 49 integrated in Wave 31 batch 3) | evidence: `ContentOrphanCertificationEngine` + `AtmosphereTextDeliveredSeam`; **but `ContentOrphanCertificationEngine` is itself host-unreachable** (PLAN-ORPHAN-SEAL-01) — the seal is Core-only |

### 2.2 Signed artifacts that are still host-pending
Host reachability probe (`grep -rl '\b<T>\b' src/`):

| Signed artifact | Core file | Host refs | Tests |
|---|---|---:|---:|
| `FundsLedger` | `Assets/Ashfall.Core/.../FundsLedger.cs` | 0 | 5 |
| `TradeRouteContract` | `Assets/Ashfall.Core/.../TradeRouteContract.cs` | 0 | 3 |
| `CrossRunProfileStore` | `Assets/Ashfall.Core/...` | 0 | 1 |
| `SeasonalHumanMigrationEngine` | `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs` | 0 | 2 |
| `RestockAllocationEngine` | `Assets/Ashfall.Core/Economy/RestockAllocationEngine.cs` | 0 | 1 |
| `SurvivorBodyState` | `Assets/Ashfall.Core/.../SurvivorBodyState.cs` | 0 | 3 |
| `DifficultyConsequenceWeave` | `Assets/Ashfall.Core/.../DifficultyConsequenceWeave.cs` | 0 | 1 |
| `UndergroundEconomyPressure` | `Assets/Ashfall.Core/.../UndergroundEconomyPressure.cs` | 0 | 1 |
| `RehabilitationSlateProjection` | `Assets/Ashfall.Core/.../RehabilitationSlateProjection.cs` | 0 | 1 |
| `BootstrapLifecycleGate` | `Assets/Ashfall.Core/Orchestration/BootstrapLifecycleGate.cs` | 0 | 1 |
| `SurvivorOriginModifier` | `SurvivorEnrichmentService` | 0 | 0 (covered via service tests) |
| `SleepNarrativeProjection` | `Assets/Ashfall.Core/.../SleepNarrativeProjection.cs` | 1 | 2 |

The last row is the proof that this is fixable cheaply: `Main.SleepNarrative.cs`
wired one signed artifact and it left the cohort. Every row above needs the
same bounded host adapter.

### 2.3 Census
`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` measured **111
`AUDIT-PENDING`** corpus rows and 1 `READY-UNCLAIMED` (E1/Plan 53). The
per-clause audits have not run. `WORKTREE_OWNERSHIP.md` holds **148 claims**;
several non-DONE claims (e.g. `claim-c1-plan14-economy-core-2026-09-15`) have
no completion line and no release note — claim hygiene is part of this plan.

---

## 3. Unblock protocol (applies to every item below)

1. **Re-verify the premise in current source** (AGENTS.md Rule 7). A register
   row is not proof the artifact exists; a census row is not proof the gap does.
2. **Classify** as: `EXECUTE` (artifact missing), `INTEGRATE` (artifact exists,
   unreachable), `RETIRE` (superseded), or `STALE_PLAN` (premise false).
3. **Bounded package** with exact paths, authority map, acceptance criteria,
   focused verification — the `AI_AGENT_WORKFLOW.md` format.
4. **Evidence line** recorded in the decision register's evidence column with a
   command and result; never "done" without both.
5. **Gate** — the PLAN-INTEGRATION-KIT-02 reachability + ledger checks prove
   the artifact became reachable.

---

## 4. Phases

### U0 — Ledger and rulebook truth repair (foreman-only)
- Update `AGENTS.md` "ACTIVE QUEUE" and "DECISION-BLOCKED" lists to match the
  register; move executed items to a completed list; keep `INTEGRATION_PLANS.md`
  as queue authority.
- Fix stale rows identified by the 2026-09-19 audit: DEC-01/DEC-16 verdict
  wording, DEC-05 test-count drift (14/14 → 6/6), INTEGRATION_PLANS restock
  paragraph vs DEC-05 `SIGNED`, census anchors C2[9]–C2[13] → `SEALED`.
- Add one line to `KNOWN_DEBT.md` for every claim that is neither DONE nor
  released (claim hygiene).
- Acceptance: `check-register-truth.py` green; no AGENTS.md line names a
  decision-blocked item that has a terminal register verdict.
- Verify: `python3 scripts/ci/check-register-truth.py --check`;
  `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/AgentRuleIntegrityTests.cs`.

### U1 — Execute the host-pending signed dispositions (bounded adapters)
Each row is one small package; the pattern is fixed by `Main.SleepNarrative.cs`:

| Package | Artifact → host path | Acceptance | Verify |
|---|---|---|---|
| U1a | `FundsLedger` → `HoldfastTradeSession` + a funds line in the trade/barter panel | balances persist in the existing economy/holdfast section; zero duplicate ledger | `bash scripts/run_test.sh Ashfall.Core.Tests/...` economy suite |
| U1b | `TradeRouteContract` → `TravelingCaravan`/trade-network day owner | route tier affects arrival/discounts; contract state in the caravan section | caravan + economy focused tests |
| U1c | `CrossRunProfileStore` → endgame/profile surface only | user-level `profile.json` written outside campaign slots; DEC-20/33 boundaries hold | profile + endgame tests |
| U1d | `SeasonalHumanMigrationEngine` → settlement/world day owner | regional population weights tick with hysteresis; day event emitted | world/economy tests |
| U1e | `RestockAllocationEngine` → `ShelterBarterSystem` restock seam (CF-P5) | allocation replaces ad-hoc priority math; DEC-05 still green | `Plan147RestockPriorityTests` + new wiring test |
| U1f | `SurvivorBodyState` + `RehabilitationSlateProjection` + `RehabilitationProgressionEngine` → medical host | clinic read model visible in a medical panel row; phase from canonical state | medical focused suite |
| U1g | `DifficultyConsequenceWeave` → briefing/difficulty panel | projected war/crisis/shock multipliers render from live scalars | difficulty tests |
| U1h | `UndergroundEconomyPressure` → black-market panel | temperature band + price/attention multipliers read live | black-market tests |
| U1i | `BootstrapLifecycleGate` → CI/selftest verb | fresh/restore/reset lifecycle sets compared in one headless verb | `--bootstrap-lifecycle-selftest` |
| U1j | `SurvivorOriginModifier` → character sheet/creation surface | origin modifier applied once at creation, bounded | survivor creation tests |

- Acceptance for U1: zero host-pending rows remain in §2.2; every artifact is
  in the reachability closure; no new save section (all ride existing owners).
- Verify: reachability gate + the focused suite named per row.

### U2 — D21 quarantine drain (protocol, not a bulk re-enable)
- Premise: `DEBT-TEST-QUARANTINE-2026-09-12` is `RECONCILED`; ghost entries
  were removed; `QuarantineManifestGateTests` now requires real files. There is
  no active quarantine left to drain in the compiled test set.
- Define the **restoration protocol** for historical drafts: rematch to current
  APIs/content → focused suite passes → drop any explicit exclusion → register
  evidence. Record it in `docs/ci/TEST_QUARANTINE_PROTOCOL.md`.
- Acceptance: protocol documented; zero unexplained `Compile Remove` entries
  (`QuarantineManifestGateTests` green).
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/QuarantineManifestGateTests.cs`.

### U3 — String freeze and the two open decisions (DEC-11, DEC-13)
- Run `scripts/ci/extract_l10n_inventory.py`; produce the inventory baseline.
- Freeze user-facing strings: data JSON prose, panel labels, journal/radio
  text; new content may be added only through the catalog pipeline.
- After the freeze + audio bus calibration (`docs/audio/AUDIO_POLICY.md`),
  return `DEC-11` and `DEC-13` to the foreman with evidence for a terminal
  verdict.
- Acceptance: inventory baseline committed under `artifacts/`; a `--check`
  gate detects new hardcoded strings; DEC-11/13 rows cite the baseline.
- Verify: `python3 scripts/ci/extract_l10n_inventory.py --check`.

### U4 — Census AUDIT-PENDING tranche machine
- Define a per-row audit template: premise, current evidence, verdict
  (`READY-UNCLAIMED | EXECUTE | INTEGRATE | RETIRE | STALE_PLAN`), owner,
  promotion condition.
- Run tranche T1 over the rows whose systems are already in the 99-orphan
  cohort (they are ready by definition — the Core exists and only the host
  path is missing). Expected: a large fraction flip to `INTEGRATE` and join
  PLAN-ORPHAN-SEAL-01 waves.
- Acceptance: T1 report under `docs/plans/`; every row has a verdict; queue
  counts updated in `INTEGRATION_PLANS.md` by the foreman.
- Verify: static audit (no tests) + reachability gate.

### U5 — Plan 49 and prerequisite re-audits
- Plan 49 (`ContentOrphanCertificationEngine`) is Core-only per §2.1. Re-scope:
  wire the engine to the content-utilization pipeline as a CLI-only authority,
  or retire it if `--content-utilization-selftest` already covers the function.
- Re-run the C2[18]/Plan 42 and C2[20]/Plan 46 premise audits only if a new
  consumer is proposed; otherwise record "sealed, no re-open" per debt rule.
- Verify: `godot --headless --path . -- --content-utilization-selftest`.

### U6 — Head plans 37 / 48 / 53
- **C2[15]/Plan 37** — premise audit confirmed live evidence in
  `Next-steps-plans/shipped_to_chat/Plan_37_…md` (21 actions, 3 uncalled
  predicates, 0 focus usage, 0 joypad bindings). Start after the audit confirms
  the counts still hold at HEAD.
- **C2[21]/Plan 48** — premise audit confirmed no tags, no changelog, version
  string fallback, `release-gate.sh` absent. Start with 48A (versioning
  scheme).
- **E1/Plan 53** — `claim-e1-plan53-2026-09-19` is already filed; complete the
  remaining E1 packages (E1A–E1P) or return the unconsumed ones as
  `STALE_PLAN` with evidence.
- Acceptance: each head either produces its first bounded package or a
  documented `STALE_PLAN` with current-evidence citations.
- Verify: as defined in PLAN-LAUNCH-FACE-06 §4.

### U7 — XP pillar completion (active W1 and remainder)
- `XP-01` difficulty full binding: complete preset selection, persistence,
  remaining scalar consumers, panel (evidence in
  `docs/plans/CF_XP01_DIFFICULTY_FULL_BINDING_INTEGRATION_PLAN.md`).
- `XP-04/06/07/08`: the signed dispositions are U1a–U1f; the remaining
  gameplay legs (trade routes, seasonal migration, body integrity player loop)
  are scoped in PLAN-VERTICAL-BODY-INDUSTRY-05.
- Acceptance: no XP pillar remains "partially bound" in the ledger.
- Verify: `godot --headless --path . -- --difficulty-selftest`; focused
  economy/medical suites.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Rulebook edits conflict with generated client rulebooks | run `scripts/ci/sync-agent-rulebooks.py` after `AGENTS.md`; `AgentRuleIntegrityTests` |
| U1 adapters create save-section sprawl | reuse existing owners; matrix regenerated; integrator sign-off for any new section |
| Census audit becomes a rewrite of history | template verdicts only; no code edits from the audit |
| String freeze blocks content work | freeze applies to shipping strings; dated freeze report, new content goes through catalogs |
| Plan-number collisions mislead head plans | every package cites subsystem names, not only plan numbers (kit ledger-truth) |

## 6. Rollback

Ledger edits are markdown; revert is a commit. U1 adapters are additive and
each is independently revertible. No decision is reversed; open rows stay open
with better evidence.

## 7. Verification summary

```bash
python3 scripts/ci/check-register-truth.py --check
python3 scripts/ci/check-ledger-truth.py --check
python3 scripts/ci/generate-authority-reachability.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/QuarantineManifestGateTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/AgentRuleIntegrityTests.cs
python3 scripts/ci/extract_l10n_inventory.py --check
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --difficulty-selftest
```

---

## 6. Expanded census (decision surface)

| Metric | Value |
|---|---:|
| DEC rows | 301 |
| Non-terminal rows | 2 |
| Known non-terminal ids | DEC-11, DEC-13 |

## 7. Expanded surface: unblock contract

| Rule | Detail |
|---|---|
| Evidence | each disposition carries a current-source artifact path |
| Host path | a signed decision without a host path stays U1 |
| Register | the foreman owns register edits; this plan works the rows |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Row disposition | every U-row has a state |
| Artifact check | cited paths exist at claim time |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Freeze the register counts.
2. Work U1 rows with artifact verification.
3. Record dispositions; no register edits here.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| U-row | disposition + artifact path |
| Host path | proven or the row stays U1 |
| Register | untouched by this plan |

**Non-goals unchanged:** the plan supplies worklists; the foreman records decisions.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 25. Other plans referencing them: **36**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 6 |
| `PLAN-INTEGRATION-KIT-02` | 5 |
| `PLAN-LAUNCH-FACE-06` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `EVIDENCE` | 4 |
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 4 |
| `PLAN-RELEASE-OPS-20` | 3 |
| `PLAN-DEBT-DRAIN-24` | 3 |

**Governed artifacts (first 12):**

| Path |
|---|
| `AGENTS.md` |
| `AI_AGENT_WORKFLOW.md` |
| `Assets/Ashfall.Core/.../DifficultyConsequenceWeave.cs` |
| `Assets/Ashfall.Core/.../FundsLedger.cs` |
| `Assets/Ashfall.Core/.../RehabilitationSlateProjection.cs` |
| `Assets/Ashfall.Core/.../SleepNarrativeProjection.cs` |
| `Assets/Ashfall.Core/.../SurvivorBodyState.cs` |
| `Assets/Ashfall.Core/.../TradeRouteContract.cs` |
| `Assets/Ashfall.Core/.../UndergroundEconomyPressure.cs` |
| `Assets/Ashfall.Core/Economy/RestockAllocationEngine.cs` |
| `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs` |
| `Assets/Ashfall.Core/Orchestration/BootstrapLifecycleGate.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 25. Host files: **70** · Test files: **106** · Data files: **15**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 70 | `src/Host/ChemicalReconHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/EconomyHostSession.cs`, `src/Host/EquipmentConditionHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 106 | `Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/AgricultureSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/Audio/Plan52SoundOfScarcityIntegrationTests.cs`, `Ashfall.Core.Tests/AudioEventIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 15 | `Assets/StreamingAssets/Data/codex_entries.json`, `Assets/StreamingAssets/Data/field_guide.json`, `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json`, `Assets/StreamingAssets/Data/narrative/carbide_tool_wear_audits.json`, `Assets/StreamingAssets/Data/narrative/gear_quenching_fault_logs.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **31** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `archive_desk` |
| `black_projects_archive` |
| `caravan_trade_network` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `dose_ledger` |
| `economy` |
| `equipment` |
| `equipment_condition` |
| `field_guide` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **29** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--advanced-industrial-recon-selftest` |
| `--agriculture-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--chemical-dependency-save-selftest` |
| `--deep-coast-route-selftest` |
| `--difficulty-selftest` |
| `--dose-ledger-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--expedition-panel-lifecycle` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **37**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/camouflage_gear.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **17** (223 files, 1865 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `BodyMind` | 1 | 6 |
| `Codex` | 2 | 29 |
| `Combat` | 10 | 84 |
| `Difficulty` | 5 | 24 |
| `Economy` | 41 | 329 |
| `Equipment` | 1 | 4 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |

**Verdict:** 1865 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `BodyMind`, `Codex`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **394**
(45 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Dose/DoseRegisterSurface.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **52**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `archive_desk` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `combat` | no |
| `disease` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **19**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **345**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 41, OPTIONAL 8, UNRESOLVED 17).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `camouflage_gear.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |

**Verdict:** 17 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 36
**Surface:** save sections 52 (laddered 1) · RNG streams 19 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-UNBLOCK-03
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 36 other plan(s) name these artifacts (§12)
  - wave seed — claim before dependent plans
  - touches 1 versioned save ladder(s) — extend, never fork
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
