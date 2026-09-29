# Dead-code / wiring audit — 2026-09-28

Method: `docs/architecture/port-contract.json` (generated; `scripts/ci/generate-port-contract.py --check` PASS, 307 seams).
Scale: Core 1,222 files / ~341k lines; `src/` 1,370 files / ~262k lines. This is a seam-level audit, not a whole-tree
dead-method proof; the port contract only covers `Register*`/`Bind*`/`Configure*` seams.

## Done this pass
- Removed `SkillProgressionSystem.RegisterDefaultSkills` (zero-op, zero callers in Core, src and tests) and its policy row.
  Contract regenerated: TEST_ONLY 44 -> 43. Verified: SkillProgressionSystemTests 12/12, PortContractGateTests 8/8, HostPortContractTests 6/6.

## Remaining TEST_ONLY seams (no `src/` caller) — need a host owner decision before wiring or deleting
Live-looking systems with no host caller: ChildDevelopment, DiscoveryConsequence, MemoryDecay, PersonalBelongings,
RelationshipDecay, ShelterNoise, ShelterSecurity, Cartography, Rumor hubs, ItemLore, Aging (Plan 176), ModSupport (Plan 165),
SessionDurabilityManager (Plan 39), SurvivorEducation (Plan 154), MaritimeExploration, SurvivorVoice (Plan 42 loader, no consumer),
ResearchUnlockBridge (bridge unbound). Each needs its own claim: wire through the existing owner/save seam, or retire.
Pure integration seams (Bind*/Register* with no caller) in Foundry, Workshop, PharmaLab, CryoVault, Aviation etc. are the same class.

## Known unsurfaced content fields
`AuthoredAudioLog.listening_note` and `UnifiedRadioBroadcast.ListeningNote` are loaded and tested but no player UI shows them.
Radio path needs `RadioIntercept` (carries `Message` only) plus a panel change; audio logs have no player-facing reader in `src/`.

## Wired this pass (second step)
RelationshipDecay had a daily tick but no producer. Resolved conflicts (`SurvivorRelationsSystem.OnConflictResolved`) and caregiving
(`CaregivingSystem.OnCaregivingBondDeepened`) now call `RecordInteraction` on the existing decay owner. `RegisterOrUpdatePair` stays
TEST_ONLY on purpose: `RecordInteraction` registers pairs itself. ChildDevelopment and MemoryDecay already have host owners and
producers (RegisterChild is called from `Main.ChildDevelopment.cs`; MemoryDecay uses a bounded projection), so they were not the gap.

## Wired (third step, 2026-09-29): weather gates
`ExpeditionHostSession.ExtraGateBlock` had no assignment anywhere, so authored weather gates were inert. `Main.Expeditions.cs` now assigns it
from `WeatherRouteGateCatalog`. Only the 3 destination gates match location ids today; the 15 `route_*` gates need a route-to-location map.

## Verified false positives / not islands
A name-only grep flagged 26 Core classes as unreferenced by `src/`, but most are wired under a sibling class (Lyophilization, PowderMetallurgy,
Nvis, Draisine files) or were retired on purpose (`ShelterPrisonerSystem` -> `PrisonerSystem`). Real leftovers: `DebtRouteAccessResolver`
(F19, unimplemented in `LedgerDebtSystem`; doc claims otherwise), `MaritimeExplorationSystem` (750-line duplicate of the live MaritimeDiveSystem
authority; a debloat candidate under the one-authority rule).

## Wired (fourth step, 2026-09-29): weather delay on debt (F19)
`DebtRouteAccessResolver` was dead and F19 was documented but unimplemented. `LedgerDebtSystem` now pauses a signed debt's term (max 3 days per
contract) while a delay-eligible weather gate blocks the route to its creditor. Three gates carry the flag (route_01/02/05). The route_08 gate is
left unflagged because it is a `required_weather` shortcut gate. Remaining known duplicate: `MaritimeExplorationSystem` (debloat candidate).

## Retired (fifth step, 2026-09-29): MaritimeExplorationSystem
Removed as a duplicate dive/site/equipment authority (750 lines + 287-line test). Zone data archived under `docs/archive/retired-data/`.
No other verified duplicate remains in the port-contract inventory; the name-only scan's other hits were false positives (see above).

## Surfaced (sixth step, 2026-09-29): debt pause and listening notes
- F19 pause: `LedgerDebtSystem.OnWeatherDelayApplied` -> journal entry per paused day (`debt_weather_delay_*`) and "weather-paused N/3" on the ledger line.
- Radio: `RadioIntercept.ListeningNote` -> note label under the intercepts grid in `RadioPanel`.
- Audio logs: no reader existed. The 30 authored logs (now carrying their `day`) enter the journal on their recorded day via the `audio_logs` day owner; the listening note follows the untouched body as its own line.
- Verified: host build passes; Plan49DepthPass tests (27) and WeatherGateDebtInteraction tests (9) pass; port-contract gate PASS (306 seams). Not run in-game, so on-screen appearance is unchecked.
