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

## PHASE 2 — Actions Taken (2026-09-05, F17–F20 propagation)

**Canonical change (approved by task owner):** added `## MICRO-LOCATION INTEGRATION CONTRACT (F17–F20)` to `AGENTS.md` (between EVENT SYSTEM and DATA INTEGRITY). The contract records:

- the fixed consequence-application order (item → journal → location → flag → hazard) and the no-parallel-`MicroLocationXxxSystem` rule;
- `MicroLocationHazardRegistry` as the single routing point for hazard world flags, and the consumer-or-inert-list integrity gate enforced by `MicroLocationIntegrationDeterminismTests`;
- `seed_packets` plantability through the canonical 13-crop `CropCatalog` (no micro-location planting exceptions);
- one-shot discipline (Core F1 depletion + flag-transition hazard + `Infect` no-op) and the no-new-RNG determinism rule;
- pointers to `docs/discovery/MICRO_LOCATION_{HAZARDS,GREENHOUSE,RADIO,WATER}.md` and the pending selection-context extension (`docs/plans/PLAN_F21_DISCOVERY_SELECTION_CONTEXT_EXTENSION.md`).

All 12 client files regenerated from canonical via `sync-agent-rulebooks.py`; `--check` reports 0 drift.

---

## Remaining Recommendations & Next-Drift Risk Notes

- Client files remain **full derived copies** (branded 3-line header + verbatim canonical body). Thin-pointer conversion is possible but not recommended while CI diffs are this cheap; revisit only if AGENTS.md exceeds ~80 KB.
- **Next-drift risk:** hand-editing any client file directly. All client edits must land in `AGENTS.md` first, then `python3 scripts/ci/sync-agent-rulebooks.py`; CI `--check` fails the build on drift.
- The F17–F20 contract references `PLAN_F21_DISCOVERY_SELECTION_CONTEXT_EXTENSION.md` — if that plan file is moved or renamed, update the AGENTS.md pointer in the same commit.

---

## Quality Gate Checklist

- [x] Zero `CONFLICT` divergences remain
- [x] Every synced file contains the 5 non-negotiable rules
- [x] Every synced file contains the 6 core invariants
- [x] Every synced file contains the canonical MCP connection registry (`composio`, `google-stitch`)
- [x] Every synced file specifies `dotnet` + `godot --headless` as the canonical verification path
- [x] Zero gameplay code touched
