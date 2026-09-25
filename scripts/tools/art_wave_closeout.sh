#!/bin/bash
# ASHFALL — art-wave close-out watcher.
#
# Waits for the sequential wave queue to finish (portraits → locations → items),
# then imports the new art, refreshes the asset registry, runs the registry
# self-test and writes docs/ui/ART_WAVE_CLOSEOUT_2026-09-25.md with the final
# numbers. Runs in the background; safe to leave for hours.
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1
LOG="/tmp/artwave_closeout.log"
REPORT="docs/ui/ART_WAVE_CLOSEOUT_2026-09-25.md"

say() { echo "[closeout] $(date +%H:%M:%S) $*" >>"$LOG"; }

say "waiting for the wave queue to finish..."
while pgrep -f "[a]rt_wave_queue.sh" >/dev/null 2>&1; do
  sleep 60
done
say "queue finished; trimming texture imports + importing art"
python3 scripts/tools/trim_texture_imports.py >>"$LOG" 2>&1
timeout 3600 godot --headless --path . --import >>"$LOG" 2>&1
IMPORT_ERRORS=$(grep -cE "^ERROR" "$LOG" || true)

say "refreshing asset registry"
python3 scripts/ci/generate-asset-registry.py > /tmp/artwave_registry.txt 2>&1

say "running asset registry self-test"
timeout 900 godot --headless --path . -- --asset-registry-selftest > /tmp/artwave_selftest.txt 2>&1
SELFTEST_LINE=$(grep -E "ASSET_REGISTRY_SELFTEST: checked" /tmp/artwave_selftest.txt | tail -1)
EMBLEMS_LINE=$(grep -E "Faction emblems:" /tmp/artwave_selftest.txt | tail -1)

say "running snapshot drift check"
DISPLAY="${DISPLAY:-:0}" timeout 900 godot --path . -- --ui-snapshot-uitest > /tmp/artwave_snapshot.txt 2>&1
SNAPSHOT_LINE=$(grep -E "UI_SNAPSHOT_UITEST SUMMARY" /tmp/artwave_snapshot.txt | tail -1)

COUNTS=$(python3 - <<'PY'
import json
e = json.load(open('artifacts/asset_registry.json'))
rows = e.get('entries', e)
out = []
overall = {'loaded': 0, 'fallback': 0, 'missing': 0}
for cat in ('faction', 'portrait', 'location', 'item'):
    c = [x for x in rows if x.get('category') == cat]
    loaded = sum(1 for x in c if x.get('status') == 'LOADED')
    out.append(f"| {cat} | {loaded} | {len(c)} |")
    for x in c:
        s = x.get('status')
        overall['loaded' if s == 'LOADED' else ('missing' if s == 'MISSING' else 'fallback')] += 1
print('\n'.join(out))
print(f"TOTALS loaded={overall['loaded']} fallback={overall['fallback']} missing={overall['missing']}")
PY
)

FACTIONS=$(echo "$COUNTS" | grep "^| faction" | awk -F'|' '{print $3"/"$4}' | tr -d ' ')
PORTRAITS=$(echo "$COUNTS" | grep "^| portrait" | awk -F'|' '{print $3"/"$4}' | tr -d ' ')
LOCATIONS=$(echo "$COUNTS" | grep "^| location" | awk -F'|' '{print $3"/"$4}' | tr -d ' ')
ITEMS=$(echo "$COUNTS" | grep "^| item" | awk -F'|' '{print $3"/"$4}' | tr -d ' ')
TOTALS=$(echo "$COUNTS" | grep "^TOTALS")

{
  echo "# Art Wave Close-out — 2026-09-25"
  echo
  echo "Automated by \`scripts/tools/art_wave_closeout.sh\` after the sequential art queue"
  echo "(portraits → locations → items) finished."
  echo
  echo "## Registry"
  echo
  echo "| Category | Loaded | Total |"
  echo "|---|---|---|"
  echo "$COUNTS" | grep "^|"
  echo
  echo "$TOTALS"
  echo
  echo "## Gates"
  echo
  echo "- Import errors in log: $IMPORT_ERRORS"
  echo "- $SELFTEST_LINE"
  echo "- $EMBLEMS_LINE"
  echo "- $SNAPSHOT_LINE"
  echo
  echo "## Notes"
  echo
  echo "- Rendered JPGs live at \`assets/art/<id>.jpg\` (runtime probe path), plus"
  echo "  \`assets/ui/Icons/faction_icon_<stem>.png\` for factions (the emblem loader's convention)."
  echo "- Snapshot corpus was not rebaselined here; run \`--ui-snapshot-uitest\` on a display,"
  echo "  review art drift, then \`--ui-snapshot-regenerate\` if the new art is correct."
} > "$REPORT"

say "close-out report written: $REPORT"

# Final artifact verification on the frozen art corpus (exports are racy while
# a wave runs, so this is the first stable window).
say "running package verify"
if bash scripts/tools/ashfall-package-verify.sh --skip-build >>"$LOG" 2>&1; then
  say "package verify PASS"
  bash scripts/tools/ashfall-release-zip.sh >>"$LOG" 2>&1 && say "release zips regenerated"
else
  say "package verify FAIL — see $LOG"
fi

say "done"
