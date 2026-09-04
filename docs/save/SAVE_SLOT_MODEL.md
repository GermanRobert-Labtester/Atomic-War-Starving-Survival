# SAVE_SLOT_MODEL.md — Save Slot Management (Plan VIII · Task 22)

## Slot model

- **Profiles:** `SaveProfileId` (currently one active profile per install).
- **Slots:** dynamic registry — slot ids are user-creatable strings
  (`slot_1`, `slot_2`, …; the panel's NEW SLOT picks the next free index).
  There is no fixed slot count.
- **Storage:** one aggregate, checksummed, atomically-written envelope per slot
  (`campaign.json` per slot root; Initiative #42) — never per-section files.
- **Legacy migration:** pre-slot global section files auto-migrate verbatim into
  a fresh `migrated_N` slot on load; V1 filename-keyed envelopes migrate
  in-memory to V2 (persisted as V2 on the next save). Envelopes created by
  legacy import carry `migratedFromLegacy = true` (shown in the panel).

## Slot list (panel ↔ authority mapping)

Every displayed value comes from the persisted envelope/manifest — the panel
never fabricates metadata and never uses filesystem mtime for save times.

| Field | Source |
|---|---|
| Campaign name / "(empty)" | `SaveManifest.campaignName` |
| Day | `SaveManifest.currentDay` |
| Mode / terminal seal | `SaveManifest.mode`, `ironManTerminalState` |
| Last save time | `SaveManifest.lastSaveTimestamp` (ISO-8601) |
| Profile / game version / build / seed | manifest |
| Envelope health | `SaveLoadHostSession.GetEnvelopeHealth` (below) |

Empty slots are honestly empty: no manifest ⇒ "(empty)", no fabricated data.

## Envelope health (Task 22.4)

`GetEnvelopeHealth(slotId)` reads the **persisted** envelope via
`SaveSlotService.LoadAggregate` and reports exactly what the last save said:
manifest version, aggregate checksum present/MISSING, `migratedFromLegacy`,
per-section `name — ok / no checksum` lines. A corrupt envelope reads
`LOAD FAILED (corrupt — keep for recovery)`; nothing is deleted or hidden.
Health is never recomputed from live runtime state.

## Destructive actions (Task 22.6 / 22.9)

- **Delete** and **New-game reset** are two-step confirmations in the panel
  (`DEL` → `SURE?`; `NEW GAME (RESET)` → `SURE? ERASE SLOT`, armed state shown
  in red; selecting anything else disarms).
- Both route through the save authority — `SaveLoadHostSession.DeleteSlot`
  (removes the aggregate envelope and slot directory including `.bak`) and
  `SaveLoadHostSession.ResetSlotForNewGame` (clears the slot for a fresh
  campaign, clearing the active-session envelope state). The panel never calls
  raw `File` operations and never edits envelope JSON.

## Failure recovery (Task 22.7)

The seven-gate failure UX (`--save-load-ui-failure-selftest`) is unchanged and
remains green: missing, corrupt, and checksum-invalid saves surface recoverable
messages; corrupt slots stay visible for inspection (no silent discard);
overwrite of an existing slot only happens through explicit save/load actions.

## Rename / label (Task 22.5) — deferred

`SaveManifest.campaignName` is the player-facing label and is schema-safe
(optional string, absent in old manifests). A rename affordance would go
through `SaveLoadHostSession.UpdateManifest` (the sanctioned write path), but
no text-input convention exists in this UI yet — **deferred**, documented here
per the plan's non-goal discipline.

## Non-goals (v1)

- Slot copy / move / export / import between slots — deferred (legacy *file*
  import via IMPORT LEGACY remains, unchanged).
- Typed destructive confirmation — this UI uses two-step buttons (consistent
  with existing panel conventions).
- The panel is presentation-only: all reads go through the session; all writes
  through the save authority.

## Verification

- `godot --headless --path . -- --save-load-ui-failure-selftest` (7 gates)
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` (bind/unbind/
  rebind discipline — the panel keeps its existing unsubscribe pattern)
- `docs/ci/GATE_INVENTORY.md` gates `save_load_failure`, `panel_bind_lifecycle`.
