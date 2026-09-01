# Shelter Decor Item Matrix — Plan 12C

Twelve authored decor items with stable IDs, categories, acquisition paths, and morale modifiers.

| # | Item ID | Display Name | Category | Slot Compatibility | Morale Delta | Acquisition |
|---|---------|-------------|----------|-------------------|-------------|-------------|
| 1 | `item_decor_poster_ration` | Ration Propaganda Poster | poster | wall | 1.5 | Scavenge / trade |
| 2 | `item_decor_poster_warning` | Warning Sign | poster | wall | 1.2 | Scavenge / craft |
| 3 | `item_decor_locomotive_nameplate` | Locomotive Nameplate | trophy | shelf, wall | 2.0 | Expedition salvage |
| 4 | `item_decor_carved_memorial` | Carved Memorial Plaque | plaque | plaque, wall | 1.8 | Memorial outcome / craft |
| 5 | `item_decor_chalk_drawing` | Child's Chalk Drawing | drawing | wall, shelf | 1.0 | Social event (child creation) |
| 6 | `item_decor_pressed_flower` | Pressed Flower Frame | personal | shelf, wall | 0.8 | Scavenge / greenhouse |
| 7 | `item_decor_medal_civic` | Civic Service Medal | trophy | shelf, plaque | 1.5 | Quest reward / leadership |
| 8 | `item_decor_classroom_chart` | Classroom Chart | poster | wall, shelf | 1.0 | Schooling completion |
| 9 | `item_decor_signal_log` | Signal Log Book | personal | shelf | 1.2 | Apprenticeship completion |
| 10 | `item_decor_memorial_plaque_generic` | Generic Memorial Plaque | plaque | plaque | 1.6 | MemorialSystem outcome |
| 11 | `item_decor_memorial_plaque_carving` | Carved Memorial Plaque | plaque | plaque | 1.8 | MemorialSystem + keepsake |
| 12 | `item_decor_memorial_plaque_drawing` | Drawing Memorial Plaque | plaque | plaque | 2.0 | MemorialSystem + child art |

## Item Schema

Each decor item in `items.json` carries:
- `id`: stable snake_case identifier
- `name`: display name
- `description`: flavor text
- `category`: item category (decor, plaque, etc.)
- `tags`: list of tags (wall, shelf, plaque, trophy, poster, drawing, personal)
- `decorLocalizedMoraleDelta`: float, positive morale modifier
- `acquisitionPath`: how the item is obtained

## Acquisition Diversity

- **Generic loot:** poster_ration, poster_warning (scavenge/trade)
- **Expedition:** locomotive_nameplate (surface salvage)
- **Social/memorial:** carved_memorial, chalk_drawing, pressed_flower (event outcomes)
- **Quest/apprenticeship:** medal_civic, classroom_chart, signal_log (completion rewards)
- **Memorial system:** 3 plaque variants (death remembrance outcomes)

## Slot Compatibility

| Category | Valid Slots |
|----------|------------|
| poster | wall |
| trophy | shelf, wall |
| plaque | plaque, wall |
| drawing | wall, shelf |
| personal | shelf, wall |

## Validation Rules

- Unknown room → reject
- Unknown slot → reject
- Occupied slot → overwrite (last write wins)
- Incompatible category → reject
- Item not owned → reject
- Item already installed elsewhere → reject
