# PLAN-THIRDONARY-COVENANT-TRUTH-134 — Covenant & Dispute Lifecycle with Save Truth

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-JUSTICE-LAW-37, PLAN-SHELTER-POLITICS-69, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-THIRDONARY-COVENANT-TRUTH-134_APPENDIX-A_SCAFFOLD.md`](PLAN-THIRDONARY-COVENANT-TRUTH-134_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-JUSTICE-LAW-37` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no second law system (Plan 37), no faction diplomacy (Plan 29),
no new lore beyond the existing catalog.

## 1. Outcome
`Thirdonary/` owns a distinct institution: `ThirdonaryCatalogLoader.cs`,
`ThirdonaryQuestSystem.cs`, `ThirdonarySave.cs`, `ThirdonaryTypes.cs`, with the
save registry already declaring `("thirdonary", "SaveThirdonary",
"SetupThirdonary", "thirdonary", "Thirdonary covenant & dispute states")`. No
plan states the lifecycle: how a covenant is formed, what a dispute is, how it
resolves, and what persists.

| Deliverable | Detail |
|---|---|
| Covenant lifecycle | offered → sworn → active → fulfilled/renounced, with the owner of each transition |
| Dispute model | a dispute names the parties, the claim, and the resolution path; unresolved disputes have visible standing consequences |
| Arbitration | resolutions run through Plan 37's justice seam where the claim is legal, and the covenant's own terms otherwise |
| Save truth | `ThirdonarySave` round-trips exactly; a load mid-dispute preserves the claim state |
| Catalog binding | every covenant/term id resolves through the loader; unknown ids fail loud |

## 2. Evidence
- `Assets/Ashfall.Core/Thirdonary/`: the four files above (verified).
- `SaveSectionRegistry.All`: `("thirdonary", "SaveThirdonary", "SetupThirdonary", "thirdonary", "Thirdonary covenant & dispute states")`.
- Plan 37 owns verdicts/law; covenants route legal claims there rather than duplicating adjudication.
- Plan 1 Appendix M: catalog binding check for Thirdonary-domain catalogs.

## 3. Packages
- **TCT-134A** lifecycle document + transition table.
- **TCT-134B** dispute model + resolution path tests (legal vs covenant terms).
- **TCT-134C** catalog binding tests (unknown id fails; all terms resolve).
- **TCT-134D** save round-trip incl. mid-dispute state.
- **TCT-134E** standing-consequence wiring to its existing owner.

## 4. Acceptance & verification
- Every covenant reaches exactly one terminal state; renouncement is never silent.
- Mid-dispute save/load preserves parties and claim.
- Unknown catalog id fails at load with the id named.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Thirdonary/` (create if absent).

## 5. Risks
Legal duplication → arbitration delegates to Plan 37 where the claim is legal; the boundary table states which claims go where.

---

## 6. Expanded census (4 files · 506 lines)

Scope: `Assets/Ashfall.Core/Thirdonary/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Loader 1 · Save 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ThirdonaryCatalogLoader.cs` | 48 | Loader | — | 0 | 0 | 0 |
| `ThirdonaryQuestSystem.cs` | 296 | System | **yes** | 0 | 0 | 2 |
| `ThirdonarySave.cs` | 57 | Save | **yes** | 0 | 0 | 0 |
| `ThirdonaryTypes.cs` | 105 | DTO/Type | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `thirdonary_quests.json` | object[2 keys] |

**State surfaces:** `ThirdonaryQuestSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Thirdonary/` (create if absent) |
| Test references | 3 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TCT-134A` | no name match — resolve at claim time |
| `TCT-134B` | no name match — resolve at claim time |
| `TCT-134C` | `ThirdonaryCatalogLoader.cs` |
| `TCT-134D` | no name match — resolve at claim time |
| `TCT-134E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **1** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/ThirdonaryHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `ThirdonaryTypes` | `ThirdonaryQuestSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expansion_quest` |
| `thirdonary` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/quest_templates.json` |
| `Assets/StreamingAssets/Data/thirdonary_quests.json` |

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
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |
| `src/Host/PersonalQuestSelfTest.cs` |
| `src/Host/ThirdonaryHostSession.cs` |
| `src/Host/ThirdonarySaveStore.cs` |
| `src/UI/CrossingQuestPanel.cs` |
| `src/UI/PanelSceneLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expansion_quest` | no |
| `thirdonary` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 1, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/quest_narrative_documents.json` | CODEX_ONLY |
| `thirdonary_quests.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 8 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-THIRDONARY-COVENANT-TRUTH-134
wave: 11
status: PROPOSED — foreman claim required
packages: TCT-134A, TCT-134B, TCT-134C, TCT-134D, TCT-134E
claim paths:
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --personal-quest-selftest
dependencies:
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
