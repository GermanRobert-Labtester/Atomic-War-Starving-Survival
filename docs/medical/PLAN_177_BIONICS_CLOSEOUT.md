# Plan 177 — Bionics & Cybernetic Prosthetics: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-174-177-FLAGSHIP-SURVIVOR-WORLD`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Medical/BionicsSystem.cs` (`bionics`) — eligibility, surgery through the limb authority, integration/rehab, maintenance/repair, power state, typed electrical disruption, bounded capability; strict catalog loader |
| Data | `Assets/StreamingAssets/Data/bionics.json` — 5 implants (2 arm grades, 2 leg grades, 1 high-risk neuro-linked) with canonical item FKs |
| Core (additive) | `AmputationSystem`: `RevertBionicToAmputated()` + `chargeInventoryItem` flag on `UpgradeToBionic` (default true preserves existing callers) |
| Host | `src/Host/BionicsSaveStore.cs`, `src/Main.Bionics.cs` (grid-gated charger, malfunction fork, event journal, Plan 176 electrostatic contract, combat-damage command) |
| Save | `SaveSectionRegistry` row (`bionics`, owner `medical`) + `bionics_save.json`; RNG stream `bionics` |
| Tests | Core 14/14 (`Plan177BionicsTests`) · wiring 4/4 (`Plan177BionicsHostWiringTests`) · campaign harness (limb loss → surgery → rehab → maintenance) |

## Contract guarantees (§33 DoD — all satisfied)

- **No second body model (Trap I).** Every limb mutation routes through `AmputationSystem.UpgradeToBionic` / `RevertBionicToAmputated` — socket rules, phantom-pain clearing, and limb truth stay in the amputation section; implant components key to it.
- **Surgery uses the medical path.** Install requires an amputated/prosthetic socket with recovery elapsed; the authored tool + item bill is consumed atomically through the bound inventory; no double-charge.
- **No instant recovery (§6.8).** Integration starts at 50% capability and ramps over the authored rehab days; recovery-gated installation.
- **Functional restoration is bounded (§6.7).** Capability bonus capped at +200 bp; gated by condition floor (25), malfunctions, complications (×0.5), and power; the limb authority's own multipliers remain canonical.
- **Maintenance is real (§6.9).** Authored daily decay; overdue maintenance rolls typed malfunctions (actuator lock / sensor blackout / stun by class); kits restore bounded condition and reset the clock.
- **No free energy (§6.10).** Rechargeable/high-draw implants charge only when the host reports a powered charger (`room_ward_clinical` powered, grid healthy); batteries drain otherwise; expedition endurance modeled.
- **Electrical disruption is typed, never blanket damage (Trap J).** Passive-mechanical immune; battery drain by vulnerability class; high-draw → stun; actuator/sensor lock by slot; condition damage scales — biological health is never touched (structurally guarded: no Needs/Health surface).
- **Combat handoff (§6.12).** `ApplyCombatDamage` damages/destroys implants; destruction reverts the limb through the limb authority — trauma stays canonical.
- **Psychological/narrative layer is character-specific.** Complications (inflammation / chronic pain / neural adaptation failure) are typed events for the medical host; no objective "loss of humanity" is asserted anywhere.
- **Save/load exact; old saves baseline empty** — existing prosthetics and limb state untouched (§10).

## Deferred (flagged, not silent)

`CyberneticsUI` panel (presentation wave); combat-seam call site for `BionicsCombatDamage`; ideology consent modifiers (Plan 175 contract §8.6); watts ledger feed into `PowerGridSystem` (power-grid feed package).
