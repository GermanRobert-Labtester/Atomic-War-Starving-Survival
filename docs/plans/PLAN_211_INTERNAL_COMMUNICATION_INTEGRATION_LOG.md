# Plan 211 — Internal Communication Network Integration Log

**Package:** `PFGL-PLAN211-INTERNAL-COMMUNICATION`
**Claim:** `claim-pfgl-plan211-internal-communication-2026-09-25`
**Date:** 2026-09-25
**Terminal state:** **COMPLETE — first vertical slice fully integrated and hardened**

## Outcome

The signed `InternalCommunicationSystem` authority is now constructed by the
Godot host, loaded from the authored `communication_templates.json` catalog,
persisted in its own checksummed `internal_communication` section, advanced by
the canonical campaign day, and projected through the existing `shelter_social`
player route.

The reachable slice is deliberately narrow and truthful:

- a current shelter leader can post the authored water advisory (the host binds the canonical leadership coordinator before exposing the command);
- an ordinary survivor is refused with `author_not_authorized`, while an unknown author receives `author_unknown`;
- public notices can be read and acknowledged by a canonical roster actor;
- ordinary and leadership-only board creation is identity-checked at the host boundary;
- private mail can be sent/read/acknowledged through the host adapter but is
  never included in the public projection;
- expiry is deterministic and idempotent at the authored `expires_day`;
- legacy saves without the section/state fields start with an empty message
  ledger, a safe `NextSequence` that advances past any persisted IDs, and the
  default commons board;
- unsupported future state schemas are rejected rather than silently
  downgraded;
- a successful catalog reload replaces removed template definitions instead of
  leaving stale rows reachable;
- a corrupt existing section is preserved and both standalone and aggregate save
  capture are blocked until repaired, rather than overwriting the only durable
  copy with an empty state.

The external `communications` section remains the antenna/radio authority. No
message was copied into it and no second radio or notification authority was
created. The live corpus census row `E1[24]` is now sealed for this bounded
slice with the same evidence. The host probe also covers precise unknown-author
refusals and fresh-composition leadership binding.

## Current-owner changes

| Layer | Path | Result |
|---|---|---|
| Core | `Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs` | Added current-schema guard, legacy normalization, default-board restoration, authoritative catalog replacement, ID normalization, and null-safe acknowledgement cloning. |
| Registry | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | Added `internal_communication` → `internal_communication_save.json`, under the expanded-shelter lifecycle group. |
| Host | `src/Host/InternalCommunicationSaveStore.cs` | Checksummed/slot-root-aware store using the existing `SaveStoreHub`. |
| Host | `src/Host/InternalCommunicationHostSession.cs` | Strict catalog validation, roster/leadership identity refusals, public/private projections, commands, day tick, and capture/restore adapter. |
| Main | `src/Main.InternalCommunication.cs` | Canonical setup, leadership binding, journal fact for accepted public posts, save/flush/reset (including aggregate abort on blocked restore), actor resolution, and panel binding. |
| Main seam | `src/Main.CampaignServices.cs`, `src/Main.Plans46_49.cs` | Fresh composition plus setup/save composition and one canonical day tick without editing the active PFGL/C1 campaign-owner files. |
| Lifecycle | `src/Main.ExpandedShelterSystems.cs` | In-memory reset/dispose of the communication host. |
| UI | `src/UI/ShelterSocialPanel.cs` | Existing route now shows public notices, read/ack actions, leader-only water advisory action, and truthful blockers; no private inbox is rendered publicly. |
| CLI | `Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`, `src/Main.Application.cs` | Added `--internal-communication-selftest` and alias registration. |
| Tests | `Ashfall.Core.Tests/Communication/Plan211InternalCommunicationHostWiringTests.cs` | Added source/registry/privacy/CLI seam checks. |
| Generated | architecture map, save-store matrix, self-test manifest | Regenerated through their owning generators after the new section/store/probe appeared. |

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/Plan211InternalCommunicationIntegrationTests.cs` | PASS 9/9 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/Plan211InternalCommunicationHostWiringTests.cs` | PASS 5/5 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/SaveSectionRegistryTests.cs` | PASS 5/5 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs` | PASS 11/11 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | PASS 1604/1604 |
| `dotnet build Ashfall.csproj --no-restore` | PASS, 0 warnings / 0 errors |
| `godot --headless --path . -- --internal-communication-selftest` | PASS, all 19 probe checks |
| `godot --headless --path . -- --player-panels-uitest` | PASS, 21/21 lifecycle gates and player-panel checks |
| `godot --headless --path . -- --real-campaign-journey-selftest` | PASS, real composition/day/save/restore journey; existing calendar/resource warnings only |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 426 catalogs, 0 errors, 5 pre-existing radio precedence warnings |
| `python3 scripts/ci/generate-architecture-map.py --check` | PASS, 267 subsystems mapped |
| `python3 scripts/ci/generate-save-store-matrix.py --check` | PASS, 269 store classes |
| `python3 scripts/ci/generate-selftest-manifest.py --check` | PASS, 208 tests cataloged |

## Deliberate residuals

The Core system still owns board capacity/leadership metadata as its existing
low-level model; the host now validates board-author identity and leadership
access, but the first live slice does not invent automatic rationing producers,
intercom UI, private-mail navigation, relationship-triggered mail,
schedule/memorial/security event producers, localization authoring, or a new
notification system. Those are separate follow-on slices, not hidden claims in
this completion. The active PFGL navigation files, C1-owned social composition,
external radio state, and dirty `docs/INDEX.md` were intentionally untouched.
