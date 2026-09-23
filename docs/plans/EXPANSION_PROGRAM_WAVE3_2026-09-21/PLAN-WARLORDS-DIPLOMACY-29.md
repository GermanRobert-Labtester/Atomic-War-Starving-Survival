# PLAN-WARLORDS-DIPLOMACY-29 — Doctrines, Summits, Treaties & Contested Ground

**Wave:** 3 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 7, PLAN-UNBLOCK-03 U1 (territory/
diplomacy signed artifacts), PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-WARLORDS-DIPLOMACY-29_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-WARLORDS-DIPLOMACY-29_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's warlords & diplomacy
systems (2 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real-world nations/leaders, no new faction authority
(`FactionWarSystem`, standing, branch systems remain canonical), no tactical
combat rewrite.

---

## 1. Outcome

Turn the Year of Ash from a background war into a **negotiable, personal
conflict**: warlords with doctrines, summits with agendas, treaties with
obligations, and ground you can hold or lose. The corpus already contains
`WarlordDoctrineSystem`/`Catalog`/`ResponseActions`,
`DiplomaticSummitSystem`, `DiplomaticTreatyCatalog`, `FactionDiplomacySystem`,
`TreatyConsequences`, `PrpfStandingSystem`, `TerritoryControlSystem`,
`FactionEmbargoLedger`, `RegionalTreatySystem`, `PatrolTerritoryAuthority`, and
the `FactionWar*` content families (communiqués, dialogue, events, journal,
radio, location overrides) with 22 faction data files.

Player loop: **identify doctrine → attend or refuse a summit → sign/breach
treaties → hold territory → live with the consequences**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Warlord doctrines | `WarlordDoctrineSystem`, `warlord_doctrines.json` | read doctrine, choose counters | rival behavior changes; response actions fire |
| Summits | `DiplomaticSummitSystem`, `DiplomaticTreatyCatalog` | attend, negotiate, walk out | standing shifts, treaty offers |
| Treaties | `TreatyConsequences`, `treaty_templates.json` | sign, meet/miss obligations | supplies, standing, breach fallout |
| Territory | `TerritoryControlSystem`, `faction_territory.json` | fortify, patrol, contest | control %, travel safety, taxes |
| War economy | `FactionEmbargoLedger`, `LedgerDebtSystem`, `FactionBountySystem` | embargo, trade, bounty | prices, shortages, contracts |
| Intelligence | `faction_intelligence.json`, psyops/radio | intercept, broadcast | suspicion, morale, defections |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core authorities | `Warlords/` (4 files), `Diplomacy/` (3), `Treaties/TreatyConsequences.cs`, `Factions/TerritoryControlSystem.cs`, `Factions/PrpfStandingSystem.cs`, `FactionEmbargoLedger.cs` |
| Host-unreachable | `FactionDiplomacySystem`, `TerritoryControlSystem`, `PrpfStandingSystem`, branch systems (independent/military/rebel), `WarlordDoctrineSystem` demo-only |
| Data | `warlord_doctrines.json`, `diplomatic_treaties.json`, `treaty_templates.json`, `faction_war_*.json` (5), `faction_territory.json`, `faction_intelligence.json`, `faction_lore.json`, branch files (3) |
| Prior seals | Plan 30 war projection/clock, Plan 124 overrides, Plan 125 moral flags, Plan 134 territory control, Plan 153 espionage, branch expansions (DEC-287…292) |
| Baselines | war clock maps playable day 180→authored 480 (`DEBT-PLAN30-RUNTIME-CLOCK`) |

---

## 3. Packages

### WF-29A — Doctrine-driven warlords
- Bind `WarlordDoctrineSystem` to a host session; doctrines drive AI response
  selection and `WarlordResponseActions`; the player sees a doctrine dossier
  and counter options.
- **Acceptance:** rival behavior differs measurably by doctrine in a seeded
  run; no random personality; dossier readable.
- **Verify:** `--warlords-selftest` (existing demo) + focused tests.

### WF-29B — Diplomatic summits
- `DiplomaticSummitSystem` becomes a timed, scheduled event (communiqué + radio
  + briefing) with an agenda; player choices map to `FactionDiplomacySystem`
  outcomes; refusal has a cost.
- **Acceptance:** summit outcomes are deterministic per seed; standing deltas
  bounded; the event is forecast-visible on the calendar.
- **Verify:** `--communique-board-selftest` + faction suites.

### WF-29C — Treaties and consequences
- `TreatyConsequences` evaluates met/missed/violated obligations from live
  state (water/power/patrol deliveries); breach routes to standing, embargo,
  and consequence policies already authored.
- **Acceptance:** every treaty has a measurable obligation; breach is
  explainable in the panel; no parallel treaty store.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/` (consequence
  policies) + regional treaty suites.

### WF-29D — Territory, patrols and checkpoints
- `TerritoryControlSystem` consumes fortification, garrison, and supply line
  state; `PatrolTerritoryAuthority` produces checkpoints/tolls on routes;
  contest resolution is seeded and reversible.
- **Acceptance:** map shows control and contest; travel safety/tax derive from
  it; losses can be recovered; no duplicate territory store.
- **Verify:** `--territory-selftest` (existing Plan 134 probe) + world suites.

### WF-29E — War economy
- Embargo/blockade/bounty run through `FactionEmbargoLedger`,
  `LedgerDebtSystem`, `FactionBountySystem`; player response (trade other
  routes, pay, default) uses the canonical funds/ledger.
- **Acceptance:** no second ledger; DEC-22/26/30/37 contracts preserved.
- **Verify:** economy + faction focused suites.

### WF-29F — Intelligence and psyops
- Intercepts from `faction_intelligence.json` feed the rumor/briefing path;
  psyops campaigns target morale/suspicion through existing PsyOps owner;
  defection offers use the existing survivor recruitment path.
- **Acceptance:** intelligence is actionable (named threat/opportunity), not
  flavor text; psyops capped; no duplicate influence store.
- **Verify:** radio/psyops suites.

### WF-29G — Content volumes
- +8 doctrines, +10 summit agendas, +15 treaty obligations, +12 territory
  events, +20 dialogue lines; fictional entities only; every row consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Diplomacy becomes a text wall | one decision per event; panel shows obligation deltas |
| Territory contest unpredictability | seeded resolution with visible odds band; recovery path |
| War economy collapses early game | difficulty presets + embargo exceptions for medicine/food |
| Warlord behavior opaque | doctrine dossier + prior acts in the chronicle |

## 5. Verification

```bash
godot --headless --path . -- --communique-board-selftest
godot --headless --path . -- --faction-war-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Factions/
bash scripts/run_test.sh Ashfall.Core.Tests/World/
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
```

---

## 6. Expanded census (13 files · 3,773 lines)

Scope: `Assets/Ashfall.Core/Factions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 4 · Demo 1 · Support 2 · System 6

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FactionDiplomacySystem.cs` | 451 | System | **yes** | 0 | 0 | 2 |
| `FactionBountySystem.cs` | 254 | System | — | 0 | 0 | 2 |
| `FactionBranchCoordinator.cs` | 668 | System | — | 0 | 0 | 4 |
| `FactionCovertOpsCoordinator.cs` | 518 | System | — | 0 | 0 | 2 |
| `FactionDisplayNameCatalog.cs` | 118 | Catalog | — | 0 | 0 | 0 |
| `FactionIntelligenceCatalog.cs` | 57 | Catalog | — | 0 | 0 | 0 |
| `FactionStandingIdResolver.cs` | 131 | Support | — | 0 | 0 | 0 |
| `PrpfStandingSystem.cs` | 204 | System | — | 0 | 0 | 2 |
| `TerritoryControlSystem.cs` | 451 | System | **yes** | 0 | 0 | 1 |
| `FactionActionBoard.cs` | 405 | Support | — | 0 | 0 | 2 |
| `FactionActionCatalog.cs` | 227 | Catalog | — | 0 | 0 | 0 |
| `FactionCultureCatalog.cs` | 90 | Catalog | — | 0 | 0 | 0 |
| `FactionEcologyHeadlessDemo.cs` | 199 | Demo | — | 0 | 0 | 9 |

**Totals:** 0 banned refs · 0 empty catches · 8 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_territory.json` | object[4 keys] |
| `faction_war_events.json` | object[2 keys] |
| `faction_war_journal.json` | array[26] |
| `faction_war_radio.json` | object[2 keys] |
| `foundry_faction.json` | object[9 keys] |
| `holdfast_factions.json` | object[2 keys] |

**State surfaces:** `FactionDiplomacySystem.cs`, `FactionBountySystem.cs`, `FactionBranchCoordinator.cs`, `FactionCovertOpsCoordinator.cs`, `PrpfStandingSystem.cs`, `TerritoryControlSystem.cs`, `FactionActionBoard.cs`, `FactionEcologyHeadlessDemo.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Factions/` |
| Test references | 29 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 11. Tier-2: intra-domain reference graph

Computed across 13 domain files: **10 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `FactionBranchCoordinator.cs` | 668 | 0 | 1 |
| `FactionCovertOpsCoordinator.cs` | 518 | 0 | 0 |
| `FactionDiplomacySystem.cs` | 451 | 0 | 0 |
| `TerritoryControlSystem.cs` | 451 | 0 | 0 |
| `FactionActionBoard.cs` | 405 | 3 | 4 |
| `FactionBountySystem.cs` | 254 | 0 | 1 |
| `FactionActionCatalog.cs` | 227 | 5 | 1 |
| `PrpfStandingSystem.cs` | 204 | 1 | 0 |
| `FactionEcologyHeadlessDemo.cs` | 199 | 0 | 3 |
| `FactionStandingIdResolver.cs` | 131 | 1 | 0 |

**Highest-coupling files (in×2 + out):**

- `FactionActionCatalog.cs` — in 5, out 1
- `FactionActionBoard.cs` — in 3, out 4
- `FactionEcologyHeadlessDemo.cs` — in 0, out 3
- `FactionStandingIdResolver.cs` — in 1, out 0
- `PrpfStandingSystem.cs` — in 1, out 0
- `FactionBountySystem.cs` — in 0, out 1
- `FactionBranchCoordinator.cs` — in 0, out 1
- `FactionDiplomacySystem.cs` — in 0, out 0

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 9. Other plans referencing their names: **10**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 8 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-JUSTICE-LAW-37` | 1 |
| `PLAN-ESPIONAGE-COUNTERINTEL-41` | 1 |
| `PLAN-CRIME-SYNDICATES-44` | 1 |
| `PLAN-ESPIONAGE-SYSTEM-TRUTH-161` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WF-29A` | no name match — resolve at claim time |
| `WF-29B` | no name match — resolve at claim time |
| `WF-29C` | no name match — resolve at claim time |
| `WF-29D` | `TerritoryControlSystem.cs` |
| `WF-29E` | no name match — resolve at claim time |
| `WF-29F` | `FactionIntelligenceCatalog.cs` |
| `WF-29G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 14. Host files: **10** · Test files: **30** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/BlackMarketHostSession.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/FactionBranchHostSession.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/MusterHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 30 | `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs`, `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Ashfall.Core.Tests/Diplomacy/Plan197FactionDiplomacyIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/LoanSharkEnforcerEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/black_market_inventory.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `counter_intelligence` |
| `dose_ledger` |
| `faction_espionage` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--communique-board-selftest` |
| `--dose-ledger-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-tick-demo` |
| `--ledger-debt-selftest` |
| `--standing-record-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnBountyRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBountyRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBranchDecided` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnEmbargoAdded` | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` |
| `OnEmbargoRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnEmbargoRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (10 files, 60 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Culture` | 7 | 40 |
| `Diplomacy` | 1 | 6 |
| `Warlords` | 2 | 14 |

**Verdict:** 60 cases sit under matching regions — run those first (`Culture`, `Diplomacy`, `Warlords`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **30**
(14 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/StandingRecordHostSession.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.FactionBranch.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `counter_intelligence` | no |
| `dose_ledger` | yes |
| `faction_espionage` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `black_market_bounty` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **27**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 18, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **6**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_broken_compact_locked` |
| `flag_branch_iron_way_locked` |
| `flag_branch_listener_locked` |
| `flag_branch_mercy_road_locked` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 3 (laddered 1) · RNG streams 1 · host files 19 · catalogs 22 · test regions 3 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WARLORDS-DIPLOMACY-29
wave: —
status: PROPOSED — foreman claim required
packages: WF-29A, WF-29B, WF-29C, WF-29D, WF-29E, WF-29F, WF-29G
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceHostSession.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bounty_board.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
  - godot --headless --path . -- --communique-board-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
