# Plan 145 UI Surface Matrix

Specification of user interface rendering surfaces, presentation standards, and display contracts for graffiti and wall text.

## 1. Primary Surfaces

| Surface | Host Node / Class | Trigger / Interaction | Display Format |
|---|---|---|---|
| **Shelter Room Inspection** | `ShelterPanel` (`src/UI/ShelterPanel.cs`) | Room selected via `HoldfastInteriorView` hotspot or room navigation | Dedicated `"WALL MARKINGS & CORRIDOR TEXT"` card within Overview/Room detail |
| **Shelter Room Tooltips** | `HoldfastInteriorView` (`src/World/HoldfastInteriorView.cs`) | Hover over room hotspot in 2D shelter view | Short ambient line in room tooltip: `Wall: "{content_snippet}"` |
| **Wasteland Location Detail** | `MapDetailPanel` (`src/UI/MapDetailPanel.cs`) | Location selected on Wasteland Map | Dedicated `"FIELD MARKINGS & ENVIRONMENTAL TEXT"` card in sector intelligence |

## 2. Presentation Standard (No Database Dumps)

Graffiti is environmental narrative texture, not a spreadsheet. The presentation must follow these design standards:

1. **Prominent Prose:** The inscription `content` is primary, styled using `DesignTheme.FontSizeBody` with high legibility (`DesignTheme.Pale` or `DesignTheme.Warm`).
2. **Contextual Inscription Medium:** `medium` (e.g. *White chalk on cast-iron pipe*, *Scratched with a nail*) rendered as secondary italic/small metadata (`DesignTheme.Muted`).
3. **Attribution & Chronology:** `author_signature` and `Day {recorded_day}` displayed subtly (e.g. `— Fyodor the Stoker · Day 3`).
4. **No Raw IDs:** Never display `posting_id`, internal enum names, or raw JSON keys to the player.
5. **No Decorative Mechanical Claims:** `morale_effect` is authoring tone and must NOT be shown as an interactive button, buff icon, or numeric stat modifier.
6. **Graceful Degradation:** If a room or location has no eligible markings on the current day, the section either gracefully omits the card or displays a quiet atmospheric fallback (`"The walls here bear no legible markings."`).
