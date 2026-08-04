# ASHFALL Master Issue Register

**Audit date:** 2026-08-04  
**Status legend:** Open | Investigating | Fixing | Verifying | Closed  
**Owner default:** Unassigned (team)

Severity × Frequency × User impact × Regression risk used for priority ordering (see report § prioritization).

---

## Blockers

### AUDIT-001 — CI primary workflow deleted
| Field | Value |
|-------|-------|
| Category | build |
| Severity | **Blocker** |
| Frequency | Always (for CI-dependent merges) |
| Environment | GitHub Actions; any PR/main push |
| Reproduction | `ls .github/workflows/`; note `ci.yml` deleted in working tree; only `build.yml` remains |
| Expected | Automated validate + test + build on every PR |
| Actual | Historical `ci.yml` removed; release gate incomplete / secrets-dependent |
| Evidence | `git status` shows `D .github/workflows/ci.yml`; `build.yml` present |
| Root cause | Workflow removed during local WIP without replacement parity |
| Affected | CI/CD, merge quality gate |
| Proposed fix | Restore or replace with `build.yml` + explicit Unity version pin; document secrets |
| Regression risk | CI flakiness if secrets missing |
| Validation | Green Actions run on clean branch |
| Owner | Unassigned |
| Status | **Closed** (P0: restored `.github/workflows/ci.yml` + pinned `build.yml` to 6000.5.5f1) |
| Commit | Working tree (uncommitted) |

### AUDIT-002 — Unshippable working tree (70 commits ahead, ~414 dirty paths)
| Field | Value |
|-------|-------|
| Category | build / process |
| Severity | **Blocker** (for release) |
| Frequency | Always in current workspace |
| Environment | Local `main` → **`integration/audit-p1`** (P1 branch cut) |
| Reproduction | `git status -sb`; `git rev-list --count origin/main..HEAD` |
| Expected | Integratable, reviewable history |
| Actual | Large unpushed history + massive dirty tree (complexity splits + features) |
| Evidence | `main...origin/main [ahead 70]`; ~414 short-status lines |
| Root cause | Sustained multi-session feature + refactor without integration branch hygiene |
| Affected | All systems |
| Proposed fix | Branch cut; logical commits; push; PR; freeze WIP size |
| Regression risk | Merge conflicts |
| Validation | Clean CI on integration branch |
| Status | **Investigating** (P1: cut `integration/audit-p1`; remaining: logical commits + PR; see `docs/CI.md`) |

---

## Critical

### AUDIT-003 — Core tick path NullReference if foundation systems missing
| Field | Value |
|-------|-------|
| Category | crash |
| Severity | **Critical** |
| Frequency | Rare in full bootstrap; **Always** if partial init / test host misuse |
| Environment | PlayMode / runtime tick |
| Reproduction | Call `TickSystems` with `WeatherSystem` or `NeedsSystem` null (or destroy mid-session) |
| Expected | Hard fail at init or soft skip with fatal log |
| Actual | Direct calls e.g. `WeatherSystem.Tick`, `NeedsSystem.Tick`, `Shelter.Tick` without `?.` |
| Evidence | `Assets/_Game/Core/GameBootstrap.TickSystems.cs` lines ~43–45, 62, 256, 339, 357 |
| Root cause | Assumes InitializeSystems always completed; no runtime invariant guard |
| Affected | GameBootstrap tick loop |
| Proposed fix | Assert foundation non-null after init; use guarded tick or freeze GameState on failure |
| Regression risk | Masking real init bugs if over-guarded |
| Validation | PlayMode test: bootstrap then null-out forbidden; unit test init failure path |
| Status | **Closed** (P0: `AssertFoundationSystems` after init; EditMode `AuditP0SaveAndFoundationTests`) |

### AUDIT-004 — Partial ISaveable restore leaves hybrid world state
| Field | Value |
|-------|-------|
| Category | save system |
| Severity | **Critical** |
| Frequency | Intermittent (when one subsystem throws) |
| Environment | Load path |
| Reproduction | Register ISaveable that throws in RestoreState; load save |
| Expected | Load aborts or rolls back to last good snapshot |
| Actual | Error logged; other systems already restored → inconsistent state |
| Evidence | `SaveSystem.Entities.cs` catch logs error and continues loop |
| Root cause | Best-effort restore without transaction/rollback |
| Affected | SaveSystem, all ISaveable systems |
| Proposed fix | Two-phase restore (validate all JSON first) or fail entire Load on any subsystem error |
| Regression risk | Stricter load may reject older partial saves |
| Validation | EditMode: throwing ISaveable fails whole Load; state unchanged |
| Status | **Closed** (P0 option + P1: bootstrap applies `DefaultFailFastRestoreForEnvironment` in Editor/Dev) |

### AUDIT-005 — Corrupt save parse is silent (no log)
| Field | Value |
|-------|-------|
| Category | save system / logging |
| Severity | **Critical** |
| Frequency | Rare (disk corruption, kill mid-write of bak too) |
| Environment | Load |
| Reproduction | Truncate `save_*.json` mid-object; call Load |
| Expected | Logged error with path + exception type |
| Actual | Empty `catch` returns `(false,null,null)` without log |
| Evidence | `SaveSystem.IO.cs` `TryLoadFile` lines 141–144 |
| Root cause | Intentional fail-soft without observability |
| Affected | SaveSystem |
| Proposed fix | `Debug.LogWarning` with path + ex; metrics counter |
| Validation | EditMode AtomicSaveWrite / corrupt file test asserts log or return code |
| Status | **Closed** (P0: `TryLoadFile` logs parse/null/checksum; EditMode coverage) |

### AUDIT-006 — Package `com.unity.modules.physicscore2d` resolution failure on some editors
| Field | Value |
|-------|-------|
| Category | build |
| Severity | **Critical** (on affected installs) |
| Frequency | Always on Unity 6000.3.20f1 in this environment |
| Environment | Linux Editor 6000.3.20f1 |
| Reproduction | Open/batch project with 6000.3; package resolve error |
| Expected | Deterministic resolve on pinned editor |
| Actual | “Package cannot be found” for physicscore2d@1.0.0 |
| Evidence | Prior session `compile-complexity.log`; `Packages/manifest.json` line |
| Root cause | Module not available / wrong editor vs project version |
| Affected | Build, new contributors |
| Decision (P1) | **KEEP** `com.unity.modules.physicscore2d` — required by `com.unity.modules.physics2d` (packages-lock) and `ProjectSettings/PhysicsCoreProjectSettings2D.asset`. Do not remove. |
| Proposed fix | Pin docs + CI to **6000.5.5f1** (done); never open on 6000.3; document in `docs/CI.md` |
| Validation | Clean Library open on pinned editor; package remains in manifest |
| Status | **Closed** (P1: keep module; pin editor; documented) |

---

## High

### AUDIT-007 — GameBootstrap composition-root complexity
| Field | Value |
|-------|-------|
| Category | architecture |
| Severity | **High** |
| Frequency | Always (maintainability) |
| Evidence | ~94 public system properties; multi-file partial still central |
| Root cause | Organic growth of systems without service locator/DI boundaries beyond registry |
| Proposed fix | Continue extracting facades (EnvironmentHost, NarrativeHost, ShelterHost); inject interfaces |
| Validation | CodeScene/complexity budgets per file; compile + tests |
| Status | **Open** (partially mitigated by partials) |

### AUDIT-008 — Dual tick registration risk (explicit TickSystems vs SystemRegistry)
| Field | Value |
|-------|-------|
| Category | architecture / gameplay |
| Severity | **High** |
| Frequency | Unknown without full mapping |
| Evidence | `SystemRegistry`, `SystemWiring.WireDaily`, and large explicit `TickSystems` coexist |
| Root cause | Migration to registry incomplete |
| Proposed fix | Single dispatch table; assert no double registration |
| Validation | Day-tick test with spy counters per system |
| Status | **Open** |

### AUDIT-009 — JsonUtility full-graph save cost and schema fragility
| Field | Value |
|-------|-------|
| Category | performance / save system |
| Severity | **High** |
| Frequency | Every save |
| Evidence | Double `JsonUtility.ToJson` for checksum; large `SaveData` |
| Proposed fix | Stream hash while writing; optional binary; keep ISaveable expansion |
| Validation | Save duration benchmark EditMode |
| Status | **Open** |

### AUDIT-010 — Day-tick main-thread cost unbounded as systems grow
| Field | Value |
|-------|-------|
| Category | performance |
| Severity | **High** |
| Frequency | Frequent in long sessions / fast-forward |
| Evidence | `TickSystems` fans out 30+ systems; PlayMode `FastForwardStabilityPlayModeTests` exists but no FPS budget |
| Proposed fix | Profile; stagger non-critical daily systems; budget asserts |
| Validation | Profiler markers + CI perf smoke |
| Status | **Open** |

### AUDIT-011 — Utility AI evaluation scales with survivors × actions
| Field | Value |
|-------|-------|
| Category | performance |
| Severity | **High** |
| Frequency | Every AI evaluation interval |
| Evidence | `TickAiWave` fills large AIContext; loops survivors |
| Proposed fix | Dirty flags, action masks, spatial partitioning of actions |
| Validation | DayTickGc + timing tests with 8+ survivors |
| Status | **Open** |

### AUDIT-012 — Product scenes not production-ready
| Field | Value |
|-------|-------|
| Category | asset / gameplay |
| Severity | **High** |
| Frequency | Always for first-time launch |
| Evidence | Only `SampleScene` + URP template scene under Assets |
| Proposed fix | Boot scene → bunker scene; required refs validated |
| Validation | PlayMode enter-play smoke |
| Status | **Open** |

### AUDIT-013 — Large unreviewed economy / encounter factories
| Field | Value |
|-------|-------|
| Category | architecture / maintainability |
| Severity | **High** |
| Evidence | `DynamicEconomySystem.cs` ~1502 LOC; `EncounterEventFactory.cs` ~1517 LOC |
| Proposed fix | Split by domain; data-drive more content |
| Validation | Existing economy/event tests stay green |
| Status | **Open** |

### AUDIT-014 — Fast-forward / long-run stability not proven with metrics
| Field | Value |
|-------|-------|
| Category | performance / crash |
| Severity | **High** |
| Evidence | PlayMode fast-forward tests exist; no memory-growth CI gate |
| Proposed fix | 1000-day accelerated soak with memory ceiling |
| Status | **Open** |

---

## Medium

### AUDIT-015 — Empty catch in EventIdValidator
| Category | logging | Severity | Medium | Status | Open |
| Evidence | `EventIdValidator.cs` `catch { return; }` |
| Fix | Log warning with path |

### AUDIT-016 — ISaveable capture failure still produces incomplete save
| Category | save system | Severity | Medium | Status | Open |
| Evidence | Capture loop continues after per-system exception |
| Fix | Fail save if any ISaveable fails (optional strict mode) |

### AUDIT-017 — Diagnostics overlay may ship if left enabled
| Category | logging | Severity | Medium | Status | Open |
| Fix | `#if DEVELOPMENT_BUILD \|\| UNITY_EDITOR` hard gate |

### AUDIT-018 — Duplicate StreamingAssets roots
| Category | asset | Severity | Medium | Status | Open |
| Evidence | `Assets/StreamingAssets/items.json` and `.../Data/items.json` both present |
| Fix | Single source of truth; importer path docs |

### AUDIT-019 — Quarantine legacy still in tree
| Category | architecture | Severity | Medium | Status | Open |
| Evidence | `_quarantine_legacy/` 560K |
| Fix | Remove from release packaging; exclude from asmdefs (already outside `_Game`) |

### AUDIT-020 — PlayMode coverage thin vs system count
| Category | other (test) | Severity | Medium | Status | Open |
| Evidence | 13 PlayMode vs 76 EditMode files |
| Fix | Critical path PlayMode matrix |

### AUDIT-021 — Static EventBus lifetime across domain reloads
| Category | architecture | Severity | Medium | Status | Open |
| Evidence | `EventBus` static; version bump helps mid-dispatch but session reset needed |
| Fix | Clear on bootstrap Awake / new game |

### AUDIT-022 — Checksum uses full pretty JSON rewrite
| Category | performance | Severity | Medium | Status | Open |
| Fix | Hash payload bytes once |

### AUDIT-023 — No structured log schema
| Category | logging | Severity | Medium | Status | Open |
| Fix | Adopt GameLog fields: build, commit, session, system |

### AUDIT-024 — WebGL build in CI may be premature
| Category | build | Severity | Medium | Status | Open |
| Evidence | `build.yml` builds WebGL; Input/file IO assumptions may break |
| Fix | Gate WebGL until platform QA |

---

## Low

### AUDIT-025 — Inconsistent optional chaining on tick calls
| Severity | Low | Mix of `?.` and bare calls reduces readability |

### AUDIT-026 — Complexity-refactor meta files may lag
| Severity | Low | New `.cs` partials need Unity `.meta` generation (Editor creates on import) |

### AUDIT-027 — Visual Scripting package possibly unused
| Severity | Low | Audit package usage; remove if dead |

### AUDIT-028 — Collab Proxy package
| Severity | Low | Prefer pure git; remove if unused |

### AUDIT-029 — Naming: PowerNetwork vs “network” audit confusion
| Severity | Low | Document glossary |

### AUDIT-030 — Obsolete comments after partial splits
| Severity | Low | Doc-updater pass |

---

## Closed / Not applicable

| ID | Note |
|----|------|
| NET-* | Multiplayer networking **N/A** for this product scope |
| COMPLEXITY-* | Large-file complexity partially **mitigated** by 2026-08-04 partial-class work (compile PASS) — residual tracked as AUDIT-007/013 |

---

## Suggested first engineering sprint (1 week)

1. **AUDIT-001 + AUDIT-006** — CI + Unity pin  
2. **AUDIT-005 + AUDIT-004** — save observability + fail policy  
3. **AUDIT-003** — tick invariants  
4. Run full EditMode suite; file failures as new issues  
5. **AUDIT-002** — integrate dirty tree onto reviewable PRs  

---

*Issues are not “Closed” until reproduction, root cause, fix, regression test, and verification evidence exist (audit plan §1).*
