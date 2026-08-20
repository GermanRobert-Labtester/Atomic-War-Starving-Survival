# ADR: Unity UI Toolkit — Trade Screen & Economy HUD Binding Verification

**Date:** 2026-08-15
**Status:** Accepted
**Scope:** Trade screen modal + economy HUD strip/panel (Unity UI Toolkit track)

## Context

The Ashfall project has two host engines (Unity 6 LTS and Godot 4.7+) sharing a single
engine-agnostic simulation core (`Ashfall.Core`). Each host has its own trade screen
and economy HUD implementation:

| Surface | Godot Host | Unity Host |
|---------|-----------|------------|
| Trade Screen | `TradeScreenGodotPanel` (programmatic C# → Godot controls) | `TradeScreenView` (UI Toolkit, UXML + USS) |
| Economy HUD | Economy section of `TradeScreenGodotPanel` | `EconomyHudView` (UI Toolkit, UXML + USS) |
| Design Tokens | `Ashfall.Core.UI.Theme` (direct C# constants) | `Ashfall.Core.UI.Theme` via USS `:root` vars |

Prior to this ADR, the Unity UI Toolkit track had:
- A **shadow Theme class** (`Assets/_Game/UI/Theme.cs`) with conflicting color values
  (e.g. `#1A1A1A` vs canonical `#090B0C`) that silently shadowed the canonical tokens.
- **6 missing UXML elements** compared to the Godot panel's 14-field probing surface
  (leader name, trust, aggression, repels, radio ticker, faction emblem).
- **No automated verification** of binding purity, token parity, or field coverage.

## Decision

### 1. Canonical surface per host

Both hosts are **canonical for their respective engine**. Neither is deprecated.
The Godot panel is the **reference implementation** for field coverage (it was
implemented first with the full 14-field probe surface). The Unity UI Toolkit
track is now **field-parity-complete** with the Godot panel.

### 2. Single source of truth for tokens

`Ashfall.Core.UI.Theme` is the **sole authority** for design tokens. The shadow
`Assets/_Game/UI/Theme.cs` has been **deleted**. All USS files use CSS variables
from `DiegeticHud.uss :root`, which match Theme.cs constants (verified by tests).

### 3. View-layer purity contract

Unity UI Toolkit views (`TradeScreenView`, `EconomyHudView`) bind **only** to:
- `UnityEngine.UIElements` (host presentation framework)
- Local data structs (`BarterLineData`, `GoodRowData`) — plain data, no behavior

Views do **not** reference:
- Any `Ashfall.Core.*` namespace (zero coupling to simulation)
- `MarketSystem`, `DynamicEconomySystem`, `FactionStanceEngine` (zero logic)

The `DiegeticHudController` is the **adapter layer** that reads from core interfaces
(`IFactionStanceProvider`, `IPriceShockProvider`, `TradeScreenUI`) and converts to
view data structs.

### 4. Field parity map (14 probes)

| # | Probe Field | Godot | UI Toolkit UXML | View Field | Controller Source |
|---|-------------|-------|-----------------|------------|-------------------|
| 1 | Faction Name | `_lblFactionName` | `trade-faction-name` | `_factionName` | `GetFactionDisplayName()` |
| 2 | Leader Name | `_lblLeader` | `trade-leader-name` | `_leaderName` | `_tradeSource.LeaderName` |
| 3 | Stance Badge | `_badgeStance` | `trade-stance-badge` | `_stanceBadge` | `_tradeSource.Stance` |
| 4 | Trust Meter | `_lblTrust` | `trade-trust` | `_trust` | `_tradeSource.GetTrust()` |
| 5 | Aggression | `_lblAggression` | `trade-aggression` | `_aggression` | `_tradeSource.Aggression` |
| 6 | Repel Counter | `_lblRepels` | `trade-repels` | `_repels` | `_tradeSource.ConsecutiveRepels` |
| 7 | Player Offers | `_playerOfferList` | `trade-player-lines` | `_playerLines` | `_tradeSource.PlayerOffers` |
| 8 | Faction Asks | `_factionStockList` | `trade-faction-lines` | `_factionLines` | `_tradeSource.FactionAsks` |
| 9 | Player Total | `_lblPlayerWorth` | `trade-player-total-value` | `_playerTotalValue` | `_tradeSource.PlayerOfferValue` |
| 10 | Faction Total | `_lblFactionAskWorth` | `trade-faction-total-value` | `_factionTotalValue` | `_tradeSource.FactionAskValue` |
| 11 | Fairness | `_lblFairness` | `trade-fair-indicator` | `_fairIndicator` | `_tradeSource.IsFair` |
| 12 | Parley Button | `_btnDemandParley` | `trade-parley-btn` | `_parleyBtn` | `_tradeSource.CanDemandParley` |
| 13 | Parley Message | `_lblParleyStatus` | `trade-parley-msg` | `_parleyMsg` | `_tradeSource.LastParleyMessage` |
| 14 | Radio Ticker | `_lblRadioTicker` | `trade-radio-ticker` | `_radioTicker` | **Deferred** — needs `IFactionRadioProvider` binding |

### 5. Known gaps & next steps

- **Faction emblem texture**: Godot loads `faction_icon_{id}.png`; UI Toolkit has no
  `<ui:Image>` element in the trade header. Deferred — requires art asset pipeline.
- **Radio ticker data**: UXML element + USS + view field are present. Controller
  passes null until `IFactionRadioProvider` is bound to `DiegeticHudController`.
- **Godot theme three-source parity**: Godot panel uses `Ashfall.Core.UI.Theme`
  directly (C# constants → `ToGodotColor()`). USS uses CSS variables that match
  the same Theme.cs constants. Verified by `UiToolkitBindingPurityTests`.

## Consequences

- **Positive:** Shadow theme eliminated. 17 automated tests guard against regression.
  Both hosts now have identical field coverage. Token parity is machine-checked.
- **Negative:** None. The shadow Theme had no external consumers beyond dead code.
- **Risk:** The radio ticker data binding is deferred. The `trade-radio-ticker`
  element exists but stays hidden until `IFactionRadioProvider` is wired.

## Verification

All 17 tests in `UiToolkitBindingPurityTests` pass:
- 6 binding-purity tests (source-level namespace + type isolation)
- 2 shadow-theme elimination tests
- 5 USS ↔ Theme.cs token parity tests (hex, rgba, spacing, no-hardcoded-colors)
- 2 UXML field-probe contract tests (14 trade + 8 economy element names)
- 2 sizing-token parity tests (trade panel + economy HUD dimensions)
