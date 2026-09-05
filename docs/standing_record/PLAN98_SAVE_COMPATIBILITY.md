# Plan 98 — Save Compatibility & Persistence Contract

## 1. Overview & Policy

This document details the save-compatibility verification for Plan 98, demonstrating that expanding `standing_record_factions.json` from 1 to 8 factions introduces zero save-breaking regressions, corruptions, or checksum mismatches across campaign envelopes (`campaign.json`).

---

## 2. Invariant Verification

| Invariant | Requirement | Verification Evidence |
|---|---|---|
| **Old-Save Compatibility** | Existing saves created prior to Plan 98 must load cleanly. Missing factions must initialize with catalog defaults (`trust: 0`) without crash or error. | Verified by `StandingRecordFactionExpansionTests.Persistence_OldSaveInitialization_DefaultsGracefully`. |
| **No Overwrite on Load** | Loading a save must never overwrite mutated campaign standing with static JSON values. | Verified by `StandingRecordFactionExpansionTests.Persistence_MutableTrustRoundTrip_PreservesDynamicStanding`. |
| **Checksum Invariant** | Campaign envelope checksums must remain deterministic across save/load cycles. | Verified by `SaveStoreCoverageGateTests` and full suite run. |
| **Envelope Architecture** | Standalone section files are packed into `campaign.json` via `SaveStoreHub` atomic write. | Full compliance with single-envelope migration (Initiative #42). |

---

## 3. Save / Load Lifecycle Walkthrough

1. **New Game Initialization:**
   - Catalog loads 8 records from `standing_record_factions.json`.
   - Campaign state initializes a dictionary of `{ factionId -> 0 }` for all 8 factions.
2. **Old-Save Load:**
   - Saved campaign state contains only legacy data (or only `faction_the_overlay`).
   - The runtime identifies missing faction keys from the authoritative catalog.
   - Missing keys are assigned starting trust `0` in memory. No existing progress is overwritten.
3. **Save Capture:**
   - `CapturePersisted` serializes current live standing dictionary into memory bytes.
   - `CampaignEnvelopeBuilder` computes the checksum and writes atomically to `campaign.json`.
4. **Re-Load & Verification:**
   - Deserialization restores exact integer trust scores.
   - Fresh catalog reloads leave runtime dictionary untouched.
