# Plan 20 — Wasteland Inhabitants: Field Guide, Settlements & Random Encounters

> **Theme:** Who and what lives out there. The world has fauna (trapping), flora (greenhouse),
> factions, and NPCs, but no unified *inhabitant* layer — no field guide, thin random
> encounters, and few named wasteland settlements. This plan populates the world.
>
> **Key evidence (verified):** `characters.json` = 36; `WildlifeTrappingSystem` + 10A bestiary;
> `door_encounters.json` = 68; `GreenhouseSystem` crops; `FactionRadioEngine`; no dedicated
> wasteland-settlement or field-guide catalog exists.

---

## Task 20A — Mutated flora & fauna field guide (codex)

**Goal:** Author a discoverable field guide to the wasteland's mutated life — part lore, part
*mechanical intel* (knowing a beast's behavior helps you trap/fight/avoid it).

**Files:** new `field_guide.json` (codex-backed), ties to `WildlifeTrappingSystem` (13B),
`combat_catalog.json` (10A), `GreenhouseSystem`, read-only `JournalCodex.cs`.

**Substeps:**
1. Inventory every fauna/flora entity across trapping (13B), combat bestiary (10A), and greenhouse to catalog what exists.
2. Read `JournalCodex` to reuse its entry/unlock structure for the field guide.
3. Author 20 fauna entries (what it is, behavior, danger, how to trap/fight/avoid, rad-taint note) — grounded mutations only.
4. Author 12 flora/fungus entries (edible vs toxic vs medicinal vs blight) tied to greenhouse/foraging.
5. Make each entry's "intel" mechanically true: the trap affinity / combat behavior it describes matches the system values (verify against 10A/13B data).
6. Gate entries behind first encounter/sighting (kill it, trap it, or survive it).
7. Add a field-guide section to the codex UI (reuses codex rendering).
8. Validate ids; data-integrity selftest; `DataRuleComplianceTests` (no real species misrepresented in a misleading way — it's fiction, keep it grounded).
9. xUnit: entry unlock on encounter, intel accuracy cross-check against system data.
10. Snapshot-diff the codex/field-guide panel.

**Next steps:** a completed field guide as a tradeable "surveyor's manual"; field-guide sketches
as art (08) for a bestiary spread.

---

## Task 20B — Wasteland settlements & named NPCs

**Goal:** Populate the routes with named settlements and recurring NPCs (traders, keepers,
hermits, pilgrims) so the world has persistent people, not just faction abstractions.

**Files:** `characters.json` (extend), `locations.json` (settlement `loc_*`), faction/standing
data, read-only `FactionStanceEngine`, `TradeTellEngine`, `WaystationSystem` (16B).

**Substeps:**
1. Read `characters.json` schema + how NPCs bind to locations/factions/standing.
2. Read `FactionStanceEngine` + `TradeTellEngine` so NPC dialogue/behavior reflects standing.
3. Author 6 named settlements (a salt camp, a rail-car town, a lighthouse commune, a quarry enclave, a pilgrims' rest, a scrapyard market) as `loc_*` with distinct character.
4. Author 18 named NPCs (3 per settlement): a keeper, a trader, a fixture — each with a trade specialty (TradeSpecialtySystem) and one personal thread.
5. Give each NPC 2–3 trade-tell lines (07/Plan 05 style) and a standing-reactive greeting.
6. Wire 6 NPCs to offer repeatable side-work (deliveries, hunts, escorts) feeding the quest pool.
7. Place settlements on the 16A map at waystation-adjacent nodes.
8. Validate ids; data-integrity selftest; dialog-graph lint.
9. xUnit: NPC load, standing-reactive dialogue selection, side-work offer/turn-in.
10. Portraits for the 18 NPCs (feeds 08B portrait pass).

**Next steps:** NPCs appear at the Muster (endgame) if befriended; settlement opinions ripple
to faction standing (16C); a traveling companion NPC (hard, later).

---

## Task 20C — Random wasteland encounter tables

**Goal:** Build tiered, route-aware random encounter tables so travel between nodes (16A) is
textured with risk and micro-stories — beyond the 68 door encounters.

**Files:** new/extended encounter tables (travel encounters), `events.json`,
read-only `ExpeditionEncounterBridge`, `ExpeditionSystem`, `WastelandMapSystem` (route danger).

**Substeps:**
1. Read `ExpeditionEncounterBridge` (the live domain class with a dedicated selftest) + how travel encounters trigger per route/tick.
2. Design encounter tables keyed by route danger tier and region (16A): what you meet on a tier-2 suburb road vs. a tier-9 crater approach.
3. Author 24 travel encounters across: 8 creature (20A fauna), 8 human (traders, beggars, deserters, a patrol, a funeral), 8 environmental (a collapsed bridge, a rad pocket, a wreck to strip, a weather trap).
4. Give each encounter 2–3 resolution paths incl. at least one non-violent (talk, pay, avoid) — the game already supports surrender/bribery.
5. Weight encounters by expedition stance (the 5 stances already exist) so a cautious stance meets fewer combat encounters.
6. Make 4 encounters chain (meet a pilgrim → later find their camp → later their fate) for continuity.
7. Key rare encounters to season (19C) and faction-war state (06C).
8. Validate ids; data-integrity selftest; encounter-bridge selftest.
9. xUnit: table selection by tier/region/stance; resolution paths apply outcomes; determinism via `ISeededRng`.
10. Balance sim: encounter frequency vs. expedition length must not make travel pure punishment.

**Next steps:** encounter "reputation" (spare travelers → later allies); a rare wandering
merchant with unique stock (13A goods); vehicle-specific encounters (10C chase) reuse these tables.
