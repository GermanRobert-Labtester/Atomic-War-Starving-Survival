# UI Audit + Developer Session Asset Inspector (all categories)

> **STATUS: APPROVED BY USER** — authorized by the user's direct request in this
> session: "audit the UI, diagnose and repair ... settings menu functionality,
> wiring in inventory assets and building a start game option which is a
> developer session where i can check all items and their assets".

## Bounded outcome

Deliver a read-only **Developer Session — Asset Inspector** reachable from
the main menu, so the developer can enumerate every authored entry across all
asset categories (items, portraits, locations, factions) and see, per entry,
whether its art asset resolves (and to which path) or falls back to a
placeholder. Adds no gameplay authority and no save section.

## Non-goals

- No gameplay / Core / save changes.
- No changes to `AssetRegistry`, `AssetCoverageScanner`, or catalog data.
- Not registered as a player-navigable `PanelRegistry` route (avoids the
  generated manifest / live-count / liveness gates; this is a dev overlay).
- Broad visual redesign of every panel is out of scope for this tranche (see
  audit findings for the roadmap).

## Current evidence (audit)

- Inventory art wiring is **already healthy**: 953 / 969 unique authored item
  IDs resolve an `assets/art/item_*.jpg` (or `*.png`) file; the 16 remaining
  generic items (`item_clean_water`, `item_fuel`, `item_medical_kit`, ...) rely
  on `AssetRegistry.ItemIdAliases` / fallback icon by design.
- `AssetRegistry.GetItem` + `AshfallUiHelpers.MakeItemIcon` already probe
  `res://assets/art/{key}.jpg|.png` and fall back to a placeholder icon.
- Motion/animation system already exists (`UiMotion`: open/close tweens,
  button focus/hover FX); `UiPanelFlow` also present.
- `SettingsPanel` is already feature-rich (window mode, resolution, UI scale,
  vsync, max fps, 5 volume channels, language, colorblind, high contrast,
  hazard labels, reduced motion, large fonts, tutorial, autosave).

## Files owned (this claim)

- `src/UI/AssetInspectorPanel.cs` (new) — the inspector overlay (all categories).
- `src/UI/MainMenuPanel.cs` — add `OnInspectorRequested` event + DEV SESSION button.
- `src/Main.UiPanels.cs` — instantiate panel, wire open, add field.
- `src/Main.PanelLifecycle.cs` — add to `OverlayPanelCatalog()` for Esc/globals.
- `.ai/state.md` — task state.

## Verification

- `dotnet build` of the Godot host target (0 errors).
- Scoped UI source gates: `AccessibilitySourceAuditTests`,
  `ProductionUiNoFabricatedFallbackGateTests`, `PanelSubscriptionHygieneTests`,
  `PanelLiveRefreshGateTests`, `PanelRouteGateTests`.
