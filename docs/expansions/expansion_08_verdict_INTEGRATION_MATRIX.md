# ASHFALL — Expansion 08 (The Verdict) Integration Matrix

**Audit date:** Exchange + (this pass). **Architecture source:** live `Ashfall.Core` + `src/` reads.

## Summary verdict

| Layer | Verdict content | Runtime consumer | Status |
|---|---|---|---|
| `verdict_data.json` | master/currencies/readouts/facets/endings | **none** | DATA ONLY |
| `verdict_locations.json` | 4 locations | **none** — `YearOfAshCatalogLoader.LoadLocations` reads `year_of_ash_locations.json` only | DATA ONLY (unreachable) |
| `verdict_door_encounters.json` | 8 encounters | `DoorEncounterSystem` + `DoorEncounterCatalogLoader` (**camelCase DTO matches!**) but loader's `DefaultFileName` = `door_encounters.json`; **no code loads verdict_door_encounters.json** | DATA ONLY (schema-wired, catalog-unregistered) |
| `verdict_items.json` | 15 items | **none** — runtime `ItemDefinition` DTO uses `id/displayName/description/type/stackMax/…`; verdict uses `name/category/tier/rarity/…` — **schema mismatch, silent drop** | DATA ONLY + BROKEN schema |
| `verdict_quests.json` | 8 quests | fails against `QuestlineSystem` DTO (`questlineId/firstStageId/stages{stageId,choices}`) — verdict uses legacy flat stage shape | DATA ONLY + BROKEN schema |
| `verdict_radio.json` | 13 radio | **none** — `YearOfAshRadioEntry` read from `year_of_ash_radio.json`; `CensusBroadcastScheduler` (Radio/, robust) is **unreferenced dead code** | DATA ONLY |
| The Tempest (currents.json, 16th, dormant) | 1 Current | `CurrentsCatalogLoader` loads it (roster UI) but no gameplay branch reads `faction_the_tempest` | PARTIALLY WIRED (roster-visible, inert) |
| MachineLogSystem | **missing** | — | DOCUMENTATION ONLY |
| Reckoning 3-phase machine | **missing** | — | DOCUMENTATION ONLY |
| shelter readouts | **missing** | — | DOCUMENTATION ONLY |
| evidence_ namespace | ids only | `IdPrefixes` recognizes prefix, no runtime representation | DATA ONLY |
| ending_verdict_ namespace | ids + vignettes | `EpilogueMatrixRuntime` has `tempestDecommissioned` + `RegionalFate.TempestSterilization` but no `ending_verdict_` consumer | PARTIALLY WIRED |
| 6 NPCs | prose only | none | DOCUMENTATION ONLY |
| corruption corpus | prose only | none | DOCUMENTATION ONLY |
| witness exchange / journal coloring | prose only | `JournalVoice` generic only | DOCUMENTATION ONLY |

## Key existing pieces (can reuse — do NOT duplicate)

1. **`DoorEncounterSystem` + `DoorEncounterCatalogLoader`** — live, camelCase DTO; Verdict door JSON matches; needs only registration (merge into `door_encounters.json` or a loader call).
2. **`QuestlineSystem`** — live stageId-DAG runtime; Verdict quests must be authored in this model.
3. **`ItemCatalog` / `ItemDefinition`** — live runtime item registry; Verdict items must be authored to this DTO.
4. **`CensusClaimSystem`** — live holdfast census/levy (Day 40), already wired in Main/Core Demo.
5. **`EpilogueMatrixRuntime`** — `tempestDecommissioned`, `TempestSterilization` fate exist; no Verdict ending predicate.
6. **`ISimClock`, `IEventBus`, `IFlagLedger`** — live infra for deterministic time/events/flags.
7. **`IFileIO`/`IJsonSerializer` ports + `HostCli` selftest pattern + `MusterSaveStore`/`YearOfAshSaveStore` codec pattern** — canonical save/selftest rigs.
8. **`Ashfall.Core.Verdict.CensusBroadcastScheduler`** — math-only class, dead; fold the *culpability formula* into the Reckoning system, then delete.
9. **`Ashfall.Core.Radio.CensusBroadcastScheduler`** — richer (live census, 99.0 MHz carrier, degradation grammar); expand_09/12 hooks unreferenced; adopt/replace via one authoritative Verdict scheduler, not two.

## Decisions (architectural, from evidence)

- **D1 — One Verdict runtime namespace `Ashfall.Core.Verdict`** owning: `MachineLogSystem`, `ReckoningSystem`, `VerdictCatalog` (data loaders), `VerdictSave` codec. Deletes/absorbs duplicate schedulers (D3).
- **D2 — Data must match runtime DTOs**; keep authoring catalogs as-is where schema matches (door encounters), migrate items/quests to runtime schemas (a `verdict_*runtime*` concern documented; do NOT fork the shipped catalogs).
- **D3 — Adopt the Radio/ census scheduler as the single census broadcast engine** (canonical 99.0 MHz, 1.7 s held-breath pause, degradation grammar), re-parented under `Verdict`, wired through a host session; the `Verdict/` math copy is deleted.
- **D4 — Evidence as a dedicated `EvidenceLedger`** (flag+record hybrid) rather than items — authoritative for the three endings; no parallel inventories.
- **D5 — Reckoning as a state machine** with `ISimClock`-based transitions, idempotent, persistent via `VerdictSave`.
- **D6 — Endings hook into `EpilogueMatrixRuntime`** via `ending_verdict_*` predicates + `tempestDecommissioned`, keeping base-ending priority.

## Implementation order (dependencies)

`3 MachineLog` → `4 Reckoning` → `8 radio (census)` → `9 evidence+endings` → `6 quests` → `7 locations/encounters/items` → `5 readouts` → `10 save/tests`.
