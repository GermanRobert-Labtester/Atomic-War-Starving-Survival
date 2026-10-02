# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

# Language Policy Encoding + Agent-Speed Follow-ups

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **Authority.** User-directed 2026-10-02: adopt the four-language policy
> (**C# primary; Rust secondary; Python for AI/automation/tools/prototyping;
> GDScript for Godot glue**) and **prohibit Go, JS, TS, and all other
> resource-heavy languages**; make Rust the preferred language wherever a tool
> or large system would work better in Rust than C#; "remove and prohibit
> [Go/Python versions of long checks] in AGENTS.md and many .md files … so it's
> hard to avoid". Also: the reference-budget rule is **≤ 30 % over is
> tolerable; > 30 % means investigate and optimize the test**, not auto-extend.
> The user also selected the four follow-ups from the prior batch.

## Bounded outcome

1. **Encode the language policy** in the canonical `AGENTS.md` (which the 13
   client rulebooks symlink to) and `TEST_POLICY.md`; add
   `docs/ci/LANGUAGE_POLICY.md` as the authoritative detail; add an enforcement
   gate `scripts/ci/language-policy-gate.py`.
2. **Budget rule = 30 %**, with the `>30 % → investigate & optimize` semantics.
3. **Follow-up (a):** exclude coordination ledgers (`WORKTREE_OWNERSHIP.md`,
   `INTEGRATION_PLANS.md`) from `docs/INDEX.md` — they are edited by every agent
   every few minutes and make the docs-index gate drift constantly.
4. **Follow-up (b):** wire `timing-budget.py` suspicion into `run-gates.py`
   (non-fatal warning) if that file is not concurrently hot.
5. **Follow-up (c):** the per-file scan cache is superseded by moving long
   checks to Rust; recorded as the approach for `docs_index_check`.
6. **Follow-up (d):** commit the batch.

## Explicitly out of scope this batch

- The Rust port **Stage 2** (validation cluster) is being executed by a
  concurrent delegated agent under `tools/rstools/**`; this plan does not touch
  those paths.
- Deleting `tools/gotools/**`: that is Stage 4 of the port, after parity.
- Rewriting all `.sh` CI glue: shell is prohibited only *as a project
  programming language*; CI/hook glue is a documented exception.

## Language policy (authoritative content)

| Language | Role | Use for |
|---|---|---|
| C# | Primary | All normal gameplay, domain, UI, tests for C# code |
| Rust | Secondary | Tools, long-running/heavy checks, validators, deterministic/CPU-heavy systems, large subsystems that would work better in Rust than C# |
| Python | Tertiary | AI/agent orchestration, automation, tools, prototyping, data/report processing |
| GDScript | Tertiary | Small Godot scene/editor glue only |

**Prohibited:** Go, JavaScript, TypeScript, C, C++, Java, Kotlin, Swift,
Objective-C, Lua, Ruby, PHP, Dart, Zig, Haskell, Perl, VB, and any other
unlisted language — unless the user explicitly changes the policy. Config/data
formats (JSON/YAML/TOML/XML/INI, engine scene/resource formats) are not
programming languages and are allowed. External precompiled dependencies whose
internals are another language are acceptable when no such source is added to
this repo.

**Rust-preference enforcement:** when a component would plausibly work better in
Rust than C# (performance, memory, isolation, determinism, fast startup, or a
long-running check), Rust is the preferred implementation; the new gate and this
policy make that the default expectation rather than an afterthought.

## Files owned by this plan

`AGENTS.md`, `TEST_POLICY.md` (and the symlinked rulebooks, which change through
`AGENTS.md`), new `docs/ci/LANGUAGE_POLICY.md`, new
`scripts/ci/language-policy-gate.py`, `scripts/ci/generate-docs-index.py`,
`docs/INDEX.md`, `docs/ci/TIMING_BASELINE.json`, `scripts/ci/timing-budget.py`,
`docs/ci/TIMING_BUDGET.md`, `scripts/ci/run-gates.py` (only if not hot),
this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`.

**Not owned:** `tools/rstools/**` (delegated Stage 2),
`tools/gotools/**`, `docs/ci/CI_GATE_MANIFEST.json`, `docs/ci/GATE_INVENTORY.md`
(concurrently hot), `.github/**`, all game/Core/data paths.

## Verification

- `language-policy-gate.py` reports only the allowlisted transitional
  `tools/gotools/*.go` and no unexpected prohibited files; fails on a synthetic
  `.js`/`.ts`.
- `generate-docs-index.py --check` passes after the ledger exclusion and
  regeneration; `docs/INDEX.md` contains no `WORKTREE_OWNERSHIP.md` /
  `INTEGRATION_PLANS.md` rows.
- `timing-budget.py` default tolerance is 1.30 and flags strictly `> 1.30`.
- `AGENTS.md` still contains the `## READ THIS FIRST — NON-NEGOTIABLE RULES`
  marker (required by `sync-agents`).
- No full test suite. Focused checks only.

## Results (measured 2026-10-02)

| Check | Result |
|---|---|
| `language-policy-gate.py` on the real repo | **PASS** (only `tools/gotools/*.go` allowlisted) |
| Gate on synthetic `.ts` + `.js` | **FAIL, exit 1**, both flagged; `.rs`/`.py`/allowlisted `.go` ignored |
| `AGENTS.md` + `TEST_POLICY.md` policy | Go-only replaced with the four-language policy + Rust preference + 30 % rule |
| `docs/ci/LANGUAGE_POLICY.md` | new authoritative detail |
| Rulebooks | 13 are symlinks to `AGENTS.md` → updated automatically |
| `timing-budget.py` tolerance | 1.30, boundary `OK`, strictly `>` flagged |
| Ledger exclusion | `WORKTREE_OWNERSHIP.md` refs in `docs/INDEX.md`: 0; docs 4160→4159 |
| `run-gates.py` wiring | `--check-only` PASS (73 gates); hook flags 1.59× as `SUSPICIOUS`, no flag at 0.94× |
| `py_compile` on 4 touched scripts | OK |
| `docs_index_drift --check` | **OK (4159 documents verified)** |

- Go is now explicitly retired in policy; the transitional `tools/gotools` tree is
  allowlisted by the gate and slated for deletion in the Rust port's Stage 4.
- **Stage 2** of the Rust port (validation cluster: `validate-json`,
  `validate-config`, `parse-results`, `scan-saves`, `build-manifest`, `index`) is
  executing under a concurrent delegated agent in `tools/rstools/**`; it is
  verified and integrated separately when it reports.
- Per-file scan caching is superseded: long checks move to Rust (policy), which
  makes a Python cache unnecessary.