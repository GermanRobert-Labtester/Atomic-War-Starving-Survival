# UtilityAI Expanded Catalog Tests — Quarantine Ticket

**Audit:** local post-PR #36 issue **#39**
**Date:** 2026-09-05
**Status:** Remain Compile-Remove (`UtilityAiExpandedCatalogTests.cs`)

## Evidence from dequarantine attempt

- Tests expect **20** catalog actions; live catalog loads **6**.
- Trait refusal / skill-scaling scorer assertions fail against current `UtilityActionScorer` behavior.
- Host Utility AI panel remains live via `Main.UiTests.UtilityAi`.

## Activation criteria (UA-01)

1. Decide authority: expand `utility_actions` JSON to 20, or rewrite tests to the live 6-action set.
2. Rematch trait refusal predicates to current scorer contracts.
3. Soften any remaining exact census to floor + uniqueness (same pattern as audit #38).
4. `dotnet test --filter UtilityAiExpandedCatalog` green → remove Compile Remove.
5. Close this ticket.

**Owner lane:** Utility AI / survivors
**Expiry review:** 2026-Q4
