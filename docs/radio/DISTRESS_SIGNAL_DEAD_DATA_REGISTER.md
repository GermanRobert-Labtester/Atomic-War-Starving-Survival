# Distress Signal — Authoritative Dead-Data Register

**Status:** CURRENT
**Date:** 2026-09-17
**Owner:** Radio / narrative content authority
**Purpose:** single authoritative register of expansion rows that are
unreachable behind the documented primary-wins load order, so they stop
recurring as ambiguity and are never silently re-authored or deleted.

## Load-order contract

`RadioHostSession` loads `radio_distress_signals_expansion.json` **first** and
`radio_distress_signals.json` **last**; the primary layer wins on a shared
`frequency_id`. `CatalogIntegrityValidator.ValidateDistressSignalStages`
therefore emits a **warning** (not an error) for each cross-file duplicate.
This is the sealed Wave 1 policy (`DISTRESS_SIGNAL_STAGE_CONTRACT.md` §5).

## Registered rows (5)

| Row ID | Source catalog | Override mechanism | Reason retained (not deleted) | Deletion authority required |
|---|---|---|---|---|
| `freq_distress_55_1`  | `radio_distress_signals_expansion.json` | primary `radio_distress_signals.json` loads last | expansion is an additive content layer; the primary retains the canonical Plan 50 definition. The expansion row is unreachable dead data. | Radio/narrative content authority (Plan 50) + signed data tranche |
| `freq_distress_401_9` | `radio_distress_signals_expansion.json` | same | same | same |
| `freq_distress_217_4` | `radio_distress_signals_expansion.json` | same | same | same |
| `freq_distress_148_2` | `radio_distress_signals_expansion.json` | same | same | same |
| `freq_distress_392_7` | `radio_distress_signals_expansion.json` | same | same | same |

## Rules

1. **No new content in dead rows.** The PR2 hint tranche deliberately left 28
   stage positions un-hinted on these five rows; authoring into them writes
   unreachable content (`DISTRESS_SIGNAL_PR2_HINT_TRANCHE.md`).
2. **No deletion without signature.** Removing the rows is a data-authority
   decision, not a hygiene pass; until then they remain registered here.
3. **Warning count is pinned.** The integrity gate must emit exactly these five
   warnings. A delta is a new finding, not a register update
   (`DISTRESS_SIGNAL_STAGE_CONTRACT.md` §5).

## Verification

`godot --headless --path . -- --data-integrity-selftest` reports exactly 5
warnings naming the five IDs above (0 errors, 333 catalogs), matching this
register row-for-row.
