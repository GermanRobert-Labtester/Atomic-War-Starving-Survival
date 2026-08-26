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
#   --disease-selftest          → Disease Expansion 25/25
#   --expansions-selftest      → ALL EXPANSIONS GREEN (01–10, incl. Muster/Dose/Verdict/Black Flotilla)
#   --black-flotilla-selftest  → Black Flotilla (Exp 09) vertical slice
#   --radio-selftest           → radio persistence round-trip + tamper rejection
#
# Exit code 0 = all gates green; non-zero = something failed.
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}" )/../.." && pwd)"
cd "$DIR"

fail=0
run() { echo; echo "── $* ──"; "$@"; }

# Repo bootstrap (idempotent): core.ignorecase + LFS filters.
if [[ -f ./setup-repo.sh ]]; then ./setup-repo.sh; fi

# Orphan-sidecar pre-flight (Task 2 follow-up from 2026-08-19 audit addendum).
# Block the gate when art/.jpg or sprites/.png files lack their .jpg.import /
# .png.import siblings — without the sidecar, ResourceLoader.Load returns
# null at runtime even though the file sits on disk. Lane A (d88bd8a3)
# closed ten such orphans; this chore catches any that follow.
if [[ -x ./scripts/ci/asset-orphan-sweep.sh ]]; then
    if ! ./scripts/ci/asset-orphan-sweep.sh; then
        echo "asset-orphan-sweep FAILED — assets/art or assets/sprites have" >&2
        echo "files without their .import sidecar. Run:" >&2
        echo "  godot --headless --no-window --path . --import" >&2
        echo "then re-run ./scripts/ci/asset-orphan-sweep.sh to confirm." >&2
        exit 1
    fi
else
    echo "WARN: scripts/ci/asset-orphan-sweep.sh not executable; skipping" >&2
fi

# One-time / first-clone compile + import (idempotent; no-ops if already done).
run dotnet build Ashfall.csproj || fail=1
if ! godot --headless --path . --import >/tmp/godot-import.log 2>&1; then
    echo "godot --import reported a problem (see /tmp/godot-import.log) — continuing to gates." >&2
fi

for gate in --asset-registry-selftest --data-integrity-selftest --disease-selftest --expansions-selftest --black-flotilla-selftest --radio-selftest; do
    echo; echo "── gate: $gate ──"
    if godot --headless --path . -- "$gate"; then
        echo "GATE PASS: $gate"
    else
        echo "GATE FAIL: $gate" >&2
        fail=1
    fi
done

# ── Isolated Linux export regression gate (P0) ─────────────────────────
# Verifies that a fresh Linux export is self-contained: authoritative Data
# beside the executable + .NET runtime must allow isolated data-integrity.
# Skipped only when export templates are absent (CI without templates).
echo; echo "── gate: isolated-linux-export ──"
TEMPLATE="$HOME/.local/share/godot/export_templates/4.7.1.stable.mono/linux_release.x86_64"
if [[ ! -f "$TEMPLATE" ]]; then
    echo "SKIP: export template $TEMPLATE not found — skipping isolated export gate" >&2
else
    if ! ./scripts/ci/godot-export-linux.sh; then
        echo "GATE FAIL: godot-export-linux.sh" >&2
        fail=1
    else
        echo "GATE PASS: godot-export-linux.sh"
        # Isolated proof: copy only runtime artifacts to /tmp and run self-tests without source Assets/.
        TMP_ISO=$(mktemp -d)
        echo "Isolated export test in $TMP_ISO"
        cp builds/linux/ashfall.x86_64 builds/linux/ashfall.pck "$TMP_ISO/"
        cp -r builds/linux/data_Ashfall_linuxbsd_x86_64 "$TMP_ISO/"
        # Data is now at builds/linux/Assets/StreamingAssets/Data — copy the deployed layout
        if [[ -d builds/linux/Assets/StreamingAssets/Data ]]; then
            mkdir -p "$TMP_ISO/Assets/StreamingAssets"
            cp -r builds/linux/Assets/StreamingAssets/Data "$TMP_ISO/Assets/StreamingAssets/"
        fi
        # Representative catalogs must exist in the deployed copy
        for rep in items.json economy_goods.json; do
            if [[ ! -f "$TMP_ISO/Assets/StreamingAssets/Data/$rep" ]]; then
                echo "GATE FAIL: $TMP_ISO/Assets/StreamingAssets/Data/$rep missing" >&2
                fail=1
            fi
        done
        echo "── isolated: $TMP_ISO/ashfall.x86_64 -- --data-integrity-selftest ──"
        if "$TMP_ISO/ashfall.x86_64" -- --data-integrity-selftest 2>&1 | tee /tmp/isolated-data-integrity.log; then
            if grep -q "DATA_INTEGRITY_SELFTEST PASS" /tmp/isolated-data-integrity.log; then
                echo "GATE PASS: isolated data-integrity-selftest"
            else
                echo "GATE FAIL: isolated data-integrity log missing PASS" >&2
                fail=1
            fi
        else
            # Godot headless may exit 0 even with our error handling; check log
            if grep -q "DATA_INTEGRITY_SELFTEST PASS" /tmp/isolated-data-integrity.log; then
                echo "GATE PASS: isolated data-integrity-selftest (exit code check)"
            else
                echo "GATE FAIL: isolated data-integrity-selftest" >&2
                fail=1
            fi
        fi
        # Also smoke the exported binary for other self-tests
        for iso_gate in --bridge-selftest --survivors-selftest --world-selftest --economy-selftest --asset-registry-selftest; do
            echo "── isolated: $iso_gate ──"
            if "$TMP_ISO/ashfall.x86_64" -- "$iso_gate" >/tmp/isolated-"$(echo $iso_gate | tr -d '-')".log 2>&1; then
                echo "GATE PASS: isolated $iso_gate"
            else
                echo "GATE FAIL: isolated $iso_gate" >&2
                cat /tmp/isolated-"$(echo $iso_gate | tr -d '-')".log | tail -n 20 >&2
                fail=1
            fi
        done
        rm -rf "$TMP_ISO"
    fi
fi

echo
if [[ $fail -eq 0 ]]; then
    echo "godot-asset-gate: ALL GATES GREEN"
else
    echo "godot-asset-gate: FAILED (see above)" >&2
fi
exit $fail
