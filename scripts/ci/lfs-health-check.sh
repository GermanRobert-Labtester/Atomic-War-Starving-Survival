#!/usr/bin/env bash
# SPDX-License-Identifier: MIT
# ASHFALL — Git LFS Health & Integrity Checker
#
# Inspects local Git LFS installation, repo configuration, pointer validity,
# and missing binary objects without modifying repository state.
#
# Usage:
#   bash scripts/ci/lfs-health-check.sh
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$ROOT"

echo "── ASHFALL GIT LFS HEALTH CHECK ──"

# 1. Confirm Git LFS is installed
if ! command -v git-lfs >/dev/null 2>&1; then
  echo "❌ FAIL: 'git-lfs' command is not installed or not in PATH."
  echo ""
  echo "Remediation command:"
  echo "  sudo apt-get install -y git-lfs && git lfs install   # Linux / Ubuntu"
  echo "  brew install git-lfs && git lfs install              # macOS"
  echo "  choco install git-lfs && git lfs install             # Windows"
  exit 1
fi

LFS_VER="$(git lfs version | head -n1)"
echo "✅ Git LFS Installed: $LFS_VER"

# 2. Check core.ignorecase configuration
IGNORE_CASE="$(git config --get core.ignorecase || echo "unset")"
if [ "$IGNORE_CASE" = "true" ]; then
  echo "⚠️  WARNING: core.ignorecase is set to true."
  echo "    ASHFALL maintains distinct 'Assets/' (Core/data) and 'assets/' (Godot) trees."
  echo ""
  echo "Remediation command:"
  echo "  git config core.ignorecase false"
else
  echo "✅ Case Sensitivity: core.ignorecase is $IGNORE_CASE"
fi

# 3. Detect tracked LFS files
TRACKED_COUNT="$(git lfs ls-files 2>/dev/null | wc -l || echo "0")"
echo "✅ Tracked LFS Files: $TRACKED_COUNT pointers tracked"

# 4. Check for missing or corrupted LFS objects via non-destructive fsck
echo "Inspecting LFS object storage integrity..."
if ! FSCK_OUT="$(git lfs fsck 2>&1)"; then
  echo "❌ FAIL: Missing or corrupt Git LFS objects detected."
  echo "Details:"
  echo "$FSCK_OUT" | sed 's/^/  /'
  echo ""
  echo "Remediation commands (run to pull missing binaries):"
  echo "  git lfs fetch --all"
  echo "  git lfs pull"
  echo "  git lfs checkout"
  exit 1
fi

echo "✅ Object Integrity: Git LFS fsck passed (zero missing objects)"

# 5. Check for un-hydrated pointer stubs in working tree
STUB_COUNT=0
if [ -d "assets" ]; then
  # Sample check if pointer stubs exist instead of actual media files
  while IFS= read -r f; do
    if [ -f "$f" ] && [ "$(wc -c < "$f")" -lt 300 ]; then
      if grep -q "version https://git-lfs.github.com/spec/v1" "$f" 2>/dev/null; then
        STUB_COUNT=$((STUB_COUNT + 1))
      fi
    fi
  done < <(find assets -type f \( -name "*.png" -o -name "*.ttf" \) 2>/dev/null || true)
fi

if [ "$STUB_COUNT" -gt 0 ]; then
  echo "⚠️  WARNING: Found $STUB_COUNT un-hydrated LFS pointer stubs in assets/."
  echo ""
  echo "Remediation command:"
  echo "  git lfs pull"
  exit 1
fi

echo "✅ Working Tree: All checked asset binaries are fully hydrated"
echo ""
echo "=== GIT LFS HEALTH CHECK PASSED (All LFS checks green) ==="
exit 0
