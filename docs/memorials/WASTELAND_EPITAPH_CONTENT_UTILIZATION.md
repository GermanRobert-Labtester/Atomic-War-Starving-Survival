# Wasteland Grave Epitaphs — Content Utilization & Utilization Gate

**File Under Test:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`
**Registry Role:** `GAMEPLAY_CONSUMED`
**Consumer:** `MemorialSystem`, `MemorialPanel`, Micro-location grave events

---

## 1. Content Utilization Audit

In `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`:
- `wasteland_grave_epitaphs.json` is registered under `loaderPatterns` mapped to `MemorialSystem`.
- `registryMap` assigns it to `MemorialSystem`.
- `runtimeConsumers` maps it to `MemorialSystem`.
- `uiConsumers` maps it to `MemorialPanel`.
- `codexConsumers` registers it as an authoritative text source.

---

## 2. Utilization Metrics

- **Total Records:** 30
- **Reachable Records:** 30/30 (100%)
- **Dead / Unused Records:** 0
- **Supported Causes:** 17 distinct classification keys covering all 16 requested game causes + `unspecified` fallback.
- **CI Gate Status:** Verified clean with `--content-utilization-selftest` (PASS).
