# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — zero production consumers before; wired, built, verified.

---

## Integration update (2026-09-27) — `NeedsComponentParity`

`NeedsComponentParity.Compare(NeedsSystem, NeedsComponentStore)` is the authored
dual-run parity comparator for the needs migration boundary (“the comparison
deliberately does not tick or mutate either side”). It was documented and tested
but never run — the dual-run phase had no consumer.

**What shipped.**

- **`src/Host/SurvivorsHostSession.cs`** — `BuildNeedsParityReport()` mirrors the live
  `NeedsSystem` roster into a `NeedsComponentStore` through the typed
  `SurvivorId.TryParse` key and returns
  `NeedsComponentParity.Compare(Needs, typed)`.
- **`src/Main.Survivors.cs`** — `SaveSurvivors()` runs the check once per save and
  reports a mismatch through the log. Unparseable legacy ids are themselves a
  parity finding, not silently skipped.

**Evidence.** `grep -rlw NeedsComponentParity src/ --include=*.cs | grep -v HostCli`
→ `src/Host/SurvivorsHostSession.cs`. `NeedsComponentStoreTests` (47s, includes the
parity suite) pass.

**Rule 5:** `NeedsSystem` remains the gameplay authority; the typed store is only
the migration mirror, and the comparator never reconciles by guessing which side
is right — it reports.

---
