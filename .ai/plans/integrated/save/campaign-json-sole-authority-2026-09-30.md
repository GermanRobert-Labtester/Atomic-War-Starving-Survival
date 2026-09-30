# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (approved by user: "continue working on the still opened", 2026-09-30)

## Outcome
`campaign.json` is the only save authority. A Continue → SaveAll → Continue cycle no longer fails closed.

## Defects found (reproduced red in `--real-campaign-journey-selftest`)
1. 83 `Main*.cs` Save methods also wrote a per-file store (`XSaveStore.TrySave`) mid-day, leaving section files the envelope did not contain; the next Continue refused them as out-of-generation.
2. Continue rebuilt only sessions on a hand-written list; nine registered sections (counter_intelligence, informant_network, recon_telemetry, food_preservation, prewar_archives, survivor_letter_delivery, plastic_pyrolysis, cargo_airdrop, cryo_vault, aquaponics) were built lazily, so the next SaveAll dropped them.
3. `SaveEncounterChoice` / `SaveTravelEncounters` returned early when clean, dropping the section from a fresh generation.

## Changes
- Removed direct `TrySave` from 83 Main save methods (capture-only via `CaptureSection`).
- `RestoreAllSubsystemsFromDisk` now calls the nine missing setups.
- Removed the two dirty guards in `Main.Expeditions.cs`.
- Gates: `SaveSectionRegistry_SetupMethods_AreReachableFromContinue`, `RegistrySaveMethods_DoNotSkipCleanSystems` (MainTriadDriftGateTests).
- Journey selftest: mid-day save probe and save→load→save→load probe.

## Non-goals / held
Host-session `Save()` overrides (129 `TrySave`) have no production caller (only `MedicalWardSaveSelfTest`); left untouched, reported as dead code.
