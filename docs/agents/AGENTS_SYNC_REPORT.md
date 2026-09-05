# ASHFALL Agent-Rulebook Synchronization Report

**Canonical source:** `AGENTS.md`<br>
**Synced files:** 12 derived client files<br>
**Sync date:** 2026-09-05<br>
**Tool:** `scripts/ci/sync-agent-rulebooks.py`

---

## PHASE 1 — Drift Audit & Status

All 12 derived files are structurally synchronized with `AGENTS.md`.

| File | Divergence Class | Header Branding |
|---|---|---|
| `.clinerules` | SYNCED | `ASHFALL PROJECT — Cline Rules` |
| `.cursorrules` | SYNCED | `ASHFALL PROJECT — Cursor Rules` |
| `.windsurfrules` | SYNCED | `ASHFALL PROJECT — Windsurf Rules` |
| `ANTIGRAVITY.md` | SYNCED | `ASHFALL PROJECT — ANTIGRAVITY Instructions` |
| `CLAUDE.md` | SYNCED | `CLAUDE CODE INSTRUCTIONS — ASHFALL PROJECT` |
| `CODEX.md` | SYNCED | `ASHFALL PROJECT — CODEX Instructions` |
| `CRUSH.md` | SYNCED | `ASHFALL PROJECT — CRUSH Instructions` |
| `GOOSE.md` | SYNCED | `ASHFALL PROJECT — GOOSE Instructions` |
| `MIMOCODE.md` | SYNCED | `ASHFALL PROJECT — MIMOCODE Instructions` |
| `OPENSETUP.md` | SYNCED | `ASHFALL PROJECT — OPENSETUP Instructions` |
| `QWEN.md` | SYNCED | `ASHFALL PROJECT — QWEN Instructions` |
| `VIBE.md` | SYNCED | `ASHFALL PROJECT — VIBE Instructions` |

**STALE:** 0<br>
**NEWER:** 0<br>
**CONFLICT:** 0

---

## Quality Gate Checklist

- [x] Zero `CONFLICT` divergences remain
- [x] Every synced file contains the 5 non-negotiable rules
- [x] Every synced file contains the 6 core invariants
- [x] Every synced file contains the canonical MCP connection registry (`composio`, `google-stitch`)
- [x] Every synced file specifies `dotnet` + `godot --headless` as the canonical verification path
- [x] Zero gameplay code touched
