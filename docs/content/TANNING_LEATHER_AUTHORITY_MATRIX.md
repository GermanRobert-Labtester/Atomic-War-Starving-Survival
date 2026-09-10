# TANNING LEATHER AUTHORITY MATRIX — Plan 159

> Ownership matrix for every behavior the tanning/leather corpus touches.
> Plan 159 introduces **no new system authority**; the
> `LeatherworkArchiveSystem` is a read-only projection with a discovery
> ledger. Green = activated by this plan; Red = explicitly NOT owned.

## 1. Authority ownership matrix

| Concern | Owning authority | Plan 159 relationship |
|---|---|---|
| Historical/process prose, batch measurements, failure descriptions, tags, timestamps | `TanningLeatherCatalog` (JSON data authority) | ✅ **activated** — projected read-only |
| Discovery state (which records the player has seen) | `LeatherworkArchiveSystem` + `LeatherworkArchiveSaveStore` (IDs only) | ✅ **new read-only ledger** |
| Creatures, wildlife, hunting, harvested outputs | wildlife/expedition systems | 🔴 **untouched** — no drops added |
| Raw hide / leather / oil / chemical **inventory items** | `items.json` + `InventoryHostSession` | 🔴 **untouched** — no items created, no quantities changed |
| Transformations, crafting costs, recipes | Plan 55 recipe authority | 🔴 **untouched** — no recipes generated from formulas |
| Equipment condition / durability | `EquipmentConditionSystem` | 🔴 **untouched** — tensile PSI is archival, never applied |
| Protective performance | protective-gear systems | 🔴 **untouched** — shrink temp ≠ armor resistance |
| Trade prices / scarcity | economy systems | 🔴 **untouched** — provenance is display-only |
| Health / chemical exposure / disease | Disease/Medical systems | 🔴 **untouched** — Cr(VI)/sulfide harm is historical narrative |
| Locations, settlements, workshops | settlement/location catalogs | 🔴 **untouched** — no tannery location created |
| Manuals / research / skills | Plans 80/34/33 | ⚪ **deferred** — no typed manual hook exists for these records (see disposition) |
| Harness/transport performance | vehicle/expedition systems | ⚪ **n/a** — no animal-haulage system exists; none invented |
| UI presentation of provenance | `InventoryDetailPanel` (display-only rows) | ✅ **extended** — new optional provenance lines |

## 2. Command/flow contract (§26 vertical slice)

```
real producer (site discovery / item inspection)
  → stable tanning record id (30 authored, unique)
  → discovery authority (LeatherworkArchiveSystem; idempotent, ordinal-ordered)
  → read-only projection (LeatherworkRecord — labels + summaries only)
  → existing inspection surface (InventoryDetailPanel provenance rows)
  → save/reload (leatherwork_archive_save.json; checksummed envelope)
  → NO change to: inventory quantity, item condition, trade value,
                  health, crafting output, wildlife drops
```

## 3. Firewall mechanics (enforced by construction + tests)

| Forbidden leak | Enforcement |
|---|---|
| record mention of hide ⇒ hide inventory | no inventory API on the archive; `Firewall_ProjectionSurface_HasNoMechanicalAPIs` |
| steep months ⇒ crafting timer | duration renders only inside `MeasurementSummary`; `Firewall_HideSteepMonths_AreNotCraftingTimers` |
| pH / chemicals ⇒ exposure damage | pH renders as assay text; no health API; `Firewall_FormulaAndPh_RenderAsProvenanceOnly` |
| shrink temp ⇒ armor resistance | temperature renders as assay text only |
| tensile PSI ⇒ durability | PSI renders as archival spec; inspection line reads "NOT THIS ITEM'S MEASURED QUALITY" |
| fatliquor formula ⇒ recipe | formula is a raw label string; never parsed into components |
| bark source ⇒ harvestable plant | botanical names are labels; no gatherable added |
| vat/pit/bench label ⇒ world location | labels never used as producer IDs; `CanonicalLinks_NeverPromoteFacilityLabelsToWorldOrItemIds`; `ProducerMap_ValidLocations_NoInventedSites` |
| discovery replay after reload | `SaveRoundTrip_PreservesDiscovery_NoReplay`; `DiscoverForItem_RoundTrips_ThroughSave` |
| auto-discovery on old saves | `Restore_OldSave_Null_DiscoversNothing_Automatically` |
| index-based record state | ledger stores stable record IDs only; unknown future IDs tolerated (`Restore_UnknownFutureIds_Tolerated`) |

## 4. Persistence contract (§19)

- New section: `leatherwork_archive` in `SaveSectionRegistry` (canonical
  filename `leatherwork_archive_save.json`), thin façade over Core
  `SaveStore<T>` via `SaveStoreHub` — checksummed
  `{ SchemaVersion, State, Checksum }` envelope, atomic write, no legacy
  bare-state format (none ever existed).
- Inside the single `campaign.json` aggregate envelope (Initiative #42) via
  `CaptureSection` in `SaveLeatherworkArchive`.
- Old saves: section absent ⇒ `TryLoad` returns null ⇒ empty ledger ⇒
  nothing auto-discovered.
- Item/location links are **not** persisted; they re-derive from current
  catalog data each run, so link evolution cannot corrupt discovery state.

## 5. Manuals / research / skills (§18 disposition)

Plan 80/34/33 remain authority. The corpus contains no manual/research
references resolvable to an existing typed manual or research node — no
`knowledge_` id corresponds to tanning. Rather than invent an integration
point, record discovery stands on its own site/inspection producers. A
future manual node ("field tannery handbook") could route through
`DiscoverRecord(recordId)` with no schema change.
