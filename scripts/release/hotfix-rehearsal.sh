#!/usr/bin/env bash
# scripts/release/hotfix-rehearsal.sh — Plan 48 / C2[21] phase-5 rehearsal harness
# =============================================================================
# A hotfix path nobody has rehearsed is a rumour of a route. This builds a
# throwaway git repository that mirrors the three version sources and a
# production save-schema constant, then proves the iron rule both ways:
#
#   1. a docs-only change          -> version-gate --hotfix must PASS
#   2. a production schema bump    -> version-gate --hotfix must FAIL
#   3. the three version sources    -> normal version-gate must PASS
#
# It never touches the caller's worktree, branches, index, or tags: everything
# happens under mktemp and is removed on exit. Pair it with the real-repo
# snapshot proof (scripts/run_test.sh Ashfall.Core.Tests/Save/HotfixRehearsalGateTests.cs)
# for the byte-stable save-schema half.
#
# Usage: bash scripts/release/hotfix-rehearsal.sh
# Exit 0 = rehearsal PASS, 1 = a step failed.

set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
GATE="$REPO_ROOT/scripts/ci/version-gate.py"
TMP="$(mktemp -d -t ashfall_hotfix_rehearsal_XXXXXX)"
cleanup() { rm -rf "$TMP"; }
trap cleanup EXIT

FAIL=0
step() { echo "[rehearsal] $1"; }
report() { if [[ "$1" == "0" ]]; then echo "  PASS: $2"; else echo "  FAIL: $2"; FAIL=1; fi; }

step "building throwaway fixture repo at $TMP"
cd "$TMP"
git init -q
git config user.email rehearsal@ashfall.local
git config user.name "Ashfall Rehearsal"

printf '[application]\nconfig/version="1.1.0"\n' > project.godot
printf '<Project><PropertyGroup><VersionPrefix>1.1.0</VersionPrefix></PropertyGroup></Project>\n' > Directory.Build.props
printf '[preset.0.options]\napplication/file_version="1.1.0"\napplication/product_version="1.1.0"\n' > export_presets.cfg
mkdir -p src/Save
printf 'public static class Store { public const int SchemaVersion = 5; }\n' > src/Save/Store.cs
git add -A
git commit -qm base
git tag v0.0.1

# 1. docs-only change -> iron rule PASS
printf 'rehearsal\n' > README.md
git add -A
git commit -qm docs-only
if python3 "$GATE" --repo-root "$TMP" --hotfix --base-ref v0.0.1 >/dev/null 2>&1; then
    report 0 "docs-only change passes the hotfix iron rule"
else
    report 1 "docs-only change passes the hotfix iron rule"
fi

# 2. schema bump -> iron rule FAIL
printf 'public static class Store { public const int SchemaVersion = 6; }\n' > src/Save/Store.cs
git add -A
git commit -qm schema-bump
if python3 "$GATE" --repo-root "$TMP" --hotfix --base-ref v0.0.1 >/dev/null 2>&1; then
    report 1 "production schema bump is rejected by the hotfix iron rule"
else
    report 0 "production schema bump is rejected by the hotfix iron rule"
fi

# 3. three-source agreement still holds on the fixture
if python3 "$GATE" --repo-root "$TMP" >/dev/null 2>&1; then
    report 0 "three version sources agree"
else
    report 1 "three version sources agree"
fi

if [[ "$FAIL" == "0" ]]; then
    echo "[rehearsal] HOTFIX_REHEARSAL PASS"
    exit 0
fi
echo "[rehearsal] HOTFIX_REHEARSAL FAIL"
exit 1
