---
name: ashfall-lfs-gate
description: Gates Git LFS policy, .gitattributes correctness, Assets/ vs assets/ case-sensitivity (core.ignorecase false), and binary hygiene. Use when adding art/audio/fonts, on fresh clone, or before any commit touching assets/.
---

# ASHFALL Git LFS Gate

## ROLE
The repo intentionally keeps two case-distinct trees (`Assets/` Unity legacy + Core, `assets/` Godot-native) and 565 MB of untracked-LFS risk. You prevent `core.ignorecase=true` aliasing and LFS pointer/plain drift.

Authority: `AGENTS.md:REPOSITORY SETUP`, `GIT RULES`, `.gitattributes`, `setup-repo.sh`, `scripts/ci/godot-asset-gate.sh`.

## RULES
1. `setup-repo.sh` pins `core.ignorecase false` — every fresh clone must run it before `git add assets/`.
2. Images/fonts are **Git LFS pointers** (`git lfs ls-files`); `*.wav/*.mp3/*.ogg` stay **plain binary** by `.gitattributes`. Never add large PNG/AI outside LFS.
3. Never hand-edit `.import` / `.meta` for migrated assets.
4. `unity-assets-archive-2026-08-14.tar.gz` (140 MB) must not be re-added to history.

## WORKFLOW
### PHASE 1 — Config Census
- `git config --get core.ignorecase` → must be `false`; if `true`, fail with `setup-repo.sh` remediation.
- `git check-attr filter -- assets/art/foo.png Assets/art/foo.png` confirms case-distinct attrs.
- `cat .gitattributes` — verify `*.png filter=lfs`, `*.ttf filter=lfs`, `*.wav -text` etc.

### PHASE 2 — Pointer Audit
- `git lfs ls-files | wc -l` vs `find assets -name "*.png" -o -name "*.ttf" | wc -l` — drift?
- `git lfs status`, `git lfs track` — any new asset added without LFS that should use it?
- `find . -name "*.wav" -exec git check-attr filter {} \;` — must NOT be `lfs`.

### PHASE 3 — Case Hygiene
- `git status` after `touch assets/test_case.txt Assets/test_case.txt` (dry-run) — both trees distinct?
- `git ls-files | grep -i "^assets/" | sort -f | uniq -Di` — no case-colliding duplicates.

### PHASE 4 — Verify
- `./setup-repo.sh` idempotent 0 exit.
- `git add --dry-run assets/` stages lowercase tree only.
- `./scripts/ci/godot-asset-gate.sh` (import + 48/48 registry) if assets changed.

## OUTPUT
`docs/repo/LFS_GATE_REPORT.md` — ignorecase state, LFS pointer count, .gitattributes table, case-distinct proof, violations with exact `git add` remediation.

## QUALITY GATE
- `core.ignorecase=false`, LFS pointer coverage = image/font count, 0 WAVs under LFS, 0 case collisions, `git lfs ls-files` clean.
