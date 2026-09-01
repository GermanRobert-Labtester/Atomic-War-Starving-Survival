# Plan 29 — Room Identity Contract (Task 29A)

> **Status:** Phase 1 pilot + **Phase 2 completion IMPLEMENTED** (2026-09-01). Pins the
> schema, alias policy, discovery ownership, save contract, and UI surfacing rules for
> room identity. **12 rooms, 20 vignettes, 49 fixtures** authored and contract-tested.
> Later phases (origin thread, 29B/29C integration) extend this contract — they must
> not fork it.

---

## 1. Data authority

**File:** `Assets/StreamingAssets/Data/shelter_room_identities.json` (root Data dir —
validated by `CatalogIntegrityValidator`; `schema_version` required).

```json
{
  "schema_version": 1,
  "collection_id": "shelter_room_identities",
  "rooms": [{
      "id": "room_filtration",          // canonical runtime room id (definition position)
      "display_name": "...",            // matches the spatial roster display name
      "former_use": "...",              // pre-war/original use (§29A.4)
      "current_use": "...",             // crisis conversion + current use
      "one_line_history": "...",        // 10–36 words enforced (§29A.5 target 12–30)
      "inspection_summary": "...",      // short surface text
      "legacy_aliases": ["room_filtration_stack", "room_air_filtration"],
      "fixture_ids": ["room_fixture_filtration_nameplate_tin", "…"]  // §29A.10 pool
  }],
  "vignettes": [{
      "id": "room_history_the_first_filter_change",  // prefix room_history_
      "room_id": "room_filtration",                  // must resolve to a room above
      "title": "...",
      "time_period": "early shelter occupancy",
      "unlock": "inspect_room",                      // §4a vocabulary
      "unlock_day": 0,                               // required day, day_milestone only
      "body": "..."                                  // 100–300 words enforced
  }],
  "fixtures": [{                                     // §29A.10–29A.11
      "id": "room_fixture_filtration_nameplate_tin", // prefix room_fixture_
      "room_id": "room_filtration",
      "detail": "...",                               // short visible line
      "historical_meaning": "...",                   // why it is there
      "inspectable": false,                          // true only if a UI action exists
      "art_visible": true,
      "renovation_sensitive": false,                 // 29C may cover/remove it
      "codex_entry_id": ""
  }]
}
```

**Bands enforced by `Validate()`:** one-line history 10–36 words · vignette body
100–300 words · ≤ 6 fixtures per room · fixture & vignette `room_id` must resolve ·
`fixture_ids` ↔ `fixtures` agree **both directions** (no orphans, no dangling refs) ·
duplicate room/vignette/fixture ids rejected · an alias may not collide with another
canonical room id · `unlock` must be in the §4a vocabulary · `day_milestone` requires
`unlock_day >= 1` and every other unlock requires `unlock_day == 0`.

Missing file → **empty, valid catalog** (identity is an overlay, never a domain
dependency — same rule as `HoldfastFlavorCatalog`).

## 2. Canonical room ids & alias policy (§5.1)

- Identity records bind to the **canonical spatial roster** (`HoldfastInteriorView`
  8-room set; see `SHELTER_ROOM_INVENTORY.md` §5). Runtime room ids are **never
  renamed** by this system.
- Legacy runtime ids (`room_filtration_stack`, `room_bunks_living`,
  `room_air_filtration`, …) are declared as `legacy_aliases` and resolved by
  `ShelterRoomIdentityCatalog.ResolveRoomId()` → canonical id.
- `GetLegacyAliases(canonical)` bridges the other direction (canonical click →
  legacy Day-1 roster entry).
- Code-authored runtime room ids are whitelisted in
  `CatalogIntegrityValidator.KnownRuntimeIds` (they are legitimately defined outside
  JSON catalogs; the whitelist comment lists which rooms are data-registered instead).

## 3. Discovery & unlock ownership (§29A.18 — no second save authority)

| State | Owner | Persistence |
|---|---|---|
| Room inspected (Day-1 roster) | `StartingLevelSystem.rooms[].isInspected` | starting-level save section |
| Vignette discovered | `JournalSystem` knowledge key `room_history_seen_<vignette_id>` | journal save (`KnowledgeBaseSave`) |
| Room identity visibility | none needed — identity shows immediately if the room exists (§29A.19) | n/a (static data) |

`ShelterRoomIdentityCatalog` is a **read-only projection**: no condition state, no
Capture/Restore, no save section. Adding a second discovery ledger would violate
Plan 29 §1.2/§29A.18.

**Old-save defaults:** a pre-Plan-29 save has no `room_history_seen_*` key → all
vignettes locked; they unlock on the next inspection. No unlock spam at load.

## 4. Runtime flow (Godot host)

### 4a. Unlock vocabulary — every value must have a live trigger

| `unlock` | Core trigger | Host seam that raises it | Content |
|---|---|---|---|
| `inspect_room` | `RoomHistoryTrigger.RoomInspected` | shelter room hotspot click → `ShelterPanel.RoomSelected` → `Main.HandleShelterRoomSelected` | 5 vignettes |
| `repair_performed` | `RoomHistoryTrigger.RepairPerformed` | dashboard filter service/replace, on success → `Main.HandleShelterRoomRepairPerformed("room_filtration")` | 1 vignette |
| `day_milestone` | `RoomHistoryTrigger.DayElapsed` + `unlock_day` | `CampaignDayCoordinator` owner **`shelter_room_history`** (phase 5) → `Main.TickShelterRoomHistoryMilestones(day)` | 2 vignettes |

A fourth unlock value may only be added **together with** its host seam.
`HostSourceGate_EveryUnlockPathHasAWiredTrigger` fails if the host stops raising one, and
`Vignettes_AreReachable_AndTheirUnlockKeysRoundTrip` fails if any authored vignette stops
being reachable through these three seams.

**Catch-up rule:** day milestones fire on `currentDay >= unlock_day`, so an older save
loaded at day 30 reveals its pending vignettes on the next day advance — once each, the
journal key being idempotent. Inspection histories are never bulk-revealed at load.

```
ShelterPanel room hotspot click
  → ShelterPanel.RoomSelected (forwarded from HoldfastInteriorView.RoomSelected)
  → Main.HandleShelterRoomSelected(roomId)            [Main.ShelterInfrastructure.cs]
      → catalog.ResolveRoomId → canonical
      → StartingLevelSystem.InspectRoom(canonical)     (falls back to legacy aliases)
      → catalog.GetUnlockableVignettes(canonical, RoomInspected)
      → JournalSystem.UnlockRoomHistorySeen(id)        (idempotent; marks journal dirty)
```

Codex surfacing: `JournalCodex` Places tab appends vignette rows
(`JournalCatalogs.RoomHistories`, joined with room display names at load). Locked rows
show `"<Room> — untold history"` (no title spoiler).

Inspection UI surfacing: `HoldfastInteriorView.GetRoomStatusSummary` appends
`"Formerly: <former_use>"`, the one-line history, and up to **three** `art_visible`
fixture lines as `"Notable: …"` — **tooltip only**. The live status line stays first, so
lore never buries an actionable state (§29A.6, §15.1).

## 5. Validation & tests

- `ShelterRoomIdentityCatalog.Validate()` — see §1 bands. Pinned by
  `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs` (**26 tests**): load, zero-error
  validation, full major-room roster, 8-vignette count with varied paths, fixture pools +
  bidirectional refs, legacy-id fixture lookup, `inspectable` false everywhere, trigger
  separation (inspection never fires repair/day), day-milestone boundary, catalog-wide day
  pass stability, unknown room yields nothing, unlock-vocabulary mapping, host source gate
  for wired triggers, journal unlock idempotence + round-trip + old-save default,
  location migration, and the inspection seam's return contract.
- `--journal-selftest` places-row count updated to `Locations + RoomHistories`
  (deliberate contract change, noted in `JournalSelfTest.cs`).
- Data-integrity selftest validates the new root catalog (tier-1 refs resolve via
  registry + `KnownRuntimeIds` whitelist).

## 6. Location-id drift resolution (Phase 1 scope)

`StartingLevelSystem.HoldfastLocationId`: `loc_bunker_holdfast` → **`loc_holdfast`**
(data authority `locations.json:992`). Migration: `RestoreState` maps the legacy value
to canonical; `LegacyHoldfastLocationId` const retained for the migration. Test
expectation updated (`StartingLevelSystemTests`).

## 7. Phase 3+ extension points (do not fork)

1. **Remaining roster:** `room_memorial_wall`
   (`ShelterDecorHostSession.MemorialWallRoomId`) joins the identity set alongside Task
   29B.20's machine-memorial hook.
2. **More vignettes / unlocks:** extend `RoomHistoryTrigger` **with** its host seam in the
   same change (§4a). Candidates named by §29A.9: archive document read (Plan 17B
   `ArchiveDeskSystem`), renovation stage complete (29C), specific glitch (29B),
   occupancy/assignment history (`ShelterAssignmentSystem`).
3. **Fixture interactivity:** set `inspectable: true` only together with a real UI action;
   until then fixtures stay an ambient pool + art backlog (`ROOM_FIXTURE_MATRIX.md`).
4. **`codex_entry_id`:** reserved — populate only when the origin thread ships through
   Plan 17B archive infrastructure, gated by `BUNKER_ORIGIN_CONTINUITY.md` §9.
5. **Snapshots:** tooltip text is not rendered by the golden-snapshot system, so Phases
   1–2 changed no pixels. Capture snapshots only once a visible inspection card exists.
6. **Machine→room binding (29B):** identity records are the lookup surface; continuity
   §7.3 still owes the generator a room decision.
