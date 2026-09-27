# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — production consumer wired, verified by per-file reachability.

---

## Integration update (2026-09-27) — `EncounterChoiceEffectDispatcher`

`EncounterChoiceEffectDispatcher` was authored, documented by this plan and unit-tested
(`MicroLocationHazardIntegrationTests`) but had **zero production consumers**: the live
expedition host hand-rolled the identical world-flag step inline.

**What shipped.**

- **`src/Host/ExpeditionHostSession.cs`** — the world-flag step of the encounter
  resolution now routes through `EncounterChoiceEffectDispatcher.ApplyWorldFlag(result, Flags)`.
  Provenance (`NarrativeEncounterSystem.SystemId` + `ResolutionId` + `Day`), the idempotency
  verdict (`Applied` / `AlreadyKnown` / `SkippedNoAuthority`) and the `NotApplicable` short-circuit
  all come from the dispatcher. The host maps the returned status onto its own
  `EncounterApplicationResult.Status`; the F17 hazard branch still consumes the same
  `AlreadyKnown` verdict, so a persistent flag cannot re-infect on revisit, save/reload or replay.

The hand-rolled `Flags?.Set(...)` call is gone — one authority owns this consequence.

**Evidence.** `grep -rlw EncounterChoiceEffectDispatcher src/ --include=*.cs | grep -v HostCli`
→ `src/Host/ExpeditionHostSession.cs`. `EncounterChoiceResolverTests` and
`Plan10_11CombatExplorationIntegrationTests` pass.

*Note on the recorded false claim:* the plan file itself is a documentation-only design
proposal. It was **not** a fabricated partial. What was false was the implication of production
wiring that never existed; this record closes it.

---
