# PLAN-SKILL-PROGRESSION-TRUTH-113 — Skill Growth, Atrophy & Display Consistency

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BALANCE-DIFFICULTY-INTEGRATION-73, PLAN-LABOUR-PROFESSIONS-68, PLAN-DETERMINISM-CROSS-HOST-89.
**Non-goals:** no new skill catalog, no XP rebalance owned by the active
XP-WAVE1 package (INTEGRATION_PLANS.md queue authority), no parallel
progression state.

## 1. Outcome
`Survivors/` already contains the progression stack: `SkillProgressionSystem.cs`,
`SkillProgressionState.cs`, `SkillAtrophySystem.cs`, `SkillDef.cs`, and
`SkillCatalogLoader.cs`. The system documents "NO `UnityEngine.Random`" in its
own contract comment — good — but nothing proves the **display and the state
agree**: that a survivor's shown skill level is the stored one, that atrophy
accrues only from documented inactivity, and that growth is deterministic per
seed. XP waves own balance; this plan owns truth.

| Deliverable | Detail |
|---|---|
| State↔display check | panel value equals stored value after any scripted sequence (no derived double-rounding) |
| Atrophy rules | inactivity window and floor documented; atrophy never removes a milestone flag |
| Deterministic growth | growth rolls use the registered stream; two same-seed runs match |
| Catalog binding | every `SkillDef` id resolves through the loader; unknown ids fail loud, not silently ignored |
| Save round-trip | progression state survives save/load mid-growth; no reroll on load |

## 2. Evidence
- `Survivors/SkillProgressionSystem.cs`, `SkillProgressionState.cs`, `SkillAtrophySystem.cs`, `SkillDef.cs`, `SkillCatalogLoader.cs` (file list re-verified per package).
- `SkillAtrophySystem` doc comment: engine-free, no `UnityEngine.Random`; `CampaignRngStream` (63 streams) is the seeded contract.
- `Ashfall.Core.Tests/` already has determinism precedent (paired-run style from Plan 89).
- Active XP queue: this plan must **not** touch XP balance paths; premise check confirms no overlapping claim before any edit.

## 3. Packages
- **SKP-113A** state↔display test matrix (grow, atrophy, cap, milestone).
- **SKP-113B** atrophy rules doc + boundary tests (inactivity window edges).
- **SKP-113C** same-seed growth equality test using the registered stream.
- **SKP-113D** catalog binding: unknown id failure test + loader coverage report.
- **SKP-113E** mid-growth save round-trip test.

## 4. Acceptance & verification
- Display equals stored at every checkpoint; no rounding drift.
- Two same-seed runs: identical progression vectors.
- Unknown skill id fails typed at load; no silent default.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Colliding with the active XP package → the claim must exclude XP balance files; truth checks may land in tests only if the overlap persists.
Atrophy as hidden punisher → rules and floors are explicit and tested.

---

## 6. Expanded census (5 files · 1,083 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Loader 1 · Support 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SkillAtrophySystem.cs` | 166 | System | **yes** | 0 | 0 | 2 |
| `SkillCatalogLoader.cs` | 132 | Loader | **yes** | 0 | 0 | 0 |
| `SkillDef.cs` | 46 | Support | — | 0 | 0 | 0 |
| `SkillProgressionState.cs` | 116 | DTO/Type | — | 0 | 0 | 0 |
| `SkillProgressionSystem.cs` | 623 | System | **yes** | 1 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `skills.json` | object[3 keys] |

**State surfaces:** `SkillAtrophySystem.cs`, `SkillProgressionSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 37 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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
Domain files: 5. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 5 |
| `PLAN-LATENT-EXPERT-TRUTH-239` | 5 |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SKP-113A` | `SkillAtrophySystem.cs`, `SkillProgressionState.cs` |
| `SKP-113B` | `SkillAtrophySystem.cs` |
| `SKP-113C` | no name match — resolve at claim time |
| `SKP-113D` | `SkillCatalogLoader.cs` |
| `SKP-113E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **28** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/LibraryStudyHostSession.cs`, `src/Main.CampaignServices.cs`, `src/Main.Plans162_185.cs`, `src/UI/SkillMatrixPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 28 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs`, `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **8**; isolated: **0**.

| From | → To |
|---|---|
| `SkillCatalogLoader` | `SkillDef` |
| `SkillCatalogLoader` | `SkillProgressionSystem` |
| `SkillDef` | `SkillProgressionSystem` |
| `SkillProgressionState` | `SkillAtrophySystem` |
| `SkillProgressionState` | `SkillProgressionSystem` |
| `SkillProgressionSystem` | `SkillCatalogLoader` |
| `SkillProgressionSystem` | `SkillDef` |
| `SkillProgressionSystem` | `SkillProgressionState` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

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

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnAtrophyDangerPassed` | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` |
| `OnSkillAtrophied` | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative_progression.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (11 files, 83 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Progression` | 11 | 83 |

**Verdict:** 83 cases sit under matching regions — run those first (`Progression`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **4**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/SkillMatrixPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `narrative_progression.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 4 · catalogs 2 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SKILL-PROGRESSION-TRUTH-113
wave: 9
status: PROPOSED — foreman claim required
packages: SKP-113A, SKP-113B, SKP-113C, SKP-113D, SKP-113E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/UI/PanelSceneLoader.cs  # §19 candidate host surface
  - src/UI/SkillMatrixPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative_progression.json  # §17 catalog (verify schema + consumer)
  - narrative_progression.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Progression/
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
