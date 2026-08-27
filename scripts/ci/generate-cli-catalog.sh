#!/usr/bin/env bash
# ASHFALL CLI catalog generator — docs/cli/HOST_CLI_COMMAND_CATALOG.md
#
# Regenerates the host CLI command catalog from the live `--host-help`
# output so the documentation can never drift from the implementation.
# Source of truth: HostCli.PrintHelp (src/Host/HostCli.cs and partials).
#
# Usage:
#   bash scripts/ci/generate-cli-catalog.sh           # regenerate the doc
#   bash scripts/ci/generate-cli-catalog.sh --check   # CI gate: fail on drift
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
DOC="$ROOT/docs/cli/HOST_CLI_COMMAND_CATALOG.md"
CHECK=0
if [ "${1:-}" = "--check" ]; then
  CHECK=1
elif [ $# -gt 0 ]; then
  echo "Usage: $0 [--check]" >&2
  exit 2
fi

RAW="$(mktemp)"
trap 'rm -f "$RAW"' EXIT

cd "$ROOT"
if ! godot --headless --path . -- --host-help >"$RAW" 2>&1; then
  echo "ERROR: --host-help failed to run:" >&2
  cat "$RAW" >&2
  exit 1
fi

python3 - "$RAW" "$DOC" "$CHECK" <<'PY'
import datetime
import re
import sys

raw_path, doc_path, check = sys.argv[1], sys.argv[2], sys.argv[3] == "1"

# Collect flag lines from the help output. Flag lines start (after optional
# leading whitespace) with '--'; everything else is engine banner / init noise.
entries = []
for line in open(raw_path, encoding="utf-8"):
    stripped = line.strip()
    if not stripped.startswith("--") or stripped.startswith("---"):
        continue
    tokens = stripped.split()
    # Leading flag tokens (long '--flag', short '-f', separated by '/') are the
    # flag + its aliases; the first token that is neither is the description.
    def is_flag(tok):
        return tok.startswith("--") or (len(tok) >= 2 and tok[0] == "-" and tok[1] != "-")
    flags = []
    i = 0
    while i < len(tokens) and (is_flag(tokens[i]) or tokens[i] == "/"):
        if tokens[i] != "/":
            flags.append(tokens[i])
        i += 1
    if not flags:
        continue
    desc = " ".join(tokens[i:])
    entries.append((flags[0], flags[1:], desc))

if not entries:
    print("ERROR: no flag lines found in --host-help output", file=sys.stderr)
    sys.exit(1)

total_tokens = sum(1 + len(aliases) for _, aliases, _ in entries)

def esc(text: str) -> str:
    return text.replace("|", "\\|")

today = datetime.date.today().isoformat()
verified_date = today

if check:
    try:
        current = open(doc_path, encoding="utf-8").read()
        date_match = re.search(r"\*\*Last Verified:\*\*\s+(\d{4}-\d{2}-\d{2})", current)
        if date_match:
            verified_date = date_match.group(1)
    except FileNotFoundError:
        current = None

lines = [
    "# ASHFALL — Host CLI Command Catalog",
    "",
    f"**Last Verified:** {verified_date}  ",
    f"**Total Registered Actions:** {len(entries)} entries / {total_tokens} flag tokens (aliases included)",
    "",
    "> **GENERATED FILE — do not edit by hand.**",
    "> Source of truth: the live `godot --headless --path . -- --host-help`",
    "> output (`HostCli.PrintHelp` in `src/Host/HostCli.cs` and its partials).",
    "> Owning runner code for each verb lives under `src/` (grep the flag name).",
    ">",
    "> Regenerate: `bash scripts/ci/generate-cli-catalog.sh`",
    "> Drift gate: `bash scripts/ci/generate-cli-catalog.sh --check` (fails on drift)",
    "",
    "| Primary Flag | Aliases | Description |",
    "|---|---|---|",
]
for primary, aliases, desc in entries:
    alias_cell = ", ".join(f"`{a}`" for a in aliases) if aliases else "—"
    lines.append(f"| `{primary}` | {alias_cell} | {esc(desc)} |")
lines.append("")

content = "\n".join(lines)

if check:
    if current != content:
        print("FAIL: docs/cli/HOST_CLI_COMMAND_CATALOG.md is out of date")
        print("      relative to the live --host-help output.")
        print("Fix:  bash scripts/ci/generate-cli-catalog.sh && commit the result")
        sys.exit(1)
    print(
        f"OK: CLI catalog in sync with --host-help "
        f"({len(entries)} entries / {total_tokens} flag tokens, Last Verified: {verified_date})"
    )
else:
    with open(doc_path, "w", encoding="utf-8") as fh:
        fh.write(content)
    print(
        f"Wrote {doc_path} ({len(entries)} entries / {total_tokens} flag tokens, Last Verified: {today})"
    )
PY
