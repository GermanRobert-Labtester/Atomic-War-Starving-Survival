# Plan 138 — Low-Background Radiation Metrology: Closeout

**Status:** COMPLETE · **Date:** 2026-09-19 · **Batch:** `PLANS-138-139-141-LATER-PHASES`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Shelter/LowBackgroundLeadEngine.cs`, `LowBackgroundLeadCatalog.cs` (`low_background_metrology`) |
| Data | `Assets/StreamingAssets/Data/low_background_lead_catalog.json` |
| Host | `src/Host/LowBackgroundMetrologyHostSession.cs`, `src/Host/LowBackgroundMetrologySaveStore.cs`, `src/Main.LowBackgroundMetrology.cs` |
| Save | `LowBackgroundMetrologySaveStore` (`low_background_metrology`) |
| UI | `src/UI/LowBackgroundLeadPanel.cs`, route `low_background_lead` (Expanded) |
| Tests | `Plan138LowBackgroundLeadEngineTests` 16/16; `Plan138LowBackgroundHostWiringTests` 3/3 |

## Contract guarantees

- **Metrology, not purification.** Shielding improves detector sensitivity; it never removes contamination or creates clean food magically.
- **Honest detection limits.** Low background reduces environmental noise; assays report confidence and detection limits rather than guaranteed zero activity.
- **Shield and detector degradation.** Detector requires periodic calibration; shield modules require maintenance and can become contaminated.
- **Deterministic assay math.** Seeded campaign stream provides deterministic assay variance; pure math functions govern background attenuation.
- **Canonical inventory consumption.** Batch feedstocks and calibration sources consume real items from the canonical inventory.

## Verification

- `dotnet test --filter "FullyQualifiedName~Plan138"` (19/19 PASS)
- `dotnet build Ashfall.csproj` 0 warnings / 0 errors
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` PASS
- `godot --headless --path . -- --ui-accessibility-selftest` PASS

## Non-goals preserved

No magical food purifying, no instant assays without bench time, no infinite shield life, no UI-side calculation bypasses.
