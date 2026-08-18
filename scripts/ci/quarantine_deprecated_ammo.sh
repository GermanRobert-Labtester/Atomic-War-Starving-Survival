#!/usr/bin/env bash
set -euo pipefail
# Phase 15.4 quarantine script — DRY-RUN target. Review _phase15_quarantine_plan.json
# before invoking. Run with --apply to actually execute.
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
QUAR="$REPO_ROOT/assets/_quarantine_legacy"
ART="$REPO_ROOT/assets/art"
mkdir -p "$QUAR"
PLAN="docs/visual/_phase15_quarantine_plan.json"
# Eligible moves (no live runtime refs):
# assets/art/ammo_deprecated_.jpg
# assets/art/ammo_deprecated_12ga.jpg
# assets/art/ammo_deprecated_16ga.jpg
# assets/art/ammo_deprecated_300blk.jpg
# assets/art/ammo_deprecated_338lapua.jpg
# assets/art/ammo_deprecated_380acp.jpg
# assets/art/ammo_deprecated_408cheytac.jpg
# assets/art/ammo_deprecated_45acp.jpg
# assets/art/ammo_deprecated_46x30.jpg
# assets/art/ammo_deprecated_50bmg.jpg
# assets/art/ammo_deprecated_545x39.jpg
# assets/art/ammo_deprecated_556x45.jpg
# assets/art/ammo_deprecated_57x28.jpg
# assets/art/ammo_deprecated_762x25.jpg
# assets/art/ammo_deprecated_762x39.jpg
# assets/art/ammo_deprecated_762x51.jpg
# assets/art/ammo_deprecated_762x54r.jpg
# assets/art/ammo_deprecated_765x21.jpg
# assets/art/ammo_deprecated_9x21.jpg
# assets/art/ammo_deprecated_cal_545x39_v2.jpg
# assets/art/ammo_deprecated_unknown.jpg

# Apply only with --apply:
if [ "${1:-dry-run}" = "--apply" ]; then
  if [ -f "assets/art/ammo_deprecated_.jpg" ]; then git mv "assets/art/ammo_deprecated_.jpg" "assets/_quarantine_legacy/ammo_deprecated_.jpg" || mv "assets/art/ammo_deprecated_.jpg" "assets/_quarantine_legacy/ammo_deprecated_.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_.jpg.import" ]; then mv "assets/art/ammo_deprecated_.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_12ga.jpg" ]; then git mv "assets/art/ammo_deprecated_12ga.jpg" "assets/_quarantine_legacy/ammo_deprecated_12ga.jpg" || mv "assets/art/ammo_deprecated_12ga.jpg" "assets/_quarantine_legacy/ammo_deprecated_12ga.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_12ga.jpg.import" ]; then mv "assets/art/ammo_deprecated_12ga.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_12ga.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_16ga.jpg" ]; then git mv "assets/art/ammo_deprecated_16ga.jpg" "assets/_quarantine_legacy/ammo_deprecated_16ga.jpg" || mv "assets/art/ammo_deprecated_16ga.jpg" "assets/_quarantine_legacy/ammo_deprecated_16ga.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_16ga.jpg.import" ]; then mv "assets/art/ammo_deprecated_16ga.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_16ga.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_300blk.jpg" ]; then git mv "assets/art/ammo_deprecated_300blk.jpg" "assets/_quarantine_legacy/ammo_deprecated_300blk.jpg" || mv "assets/art/ammo_deprecated_300blk.jpg" "assets/_quarantine_legacy/ammo_deprecated_300blk.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_300blk.jpg.import" ]; then mv "assets/art/ammo_deprecated_300blk.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_300blk.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_338lapua.jpg" ]; then git mv "assets/art/ammo_deprecated_338lapua.jpg" "assets/_quarantine_legacy/ammo_deprecated_338lapua.jpg" || mv "assets/art/ammo_deprecated_338lapua.jpg" "assets/_quarantine_legacy/ammo_deprecated_338lapua.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_338lapua.jpg.import" ]; then mv "assets/art/ammo_deprecated_338lapua.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_338lapua.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_380acp.jpg" ]; then git mv "assets/art/ammo_deprecated_380acp.jpg" "assets/_quarantine_legacy/ammo_deprecated_380acp.jpg" || mv "assets/art/ammo_deprecated_380acp.jpg" "assets/_quarantine_legacy/ammo_deprecated_380acp.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_380acp.jpg.import" ]; then mv "assets/art/ammo_deprecated_380acp.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_380acp.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_408cheytac.jpg" ]; then git mv "assets/art/ammo_deprecated_408cheytac.jpg" "assets/_quarantine_legacy/ammo_deprecated_408cheytac.jpg" || mv "assets/art/ammo_deprecated_408cheytac.jpg" "assets/_quarantine_legacy/ammo_deprecated_408cheytac.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_408cheytac.jpg.import" ]; then mv "assets/art/ammo_deprecated_408cheytac.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_408cheytac.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_45acp.jpg" ]; then git mv "assets/art/ammo_deprecated_45acp.jpg" "assets/_quarantine_legacy/ammo_deprecated_45acp.jpg" || mv "assets/art/ammo_deprecated_45acp.jpg" "assets/_quarantine_legacy/ammo_deprecated_45acp.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_45acp.jpg.import" ]; then mv "assets/art/ammo_deprecated_45acp.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_45acp.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_46x30.jpg" ]; then git mv "assets/art/ammo_deprecated_46x30.jpg" "assets/_quarantine_legacy/ammo_deprecated_46x30.jpg" || mv "assets/art/ammo_deprecated_46x30.jpg" "assets/_quarantine_legacy/ammo_deprecated_46x30.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_46x30.jpg.import" ]; then mv "assets/art/ammo_deprecated_46x30.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_46x30.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_50bmg.jpg" ]; then git mv "assets/art/ammo_deprecated_50bmg.jpg" "assets/_quarantine_legacy/ammo_deprecated_50bmg.jpg" || mv "assets/art/ammo_deprecated_50bmg.jpg" "assets/_quarantine_legacy/ammo_deprecated_50bmg.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_50bmg.jpg.import" ]; then mv "assets/art/ammo_deprecated_50bmg.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_50bmg.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_545x39.jpg" ]; then git mv "assets/art/ammo_deprecated_545x39.jpg" "assets/_quarantine_legacy/ammo_deprecated_545x39.jpg" || mv "assets/art/ammo_deprecated_545x39.jpg" "assets/_quarantine_legacy/ammo_deprecated_545x39.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_545x39.jpg.import" ]; then mv "assets/art/ammo_deprecated_545x39.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_545x39.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_556x45.jpg" ]; then git mv "assets/art/ammo_deprecated_556x45.jpg" "assets/_quarantine_legacy/ammo_deprecated_556x45.jpg" || mv "assets/art/ammo_deprecated_556x45.jpg" "assets/_quarantine_legacy/ammo_deprecated_556x45.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_556x45.jpg.import" ]; then mv "assets/art/ammo_deprecated_556x45.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_556x45.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_57x28.jpg" ]; then git mv "assets/art/ammo_deprecated_57x28.jpg" "assets/_quarantine_legacy/ammo_deprecated_57x28.jpg" || mv "assets/art/ammo_deprecated_57x28.jpg" "assets/_quarantine_legacy/ammo_deprecated_57x28.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_57x28.jpg.import" ]; then mv "assets/art/ammo_deprecated_57x28.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_57x28.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_762x25.jpg" ]; then git mv "assets/art/ammo_deprecated_762x25.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x25.jpg" || mv "assets/art/ammo_deprecated_762x25.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x25.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_762x25.jpg.import" ]; then mv "assets/art/ammo_deprecated_762x25.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_762x25.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_762x39.jpg" ]; then git mv "assets/art/ammo_deprecated_762x39.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x39.jpg" || mv "assets/art/ammo_deprecated_762x39.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x39.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_762x39.jpg.import" ]; then mv "assets/art/ammo_deprecated_762x39.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_762x39.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_762x51.jpg" ]; then git mv "assets/art/ammo_deprecated_762x51.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x51.jpg" || mv "assets/art/ammo_deprecated_762x51.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x51.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_762x51.jpg.import" ]; then mv "assets/art/ammo_deprecated_762x51.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_762x51.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_762x54r.jpg" ]; then git mv "assets/art/ammo_deprecated_762x54r.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x54r.jpg" || mv "assets/art/ammo_deprecated_762x54r.jpg" "assets/_quarantine_legacy/ammo_deprecated_762x54r.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_762x54r.jpg.import" ]; then mv "assets/art/ammo_deprecated_762x54r.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_762x54r.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_765x21.jpg" ]; then git mv "assets/art/ammo_deprecated_765x21.jpg" "assets/_quarantine_legacy/ammo_deprecated_765x21.jpg" || mv "assets/art/ammo_deprecated_765x21.jpg" "assets/_quarantine_legacy/ammo_deprecated_765x21.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_765x21.jpg.import" ]; then mv "assets/art/ammo_deprecated_765x21.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_765x21.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_9x21.jpg" ]; then git mv "assets/art/ammo_deprecated_9x21.jpg" "assets/_quarantine_legacy/ammo_deprecated_9x21.jpg" || mv "assets/art/ammo_deprecated_9x21.jpg" "assets/_quarantine_legacy/ammo_deprecated_9x21.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_9x21.jpg.import" ]; then mv "assets/art/ammo_deprecated_9x21.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_9x21.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_cal_545x39_v2.jpg" ]; then git mv "assets/art/ammo_deprecated_cal_545x39_v2.jpg" "assets/_quarantine_legacy/ammo_deprecated_cal_545x39_v2.jpg" || mv "assets/art/ammo_deprecated_cal_545x39_v2.jpg" "assets/_quarantine_legacy/ammo_deprecated_cal_545x39_v2.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_cal_545x39_v2.jpg.import" ]; then mv "assets/art/ammo_deprecated_cal_545x39_v2.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_cal_545x39_v2.jpg.import" || true; fi
  if [ -f "assets/art/ammo_deprecated_unknown.jpg" ]; then git mv "assets/art/ammo_deprecated_unknown.jpg" "assets/_quarantine_legacy/ammo_deprecated_unknown.jpg" || mv "assets/art/ammo_deprecated_unknown.jpg" "assets/_quarantine_legacy/ammo_deprecated_unknown.jpg"; fi
  if [ -f "assets/art/ammo_deprecated_unknown.jpg.import" ]; then mv "assets/art/ammo_deprecated_unknown.jpg.import" "assets/_quarantine_legacy/ammo_deprecated_unknown.jpg.import" || true; fi
else
  echo "[quarantine] DRY-RUN. invoke with --apply to execute."
fi