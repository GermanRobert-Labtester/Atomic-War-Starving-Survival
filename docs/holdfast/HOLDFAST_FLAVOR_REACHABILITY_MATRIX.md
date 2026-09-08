# Holdfast Flavor Reachability Matrix

## Runtime Selection Flow
1. User opens Holdfast Terminal (`HoldfastTerminalPanel`).
2. Terminal calls `_session.Catalog.Factions` (`holdfast_factions.json`).
3. All 9 canonical Holdfast factions are present in the list and can be selected via `SelectFaction(factionId)`.
   Eight have specialized Plan 128 voice profiles; `faction_scavengers` intentionally uses the neutral fallback.
4. User executes a transaction (`PressBuy()`, `PressSell()`, or triggers a rejection).
5. `_dispatch` calls `_flavor.GetFactionVoice(factionId)`.
6. `HoldfastFlavorCatalog` does an exact lookup in `FactionVoices[factionId]`.
7. The specialized voice strings are returned and rendered.

## Reachability Audit per Faction

| Faction ID | Present in `holdfast_factions.json`? | Selectable in UI? | Trade Execution Path | Fallback Required? |
|---|---|---|---|---|
| `faction_the_office` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_the_cutters` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_the_fleet` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_black_flotilla` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_supply_corps` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_railway_guild` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_hydro_barons` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_ordnance_foundry` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | No (Direct Match) |
| `faction_scavengers` | Yes | Yes (`_factionList`) | `PressBuy()`, `PressSell()`, `PressBuy()` failure | Yes (NeutralFallback) |
| `faction_nonexistent` (test) | No | No (Raw test only) | `PressBuy()` failure | Yes (NeutralFallback) |
