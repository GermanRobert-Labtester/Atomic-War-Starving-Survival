# PLAN-CULTURAL-ARCHIVE-TRUTH-169 — Preservation Vaults, Access Rules & Loss Events

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-CODEX-SURFACE-TRUTH-110, PLAN-MORTUARY-MEMORIAL-TRUTH-123.
**Implementation scaffold:** [`PLAN-CULTURAL-ARCHIVE-TRUTH-169_APPENDIX-A_SCAFFOLD.md`](PLAN-CULTURAL-ARCHIVE-TRUTH-169_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-LORE-ARCHIVE-TRUTH-238` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no museum/festival content (Plan 4), no codex surface work
(Plan 110), no memorial pipeline (Plan 123).

## 1. Outcome
`Culture/CulturalArchiveVaultSystem.cs` (675 lines) is reachable and
unaddressed: a vault that preserves cultural records through the campaign.
Culture content and codex display are owned (Plans 4, 110); what is missing is
the **vault contract** — what is deposited, who may access it, what threatens
it, and what loss means. Without that, preservation is cosmetic.

| Deliverable | Detail |
|---|---|
| Deposit model | items/records that can be archived with their source record id; deposit removes from active use or copies per documented rule |
| Access rules | who may read/deposit from role assignments (Plan 141) and public/restricted tiers |
| Threat model | fire, flood, theft, neglect each with a documented loss path and mitigation that consumes existing supplies |
| Loss truth | a lost record is marked lost (not silently deleted from catalogs); restorations are earned, never automatic |
| Save truth | vault contents and threat state restore; a load never re-rolls a loss event |

## 2. Evidence
- `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` (675 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 4 owns culture surfaces; Plan 110 owns codex rendering of records.
- Plan 141 owns roles that access rules read.
- Plan 123 supplies the memorial pipeline where a lost record's memory lives.

## 3. Packages
- **CAT-169A** deposit model + copy/remove rule.
- **CAT-169B** access tiers from Plan 141 + test per tier.
- **CAT-169C** threat table + one loss fixture per threat (mitigation consumable).
- **CAT-169D** loss marking + restoration path test.
- **CAT-169E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Access decisions match role assignment; a restricted record is unreadable to an unauthorized role.
- A threat fixture marks the record lost and consumes its mitigation; none is silently undeleted.
- Save/load preserves vault state exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/`.

## 5. Risks
Vault as a second catalog → it references record ids; content stays in data.
Silent loss → loss is a marked state with a visible trace, never a deletion.

---

## 6. Expanded census (3 files · 801 lines)

Scope: `Assets/Ashfall.Core/Culture/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ArchiveChronicleMilestones.cs` | 24 | Support | — | 0 | 0 | 0 |
| `CulturalArchiveTomeCatalog.cs` | 102 | Catalog | — | 0 | 0 | 0 |
| `CulturalArchiveVaultSystem.cs` | 675 | System | **yes** | 0 | 0 | 5 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `lore_archives.json` | object[2 keys] |
| `cultural_archive_tomes.json` | object[2 keys] |
| `prewar_archives.json` | object[2 keys] |
| `archive_inks.json` | object[3 keys] |
| `archive_categories.json` | object[2 keys] |
| `architect_vault_audits.json` | array[7] |

**State surfaces:** `CulturalArchiveVaultSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Culture/` |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VERTICAL-CULTURE-04` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CAT-169A` | no name match — resolve at claim time |
| `CAT-169B` | no name match — resolve at claim time |
| `CAT-169C` | no name match — resolve at claim time |
| `CAT-169D` | no name match — resolve at claim time |
| `CAT-169E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.FlagshipInstitutions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`, `Ashfall.Core.Tests/Culture/Plan178CreationToVaultTests.cs`, `Ashfall.Core.Tests/Culture/Plan219DocumentationIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `CulturalArchiveTomeCatalog` | `CulturalArchiveVaultSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `cryo_vault` |
| `cultural_archives` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/narrative/architect_vault_audits.json` |
| `Assets/StreamingAssets/Data/narrative/vault_seal_breach_logs.json` |
| `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **14**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CryoVaultSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |
| `src/Host/PrewarArchiveSaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |
| `src/UI/ArchiveDeskPanel.cs` |
| `src/UI/BlackProjectsArchivePanel.cs` |
| `src/UI/ChroniclePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **8**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `cryo_vault` | no |
| `cultural_archives` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `technical_material_archive` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/vault_seal_breach_logs.json` | CODEX_ONLY |
| `narrative/vinyl_record_archive.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 8 (laddered 0) · RNG streams 0 · host files 13 · catalogs 13 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CULTURAL-ARCHIVE-TRUTH-169
wave: 13
status: PROPOSED — foreman claim required
packages: CAT-169A, CAT-169B, CAT-169C, CAT-169D, CAT-169E
claim paths:
  - src/Host/ArchiveDeskHostSession.cs  # §19 candidate host surface
  - src/Host/BlackProjectsArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/CryoVaultSaveStore.cs  # §19 candidate host surface
  - src/Host/CulturalArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
