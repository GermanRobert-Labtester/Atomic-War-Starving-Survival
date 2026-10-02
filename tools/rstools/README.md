# tools/rstools — ASHFALL dev tool suite (Rust)

Rust home of the ASHFALL development/CI tool suite, replacing the Go module at
`tools/gotools/` under the staged port plan
`.ai/plans/rust-port-gotools-2026-10-02.md` (user-authorized, 2026-10-02).

The binary is named `ashfall-dev` and reproduces the `tools/gotools` CLI contract
so CI gates, the pre-commit hook, and `bin/run-scoped-tests` can be cut over in
Stage 4 without changing callers.

## Layout

```
tools/rstools/
  Cargo.toml                       # workspace
  crates/ashfall-dev/
    Cargo.toml
    src/main.rs                    # CLI dispatch
    src/selector.rs                # port of pkg/selector
    src/runner.rs                  # port of pkg/runner
    src/scopedtest.rs              # port of pkg/scopedtest
    src/validator.rs               # port of pkg/validator      (validate-json)
    src/config.rs                  # port of pkg/config         (validate-config)
    src/parser.rs                  # port of pkg/parser         (parse-results)
    src/scanner.rs                 # port of pkg/scanner        (scan-saves)
    src/manifest.rs                # port of pkg/manifest       (build-manifest)
    src/indexer.rs                 # port of pkg/indexer        (index)
    src/checkplan.rs               # port of pkg/checkplan      (check-plan)
    src/agentsync.rs               # port of pkg/agentsync      (sync-agents)
    src/catalogaudit.rs            # port of pkg/catalogaudit   (audit-catalogs)
    src/releasepolicy.rs           # port of pkg/releasepolicy  (releasepolicy)
    src/monitor.rs                 # port of pkg/monitor        (monitor-size, monitor-compile)
    src/proxy.rs                   # port of pkg/proxy          (llm-proxy, CLI stub)
    src/orchestrator.rs            # port of pkg/orchestrator   (agent-core, CLI stub)
    src/gowalk.rs                  # filepath.WalkDir order parity
    src/gotime.rs                  # time.Time JSON (RFC3339Nano) parity
    src/jsonout.rs                 # encoding/json HTML-escaping + Go float parity
```

## Stages

| Stage | Status | Surface |
|---|---|---|
| 1 | **implemented** | `select-tests`, `run-tasks`, `run-scoped-tests` |
| 2 | **implemented** | `validate-json`, `validate-config`, `parse-results`, `scan-saves`, `build-manifest`, `index` (except the resident mode) |
| 3 | **implemented** (resident servers excepted) | `check-plan`, `sync-agents`, `audit-catalogs`, `releasepolicy`, `monitor-size`, `monitor-compile`, `llm-proxy`*, `agent-core`* |
| 4 | pending | cutover + removal of `tools/gotools/` |

\* `llm-proxy` and `agent-core` are resident HTTP servers that no gate invokes;
a faithful port needs an async HTTP stack the workspace deliberately does not
carry. They exit `2` with an explicit "not supported in the Rust port yet
(Stage 3)" message naming `tools/gotools`, exactly as Stage 2 does for the
resident indexer. `index --watch`/`--serve` remain unsupported for the same
reason. (No server process is left running.)

Unknown subcommands now match Go: `Unknown command: <cmd>` on stderr, the usage
text, exit `1`.

## Build & test

```bash
cargo build --release --manifest-path tools/rstools/Cargo.toml
cargo test  --manifest-path tools/rstools/Cargo.toml
```

Unix-only (peak RSS is read from `wait4(2)`'s `rusage`, exactly as the Go
original reads `ProcessState.SysUsage().Maxrss`; local-zone offsets come from
`localtime_r`).

### Dependencies added in Stage 2

`regex` (Go `regexp` parity), `serde_yaml` (Go `goccy/go-yaml`),
`sha2` (Go `crypto/sha256`), and `jsonschema` with
`default-features = false` — the default features pull `reqwest` + TLS for HTTP
`$ref` resolution, which the Go original never performs. Directory walking uses
`gowalk.rs` rather than `walkdir`, so no ordering crate is needed.

## Stage 2 deltas (deliberate, documented)

Parity was measured against a Go binary built from `tools/gotools` on the live
worktree. `validate-json --json`, `parse-results`, `scan-saves`, and the
`index`/`build-manifest` JSON payloads matched byte-for-byte after normalising
only the fields that embed wall-clock time or a scheduler-dependent ordering.

- **Ordering.** Go's `filepath.WalkDir` sorts each directory and descends
  immediately; the `walkdir` crate yields raw `readdir` order. `gowalk.rs`
  reimplements Go's order so `scan-saves` (single-threaded in Go) and every
  other walk are comparable. Where Go itself is non-deterministic (`index`,
  `build-manifest` and `validate-json` fan out over goroutines), the port is
  deterministic in lexical order instead.
- **JSON text.** Go's `encoding/json` escapes `<`, `>`, `&`, U+2028, U+2029 in
  strings; `serde_json` does not. `jsonout.rs` applies the same escaping so
  payloads containing those characters still match.
- **Timestamps.** `gotime.rs` renders `time.Time` as `RFC3339Nano` (trailing
  nanosecond zeros trimmed, `Z` or the value's own offset). `mod_time` in
  `index --json` uses the local zone via `localtime_r`, matching Go exactly
  (`+03:00` on this host); it falls back to UTC on musl/non-Unix targets.
- **PNG dimensions.** Go registers only `image/png`, and `image.DecodeConfig`
  sniffs content rather than the extension. `manifest.rs` parses the IHDR chunk
  directly, with the same validity checks (signature, IHDR length and CRC-32,
  bit depth, colour type, methods, non-zero size).
- **`validate-config` failure text.** Go uses `santhosh-tekuri/jsonschema/v6`,
  this port uses the `jsonschema` crate. Exit codes and the
  `- <instance-pointer>: <message>` line structure are identical and the same
  JSON pointers are reported; the human-readable wording of a violation differs.
  The valid path (`OK`, exit 0) is byte-identical.
- **`validate-config` base URI.** The schema file's own `file:` URL is the base
  URI (percent-encoded, because this repository path contains a space), so
  relative `$ref`s resolve from disk like Go compiling an absolute path.
- **`index --watch` / `index --serve`.** Not ported: they keep a resident
  process (fsnotify watcher + HTTP API) alive and are not used by any gate. The
  CLI prints
  `[ashfall-dev] index --watch/--serve is not supported in the Rust port yet (Stage 2); use tools/gotools for the resident indexer.`
  and exits `2` rather than pretending to serve. One-shot `index` is complete.
- **`validate-json` read/syntax errors.** The `rule` and exit code match Go; the
  OS error text and `serde_json`'s syntax-error position message differ from
  Go's `os`/`encoding/json` wording.

## Stage 3 deltas (deliberate, documented)

Parity was measured against a Go binary built from `tools/gotools` on the live
worktree. No new crates were needed: `serde_json`/`serde_yaml`/`regex` (already
present from Stage 2) cover the whole cluster, and Go's `filepath.Match` is
reimplemented in `catalogaudit.rs` rather than pulling a glob crate with
different semantics.

- **Byte-identical outright.** `audit-catalogs` (all of `--json`, the human
  report, `--check`, `--summary`, `--list-checks`, `--dump-ids`,
  `--dump-duplicates`, `--list-advisories`, `--strict-stale`/`--fail-on-advisory`
  exit codes), `check-plan` (staged, explicit-file, and `git status --porcelain`
  paths), `sync-agents --check`, `monitor-size`, and `monitor-compile` matched
  `diff`-clean on the real repository. `monitor-size` and `monitor-compile`
  reports are byte-identical (no wall-clock field).
- **Normalised only.** `releasepolicy`'s report matches after normalising
  `generated_at` (wall-clock `time.RFC3339`) and `commit` (`git rev-parse HEAD`);
  both were in fact equal run-to-run except `generated_at`. The PASS/FAIL stdout
  line is byte-identical.
- **Go float rendering.** `monitor` violations carry a Go `float64`
  `confidence`; `encoding/json` writes `1.0` as `1`, so `jsonout::GoFloat`
  reproduces that (`0.8`/`0.9` already round-trip).
- **Ordering.** Where Go iterates a `map` (`sync-agents`' `TargetClients`, the
  HEAD blob map in `monitor-size`, `map[string]interface{}` documents in
  `catalogaudit`), the port is deterministic instead: sorted `sync-agents`
  clients, sorted new-Markdown paths, and `serde_json`'s stable key order. Every
  *ordered* `catalogaudit` output is `sortFindings`/`sort.Strings`-normalised
  before emission, so the payloads still match byte-for-byte. `sync-agents`'
  `Updated` line order and the generated report's bullet order differ from Go's
  per-run-random map order (contents are identical); `--check` output does not.
- **`monitor-compile` MSBuild authority.** `dotnet msbuild <proj>
  -getItem:Compile -nologo` is still the sole compile-set authority (globs,
  `Link`, `Remove`, `Exclude` all applied by MSBuild). `-getItem` JSON uses
  PascalCase keys (`Items`/`Compile`/`FullPath`), which the port matches exactly.
- **`check-plan` porcelain quirk.** The Go `git status --porcelain` branch does
  `strings.TrimSpace(line)[3:]`, which over-shifts on an unstaged ` M` status
  (the same quirk Stage 1 flagged in `pkg/selector`). The port reproduces it
  exactly for behavioural parity.
- **Resident servers.** `llm-proxy` and `agent-core` exit `2` with an explicit
  unsupported notice naming `tools/gotools`; see the Stages note above.
- **`catalogaudit` parse-error text.** Go reports the `encoding/json` /
  `os` error string; the port reports `serde_json` / `std::io` text. The
  `parse_error` finding's `file` and the `--check` exit code match.
- **`releasepolicy` YAML.** `serde_yaml` replaces `goccy/go-yaml` for the
  `jobs.*` subset the check reads; unknown top-level keys (including the YAML
  1.1 `on:` literal) are ignored, as in Go.

## Stage 1 parity notes

- `select-tests --json` reproduces Go's `json.MarshalIndent` shape, including
  `null` for nil slices and identical explanation strings.
- The Go status-line parser trims the whole line before slicing at index 3, which
  mangles the path when the index status byte is a space (` M`). This port
  reproduces that quirk exactly for behavioural parity; it is flagged, not
  silently fixed.
- `run-tasks --json` matches the payload consumed by `scripts/ci/run-gates.py`:
  `timeout` in nanoseconds, and `id`/`exit_code`/`duration_seconds`/`output`
  plus `peak_rss_kb`, optional `error`, `timed_out`.