---
name: ashfall-agents-sync
description: Detects drift across ASHFALL's many AI-agent rule files (AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, QWEN.md, VIBE.md, MIMOCODE.md, OPENSETUP.md, ANTIGRAVITY.md, .clinerules, .cursorrules, .windsurfrules) and re-syncs them so all agents read one truth.
---

# ASHFALL Agent-Rulebook Synchronizer

## ROLE

ASHFALL is worked on by many AI clients, each with its own instruction file at the repo root. These files are near-duplicates and WILL drift, letting one agent operate on stale rules. You keep them coherent.

`AGENTS.md` is the canonical source. Every other client file is a derived copy or a pointer.

## TARGET FILES
`AGENTS.md` (canonical), `CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GOOSE.md`, `QWEN.md`, `VIBE.md`, `MIMOCODE.md`, `OPENSETUP.md`, `ANTIGRAVITY.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, plus any new `*RULES*`/`*.md` instruction files discovered at root.

## WORKFLOW

### PHASE 1 — Drift Audit (always run first)
- Normalize cosmetic branding (client name substitutions) then pairwise diff each file against `AGENTS.md`.
- Classify every divergence:
  - `COSMETIC` — client-name/branding only.
  - `STALE` — older rule text than canonical (e.g. still mentions `src/Bridge/` as active, or pre-migration stack).
  - `NEWER` — file contains a rule NOT in canonical (candidate promotion).
  - `CONFLICT` — contradicts canonical (e.g. permits Unity batchmode).
- Report a drift table: file × divergence class × evidence lines.

### PHASE 2 — Reconcile
- `NEWER` items: propose promotion into `AGENTS.md` first (ask before changing canonical).
- `CONFLICT` items: canonical wins; fix the client file.
- `STALE`/`COSMETIC`: regenerate the client file from canonical with only the branding changed.

### PHASE 3 — Decide Structure (recommend once)
- Recommend whether each client file should remain a full copy or become a thin pointer (`Read AGENTS.md before any work. Client-specific additions: …`). Present trade-offs; only convert with explicit approval.

## RULES
1. Never edit `AGENTS.md` silently — every canonical change is listed and approved.
2. Preserve each file's client-specific header/notes if present.
3. Verify after sync: pairwise diff must show only permitted branding deltas.
4. `dotnet`/`godot --headless` remain the only verification verbs in every file.

## OUTPUT
`docs/agents/AGENTS_SYNC_REPORT.md` — drift table, actions taken, remaining recommendations, next-drift risk notes.

## QUALITY GATE
- Zero `CONFLICT` divergences remain.
- Every synced file still contains the five non-negotiable rules and the verification checklist.
- No gameplay code touched.
