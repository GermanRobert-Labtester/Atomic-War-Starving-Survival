# PLAN-GUILT-INSOMNIA-TRUTH-246 — Sleeplessness from Conscience: Triggers & Relief

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-TRAUMA-SYSTEM-TRUTH-230, PLAN-SHELTER-CAPACITY-AUTHORITY-103, PLAN-DREAM-SYSTEM-TRUTH-229.
**Non-goals:** no psychological model (Plan 64), no trauma exposure (Plan 230),
no sleep quality model (Plan 103).

## 1. Outcome
`Survivors/GuiltInsomniaSystem.cs` (**225 lines**) is reachable and unaddressed:
insomnia driven by a survivor's own acts (Plan 136/231 history). It is a small
system that either does nothing or becomes an unexplained energy drain.

| Deliverable | Detail |
|---|---|
| Trigger model | guilt load derives from documented facts (moral history Plan 231, committed acts Plan 136, losses Plan 123) |
| Sleep effect | insomnia reduces sleep quality through Plan 103's model as a typed input; no private energy penalty |
| Relief paths | confession (Plan 127), care (Plan 144), time — each with a documented effect |
| Bounds | insomnia cannot drive sleep quality below the model's floor; recovery is possible |
| Save truth | guilt load and sleep state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` (225 lines; unaddressed — Wave 18 audit).
- Plan 231's moral trajectory and Plan 136's history are the load source.
- Plan 103 owns sleep quality; Plan 127 confession; Plan 144 care.

## 3. Packages
- **GIT-246A** guilt-load model + source table.
- **GIT-246B** sleep-input contract with Plan 103 + floor test.
- **GIT-246C** relief paths (confession/care/time) + tests.
- **GIT-246D** bounds/recovery fixtures.
- **GIT-246E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Insomnia appears only in Plan 103's sleep model; relief paths measurably help.
- Bounds hold; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Unexplained drain → the input contract and relief paths are fixtures.
Duplicate morale → sleep only; no parallel mood value.

---

## 6. Expanded census (2 files · 332 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `GuiltInsomniaSystem.cs` | 225 | System | **yes** | 0 | 0 | 2 |
| `GuiltSourceCatalog.cs` | 107 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `guilt_sources.json` | array[40] |

**State surfaces:** `GuiltInsomniaSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 9 name references across the test tree |
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
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `GIT-246A` | `GuiltSourceCatalog.cs`, `GuiltInsomniaSystem.cs` |
| `GIT-246B` | no name match — resolve at claim time |
| `GIT-246C` | no name match — resolve at claim time |
| `GIT-246D` | no name match — resolve at claim time |
| `GIT-246E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.PanelTests.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.FlagshipInstitutions.cs`, `src/UI/Phase0Panel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`, `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`, `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`, `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`, `Ashfall.Core.Tests/InstitutionCanonicalReliefTests.cs` |
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

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnGuiltInsomniaCritical` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |
| `OnGuiltRecorded` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |
| `OnGuiltResolved` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/guilt_sources.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

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
(OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `guilt_sources.json` | OPTIONAL |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 2 · test regions 0 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-GUILT-INSOMNIA-TRUTH-246
wave: 18
status: PROPOSED — foreman claim required
packages: GIT-246A, GIT-246B, GIT-246C, GIT-246D, GIT-246E
claim paths:
  - Assets/StreamingAssets/Data/guilt_sources.json  # §17 catalog (verify schema + consumer)
  - guilt_sources.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --expedition-panel-lifecycle
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
