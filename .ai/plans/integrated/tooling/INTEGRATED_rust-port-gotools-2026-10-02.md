# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

# Staged Rust Port of the ASHFALL Go Dev Toolchain

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

**Stage 4 executed 2026-10-02.** Go toolchain deleted (`git rm -r tools/gotools`,
41 files; `git ls-files '*.go'` = 0); Rust (`tools/rstools`) is authoritative.
Manifest retargeted to `tools/rstools/target/release/ashfall-dev` with
`rstools_test`/`rstools_vet`/`build_rstools` (74 gates / 70 fast, note 1.1.9);
`.github` → `dtolnay/rust-toolchain`; `language-policy-gate` allowlist removed;
`reachability-report` ported to Rust. Verified: manifest valid, inventory PASS,
`cargo test` 124/124, `clippy -D warnings` clean, retargeted gates PASS,
language gate PASS, docs index PASS.

> **Authority note.** The user directed on 2026-10-02: *"Please remove any .go
> code and either convert to rust or remove!"* and, when asked to disambiguate,
> chose **"Staged full Rust port."** This is a deliberate, user-authorized
> override of the current *Go-Only Tool Creation Policy* recorded in
> `QWEN.md` / `AGENTS.md` / `TEST_POLICY.md`. Per `QWEN.md` rule 10 this is a new
> architecture decision, so it is recorded here and reasoned explicitly; the user
> is the authority that grants it. No Unity, no Core, no gameplay, no save, no
> determinism, and no data-authority behavior is in scope.

## Bounded outcome

Replace the Go toolchain at `tools/gotools/` (38 files, 8091 LOC, module
`ashfall/gotools`, Go 1.26) with a Rust workspace at `tools/rstools/` that
preserves the observable CLI contract (`ashfall-dev <subcommand>`), the JSON
shapes consumed by `scripts/ci/run-gates.py`, and the exit codes relied on by CI
gates, the pre-commit hook, and `bin/run-scoped-tests`. Cut over all callers,
then delete the Go source.

## Non-goals

- No change to game code, Core, data JSON, save format, determinism, or RNG.
- No change to gate *semantics* — only the implementation language of the tools
  the gates invoke. Gate IDs are renamed only in the final cutover stage.
- No new gameplay/data authority and no new persistence.

## Why staged (not a big-bang rewrite)

`bin/ashfall-dev` is load-bearing: `scripts/ci/run-gates.py --jobs N` pipes task
JSON into `bin/ashfall-dev run-tasks -j N -json`; CI gates run
`go run -C tools/gotools ./cmd/...`; the pre-commit hook calls
`bin/check-approved-plan`; every scoped test run calls `bin/run-scoped-tests`.
Deleting Go first would break the build/test loop. Staging adds the Rust tool
*alongside* the Go one, proves parity per command, then flips callers, then
removes Go — so at every commit the toolchain works.

## Port surface (Go → Rust)

| Stage | Go packages / commands | Rust modules |
|---|---|---|
| 1 (this turn) | `pkg/selector`, `pkg/runner`, `pkg/scopedtest`; `select-tests`, `run-tasks`, `run-scoped-tests` | `selector`, `runner`, `scopedtest` |
| 2 | `pkg/validator`, `pkg/config`, `pkg/parser`, `pkg/scanner`, `pkg/manifest`, `pkg/indexer`; `validate-json`, `validate-config`, `parse-results`, `scan-saves`, `build-manifest`, `index` | `validator`, `config`, `parser`, `scanner`, `manifest`, `indexer` |
| 3 | `pkg/checkplan`, `pkg/agentsync`, `pkg/catalogaudit`, `pkg/releasepolicy`, `pkg/monitor`, `pkg/proxy`, `pkg/orchestrator`; `check-plan`, `sync-agents`, `audit-catalogs`, `releasepolicy`, `monitor-size`, `monitor-compile`, `llm-proxy`, `agent-core` | `checkplan`, `agentsync`, `catalogaudit`, `releasepolicy`, `monitor`, `proxy`, `orchestrator` |
| 4 | cutover + removal | — |

## Stage 4 cutover checklist (concrete — started 2026-10-02)

**Live-gate surface — Rust parity verified for every entry:**

| Caller | Current (Go) | Rust replacement | Parity |
|---|---|---|---|
| manifest `releasepolicy` gate | `go run -C tools/gotools ./cmd/releasepolicy …` | `ashfall-dev releasepolicy …` | ✅ byte-id mod clock/HEAD |
| manifest `monitor-size` | `go run … ./cmd/ashfall-dev monitor-size …` | `ashfall-dev monitor-size …` | ✅ (agent-verified) |
| manifest `monitor-compile` ×2 | `go run … monitor-compile …` | `ashfall-dev monitor-compile …` | ✅ (agent-verified) |
| manifest `catalog_audit` | `go run … audit-catalogs --check/--json` | `ashfall-dev audit-catalogs …` | ✅ byte-identical |
| manifest `gotools_test` | `go test -C tools/gotools ./...` | `cargo test --manifest-path tools/rstools/Cargo.toml` | ✅ 121/121 |
| manifest `gotools_vet` | `go vet -C tools/gotools ./...` | `cargo clippy --manifest-path tools/rstools/Cargo.toml -- -D warnings` | clippy 0.1.98 present |
| pre-commit | `bin/check-approved-plan` (Go) | Rust `check-plan` shim | ✅ byte-identical |
| `run-gates.py --jobs` | `bin/ashfall-dev run-tasks -j N -json` | Rust binary | ✅ |

**Design constraint:** Go gates use `go run` (compile-on-demand). Rust gates need
the binary built first, so rewritten gates must add a `build_rstools`
(`cargo build --release`) dependency, or use
`cargo run --release --manifest-path tools/rstools/Cargo.toml -p ashfall-dev -- …`.

**Exact edits:**
1. `bin/` shims for the Rust multicall binary (`ashfall-dev`,
   `run-scoped-tests`, `validate-config`, `check-approved-plan`) — `bin/` is
   gitignored; no existing script builds it, so add a small build/install step.
2. `docs/ci/CI_GATE_MANIFEST.json`: swap the 5 `go run` commands to the Rust
   binary; `gotools_test`→`rstools_test` (`cargo test`), `gotools_vet`→
   `rstools_vet` (`cargo clippy`); regenerate `docs/ci/GATE_INVENTORY.md`; keep
   `gate_inventory_drift` and `CiGateManifestDriftTests` green.
3. `.github/workflows/{ci,build,release,selftest-manifest-regen}.yml`: Go →
   Rust (`dtolnay/rust-toolchain`).
4. `scripts/ci/language-policy-gate.py`: drop the `tools/gotools/` allowlist.
5. `scripts/ci/git-hooks/pre-commit` + `scripts/ci/release-gate.sh`: Rust refs.
6. `git rm -r tools/gotools` + `go.mod`/`go.sum`.
7. Policy docs: `AGENTS.md` already states Go-retired/Rust-first, so little is
   needed — but **editing `AGENTS.md` is blocked by auto-mode policy** and needs
   explicit approval.

**Not at parity, but NOT live callers** (verified by grep — no gate, script, or
workflow invokes them): `llm-proxy`, `agent-core`, `index --watch/--serve`,
`cmd/reachability-report`. They exit 2 in the Rust port. A cutover retires them.

**Gated before the destructive step (review rulings):** "full parity" holds for
every *live* caller but not the four non-live commands; the coordinated
`get_changed_files` truncation fix is still pending; and the CI flip + Go
deletion is authoritative shared infrastructure requiring an explicit go-ahead.

## Parity strategy

- Rust modules are ported line-for-line against the current Go source; the
  selector's 7 mapping sections, candidate ordering, fallback branches, and
  explanation strings are reproduced exactly.
- The Rust runner reproduces the `run-tasks --json` payload exactly:
  `id`, `exit_code`, `duration` (ns), `duration_seconds`, `output`,
  `peak_rss_kb`, optional `error`, `timed_out`; `timeout` is read as ns.
- Verification per stage: `cargo test` (unit parity tests mirroring the Go
  tests) **plus** a manual diff of `select-tests --json` and
  `run-tasks -json` output between `go run` and the Rust binary on the live
  worktree.
- The Go toolchain is not deleted until every stage's parity diff is clean.

## Files owned by this plan (Stage 1, this turn)

- `tools/rstools/` (new): `Cargo.toml`, `README.md`,
  `crates/ashfall-dev/{Cargo.toml,src/main.rs,src/selector.rs,src/runner.rs,src/scopedtest.rs}`
- `tools/rstools/Cargo.lock` (generated by cargo)
- `.ai/plans/rust-port-gotools-2026-10-02.md` (this file)
- `WORKTREE_OWNERSHIP.md` (this claim), `.ai/state.md`

**Additive only this turn.** No Go file, no CI manifest, no `bin/` shim, and no
policy doc is edited in Stage 1, so a concurrent session touching `tools/gotools`
cannot conflict.

## Verification for Stage 1

- `cargo build --release` clean in `tools/rstools/`.
- `cargo test` green (selector mapping, runner timeout/RSS/concurrency,
  scopedtest full-test ban + dry-run).
- Manual parity: `select-tests --json` and `run-tasks -json` produce the same
  results as the Go binary for identical inputs.
- The Go toolchain still builds and `bin/ashfall-dev` still works (no regression).

## Stage 1 results (measured 2026-10-02)

| Check | Result |
|---|---|
| `cargo build --release` (workspace) | 0 warnings / 0 errors |
| `cargo test` | 18/18 pass |
| `select-tests --json` vs `go run … select-tests --json` | **byte-identical** (`diff` empty) |
| `run-tasks -json` structure (ids/exit_code/output/timed_out) | identical; only timing/RSS numbers differ |
| `run-tasks` timeout task | both `exit_code=124`, `timed_out=true` |
| `run-scoped-tests --dry-run` | **byte-identical** |
| `run-scoped-tests --full` (ban, vs compiled `bin/ashfall-dev`) | **byte-identical**, both exit 2 |
| Go toolchain regression | none — no `.go` file modified |

Ports live in `tools/rstools/crates/ashfall-dev/src/{selector,runner,scopedtest}.rs`.

## Stage 1 finding (flagged, not fixed)

`pkg/selector.GetChangedFiles` does `strings.TrimSpace(line)[3:]` on each
`git status --porcelain` line. When the index-status byte is a space — an
unstaged modification (` M`) or deletion (` D`) — `TrimSpace` removes that byte,
so `line[3:]` over-shifts by one and the returned path loses its first
character. Observed live: `WORKTREE_OWNERSHIP.md` → `ORKTREE_OWNERSHIP.md`,
`docs/ci/CI_GATE_MANIFEST.json` → `ocs/ci/CI_GATE_MANIFEST.json`. Consequence:
unstaged edits under-select scoped tests. This port **reproduces the quirk
exactly** for behavioural parity; fixing it changes `bin/run-scoped-tests`
behaviour and needs its own approved change (and should then be fixed in both
the Go original and the Rust port before cutover, or only in Rust once Go is
removed).