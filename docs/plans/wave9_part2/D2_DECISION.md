# Wave 9 Part 2 D2 — F1/F9 Repository Governance Decision

## Decision ID

`WAVE9-PART2-D2-GOVERNANCE-F1-F9`

## Header

- **Source blocker:** `POTENTIALCLUTTER.md` findings F1 (AI-tool workspaces tracked inconsistently with `.gitignore` policy) and F9 (Skill catalog drift between `.agents/skills/` and `.qwen/skills/`).
- **Current HEAD facts:**
  - `repo-hygiene-report.sh` flags tracked tool directories:
    - `.qwen/`: 62 tracked files (1.3 MB)
    - `.codex/`: 57 tracked files (1.3 MB)
    - `.cursor/`: 59 tracked files (1.3 MB)
    - `.agents/`: 36 tracked files (348 KB)
  - Other AI-tool workspaces (`.crush`, `.composio`, `.mimocode`, `.aider`, `.commandcode`, `.mistral`) are gitignored and untracked.
  - `.agents/skills/` contains 35 clean, canonical skills used by all project documentation and rulebooks (`AGENTS.md`, `AI_AGENT_WORKFLOW.md`).
  - `.qwen/skills/` contains 57 skills + `skills.json` (unmaintained mirror from earlier iterations with drift/orphans).
- **Mandatory Safety Rule:** NO DISK DELETION. Tools and user configurations must never be wiped via `rm -rf`. Any untracking must use `git rm --cached` only.

---

## Why a Decision is Required

1. **F1:** The `.gitignore` policy excludes AI-tool workspaces, yet historical commits tracked `.qwen/`, `.codex/`, and `.cursor/` (which mirror IDE-specific settings and duplicate skills). We must establish a clear per-directory policy: which directories belong to repository version control, and which should be untracked.
2. **F9:** Having two parallel skill trees (`.agents/skills/` and `.qwen/skills/`) causes agent confusion and drift. We must designate the single canonical source of truth and determine whether `.qwen/skills/` is untracked or maintained.

---

## Per-Directory Analysis & Options

| Directory | Current Tracked Count | Purpose | Shared Asset or Tool-Local? | Proposed Policy |
|---|---|---|---|---|
| `.agents/` | 36 files | Canonical shared skills & workflows | **Shared project asset** | **KEEP TRACKED (Canonical)** |
| `.qwen/` | 62 files | Legacy Qwen-specific mirror & settings | **Tool-local workspace / legacy mirror** | **UNTRACK via `git rm --cached` + gitignore** |
| `.codex/` | 57 files | Codex-specific mirror | **Tool-local workspace** | **UNTRACK via `git rm --cached` + gitignore** |
| `.cursor/` | 59 files | Cursor-specific settings & mirror | **Tool-local workspace** | **UNTRACK via `git rm --cached` + gitignore** |

### Finding F9 (Skill Corpus Authority) Options:
- **Option A (Recommended):** Declare `.agents/skills/` the sole canonical version-controlled skills corpus. Untrack `.qwen/skills/` (via `git rm --cached`) so git history no longer maintains a redundant mirror. Files remain on disk for local tool operation.
- **Option B:** Maintain `.qwen/skills/` as a live mirror in git and build an automated drift gate in CI.
- **Option C:** Generate `.qwen/skills/` via a sync script on pre-commit.

---

## Architecture-Safe Recommendation

1. **F1:** Adopt **Option C (Mixed/Attributed Policy)**:
   - Explicitly keep `.agents/` tracked as canonical shared project infrastructure.
   - Untrack `.qwen/`, `.codex/`, and `.cursor/` from git index using `git rm --cached` (preserving all local files on disk).
   - Add `.qwen/`, `.codex/`, and `.cursor/` to `.gitignore` under the `# AI-tool workspaces` section without disturbing existing user entries.
2. **F9:** Adopt **Option A**:
   - `.agents/skills/` is the sole version-controlled canonical authority.
   - Eliminating the tracked `.qwen/skills/` mirror resolves the F9 drift finding completely.

---

## Foreman Signature Gate

- **Chosen Policy:** [PENDING FOREMAN DECISION]
- **Signer:** [User / Foreman]
- **Date:** [YYYY-MM-DD]
- **Conditions:**
  1. `git rm --cached` ONLY. No local files or directories on disk may be deleted.
  2. `.agents/` remains tracked in git.
  3. Pre-existing dirty `.gitignore` changes must be preserved additively.
