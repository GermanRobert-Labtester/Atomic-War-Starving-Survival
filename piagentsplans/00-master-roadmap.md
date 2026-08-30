# ASHFALL — Master Roadmap: 20 Evidence-Grounded Next Steps

> Method: forensic inspection of `Assets/Ashfall.Core/`, `src/`, `Assets/StreamingAssets/Data/`,
> `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` (53 capabilities: 47 live, 4 data-only, 2 partial,
> 0 stubs), `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` (white space §34), and `AGENTS.md` known
> issues (H1–H12).
>
> **Central finding:** ASHFALL is system-complete but content-starved at specific seams. The
> highest-value work is (a) closing four remaining verification/data-hygiene gaps, then
> (b) pouring content into already-live, underused systems, then (c) activating the three
> documented white-space mechanics. Do **not** invent new parallel systems (registry §21, §23, §24).
>
> **Registry drift warning:** the canon registry (audit 2026-08-20) overstates two expansion
> targets that are already done (`pharma_recipes.json` has 25 recipes) and one issue that is
> already fixed (`AirlockSecuritySystem.cs` GetHashCode — no occurrence in source today).
> Verify before executing any plan.

---

## 1. Immediate blockers (P0)

None block development outright — tests are green and migration is complete. The P0 list below
is *verification and data-authority hygiene* that should precede heavy content work so new
content lands on a trustworthy base.

---

## 2. The 20 next steps

### P0 — Verification & data hygiene (do first)

---

### 1. NeedsSystem & RadiationSystem save round-trip tests
- **Type:** VERIFY | **Priority:** P0 | **Risk:** LOW
- **Why now:** Known issue **H10** — tick behavior is covered (58 tests) but save/load
  round-trips are not. These two systems sit at the center of the entire survival loop; a
  silent capture/restore gap corrupts every campaign.
- **Player value:** Invisible until it breaks — then it loses saves.
- **Evidence:** `AGENTS.md` H10; `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`.
- **Plan:** `01-needs-radiation-save-roundtrip-tests.md`
- **DoD:** Round-trip tests (capture → mutate → restore → assert) for both systems green;
  coverage of fallback decay path in `HoldfastRuntimeSession.TickDay`.

### 2. JournalSystem core behavior tests
- **Type:** VERIFY | **Priority:** P0 | **Risk:** LOW
- **Why now:** Known issue **H11** — 6 Core files, save-store integrity tested, but core
  journal behavior untested. Journal is the narrative spine (codex + 196 narrative docs).
- **Evidence:** `AGENTS.md` H11; `Assets/Ashfall.Core/Journal/`.
- **DoD:** xUnit suite over entry add/query/unlock, codex paging, flag-gated entries;
  save round-trip via existing `JournalSaveStore` contract.

### 3. Eliminate 13 bare `catch { }` blocks in catalog loaders
- **Type:** FIX | **Priority:** P0 | **Risk:** LOW
- **Why now:** Known issue **H4** — `YearOfAshCatalogLoader.cs` (7) and
  `VerdictCatalogLoader.cs` (3) silently swallow parse/IO errors, meaning corrupt or
  misspelled JSON fails invisibly instead of surfacing in `--data-integrity-selftest`.
- **Player value:** Prevents "content I authored silently never appears" — a direct threat
  to every content plan below.
- **Evidence:** `AGENTS.md` H4.
- **Plan:** `02-loader-bare-catch-hardening.md`
- **DoD:** 0 bare catches in both loaders; failures route through `ILog` and the data
  integrity selftest; regression test proving malformed JSON produces a logged error.

### 4. `schema_version` sweep across data authority
- **Type:** DATA hygiene | **Priority:** P0 | **Risk:** LOW
- **Why now:** AGENTS.md flags that only ~35 of ~280 JSON files carry `schema_version`.
  Content plans #8–#17 add many new files; establish the convention *before* scaling.
- **Plan:** `03-schema-version-data-sweep.md`
- **DoD:** All root-level catalogs carry `schema_version`; a validator gate fails CI on
  missing versions for new files; snake_case naming confirmed by `CatalogIntegrityValidator`.

---

### P1 — Content into underused live systems (highest player leverage)

All five systems below are `LIVE_CORE + LIVE_GODOT` with thin catalogs — pure DATA work with
zero new Core code, the cheapest possible content multiplier (registry §20).

### 5. Workshop relic blueprint expansion (6 → 30 relics)
- **Type:** AUTHOR CONTENT | **Priority:** P1 | **Risk:** LOW
- **Why now:** `WorkshopReverseEngineeringSystem` is fully implemented; `relic_recipes.json`
  has exactly **6** entries (verified). Registry §20 lists this as a top safe extension.
- **Player value:** 30 pre-war schematics turn the workshop into a mid-game progression spine.
- **Plan:** `04-relic-blueprint-expansion.md`
- **DoD:** 30 relics with snake_case ids resolving against `items.json`; data-integrity
  selftest 0 errors; loader binding test.

### 6. Vinyl record catalog (1 → 20 albums)
- **Type:** AUTHOR CONTENT | **Priority:** P1 | **Risk:** LOW
- **Why now:** `VinylMoraleSystem` (playback, shelter-wide morale buffs, host session, panel)
  is fully wired, but the entire game has **one** generic `item_vinyl_collection`.
- **Player value:** Collectible morale economy + scavenging motivation + diegetic pre-war culture.
- **Plan:** `05-vinyl-record-catalog.md`
- **DoD:** 20 albums across 4 genres with distinct buff profiles; items + vinyl catalog +
  integrity gate green.

### 7. Trade tell line expansion (4 → 40 lines)
- **Type:** AUTHOR CONTENT | **Priority:** P1 | **Risk:** LOW
- **Why now:** `TradeTellEngine` + 5 trust bands are live; `trade_tell_lines.json` contains
  only **4** lines (verified), so every negotiation reads identically.
- **Player value:** Barter becomes a readable skill instead of a repeated sentence.
- **DoD:** 40 lines covering all trust bands × factions; `ashfall-write` tone rules; integrity pass.

### 8. Deep-strata excavation expeditions
- **Type:** AUTHOR CONTENT + WIRING | **Priority:** P1 | **Risk:** MEDIUM
- **Why now:** `ExcavationSystem` (depth, shoring, cave-ins) is fully coded but used only for
  starting-room unlocks (registry §20). Data-driven vault expeditions reuse it without new Core.
- **Player value:** A new high-risk/high-reward expedition tier: buried command vaults.
- **DoD:** 5+ vault sites in data, wired through expedition dispatch; cave-in hazard events;
  save round-trip.

### 9. Wildlife trapping active-management loop
- **Type:** AUTHOR CONTENT + MINOR CORE EXTENSION | **Priority:** P1 | **Risk:** MEDIUM
- **Why now:** `WildlifeTrappingSystem` (deadfalls, snares, butchery, rad-taint) runs as a
  background tick with no player agency.
- **Player value:** Turns a passive drip into a bait/lure/yield decision loop tied to hunting skill.
- **DoD:** Bait crafting recipes, mutated-beast lures, trap-line panel surface; determinism
  via `ISeededRng`; tests.

---

### P2 — Integration & depth (cross-system value)

### 10. Sky-armor siege events
- **Type:** AUTHOR CONTENT + WIRING | **Priority:** P2 | **Risk:** MEDIUM
- **Why now:** `SkyLayerArmorSystem` (cell-grid roof armor, kinetic penetration) is complete
  but no hazard events test it (registry §20). Pair with `OrbitalHarrowTelemetrySystem`
  warnings for an observe→shore→survive loop.
- **DoD:** Artillery/debris strike events that damage specific roof cells; telemetry
  forecast hook; repair material sink.

### 11. Cipher / number-station decode quests
- **Type:** AUTHOR CONTENT + MINOR WIRING | **Priority:** P2 | **Risk:** LOW
- **Why now:** `SignalIntelligenceCatalog` holds cipher dictionaries, signal logs, wiretap
  transcripts — data exists, no interactive hook (registry §20).
- **Player value:** Radio mastery pays off in hidden bunker coordinates — deepens the
  radio→expedition pipeline.
- **DoD:** 3+ multi-stage decode quests; rewards are real `loc_` entries; narrative lint pass.

### 12. Generational arcs: schooling, apprenticeship, adoption
- **Type:** AUTHOR CONTENT | **Priority:** P2 | **Risk:** LOW
- **Why now:** `CohortSystem` + `GenerationalLineageExtension` + `ApprenticeshipSystem`
  exist but only fire in late multi-year play; no narrative content surrounds them.
- **DoD:** Schooling curricula events, orphan adoption arcs, apprenticeship assignments
  surfaced via duty roster; quest + flag wiring.

### 13. Mutant combat behaviors + vehicle chase encounters
- **Type:** AUTHOR CONTENT | **Priority:** P2 | **Risk:** MEDIUM
- **Why now:** `TacticalCombatSystem` (5 lanes, 7 stances, ballistics) and
  `ExpeditionVehicleSystem` are live; registry recommends specific enemy behaviors and
  chase encounters rather than new combat systems (§21 moderate-saturation rules).
- **DoD:** 5+ behavior-defined enemies in `combat_catalog.json`; chase encounter type using
  vehicle stats; combat selftest green.

### 14. Debt-collection bounty raids
- **Type:** AUTHOR CONTENT + WIRING | **Priority:** P2 | **Risk:** MEDIUM
- **Why now:** `LedgerDebtSystem` (compound debt, collateral forfeiture) is live; forfeiture
  currently resolves abstractly. Consequence made physical = raid pressure.
- **DoD:** Default → escalating collector visits → raid hook into combat; economy balance sim
  (`ashfall-balance-sim`) before merge.

### 15. Weather-specific crisis events
- **Type:** AUTHOR CONTENT | **Priority:** P2 | **Risk:** LOW
- **Why now:** 22 weather states exist; most have no bespoke crisis event. Pure data work into
  `events.json` family using existing weather keys.
- **DoD:** 1–2 crisis events per major weather kind (fallout storm, black rain, EMP, acid
  snow, bio-fog); reachability lint.

---

### P3 — White space & structural (strategic)

### 16. Tactile mini-game interfaces (White Space 1)
- **Type:** NEW FEATURE (host UI) | **Priority:** P3 | **Risk:** MEDIUM
- **Why later:** Genuine open design space (atlas §34.1) but host-UI-heavy: oscilloscope radio
  tuning, circuit-breaker rerouting boards, safe tumblers. Build after P1 content lands so the
  mini-games gate real rewards.
- **DoD:** One mini-game (radio oscilloscope) fully playable, Core logic engine-agnostic,
  audio cues via `AudioEventBridge`, snapshot test.

### 17. Cloud-seeding atmospheric countermeasures (White Space 2)
- **Type:** CORE EXTENSION | **Priority:** P3 | **Risk:** HIGH
- **Why later:** The only proposed plan that adds a new Core system (foundry-cast dispersion
  shells → `WeatherSystem` modification). Touches the weather hub — highest blast radius of
  the list; schedule when verification debt (#1–#4) is closed.
- **DoD:** Roof mortar + shell recipes + 48h weather suppression with deterministic rolls;
  balance sim; save migration path.

### 18. Shelter interior decoration & trophies (White Space 3)
- **Type:** NEW FEATURE (DATA + MINOR WIRING) | **Priority:** P3 | **Risk:** MEDIUM
- **Why later:** Room-level decor slots (posters, plaques, trophies) granting localized morale;
  depends on `ShelterAssignmentSystem` + `MemorialSystem`. Player-visible but cosmetic-first.
- **DoD:** Decor item category, per-room slots in UI, morale modifiers through `NeedsSystem`,
  snapshot diff.

### 19. Main.cs partial-file decomposition (H7)
- **Type:** REFACTOR | **Priority:** P3 | **Risk:** HIGH
- **Why later:** ~7k-line orchestrator with 38 Setup / 30 Save / 18 Flush triads; triad-drift
  risk grows with every content plan above. Behavior-preserving decomposition into per-domain
  partials. Do **not** mix with feature work.
- **DoD:** Zero behavior change; all selftests + snapshot diffs identical; one partial per
  domain; use `ashfall-decompose-godot` plan first.

### 20. Duplicate `WornGear` consolidation (H2)
- **Type:** REFACTOR | **Priority:** P3 | **Risk:** MEDIUM
- **Why later:** `WornGear` exists in both `Inventory/` and `Radiation/`; the
  `Radiation.WornGear.FromInventory` bridge is the sanctioned conversion point. Consolidation
  is opportunistic — do it when next touching either system (e.g. during #9 or #13).
- **DoD:** Single canonical class; bridge removed; `InventoryGearBridgeTests` updated and green.

---

## 3. NOW / NEXT / LATER

### NOW (next 1–3 tasks)
1. **#1** Needs/Radiation save round-trips (H10)
2. **#3** Loader bare-catch hardening (H4)
3. **#5** Relic blueprint expansion (cheapest big content win)

### NEXT (dependency-following)
4. #4 schema_version sweep → 5. #6 vinyl catalog → 6. #7 trade tells →
7. #2 JournalSystem tests → 8. #8 excavation expeditions → 9. #15 weather crisis events →
10. #11 cipher quests

### LATER (strategic)
11–15. #9 trapping loop, #10 sky-armor sieges, #12 generational arcs, #13 combat behaviors,
#14 debt raids → 16–18. white-space features #16/#17/#18 → 19–20. refactors #19/#20

---

## 4. What NOT to do yet

- **Do not add another pharma recipe batch** — `pharma_recipes.json` already has 25 entries
  (registry §28 guidance is stale).
- **Do not build a sanity meter, hunger system, weather system, epidemic system, radio tuner,
  or debt system** — all exist and are saturated (registry §21/§23). Extend, don't duplicate.
- **Do not touch `Assets/_Game/` or quarantined Unity code** — Unity direction is closed.
- **Do not decompose `Main.cs` mid-feature** — #19 must be a standalone, behavior-preserving task.
- **Do not start white-space feature #17 (weather countermeasures) before P0 verification
  items close** — it modifies the weather hub that every survival system reads.
- **Do not create new id prefixes** outside the `CatalogIntegrityValidator` master list.

## 5. Suggested follow-up skills

- Plans #5–#8, #11–#15: run `ashfall-data-add` + `ashfall-write`, gate with
  `--data-integrity-selftest`.
- Plans #9, #13, #14, #17: run `ashfall-balance-sim` before merge (≥2 coupled variables →
  cross-tool QA rule applies: implementer ≠ reviewer).
- Plan #19: run `ashfall-decompose-godot` first (read-only plan), then `ashfall-implement`.
- Any narrative content: `ashfall-narrative-continuity` + `ashfall-dialog-graph-lint`.
