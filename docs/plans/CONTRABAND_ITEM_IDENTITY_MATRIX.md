# CONTRABAND ITEM IDENTITY MATRIX — Plan 147 Task A.3/A.4

Identity classification per record. A contraband record is **not automatically
an inventory item**: each row is classified as ITEM-BACKED (link to existing
canonical id), SERVICE/DOCUMENT (typed action/discovery reward — no item),
NARRATIVE/STASH TOKEN (discovery event only), or DUPLICATE-CONCEPT RISK.
Duplicate item rows are never created just to make a record inventory-backed
(Plan Task A.4).

## Classification

| Record | Identity | Canonical linkage / route | Status |
|---|---|---|---|
| `contraband_card_deck_pinned_kings` | **ITEM-BACKED** | `item_playing_cards` (items.json; "Plastic-Coated Playing Cards", tradeValue 12). A marked deck *is* a card deck; the "marked" edge is flavor (no gambling mechanic). | **ACTIVATED (tier-1 slice)** — stash grant 1× |
| `contraband_unrationed_sugar_brick` | **ITEM-BACKED** | `sugar` ×8 (items.json; moraleEffect 1, tradeValue 2). 800 g brick = 8 canonical 100 g packets. | **ACTIVATED (tier-2 slice)** — stash grant 8× |
| `contraband_century_seed_grain_vial` | **ITEM-BACKED** | `item_seed_wheat` (greenhouse_items.json; plantable via `GreenhouseExpansionCatalog.CropCatalog` → `crop_wheat`). Heirloom wheat ≡ wheat seed identity; the "unbroken/non-irradiated" prose is flavor. | **ACTIVATED (tier-3 slice)** — stash grant 1× |
| `contraband_paraffin_candle_hoard` | Item-concept, **no canonical item** | No candle item in items.json. Creating one is allowed later via normal data-authority channels (not by this plan). | DEFERRED |
| `contraband_smuggled_coffee_grounds` | Item-concept, **no canonical item** | No coffee item exists. A future `coffee` item would own morale/fatigue effects. | DEFERRED |
| `contraband_uninspected_lard_tin` | Item-concept, **no canonical item** | No lard item. | DEFERRED |
| `contraband_bootleg_morphine_ampoules` | Item-concept, **no canonical item** | `morphine` exists ONLY in `chemical_dependency_items.json` (dependency catalog), not as an items.json item. A canonical morphine item must be authored (data authority) before this can be item-backed; dependency then routes through `ChemicalDependencySystem`. | DEFERRED (highest-value next candidate) |
| `contraband_copper_condenser_coil` | **EQUIPMENT-CONCEPT** (not a consumable) | Not `spirits` — the coil is a still *part*, not the drink. No equipment slot accepts it. | DEFERRED |
| `contraband_distillery_hydrometer_glass` | Equipment-concept | No lab-tool item/interaction. | DEFERRED |
| `contraband_illicit_triode_tube` | Component-concept | No radio-component item contract (and no radio-range mechanic to serve). | DEFERRED |
| `contraband_unregistered_geiger_crystal` | Component-concept | No detection-precision mechanic. | DEFERRED |
| `contraband_modified_filter_cartridge` | Equipment-concept (hazardous variant) | Would need its own item def with reduced `radProtection`; must never mutate an existing filter item. | DEFERRED |
| `contraband_siphon_hose_and_bulb` | **SERVICE/ACTION-CONCEPT** | Its value is a fuel-theft *action*, not a good. Requires a designed action with costs/consequences (Plan §5.12-13 rules). Not a zero-weight fake item. | DEFERRED |
| `contraband_subverted_keycard_flasher` | Service/action-concept | Door-bypass action; **blocked on missing AirlockSecuritySystem extension point**. | DEFERRED |
| `contraband_forged_muster_stamp` | Service/document-concept | Ration-forgery action; no muster forgery contract. | DEFERRED |
| `contraband_mimeographed_heresy_pamphlet` | **DOCUMENT/NARRATIVE TOKEN** | Reading event could route to faction-stance or journal; producer must be a real encounter/choice, not a load effect. | DEFERRED |
| `contraband_lead_counterfeit_slugs` | **INVALID-AS-EXECUTABLE** (counterfeit currency) | No scrip currency runtime exists (audit doc). Falsification mechanics are non-executable by plan §10; the row survives as prose only. | DEFERRED / non-executable |
| `contraband_prison_tattoo_needle_rig` | Service-concept | Loyalty/infection have no item-use hooks. | DEFERRED |
| `contraband_hand_wound_dynamo_spool` | Equipment-concept | No attachable power contract. | DEFERRED |
| `contraband_stolen_nickel_cadmium_cell` | Equipment-concept | No attachable battery contract. | DEFERRED |

## Rules honored

- **Zero duplicate item rows created.** The three activated rows link to
  *existing* canonical ids only; 17 rows stay deferred rather than being forced
  into invented item definitions.
- No contraband id was duplicated as an item id, and no item id collides with a
  `contraband_*` id (cross-ref sweep in `CONTRABAND_ENTRY_MATRIX.md`).
- Non-item services/documents must never be faked as zero-weight inventory
  items (Plan Task B.6) — hence their DEFERRED status instead of grants.
- Identity re-review trigger: any row promoted from DEFERRED requires (a) an
  owner row in the mechanics authority matrix, (b) a canonical identity (item
  id, typed action or narrative producer), (c) tests in the regression matrix.
