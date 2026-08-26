# ASHFALL Agent-Rulebook Synchronization Report

**Canonical source:** `AGENTS.md`  
**Synced files:** 12 derived client files  
**Sync date:** 2026-08-22  
**Tool:** `ashfall-agents-sync` skill  

---

## PHASE 1 — Drift Audit

All 12 derived files are structurally identical to `AGENTS.md` beyond the client-branding header.
Each diff is exactly 12 lines (6 removed from canonical header + 6 added branding header).

| File | Divergence Class | Evidence |
|------|------------------|----------|
| `CLAUDE.md` | COSMETIC | Header replaced with `# CLAUDE CODE INSTRUCTIONS — ASHFALL PROJECT` + auto-generated notice |
| `CODEX.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — CODEX Instructions` + auto-generated notice |
| `CRUSH.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — CRUSH Instructions` + auto-generated notice |
| `GOOSE.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — GOOSE Instructions` + auto-generated notice |
| `QWEN.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — QWEN Instructions` + auto-generated notice |
| `VIBE.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — VIBE Instructions` + auto-generated notice |
| `MIMOCODE.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — MIMOCODE Instructions` + auto-generated notice |
| `OPENSETUP.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — OPENSETUP Instructions` + auto-generated notice |
| `ANTIGRAVITY.md` | COSMETIC | Header replaced with `# ASHFALL PROJECT — ANTIGRAVITY Instructions` + auto-generated notice |
| `.clinerules` | COSMETIC | Header replaced with `# ASHFALL PROJECT — Cline Rules` + auto-generated notice |
| `.cursorrules` | COSMETIC | Header replaced with `# ASHFALL PROJECT — Cursor Rules` + auto-generated notice |
| `.windsurfrules` | COSMETIC | Header replaced with `# ASHFALL PROJECT — Windsurf Rules` + auto-generated notice |

**STALE:** 0  
**NEWER:** 0  
**CONFLICT:** 0  

---

## PHASE 2 — Reconcile

No reconciliation required. All files already contain the canonical rulebody; only the header branding differs.
The auto-generated notice (`Run ashfall-agents-sync to regenerate`) is present and correct in all files.

---

## PHASE 3 — Structure Recommendation

**Current state:** All 12 files are full copies of `AGENTS.md` with a branded header.  
**Recommendation:** Keep as **full copies** for now.

### Trade-offs

| Approach | Pros | Cons |
|----------|------|------|
| **Full copy** (current) | Each client file is self-contained; works offline; no parsing required; safe if canonical is moved | Drift risk over time; 12× duplication |
| **Thin pointer** | Single source of truth; zero drift | Requires every client to follow the pointer; fragile if canonical is renamed or moved; some clients may not resolve relative includes |

### When to convert
Convert to thin pointers only if:
1. A client is added/removed frequently, OR
2. The canonical file moves to a non-root path, OR
3. Drift audits start catching STALE/CONFLICT entries again.

**Conversion template for a thin pointer:**
```markdown
# ASHFALL PROJECT — <CLIENT> Instructions

Read `AGENTS.md` before any work. This file contains only <CLIENT>-specific additions.

## Client-specific additions
(Add here if needed.)
```

---

## Quality Gate

- [x] Zero `CONFLICT` divergences remain
- [x] Every synced file still contains the five non-negotiable rules
- [x] Every synced file still contains the verification checklist (`dotnet` + `godot --headless`)
- [x] No gameplay code touched

---

## Next-Drift Risk Notes

- The header `Last generated: 2026-08-22` is hardcoded. Run `ashfall-agents-sync` after every meaningful edit to `AGENTS.md`.
- Watch for new root-level `*RULES*.md` or `*.md` instruction files; add them to the target list in this skill.
