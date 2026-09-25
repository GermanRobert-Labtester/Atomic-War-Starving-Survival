#!/usr/bin/env bash
# =============================================================================
# no-whitespace-churn.sh
# Verifies that no modified file introduces trailing whitespace or whitespace churn.
#
# Scope (WHITESPACE_SCOPE):
#   all     (default) staged + working tree + CI base diff; legacy behaviour.
#   staged  only the staged change set — the pre-commit contract.
#   branch  only the commit range BASE...HEAD, where BASE is WHITESPACE_BASE
#           (default origin/main, falling back to v1.1.0 then HEAD~1).
#           This is the release-captain scope: it judges the release diff, not
#           every agent's uncommitted work in a shared workspace.
# =============================================================================
set -euo pipefail

SCOPE="${WHITESPACE_SCOPE:-all}"

echo "── Checking for trailing whitespace and whitespace errors (scope: ${SCOPE}) ──"

if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  if [ "$SCOPE" = "all" ] || [ "$SCOPE" = "staged" ]; then
    # 1. Check staged changes
    if ! git diff --cached --check; then
      echo "[FAIL] Trailing whitespace detected in staged changes." >&2
      exit 1
    fi
  fi

  if [ "$SCOPE" = "all" ]; then
    # 2. Check unstaged working-tree changes
    if ! git diff --check; then
      echo "[FAIL] Trailing whitespace detected in working tree." >&2
      exit 1
    fi
  fi

  if [ "$SCOPE" = "branch" ]; then
    BASE="${WHITESPACE_BASE:-origin/main}"
    if ! git rev-parse --verify "$BASE" >/dev/null 2>&1; then
      for candidate in v1.1.0 v1.0.0 HEAD~1; do
        if git rev-parse --verify "$candidate" >/dev/null 2>&1; then BASE="$candidate"; break; fi
      done
    fi
    if git rev-parse --verify "$BASE" >/dev/null 2>&1; then
      echo "  -> comparing against ${BASE}"
      if ! git diff "${BASE}...HEAD" --check; then
        echo "[FAIL] Trailing whitespace detected in branch changes against ${BASE}." >&2
        exit 1
      fi
    else
      echo "  -> no comparison base found; branch scope is a no-op"
    fi
  fi

  # 3. If in CI and checking against a base commit, check the commit range
  if [ "$SCOPE" = "all" ] && [ -n "${GITHUB_BASE_REF:-}" ]; then
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
