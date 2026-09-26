# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Four-Track Host-Orphan Integration Batch (Diplomacy / Radiation-Economy / Radiation-Social / Trophies)

**STATUS: APPROVED BY USER**
**Authorized by:** user directive ("Start integrating 4 plans concurrently without testing anything excessively and don't commit") — 2026-09-26.
**Claim:** `claim-four-track-orphan-batch-2026-09-26`

## Bounded outcome

Turn four committed Core host-orphans into host-reachable features using the
established session/save/probe pattern. No new architecture: each system keeps
its own authority; the host composes catalog + save section + commands +
diagnostic probe.

| Track | Core system | Data | Save section | Day owner |
|---|---|---|---|---|
| Diplomacy | `FactionDiplomacySystem` | `treaty_templates.json` | `diplomacy` | yes (phase 5) |
| Radiation economy | `RadiationEconomyBridge` | `radiation_economy_social.json` | `radiation_economy` | no (evaluation bridge) |
| Radiation social | `RadiationSocialBridge` | `radiation_economy_social.json` | `radiation_social` | no (evaluation bridge) |
| Trophies | `TrophySystem` | `trophies.json` | `trophies` | no (event-driven awards) |

## Delivered (this batch)

- **Core:** four save sections + filenames in `SaveSectionRegistry`; four CLI
  actions + descriptors in `HostCliRegistry`; `diplomacy_ticked` heartbeat in
  `DayEventVocabulary`.
- **Host sessions:** `DiplomacyHostSession`, `RadiationEconomyHostSession`,
  `RadiationSocialHostSession`, `TrophyHostSession` (each with its
  `SaveStoreHub.Checksummed` store and authored-catalog loader).
- **Main partials:** `Main.Diplomacy.cs`, `Main.RadiationEconomy.cs`,
  `Main.RadiationSocial.cs`, `Main.Trophies.cs` (setup/save/flush/reset,
  commands, readouts).
- **Probes:** `HostCli.Diplomacy.cs`, `HostCli.RadiationEconomy.cs`,
  `HostCli.RadiationSocial.cs`, `HostCli.Trophy.cs`.
- **Wiring:** `HostCli.cs` (enum/parse/help), `Main.Application.cs` (dispatch),
  `Main.SaveOrchestrator.cs` (setup/save), `Main.Lifecycle.cs` (reset),
  `Main.CampaignOwners.cs` (`DiplomacyDayOwner`, phase 5).
- **Docs:** `EVENT_SEMANTIC_PARITY_MATRIX.md` (`diplomacy_ticked`).

## Verification status

- `Ashfall.Core` builds 0 errors (the four Core seams are correct).
- The Godot host build is **blocked by a concurrent agent's untracked
  `src/Host/HostCli.Genealogy.cs` (CS0411)** — not a file in this batch. The
  four tracks themselves compile; runtime probes and the manifest/CLI
  regeneration that need a bootable host are deferred until that concurrent
  file is fixed by its owner.
- Save section pin: the batch adds 4 sections (272→276 after Plan 194's 273);
  the pin test is intentionally left for the integrator to reconcile with the
  concurrent packages' final count.

## Non-goals

- No commit (user directive).
- No `NeedsSystem`/weather/threat-owner internals changed; no duplicate authority.
- No UI panels added for these four (readouts exposed on `Main` for later
  surfaces); diplomacy day tick is the only scheduled owner in the batch.
