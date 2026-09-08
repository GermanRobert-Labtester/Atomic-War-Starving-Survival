# Holdfast Flavor Authority Map

## Separation of Authority

| Domain Fact | Authoritative Source | Plan 128 Responsibility |
|---|---|---|
| Faction Existence & Schema | `Assets/StreamingAssets/Data/holdfast_factions.json` | Read-only reference; key by canonical ID |
| Faction Alignment, Wants & Offers | `holdfast_factions.json` & `FactionStanceEngine.cs` | Read-only reference; do not duplicate |
| Faction Trust & Standing | `HoldfastTradeSession.cs` & save store | Do not alter; flavor is an overlay |
| Trade Execution (Buy/Sell/Reject) | `HoldfastTradeSession.cs` & `HoldfastTradeResult` | Presentation only; never mutates result |
| Trade Inventory & Stock | `HoldfastTradeSession.cs` & `holdfast_items.json` | Read-only presentation of stock counts |
| Item Catalog & Properties | `Assets/StreamingAssets/Data/holdfast_items.json` | Reference canonical item IDs in logs |
| In-Universe Item Marginalia | `holdfast_flavor.json` (`items` dict) | Preserved 40 baseline entries verbatim |
| Dispatch Flavor Copy | `holdfast_flavor.json` (`factions` dict) | Expanded 3 → 8 canonical faction profiles |
| Dispatch Log Formatting | `src/Host/HoldfastDispatchLog.cs` | Consumed as presentation overlay |
| Quest Progression & Gating | `HoldfastQuestSystem.cs` | Independent runtime; no flavor coupling |
| Log Persistence | Volatile in-session (`HoldfastDispatchLog._entries`) | Re-rendered on demand; no save migration |

## Roster exception

The trade roster contains the pre-existing `faction_scavengers` compatibility
entry in addition to the eight authored faction profiles. It intentionally
uses `HoldfastFlavorCatalog.NeutralFactionVoice`; this keeps flavor
presentation separate from the global Scavengers standing/travel content and
does not create a second trust or save authority.
