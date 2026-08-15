---
name: ashfall-agents
description: UNIVERSAL — Generate or update ALL 13 tool-specific context files (AGENTS.md, QWEN.md, .clinerules, .cursorrules, CLAUDE.md, CODEX.md, GOOSE.md, OPENSETUP.md, ANTIGRAVITY.md, CRUSH.md, VIBE.md, MIMOCODE.md, .windsurfrules) from the canonical AGENTS.md source. Use when any agent context file needs refreshing, when a new developer joins, or when the project architecture has evolved.
---

# ASHFALL Universal Agents.md Generator

This skill maintains a **single source of truth** (`AGENTS.md`) and propagates it to ALL 13 AI coding agent context files.

## Supported Tools

| # | Tool | File | Type |
|---|------|------|------|
| 1 | **Qwen Code** | `AGENTS.md` | **CANONICAL SOURCE** |
| 2 | **Qwen Code** | `QWEN.md` | Copy |
| 3 | **Cline** | `.clinerules` | Copy |
| 4 | **Cursor** | `.cursorrules` | Copy |
| 5 | **Claude Code** | `CLAUDE.md` | Copy |
| 6 | **Codex (OpenAI)** | `CODEX.md` | Copy |
| 7 | **Goose (Block)** | `GOOSE.md` | Copy |
| 8 | **OpenCode** | `OPENSETUP.md` | Copy |
| 9 | **Antigravity IDE/Desktop/CLI** | `ANTIGRAVITY.md` | Copy |
| 10 | **Crush** | `CRUSH.md` | Copy |
| 11 | **Vibe** | `VIBE.md` | Copy |
| 12 | **MimoCode** | `MIMOCODE.md` | Copy |
| 13 | **Windsurf** | `.windsurfrules` | Copy |

## When Invoked

1. Read the current `AGENTS.md` (canonical source)
2. Read `REPO_REVIEW_REPORT.md` for the latest architectural findings
3. Read `docs/GODOT_MIGRATION_STATUS.md` for migration status
4. Read key project files to verify claims (`.csproj` targets, namespace conventions, etc.)
5. Generate/update `AGENTS.md` with the latest comprehensive architecture
6. **Propagate AGENTS.md to ALL 13 tool-specific files** with appropriate headers

## The Generated AGENTS.md Must Include (16 sections)

See the existing `AGENTS.md` for the full template. Every section must be preserved:

1. **Project Identity** — name, genre, engine stack, original IP rule, tone
2. **Technology Stack** — table with file counts, namespaces, .NET targets for all 5 layers
3. **Core Architecture** — 6 invariants with violation status
4. **Port/Adapter Interfaces** — table with Godot and Unity adapter status
5. **Determinism Rules** — `ISeededRng` only, known violations
6. **Save/Load Architecture** — pattern, SaveChecksum, versioned migration, known gaps
7. **Event System** — two-bus split documentation
8. **Data Integrity** — CatalogIntegrityValidator, ID rules, JSON issues
9. **Expansion System** — ExpansionMasterSession, 5-phase pattern, Phase 0 placeholders
10. **Known Issues & Anti-Patterns** — 6 CRITICAL + 12 HIGH tables with file paths
11. **Bridge Shim Rules** — BridgeGap, BridgeSelfTest, migration aid
12. **Namespace Conventions** — table mapping layer to namespace
13. **Verification Checklist** — 5 exact commands
14. **Git Rules** — commit frequency, LFS, gitignore
15. **Domain Reference** — 30+ items across 7 categories
16. **Workflow** — 5-step + pre-step 0

## Propagation (MUST run after generating AGENTS.md)

```bash
AGENTS=$(cat AGENTS.md)
TODAY=$(date +%Y-%m-%d)

for f in QWEN.md .clinerules .cursorrules CLAUDE.md CODEX.md GOOSE.md OPENSETUP.md ANTIGRAVITY.md CRUSH.md VIBE.md MIMOCODE.md .windsurfrules; do
  tool_name=$(echo "$f" | sed 's/\.md$//' | sed 's/^\.//')
  echo "# ASHFALL PROJECT — ${tool_name} Rules
# AUTO-GENERATED from AGENTS.md (canonical source). Run /ashfall-agents to regenerate.
# Last generated: ${TODAY}" > "$f"
  echo "$AGENTS" >> "$f"
done

echo "Propagated AGENTS.md to 13 files"
ls -la AGENTS.md QWEN.md .clinerules .cursorrules CLAUDE.md CODEX.md GOOSE.md OPENSETUP.md ANTIGRAVITY.md CRUSH.md VIBE.md MIMOCODE.md .windsurfrules
```

## Output

After completion, report:
- Number of sections in AGENTS.md
- Number of tool files propagated (should be 13)
- File sizes
- Any verification failures
- The exact command: `git status` to review changes