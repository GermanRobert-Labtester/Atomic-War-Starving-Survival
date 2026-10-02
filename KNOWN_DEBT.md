# ASHFALL Known Debt

> **TRIMMED 2026-10-02 (Task 55).** The 63 RETIRED historical rows are
> archived verbatim at
> [`docs/archive/coordination/2026-10-02/KNOWN_DEBT_full.md`](docs/archive/coordination/2026-10-02/KNOWN_DEBT_full.md)
> (`sha256:71d609e60ced06b0`). Only current, decision-relevant debt remains here.


# ASHFALL Known Debt

Only current, decision-relevant debt belongs here. Historical detail lives in
the linked archive or source document.

| ID | Status | Area | Evidence / reason | Owner role | Promotion condition | Reviewed |
|---|---|---|---|---|---|---|

## Status meanings

- `ACCEPTED`: known, intentionally deferred, and not repeatedly rediscovered.
- `BLOCKED`: valid need lacks an API, content authority, decision, or dependency.
- `QUARANTINED`: preserved outside active compilation/runtime with evidence.
- `RETIRED`: historical behavior that must not be restored.
- `PROMOTED`: approved for a named active package.

Every new row needs an evidence pointer, owner role, and promotion condition.
| DEBT-COORDINATOR-RETRY-NONRESTORABLE-OWNERS | ACCEPTED | Campaign day coordinator / retry hardening | **Recorded 2026-10-01 (ui-composition-harness integration).** The new `--ui-composition-harness-selftest` builds the real UI/composition root and reports **120 production day owners, 92 implementing `IPreDaySnapshotRestore`, 28 not**. The harness's canonical inventory witness (`canned_food`) proves the restorable path is exactly-once across repeated fault/retry cycles, but a non-restorable owner that mutates state during a failed day may double-apply on the same-day retry. Artifact: `artifacts/ui-composition-harness.json` (`non_restorable_owners`). | Integrator | Each listed owner must either implement `IPreDaySnapshotRestore` with a real capture/restore, or be proven idempotent; the harness is the verification instrument | 2026-10-01 |
| DEBT-TEST-QUARANTINE-2026-09-12 | RECONCILED | Historical xUnit quarantine | **Actuality reconciled 2026-09-18:** the 51 active `Compile Remove` entries were ghost metadata; their referenced source files are absent from the current test tree, so the exclusions changed no compiled test set. The ghost entries were removed and `QuarantineManifestGateTests` now requires every future explicit `Compile Remove` quarantine to reference a real source file. Historical drafts remain recoverable from git/Twin evidence; restoring one is a deliberate implementation package, not a csproj placeholder. | Integrator | Restore a named historical source only after rematching it to current APIs/content and proving its focused suite; any explicit quarantine must point to a real file | 2026-09-18 |
| DEBT-PLAN-SPRAWL | ACCEPTED | Planning process | Historical plan trees remain preserved but are not live authority; new work enters the single integration ledger | Foreman | Historical status/archive pass is separately scheduled | 2026-09-12 |
| DEBT-RULEBOOK-SNAPSHOT | ACCEPTED | Instruction history | Pre-foreman rulebooks are byte-preserved with hashes in `docs/archive/agent-rules/2026-09-12-pre-foreman/` | Foreman | Retain as history; do not restore contradictory active rules | 2026-09-12 |
| DEBT-GODOT-PARTIAL-REQUIRED | ACCEPTED | Godot source-generator contract | `Godot.NET.Sdk/4.7.1` requires `partial` on every `Godot.Node`/`Control`-derived class — generators emit script-path attributes and binder code into a companion partial part. POTENTIALCLUTTER.md finding F10 ("dead `partial` on 43 UI singletons") is a FALSE POSITIVE: real scope is 241 files, all Godot-derived; removing the keyword breaks scene script attachment. | Foreman | Never remove `partial` from Node/Control-derived classes; only non-Godot POCOs are candidates | 2026-09-12 |
| DEBT-WORKTREE-DECLUTTER-2026-09-12 | QUARANTINED | Worktree deletion landing | 2,852 tracked deletions landed (index synced to worktree) per POTENTIALCLUTTER.md C2/A4/A5/A6/A7/A10/F7. 2,506 files extracted from git HEAD and archived with full project-relative paths + SHA256SUMS + ARCHIVE_NOTE.md in `Twin_ASHFall/quarantine/2026-09-12-worktree-deletions/` (verified 0 mismatches). Also untracked: 314 `.cache/audio_*` files (live via Twin symlink), 3 `TestResults/*.trx`, 28 scratch reauthor scripts, `_migrated/UI_StyleReference_01.jpg`. 7 orphan forensics reports archived to `docs/archive/forensics/2026-09-12/`. SPDX `// SPDX-License-Identifier: MIT` added to 1,867 C# files (1,223 src/Core per F5 + 644 tests); `license-header-check.sh --strict` PASS (1,930 changed files); build 0/0. | Integrator | Restore from Twin archive by copying paths back; do not delete the archive | 2026-09-12 |
| DEBT-CLUTTER-HOLD-2026-09-27 | BLOCKED | POTENTIALCLUTTER.md held findings | A9 ziva addon (untracked), B3 `src/Main.Plans126_129.cs` (lane rename), B5/F9 `.qwen` mirror, C2 lane deletions, C4 Unity UI/SDF font cluster, C6 tool outputs, F1 AI-workspace ignore policy, F2 `snapshot-capture/`, F4 `.gitattributes` Unity template. Evidence: `docs/hygiene/CLUTTER_DISPOSITION_2026-09-27.md`. | Foreman | Foreman decision per finding; archive POTENTIALCLUTTER.md once all are closed | 2026-09-27 |
| DEBT-ADDONS-GODOT-MCP-UNTRACKED | BLOCKED | Repo hygiene / addon policy | `.gitignore` `/addons/` also ignores the enabled `godot_mcp` plugin (`project.godot` enabled plugins), so a fresh clone lacks a live dependency. | Foreman | Decide: track `addons/godot_mcp/` via an ignore exception, or document the per-machine install | 2026-09-27 |
| DEBT-SENTRY-DANGLING-PACKAGE | BLOCKED | Build dependencies | `Sentry` 6.9.0 pinned (`Directory.Packages.props`) and referenced (`Ashfall.csproj`) but never initialized; no DSN or crash hook exists (`docs/telemetry/SENTRY_CRASH_REPORTING_AUDIT_2026-09-27.md`). Removal waits on the active build-builder claim on `Ashfall.csproj`. | Integrator | Remove both references when the `Ashfall.csproj` claim closes, or approve a real crash pipeline | 2026-09-27 |
| DEBT-RATIONING-CRISIS-NO-PRODUCER | BLOCKED | Economy / rationing crises | `ResourceRationingSystem.DeclareCrisis`/`ResolveCrisis` have no production caller, so `OnCrisisDeclared`/`OnCrisisResolved` never fire and crisis audio cannot be wired truthfully (`docs/audio/AUDIO_TASK8_COVERAGE_RECORD_2026-09-27.md`). | Foreman | Approve a crisis producer; then bind audio through `ExpansionAudioBridge` | 2026-09-27 |
