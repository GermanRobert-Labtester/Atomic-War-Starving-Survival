# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> STATUS: APPROVED BY USER ("deduplicate the naval system and also integrate encounter choices!", 2026-09-29)

# Naval dedup + encounter-choice ledger — 2026-09-29

## Naval (Drowned Coast F4 / DC-P0 premise)
- Single owner: `ExpeditionHostSession._naval`, exposed read-only as `Naval`; `Create()` now layers the authored
  `naval_vessels.json` over the built-in vessels (previously only the dead Main copy loaded it).
- Deleted `src/Main.NavalExpeditions.Integration.cs` (`Main._navalSystem` / `EnsureNavalSystem`, zero callers).
- Dispatch unchanged: authored `vessel_improvised_raft` is identical to the built-in raft.

## Encounter choices
- `EncounterChoiceResolver` is now the persistent at-most-once ledger in front of consequence application in
  `ExpeditionHostSession.EncounterApplyChoice` (key = surfacing `location@dDay#leg` from the pending queue, fallback
  `location@dDay`). Outcomes remain owned by the narrative/travel engines; the ledger records only "applied".
- Closes: bridge guard covered only the newest surfaced encounter and was in-memory, so backlog rows, patrol
  encounters or a reload could reapply consequences.
- `ExpeditionPanel` dismisses a refused duplicate instead of showing "requirements not met".
- Main binds the ledger at expedition setup; `ResetEncounterChoice` added to lifecycle reset (was leaking across campaigns).

## Verification
Host build 0 errors / 14 warnings. ExpeditionEncounterBridge 11/11, EncounterChoiceResolver 14/14, MainTriadDriftGate 7/7,
NarrativeSaveChecksum 6/6, CompositionRootArchitectureGate 4/4. Headless: --expedition-selftest PASS incl. new M10b
(replay refused before and after reload, no second offering), --real-campaign-journey-selftest PASS, --7-day-smoke PASS.
Arch map --check OK. CatalogPathForbiddenGate 1/2 — pre-existing failures in NarrativeAssayLogCatalogs.cs and
BioFermentationPanel.cs (untouched here).

## Not done
- `ExpeditionHostSession.ResolveTravelChoiceWithCombat` has no callers (travel combat escalation unreachable).
- `docs/expansions/expansion_drowned_coast_plan.md` F4 row still describes two naval owners (file is being edited
  concurrently; left for its owner).
