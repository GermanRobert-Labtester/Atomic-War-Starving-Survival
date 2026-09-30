# ASHFALL — "THE WORLD MOVES WITHOUT YOU": Family Index & Boundary Sheet
**Status:** Story-director coordination sheet. **Proposal — not a claim, not an authorization.** 2026-09-29.
Covers four expansions (subjects 4–7 of the user's list). Year Two (Days 361–720) is a sibling plan set; this sheet records how they meet.

| # | Expansion | Prose plan | Integration plan | Package prefix |
|---|---|---|---|---|
| 4 | **The Living Region** — settlements, refugees, prices shift with the war | `expansion_living_region_plan.md` | `.ai/plans/living-region-2026-09-29.md` | `LR-` |
| 5 | **The Long Line: Freight** — run a caravan company | `expansion_long_line_freight_plan.md` | `.ai/plans/long-line-freight-2026-09-29.md` | `LF-` |
| 6 | **The Drowned Coast** — boats, harbours, dives | `expansion_drowned_coast_plan.md` | `.ai/plans/drowned-coast-2026-09-29.md` | `DC-` |
| 7 | **The Plague Year** — outbreaks, quarantine politics, zoonotic vectors | `expansion_plague_year_plan.md` | `.ai/plans/plague-year-2026-09-29.md` | `PY-` |

All four are `STATUS: DRAFT — awaiting user approval`. Per-package derived plans (Year Two style) are **not** written: the user's earlier "I authorise the each seperate plan" was given for Year Two; this sheet does not assume it extends here.

> *"The world does not wait for you to look at it. That is the whole design, and it is the hardest
> thing in this family to build."*
>
> Four expansions about **events that proceed whether or not the player is watching**: a region that
> fails on its own schedule, a route that closes, a plague that spreads by vector and not by
> decision, a coast that drowns its own harbours. None of them is aimed at the player. All of them
> arrive at the player anyway. The shared feeling is not threat — it is *lateness*. You find out
> after it matters.

## 1. The one idea they share
The region already has five clocks: weather, wildlife, migration, faction war, embargo. Each expansion **reads them together and adds one small owner-respecting layer**; none adds a second economy, disease, expedition, or map authority.

## 2. What is shared (build once)

| Shared thing | Built by | Read by |
|---|---|---|
| **Canonical region vocabulary** (supply-tag set + one mapping file) | LR-P1 | PY (watch regions), LF (route regions), DC (`deep_coast` harbours) |
| **News grades** (Heard/Rumour · Told · Seen) as a presentation rule | LR-P7 | PY-P6, LF-P7 ledger, DC Board lines |
| **Market shock seam** (existing; idempotent) | exists (`src/Main.MigrationConsequence.cs`) | LR-P5, PY-P5 |
| **Read-only "blocked/closed" flags** (route, harbour, gate) | LF, DC, PY each expose one | LR Board |
| **Seeded RNG streams** (`CampaignStreamIds` forks) | integrator adds ids once | LR-P4, LF-P1, DC-P3, PY-P2 |
| **Additive nested save DTOs** (no new sections) | each owner | — |

## 3. Who owns what (no overlaps)

| Concern | Owner | Others |
|---|---|---|
| Population weights | `SeasonalHumanMigrationEngine` (`human_migration`) | LR nests settlement state here |
| Route contracts/runs | `PlayerTradeRouteSystem` (`trade_routes`) | LF nests company state here |
| Vessels/dives/harbours | `maritime` section (+ naval system, once E3 resolved) | DC nests here |
| Infection/outbreak | `DiseaseSystem` (`disease`) | PY nests watch + protocol here |
| Embargo/cordon | `TradeEmbargoSystem` | PY adds a trigger kind (additive) |
| War | `FactionWarSystem` | LR reads |
| Outpost/waystation custody | Year Two/signed custody | LF/DC/PY read-only |

## 4. Integrator-owned shared paths touched by more than one plan (serialise these)
`src/Main.CampaignOwners.cs` (LR-P5, PY day-owner) · `CampaignStreamIds` (LR, LF, DC, PY) · `CatalogIntegrityValidator.cs` (all four) · `Assets/StreamingAssets/Data/trade_embargoes.json` (PY-P5; LF reads) · `src/Host/TradeRouteHostSession.cs` / `src/Main.TradeRoutes.cs` (LF only) · `src/Host/DiseaseOutbreakHostAdapter.cs` (PY only) · `src/Main.NavalExpeditions.Integration.cs` and `ExpeditionHostSession.cs` naval line (DC only).

## 5. Dependencies and recommended order

```
LR-P0..P1 (vocabulary) ─┬─► PY-P1 (watch regions)
                        ├─► LF-P1 (route conditions)
                        └─► DC-P2 (harbour → region)
DC-P0 (naval owners, dive authority) ─► DC-P1..
LF-P0 (name collision, wagon fit)    ─► LF-P1..
PY-P0 (gate owner, difficulty hook)  ─► PY-P3/P4
LR-P6 (gate petitions) ◄── shares the gate owner with PY-P4  → one adapter, agreed in both P0s
Year Two P1 (horizon) ─► LF-P8 (season table), LR-P8 (profile hook)
```

**Recommended sequence:** run all four **P0 audits in parallel** (read-only, disjoint outputs) → build **LR** through P4 → **LF** (independent) → **PY** (needs LR-P1) → **DC** (after DC-P0 settles the two-naval-owner question). Soft cross-hooks (Health pillar, freight legs, yellow flag) ship dark until both ends exist.

## 6. Conflicts found while writing (Rule 6 — logged, not resolved here)

1. **Name collision:** "The Long Line" is already Expansion 11 (telephone trunk) in `expansion_11_the_long_line_creative_pack.md` and the master catalog. → DEC-LF-01.
2. **Plan 27 is stale:** it lists `MaritimeExplorationSystem` and `maritime_zones.json` as existing; both were retired by `claim-retire-maritime-exploration-duplicate-2026-09-29` (data archived). → DEC-DC-01.
3. **Two naval holders** (`Main._navalSystem`, `ExpeditionHostSession._naval`) and **no vessel persistence** visible. → DEC-DC-02, DC-P0.
4. **`MaritimeDiveSystem` has no `src/` reference** while `DiveInstanceRunner` does. → DEC-DC-06 (foreman).
5. **A trade-route "run" is a tariff debit** that always records on-time; `GoodsOut/GoodsIn` have no consumer; the risk engine is not called by the tick. → LF-P1/P2.
6. **Four unmapped region vocabularies.** → LR-P1.
7. **`EvolvingWorldDayOwner` ownership loop** re-owns only seeds authored to the new dominant faction (restoration, not conquest). → LR E7 / DEC-LR-05.
8. **`PatrolTerritoryAuthority` has no `src/` reference.** → LR E8.
9. **Trade route `season_end_day` (280–360)** ends the network as Year Two begins. → LF-P8 with Year Two P1.

## 7. Combined decision surface (all unsigned)
LR: DEC-LR-01…10 · LF: DEC-LF-01…10 · DC: DEC-DC-01…10 · PY: DEC-PY-01…10. **Blocking-first:** DEC-LF-01 (naming), DEC-DC-02/06 (naval and dive authority), DEC-LR-02 (where live state nests), DEC-PY-02/05 (watch home, embargo trigger).

## 8. Player-facing arc when all four exist (illustrative)
Deep winter: a cough at the Ferry is a rumour (PY). The Toll road is cut (LR); the house's salt wagon is late for a reason (LF); Cape Beacon closes its lamp and with it the sea-mark (DC/PY). Thaw: the meltwater carries the second strain along the same roads; the harbour that held is under a hand of water. At every step the player is looking at the same five clocks, read together.

## 9. What this sheet is not
Not a ledger entry, not a claim, not an approval. The foreman records the `INTEGRATION_PLANS.md` entry and `WORKTREE_OWNERSHIP.md` claims; the user sets `STATUS: APPROVED BY USER` on any plan that should ship.

---

## 10. Update (later 2026-09-29 pass) — the gate is three systems, and five plans need it
The "stranger at the door" owner (LR E11, PY E13) was located: `AirlockSecuritySystem` (decision point: Admit / Inspect / Quarantine / TurnAway / Defend), `DoorEncounterSystem` (80 authored knocks), `VisitorIntegrationSystem` (the stay, `SourceVisitorId` handoff). **One shared gate adapter** is now needed by LR-P6, PY-P3/P4, *The Quiet War* QW-P2, *Radio Free Ashfall* RF-P4 (signature visitors) and *Crews and Companions* (returning parties). Design it once, in `docs/expansions/expansion_new_ways_to_play_index.md` §4. Related: `docs/expansions/expansion_new_ways_to_play_index.md` for subjects 8–12.

---

## The deeper layer — the family as a shape (second prose pass)

*(Second prose pass, non-contractual: texture and writing guidance only — not a claim, not an
authorization. The shared-silences register below is unchanged; the fragments are content
candidates, not new recorded questions.)*

**The second layer.** The shared feeling of this family is not threat — it is *lateness*. You find
out after it matters. The hardest thing here to build is the honesty of a world that is not aimed
at the player and arrives anyway: the world is not ignoring you, it is not considering you either.
Five clocks run — weather, wildlife, migration, war, embargo — and none of them is yours.

**What the family leaves between its members.**

> "A cough at the Ferry is a rumour. The Toll road is cut. The salt wagon is late for a reason. Cape Beacon closes its lamp."

> "Thaw: the meltwater carries the second strain along the same roads that carried the first news."

> "War tension is an input. The war is never a subject. That is what makes the family work."

*(Texture only. The silences below are the register; nothing here adds to them.)*

---

## What this family refuses to answer (shared silences — cross-expansion)

Shared across all four and only safe while *none* of them fills it. See
`.ai/plans/OPEN_MYSTERY_INDEX_2026-09-29.md` §3.

- **Where the redirected people go.** *The Living Region* conserves population at the gate;
  *The Plague Year* counts its dead. Neither ledger follows anyone past its own boundary.
- **Whether the world knows the shelter is there.** Regional pulse, route state, outbreak stage and
  waterline are all pure or read-only with respect to the player. The world is not ignoring you. It
  is not considering you either.
- **What caused the war that moves all of this.** War tension is an input. The war is never a
  subject. That is deliberate and it is what makes the family work.
- **Why everything here is *late*.** The shared feeling is lateness and no mechanism explains it.
  The world reports; the player arrives second.
