# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — zero production consumers before; wired, built, verified.

---

## Integration update (2026-09-27) — `RegionalTreatyFeed`

`RegionalTreatyFeed` is the single authored interpreter from the narrative
`RegionalTreatyEntry` schema into the treaty system's own `TreatyDefinition`. It
was never called: `SetupRegionalTreaty` loaded **only** the mechanical
`regional_treaties.json`, guarded by a comment asserting that narrative protocols
and foundry accords “are different schemas and must not be fed into this
system.” That comment is the reason 34 authored treaties never became gameplay.

**What shipped.**

- **`src/Main.ShelterSocial.cs`**
  - `LoadNarrativeTreatyDefinitions` reads `foundry_accords.json` (18 treaties) and
    `narrative/regional_treaty_protocols.json` (16 treaties) through
    `RegionalTreatyCatalog` and maps them with `RegionalTreatyFeed.Map` — the one
    interpretation point. Both corpora land as `TreatyDefinition` on the **same**
    `RegionalTreatySystem`, alongside the mechanical catalog.
  - The misleading comment is replaced with the actual rule.
  - Absent corpora are not an error; an unreadable one is reported, never swallowed.

**Evidence.** `grep -rlw RegionalTreatyFeed src/ --include=*.cs | grep -v HostCli`
→ `src/Main.ShelterSocial.cs`. `RegionalTreatyFeedTests` pass.

**Rule 5:** the treaty system stays the single authority; no second registry,
no verbatim copy of the mechanical catalog, no panel-side treaty state.

---
