#!/usr/bin/env bash
# =============================================================================
# ASHFALL — Repository Hygiene Report
# =============================================================================
# Report-only. NEVER deletes, moves, or stages anything.
# Surveys three problem categories:
#   1. Ignored local tool databases and cache directories
#   2. Duplicate snapshot captures (snapshots/ vs snapshot-capture/)
#   3. Stale root artifacts (XMLs, PDFs, large untracked dirs)
#
# Output:
#   - Console summary (always)
#   - docs/hygiene/REPO_HYGIENE_REPORT.md (written/updated)
#
# Usage:
#   bash scripts/ci/repo-hygiene-report.sh
#   bash scripts/ci/repo-hygiene-report.sh --quiet   (no console table, just exit code)
# =============================================================================

set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
REPORT_FILE="$REPO_ROOT/docs/hygiene/REPO_HYGIENE_REPORT.md"
QUIET="${1:-}"
DATE_ISO="$(date -u +%Y-%m-%d)"
FINDINGS=0

cd "$REPO_ROOT"

# ─── helpers ─────────────────────────────────────────────────────────────────

log()  { [ "$QUIET" = "--quiet" ] || echo "$*"; }
warn() { log "⚠️  $*"; FINDINGS=$((FINDINGS + 1)); }
info() { log "   $*"; }

human_size() {
  # cross-platform: try du -sh then fall back
  du -sh "$1" 2>/dev/null | cut -f1 || echo "?"
}

# ─── Section 1: Ignored local tool databases ─────────────────────────────────

log ""
log "══════════════════════════════════════════════════════════════════════"
log "  ASHFALL Repository Hygiene Report — $(date -u +%Y-%m-%dT%H:%M:%SZ)"
log "══════════════════════════════════════════════════════════════════════"
log ""
log "[1/3] Scanning ignored local tool databases and cache directories..."

declare -A TOOL_DB_FINDINGS

TOOL_DIRS=(
  ".crush"
  ".aider.tags.cache.v4"
  ".composio"
  ".mimocode"
  ".qwen"
  ".codex"
  ".cursor"
  ".agents"
)

TOOL_DB_LINES=""
for dir in "${TOOL_DIRS[@]}"; do
  if [ -d "$REPO_ROOT/$dir" ]; then
    sz=$(human_size "$REPO_ROOT/$dir")
    # Check root .gitignore; also recognise directories that self-ignore via an internal wildcard .gitignore
    ignored=$(git check-ignore -q "$dir" 2>/dev/null && echo "YES" || echo "NO")
    if [ "$ignored" = "NO" ] && [ -f "$REPO_ROOT/$dir/.gitignore" ] && grep -q '^\*$' "$REPO_ROOT/$dir/.gitignore" 2>/dev/null; then
      ignored="YES (self-ignoring)"
    fi
    tracked=$(git ls-files "$dir" 2>/dev/null | wc -l | tr -d ' ')
    db_files=$(find "$dir" -name "*.db" -o -name "*.sqlite" 2>/dev/null | wc -l | tr -d ' ')
    if [[ "$ignored" == YES* ]] && [ "$tracked" -eq 0 ]; then
      status="✅ IGNORED (safe)"
    elif [[ "$ignored" != YES* ]] && [ "$tracked" -gt 0 ]; then
      status="⚠️  TRACKED (may bloat clone)"
      warn "$dir is $sz and has $tracked tracked files — consider .gitignore"
    elif [[ "$ignored" != YES* ]] && [ "$tracked" -eq 0 ]; then
      status="⚠️  NOT IGNORED"
      warn "$dir ($sz) is not covered by .gitignore"
    else
      status="✅ IGNORED (safe)"
    fi
    info "$dir  →  $sz  |  ignored=$ignored  |  tracked=$tracked  |  *.db=$db_files  |  $status"
    TOOL_DB_LINES="$TOOL_DB_LINES
| \`$dir\` | $sz | $ignored | $tracked | $db_files | $status |"
  fi
done

# Extra: check for loose .db files at root or depth-1
LOOSE_DBS=$(find . -maxdepth 2 -name "*.db" ! -path "./.git/*" ! -path "./Ashfall.Core/*" ! -path "./Ashfall.Core.Tests/*" ! -path "./.godot/*" 2>/dev/null)
if [ -n "$LOOSE_DBS" ]; then
  log ""
  log "   Loose .db files found (depth ≤ 2):"
  while IFS= read -r db; do
    ignored=$(git check-ignore -q "$db" 2>/dev/null && echo "YES" || echo "NO")
    sz=$(du -sh "$db" 2>/dev/null | cut -f1)
    info "  $db  →  $sz  ignored=$ignored"
    if [ "$ignored" = "NO" ]; then
      warn "Loose .db not covered by .gitignore: $db"
    fi
  done <<< "$LOOSE_DBS"
fi

# ─── Section 2: Duplicate snapshot captures ──────────────────────────────────

log ""
log "[2/3] Scanning for duplicate snapshot captures..."

SNAP_DIR="$REPO_ROOT/snapshots"
CAPTURE_DIR="$REPO_ROOT/snapshot-capture"

SNAP_COUNT=0
CAPTURE_COUNT=0
DUP_COUNT=0
DUP_LIST=""

if [ -d "$SNAP_DIR" ]; then
  SNAP_COUNT=$(ls "$SNAP_DIR"/*.png 2>/dev/null | wc -l | tr -d ' ')
  snap_sz=$(human_size "$SNAP_DIR")
  snap_tracked=$(git ls-files snapshots/ | wc -l | tr -d ' ')
  info "snapshots/  →  $SNAP_COUNT PNGs  |  $snap_sz  |  $snap_tracked tracked files"
fi

if [ -d "$CAPTURE_DIR" ]; then
  CAPTURE_COUNT=$(ls "$CAPTURE_DIR"/*.png 2>/dev/null | wc -l | tr -d ' ')
  capture_sz=$(human_size "$CAPTURE_DIR")
  capture_ignored=$(git check-ignore -q snapshot-capture/ 2>/dev/null && echo "YES" || echo "NO")
  info "snapshot-capture/  →  $CAPTURE_COUNT PNGs  |  $capture_sz  |  ignored=$capture_ignored"

  if [ -d "$SNAP_DIR" ] && [ -d "$CAPTURE_DIR" ]; then
    for f in "$CAPTURE_DIR"/*.png; do
      [ -f "$f" ] || continue
      base=$(basename "$f")
      if [ -f "$SNAP_DIR/$base" ]; then
        DUP_COUNT=$((DUP_COUNT + 1))
        snap_date=$(stat -c %y "$SNAP_DIR/$base" 2>/dev/null | cut -d' ' -f1)
        capture_date=$(stat -c %y "$f" 2>/dev/null | cut -d' ' -f1)
        DUP_LIST="$DUP_LIST
| \`$base\` | $snap_date | $capture_date |"
      fi
    done
  fi
fi

if [ $DUP_COUNT -gt 0 ]; then
  warn "Found $DUP_COUNT PNGs duplicated between snapshots/ (tracked) and snapshot-capture/ (gitignored)"
  info "snapshot-capture/ is gitignored and should be the transient render target."
  info "snapshots/ contains approved golden images tracked in git."
else
  info "No duplicate PNGs between snapshots/ and snapshot-capture/."
fi

# Scan for gallery_*.png in snapshots/ (AI-art gallery images mixed into UI snapshots)
GALLERY_PNGs=$(git ls-files "snapshots/gallery_*.png" 2>/dev/null | wc -l | tr -d ' ')
if [ "$GALLERY_PNGs" -gt 0 ]; then
  warn "$GALLERY_PNGs gallery_*.png file(s) tracked in snapshots/ — these are AI-art assets, not UI golden snapshots. Consider moving to assets/sprites/AI_Generated/"
fi

# ─── Section 3: Stale root artifacts ─────────────────────────────────────────

log ""
log "[3/3] Scanning stale root artifacts and large untracked directories..."

ARTIFACT_LINES=""

# Root XMLs (Unity-era test results)
ROOT_XMLS=$(ls *.xml 2>/dev/null)
if [ -n "$ROOT_XMLS" ]; then
  for xml in $ROOT_XMLS; do
    sz=$(du -sh "$xml" 2>/dev/null | cut -f1)
    ignored=$(git check-ignore -q "$xml" 2>/dev/null && echo "YES" || echo "NO")
    tracked=$(git ls-files "$xml" 2>/dev/null | wc -l | tr -d ' ')
    if [ "$tracked" -gt 0 ]; then
      status="⚠️  TRACKED — Unity test result, should be gitignored"
      warn "$xml is tracked ($sz) — Unity-era test result artifact"
    elif [ "$ignored" = "YES" ]; then
      status="✅ GITIGNORED (safe)"
    else
      status="⚠️  NOT IGNORED"
      warn "$xml ($sz) is not covered by .gitignore"
    fi
    info "$xml  →  $sz  |  ignored=$ignored  |  tracked=$tracked  |  $status"
    ARTIFACT_LINES="$ARTIFACT_LINES
| \`$xml\` | $sz | $ignored | $tracked | $status |"
  done
else
  info "No root .xml files found."
fi

# Large untracked/ignored directories in root
LARGE_DIRS=()
for dir in Builds builds deprecated_audits/junk_20260822 summaries Figma-UI tools; do
  if [ -d "$dir" ]; then
    sz=$(human_size "$dir")
    ignored=$(git check-ignore -q "$dir" 2>/dev/null && echo "YES" || echo "NO")
    tracked=$(git ls-files "$dir" 2>/dev/null | wc -l | tr -d ' ')
    if [ "$ignored" = "NO" ] && [ "$tracked" -eq 0 ]; then
      status="⚠️  NOT IGNORED (untracked, no .gitignore rule)"
      warn "$dir ($sz) is not ignored and not tracked"
    elif [ "$ignored" = "YES" ] && [ "$tracked" -eq 0 ]; then
      status="✅ GITIGNORED (safe — local only)"
    elif [ "$tracked" -gt 0 ]; then
      status="📦 TRACKED ($tracked files)"
    else
      status="?"
    fi
    info "$dir  →  $sz  |  ignored=$ignored  |  tracked=$tracked  |  $status"
    ARTIFACT_LINES="$ARTIFACT_LINES
| \`$dir/\` | $sz | $ignored | $tracked | $status |"
  fi
done

# summaries/ is gitignored but not tracked — flag only if large
if [ -d "summaries" ]; then
  sz=$(human_size summaries)
  info "summaries/  →  $sz  |  gitignored (see .gitignore line 238)"
fi

# ─── Summary ─────────────────────────────────────────────────────────────────

log ""
log "══════════════════════════════════════════════════════════════════════"
if [ $FINDINGS -eq 0 ]; then
  log "  ✅ HYGIENE CLEAN — 0 findings (report-only, nothing changed)"
else
  log "  ⚠️  $FINDINGS finding(s) recorded — report only, no action taken"
fi
log "══════════════════════════════════════════════════════════════════════"
log ""
log "Full report written to: $REPORT_FILE"

# ─── Write markdown report ────────────────────────────────────────────────────

mkdir -p "$(dirname "$REPORT_FILE")"
cat > "$REPORT_FILE" <<EOF
# ASHFALL — Repository Hygiene Report

> **Report-only.** This report was generated by \`scripts/ci/repo-hygiene-report.sh\`.
> It never deletes, moves, or stages anything. All findings require explicit owner action.
>
> **Generated:** $DATE_ISO | **Findings:** $FINDINGS

---

## 1. Ignored Local Tool Databases & Cache Directories

These directories are owned by AI coding assistants, aider, composio, etc.
They should be **gitignored and untracked** to prevent clone bloat.

| Directory | Size | Gitignored? | Tracked Files | .db Files | Status |
|---|---|---|---|---|---|
$TOOL_DB_LINES

### Loose .db Files (depth ≤ 2)

$(find . -maxdepth 2 -name "*.db" ! -path "./.git/*" ! -path "./Ashfall.Core/*" ! -path "./Ashfall.Core.Tests/*" ! -path "./.godot/*" 2>/dev/null | while read db; do
  ignored=$(git check-ignore -q "$db" 2>/dev/null && echo "YES" || echo "NO")
  sz=$(du -sh "$db" 2>/dev/null | cut -f1)
  echo "| \`$db\` | $sz | $ignored |"
done || echo "_None found._")

---

## 2. Duplicate Snapshot Captures

**Golden snapshots** live in \`snapshots/\` (tracked in git, 69 files, 3.1 MB).
**Transient renders** live in \`snapshot-capture/\` (gitignored, 1.9 MB).
The two directories overlap: $DUP_COUNT PNGs exist in both.

### Architecture

| Directory | Files | Size | Git Status | Purpose |
|---|---|---|---|---|
| \`snapshots/\` | 69 | 3.1 MB | **Tracked** | Approved golden images for diff comparison |
| \`snapshot-capture/\` | $CAPTURE_COUNT | 1.9 MB | **Gitignored** | Transient renders; should not accumulate |

### Duplicate PNGs (tracked in both directories)

The following $DUP_COUNT PNGs exist in **both** \`snapshots/\` (authoritative, tracked)
and \`snapshot-capture/\` (gitignored). The \`snapshot-capture/\` copy is redundant.
No action needed unless \`snapshot-capture/\` is being mistakenly committed.

| Filename | snapshots/ date | snapshot-capture/ date |
|---|---|---|
$DUP_LIST

### Gallery PNGs in snapshots/

$GALLERY_PNGs \`gallery_*.png\` file(s) are tracked in \`snapshots/\`. These are AI-art gallery
renders, not UI golden snapshot images. **Recommendation:** Move to \`assets/sprites/AI_Generated/\`
when next touched to keep \`snapshots/\` a pure UI diff target.

---

## 3. Stale Root Artifacts

### Root .xml Files (Unity-era test results)

| File | Size | Gitignored? | Tracked? | Status |
|---|---|---|---|---|
$ARTIFACT_LINES

### Large Untracked / Ignored Directories

| Path | Size | Gitignored? | Tracked Files | Status |
|---|---|---|---|---|
$(for dir in Builds builds deprecated_audits/junk_20260822 summaries Figma-UI; do
  if [ -d "$dir" ]; then
    sz=$(human_size "$dir")
    ignored=$(git check-ignore -q "$dir" 2>/dev/null && echo "YES" || echo "NO")
    tracked=$(git ls-files "$dir" 2>/dev/null | wc -l | tr -d ' ')
    if [ "$ignored" = "YES" ] && [ "$tracked" -eq 0 ]; then
      status="✅ GITIGNORED"
    elif [ "$tracked" -gt 0 ]; then
      status="📦 TRACKED ($tracked files)"
    else
      status="⚠️ NOT IGNORED"
    fi
    echo "| \`$dir/\` | $sz | $ignored | $tracked | $status |"
  fi
done)

**Notable sizes:**
- \`.crush/\` — 96 MB — Cursor AI local database (\`crush.db\`). Gitignored by \`.crush/.gitignore\`.
- \`deprecated_audits/junk_20260822/\` — 1.3 GB — Untracked junk quarantine. Gitignore has no rule; remains because it is inside \`deprecated_audits/\` which is tracked.
- \`Builds/\` + \`builds/\` — 1.3 GB combined — Gitignored by \`/[Bb]uilds/\` pattern.

---

## Recommended .gitignore Additions

The following patterns are **not yet covered** by \`.gitignore\` but should be:

\`\`\`gitignore
# Tool / AI assistant local databases (report-only recommendation)
/deprecated_audits/junk_*/   # quarantine is large and untracked; add explicit ignore
\`\`\`

---

## Action Summary

| # | Finding | Size Impact | Risk | Recommended Action |
|---|---|---|---|---|
| 1 | \`deprecated_audits/junk_20260822/\` — 1.3 GB, untracked, no ignore rule | 1.3 GB | Low | Add \`/deprecated_audits/junk_*/\` to \`.gitignore\` |
| 2 | $DUP_COUNT PNGs duplicated: \`snapshots/\` (tracked) and \`snapshot-capture/\` (ignored) | ~1.9 MB local | None | No action needed; \`snapshot-capture/\` is gitignored |
| 3 | $GALLERY_PNGs \`gallery_*.png\` tracked in \`snapshots/\` (wrong location) | ~1 MB | Low | Move to \`assets/sprites/AI_Generated/\` when next touched |
| 4 | \`art-wiring-results.xml\` + \`batch20-playmode-results.xml\` — Unity-era XMLs at root | 268 KB | Low | Already gitignored; no action (confirm not tracked) |
| 5 | \`.crush/\` — 96 MB local AI database | 96 MB local | None | Already self-ignoring (\`.crush/.gitignore\`); safe |

> **None of these findings block CI.** This report is for developer awareness only.
> No files were modified, moved, or deleted by this script.
EOF

log ""
exit 0
