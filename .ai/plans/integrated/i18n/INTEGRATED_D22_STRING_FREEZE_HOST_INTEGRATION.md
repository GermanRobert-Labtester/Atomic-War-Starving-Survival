# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# D22 Localization String Freeze Policy Host Integration

**STATUS: APPROVED BY USER** (user authorized D22, 2026-09-26)
**Package:** `D22-STRING-FREEZE-GATE`
**Gate authority:** `Ashfall.Core.Localization.StringFreezePolicy` (previously 0 `src/` references)

## Bounded outcome

Give the signed D22 policy an operational host surface. It enforces that player-facing frozen text
classes (`ui.`, `settings.`, `tutorial./onboarding.`, `warning./alert.`, `item.`, `voice.`, codex,
briefing) use structured localization keys, and refuses unkeyed raw strings unless allowlisted as
tracked debt. Read-only gate; no save, no new localization store.

## Delivered

- New `src/Host/HostCli.StringFreeze.cs` — 8-check probe `StringFreezeSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `StringFreezeSelfTest` + descriptor
  `--string-freeze-selftest` (alias `--localization-freeze-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --string-freeze-selftest` → 8/8 (valid ui key; wrong prefix
refused; raw frozen string refused; allowlisted debt allowed; valid item key; empty refused; frozen
class check; non-dot-delimited codex refused). Parity 4/4.

## Non-goals

No change to `StringFreezePolicy`; no new translation catalog; no extraction of production strings
(the policy is exercised as its own authority via the CLI surface).
