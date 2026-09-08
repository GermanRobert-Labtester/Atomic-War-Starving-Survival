# Plan 120 Closeout Report — Crossing Factions Expansion (3 → 8 Factions)

## 1. Executive Summary

Plan 120 successfully expanded `crossing_factions.json` from the verified baseline of 3 charter factions to exactly **8 factions**, deepening the multi-faction arbitration political ecology of Nobody's Charter (`expansion_nobodys_charter`).

The implementation is **pure data + minor catalog ID registration & demo capacity update**:
- Data catalogs updated: `Assets/StreamingAssets/Data/crossing_factions.json` and `builds/linux/Assets/StreamingAssets/Data/crossing_factions.json`.
- Core constants added: `CrossingIds.FactionLamplighters`, `FactionGranaryWardens`, `FactionWaterCommittee`, `FactionQuarantinePost`, `FactionSmugglersCourt`.
- Core demo updated: `CrossingHeadlessDemo.cs` expanded from checking 3 factions to checking `>= 8` factions and verifying all eight individual faction records resolve.
- Automated tests authored: `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs` (16 comprehensive tests, 100% green).

---

## 2. Final Eight-Faction Roster

1. **The Scale (`faction_the_scale`):** Weights and measure verification at Stallrow Deck Scale. Wants `trade_goods`, offers `stallrow_trade_access`, `verification`. Alignment: `conditional`.
2. **The Underwrite (`faction_the_underwrite`):** Commercial risk underwriting and brutal forfeit liens. Wants `pledged_goods`, offers `seed_stock`, `covered_loss`, `favour_bank`. Alignment: `conditional`.
3. **The Compact (`faction_the_compact`):** Charter drafting and citizen assembly democracy. Wants `signatories`, offers `charter_draft`, `ratification`. Alignment: `peaceful`.
4. **The Lamplighters (`faction_the_lamplighters`):** Viaduct street lighting, nocturnal escort, and oil cistern management. Wants `fuel_stores`, `reflector_glass`, offers `street_lighting`, `night_watch_escort`, `route_visibility`. Alignment: `conditional`.
5. **The Granary Wardens (`faction_the_granary_wardens`):** Concrete silo storage, moisture inspection, and emergency caloric ration release. Wants `staple_grain`, `burlap_sacks`, offers `ration_distribution`, `emergency_grain_draw`, `spoilage_inspection`. Alignment: `conditional`.
6. **The Water Committee (`faction_the_water_committee`):** Wellhead pumping, sand filtration beds, and clean water allocation. Wants `filter_media`, `sanitation_salts`, offers `clean_water_rights`, `well_head_access`, `quality_certification`. Alignment: `conditional`.
7. **The Quarantine Post (`faction_the_quarantine_post`):** Viaduct gate health inspection, pulmonary screening, and epidemic containment. Wants `medical_tinctures`, `respirator_masks`, offers `gate_health_screening`, `quarantine_clearance`, `outbreak_warning`. Alignment: `neutral`.
8. **The Smugglers' Court (`faction_the_smugglers_court`):** Unmapped viaduct drainage culverts, off-book trade, and informal dispute mediation. Wants `unchartered_salvage`, `route_intelligence`, offers `off_ledger_trade`, `culvert_transit`, `discreet_arbitration`. Alignment: `conditional`.

---

## 3. Verified Verification Evidence

- `dotnet test --filter CrossingFactionExpansionTests`: **16/16 passed**
- `dotnet test --filter ExpansionsIntegrationTests`: **10/10 passed**
- `godot --headless --path . -- --crossing-selftest`: **PASS 40/40 checks**
- `godot --headless --path . -- --data-integrity-selftest`: **PASS (0 findings across 298 catalogs)**
- `godot --headless --path . -- --content-utilization-selftest`: **PASS (CI Content Utilization Gate: PASS)**
- `godot --headless --path . -- --scene-binding-selftest`: **PASS (25/25 passed)**
- `python3 scripts/ci/scene-lint.py`: **Clean (0 errors, 0 warnings)**
- `dotnet build Ashfall.csproj`: **Clean (0 errors, 0 warnings)**
