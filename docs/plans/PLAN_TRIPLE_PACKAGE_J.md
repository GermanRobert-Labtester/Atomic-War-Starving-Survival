# Triple Package J — Voluntary Register + World Evolution + Mercenary System

**Status:** APPROVED BY USER
**Date:** 2026-09-26
**Claim:** `claim-triple-j-voluntaryregister-worldevolution-mercenary-2026-09-26`

## Scope

Integrate 3 Core authorities as full host features:

1. **VoluntaryRegisterSystem** (148 lines) — stateful system for high-dose volunteer work
2. **WorldEvolutionEngine** (317 lines) — stateful engine with event catalog
3. **MercenarySystem** (357 lines) — stateful system for mercenary contracts

## Implementation

For each:
- Host session + save store
- CLI probe
- Wire into Main.*.cs
- Register save section
- Add day owner (phase 5)
- Create tests
- Verify and archive

## Verification

- Host build: 0 errors
- Probes: 10+ checks each
- Tests: 5+ focused cases each
- Adjacent gates: green

## Archival

Move to `docs/plans/integrated/<category>/INTEGRATED_PLAN_*.md`
