# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — production consumer wired, verified by per-file reachability.

---

## Integration update (2026-09-27) — `UndergroundEconomyPressure`

`UndergroundEconomyPressure` (market temperature bands, price pressure, attention risk) was a
pure authored view over existing ledger state with **zero production consumers**. It was
reachable only through its unit test.

**What shipped.**

- **`src/Host/BlackMarketHostSession.cs`** — `GetMarketPressure(syndicateId)` and
  `GetHottestMarketPressure()` project the pressure from that contact's **live** underworld
  ledger: heat and trust are read straight from the owning `BlackMarketSystem` ledger. Heat is
  clamped to `BlackMarketSystem.HeatMax`, trust to 0..100.
- **`src/UI/BlackMarketPanel.cs`** — each contact now renders a truthful market line
  (`BAND`, price multiplier, scrutiny multiplier) plus the projection's `StatusSummary`.

No new ledger, no price book, no mutable pressure store — no parallel authority.

**Evidence.** `grep -rlw UndergroundEconomyPressure src/ --include=*.cs | grep -v HostCli`
→ `src/Host/BlackMarketHostSession.cs`. `UndergroundEconomyPressureTests` (35s) pass.

---
