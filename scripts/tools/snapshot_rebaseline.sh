#!/bin/bash
# ASHFALL — snapshot drift review + rebaseline.
#
# 1. Renders the 32 pinned targets and reports drift.
# 2. With --apply: regenerates the corpus, then re-runs the diff to prove 32/32.
# Requires a renderer session (DISPLAY) — headless captures are suppressed.
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1
APPLY=0
[ "${1:-}" = "--apply" ] && APPLY=1
DISPLAY_VALUE="${DISPLAY:-:0}"

echo "[snapshots] diff run (display $DISPLAY_VALUE)"
DISPLAY="$DISPLAY_VALUE" timeout 900 godot --path . -- --ui-snapshot-uitest > /tmp/snapshot_diff.log 2>&1
grep -E "SUMMARY|DRIFT:" /tmp/snapshot_diff.log | tail -5

if [ "$APPLY" = 1 ]; then
  echo "[snapshots] regenerating corpus"
  DISPLAY="$DISPLAY_VALUE" timeout 900 godot --path . -- --ui-snapshot-regenerate > /tmp/snapshot_regen.log 2>&1
  echo "[snapshots] re-verify"
  DISPLAY="$DISPLAY_VALUE" timeout 900 godot --path . -- --ui-snapshot-uitest 2>&1 | grep -E "SUMMARY" | tail -1
else
  echo "[snapshots] review the drift list above; rerun with --apply to rebaseline"
fi
