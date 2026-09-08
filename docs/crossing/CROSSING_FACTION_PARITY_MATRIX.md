# Crossing Faction Parity Matrix

## 1. Complete Eight-Faction Roster

| # | Faction ID | Display Name | Alignment | Region | Active | Trust | Wants | Offers |
|---|---|---|---|---|---|---:|---|---|
| 1 | `faction_the_scale` | The Scale | conditional | `region_crossing` | true | 0 | `trade_goods` | `stallrow_trade_access`, `verification` |
| 2 | `faction_the_underwrite` | The Underwrite | conditional | `region_crossing` | true | 0 | `pledged_goods` | `seed_stock`, `covered_loss`, `favour_bank` |
| 3 | `faction_the_compact` | The Compact | peaceful | `region_crossing` | true | 0 | `signatories` | `charter_draft`, `ratification` |
| 4 | `faction_the_lamplighters` | The Lamplighters | conditional | `region_crossing` | true | 0 | `fuel_stores`, `reflector_glass` | `street_lighting`, `night_watch_escort`, `route_visibility` |
| 5 | `faction_the_granary_wardens` | The Granary Wardens | conditional | `region_crossing` | true | 0 | `staple_grain`, `burlap_sacks` | `ration_distribution`, `emergency_grain_draw`, `spoilage_inspection` |
| 6 | `faction_the_water_committee` | The Water Committee | conditional | `region_crossing` | true | 0 | `filter_media`, `sanitation_salts` | `clean_water_rights`, `well_head_access`, `quality_certification` |
| 7 | `faction_the_quarantine_post` | The Quarantine Post | neutral | `region_crossing` | true | 0 | `medical_tinctures`, `respirator_masks` | `gate_health_screening`, `quarantine_clearance`, `outbreak_warning` |
| 8 | `faction_the_smugglers_court` | The Smugglers' Court | conditional | `region_crossing` | true | 0 | `unchartered_salvage`, `route_intelligence` | `off_ledger_trade`, `culvert_transit`, `discreet_arbitration` |

---

## 2. Parity Invariants

1. **Baseline Preservation:** Indices 0, 1, 2 correspond to The Scale, The Underwrite, and The Compact, matching their historical attributes byte-for-byte.
2. **Distinct Economic Niches:** Every faction has a unique profile of wants and offers with zero duplication.
3. **Plausible Starting Disposition:** Starting trust is 0 across all factions, reflecting institutional caution rather than unearned familiarity.
