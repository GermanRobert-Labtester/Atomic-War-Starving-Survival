# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan Quad Package L — Seal 2: Consequence Ledger Authority (Flag-Ledger De-duplication)

**STATUS: FULLY INTEGRATED** — see the package record
`docs/plans/integrated/campaign/INTEGRATED_PLAN_QUAD_PACKAGE_L_F16_PERSISTENCE_FAIL_CLOSED.md` for the full evidence.

## Seal

`src/Host/HostCli.KnockWhitelist.cs` constructed `new InMemoryFlagLedger()` twice
(lines 27 and 33), opening a second flag authority beside the campaign's single
`ConsequenceLedger` (Rule 5). The gate
`ConsequenceLedgerSourceGateTests.SourceGate_NoProductionPrivateInMemoryFlagLedgerConstruction`
proved the violation.

**Fix:** one shared `Ashfall.Core.Flags.CampaignConsequenceLedger` drives every check.
The "no flag recorded" condition for probe check 4 is expressed with `ClearAll()`
on that same ledger rather than by constructing a second one; check 5 re-sets the
gated flag so it still proves refusal is a property of the knock id, not the flags.

**Verified:** `--knock-whitelist-selftest` 6/6 PASS; zero `new InMemoryFlagLedger(`
remain anywhere under `src/`.
