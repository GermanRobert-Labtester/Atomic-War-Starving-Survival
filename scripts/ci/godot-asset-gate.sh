#!/usr/bin/env bash
# godot-asset-gate.sh — verify a (fresh or existing) checkout can build + load
# all catalog-referenced Godot assets AND passes the required headless gates.
#
# On a brand-new clone this performs the one-time steps AGENTS.md requires:
#   dotnet build Ashfall.csproj     (compile the Godot .NET host)
#   godot --headless --path . --import   (build the .godot/ texture import cache)
# then runs the headless selftests:
#   --asset-registry-selftest  → every catalog id must resolve to a real asset (48/48)
#   --data-integrity-selftest  → 0 errors across the JSON catalogs
#   --bridge-selftest          → 0 semantic shim gaps
#   --disease-selftest          → Disease Expansion 25/25
#   --expansions-selftest      → ALL EXPANSIONS GREEN
#
# Exit code 0 = all gates green; non-zero = something failed.
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}" )/../.." && pwd)"
cd "$DIR"

fail=0
run() { echo; echo "── $* ──"; "$@"; }

# Repo bootstrap (idempotent): core.ignorecase + LFS filters.
if [[ -f ./setup-repo.sh ]]; then ./setup-repo.sh; fi

# One-time / first-clone compile + import (idempotent; no-ops if already done).
run dotnet build Ashfall.csproj || fail=1
if ! godot --headless --path . --import >/tmp/godot-import.log 2>&1; then
    echo "godot --import reported a problem (see /tmp/godot-import.log) — continuing to gates." >&2
fi

for gate in --asset-registry-selftest --data-integrity-selftest --bridge-selftest --disease-selftest --expansions-selftest; do
    echo; echo "── gate: $gate ──"
    if godot --headless --path . -- "$gate"; then
        echo "GATE PASS: $gate"
    else
        echo "GATE FAIL: $gate" >&2
        fail=1
    fi
done

echo
if [[ $fail -eq 0 ]]; then
    echo "godot-asset-gate: ALL GATES GREEN"
else
    echo "godot-asset-gate: FAILED (see above)" >&2
fi
exit $fail
