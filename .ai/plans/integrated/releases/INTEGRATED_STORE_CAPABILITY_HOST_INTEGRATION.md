# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Store Capability Manifest Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `RELEASE-CRAFT-STORE-TRUTH`
**Gate authority:** `Ashfall.Core.Launch.StoreCapabilityManifest` (Plan 57 / Plan 48 release craft; previously 0 `src/` references)

## Bounded outcome

Expose the store-capability truth gate through the host CLI so marketing/store claims cannot run
ahead of shipped systems and passing verification gates. Read-only; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.StoreCapability.cs` — 8-check probe `StoreCapabilitySelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `StoreCapabilitySelfTest` + descriptor
  `--store-capability-selftest` (alias `--store-manifest-selftest`).
- `src/Host/HostCli.cs` — host enum + parse.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --store-capability-selftest` → 8/8 (unbacked, unavailable-system, failing-gate,
null-claim refusals; valid pass; audit tally; seam firing; empty manifest refusal).
Build 0 errors; parity gate 4/4; manifest + CLI catalog regenerated.

## Non-goals

No change to `StoreCapabilityManifest` logic, no data catalog authoring, no new save section.
