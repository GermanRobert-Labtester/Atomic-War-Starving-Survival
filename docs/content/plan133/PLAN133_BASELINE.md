# Plan 133 Baseline — Faction War Communiqués (18 → 40)

> Verified against trunk 2026 (commit c3a5bf6a-era working tree). Read-only recon.
> The communiqué catalog is a **presentation/evidence layer** over faction-war event
> chains. Communiqués interpret, deny, frame, justify, correct, or exploit events that
> happened; they never determine that those events happened.

## 0. Executive implementation contract — verified answers

| # | Question | Verified answer |
|---|---|---|
| 1 | DTO + required fields | `FactionWarCommunique` (`Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:236-246`): `id, eventChainId, factionId, day, title, body` + optional `authorNote` (default `string.Empty`). Loader (`LoadCommuniques`, same file) skips only entries with null/empty `id`. No other validation at load. |
| 2 | `eventChainId` validated/consumed? | **Not validated by the loader or `CatalogIntegrityValidator`.** No FK tier covers `eventChainId`. No runtime consumer joins communiqués to chains today; resolution must be pinned by tests (Plan 133 adds a strict-FK test). |
| 3 | `day` role | **Availability gate only.** `GetCommuniquesForFaction(factionId, day)` returns entries with `c.day <= day`, in file order. No sorting, no ordering semantics. The shipped file is chronologically ordered; Plan 133 preserves full-file chronological order (file order is not a pinned contract — no test or consumer relies on it — but chronological order is kept as the authored presentation order). |
| 4 | `factionId` resolution | Ordinal string comparison only. The known-faction list is pinned by `FactionWarContentCatalogTests.All_Faction_References_Resolve` (includes all four communiqué voices). `FactionWarSystem` defaults cover `faction_central_garrison/rebuilders/ash_sign/forward_roster`. |
| 5 | `authorNote` exposure | **Zero consumers.** Repo-wide grep: `authorNote` appears only in the DTO definition and the JSON files. No test, no `src/` code, no UI, no epilogue, no journal consumer reads it. No player-facing renderer for communiqués exists at all today (the `FactionsPanel` "communiqués" sections are hardcoded fixtures, not this catalog). Plan 133 adds mechanical gates to keep it that way. |
| 6 | Branch-impossible statements? | Yes, possible. Chains branch by player choice (see §3 below). Plan 133's gate: communiqués may only reference auto-firing main-arc chains (`evt_d480`–`evt_d605`); the flag-gated `evt_p25_*` chains (E-P/E-W/E-R) may never have fired, so no static public statement may reference them as fact. Branch-sensitive facts (who received the LN74 intercept, Denner's fate, player evacuation choices) are excluded from all new prose; the **COMMUNIQUE_BRANCH_SAFETY_MATRIX.md** classifies every entry A–E. |
| 7 | Multiple per faction/event/day? | Legal. No uniqueness constraint except non-empty `id`. Duplicate ids both load (deterministic file order); Plan 133 pins uniqueness by test. |
| 8 | Selection determinism / seen-state | Selection is a pure deterministic day-filter over file order. **No seen-state, no cooldown, no persistence coupling**: `FactionWarChainRunnerState` (schemaVersion 1: chains, visitedLocations, cumulativeMoraleDelta) does not reference communiqués. §15 disposition: repetition is not a selector problem today because no selector/renderer consumes the corpus; Plan 133 remains data-only (§23.13) and reports this as a surfaced-content gap owned by a future host plan. |

## 1. Catalog shape at baseline

- File: `Assets/StreamingAssets/Data/faction_war_communiques.json`, `schema_version: 1`,
  18 entries, chronological order, ids `comm_d<day>_<faction>_<slug>`.
- Voices: `faction_central_garrison` ×7, `faction_rebuilders` ×6, `faction_ash_sign` ×4,
  `faction_forward_roster` ×2 (one of which, `comm_d593_forward_roster_ceasefire_toll`,
  attaches to `evt_d570` rather than the ceasefire chain).
- 10 of 18 entries carry `authorNote` (truth-layer annotations: "False", "True by
  accident", "Honest agnosticism", "True and central to the mystery", etc.).
- Chains referenced: 8 distinct (`evt_d495`, `evt_d517`, `evt_d533`, `evt_d545`,
  `evt_d570`, `evt_d578`, `evt_d588`, `evt_d605`).
- MD5 snapshot of the baseline file captured before any edit
  (`/tmp/plan133_baseline.md5`); all 18 ids/texts are compatibility anchors and are
  preserved verbatim (pinned by test `Existing18Ids_PreservedVerbatim`).

## 2. The 18-entry inventory

Full per-entry inventory (id, chain, day, faction, public claim, hidden truth,
rhetorical mode, contradiction grouping, branch safety, consumers):
see **FACTION_WAR_COMMUNIQUE_BASELINE_MATRIX.md**.

## 3. Event-chain chronology and branch map

Full chain/stage inventory, branching analysis, and coverage gaps:
see **FACTION_WAR_EVENT_COMMUNIQUE_COVERAGE.md**.

Key branch findings (from `faction_war_events.json` + `FactionWarChainRunner`):

- **Invariant outcomes** (s2 fires regardless of choice — safe to comment publicly):
  span charges re-armed (d509), almshouse struck (d517), waystation fee rescinded
  (d522), Exchange guard detail formed (d524), grain-silo inspection post stands
  (d533), ration plaza strike happens (d545 — explicitly not preventable), twelve
  walk out of the fracture meeting (d552), column slows for the Roster (d570),
  shrine strike anomaly (d578), ceasefire talks + stand-down (d588), theory circulates
  at the cairn (d600), the fence holds its first line (d605).
- **Branch-sensitive facts** (excluded from static prose): which party the player gave
  the LN74 intercept to (d558); Denner's petition/disappearance (d503); whether the
  plaza queue was publicly warned, the stores moved, or the window kept silent (d541);
  whether the player exposed the pumphouse arrangement (d565); the player's push at
  the post-ceasefire checkpoint (d605 — the shipped d607 statement is already carefully
  worded to concede nothing).
- **Flag-gated chains** (`evt_p25_*`): may never have fired in a given campaign;
  excluded from all communiqué references (mechanical gate test added).
- **D/9 (`evt_d583_d9_reassessment`, `faction_black_ops`)**: clandestine detachment;
  canonically does not issue public statements. No communiqué authored for it.

## 4. Cross-surface differentiation check (§12)

- Radio (33 entries): immediate, spoken, scheduled, signal-aware (Understory relay,
  Continuity bulletins, Roster wire, shrine transmissions). Communiqués are formal
  statements — new entries must not reuse radio phrasings. Overlap points audited:
  d481/d484 (tally), d488 (grain rebuttal), d504/d507 (conscription), d534 (exchange
  order), d546/d547 (plaza), d589/d590 (ceasefire), d571/d596/d606 (roster notices).
- Journal (26): private witness (Mira, Denner, Adaeze, Toma, Sella, Fennick…).
  Communiqués record institutional claims only; no journal voice reused.
- Dialogue (40): overheard human texture at locations. No communiqué text overlaps.
- Location overrides (20): place-state presentation; communiqués may discuss places,
  never activate overrides (no coupling exists — verified).
- Epilogue: no communiqué consumer (verified) — no ending authority risk.

## 5. Downstream consumers at baseline

| Consumer | Coupling |
|---|---|
| `FactionWarContentCatalog` / loader | owns DTO + load; tolerant of missing files |
| `GetCommuniquesForFaction(faction, day)` | the only query API; day-filter, file order |
| `ContentUtilizationScanner` | maps `faction_war_communiques.json` → `FactionWarContentCatalog` consumer (stays satisfied) |
| `FactionWarContentCatalogTests` / `DataWiringIntegrationTests` | count `> 0` only — no count pin (safe for expansion) |
| Any player-facing renderer | **none exists** (FactionsPanel fixture text is unrelated) |
| `authorNote` readers | **none** |

## 6. Save behavior at baseline

Communiqués have no seen/unlock state in any save section (verified against
`FactionWarChainRunnerState` and the `year_of_ash` envelope consumers). Consequences
for §16: old saves are trivially valid; content reload is a plain catalog reload;
no campaign section changes in Plan 133. Exact-day unlock behavior is pinned by test
(`GetCommuniquesForFaction` day boundary: `day == entry.day` includes, `day-1` excludes).
