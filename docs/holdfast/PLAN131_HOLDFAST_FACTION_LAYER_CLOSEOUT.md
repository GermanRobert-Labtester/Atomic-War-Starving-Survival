# Plan 131 Holdfast Faction Layer Closeout

> Repository note: `Next-steps-plans/Plan_131_Wasteland_Information_Rumor_Network.md`
> is the repository's canonical Plan 131 and describes a separate rumor-network
> system. This document records the separately requested Holdfast faction
> identity audit; it is not a completion claim for the rumor-network plan.

## Scope

This closeout records the canonical Holdfast faction boundary. The original
three trade IDs remain unchanged. Five additional authored identities are
available to trade, dispatch, territory, radio, quest, and epilogue content.

The live trade roster has **nine** entries, not eight. `faction_scavengers` is a
pre-existing compatibility entry used by global travel and verdict content. It
is retained in `holdfast_factions.json`, but it is not one of the eight authored
Holdfast identity profiles and intentionally resolves to the neutral dispatch
voice.

## Canonical authored identities

| ID | Display name | Home region | Dispatch profile |
|---|---|---|---|
| `faction_the_office` | The Office | `the_cluster` | `bureaucratic` |
| `faction_the_cutters` | The Cutters | `the_cut` | `salvage` |
| `faction_the_fleet` | The Fleet | `the_shelf` | `maritime` |
| `faction_black_flotilla` | The Black Flotilla | `coastal_shelf` | `privateer` |
| `faction_supply_corps` | The Supply Corps | `district_8` | `allocation` |
| `faction_railway_guild` | The Railway Guild | `the_rail_south` | `logistics` |
| `faction_hydro_barons` | The Hydro Barons | `the_aquifer` | `monopoly` |
| `faction_ordnance_foundry` | The Ordnance Foundry | `the_foundry_yard` | `foundry` |

Locations, vessels, services, subordinate crews, and Crossing-only
institutions are not promoted into Holdfast faction IDs. The Lamplighter role
and the Kittiwake vessel remain content under their existing owners.

## Authority and persistence

- Faction identity, alignment, region, wants, offers, access copy, and starting
  trust are authored in `holdfast_factions.json`.
- Mutable standing remains owned by `FactionStanceEngine` and its existing
  save path. `HoldfastFactionEntry.Trust` is static catalog data.
- Trade value, inventory, stock, selection validation, Fleet restriction, and
  trade save state remain owned by `HoldfastTradeSession`.
- Dispatch text remains a presentation overlay in `holdfast_flavor.json`.
- NPC membership remains in `holdfast_npcs.json`; quest prose remains in the
  Holdfast quest catalogs. Neither is duplicated into the faction entry.
- Alias-bearing global catalogs continue to resolve through
  `FactionStandingIdResolver`; canonical Holdfast consumers use the
  `faction_*` systems IDs.

## Corrective changes

- Removed retired `faction_holdfast_*` profile payloads from the Core faction
  entry. The public compatibility methods now return empty collections and
  direct callers to the authored NPC, quest, and standing authorities.
- Changed the mercenary fallback issuer from `faction_holdfast_schedule` to
  `faction_the_office`.
- Changed the Holdfast Hydro Barons NPC reference from `hydro_barons` to
  `faction_hydro_barons` in both the JSON authority and the Core fallback.
- Added identity, flavor-set, NPC alias, save-boundary, and retired-ID tests.

## Validation

The focused Holdfast baseline passed before this corrective patch:

- Core build: PASS
- Focused Holdfast tests: 24/24 PASS
- `--holdfast-selftest`: 25/25 PASS
- `--data-integrity-selftest`: 0 findings across 300 catalogs

Post-patch verification:

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: PASS, 0 warnings,
  0 errors.
- Focused Holdfast tests: PASS, 107/107.
- `dotnet build Ashfall.csproj`: PASS, 0 warnings, 0 errors.
- `--holdfast-selftest`: PASS, 25/25.
- `--data-integrity-selftest`: PASS, 0 findings across 300 catalogs.
- `--bridge-selftest`: PASS.
- `git diff --check`: PASS.
- Holdfast JSON parse: PASS for factions, flavor, and NPC catalogs.

The full Core suite reached 10,168/10,169 passing tests. Its single failure is
the unrelated `MainTriadDriftGateTests.SetupWithoutSave_IsAllowlistedOrHasSaveTwin`
check for the concurrent `Enrichment` setup change in
`Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs`. That worktree change was
not overwritten or repaired as part of this audit.
