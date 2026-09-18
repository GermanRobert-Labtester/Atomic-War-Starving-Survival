# WAVE 11 PART 1 — TASK B1 IMPLEMENTATION LOG
## C2[10] Plan 30 — The War Runs Without You: An Autonomous Outside World

### Terminal State: PARTIALLY-SEALED

**Premise finding:** The census row marked C2[10] as `SEALED-ELSEWHERE` citing `docs/spiritual/PLAN30_COMPLETION_REPORT.md`, but that document covers Plan 30 as "Ritual, Faith & Meaning: The Spiritual World" — a different Plan 30 axis. The C2[10] plan document explicitly names its source scope as Plan 30 — "The War Runs Without You: An Autonomous Outside World" (world politics, faction autonomy, consequence reach). These are distinct Plan 30 interpretations; the census label was a **subject mismatch**. The SEALED-ELSEWHERE claim is not supported.

---

### 1. Current-Source Evidence (HEAD)

#### Phase 30A — Faction War Daily Tick (PARTIALLY DONE at HEAD)

| Claim | Evidence |
|---|---|
| `FactionWarSystem.SimulateDailyFriction` exists | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs:185` |
| `TickDay` now calls `SimulateDailyFriction` | `src/YearOfAsh/YearOfAshHostSession.cs:164` |
| `FactionWarChainRunner.TickDay` also wired | `src/YearOfAsh/YearOfAshHostSession.cs:165` |
| War runner events `OnStageSurfaced/OnStageResolved/OnChainResolved` have zero src/ subscribers | `src/YearOfAsh/FactionWarChainRunner.cs:311-313` — grep confirms no src/ subscription |
| Faction-standing changes have a live map subscriber | `src/YearOfAsh/FactionWarMapWidget.cs:75,85,89-92` — binds/unbinds and refreshes the map view |
| `RecordWarLocationVisited` / `ResolveWarChoice` have zero src/ callers | `src/YearOfAsh/YearOfAshHostSession.cs:216,221` |

**30A wiring: PARTIALLY DONE.** The faction-war tick fires and standing changes refresh the existing map widget; the other four emitted projection events have no `src/` subscribers, and the plan's broader consequence reach remains unsealed.

#### Phase 30A — CRITICAL BLOCKER (runtime clock)

The faction war day axis (chains 480–607, communiqués 489–607, radio 480–600) exceeds the hard runtime cap:
- `YearOfAshTimelineSystem` clamps `StartDay=180, EndDay=360`
- Campaign day owner ticks Year of Ash only `if (day >= 180 && day <= 360)` (`src/Main.CampaignOwners.cs:1396-1399`)
- Timeline freezes at 360; war event chains **never fire** in any campaign

This is a design decision: either the war chains fire within the 180–360 window (requiring the calendar to use `Timeline.CurrentDay`, not `_simDay`), or the runner must be ticked past day 360 in extended play. **This requires a foreman decision before implementation.**

#### Phase 30B — Consequence Reach (NOT DONE)

Faction-war consequence projection to economy, expedition risk, trade availability, airlock security pressure, and map overlay is not present in current source. These require routing faction war state through existing system owners (`MarketSystem`, `ExpeditionSystem`, etc.) with explicit attribution.

#### Phase 30C — Caravan/Waystation/Wildlife Autonomy (NOT EVALUATED)

The C2[10] plan also covers caravan autonomous intentions, waystation state, wildlife autonomous schedules, and coast/maritime actors. These are the "30C" phases and were not evaluated in this pass. They represent significant independent work and may overlap with plan authority already claimed elsewhere.

#### No-Action Integration Test (NOT DONE)

No xUnit test exists that advances a no-action campaign and asserts that faction state changes independently of player input.

---

### 2. Projection Reach Audit (per §7.8)

| Actor Transition | Owner | Projection Event | src/ Subscriber | Player-Visible Effect |
|---|---|---|---|---|
| `SimulateDailyFriction` tension +1/day | `FactionWarSystem` | `OnTerritorialClashOccurred` | **NONE** | **NONE** |
| Territorial clash (every 15 days) | `FactionWarSystem` | `OnTerritorialClashOccurred` | **NONE** | **NONE** |
| War chain stage surfaced | `FactionWarChainRunner` | `OnStageSurfaced` | **NONE** | **NONE** |
| War chain resolved | `FactionWarChainRunner` | `OnChainResolved` | **NONE** | **NONE** |
| Faction standing change | `FactionWarSystem` | `OnFactionStandingChanged` | `FactionWarMapWidget` | Existing faction-war map refresh |
| Decree enacted | `FactionWarSystem` | `OnDecreeEnacted` | **NONE** | **NONE** |

Five projection events (`OnTerritorialClashOccurred`, `OnDecreeEnacted`, `OnStageSurfaced`, `OnStageResolved`, and `OnChainResolved`) have zero `src/` subscribers. `OnFactionStandingChanged` already refreshes the live faction-war map. That narrow surface does not provide the plan's required broad consequence reach for clashes, decrees, or chain stages, so C2[10] remains unsealed; it must not, however, be described as wholly invisible.

---

### 3. Verdict and Decision Block

Per master plan B1 §7.9 and rule 10, this task cannot be sealed without a foreman decision on:

1. **Runtime clock:** Should war chains fire at `Timeline.CurrentDay` (capped 180–360) or at `_simDay` (uncapped campaign day)? This determines which content is reachable.
2. **Consequence reach scope:** Which consequence routes (economy shock, expedition risk, radio, airlock) are in scope for the next implementation package?
3. **30C scope:** Are caravan/wildlife autonomy phases to be included in the same package or a separate one?

**Terminal state: PARTIALLY-SEALED**
- Sealed: `SimulateDailyFriction` wired to `TickDay`; `FactionWarChainRunner.TickDay` wired
- Unsealed: event projection subscribers; consequence reach (30B); caravan/wildlife autonomy (30C); no-action integration test; runtime clock decision

---

### 4. Recommended Next Package

A bounded C2[10] continuation package should:
1. Get a foreman decision on the runtime clock.
2. Wire at minimum one consequence reach path (e.g., `OnTerritorialClashOccurred` → radio) as proof-of-concept.
3. Produce a no-action campaign test.
4. Promote 30B full consequence reach and 30C to a subsequent package.
