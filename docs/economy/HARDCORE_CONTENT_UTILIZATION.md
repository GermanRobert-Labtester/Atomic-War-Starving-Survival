# Hardcore Content Utilization

## 1. Scanner Alignment & Consumer Registry

`hardcore_economy_tuning.json` is tracked by `ContentUtilizationScanner.cs`:

- **Primary Consumer:** `HardcoreEconomyTuningLoader`
- **Secondary Systems:** `MarketSystem`, `TradeScreenPresenter`, `TradeScreenGodotPanel`, `HostCli.PanelTests.cs`

```mermaid
graph TD
    JSON[hardcore_economy_tuning.json] --> Loader[HardcoreEconomyTuningLoader.Load]
    Loader --> Bundle[HardcoreEconomyTuningBundle]
    Bundle --> Tuning[HardcoreEconomyTuning Overlay]
    Tuning --> Presenter[TradeScreenPresenter / Badges]
    Tuning --> Market[MarketSystem / Price Evaluation]
    Tuning --> Panel[TradeScreenGodotPanel]
```

### CI Verification
- Verified by `--content-utilization-selftest` (PASS).
- Verified by `--data-integrity-selftest` (PASS, 0 errors).
- Gated by `CatalogIntegrityValidatorTests`.
