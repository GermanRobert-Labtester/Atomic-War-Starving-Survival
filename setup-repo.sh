#!/usr/bin/env bash
# setup-repo.sh — bootstrap this repository on every machine / fresh clone.
#
# The repo deliberately keeps TWO distinct trees that differ only by case:
#   Assets/   (uppercase)  → the Unity legacy tree (Ashfall.Core, StreamingAssets/Data, _Game)
#   assets/   (lowercase)  → the Godot-native asset tree (art/audio/ui/sprites/fonts)
# Git's core.ignorecase (default TRUE on macOS/Windows) aliases the two paths,
# which breaks `git add assets/` (it wrongly stages the uppercase tree). This
# script pins the required settings so the dual-tree layout stays intact.
#
# Idempotent: safe to re-run. Run once after every clone.
set -euo pipefail

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$DIR"

echo "[setup-repo] ASHFALL — repository bootstrap"

# 1. Case-sensitivity: keep Assets/ and assets/ distinct (see header).
git config core.ignorecase false
echo "[setup-repo] core.ignorecase=false  (dual Assets/ + assets/ trees stay distinct)"

# 2. Git LFS filter drivers (images/fonts are LFS-pointers; audio stays plain).
git lfs install --local 2>/dev/null || git lfs install || echo "[setup-repo] WARN: git-lfs not installed — run \`git lfs install\`"; \
  which git-lfs >/dev/null 2>&1 || echo "[setup-repo] WARN: git-lfs binary not found on PATH; install from https://git-lfs.com"

echo "[setup-repo] done. Verify binaries from a fresh checkout with:"
echo "    dotnet build Ashfall.csproj"
echo "    godot --headless --path . --import          # one-time texture import (.godot/ is gitignored)"
echo "    godot --headless --path . -- --asset-registry-selftest"
