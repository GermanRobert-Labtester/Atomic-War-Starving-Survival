# ASHFALL — "NEW PRESSURES AND PLACES": Family Index & Boundary Sheet
**Status:** Story-director coordination sheet. **Proposal — not a claim, not an authorization.** 2026-09-29.
Covers subjects 13–16 of the user's list. Sibling sets: Year Two (Days 361–720); "The world moves without you" (`expansion_world_moves_without_you_index.md`, subjects 4–7); "New ways to play" (`expansion_new_ways_to_play_index.md`, subjects 8–12); the director's picks P1–P4 (`expansion_shelter_under_pressure_index.md`, formerly labelled 13–16 in-session and relabelled to avoid this clash).

| # | Expansion | Prose plan | Integration plan | Prefix |
|---|---|---|---|---|
| 13 | **Faith and Schism** — belief movements split and fight | `expansion_faith_and_schism_plan.md` | `.ai/plans/faith-and-schism-2026-09-29.md` | `FS-` |
| 14 | **The Underworld** — smuggling, brokers and bounty hunters | `expansion_the_underworld_plan.md` | `.ai/plans/underworld-2026-09-29.md` | `UW-` |
| 15 | **The Deep** — sealed levels and anomalies below the shelter | `expansion_the_deep_plan.md` | `.ai/plans/the-deep-2026-09-29.md` | `TD-` |
| 16 | **The Sky (Orbital Harrow)** — storms that breach roofs, and an orbital countdown | `expansion_the_sky_plan.md` | `.ai/plans/the-sky-2026-09-29.md` | `SK-` |

All four are `STATUS: DRAFT — awaiting user approval`. No per-package derived plans were written.

## 1. What these four have in common
Each subject turned out to be **a machine that is finished and unplugged.** The belief ladder has a top rung nothing can reach. The underworld keeps score with great care and never collects. The shaft goes down to a floor with nothing under it, beside a pile of instrument numbers no code reads. The sky has a roof, an alarm and a gun, and nothing has ever scheduled an impact. These plans **connect existing owners and add one small ledger each**; none adds a second authority, a new save section or a new routed panel.

## 2. What each one found (the central gap)

| Plan | Central gap (verified by reading) |
|---|---|
| FS | The escalation ladder's last stage, **Schism, is unreachable** (the advance clamps at AssaultThreat); its only subscriber writes a journal line; opposing pairs come from a static table; no public method reassigns a believer's belief; three different "schism" mechanics exist and none splits a movement. |
| UW | Marks are never read; the heat attention engine is **fed only by a self-test** and its raid methods (like the enforcer's) have **no gameplay caller**; two heat models and two debt ledgers; syndicates are ids, not people; nothing is ever smuggled. |
| TD | Depth 5 is a floor. Seven architect audits carry `mechanics` no code reads; 13 of 30 abyssal records are switched off; `AmbiguousRequiresReconciliation` is only a fallback label; the vault they describe is **narrative-only canon**. |
| SK | **No campaign path schedules an impact**; the player cannot install roof armour (only a demo can); the armour catalogue's costs and degradation rates are never read; storms lower an abstract resilience number and touch no roof cell; a breach's only consequence is a power surge; **catalogue blast resistance disagrees with the evaluator for three configs**; `Brace` is free; five event fields are unread. |

## 3. Shared things (build once) and cross-links

| Shared thing | Built by | Read by |
|---|---|---|
| **Gate adapter** (door encounters / visitors; designed in `expansion_new_ways_to_play_index.md` §4) | Quiet War | FS (pilgrims, soft), UW (Door leg, soft) |
| **Seeded RNG stream ids** | integrator adds once | all four |
| **Save homes** — no new section: `zealotry` (FS), `black_market` (UW), `shelter_expansion` (TD), world-save nested DTOs (SK) | each plan, additive nested DTOs | — |
| **Presentation-only grades** (news lines, track confidence) | FS, UW, SK | — |

| Link | Nature |
|---|---|
| FS ↔ SK | Impact days and the repeating dead-hand ping are sect flashpoints (data hooks only). |
| FS ↔ TD | Listeners may read a level's Tell as a voice (data hook only). |
| FS ↔ UW | A claim may cover a hunter; no shared state. |
| UW ↔ SK | Impact salvage may be fenced; no shared state. |
| TD ↔ SK | None. A breached roof does not reach the shaft. |
| TD ↔ Deep Works (P4) | **Contested seam**: The Deep uses no construction project; `ExpansionTunnel` stays with the Works. |
| SK ↔ Radio Free Ashfall | The events' unread `radio_hook_text` is the line; RF owns the voice. |

## 4. Owners (no overlaps)

| Concern | Owner | Others |
|---|---|---|
| Belief, fervor, escalation | `ZealotrySystem` | FS nests schism/Question state and adds one public reassignment method |
| Opposing belief pairs | `IdeologicalFrictionSystem` | FS moves pairs to data at most once (DEC-FS-03) |
| Faction trust from belief | `BeliefStanceBridge` | FS reads |
| Syndicate stock, trust, heat, loans | `BlackMarketSystem` | UW nests brokers, runs, hunters; one bridge feeds the attention engine |
| Bounties | `FactionBountySystem` | UW reads marks; resolves only through its calls |
| Contracts | `MercenarySystem` | UW counter-bounty via `PostBounty` |
| Second debt ledger | `LoanSharkEnforcerEngine` | UW read-only combined view |
| Shaft, depth, stability | `ShelterExpansionSystem` | TD reads; nests one ledger field |
| Dose, stress, air | `RadiationSystem`, `SurvivorMentalHealthSystem`, `ShelterAtmosphereSystem` | TD and SK call public methods only |
| Instrument fidelity | `DosimeterCalibrationSystem` | TD reads the error band |
| Archives | `BlackProjectsArchiveSystem`, abyssal projection | TD reads `IsDiscovered`; deferred rows stay deferred unless signed |
| Ceiling cells | `SkyLayerArmorSystem` | SK adds `ApplyLoad` and an optional resistance override |
| Impacts, warnings, brace, salvage | `OrbitalHarrowTelemetrySystem` | SK adds a fizzle and a paid-brace wrapper |
| Intercept | `SkyDefenseBatterySystem` | SK read-only |
| Storm effects | `WeatherGameplayCascadeEngine` + host session | SK carves the roof share out of the resilience hit |
| Shelter resilience | `DisasterResponseSystem` | SK adjusts only the remainder |

## 5. Integrator-owned shared paths touched by more than one plan (serialise)
`src/Main.CampaignOwners.cs` (FS, UW, TD, SK day-owners) · `CampaignStreamIds` (all four) · `CatalogIntegrityValidator.cs` (all four) · `Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs` (**TD additive DTO field and Deep Works project pipeline — two plans**) · `src/Main.OrphanSealWave1.cs`, `src/Host/ShelterOperationsHostSession.cs`, `src/UI/ShelterOperationsPanel.cs` (TD; check against Deep Works panel work) · `src/Main.Zealotry.cs`, `src/Host/ZealotrySaveStore.cs` (FS; checksummed store) · `src/Main.EconomyFamily.cs`, `src/Host/BlackMarketHostSession.cs`, `src/Main.BlackMarket.cs` (UW) · `src/Host/WeatherCascadeHostSession.cs`, `src/Main.World.cs`, `src/Host/WorldHostSession.cs`, `src/Host/WorldSaveStore.cs`, `src/Main.FlagshipInstitutions.cs` (SK) · `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`, `Shelter/SkyLayerArmorSystem.cs` (SK) · `Narrative/AbyssalAnomaliesProjection.cs`, `Narrative/BlackProjectsArchiveSystem.cs` (TD, only if DEC-TD-03/04 are signed).

## 6. Dependencies and recommended order

```
FS-P0 (reassignment API, ladder-cap intent, static pairs)  ─► FS-P1..  ;  gate adapter ─► FS pilgrims (soft)
UW-P0 (contraband callers, heat bridge, leader read)       ─► UW-P1..  ;  gate adapter ─► UW Door leg (soft)
TD-P0 (additive-field safety, canon note, Deep Works seam)  ─► TD-P1..
SK-P0 (parallel instance, carve-out point, catalogue conflict) ─► SK-P1..  ;  SK-P2 (install) ─► SK-P3 (storm load) ─► SK-P5 (countdown)
```

**Recommended sequence:** four **P0 audits in parallel** (read-only) → **FS** first (smallest, self-contained: it makes existing content reachable) → **SK** (largest player-visible effect; wakes a whole finished subsystem; needs DEC-SK-03 signed first) → **UW** (needs the one-heat decision) → **TD** (canon-sensitive; serialise after any Deep Works change to `ShelterExpansionSystem`). Soft hooks ship dark until both ends exist.

## 7. Conflicts found while writing (Rule 6 — logged, not resolved here)
1. **BUNKER-00-ARCHITECT-PRIME is narrative-only canon** — "access mapping forbidden". The Deep frames its levels as the shelter's own, built to the same drawings (DEC-TD-01).
2. **`ExpansionTunnel` slot** is claimed by The Deep Works; The Deep opens without a construction project (DEC-TD-02).
3. **Three "schism" mechanics** (belief ladder — unreachable; morale-contagion — live; governance incident kind) — FS keeps them from firing from one cause (DEC-FS-06).
4. **Ladder cap at AssaultThreat** may be a deliberate guard (DEC-FS-05).
5. **No public belief reassignment** (FS E13).
6. **Two heat models**; the attention engine is unfed (UW E5, DEC-UW-09). **Two debt ledgers** (UW E6, DEC-UW-04). Raid methods with no caller.
7. **No campaign path schedules an orbital impact** (SK E2).
8. **Catalogue vs evaluator** blast resistance disagree for scrap overlay, steel hull, emergency canopy and sandbag (SK E8b, DEC-SK-03).
9. **Storms already have a damage path** (resilience); roof wear is a carve-out (DEC-SK-02).
10. **`Brace` is free; false alarms still yield salvage; five catalogue fields are unread** (SK E4, E5, E9).
11. **Olympus records run to day 5,110**, an 84,000 MJ yield, against a 720-day campaign and 45 MJ events (DEC-SK-04).
12. **Deferred archive rows** (13 abyssal, 3 audits) stay deferred unless signed row by row (DEC-TD-03/04).
13. **Name collisions**: "Deep" ×3 (The Deep, The Deep Works, the Deep Lore plan); "Quiet" ×3 (Quiet Counter, Expansion 41, Quiet War); "anomaly" (surface zones vs deep rules). Ids are distinct.
14. **Numbering clash** between the user's 13–16 and the earlier director's picks — the earlier set is relabelled P1–P4.
15. **`ShelterOperationsPanel` contention** — The Deep extends it; The Sky deliberately extends `SkyDefenseBatteryPanel` instead.

## 8. Combined decision surface (all unsigned)
FS: DEC-FS-01…10 · UW: DEC-UW-01…10 · TD: DEC-TD-01…10 · SK: DEC-SK-01…10. **Blocking-first:** DEC-FS-01 (checksummed save home), DEC-FS-03 (data-driven pairs), DEC-FS-05 (ladder cap), DEC-UW-01 (save home), DEC-UW-09 (one heat), DEC-TD-01 (canon frame), DEC-TD-07 (the Sleeper), DEC-SK-03 (resistance override — a balance call).

## 9. Player-facing arc when all four exist (illustrative)
Mid-autumn: acid snow eats the two sandbag columns over the greenhouse and the shelter has scrap for one repair (SK). The Listeners hear an invitation in the repeating ping on the dead band; the Bench calls it a checksum; the Penitents want the roof left thin (FS). A broker at the Ash Market offers plate at a cut you can see, and the run is stopped at a checkpoint two roads away (UW). Below, a crew calibrating a second dosimeter in the shaft gets a reading that disagrees with the first, and someone decides not to open the curtain until it doesn't (TD). On the day, the slab holds at half strength because the shelter braced it with every bolt of cloth it owned. The Chronicle records who argued for which column.

## 10. What this sheet is not
Not a ledger entry, not a claim, not an approval. The foreman records `INTEGRATION_PLANS.md` entries and `WORKTREE_OWNERSHIP.md` claims; the user sets `STATUS: APPROVED BY USER` on any plan that should ship.
