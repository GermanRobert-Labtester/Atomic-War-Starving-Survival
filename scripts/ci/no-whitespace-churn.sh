#!/usr/bin/env bash
# =============================================================================
# no-whitespace-churn.sh
# Verifies that no modified file introduces trailing whitespace or whitespace churn.
# =============================================================================
set -euo pipefail

echo "── Checking for trailing whitespace and whitespace errors ──"

if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  # 1. Check staged changes
  if ! git diff --cached --check; then
    echo "[FAIL] Trailing whitespace detected in staged changes." >&2
    exit 1
  fi

  # 2. Check unstaged working-tree changes
  if ! git diff --check; then
    echo "[FAIL] Trailing whitespace detected in working tree." >&2
    exit 1
  fi

  # 3. If in CI and checking against a base commit, check the commit range
  if [ -n "${GITHUB_BASE_REF:-}" ]; then
    if git rev-parse --verify "origin/${GITHUB_BASE_REF}" >/dev/null 2>&1; then
      if ! git diff "origin/${GITHUB_BASE_REF}...HEAD" --check; then
        echo "[FAIL] Trailing whitespace detected in branch changes against origin/${GITHUB_BASE_REF}." >&2
        exit 1
      fi
    fi
  fi
fi

echo "  -> PASS: No whitespace errors detected."
exit 0
