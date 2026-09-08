# Plan 131 Holdfast Audit Implementation Log

## Scope

This log covers the separately requested Holdfast canonical-faction audit. The
repository's canonical `Plan_131` document is the Wasteland Information &
Rumor Network plan; this audit does not claim to implement that system.

## Changes

- Preserved the nine-entry Holdfast trade roster: eight authored identities
  plus the existing `faction_scavengers` compatibility entry.
- Removed retired `faction_holdfast_*` narrative payloads from the static
  faction entry while retaining obsolete empty compatibility methods.
- Kept mutable trust in the standing/save authority rather than duplicating it
  in trade-session state.
- Made `holdfast_npcs.json` the sole NPC roster authority and canonicalized the
  Hydro Barons NPC faction reference.
- Changed the mercenary fallback issuer to `faction_the_office`.
- Added identity, alias, flavor-set, roster, and save-boundary tests.

## Verification

- Core test build: PASS.
- Holdfast-filtered tests: PASS, 107/107.
- Godot host build: PASS.
- Holdfast selftest: PASS, 25/25.
- Data-integrity selftest: PASS, 0 findings across 300 catalogs.
- Bridge selftest: PASS.
- Full Core suite: 10,168/10,169 passed; the remaining failure is the
  unrelated concurrent `Enrichment` setup/save-twin drift check.

See `docs/holdfast/PLAN131_HOLDFAST_FACTION_LAYER_CLOSEOUT.md` for the
authority map and detailed evidence.
