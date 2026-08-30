# Plan 22 — Foundry, Greenhouse & Production: The Industrial World

> **Theme:** The shelter's heavy industry and food production are mechanically deep (cupola
> metallurgy with labor strikes, 5-stage crops, apiculture, salt mining) but their *content* —
> products, casts, crop varieties, faction entanglement — is thin. This plan fills the
> production economy.
>
> **Key evidence (verified):** `foundry_items.json` = 16, `foundry_production.json` = 11,
> `foundry_faction.json` = 6 internal divisions / 8 relationships; `greenhouse_items.json` = 14;
> Core has `SilentFoundrySystem` (+Heat +TreatyLabor), `SaltMineExtractionSystem`,
> `ApicultureSystem`, `GreenhouseSystem` — all live.

---

## Task 22A — Foundry casting & ordnance catalog (11 → 25 products)

**Goal:** Expand what the Silent Foundry can cast so the cupola, crucible, and labor-strike
systems have a real production ladder.

**Files:** `foundry_production.json`, `foundry_items.json`, `recipes.json` (mold recipes),
read-only `SilentFoundrySystem.cs`, `SilentFoundrySystem.Heat.cs`, `SilentFoundryCatalog.cs`.

**Substeps:**
1. Read `SilentFoundrySystem` + `SilentFoundryCatalog` to learn the product schema (mold, material, heat curve, labor, output).
2. Map the 11 existing products to material/tier to find the empty rungs (tooling? structural? ordnance?).
3. Author 6 tooling products (dies, drills, crucible spares) that feed the workshop (04) and maintenance.
4. Author 5 structural products (roof-armor plate for 19B, shoring beams for 11A, blast-door fittings).
5. Author 3 ordnance products (mortar shells for cloud-seeding #17, ball shot, casings) — grounded, no superweapons (tone).
6. Respect `SilentFoundrySystem.Heat` (heat curves) and `TreatyLabor` (labor is treaty-bound — products may need treaty labor, tying to 16C).
7. Ensure inputs resolve to real `item_*` ores/scrap; outputs to real or new `item_*`.
8. Validate ids; data-integrity selftest.
9. xUnit: product cast consumes material+heat+labor, treaty-labor gate enforced, output granted.
10. Balance sim: foundry output must not trivialize scavenging; cross-tool QA (heat×labor×material coupled).

**Next steps:** ordnance feeds the cloud-seeding mortar (#17); a foundry "great casting"
milestone event; export castings as a trade good (13A).

---

## Task 22B — Greenhouse, apiculture & salt: food-production depth

**Goal:** Deepen the food loop — crops, bees, and salt — so survival eating has variety,
seasonality (19C), and spoilage risk instead of one ration number.

**Files:** `greenhouse_items.json`, a crop catalog (confirm loader — `GreenhouseExpansionCatalog.cs`),
`recipes.json` (cooking/preservation), read-only `GreenhouseSystem.cs`, `ApicultureSystem.cs`,
`SaltMineExtractionSystem.cs`, `CulinaryRationCatalog` (narrative docs show one exists).

**Substeps:**
1. Read `GreenhouseSystem` + `GreenhouseExpansionCatalog` + `ApicultureSystem` + `SaltMineExtractionSystem` for their schemas.
2. Inventory the 14 greenhouse items; map crops to the 5-stage growth model.
3. Author 8 new crops (hardy tuber, ash-wheat, mushroom culture for 09A, nutrient algae, medicinal herb) with season sensitivity (19C) and blight risk.
4. Author 4 apiculture products (honey, wax, propolis, mead-base) with real uses (food, candles, antiseptic, morale).
5. Author 3 salt products (preservation salt, trade salt, saline for 09 medical) tying salt mining to food preservation.
6. Author 10 preservation/cooking recipes (pickling, smoking, canning, drying) that convert perishables to stable rations — spoilage pressure.
7. Wire crop blight to a `DiseaseSystem`-adjacent event and to seasonal state.
8. Validate ids; data-integrity selftest.
9. xUnit: crop growth by season, blight event, preservation converts perishable→stable, apiculture yield.
10. Balance sim: food variety must reduce monotony without removing scarcity; cross-tool QA.

**Next steps:** a harvest-festival morale event; famine events when a blight + a bad season
stack; cookbook codex (17C).

---

## Task 22C — Foundry faction politics & labor

**Goal:** Make the Silent Foundry's 6 internal divisions and 8 relationships a political
micro-game: labor disputes, faction leverage, and treaty obligations around the furnace.

**Files:** `foundry_faction.json`, `foundry_accords.json` (16C overlap),
`foundry_treaty_consequences.json`, read-only `SilentFoundrySystem.TreatyLabor.cs`,
`FoundryActionSurface.cs`, `DutyRosterSystem` (labor).

**Substeps:**
1. Read `foundry_faction.json` (6 divisions / 8 relationships / 7 tags) + `TreatyLabor` to map the political model.
2. Author the 6 divisions as named factions-within (the cupola masters, the mold-makers, the stokers, the assayers, the carters, the apprentices) with distinct interests.
3. Author 8 labor-dispute events (a stoker walkout, a mold-makers' grievance, an apprentice injury blame) with resolution choices.
4. Wire disputes to `TreatyLabor` — some labor is treaty-bound, so a dispute becomes a diplomatic incident (16C).
5. Author 4 labor-strike escalations that halt production (22A) until resolved — real cost.
6. Add a favor economy: side with a division → production bonus + another division's resentment.
7. Ensure disputes surface through the duty roster (labor) and the faction panel.
8. Validate ids; data-integrity selftest; dialog-graph lint.
9. xUnit: dispute trigger, strike halts production, resolution restores, standing delta.
10. Foundry selftest (`SilentFoundryHeadlessDemo`) green.

**Next steps:** a foundry election/coup arc; the foundry as a Verdict defendant (15B) for wartime labor.
