#!/bin/bash
# ASHFALL — one-command alpha playtest capture.
# Regenerates the first-hour funnel report from live sessions and prints the
# triage block to fill in. Usage: scripts/tools/alpha_playtest_report.sh [out.md]
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1
OUT="${1:-docs/telemetry/FIRST_HOUR_FUNNEL.md}"

python3 scripts/tools/first_hour_funnel.py --discover --out "$OUT" || exit 1

echo
echo "=== Sessions ==="
grep -E "^## Session|^- Events|Live first-hour progress|Canonical funnel" "$OUT" | head -24
echo
echo "=== Drop-off per session ==="
grep -E "^\| [0-9] " "$OUT" | grep "| no |" | head -12
echo
echo "=== Triage block (fill in) ==="
cat <<'EOF'
Session date:
Reached stage: __/7   Max day: __
First drop-off:
   step:            (from the funnel table above)
   what happened:   (hint missing / panel confusing / control unreachable / other)
Fix 1 (system owner):
Fix 2 (system owner):
Fix 3 (system owner):
Notes (comfort / tone / keyboard):
EOF
