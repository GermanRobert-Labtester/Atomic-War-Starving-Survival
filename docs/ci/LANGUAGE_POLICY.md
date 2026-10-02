# ASHFALL Language Policy — Authoritative

**Status:** ACTIVE (user-directed 2026-10-02). Canonical summary lives in
`AGENTS.md` ("TOOLS AND CHANGE HYGIENE"); the 13 client rulebooks are symlinks to
`AGENTS.md`. Enforced by `scripts/ci/language-policy-gate.py`.

## The four permitted languages

| Language | Priority | Use for |
|---|---|---|
| **C#** | Primary | All normal game development: gameplay systems, domain logic, controllers, state machines, UI, save/load coordination, host wiring, and the tests for C# code. |
| **Rust** | Secondary | Tools, long-running/heavy checks, validators, parsers, fuzzers, deterministic or CPU-heavy systems, large subsystems, fast-startup CLIs — **and any component that would genuinely work better in Rust than C#**. |
| **Python** | Tertiary | AI/agent orchestration, automation, tools, prototyping, data/report processing. Tightly scoped, timeout-bounded, never core runtime. |
| **GDScript** | Tertiary | Small Godot scene/editor glue and rapid prototypes only. |

## Rust is preferred over C# where it is genuinely better

When a component would plausibly work better in Rust — performance, memory
behavior, isolation, deterministic execution, reliability, fast startup, or a
long-running check — implement it in **Rust**, not C#. Rust is **not** a default
for ordinary gameplay; C# remains primary for game logic. Do not introduce Rust
merely because it is interesting, and do not add native interop without a
measured reason.

**Long-running checks belong in Rust.** Any indexer, validator, scanner, or
check that takes more than a few seconds must be Rust and optimized (fast
startup, predictable memory) — not left in Python or Go.

## Prohibited languages

Every language not listed above is prohibited unless the user explicitly changes
this policy. This includes, but is not limited to:

Go · JavaScript · TypeScript · C · C++ · Java · Kotlin · Swift · Objective-C ·
Lua · Ruby · PHP · Dart · Zig · Haskell · Perl · Visual Basic · shell as a
project programming language · SQL as application logic · any generated source in
another language · any unapproved embedded scripting language.

**Enforcement:** `scripts/ci/language-policy-gate.py` fails when a prohibited
source file is added. The transitional Go toolchain (`tools/gotools/**`) was
deleted in Stage 4 of the Rust port (`.ai/plans/rust-port-gotools-2026-10-02.md`),
so there is no allowlist — Go is prohibited outright.

## Not programming languages (allowed)

Configuration and data formats required by tools/engines: JSON, YAML, TOML, XML,
INI, and engine-specific scene/resource formats (`.tscn`, `.tres`, `.godot`,
`.import`, `.csproj`, and similar). These are data, not replacement languages.

## Exceptions

- **Shell** may remain for **CI and git-hook glue** (`scripts/**`, `.git/hooks`)
  — orchestration, not application logic. Do not put game or domain logic in
  shell.
- **External precompiled dependencies** whose internals are another language are
  acceptable when no such source is added to this repository and the dependency
  is necessary for an approved C#/Rust/Python/GDScript implementation. Inform the
  user when it materially affects portability, licensing, build time, or
  maintenance.

## Reference-time budget

A stage **≤ 30 % over** its recorded reference is tolerable; **> 30 % means
investigate and optimize the test**, never simply raise its budget. Detail:
`docs/ci/TIMING_BUDGET.md`; baseline: `docs/ci/TIMING_BASELINE.json`; checker:
`scripts/ci/timing-budget.py`.