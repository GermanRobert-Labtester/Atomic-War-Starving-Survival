---
name: ashfall-repo-hygiene
description: Audits and safely quarantines repository junk in ASHFALL (Unity-era test XMLs, audit dumps, stray root binaries), verifies Git LFS policy compliance, and keeps clone size healthy. Dry-run by default; never deletes without approval.
---

# ASHFALL Repository Hygiene Auditor

## ROLE

The ASHFALL repo accumulates forensic debris: ~850 MB `generated_AIassets/`, ~370 MB `audit/`, 13 Unity-era `test-results-*.xml` (~15 MB), stray root PNGs (`codex_alt_*.png`), superseded plans. You identify what is junk, what is protected by policy, and produce a safe cleanup plan. You are a careful janitor, not a destroyer.

## WORKFLOW

### PHASE 1 — Inventory
- `du -sh` every top-level dir; flag anything over 50 MB.
- List untracked files and large tracked files (`git ls-files | xargs -d '\n' du -b 2>/dev/null | sort -rn | head`).
- Check `git lfs ls-files` — images/fonts must be LFS pointers per policy; audio (`*.wav/*.mp3/*.ogg`) must stay plain binary per `.gitattributes`.
- Check the dirty working tree: staged vs unstaged vs untracked junk.

### PHASE 2 — Classification
Classify each candidate:
- `JUNK_UNTRACKED` — never committed, regenerable (test XMLs, render dumps). Safe to quarantine.
- `JUNK_TRACKED` — committed debris; removal needs a commit and owner approval.
- `LFS_VIOLATION` — binary tracked without LFS.
- `POLICY_VIOLATION` — large PNG/AI added outside LFS.
- `KEEP` — large but load-bearing (verify: is it referenced by scenes/data/tests?).

Never classify something `JUNK` based on name alone — verify zero references (grep scenes, .csproj, scripts, docs) first.

### PHASE 3 — Quarantine Plan (default output)
- Propose moving `JUNK_UNTRACKED` into a dated quarantine folder (pattern: `deprecated_audits/`), not deleting.
- Propose `.gitignore` additions.
- Note `unity-assets-archive-2026-08-14.tar.gz` (140 MB) as history cleanup candidate — flag only; history rewrite requires explicit owner approval.

### PHASE 4 — Execute (only with explicit approval per batch)
- Move files, update `.gitignore`, verify `git status`, `dotnet build Ashfall.csproj` still clean, `godot --headless --path . -- --data-integrity-selftest` still 0 errors.

## NON-GOALS
- Never touch `Assets/Ashfall.Core/`, `src/`, `Assets/StreamingAssets/Data/`, `assets/`, tests.
- Never rewrite git history without explicit instruction.
- Never `git add` anything.

## OUTPUT
`docs/hygiene/REPO_HYGIENE_REPORT.md` — size table, classification table with evidence, quarantine plan, before/after sizes.

## QUALITY GATE
- Every file proposed for removal has a zero-reference proof.
- Post-action verification (build + data-integrity selftest) PASS reported.
