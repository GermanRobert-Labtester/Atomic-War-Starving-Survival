# ASHFALL Roadmap Governance & Single-Truth Authority

**Canonical source:** Plan 29 / C1[7] "One Truth"\
**Authority level:** Authoritative specification for planning, plan numbering, collision resolution, and deliverable completion criteria across ASHFALL.

---

## 1. Roadmap Architecture & Flow of Truth

To prevent planning fragmentation and conflicting instruction layers, roadmap truth flows in a single hierarchical pipeline:

```text
                     ┌──────────────────────┐
                     │      AGENTS.md       │
                     │ canonical rulebook   │
                     └──────────┬───────────┘
                                │
                 sync generator │ python3 scripts/ci/sync-agent-rulebooks.py
                                ▼
    ┌────────────────────────────────────────────────────────┐
    │ 13 CLIENT RULEBOOKS (CLAUDE, CODEX, GEMINI, CURSOR...) │
    └───────────────────────────┬────────────────────────────┘
                                │
                                ▼
                    docs/CURRENT_AUTHORITY.md
                                │
                                ▼
                     docs/roadmap/README.md (this document)
                                │
                                ▼
                     docs/roadmap/WAVE_LEDGER.md
                                │
                                ▼
               Current Active Wave / Claimed Task Package
```

---

## 2. Plan Numbering Policy

ASHFALL enforces a strict plan numbering policy to prevent collisions across multiple planning agents:

| Range | Purpose | Examples | Authority |
|---|---|---|---|
| **`< 100`** | **Continuity & Hardening Waves** (Core domain logic, data authority, determinism, persistence, testing, governance) | Plan 14 (Economy Core), Plan 20 (Exposure/Weather), Plan 24 (Survivor Ledger), Plan 27 (Tests That Mean It), Plan 29 (One Truth), Plan 31 (Briefing Route Map) | Active continuity batches |
| **`>= 100`** | **Systemic Expansion Waves** (Quests, radio broadcast narrative, bionics, deep coast maritime, faction narrative) | Plan 144 (Moral Choice), Plan 147 (Barter Restock), Plan 173 (Radio Schedule Coordinator), Plan 190 (Amputation Travel), Plan 211 (Black Market) | Systemic expansions |
| **`piagentsplans/00–129`** | **Historical Evidence Backlog** | Pre-foreman feature exploration and historical audits | Historical reference only; not active instructions |

### Collision & Reservation Rules

1. **Reservation Before Implementation:** Agents must reserve plan numbers in `INTEGRATION_PLANS.md` and claim paths in `WORKTREE_OWNERSHIP.md` before drafting files.
2. **No Silent Renumbering:** Historical files are never renumbered byte-for-byte.
3. **Collision Handling:** If two historical or concurrent documents share a number (e.g., Plan 191 in endgame backlog vs Plan 191 in inspection):
   - One canonical identity is assigned by subject matching.
   - The colliding row is classified as a numbering drift and documented in `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`.

---

## 3. Definitions of Done by Deliverable Class

A system, a content tranche, a UI panel, a build, and an integration plan cannot be declared "done" under identical evidence. The following criteria are binding:

### 3.1 System Done
A gameplay system is **DONE** only when:
- **Domain Logic:** Core domain logic resides in `Assets/Ashfall.Core/` (`netstandard2.1`) with zero engine references.
- **Composition:** Thin host adapter/session created in `src/Host/` and composed in `src/Main.<Domain>.cs`.
- **Day Advance:** Registered with `CampaignDayCoordinator` / `IDayAdvanceOwner` if time-driven.
- **Persistence:** Registered in `SaveSectionRegistry` with deterministic capture and restore methods if stateful.
- **Player Surface:** Wired to a live player surface or registered panel route if player-facing.
- **Verification:** Focused unit tests pass in `Ashfall.Core.Tests/`, 100% round-trip save coverage passes, and determinism gate passes.

### 3.2 Content Done
A content catalog or narrative tranche is **DONE** only when:
- **Schema Valid:** Authored in `Assets/StreamingAssets/Data/` with valid `schema_version` and snake_case keys.
- **Catalog Loader:** Loaded through `CatalogPath` and validated by `CatalogIntegrityValidator`.
- **Reachability:** Reachable from gameplay (no orphaned IDs, no unreachable quest branches).
- **Effect Produced:** Content triggers actual domain facts (`EFFECT_PRODUCED`), not silent drop.
- **Integrity Gate:** `--data-integrity-selftest` and `json-schema-policy-gate.sh` pass cleanly.

### 3.3 UI Done
A user interface surface is **DONE** only when:
- **Live Route:** Registered in `PanelRegistryBootstrap.cs` and discoverable by `PanelRegistry`.
- **Read-Model Binding:** Binds strictly to Core read-models or Host session queries (no domain math in panels).
- **Keyboard & Accessibility:** Full tab/arrow keyboard navigation and close/back behavior supported.
- **Lifecycle & Disposal:** Cleanly unbinds subscriptions and passes `--panel-bind-lifecycle-selftest`.
- **Visual Verification:** Visual snapshot captured or matched in `snapshots/` where required.

### 3.4 Build Done
A release artifact is **DONE** only when:
- **Headless Export:** Headless Godot export succeeds for Linux and Windows targets without errors.
- **Data Packaging:** Exported PCK contains complete `Assets/StreamingAssets/Data/` assets.
- **Smoke Boot:** Binary boots headless, executes initialization cycle, and exits cleanly.
- **Save Compatibility:** Successfully loads golden save fixtures across all supported save versions.

### 3.5 Plan Done
An integration plan is **DONE** only when:
- **Acceptance Checklist:** Every clause in the execution contract is verified against source.
- **Zero Ambiguity:** No clauses remain "probably done" or "unknown".
- **CI Gates:** All required fast-tier CI gates (`bash scripts/ci/verify-fast.sh`) pass cleanly.
- **Implementation Log:** An implementation log is authored in `docs/plans/` documenting the premise, changes, tests, and handoff.
- **Terminal Status:** Marked `SEALED` or `DONE` in `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.

---

## 4. Capability Claims Verification

Every high-level capability claim made in documentation or agent instructions is backed by machine-verifiable evidence in:
- **`docs/architecture/CLAIMS.json`** — The machine-readable capability registry.
- **`scripts/ci/verify-capability-claims.py --check`** — The gate enforcing that all referenced source files, tests, and CI gates exist and remain valid.
