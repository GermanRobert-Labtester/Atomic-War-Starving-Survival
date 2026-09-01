# Faction Action Coverage Audit (Plan 25 · 25A.2)

> Verified 2026-09-01. Establishes whether the plan's cited "3 actions / 1 action" gap is runtime-relevant.
> **Verdict: the cited numbers count dossier entries, not performable actions. No runtime action-execution pipeline exists for any faction.** Plan 25 builds it (FactionActionBoard) rather than padding dead files.

## Per-faction coverage before Plan 25

| Faction | Authored dossier entries | Runtime-consumed actions | Standing-sensitive behavior | Trade | Request | Threat | Dispute | Culture | War-only |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Scavenger Guild (Muster) | — (dossier in `currents.json`) | 2 hardcoded ops: `ClaimSite`, `RecordOverStrip` | trust float, no external reader | quest prose only | — | blacklist (binary) | — | — | — |
| Hydro Barons (Muster) | — (dossier in `currents.json`) | 1 one-shot: `ResolveApproach` A–D | trust float, no external reader | quest prose only | — | plant seizure | — | — | — |
| Iron Raiders (Muster) | — (dossier in `currents.json`) | 1 formula + 1 op: `EvaluateRaidChance` (never rolled), `ExecuteRaid` | **none** (aggression feed unwired) | — | — | raid only | — | — | yes |
| Coalition Camp (Muster) | — (dossier in `currents.json`) | 3 ops: `Form`, `RallyDeserter`, `SetStrategy` | lockout risk | — | rally | lockout | — | — | endgame |
| Holdfast factions (Exp 01) | 3 dossiers | 0 (terminal UI display) | dossier `trust` display | UI | — | — | — | UI quote | — |
| Standing Record (Exp 03) | 1 dossier | 0 (**file dead — no loader**) | n/a | — | — | — | — | — | — |
| Silent Foundry (Exp 09) | 1 faction + 6 divisions | 0 (divisions display-only) | own stance engine (trade) | yes (foundry trade) | — | — | — | identity text | — |
| Faction lore (codex) | 23 entries | validation + UI | — | — | — | — | — | yes (lore) | — |

## Conclusions

1. **Runtime-relevant peacetime authored actions: 0 across all four Muster factions.** The only faction interactions are hand-coded state mutations wired to specific UI handlers (`src/Main.Muster.cs:104-178`).
2. `standing_record_factions.json` must not receive content (no loader). Documented here so no future task authors into it.
3. `holdfast_factions.json` belongs to Expansion 01's terminal surface; Plan 25 leaves it untouched.
4. The Plan 25 action set (A1–A8 minimum, 10–12 preferred) lands in the new `muster_faction_actions.json` authority, consumed by `FactionActionBoard` in `Ashfall.Core.Muster` — one loader, one resolver, four factions.

## Post-Plan-25 target state

| Faction | Authored actions (target) | Standing-sensitive | Grievance hooks |
|---|---:|---|---|
| Scavenger Guild | 3 (A1, A2, internal dispute) | yes | 1 |
| Hydro Barons | 3 (A3, A4, toll dispute) | yes | 1 |
| Iron Raiders | 3 (A5, A6, code-based) | yes | 1 |
| Coalition Camp | 3 (A7, A8, neutral-ground) | yes | 1 |

Every action: consumed by the board at runtime, standing-band variants, produces/consumes named flags, save-idempotent. Gated by content-utilization selftest.
