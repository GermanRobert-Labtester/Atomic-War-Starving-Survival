#!/bin/bash
# ASHFALL Godot-side deep audit battery — IDENTIFICATION ONLY (no fixes).
# Each loop runs all checks and appends findings to a running master list.
cd "$(dirname "$0")/.."

MASTER=/tmp/ashfall_audit_master.txt
: > "$MASTER"
LOOPS=$1
LOOPS=${LOOPS:-20}

for L in $(seq 1 $LOOPS); do
  {
    echo "########## LOOP $L ##########"
    echo "--- build ---"
    dotnet build Ashfall.csproj -v:q 2>&1 | grep -E "error CS" | sort -u
    echo "--- core tests ---"
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj 2>&1 | grep -E "Failed|Passed:" | tail -1
    echo "--- NotImplemented in src/Ashfall.Core (non-bridge) ---"
    grep -rn "NotImplementedException" src Assets/Ashfall.Core --include="*.cs" | grep -v "BridgeGap.cs"
    echo "--- empty catch ---"
    grep -rn "catch\s*([^)]*)\s*{}" src Assets/Ashfall.Core --include="*.cs"
    echo "--- capture without restore ---"
    for f in $(grep -rln "CaptureState\b" Assets/Ashfall.Core --include="*.cs" | grep -v "/obj/\|Tests\|HeadlessDemo"); do
      [ "$(grep -c 'RestoreState' "$f")" -eq 0 ] && echo "  NO-RESTORE: $f"
    done
    echo "--- hardcoded id literals in src/ not in JSON/core ---"
    grep -rhoE '"[a-z]+_[a-z][a-z0-9_]+"' src --include="*.cs" | sort -u | while read id; do
      c=$(echo "$id" | tr -d '"')
      case "$c" in
        font_*|margin_*|separation|font_color|font_size|background_color|default_color|ashfall_*|anchor_broadcast_|item_seen_|location_visited_|survivor_met_|event_fired_|trait_*|records_clerk|veterinary_assistant|harbour_night_clerk) ;;
        *) grep -rq "$c" Assets/StreamingAssets/Data/*.json 2>/dev/null || grep -rq "\"$c\"" Assets/Ashfall.Core --include="*.cs" 2>/dev/null || echo "  UNKNOWN-ID: $c";;
      esac
    done
    echo "--- obj/ bin/ leaks in git ---"
    git status --short | grep -E "obj/|bin/|\.godot/" | head -5
    echo "--- selftest sweep ---"
    for t in expansions-selftest year-of-ash-save-selftest duty-roster-save-selftest expansion-hub-save-selftest journal-selftest holdfast-save-selftest bridge-selftest; do
      r=$(timeout 40 godot --headless --path . -- --$t 2>&1 | grep -iE "FAIL|PASS$|result:" | tail -1)
      echo "  $t: $(echo "$r" | grep -qi fail && echo FAIL || echo pass)"
    done
  } >> "$MASTER" 2>&1
done

echo "AUDIT COMPLETE — $LOOPS loops. Master list: $MASTER"
echo "Unique compile errors: $(grep -c 'error CS' $MASTER)"
echo "Unique test-fail lines: $(grep -cE '^  Failed ' $MASTER)"
