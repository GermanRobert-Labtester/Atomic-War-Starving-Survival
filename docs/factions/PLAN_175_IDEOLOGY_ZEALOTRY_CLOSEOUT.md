# Plan 175 — Fictional Ideological Pressure & Zealotry: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-174-177-FLAGSHIP-SURVIVOR-WORLD`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Survivors/ZealotrySystem.cs` (`zealotry`) — conversion, fervor/conviction/dissent, charismatic leaders, shrine flags, bounded buff queries, ritual demands, escalation ladder; strict catalog loader |
| Data | `Assets/StreamingAssets/Data/wasteland_religions.json` — 3 mechanical profiles keyed to the authored fictional movements (Ash Witnesses / Rebuilders / Listeners); every row **must** carry the `fictional` tag or the loader rejects it (§1.6 enforced in code) |
| Host | `src/Host/ZealotrySaveStore.cs`, `src/Main.Zealotry.cs` (adoption, ritual cadence, crisis→morale route, host commands) |
| Save | `SaveSectionRegistry` row (`zealotry`, owner `social`) + `zealotry_save.json` |
| Tests | Core 20/20 (`ZealotrySystemTests`) · wiring 5/5 (`Plan175ZealotryHostWiringTests`) |

## Contract guarantees (§31 DoD — all satisfied)

- **All beliefs fictional.** Content keys only to authored `belief_movements.json` movements; real-world religions are absent from data, code, and tests; a load-time guard rejects untagged rows.
- **Conversion deterministic and bounded.** Chance = base × leader charisma × vulnerability × shrine/bond pull × **resistance** — committed believers resist hard (proven statistically across seeds); vulnerable ≠ automatic target; same-belief attempts deepen conviction, never double-convert.
- **Fervor never bypasses morale.** The system only *reports* bounded modifiers (`GetDespairResistanceBp` ≤ 2000 bp, `GetCohesionBonusBp` ≤ 1500 bp, gated by conviction/fervor/shrine, erased by crisis); morale truth stays in `NeedsSystem`/`MoraleContagionSystem`.
- **Benefits have costs.** Fervor above the authored fanaticism threshold grows dissent (fastest under `low` tolerance); failed rituals (missing offerings) cost fervor — no free buffs.
- **Shrines use the construction authority.** Shrine presence is a host-registered flag behind a real room; no free passive global buff.
- **Ritual resources are transactional.** Bounded cadence (every 3rd day, one open demand); offerings consumed through the canonical inventory; **no survivor-sacrifice stat button** (§7.10).
- **Violence escalates through the ladder, never a silent purge.** Argument → Ostracism → WorkRefusal → PropertyDamage → **AssaultThreat (typed event; the host routes any real violence through combat/encounter authority)** — the ladder hard-stops; tension resolved → daily de-escalation (Trap F avoided).
- **PsyOps influence is bounded.** `ApplyBroadcast` moves fervor of existing adherents only (capped by `broadcast_profile`) — broadcasts never convert outsiders.
- **Crisis reversal.** Leader death / doctrinal collapse → fervor quartered, conviction shocked down, in-crisis flag; the host applies bounded morale damage (20–45) through canonical `Modify`; recovery is slow and partial.
- **Save/load exact; old saves adopt existing friction beliefs as mild adherence only** — never auto-conversions, never fabricated beliefs (§10).

## Deferred (flagged, not silent)

`BeliefsPanel` (setting-neutral naming, presentation wave); PsyOps auto-routing (host command `ZealotryApplyBroadcast` exposed; call site pending); shrine auto-registration from live construction state; leader-death auto-crisis hook (command exposed).
