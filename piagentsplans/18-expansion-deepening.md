# Plan 18 — Expansion Deepening: Holdfast, Standing Record, Crossing & Verdict

> **Theme:** The four charter expansions are implemented but their catalogs are thin and
> lopsided. This plan deepens each expansion's *content* within its existing systems — no new
> expansion scaffolding.
>
> **Key evidence (verified):** Holdfast: 38 locations / 40 items / **10 quests** / 3 faction
> actions. Standing Record: 14 layouts / 38 memories / **10 quests** / 1 faction action.
> Crossing: 13 locations / 11 items / **12 quests** / 10 encounters. Verdict: 4 locations /
> 6 NPCs / **8 questlines** / 15 items.

---

## Task 18A — Holdfast depth: ice road, census & brine quests (10 → 22)

**Goal:** Flesh out the Holdfast expansion's signature systems (ice road, census claims, brine
water) with quest content that makes the frozen-settlement economy a real arc.

**Files:** `holdfast_quests.json`, `holdfast_locations.json`, `holdfast_items.json`,
`holdfast_flavor.json`, read-only Holdfast host + `BrineWaterSystem`, `CensusClaimSystem`,
ice-road systems.

**Substeps:**
1. Read `holdfast_quests.json` + the Holdfast systems (brine, census, ice road, cluster) to learn quest grammar and which mechanics are quest-addressable.
2. Map the 38 holdfast locations to roles (settlement, ice-road stop, brine works, hazard) to find quest anchor points.
3. Author 4 ice-road quests (open the road before thaw, clear a blockage, escort a salt convoy, race a rival).
4. Author 4 census-claim quests (verify a claimant, expose a fraud, defend a legitimate claim, a disputed inheritance).
5. Author 4 brine-water quests (a brine-sickness outbreak, a salter's strike, a contaminated intake, a water-rights dispute) — ties to `BrineWaterSystem` + Plan 112/09A waterborne disease.
6. Author 2 settlement-crisis quests using `holdfast_flavor.json` factions.
7. Wire quest rewards to real `holdfast_items.json` / standing; no invented rewards.
8. Validate ids; data-integrity selftest; dialog-graph lint for quest flags.
9. xUnit: each quest loads, progresses, resolves; flag producers/consumers balanced.
10. Holdfast selftest green (`--holdfast-*` gates).

**Next steps:** Holdfast trade sessions (HoldfastTradeSession) get quest-driven price shocks;
an ice-road "last convoy before thaw" timed event.

---

## Task 18B — Standing Record depth: site memories & layouts (10 → 22 quests)

**Goal:** Deepen the Standing Record (memory-of-place) expansion so its 14 layouts and 38
memories support a real investigative questline about what each site *was*.

**Files:** `standing_record_quests.json`, `standing_record_memory.json`,
`standing_record_layouts.json`, read-only `StandingRecordEngine.cs`, `LocationMemorySystem.cs`,
`LocationLayoutSystem.cs`, `SiteEncounterSystem.cs`.

**Substeps:**
1. Read `StandingRecordEngine` + `LocationMemorySystem` + `LocationLayoutSystem` to learn how memories/layouts/site-encounters interlock.
2. Inventory the 14 layouts and 38 memories; map which sites are memory-rich vs. bare.
3. Author 12 new site memories that contradict or complicate each other (unreliable-record texture).
4. Author 6 "what happened here" investigation quests (reconstruct a site's last day from 3 memories).
5. Author 4 layout-recovery quests (find the real floor plan vs. the official one — a pre-war cover-up).
6. Author 2 memorial quests (a site that deserves a marker; ties to `MemorialSystem`).
7. Make recovered memories feed the in-game codex (17C) and 2 feed Verdict evidence (15B).
8. Validate ids; data-integrity selftest; narrative-continuity.
9. xUnit: memory collection → reconstruction resolution; layout unlock; codex integration.
10. Standing Record selftest green.

**Next steps:** a "sites reconciled" meta-tracker; conflicting-testimony mechanic feeding the
Verdict's evidence-weighing.

---

## Task 18C — Crossing & Verdict content balance (12 → 20 / 8 → 16 quests)

**Goal:** Round out the Crossing (arbitration/border) and Verdict (tribunal) expansions, whose
quest/location counts lag their system complexity.

**Files:** `crossing_quests.json`, `crossing_encounters.json`, `crossing_locations.json`,
`verdict_questlines.json`, `verdict_locations.json`, `verdict_npcs.json`, read-only
`CrossingArbitrationSystem.cs`, `ReckoningSystem.cs`, `VerdictHostSession`.

**Substeps:**
1. Read `CrossingArbitrationSystem` + `crossing_*` catalogs to learn arbitration/encounter grammar.
2. Read `ReckoningSystem` + `verdict_*` catalogs to learn questline/NPC grammar.
3. Author 8 new Crossing quests (border disputes, asylum claims, contraband arbitration, a contested crossing right).
4. Author 4 new Crossing encounters/crises (a mass crossing event, a blockade, a quarantine, a bribe attempt) extending the 10/5 base.
5. Author 8 new Verdict questlines (an alibi to verify, a witness to find, a record to authenticate, a prior verdict to appeal).
6. Author 3 new Verdict NPCs (a defense clerk, a machine-cult believer, a records keeper) with distinct voices.
7. Wire Crossing arbitration outcomes to faction standing (16C) and Verdict evidence to 15B dossiers.
8. Validate ids across both expansions; data-integrity selftest; dialog-graph lint.
9. xUnit: arbitration resolution paths, verdict questline progression, standing deltas.
10. Crossing + Verdict selftests green (`--crossing-*`, `--verdict-*`).

**Next steps:** Crossing becomes the physical border where faction-war (06C) refugees arrive;
Verdict appeals as a post-reckoning epilogue branch (15A).
