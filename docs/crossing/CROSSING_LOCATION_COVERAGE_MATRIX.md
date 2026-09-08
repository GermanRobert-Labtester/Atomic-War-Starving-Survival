# Crossing Location Coverage Matrix

## Authority and method

Location authority is `Assets/StreamingAssets/Data/crossing_locations.json`,
loaded by `CrossingCatalogLoader`. Plan 115 validates every encounter
`target_location` against its 13 location IDs. The encounter loader itself
does not perform this validation.

The repository baseline was 14 encounters, not the stale 10-entry plan
baseline. The table counts all 25 final records after adding 11 new
encounters.

| Location ID | Display name | Baseline encounters | Final encounters | New Plan 115 use | Profile |
|---|---|---:|---:|---|---|
| `loc_crossing_viaduct_gate` | The Viaduct Gate | 6 | 10 | Frozen Barge; Ice Fracture; Child at the Gate | gate, viaduct, Drown edge, admission |
| `loc_crossing_scalehouse` | The Scalehouse | 0 | 2 | Bonded Caravan Ambush; Contaminated Ford | manifests, cargo verification |
| `loc_crossing_stallrow` | Stallrow | 5 | 5 | — | market and public claims |
| `loc_crossing_watchtower` | The Watchtower | 0 | 2 | Smuggler Checkpoint; Collapsed Crossing | route watch and boundary observation |
| `loc_crossing_weighbridge` | The Deck Scale | 0 | 1 | Toll Bridge Claim | common passage and load control |
| `loc_crossing_underwrite_hall` | The Underwrite Hall | 2 | 2 | — | debt and collateral |
| `loc_crossing_records_room` | The Records Room | 0 | 0 | — | claims and historical record |
| `loc_crossing_the_lockup` | The Lockup | 0 | 0 | — | pledged goods |
| `loc_crossing_granary_pledge` | The Pledged Granary | 0 | 0 | — | visible debt collateral |
| `loc_crossing_nightfire` | The Nightfire | 0 | 0 | — | informal settlement |
| `loc_crossing_petition_tent` | The Petition Tent | 0 | 1 | Refugee Blockade | petitions and charter drafting |
| `loc_crossing_founders_marker` | The Founders' Marker | 0 | 1 | Uncleared Field | origin claim and unsafe approach |
| `loc_crossing_the_annex` | The Annex | 0 | 1 | Family at the Ledger Gate | sponsorship and admission |

## Distribution result

- Locations represented: **4 → 10**
- Maximum concentration: **6 → 9** at the Viaduct Gate
- New encounter spread: **4 route/trade, 4 hazard, 3 social/admission**
- Zero-encounter locations remain because no new locations or runtime
  location routing are in scope.

The Viaduct Gate remains intentionally central to Crossing access, but the
new catalog adds Scalehouse, Watchtower, Deck Scale, Petition Tent,
Founders' Marker, and Annex activity without inventing future location IDs.
