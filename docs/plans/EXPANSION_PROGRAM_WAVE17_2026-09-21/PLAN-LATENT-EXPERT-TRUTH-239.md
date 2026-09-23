# PLAN-LATENT-EXPERT-TRUTH-239 — Unrecognized Skill: Awakening & Recognition

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SKILL-PROGRESSION-TRUTH-113, PLAN-LABOUR-PROFESSIONS-68, PLAN-BACKSTORY-REVEAL-TRUTH-126.
**Non-goals:** no progression model (Plan 113), no profession slots (Plan 68), no
backstory fact model (Plan 126).

## 1. Outcome
`Survivors/LatentExpertAwakeningSystem.cs` (**241 lines**) is reachable and
unaddressed: a survivor has an unrecorded skill that surfaces under conditions
(an emergency, a task assignment, a revealed fact). It is a small system with a
large failure mode — a hidden bonus that appears arbitrarily.

| Deliverable | Detail |
|---|---|
| Latent model | latent skill keys tied to survivor records; a trigger condition per key (task context, crisis) |
| Awakening | awakening grants the skill through Plan 113's state; no private skill value |
| Recognition | the event is visible (a notice and a backstory fact via Plan 126); no silent stat jump |
| Bounds | at most the declared latent keys per survivor; awakening is once per key |
| Save truth | latent and awakened sets restore; no re-awakening or loss on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs` (241 lines; unaddressed — Wave 17 audit).
- Plan 113 owns skill state; Plan 126 reveals the associated fact.
- Plan 68's assignments are a trigger context.
- Plan 138 can carry the recognition notice.

## 3. Packages
- **LET-239A** latent-key model + trigger table.
- **LET-239B** awakening hand-off to Plan 113 (no private value).
- **LET-239C** recognition notice + fact link (Plan 126).
- **LET-239D** bounds/once-per-key fixtures.
- **LET-239E** save round-trip; no re-awaken on load.

## 4. Acceptance & verification
- Awakenings appear in Plan 113's state and produce a notice/fact.
- Bounds hold; save/load preserves latent/awakened sets.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Arbitrary bonus → trigger table and visible recognition.
Duplicate skill state → hand-off only; the boundary test asserts it.

---

## 6. Expanded census (6 files · 1,324 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Loader 1 · Support 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LatentExpertAwakeningSystem.cs` | 241 | System | **yes** | 0 | 0 | 2 |
| `SkillAtrophySystem.cs` | 166 | System | — | 0 | 0 | 2 |
| `SkillCatalogLoader.cs` | 132 | Loader | — | 0 | 0 | 0 |
| `SkillDef.cs` | 46 | Support | — | 0 | 0 | 0 |
| `SkillProgressionState.cs` | 116 | DTO/Type | — | 0 | 0 | 0 |
| `SkillProgressionSystem.cs` | 623 | System | — | 1 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `skills.json` | object[3 keys] |

**State surfaces:** `LatentExpertAwakeningSystem.cs`, `SkillAtrophySystem.cs`, `SkillProgressionSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 38 name references across the test tree |
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
Domain files: 6. Other plans referencing them: **4**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 5 |
| `PLAN-SKILL-PROGRESSION-TRUTH-113` | 5 |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 2 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Reading:** incoming edges are coordination risk.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **9**; isolated files:
**0**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `LatentExpertAwakeningSystem` | `SkillProgressionSystem` |
| `SkillCatalogLoader` | `SkillDef` |
| `SkillCatalogLoader` | `SkillProgressionSystem` |
| `SkillDef` | `SkillProgressionSystem` |
| `SkillProgressionState` | `SkillAtrophySystem` |
| `SkillProgressionState` | `SkillProgressionSystem` |
| `SkillProgressionSystem` | `SkillCatalogLoader` |
| `SkillProgressionSystem` | `SkillDef` |
| `SkillProgressionSystem` | `SkillProgressionState` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `SkillProgressionSystem` | 4 |
| `SkillDef` | 2 |
| `SkillAtrophySystem` | 1 |
| `SkillCatalogLoader` | 1 |
| `SkillProgressionState` | 1 |
| `LatentExpertAwakeningSystem` | 0 |

**Class split:** hub 4 · sink 1 · source 1 · isolated 0.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **5** · Test files: **28** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/LibraryStudyHostSession.cs`, `src/Host/NarrativeQuestlineHostSession.cs`, `src/Main.CampaignServices.cs`, `src/Main.Plans162_185.cs`, `src/UI/SkillMatrixPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 28 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs`, `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 4 · catalogs 2 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LATENT-EXPERT-TRUTH-239
wave: 17
status: PROPOSED — foreman claim required
packages: LET-239A, LET-239B, LET-239C, LET-239D, LET-239E
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
