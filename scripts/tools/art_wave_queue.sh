#!/bin/bash
# ASHFALL — sequential art-wave queue.
#
# The image provider serves ~1-2 images/minute, so waves must never overlap.
# This runner waits for the currently running portrait wave, then works
# through locations and items in sequence. Every stage is resumable: rerunning
# a category only generates ids whose JPG is not on disk yet.
set -u
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT" || exit 1
TOOL="scripts/tools/generate_faction_portrait_art.py"

echo "[queue] $(date +%H:%M:%S) waiting for the portrait wave to finish..."
while pgrep -f "[g]enerate_faction_portrait_art.py portrait" >/dev/null 2>&1; do
  sleep 30
done
echo "[queue] $(date +%H:%M:%S) portrait wave done; starting locations"

python3 "$TOOL" location --workers 1 --min-interval 40 >> /tmp/artwave_location.log 2>&1
echo "[queue] $(date +%H:%M:%S) locations done; starting items (long stage)"

python3 "$TOOL" item --workers 1 --min-interval 40 >> /tmp/artwave_item.log 2>&1
echo "[queue] $(date +%H:%M:%S) all queued art waves finished"
