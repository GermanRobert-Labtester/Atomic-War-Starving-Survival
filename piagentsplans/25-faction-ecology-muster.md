# Plan 25 — Faction Ecology & the Muster: Politics, War & the Gathering

> **Theme:** The faction *ecology* — how factions live, feud, trade, and gather — plus the
> Muster endgame. Faction-war content exists (unwired, 06C) but peacetime faction life and the
> Muster's witness/epilogue content are thin. This plan builds the political world.
>
> **Key evidence (verified):** `muster_witnesses.json` = **3 witnesses**; `muster_epilogues.json`
> = 12; `holdfast_factions.json` = 3 actions; `standing_record_factions.json` = 1 action;
> `foundry_faction.json` = 6 divisions; Core `Muster/` has `CoalitionCampSystem`,
> `ScavengerGuildSystem`, `IronRaidersSystem`, `HydroBaronsSystem`, `WitnessCatalog` — all live.

---

## Task 25A — Peacetime faction life: guilds, barons & raiders

**Goal:** Give the Muster's faction systems (`ScavengerGuild`, `HydroBarons`, `IronRaiders`,
`CoalitionCamp`) peacetime content — trade, disputes, culture — so factions are societies
before they're war actors (06C).

**Files:** faction data (`holdfast_factions.json`, `standing_record_factions.json`,
`foundry_faction.json`, `faction_lore.json`), read-only `ScavengerGuildSystem.cs`,
`HydroBaronsSystem.cs`, `IronRaidersSystem.cs`, `CoalitionCampSystem.cs`.

**Substeps:**
1. Read the four Muster faction systems to learn each faction's model (resources, disposition, actions).
2. Note the action-count gap: holdfast 3, standing_record 1 — these factions have almost no authored behavior.
3. Author the Scavenger Guild's economy: salvage rights, territory claims, a finders'-fee system the player can join or fight.
4. Author the Hydro Barons' leverage: water control (ties to 18A brine + Plan 56 water goods), tolls, a water-blockade threat.
5. Author the Iron Raiders' code: what they take, what they spare, a parley option (non-combat paths exist) — grounded, not glorified.
6. Author 8 peacetime faction-action entries to close the action-count gap (trade offers, disputes, requests, threats).
7. Author 6 faction-culture flavor entries (how each buries its dead, feeds itself, marks territory) for the codex (17C).
8. Validate ids; data-integrity selftest; narrative-continuity across factions.
9. xUnit: each faction system resolves its authored actions; standing affects them.
10. Muster selftest (`MusterHeadlessDemo`) green.

**Next steps:** peacetime grievances become the *causes* of the faction war (06C); a faction
summit the player can attend; guild membership as a player path.

---

## Task 25B — The Muster gathering: witnesses & the coalition camp

**Goal:** Expand the Muster endgame gathering — only **3 witnesses** today — into a real
assembly where the people the player met (or failed) testify.

**Files:** `muster_witnesses.json`, `muster_epilogues.json`, `characters.json` (witness NPCs),
read-only `MusterSystem.cs`, `CoalitionCampSystem.cs`, `WitnessCatalog.cs`, `QuestApproach.cs`.

**Substeps:**
1. Read `MusterSystem` + `CoalitionCampSystem` + `WitnessCatalog` to learn witness/epilogue schema and how witnesses are selected.
2. Map which campaign NPCs/factions should be able to appear as witnesses (20B NPCs, 06C survivors, 12A raised children).
3. Author 12 new witnesses (a spared warlord, a rescuee from 24B, a defrauded claimant from 18A, a foundry striker from 22C) — each testimony reflects a specific player choice.
4. Give each witness 2 testimony variants (you helped them / you failed them) keyed to world flags.
5. Author 4 coalition-camp scenes (the gathering itself: arrivals, old enemies meeting, a shared meal, a confrontation).
6. Wire witness selection to the campaign's actual flag state (who's alive, what you did).
7. Ensure testimony feeds the epilogue matrix (15A) as named remembrances.
8. Validate ids/flags; data-integrity selftest; narrative-continuity; dialog-graph lint.
9. xUnit: witness selection by flags, testimony variant by player history, epilogue integration.
10. Muster UI test (`--muster-uitest` exists) still green; extend if the witness surface changed.

**Next steps:** a witness the player can *prevent* from testifying (moral cost); the Muster as
the Verdict's jury (15B convergence); a "no one came" bad-faith ending.

---

## Task 25C — Faction-war escalation & the road to the Muster

**Goal:** Author the *escalation content* that carries factions from peacetime friction (25A)
through the war arc (06C) to the Muster (25B) — the connective tissue of the late game.

**Files:** `faction_war_events.json` (06C, read), `events.json` (new escalation events),
`faction_lore.json`, `foundry_accords.json` (16C), read-only `FactionWarSystem.cs`,
`RegionalTreatySystem.cs`, `MusterSystem.cs`.

**Substeps:**
1. Read 06C's faction-war chains + `FactionWarSystem` to map the war's beats (this task *surrounds* the war, doesn't re-author it).
2. Read `RegionalTreatySystem` to identify the treaty-break points that trigger escalation.
3. Author 6 pre-war escalation events (a border incident, a seized caravan, a poisoned-well accusation, a failed summit) that raise tension measurably.
4. Author 6 mid-war events that reference 06C's specific strikes/battles (the player lives through the war, not just reads it).
5. Author 4 war-weariness events (deserters arrive, a famine spreads, a peace faction forms) that push toward the Muster.
6. Author 2 paths to the Muster: a negotiated gathering (diplomacy held) vs. a victor's muster (one faction dominant) — different 25B scenes.
7. Wire escalation to treaty state (16C) and war state (06C) via real flags; no orphans.
8. Validate ids; data-integrity selftest; narrative-continuity (this is the most continuity-sensitive task — dates/factions must align with 06C).
9. xUnit: escalation trigger, tension accrual, war-event gating by 06C state, Muster path selection.
10. Full narrative-continuity + dialog-graph lint across war + treaty + muster; cross-tool QA (multi-system).

**Next steps:** this completes the late-game narrative spine (25A peace → 25C escalation → 06C
war → 25B muster → 15A epilogue); a "prevented the war" pacifist ending as the hardest path.
