# Standing Record Faction Badge & Emblem Asset Audit

## 1. Badge Asset Contract

The `badge_asset_id` property in `standing_record_factions.json` specifies an optional visual emblem asset. Per repository invariants:
- Empty string `""` is explicitly legal and serves as the baseline default (as demonstrated in `faction_the_overlay`, `crossing_factions.json`, and `holdfast_factions.json`).
- If `badge_asset_id` is empty, runtime UI queries `FactionIconCatalog.Resolve(faction.id)`.

---

## 2. Emblem Mapping Table

| Faction ID | Authoring `badge_asset_id` | Canonical File on Disk | Mapped in `FactionIconCatalog.cs`? | Runtime Presentation Behavior |
|---|---|---|---|---|
| `faction_the_overlay` | `""` | `assets/ui/Icons/faction_icon_the_overlay.png` | **Yes** (line 75) | Renders dedicated Overlay compass & plate emblem. |
| `faction_the_scale` | `""` | `assets/ui/Icons/faction_icon_the_scale.png` | **Yes** (line 66) | Renders dedicated Scale brass balance emblem. |
| `faction_the_compact` | `""` | `assets/ui/Icons/faction_icon_the_compact.png` | **Yes** (line 65) | Renders dedicated Compact treaty scroll emblem. |
| `faction_the_underwrite` | `""` | `assets/ui/Icons/faction_icon_the_underwrite.png` | **Yes** (line 67) | Renders dedicated Underwrite shield & vault emblem. |
| `faction_the_cutters` | `""` | `assets/ui/Icons/faction_icon_the_cutters.png` | **Yes** (line 69) | Renders dedicated Cutters crossed ice-chisel emblem. |
| `faction_the_fleet` | `""` | `assets/ui/Icons/faction_icon_the_fleet.png` | **Yes** (line 70) | Renders dedicated Fleet fouled-anchor emblem. |
| `faction_the_rebuilders` | `""` | `assets/ui/Icons/faction_icon_rebuilders.png` | Available via `faction_rebuilders` | Resolves fallback or aliased Rebuilder trowel & wheat emblem. |
| `faction_the_garrison` | `""` | `assets/ui/Icons/faction_icon_central_garrison.png` / `faction_icon_garrison.svg` | Available via `faction_central_garrison` | Resolves fallback or aliased Garrison chevron emblem. |

---

## 3. Art Backlog & Future Tasks

All 8 factions are fully functional and render either their verified dedicated icon or the standard fallback emblem. A follow-up art synchronization task can optionally register exact string aliases in `FactionIconCatalog.cs` without changing data or breaking saves.
