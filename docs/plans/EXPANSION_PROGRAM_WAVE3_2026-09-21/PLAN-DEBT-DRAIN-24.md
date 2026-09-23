# PLAN-DEBT-DRAIN-24 — Accepted Debt, Claim Hygiene & Documentation Archive

**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim. Foreman claim required; ledger edits are
foreman-only.
**Depends on:** PLAN-UNBLOCK-03 U0 (ledger truth), PLAN-RELEASE-OPS-20 (repo
health).
**Expanded appendix:** [`PLAN-DEBT-DRAIN-24_APPENDIX-A_LEDGER_INVENTORY.md`](PLAN-DEBT-DRAIN-24_APPENDIX-A_LEDGER_INVENTORY.md)
— the parsed ledger inventory: **6 non-terminal debt rows** and all **46 open
ownership claims** (of 148 parsed) with their package and owner role.
**Non-goals:** no deletion of historical evidence, no rewriting of the
decision register, no re-opening RETIRED rows.

---

## 1. Outcome

`KNOWN_DEBT.md` is in good shape — 48 RETIRED rows — but four rows remain
`ACCEPTED`, one `QUARANTINED`, and **46 ownership claims have no completion
line**. Meanwhile the repo carries multiple uncertified plan trees
(`C-integration-plans/`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`,
`POTENTIALCLUTTER.md`, `sources.md`) whose authority status is ambiguous.
Agents keep rediscovering this surface, which is the definition of accepted
debt that has outlived its reason.

Deliverables:

1. every `ACCEPTED`/`QUARANTINED` row driven to a **terminal disposition or a
   dated review** with a named trigger;
2. the **claim ledger** made truthful: 46 open claims classified
   `DONE`/`RELEASED`/`ABANDONED` with evidence;
3. **documentation archive**: uncertified trees indexed, stamped, and moved to
   `docs/archive/` (or kept with a banner) so `docs/CURRENT_AUTHORITY.md`
   remains the map;
4. a **recurring governance sweep** so this cannot accumulate again.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| `KNOWN_DEBT` ACCEPTED rows | 4 | `DEBT-PLAN-SPRAWL`, `DEBT-RULEBOOK-SNAPSHOT`, `DEBT-PLANS170-199-PORTFOLIO`, `DEBT-GODOT-PARTIAL-REQUIRED` |
| QUARANTINED | 1 | `DEBT-WORKTREE-DECLUTTER-2026-09-12` (2,852 deletions; 2,506 archived) |
| RECONCILED | 1 | `DEBT-TEST-QUARANTINE-2026-09-12` |
| Ownership claims without DONE/COMPLETE | 46 | `grep '^| claim-' WORKTREE_OWNERSHIP.md` |
| Historical plan trees | `C-integration-plans/`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `C1_*`, `WAVE9_*` | repo root |
| Docs indexed | 2,872 | `generate-docs-index.py` |
| Register deferred rows | DEC-11 (VO), DEC-13 (localization) | register |
| Authority map | `docs/CURRENT_AUTHORITY.md` | source-of-truth table |

---

## 3. Packages

### DD-24A — Accepted debt terminal dispositions
For each ACCEPTED/QUARANTINED row, force one of:
- **EXECUTE** (small package, e.g. the rulebook-snapshot debt becomes a
  hash-verified archive check + index);
- **RETIRE** (the reason no longer applies, e.g. plan-sprawl once the archive
  lands);
- **KEEP with review date** (only if it has a named trigger and a next review
  date; e.g. `DEBT-GODOT-PARTIAL-REQUIRED` is a permanent engineering rule and
  should be `RETIRED`-style permanent, not open debt).
- **Acceptance:** zero ACCEPTED rows without a dated review trigger; the
  ledger's own status meanings are respected.
- **Verify:** `python3 scripts/ci/check-register-truth.py --check` (extended to
  debt) + manual foreman sign-off.

### DD-24B — Claim ledger hygiene
- Classify the 46 open claims: `DONE` (artifacts exist and are reachable — cite
  a gate), `RELEASED` (no longer needed; superseded), `ABANDONED` (owner gone,
  no artifacts), or `ACTIVE` (a live builder truly owns it).
- Add a completion column with the evidence pointer; a claim without completion
  and without `ACTIVE` status cannot stay.
- **Acceptance:** ≤ 5 genuinely `ACTIVE` claims; no claim older than 14 days
  without a completion or a written continuation note.
- **Verify:** the claim checker script (new) + foreman review.

### DD-24C — Documentation archive
- For each uncertified tree: add a banner (`STATUS: HISTORICAL — not live
  authority`, date, superseding ledger), move to `docs/archive/<date>/`, and
  add an `ARCHIVE_INDEX.md` with per-file one-liners. Keep `docs/CURRENT_AUTHORITY.md`
  as the only map.
- Preserve bytes; never rewrite history; keep cross-links working (docs link
  gate).
- **Acceptance:** no uncertified tree remains at the root; archive index
  complete; docs link gate green.
- **Verify:** `bash scripts/ci/doc-link-gate.sh`;
  `python3 scripts/ci/generate-docs-index.py --check`.

### DD-24D — Seal-steps classification
- The `Seal-steps/` documents are uncertified plans with predecessor chains.
  Classify each: consumed, superseded, or reference. Register consumed ones in
  the archive index with the plan row that consumed them.
- **Acceptance:** no uncertified plan document can be mistaken for a live plan;
  the census/queue authorities remain the only source.

### DD-24E — Recurring governance sweep
- Add a release-cadence sweep (script + checklist): debt rows, open claims,
  archived trees, register terminality, ledger truth. Output a one-page report.
- **Acceptance:** first report committed; linked from the foreman workflow.
- **Verify:** the sweep script in report mode.

---

## 4. Risk register

| Risk | Mitigation |
|---|---|
| Archive moves break links | doc link gate + archived path redirects in the index |
| Marking claims ABANDONED hides live work | only the foreman may set ABANDONED; builders may set DONE/RELEASED |
| A debt "review date" becomes a snooze button | review must name a trigger (event/version), not just a date |
| Moving trees loses context | byte-preserving moves + per-file index |

## 5. Verification

```bash
python3 scripts/ci/generate-docs-index.py --check
bash scripts/ci/doc-link-gate.sh
python3 scripts/ci/check-register-truth.py --check
python3 scripts/ci/check-ledger-truth.py --check
```

---

## 6. Expanded census (bespoke: debt & claims ledgers)

This plan drains accepted debt and stale claims, so the census reads the live
ledgers (read-only).

| Ledger | Rows (non-header) |
|---|---:|
| `KNOWN_DEBT.md` table rows | 55 |
| `WORKTREE_OWNERSHIP.md` table rows | 154 |

## 7. Expanded surface: drain contract

| Rule | Detail |
|---|---|
| Disposition | each debt row is sealed, re-accepted with an owner, or retired |
| Claims | a claim without completion is either completed or released |
| Evidence | sealing cites a focused test; retirement cites a successor |
| No silent edit | the plan reads the ledgers; the foreman writes them |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Row disposition | every row has a state after a drain pass |
| Focused proof | each sealed row cites a passing focused target |
| Stale claims | released claims listed with dates |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) with row-level dispositions.
2. Seal or retire the top rows with focused proof.
3. Release stale claims.
4. Regression: ledger snapshot diff.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Debt row | sealed / re-accepted / retired with evidence |
| Claim | completed or released |
| Evidence | focused test or successor named |
| Ledger | foreman-owned; this plan supplies the worklist |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not edit the live ledgers.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 8. Other plans referencing them: **53**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-UNBLOCK-03` | 3 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `PLAN-LAUNCH-FACE-06` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 2 |
| `PLAN-SAVE-GOVERNANCE-12` | 2 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `ARCHIVE_INDEX.md` |
| `KNOWN_DEBT.md` |
| `PLAN-DEBT-DRAIN-24_APPENDIX-A_LEDGER_INVENTORY.md` |
| `POTENTIALCLUTTER.md` |
| `WORKTREE_OWNERSHIP.md` |
| `docs/CURRENT_AUTHORITY.md` |
| `generate-docs-index.py` |
| `sources.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `EXECUTE` | no name match — resolve at claim time |
| `RETIRE` | `ARCHIVE_INDEX.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 8. Host files: **11** · Test files: **17** · Data files: **20**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Host/WildlifeTrappingHostSession.cs`, `src/Journal/JournalCodex.cs`, `src/Main.Audio.cs` |
| Tests (`Ashfall.Core.Tests/`) | 17 | `Ashfall.Core.Tests/Collectibles/CollectibleContentUtilizationTests.cs`, `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs`, `Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs`, `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`, `Ashfall.Core.Tests/Medical/ChemicalDependencyStressRelapseTests.cs` |
| Data (`StreamingAssets/Data/`) | 20 | `Assets/StreamingAssets/Data/faction_war_communiques.json`, `Assets/StreamingAssets/Data/field_guide.json`, `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`, `Assets/StreamingAssets/Data/items.json`, `Assets/StreamingAssets/Data/medical_texts.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **27** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `dose_ledger` |
| `faction_espionage` |
| `field_guide` |
| `foundry` |
| `grain_milling_archive` |
| `host_event` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **20** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--chemical-dependency-save-selftest` |
| `--dose-ledger-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **31**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnDependencyFormed` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyReFormedByStress` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyRisk` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnGuiltInsomniaCritical` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |
| `OnGuiltRecorded` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/codex_entries.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **13** (241 files, 2002 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Codex` | 2 | 29 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Medical` | 49 | 435 |

**Verdict:** 2002 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Codex`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **395**
(42 of them panels/HUD).

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
| `src/Economy/EconomyMarketPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **48**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `archive_desk` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `combat` | no |
| `disease` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **20**.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **148**
(CODEX_ONLY 88, GAMEPLAY_CONSUMED 39, OPTIONAL 6, UNRESOLVED 15).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `codex_entries.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |

**Verdict:** 15 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **5**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (10/12) · **Class:** governance · **Coupling (incoming plans):** 53
**Surface:** save sections 48 (laddered 1) · RNG streams 20 · host files 27 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DEBT-DRAIN-24
wave: —
status: PROPOSED — foreman claim required
packages: DD-24A, DD-24B, DD-24C, DD-24D, DD-24E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --audio-selftest
dependencies:
  - coordinate: 53 other plan(s) name these artifacts (§12)
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: wave, verification.
