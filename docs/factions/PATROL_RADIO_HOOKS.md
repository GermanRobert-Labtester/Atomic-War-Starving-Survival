# Plan 45 / F14 — Patrol Radio Hooks Specification

## Overview

The `PatrolRadioHooks` system provides the canonical bridge connecting travel encounters (specifically human faction patrols) to atmospheric and intel radio transmissions in ASHFALL. Resolving specific patrol encounters queues relevant faction broadcast intercepts, creating believable narrative ripples across the wasteland without breaking Core engine-agnosticism.

## Encounter to Radio Signal Mappings

| Encounter ID / Group | Mapped Broadcast ID | Origin Faction | Frequency (MHz) | Signal Type |
|---|---|---|---|---|
| `enc_patrol_garrison_checkpoint` (v1/v2/v3) | `radio_patrol_garrison_checkpoint` | `military_remnants` | 88.4 | Checkpoint operational / toll notice |
| `enc_patrol_warlord_raid` (v1/v2/v3) | `radio_patrol_warlord_raid` | `upland_militia` | 104.2 | Raider warning on toll road |
| `enc_patrol_central_garrison_border` | `radio_patrol_border_closed` | `military_remnants` | 88.4 | Border closure / transit denial |
| `enc_patrol_railway_convoy` | `radio_patrol_convoy_attacked` | `military_remnants` | 88.4 | Armoured transport under fire |
| `enc_patrol_warlord_press_gang` | `radio_patrol_press_gang` | `upland_militia` | 104.2 | Refugee warning / forced conscription |

## Faction Radio Capabilities

In accordance with ASHFALL worldbuilding and signal lore:

- **Radio-Capable Factions**: Authorized to originate tactical and civil radio broadcasts:
  - `iron_garrison` / `military_remnants` (Overlord Actual / Garrison Net, 88.4 MHz)
  - `faction_central_garrison` (Central Command, 88.4 MHz)
  - `faction_railway_guild` (Railway Dispatch, 92.6 MHz)
  - `upland_militia` (Ridge Watch / Civil Defense, 104.2 MHz)
- **Non-Radio Factions**: Do not originate direct broadcasts; activity involving them is intercepted or reported by radio-capable entities:
  - `warlords_sector_4` (Raids and press gangs reported by Upland Militia)
  - `faction_scavengers` (Reported by military/militia nets)
  - `cult_of_ash_sign` / `cult_of_the_glow` (Observed/warned against by organized factions)

## Emission & Persistence Semantics

1. **One-Shot Semantics**: Each patrol radio signal may only be triggered once per campaign. Dispatched signals are permanently recorded in `ConsumedSignals` to prevent repetitive chatter.
2. **Pending Queue**: Signals queued during travel are held in `PendingSignals` until the next radio processing cycle (`TickRadio()`), where they are transferred to `ConsumedSignals` and returned for display/playback.
3. **State Persistence**:
   - `PatrolRadioHooksState` serializes `ConsumedSignals` and `PendingSignals` as part of the campaign envelope.
   - Saves cleanly round-trip and prevent duplicate broadcasts after loading.
