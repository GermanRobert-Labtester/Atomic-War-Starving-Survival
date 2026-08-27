#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# ASHFALL — Lightweight C# License Header Consistency Checker
#
# Inspects added/changed C# source files (*.cs) for accepted SPDX license headers.
# Standard header:
#   // SPDX-License-Identifier: MIT
#
# Usage:
#   bash scripts/ci/license-header-check.sh          # warning-only (exit 0)
#   bash scripts/ci/license-header-check.sh --strict # blocking mode (exit 1 on missing header)
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT"

STRICT=0
if [ "${1:-}" = "--strict" ]; then
  STRICT=1
elif [ "${1:-}" = "--help" ] || [ "${1:-}" = "-h" ]; then
  echo "Usage: $0 [--strict]"
  echo "  Checks changed/staged C# files for '// SPDX-License-Identifier: MIT'"
  exit 0
fi

echo "── ASHFALL C# LICENSE HEADER CONSISTENCY CHECK ──"

# Identify added/modified C# files against git working tree and staged index
FILES=()
if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  # Collect unstaged + staged modified/added C# files
  while IFS= read -r f; do
    [ -n "$f" ] && [ -f "$f" ] && FILES+=("$f")
  done < <(git diff --name-only --diff-filter=ACMR HEAD 2>/dev/null | grep '\.cs$' || git diff --cached --name-only --diff-filter=ACMR 2>/dev/null | grep '\.cs$' || true)
fi

# Fallback: if no git diff or clean working tree, test recent test files or report clean
if [ ${#FILES[@]} -eq 0 ]; then
  echo "No modified/added C# files detected in git working tree."
  echo "=== LICENSE HEADER CHECK PASSED (0 files to check) ==="
  exit 0
fi

MISSING_HEADERS=()
for file in "${FILES[@]}"; do
  # Check first 5 lines for SPDX-License-Identifier: MIT
  if ! head -n 5 "$file" | grep -q "SPDX-License-Identifier:\s*MIT"; then
    MISSING_HEADERS+=("$file")
  fi
done

if [ ${#MISSING_HEADERS[@]} -gt 0 ]; then
  echo "⚠️  WARNING: ${#MISSING_HEADERS[@]} changed C# file(s) missing '// SPDX-License-Identifier: MIT':"
  for f in "${MISSING_HEADERS[@]}"; do
    echo "    - $f"
  done
  echo ""
  echo "Suggested fix: add '// SPDX-License-Identifier: MIT' to the top of the file."
  if [ "$STRICT" -eq 1 ]; then
    echo "❌ FAIL: License header check failed in strict/blocking mode."
    exit 1
  else
    echo "ℹ️  NOTICE: Running in warning-only mode. Exiting 0."
    exit 0
  fi
fi

echo "✅ All ${#FILES[@]} changed C# file(s) contain valid SPDX license headers."
echo "=== LICENSE HEADER CHECK PASSED ==="
exit 0
