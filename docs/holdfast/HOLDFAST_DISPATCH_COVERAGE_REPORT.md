# Holdfast Dispatch Coverage Report

## 1. Metrics & Expansion Summary
- **Holdfast Trade Factions in Data Authority (`holdfast_factions.json`):** 9
- **Pre-Plan 128 Flavor Profiles:** 3 (`faction_the_office`, `faction_the_cutters`, `faction_the_fleet`)
- **Pre-Plan 128 Coverage:** 33.3% of Holdfast trade factions
- **Post-Plan 128 Flavor Profiles:** 8 (added `faction_black_flotilla`, `faction_supply_corps`, `faction_railway_guild`, `faction_hydro_barons`, `faction_ordnance_foundry`)
- **Post-Plan 128 Coverage:** 88.9% (8 of 9 factions covered; remaining 1 is `faction_scavengers` which cleanly falls back to neutral salvage defaults)

## 2. Generic Fallback Reduction
In typical playthrough trade interactions:
- Office, Cutters, Fleet transactions continue using their signature voices.
- Flotilla maritime dive trades now render authentic naval privateer dispatches.
- Supply Corps ration allocations render crisp bureaucratic issue tallies.
- Railway Guild diesel transactions render heavy rail freight waybills.
- Hydro Barons water filtration trades render strict aquifer monopoly receipts.
- Ordnance Foundry ammunition runs render rhythmic industrial cupola logs.
- Unflavored counterparties or invalid test IDs cleanly fall back to `NeutralFactionVoice` without crashes or missing text.
