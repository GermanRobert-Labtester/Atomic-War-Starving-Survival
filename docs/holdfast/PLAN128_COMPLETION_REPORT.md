# Plan 128 Completion Report

## Final Status
- **Status:** COMPLETE
- **Phase:** 13 (Closeout & Documentation)

## Baseline
- **Factions Before:** 3 (`faction_the_office`, `faction_the_cutters`, `faction_the_fleet`)
- **Flavor Profiles After:** 8 (+5 canonical Holdfast factions); the Holdfast trade catalog contains 9 selectable factions, with `faction_scavengers` intentionally using the neutral fallback.
- **Items Baseline:** 40 entries preserved verbatim
- **Unit Tests Baseline:** 10,031 passed
- **Current Unit Test Verification:** 10,045 passed. The historical Plan 128 snapshot recorded 10,038 after the expansion.
- **Runtime UI Census:** 55 trade-catalog items and 9 selectable Holdfast factions; the flavor overlay remains 40 item entries and 8 specialized faction profiles.

## Consumer Contract
- **Dispatch Key Source:** `HoldfastTerminalPanel` selected counterparty ID (`_selectedFactionId`), backed by `holdfast_factions.json` loaded into `HoldfastFactionsCatalog`.
- **Register Semantics:** Descriptive procedural tone tokens: `bureaucratic`, `salvage`, `maritime`, `privateer`, `allocation`, `logistics`, `monopoly`, `foundry`.
- **Voice Semantics:** Concise 2-3 sentence overview of institutional priorities and counterparty tone used on purchases and stock updates.
- **Rejected Semantics:** Reason-agnostic institutional refusal appended to mechanical failure details (`Requisition refused: <detail> <rejected>`).
- **Sold Semantics:** Directionally safe receipt statement appended to `<qty> × <item> accepted. <totalValue> credited. <sold>`.
- **Fallback Behavior:** Unknown or null factions resolve deterministically to `NeutralFactionVoice` ("The counterparty has no recorded voice.", "Transaction declined.", "Item accepted.").
- **Persistence Model:** Static catalog in JSON; volatile in-memory log capped at 64 entries. Zero save migration needed.

## Final Faction Set

| Faction ID | Display Identity | Register | Source Authority | Dispatch Producer | Status |
|---|---|---|---|---|---|
| `faction_the_office` | The Office | `bureaucratic` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Baseline Preserved |
| `faction_the_cutters` | The Cutters | `salvage` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Baseline Preserved |
| `faction_the_fleet` | The Fleet | `maritime` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Baseline Preserved |
| `faction_black_flotilla` | The Black Flotilla | `privateer` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Added (Substituted Kittiwake) |
| `faction_supply_corps` | The Supply Corps | `allocation` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Added (Substituted Estuary Camp) |
| `faction_railway_guild` | The Railway Guild | `logistics` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Added (Substituted Ice Road Guild) |
| `faction_hydro_barons` | The Hydro Barons | `monopoly` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Added (Substituted Lamplighters) |
| `faction_ordnance_foundry` | The Ordnance Foundry | `foundry` | `holdfast_factions.json` | `HoldfastTerminalPanel` | Added (Substituted Quarantine Post) |

## Source-Candidate Reconciliation
- **Lamplighters:** Substituted. In Holdfast, lighting the route is Cutter responsibility (Ivy Corrigan is a Cutter with role "Lamplighter" at KM 19). In Crossing, `faction_the_lamplighters` exists as a local municipal lighting guild. Replaced in Holdfast trade by `faction_hydro_barons` (deep aquifer & water treatment authority).
- **Estuary Camp:** Substituted. Settlement/evaporation basin `loc_estuary_camp`, not a faction. Substituted by `faction_supply_corps` (the canonical District 8 allocation and ration bureau).
- **Kittiwake:** Substituted. Aground survey launch vessel with sonar rig and logbook, not a faction. Substituted by `faction_black_flotilla` (the canonical maritime salvage privateers on the coastal shelf).
- **Ice Road Guild:** Substituted. The ice road is operated and piloted by The Cutters (`faction_the_cutters`). Creating a duplicate guild would violate Invariant 3.5. Substituted by `faction_railway_guild` (the southern rail freight transport authority).
- **Quarantine Post:** Substituted. In Crossing, a gate checkpoint. In Holdfast, quarantine is an administrative procedure on Block C. Substituted by `faction_ordnance_foundry` (the industrial forge and munitions manufacturing authority).

## Existing-Three Parity
- **Office:** Verbatim match across register, voice, rejected, and sold.
- **Cutters:** Verbatim match across register, voice, rejected, and sold.
- **Fleet:** Verbatim match across register, voice, rejected, and sold.

## Cross-Plan Integration
- **Plan 117 (Holdfast Quests):** Aligns with quest actors (Ivy Corrigan as Cutter Lamplighter, Edor Vale as Office Clerk, divers from Black Flotilla, District 8 storekeepers from Supply Corps).
- **Plan 120 (Crossing Factions):** Reconciles shared world lore; Crossing factions (`faction_the_lamplighters`, `faction_the_quarantine_post`) operate in the Crossing charter basin, while Holdfast trade operates via canonical Holdfast factions without conflicting keys or duplicated authorities.
- **Plan 92 (Faction War Dialogue):** Voice styles harmonize with ambient faction dialogue without copying strings.
- **Plan 95 / 89:** Downstream systems remain decoupled.

## Validation Evidence
- **Data Integrity:** `godot --headless --path . -- --data-integrity-selftest` passed with 0 findings across 298 catalogs.
- **Holdfast Self-Test:** `godot --headless --path . -- --holdfast-selftest` passed 25/25.
- **Holdfast Runtime UI Test:** `godot --headless --path . -- --holdfast-runtime-uitest` passed after updating its catalog census from the pre-expansion 3 to the live 9.
- **Unit Tests:** `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passed 10,045/10,045 (0 failed).
- **Content Utilization:** `godot --headless --path . -- --content-utilization-selftest` passed CI gate.
- **Scene Binding:** `godot --headless --path . -- --scene-binding-selftest` passed 25/25.
- **Scene Lint:** `python3 scripts/ci/scene-lint.py` checked 30 scenes, 0 errors, 0 warnings.
- **Host Build:** `dotnet build Ashfall.csproj` succeeded with 0 errors, 0 warnings.
- **Dual-Path Sync:** `builds/linux/Assets/StreamingAssets/Data/holdfast_flavor.json` synced and verified identical.

## Deviations from Draft Plan
- Rather than introducing non-faction entries (`faction_the_kittiwake`, `faction_the_estuary_camp`, `faction_the_ice_road_guild`) which would violate repository invariants and remain unreachable in `HoldfastTerminalPanel`, all five expanded slots were populated with real, canonical, dispatch-reachable Holdfast factions from `holdfast_factions.json`.
