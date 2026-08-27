========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# ASHFALL (Atomic War) — Comprehensive Technical Audit Report

| Field | Value |
|-------|-------|
| **Project** | ASHFALL / Atomic War — 2D nuclear-survival management |
| **Engine** | Unity 6000.5.5f1 (URP 2D) |
| **Audit date** | 2026-08-04 |
| **Git HEAD** | `a6c90a0` (main, **ahead of origin by 70 commits**) |
| **Working tree** | **~414 dirty paths** (large uncommitted refactor + feature work) |
| **Audit scope** | Static analysis, repository/build inspection, architecture mapping, compile verification |
| **Out of scope (this pass)** | Live play profiling, GPU captures, multi-platform device lab, crash-dump triage from field, network multiplayer (N/A) |
| **Authoritative compile** | Unity 6000.5.5f1 batchmode → **PASS** (`compile-complexity4.log`: *Exiting batchmode successfully*) |

---

## 1. Executive Summary

### Overall technical health: **YELLOW / pre-alpha systems-rich**

ASHFALL is a **dense, data-driven single-player simulation** with strong domain modeling (radiation, shelter, factions, save/load, utility AI) and a large EditMode test suite (**~85 test files**, **76 EditMode / 13 PlayMode**). Architecture is intentionally modular (18 asmdefs under `Assets/_Game` + tests) with plain-C# systems behind thin MonoBehaviours.

Recent work improved maintainability (partial class splits, `SaveSystem.CoreDeps`, `SystemRegistry`, atomic saves). **Release readiness is not met.**

### Release-readiness assessment: **NOT READY**

| Gate | Status |
|------|--------|
| Clean reproducible build from CI | **FAIL** — primary CI workflow deleted; remaining `build.yml` depends on secrets |
| Clean working tree / shippable branch | **FAIL** — 70 commits unpushed, 400+ local changes |
| No known blockers | **FAIL** — CI/package risk, god-object coupling, incomplete scene productization |
| Save resilience | **PARTIAL PASS** — atomic write + checksum + bak + migrations exist; silent parse failures need telemetry |
| Automated tests green in CI | **UNKNOWN** — CI not proven on current tree |
| Performance budgets measured in build | **NOT MEASURED** this pass |
| Multiplayer / network | **N/A** (single-player) |

### Issues by severity (this audit register)

| Severity | Count |
|----------|------:|
| Blocker | 2 |
| Critical | 4 |
| High | 8 |
| Medium | 10 |
| Low | 6 |
| **Total** | **30** |

### Primary crash / hard-fail risks

1. **Null-deref on core tick path** if foundation systems are missing (`WeatherSystem` / `NeedsSystem` / `Shelter` called without `?.` in `GameBootstrap.TickSystems`).
2. **Partial save restore** when individual `ISaveable` capture/restore throws — logged, but session may continue with **inconsistent hybrid state**.
3. **Corrupt/truncated save JSON** — silently fails parse (`catch` without log in `TryLoadFile`); user may see “load failed” with weak diagnostics.
4. **Package resolution fragility** (`com.unity.modules.physicscore2d`) on mismatched Unity installs (seen under 6000.3).

### Largest performance bottlenecks (inferred; not profiler-confirmed)

1. **Day-tick fan-out** — dozens of systems ticked from `GameBootstrap.TickSystems` every game-hour chunk.
2. **Utility AI wave** — per-survivor evaluation with large `AIContext` field fill (mitigated by scratch context; still O(survivors × actions)).
3. **JsonUtility full-graph save** — large `SaveData` DTO (~60+ subsystem fields) serialized twice for checksum.
4. **Encounter/event factory bulk** — large static factory files (`EncounterEventFactory` ~1.5k LOC).

Evidence of intentional GC discipline: `DayTickGcProfileTests` (100-day alloc budget), AIContext scratch reuse, cooldown key buffers in `EventRunner`.

### Highest-risk systems

| System | Why |
|--------|-----|
| `GameBootstrap` | ~94 public system properties; single composition root; init + tick + UI + narrative handlers |
| `SaveSystem` | Player progress integrity; JsonUtility limits; migration surface |
| `ExpeditionSystem` | Multi-phase state machine, encounters, hatch dilemma, UXO |
| `EventRunner` | Weighted events, delayed consequences, scheduling, flags |
| `MedicalSystem` / radiation pipelines | Coupled vitals; silent wrong state = unfair death |
| `DynamicEconomySystem` | Trust/stance/raids; large file (~1500 LOC) |

### Architectural weaknesses

1. **Composition-root god object** — almost all systems hang off `GameBootstrap`.
2. **Dual tick paths** — some systems ticked explicitly in `TickSystems`, others via `SystemRegistry` / `SystemWiring` — risk of double-tick or missed tick.
3. **JsonUtility DTO sprawl** — every new system tends to add fields + Set* + Capture/Restore (partially mitigated by `ISaveable`).
4. **Productization gap** — only template/sample scenes; no polished boot flow observed in audit.
5. **Observability incomplete** — `DiagnosticsOverlay` + `LogRotationManager` + conditional `GameLog` exist; no structured session/correlation IDs.

### Recommended remediation order

1. Restore CI + pin Unity version; remove/fix invalid package deps.
2. Stabilize save silent failures + partial-restore policy.
3. Harden tick null-safety and document init/tick contracts.
4. Run full EditMode + critical PlayMode suite; gate merges.
5. Profile day-tick + AI wave + save duration; set budgets.
6. Continue decomposing `GameBootstrap` / economy / encounter factories.
7. Scene/boot productization and release packaging.

### Estimated impact if unresolved

- **Ship now:** high chance of CI/build confusion, save edge-case data loss, late-game tick instability, unmeasured frame spikes on long runs.
- **Defer 2–4 weeks of gates above:** systems content can keep landing, but regression cost grows with 70+ local commits and dirty tree.

---

## 2. Performance Baseline

| Metric | Status | Notes |
|--------|--------|-------|
| Avg / 1% / 0.1% FPS | **NOT MEASURED** | Requires PlayMode profiler session on target hardware |
| Frame-time consistency | **NOT MEASURED** | |
| Main / render / GPU time | **NOT MEASURED** | |
| Draw calls / tris / verts | **NOT MEASURED** | 2D URP expected modest; not verified |
| GC frequency | **PARTIAL** | EditMode budget: ≤256 KB/steady day in `DayTickGcProfileTests` |
| Peak memory / growth | **NOT MEASURED** | |
| Asset load duration | **NOT MEASURED** | StreamingAssets JSON validated by importer tests |
| Scene transition | **NOT MEASURED** | Few scenes present |
| Save / load duration | **NOT MEASURED** | Atomic write path implemented; needs timing harness |
| Network latency / loss | **N/A** | Single-player |
| Server tick | **N/A** | |
| Build size | **NOT MEASURED** | No artifact from this audit |
| Startup / shutdown | **NOT MEASURED** | |

**Action:** Add a PlayMode “perf smoke” that records FPS, GC.GetTotalMemory, and save duration for 10-minute accelerated sim.

---

## 3. Architecture and Dependency Map

### 3.1 Main systems (by asmdef)

```
Data (SOs, catalogs)
  ↑
Survivors / Environment / Radiation / Inventory / Shelter / Medical / Economy / Simulation
  ↑
AI / Events / Crafting / UI
  ↑
Core (GameBootstrap, SaveSystem, Expedition, Registry, Flashpoint, …)
  ↑
Editor / Tests
```

### 3.2 Shared-state ownership

| State | Owner | Consumers |
|-------|--------|-----------|
| Survivors list + needs | `GameBootstrap` / `NeedsSystem` | AI, Medical, Events, Save |
| Shelter modules/rooms | `Shelter` | Power, Atmosphere, Craft, AI |
| Inventory | `Inventory` | Craft, AI, Economy, Save |
| World flags / scheduled events | `SaveSystem` + `EventRunner` | Narrative chains |
| Map / knowledge fog | `GeneratedMap` / `RadiationKnowledgeMap` | Expeditions, UI |
| Faction trust/economy | `DynamicEconomySystem` | Raids, trade, events |

### 3.3 Initialization order (documented from code)

`GameBootstrap.InitializeSystems()` orchestrates:

1. `InitFoundation` — registry, GameState, Time, Weather, Temp, Photoperiod, Shelter modules
2. `InitUtilityAI` — UtilityAI + action list
3. `InitMedicalSystems`
4. `InitEventsAndSurvivors` → EventRunner, mental break, addiction, world/tactical/narrative/atmosphere/diary/hatch/faction map systems
5. `InitSaveAndExpeditions`
6. `InitRadioAndEndgame`
7. `FinishSystemRegistration` → `RegisterSystemsInRegistry`

### 3.4 Update loops

| Loop | Location | Role |
|------|----------|------|
| `Update` / `TickFrame` | `GameBootstrap.Lifecycle` | Real-time → game-hours accumulation |
| `TickSystems` | `GameBootstrap.TickSystems` | Environment → needs/medical/psyche → rad/water/craft → AI → events |
| Per-system `Tick` | Many plain C# systems | Domain simulation |
| UI `Update` | HUD, overlays | Presentation only |

### 3.5 Event / message flows

- **C# events** on systems (`OnChoiceApplied`, expedition events, etc.)
- **`EventBus`** static typed pub/sub (`FlashpointEmptiedDevices`, weather, raids)
- **`EventRunner`** data-driven narrative with delayed consequences + day schedule
- **AudioEventBus** bridges gameplay → audio parameters

### 3.6 Save-data flow

```
PreCaptureHook → CaptureSnapshot (core + subsystems + ISaveable)
  → JsonUtility → checksum → temp file → File.Move atomic → .bak previous
Load: primary file → checksum → migrate → RestoreFromSnapshot
     fail → try .bak
```

### 3.7 Networking

**None** as multiplayer. “Network” wording in code refers to **PowerNetwork** (electrical grid), not online play.

### 3.8 Asset loading

- ScriptableObjects + `StreamingAssets` JSON (`items`, `recipes`, `events`, `locations`, …)
- `JsonDataImporter` + build validation gate
- No Addressables/bundle streaming observed in Core

### 3.9 Third-party packages (notable)

URP 17.6, Input System 1.19, Test Framework 1.7, 2D feature set, Timeline, Visual Scripting, AI Navigation, Collab Proxy.

---

## 4. Build & Repository Findings

| Finding | Detail |
|---------|--------|
| CI deleted | `.github/workflows/ci.yml` **deleted** in working tree (was data-validation oriented) |
| Remaining CI | `.github/workflows/build.yml` — game-ci EditMode tests + Windows/WebGL builds; needs Unity license secrets |
| Dirty tree | ~414 paths; large complexity-refactor partials uncommitted |
| Ahead of origin | 70 commits |
| Quarantine | `_quarantine_legacy` (~560K) still present — keep out of player builds |
| `.gitignore` | Standard Unity template; `*.csproj` ignored (good) |
| physicscore2d | Listed in `manifest.json`; failed resolve on Unity 6000.3.20f1 in prior session |

---

## 5. Observability

| Capability | Present? | Gap |
|------------|----------|-----|
| Diagnostics overlay | Yes (`DiagnosticsOverlay`) | Dev-only; ensure stripped/disabled in release |
| Log rotation | Yes (`LogRotationManager`) | Good |
| Conditional debug log | Yes (`GameLog` + `ASHFALL_DEBUG`) | Need define in CI debug builds only |
| Structured logs | Partial | No session ID / correlation / build metadata in every line |
| Silent failure telemetry | Partial | `TryLoadFile` parse failures unlogged |

---

## 6. Test Coverage Snapshot

| Suite | Approx files | Role |
|-------|-------------|------|
| EditMode | 76 | Systems, save, events, economy, AI, GC profile, build validation |
| PlayMode | 13 | Audio, chronic disease, endgame, storms, fast-forward, bootstrap stub, moral chronicle |

**Gaps:** full bootstrap integration smoke, long-run memory, save migration from real player files, visual regression, input remapping, performance budgets in CI.

---

## 7. Prioritized Remediation Roadmap

| Priority | Work | Est. effort |
|----------|------|-------------|
| P0 | Restore CI workflow; pin Unity 6000.5.5f1; fix/remove `physicscore2d` if invalid | 1–2 d |
| P0 | Commit or branch complexity refactor; stop growing dirty tree | 0.5 d process |
| P0 | Log + metric on save parse/checksum/partial restore failures | 1 d |
| P1 | Null-safe tick foundation + init failure hard-stop | 1–2 d |
| P1 | Full EditMode + selected PlayMode in CI green | 2–3 d |
| P1 | Day-tick / AI / save profiler baselines + budgets | 2 d |
| P2 | Double-tick audit (Registry vs explicit TickSystems) | 2 d |
| P2 | Continue god-file splits (Economy, EncounterEventFactory, HatchDefense) | ongoing |
| P3 | Scene boot UX, packaging, Addressables if needed | later |

---

## 8. Definition of Done vs This Audit

| DoD item | Met? |
|----------|------|
| Builds reproducibly from clean env | **No** (CI/package/process) |
| No known blockers | **No** |
| No known critical crashes | **Unknown in play** — static risks documented |
| Symbolicated crashes | **Not configured** in this audit |
| Save resilient | **Mostly designed; gaps logged** |
| Major loops tested | **Partial** |
| Performance within budgets | **Budgets not set / not measured** |
| Memory stable long session | **Not measured** |
| Platforms pass | **Not verified** |
| Visual regression | **None** |
| Assets validated | **Partial** (JSON gate exists) |
| Dev-only excluded from release | **Needs build define audit** |
| Logs expose failures safely | **Partial** |
| High-risk fixes have regression tests | **Many systems yes; wiring no** |
| Remaining issues documented | **Yes — this report + issue register** |

---

## 9. Appendices

- Master issue register: [`ISSUE_REGISTER.md`](../deprecated_audits/ISSUE_REGISTER.md)
- Compile evidence: `compile-complexity4.log` (repo root, session artifact)
- AGENTS workflow: `AGENTS.md`
- Domain notes: `IntelBible.md`

---

*This audit is evidence-based for static/repo/compile dimensions. Runtime FPS, GPU, and hardware validation require a follow-up Phase (PlayMode profiling + device lab).*
