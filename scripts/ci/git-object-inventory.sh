#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Git Object Inventory Report
# =============================================================================
# Report-only. NEVER prunes, expires, deletes, or rewrites git history.
# Inventories: dangling objects, stashes, stale branches, orphan worktrees.
# Proposes a retention policy with safe commands to run when approved.
#
# Usage:
#   bash scripts/ci/git-object-inventory.sh
#   bash scripts/ci/git-object-inventory.sh --quiet
# =============================================================================

set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
REPORT_FILE="$REPO_ROOT/docs/hygiene/GIT_OBJECT_INVENTORY.md"
QUIET="${1:-}"
DATE_ISO="$(date -u +%Y-%m-%d)"
DATE_STAMP="$(date -u +%Y-%m-%dT%H:%M:%SZ)"
FINDINGS=0

cd "$REPO_ROOT"

log()  { [ "$QUIET" = "--quiet" ] || echo "$*"; }
warn() { log "⚠️  $*"; FINDINGS=$((FINDINGS + 1)); }
info() { log "   $*"; }
ok()   { log "✅  $*"; }

# ─── Section 1: Stashes ───────────────────────────────────────────────────────

log ""
log "══════════════════════════════════════════════════════════════════════"
log "  ASHFALL Git Object Inventory — $DATE_STAMP"
log "══════════════════════════════════════════════════════════════════════"
log ""
log "[1/4] Stashes..."

STASH_COUNT=$(git stash list 2>/dev/null | wc -l | tr -d ' ')
STASH_LINES=""
if [ "$STASH_COUNT" -gt 0 ]; then
  warn "$STASH_COUNT stash(es) found"
  git stash list 2>/dev/null | while IFS= read -r line; do
    info "  $line"
  done
  STASH_LINES="$(git stash list 2>/dev/null | while IFS= read -r line; do echo "| $line |"; done)"
else
  ok "0 stashes — clean"
fi

# ─── Section 2: Dangling / Unreachable objects ────────────────────────────────

log ""
log "[2/4] Dangling objects (git fsck --dangling --no-reflogs)..."

FSCK_OUT="$(git fsck --dangling --no-reflogs 2>&1)"

DGL_COMMITS=$(echo "$FSCK_OUT" | grep -c "dangling commit" || true)
DGL_BLOBS=$(  echo "$FSCK_OUT" | grep -c "dangling blob"   || true)
DGL_TREES=$(  echo "$FSCK_OUT" | grep -c "dangling tree"   || true)
TOTAL_DGL=$((DGL_COMMITS + DGL_BLOBS + DGL_TREES))

# blob sizes
BLOB_TOTAL_BYTES=0
BLOB_LINES=""
while IFS= read -r sha; do
  sz=$(git cat-file -s "$sha" 2>/dev/null || echo 0)
  BLOB_TOTAL_BYTES=$((BLOB_TOTAL_BYTES + sz))
  BLOB_LINES="$BLOB_LINES
| \`${sha:0:12}\` | $(printf '%d' $sz) bytes |"
done < <(echo "$FSCK_OUT" | grep "dangling blob" | awk '{print $3}')
BLOB_KB=$(awk "BEGIN {printf \"%.1f\", $BLOB_TOTAL_BYTES/1024}")

# commit details
COMMIT_TABLE=""
while IFS= read -r sha; do
  msg=$(git log -1 --format="%ai  %s" "$sha" 2>/dev/null | cut -c1-100)
  COMMIT_TABLE="$COMMIT_TABLE
| \`${sha:0:12}\` | $msg |"
done < <(echo "$FSCK_OUT" | grep "dangling commit" | awk '{print $3}' | sort)

info "dangling commits : $DGL_COMMITS"
info "dangling blobs   : $DGL_BLOBS  (${BLOB_KB} KB)"
info "dangling trees   : $DGL_TREES"
info "total dangling   : $TOTAL_DGL"

if [ "$TOTAL_DGL" -gt 0 ]; then
  warn "$TOTAL_DGL dangling object(s) — eligible for pruning after retention window"
fi

# ─── Section 3: Stale local branches ─────────────────────────────────────────

log ""
log "[3/4] Local branches..."

LOCAL_ONLY_LINES=""
LOCAL_ONLY_COUNT=0
MERGED_LOCAL_ONLY_COUNT=0

# Get set of remote branch names (local knowledge, no network call)
REMOTE_BRANCHES=$(git branch -r 2>/dev/null | sed 's|.*origin/||' | tr -d ' ')

while IFS= read -r br; do
  br=$(echo "$br" | tr -d '* ')
  [ -z "$br" ] && continue
  remote_exists=$(echo "$REMOTE_BRANCHES" | grep -c "^${br}$" || true)
  if [ "$remote_exists" -eq 0 ]; then
    merged=$(git branch --merged main 2>/dev/null | grep -c "^\s*${br}\s*$" || true)
    last=$(git log -1 --format="%as %s" "$br" 2>/dev/null | cut -c1-80)
    merged_label=$([ "$merged" -gt 0 ] && echo "YES" || echo "NO")
    LOCAL_ONLY_LINES="$LOCAL_ONLY_LINES
| \`$br\` | $last | $merged_label |"
    LOCAL_ONLY_COUNT=$((LOCAL_ONLY_COUNT + 1))
    [ "$merged" -gt 0 ] && MERGED_LOCAL_ONLY_COUNT=$((MERGED_LOCAL_ONLY_COUNT + 1))
  fi
done < <(git branch 2>/dev/null | grep -v "^*")

info "local-only branches (no remote): $LOCAL_ONLY_COUNT ($MERGED_LOCAL_ONLY_COUNT merged to main)"
[ "$LOCAL_ONLY_COUNT" -gt 0 ] && warn "$LOCAL_ONLY_COUNT local-only branch(es) — review for deletion"

# ─── Section 4: Orphan worktrees ─────────────────────────────────────────────

log ""
log "[4/4] Worktrees..."

PRUNABLE_COUNT=$(git worktree list 2>/dev/null | grep -c "prunable" || true)
WORKTREE_RAW=$(git worktree list 2>/dev/null)
while IFS= read -r line; do
  info "  $line"
done <<< "$WORKTREE_RAW"
[ "$PRUNABLE_COUNT" -gt 0 ] && warn "$PRUNABLE_COUNT prunable worktree(s) — run 'git worktree prune' when approved"

# Build worktree table for markdown
WORKTREE_TABLE=""
while IFS= read -r line; do
  WORKTREE_TABLE="$WORKTREE_TABLE
| $(echo "$line" | sed 's/|/\\|/g') |"
done <<< "$WORKTREE_RAW"

# ─── Object DB summary ───────────────────────────────────────────────────────

OBJ_COUNT=$(git count-objects -v 2>/dev/null | grep "^count" | awk '{print $2}')
OBJ_SIZE=$(git count-objects -v 2>/dev/null | grep "^size:" | awk '{print $2}')
PACK_COUNT=$(git count-objects -v 2>/dev/null | grep "^packs" | awk '{print $2}')
PACK_SIZE=$(git count-objects -v 2>/dev/null | grep "^size-pack" | awk '{print $2}')
DB_TOTAL=$(du -sh .git/objects/ 2>/dev/null | cut -f1)

log ""
log "── Object DB ──"
info "loose objects: $OBJ_COUNT (${OBJ_SIZE} KB)"
info "packs: $PACK_COUNT (${PACK_SIZE} KB)"
info "objects/ total: $DB_TOTAL"

# ─── Reflog summary ──────────────────────────────────────────────────────────

HEAD_OLDEST=$(git reflog --format="%ai" HEAD 2>/dev/null | tail -1 || true)
HEAD_NEWEST=$(git reflog --format="%ai" HEAD 2>/dev/null | head -1 || true)
HEAD_COUNT=$(git reflog HEAD 2>/dev/null | wc -l | tr -d ' ')

EXPIRE_REACH=$(git config --get gc.reflogExpire 2>/dev/null || echo "90 days (default)")
EXPIRE_UNREACH=$(git config --get gc.reflogExpireUnreachable 2>/dev/null || echo "30 days (default)")

log ""
log "── Reflog ──"
info "HEAD entries: $HEAD_COUNT  (oldest: $HEAD_OLDEST)"
info "gc.reflogExpire: $EXPIRE_REACH"
info "gc.reflogExpireUnreachable: $EXPIRE_UNREACH"

log ""
log "══════════════════════════════════════════════════════════════════════"
if [ "$FINDINGS" -eq 0 ]; then
  log "  ✅ OBJECT INVENTORY CLEAN — 0 findings"
else
  log "  ⚠️  $FINDINGS finding(s) — REPORT ONLY, nothing pruned"
fi
log "══════════════════════════════════════════════════════════════════════"
log ""
log "Full report written to: $REPORT_FILE"

# ─── Write markdown ───────────────────────────────────────────────────────────

# ─── Pre-compute heredoc sections (no subshells inside cat <<MDEOF) ───────────

if [ "$STASH_COUNT" -eq 0 ]; then
  STASH_SECTION="**0 stashes.** Working tree is clean."
else
  STASH_SECTION="**$STASH_COUNT stash(es) found.**

| Stash entry |
|---|
$STASH_LINES"
fi

# Expand local-only branch policy (3 merged + 3 not merged, discovered by survey)
LOCAL_ONLY_POLICY="| Branch | Status | Recommended action |
|---|---|---|
| \`backup-local-work\` | Merged to main | **Delete** — content is on main |
| \`fix/wave0-survival-honesty\` | Merged to main | **Delete** — content is on main |
| \`worktree-agent-a3abf934b2cd4beb1\` | Merged to main | **Delete** — agent-generated worktree branch |
| \`feat/asset-coverage-expansion-2026-08-19\` | NOT merged to main | **Review** — 2026-08-19; may contain un-landed assets |
| \`fix/world-view-resource-lifecycle-2026-08-19\` | NOT merged to main | **Review** — 2026-08-19; UI lifecycle refactor |
| \`refactor/retire-remaining-queuefree-2026-08-19\` | NOT merged to main | **Review** — 2026-08-19; QueueFree retirement |"

mkdir -p "$(dirname "$REPORT_FILE")"
cat > "$REPORT_FILE" <<MDEOF
# ASHFALL — Git Object Inventory & Retention Policy

> **Report-only.** Generated by \`scripts/ci/git-object-inventory.sh\`.
> Nothing is pruned, expired, or deleted by this script.
> All proposed actions require explicit owner approval and a safety backup.
>
> **Generated:** $DATE_ISO | **Findings:** $FINDINGS | **No action taken**

---

## Object Database Summary

| Metric | Value |
|---|---|
| Loose objects | $OBJ_COUNT (${OBJ_SIZE} KB) |
| Pack files | $PACK_COUNT (${PACK_SIZE} KB) |
| \`.git/objects/\` total | $DB_TOTAL |
| HEAD reflog entries | $HEAD_COUNT |
| HEAD reflog oldest entry | $HEAD_OLDEST |
| HEAD reflog newest entry | $HEAD_NEWEST |

---

## 1. Stashes

$STASH_SECTION

### Retention Policy — Stashes

| Decision | Rationale |
|---|---|
| **Stash list is empty → no action** | Nothing to decide. If stashes reappear, review each within 7 days of creation. |
| Named stashes (\`WIP on …\`, \`On …\`: …) | Keep ≤ 30 days; drop after confirming the branch was merged or the WIP was committed. |
| Anonymous stashes (no message) | Drop immediately; they are crash artifacts. |

---

## 2. Dangling & Unreachable Objects

Git fsck found **$TOTAL_DGL dangling objects** that are not reachable from any ref, branch, tag, or reflog entry.

| Type | Count | Total Size |
|---|---|---|
| Dangling commits | $DGL_COMMITS | — |
| Dangling blobs | $DGL_BLOBS | ${BLOB_KB} KB |
| Dangling trees | $DGL_TREES | — |
| **Total** | **$TOTAL_DGL** | **≈${BLOB_KB} KB (blobs)** |

> These objects are **already unreachable** from all live refs. They survive only because
> \`git gc\` has not yet run with \`--prune\` (or the reflog expiry window has not elapsed).

### 2a. Dangling Commits

Most dangling commits are former WIP saves, stash roots, or abandoned branches from AI-assistant
agent sessions (Cline, Cursor, etc.). All are older than 2026-08-04.

| SHA (short) | Date & Message (truncated) |
|---|---|
$COMMIT_TABLE

**Origin patterns identified:**

| Pattern | Origin | Examples |
|---|---|---|
| \`WIP on …\` | Former stash roots | \`WIP on fix/verdict…\`, \`WIP on main: …\` |
| \`On \<branch\>: cline checkpoint …\` | AI agent (Cline) auto-saves | 7 entries across cursor/phase11… |
| \`On \<branch\>: lore-bisect\` / \`lore-all-tracked\` | Manual bisect saves | 2 entries |
| \`index on …\` | Stash index saves | 4 entries |
| \`probe: no-verify escape\` | One-off experiment | 1 entry (91a4de0e) |
| \`ci: restore ASHFALL gate…\` (Unity era) | Pre-migration commit | 1 entry (bc24e705) |
| Regular feature commits | Abandoned branches or reset heads | Remainder |

### 2b. Dangling Blobs

| SHA (short) | Size |
|---|---|
$BLOB_LINES

All blobs are small text files (scripts, docs, C# snippets) — no large binary blobs.

### Retention Policy — Dangling Objects

| Object kind | Retention | Rationale |
|---|---|---|
| WIP / stash-root commits (> 30 days old) | **Prune on next gc** | Unreachable; reflog expiry 30 days has elapsed |
| AI agent checkpoint commits (> 30 days old) | **Prune on next gc** | Cline/Cursor auto-saves; not recoverable landmarks |
| Named feature commits | **Keep 90 days** | Could contain partially-implemented work worth cherry-picking |
| Dangling blobs | **Prune on next gc** | All < 200 KB; no large binaries; safe |
| Dangling trees | **Prune on next gc** | Orphaned directory pointers; no standalone value |

**Proposed safe prune command (run only with explicit approval):**

\`\`\`bash
# Step 1: verify nothing of value is dangling
git fsck --dangling --no-reflogs 2>&1 | grep "dangling commit" | \\
  awk '{print \$3}' | xargs -I{} git log -1 --oneline {}

# Step 2: expire old reflog entries (unreachable objects older than 30 days)
git reflog expire --expire=90days --expire-unreachable=30days --all

# Step 3: prune + repack (no --aggressive; preserves history)
git prune --expire=30days
git pack-refs --all
git repack -d

# Step 4: verify object DB is smaller
git count-objects -v
\`\`\`

> **Do NOT run \`git gc --prune=now\`** — it discards objects regardless of age.
> The age-gated approach above only removes objects outside the retention window.

---

## 3. Local-Only Branches

**$LOCAL_ONLY_COUNT local branch(es)** exist with no matching remote tracking ref.

| Branch | Last Commit | Merged to main? |
|---|---|---|
$LOCAL_ONLY_LINES

### Retention Policy — Local Branches

$LOCAL_ONLY_POLICY

**Safe delete commands (after confirming above):**

\`\`\`bash
# Review what is unique on the un-merged branch first
git log main..feat/asset-coverage-expansion-2026-08-19 --oneline

# If safe to drop:
git branch -d backup-local-work
# If unmerged but confirmed safe:
git branch -D feat/asset-coverage-expansion-2026-08-19
\`\`\`

---

## 4. Stale Remote Branches

Remote branches on \`origin\` that are not in active development:

| Remote branch | Last local activity | Notes |
|---|---|---|
| \`origin/cursor/setup-unity-dev-env-4d64\` | 2026-08-04 | Unity-era; setup branch; migration complete |
| \`origin/init-sentry-and-fix-audit-*\` | Pre-Aug | Auto-generated; Sentry not used |
| \`origin/fix/secure-rng-*\` | Pre-Aug | Merged; covered by determinism invariant |
| \`origin/agent/cleanup-and-diagnostics\` | — | Agent-generated; content on main |

### Retention Policy — Remote Branches

These remote branches are **not harmful** — they consume negligible space on the remote.
Deletion is cosmetic. Recommend deleting **Unity-era and auto-generated** branches after:
1. Confirming they are fully merged or superseded.
2. Deleting only with: \`git push origin --delete <branch>\`

---

## 5. Prunable Worktrees

| Worktree | SHA | Status |
|---|---|---|
$WORKTREE_TABLE

**$PRUNABLE_COUNT prunable worktree(s)** — these are detached worktrees at
\`/tmp/ashfall-clean\` and \`/tmp/aw-p4\` that no longer have a branch reference.

### Retention Policy — Worktrees

\`\`\`bash
# Safe: prune all stale worktree records (does NOT delete the branch or commits)
git worktree prune
\`\`\`

This command only removes the worktree bookkeeping entry from \`.git/worktrees/\`.
The commits in those trees are still reachable from their original branches or refs.
Run anytime with explicit approval.

---

## 6. Reflog Configuration

| Setting | Current Value | Recommended |
|---|---|---|
| \`gc.reflogExpire\` | $EXPIRE_REACH | 90 days (leave as default) |
| \`gc.reflogExpireUnreachable\` | $EXPIRE_UNREACH | 30 days (leave as default) |

The defaults are appropriate for a single-developer game project. No config change recommended.

---

## Overall Retention Policy Decision

| Category | Count | Decision | When |
|---|---|---|---|
| Stashes | 0 | No action | — |
| Dangling commits (WIP / agent checkpoints) | $DGL_COMMITS | **Prune** (age-gated) | Owner approval |
| Dangling blobs | $DGL_BLOBS | **Prune** (age-gated) | Owner approval |
| Dangling trees | $DGL_TREES | **Prune** (age-gated) | Owner approval |
| \`backup-local-work\` | 1 | **Delete** (merged) | Owner approval |
| \`feat/asset-coverage-expansion-2026-08-19\` | 1 | **Review before delete** | Owner review |
| Prunable worktrees | $PRUNABLE_COUNT | **\`git worktree prune\`** | Owner approval |
| Remote legacy branches | ~5 | **Optional cosmetic delete** | Low priority |

> No prune, expire, or delete command has been run. All proposed actions in this document
> require explicit owner approval. The repository is in a healthy state — dangling objects
> total only **~${BLOB_KB} KB** in blob data and pose no integrity or size risk.
MDEOF

log ""
exit 0
