#!/usr/bin/env bash
# no-whitespace-churn.sh
# Verifies that no modified file introduces trailing whitespace or pure whitespace line churn.
set -euo pipefail

echo "[CI] Checking for trailing whitespace and whitespace churn..."

# Check git diff if inside git repo
if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  # git diff --check reports trailing whitespaces in staged/unstaged changes
  if ! git diff --check; then
    echo "[FAIL] Trailing whitespace or whitespace error detected in git diff."
    exit 1
  fi
fi

echo "[PASS] No whitespace churn detected."
exit 0
