#!/usr/bin/env bash
# ASHFALL bounded UI layout check.
#
# Task 15 (survival-legibility fourth wave): a single, repeatable entry point for
# the Godot headless `--ui-layout-selftest`. It routes through the shared bounded
# launcher so the 15 FPS / 180s test-policy cap is enforced and the build-staleness
# guard runs. Use this instead of invoking `godot` directly.
#
# Task 10 (fifth wave): the wrapper now parses the selftest's `Failures:` summary
# itself, so a future runner that exits 0 on a non-zero failure count still fails.
#
#   bash scripts/ci/ui-layout-check.sh
#
# Exit code: 0 when Failures: 0, otherwise non-zero.
set -uo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT"

echo "[ui-layout-check] running bounded --ui-layout-selftest (15 FPS, 180s cap)"

# Task 15 (eighth wave) — record the run start so a stale artifact left by a
# previous run cannot masquerade as this run's verdict.
start_epoch="$(date +%s)"

LOG="$(mktemp)"
trap 'rm -f "$LOG"' EXIT

ASHFALL_EXPECT_ARTIFACT="$ROOT/artifacts/ui-layout-selftest.json" \
    bash "$ROOT/scripts/ci/run-godot-bounded.sh" --headless -- --ui-layout-selftest 2>&1 | tee "$LOG"
runner_status="${PIPESTATUS[0]}"
failures="$(grep -oE 'Failures: [0-9]+' "$LOG" | tail -1 | grep -oE '[0-9]+' || true)"
failures="${failures:-0}"

if [[ "$failures" != "0" ]]; then
    echo "[ui-layout-check] FAIL: selftest reported $failures failure(s)"
    exit 1
fi

if [[ "$runner_status" != "0" ]]; then
    echo "[ui-layout-check] FAIL: runner exited $runner_status with no failure count"
    echo "[ui-layout-check] note: the bounded runner caps at 180s; a timeout is a FAIL"
    exit "$runner_status"
fi

# Task 15 (seventh wave) — verify the machine-readable artifact the selftest
# writes, so a stdout scrape cannot disagree with the recorded verdict.
ARTIFACT="$ROOT/artifacts/ui-layout-selftest.json"
if [[ ! -f "$ARTIFACT" ]]; then
    echo "[ui-layout-check] FAIL: missing artifact $ARTIFACT"
    exit 1
fi
if ! grep -q '"status":"PASS"' "$ARTIFACT"; then
    echo "[ui-layout-check] FAIL: artifact does not report PASS ($ARTIFACT)"
    exit 1
fi

# Task 15 (eighth wave) — reject a stale artifact. A missing write on a passing
# runner would otherwise be masked by the previous run's PASS file.
artifact_epoch="$(stat -c %Y "$ARTIFACT" 2>/dev/null || echo 0)"
if [[ "$artifact_epoch" -lt "$start_epoch" ]]; then
    echo "[ui-layout-check] FAIL: artifact is stale — not written by this run ($ARTIFACT)"
    exit 1
fi

echo "[ui-layout-check] PASS: Failures: 0"
exit 0
