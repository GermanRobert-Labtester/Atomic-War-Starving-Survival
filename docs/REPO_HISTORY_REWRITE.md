# REPO_HISTORY_REWRITE.md — Coordinated History Rewrite Runbook (Plan VIII · Task 25)

> **STATUS: PREPARED, NOT EXECUTED.** Every destructive step below requires an
> explicit, agreed freeze window with the project owner. The coding agent must
> stop before §6 on a production remote. Planning, scratch-clone rehearsal (§5),
> and verification are safe to run at any time.

## 1. Scope (current evidence, 2026-09-05)

- `unity-assets-archive-2026-08-14.tar.gz` is already absent from the tip
  (`d03dd555` dropped it) but its blob remains reachable in history
  (`8b6a9e47` added it) — the rewrite purges the historical blob.
- Pack size today: `git count-objects -vH` → **111.09 MiB** size-pack.
  (The AGENTS.md "~565 MB tracked without LFS" figure predates the LFS waves;
  refresh this number before execution.)
- LFS: 3,748 tracked LFS files; `.git/lfs` ≈ 800 MB (per `.gitattributes`
  commentary).

## 2. LFS policy (current `.gitattributes` authority)

`*.png/*.jpg/*.ttf` and **`*.wav/*.mp3/*.ogg/*.aif/*.aiff`** are LFS — the
audio-plain-binary rule in older AGENTS.md text is superseded by the current
`.gitattributes` (audio was migrated to LFS by a later policy change).
Audit before execution: `git lfs ls-files`, `bash scripts/ci/lfs-health-check.sh`
(also wired as a fast-tier gate). Only policy-required extensions may be
migrated in the rewrite window (`git lfs migrate import -- <ext>` scoped).

## 3. Freeze procedure (all required before §6)

1. All active AI streams + human contributors notified and acknowledged.
2. Every stream commits or stashes; `git status --porcelain` clean per stream.
3. Branch freeze agreed (no pushes to any shared branch).
4. Recovery point written down: pre-rewrite SHA of every branch/tag head.
5. Owner explicitly authorizes the window (who, when, duration).

## 4. Backup (before touching anything)

```bash
git bundle create ashfall-pre-rewrite-$(date +%Y%m%d).bundle --all
git tag pre-rewrite-backup-$(date +%Y%m%d) && git push origin pre-rewrite-backup-$(date +%Y%m%d)
# VERIFY: a bundle that has not been restore-tested is not a backup.
git clone ashfall-pre-rewrite-*.bundle /tmp/ashfall-restore-test
cd /tmp/ashfall-restore-test && git log --oneline -1 && git lfs pull
```

## 5. Scratch-clone rehearsal (safe; repeat until clean)

Work in a FRESH clone, never the active working tree; use `git filter-repo`
(never `filter-branch`):

```bash
git clone --mirror <url> ashfall-rewrite.git && cd ashfall-rewrite.git
git filter-repo --invert-paths --path unity-assets-archive-2026-08-14.tar.gz
# optional same-window LFS migration (§2 scope only):
# git lfs migrate import --everything -- include=<scoped-ext>
bash /path/to/checkout/setup-repo.sh && git lfs fetch --all && git lfs checkout
git lfs fsck
git log --all --oneline -- unity-assets-archive-2026-08-14.tar.gz   # must be empty
git count-objects -vH                                               # record delta
# run practical gates in a work-tree of the rewritten clone:
python3 scripts/ci/run-gates.py --tier fast
bash scripts/ci/case-collision-gate.sh
godot --headless --path . -- --data-integrity-selftest
```

Document the rehearsal result (sizes, gate outcomes) in this file before §6.

## 6. Production rewrite — **requires the §3 freeze + verified §4 backup**

1. Re-run §5 against the mirror of origin.
2. Force-push policy: authorized operator only; all branches + tags explicitly
   enumerated; branch protections temporarily adjusted with the owner;
   announcement posted before and after; **never a silent force-push**.
3. Post-push verification on a fresh clone: `./setup-repo.sh && git lfs pull &&
   git lfs fsck`, then `run-gates.py --tier fast` and the export parity per
   `docs/RELEASE_EXPORT.md`.

## 7. Stream re-sync (post-rewrite)

Heavily rewritten history: the safe default is **fresh clone + reapply**.

- Clean stream: `git clone <url>`; re-apply unmerged commits as patches
  (`git format-patch` from the old clone) or cherry-pick onto the new heads.
- Unpushed work: `git format-patch <base>..HEAD` in the old clone, `git am`
  in the new.
- Stashed work: `git stash show -p > patch` → apply in the new clone.
- Archive the old clone (do not delete until the next full gate cycle passes).
- Do NOT blanket-recommend `git pull --rebase` across rewritten history.

## 8. Rollback

Re-clone from the §4 bundle into a scratch mirror and re-point `origin`;
the pre-rewrite tag/bundle is authoritative. Nobody rewrites twice in one
window — abort and reschedule instead.

## 9. Size reporting (post-rewrite)

Record `.git` size, working-tree size, and LFS object size **before and after**
(`git count-objects -vH`, `git lfs ls-files | wc -l`, `du -sh .git .git/lfs`).
The rewrite is not "done" because a path vanished — quantify the win.

## 10. Follow-ups after execution

- Update AGENTS.md Git rules: archive debt resolved (with date), final LFS
  policy pointer (`.gitattributes` is the authority), case-guard command
  (`bash scripts/ci/case-collision-gate.sh`), link this runbook.
- Re-run `scripts/ci/export-build.sh` + parity — the rewrite must not change
  authoritative payload bytes (`--export-parity-selftest` on a fresh artifact).
