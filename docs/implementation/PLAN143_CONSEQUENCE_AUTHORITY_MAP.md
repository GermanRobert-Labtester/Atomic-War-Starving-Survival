# Plan 143 consequence authority map

| Consequence | Single owner | Arc integration boundary |
|---|---|---|
| Arc stage / branch / completion | `NarrativeArcEventSystem` | Typed internal state machine; only completion and branch facts are saved |
| Survivor presence | `SurvivorsHostSession` and its roster/location projection | Read-only eligibility and preflight query |
| Survivor morale | `NeedsSystem` | Host adapter performs one validated mutation after all preflights pass |
| Faction intel | `JournalSystem.Knowledge` | Host adapter discovers one canonical namespaced knowledge key |
| Faction standing | `FactionWarSystem` | Host adapter canonicalizes the authored alias and calls `ModifyStanding` once |
| Expedition offer | `NarrativeArcEventSystem` offer token plus existing expedition UI | Host validates the catalog location; dispatch remains in `ExpeditionHostSession` |
| Journal history | `JournalSystem` | Optional downstream entry/knowledge projection after commit; never arc authority |
| Feedback | `Main`/`LastEvent` | Receives post-commit confirmation only |

No arc state duplicates health, morale totals, faction standing, expedition
sorties, inventory, or medical state.
