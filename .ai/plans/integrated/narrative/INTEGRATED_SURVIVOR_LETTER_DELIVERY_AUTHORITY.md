# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — production consumer wired, verified by per-file reachability.

---

## Integration update (2026-09-27) — `SurvivorLetterDeliverySystem`

The 25 authored dead letters had a catalog, a tested delivery lifecycle (found → addressed
→ delivered/withheld/unanswered) and **zero production consumers**. No host loaded the
catalog, nothing persisted the resolved states and the morale consequence had no owner.

**What shipped.**

- **`src/Host/SurvivorLetterDeliveryHostSession.cs`** — catalog load through the shared
  `CatalogPath` authority, dweller candidates read live from the `SurvivorsHostSession` roster,
  delivery routed through `NeedsSystem` (`NeedKind.Morale`) rather than a local counter, and a
  truthful census. `GetRecord` is non-creating so callers cannot materialize records.
- **`src/Host/SurvivorLetterDeliverySaveStore.cs`** — checksummed canonical envelope save store.
- **`src/Main.SurvivorLetterDelivery.cs`** — setup / save / flush / reset plus the player-facing
  commands (`FindSurvivorLetter`, `AddressSurvivorLetter`, `DeliverSurvivorLetter`,
  `WithholdSurvivorLetter`, `DeliverSurvivorLetterTo`, `GetSurvivorLetterCensus`) and a journal
  entry on delivery.
- **`Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs`** — added `BindCatalog`
  (post-construction catalog binding for save-then-load ordering) and non-creating `GetRecord`.
- **`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`** — new `survivor_letter_delivery` section
  (`narrative` domain), same checksummed envelope discipline as every other orphan-seal section.
- **`src/Main.ExpandedShelterSystems.cs`** — save flush wired into the expanded-shelter persist path.

Deliberately not added: a display-name authority for survivors. The roster carries no such
field, so dweller candidates honestly offer the survivor id as identity; a hardcoded name table
would be invented data violating Rule 7.

**Evidence.** `grep -rlw SurvivorLetterDeliverySystem src/ --include=*.cs | grep -v HostCli`
→ `src/Host/SurvivorLetterDeliveryHostSession.cs`, `src/Main.SurvivorLetterDelivery.cs`.
`NarrativeAndFactionWarIntegrationTests` pass.

---
