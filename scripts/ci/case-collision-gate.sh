#!/usr/bin/env bash
# case-collision-gate.sh — Plan VIII · Task 25.9/25.10.
# Fails when two tracked paths collide case-insensitively (case-aliasing breaks
# Linux checkouts and cross-platform collaborators). The two sanctioned top-level
# trees Assets/ (Unity-era Core+data authority) and assets/ (Godot-native assets)
# are intentionally distinct; the guard detects COLLIDING ALIASES, not the
# sanctioned mixed-case coexistence. Skips Git-LFS pointer files' content concerns —
# path collisions are checked by path only.
set -euo pipefail
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"

# All tracked paths (index, not working tree — works on partial checkouts).
git ls-files -z | python3 -c "
import sys, collections, unicodedata

paths = [p.decode('utf-8', errors='replace') for p in sys.stdin.buffer.read().split(b'\x00') if p]

def fold(p):
    # NFC + casefold: catch the real-world aliasing shapes (Assets vs assets,
    # Foo.png vs foo.PNG), ignoring Unicode normalization drift.
    return unicodedata.normalize('NFC', p).casefold()

by_fold = collections.defaultdict(list)
for p in paths:
    by_fold[fold(p)].append(p)

collisions = []
for key, group in sorted(by_fold.items()):
    if len(group) > 1:
        collisions.append(sorted(group))

if not collisions:
    print(f'CASE_COLLISION_GATE PASS — {len(paths)} tracked paths, no case-folded collisions')
    sys.exit(0)

print(f'CASE_COLLISION_GATE FAIL — {len(collisions)} colliding path group(s):', file=sys.stderr)
for group in collisions:
    for p in group:
        print('  ' + p, file=sys.stderr)
    print(file=sys.stderr)
print('Sanctioned exception? Document it in docs/ci/README.md (case guard) with the intended logical location.', file=sys.stderr)
sys.exit(1)
"
