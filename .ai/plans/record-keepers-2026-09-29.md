# Feature / Task Plan: The Record Keepers — custody, loss, copying and evidence over the existing journal, memorial and archive owners

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_record_keepers_plan.md`. Family index: `docs/expansions/expansion_shelter_under_pressure_index.md`.
> Not a claim. The journal, memorial and Shelter Archive (a **projection**) keep their meaning; this plan adds **one custody overlay** beside the archive desk and one optional overlay parameter on the projector. It introduces **no author, no slant, no second archive**.

## 1. Goal & Outcome
- **Goal:** Give records **custody and risk**: each tracked record has a medium, place, condition, copies and state (Intact/Faded/Damaged/Lost); an archive-desk **Keeper** (the existing archivist) slows decay and can **copy**; losses become dated **Gaps** in the archive timeline; oral accounts use existing Memory Decay clarity; a **Reading** and a **Dispute** are player verbs; a well-kept record adds weight to a tribunal.
- **Outcome (observable):** on a fixed seed a Paper record in a damp room moves Intact → Faded → Damaged → Lost per the ink's `fade_rate_per_day` and the room's modifier; an empty Keeper chair accelerates decay; a copy on a durable medium in a second place resets condition and survives a loss in the first; a Lost record appears as a tagged Gap in the projected archive timeline; a memorial page is always recopiable from the memorial owner; `JusticeSystem.AddEvidence` receives a clue weighted from condition; no Keeper and no custody rows → every record behaves exactly as today; save/load round-trips.
- **Non-Goals:** no new archive/journal/memorial/knowledge store; no author or slant; no change to retention semantics or protected obligations; no new memory-decay maths; no player-authored text; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `JournalEntry { Id, Text, Timestamp, AuthorName, AuthorId, KnowledgeKey, Day, Hour }`; text in the author's voice. | `Journal/JournalEntry.cs` L12–24 | LIVE |
| E2 | Shelter Archive is a projection: `ProjectCanonicalSources(journal, memorial)`; entry types Event/Decision/Memorial/Milestone/Discovery/Achievement; significance; tags; participants. | `Shelter/ShelterArchiveSystem.cs` L11–40, L136; `src/Main.ShelterArchiveProjection.Integration.cs` L21 | LIVE |
| E3 | `RecordEvent` has one gameplay caller: a ten-day milestone heartbeat. | `src/Main.ShelterArchive.cs` L93; CLI self-tests | LIVE |
| E4 | Archive desk: transcription jobs, inks (`legibility_score`, `archival_longevity_days`, `fade_rate_per_day`), archivist must not be on a duty shift; completion writes journal discovery + `unlockedEvidenceIds`. | `ArchiveDeskSystem.cs` L11–52, L99–135, L151–183 | LIVE |
| E5 | `unlockedEvidenceIds` has no reader besides `IsEvidenceUnlocked`; ink fade fields are only printed. | grep `IsEvidenceUnlocked`, `fadeRatePerDay`; `src/UI/ArchiveDeskPanel.cs` L266–287 | GAP |
| E6 | Retention policies (8) with `IsProtectedObligation` (wills, memorials, grief never pruned). | `Records/RetentionPolicy.cs` L25–50; `retention_policies.json` | LIVE |
| E7 | Memorial system: entries, rites, epitaphs; `AttachMemorialSystem` bridge. | `Memorial/MemorialSystem.cs`; `src/Main.ShelterArchive.cs` L42 | LIVE |
| E8 | Time capsules: 4, event triggers, legacy messages. | `Communication/TimeCapsuleSystem.cs` | LIVE |
| E9 | `MemoryDecaySystem`: five domains; clarity Forgotten→Vivid; `ProjectCanonicalSources(facts, day)`. | `Cognition/MemoryDecaySystem.cs`; `src/Main.MemoryDecay.cs` L186 | LIVE |
| E10 | `JusticeSystem.AddEvidence(incidentId, evidenceId, description, weight, day)`; `min_evidence_confidence` per law. | `Narrative/JusticeSystem.cs` L51, L203–216 | LIVE |
| E11 | Fire incidents: `ShelterFireHazardSystem.Ignite(incidentId, zoneId, day, zones)`. | `Shelter/ShelterFireHazardSystem.cs` L106 | LIVE |
| E12 | Sump/flood and room condition are readable signals. | `SumpFloodingSystem`; `ShelterExpansionSystem.GetRoom` | LIVE (VERIFY read APIs) |
| E13 | Epilogue input shape: `EpilogueChronicleBuilder.Build(EpilogueChronicleInput)`. | `Endgame/EpilogueChronicleBuilder.cs` L20 | LIVE |
| E14 | Save owner for archive desk: `archive_desk`; desk DTO `ArchiveDeskState`. | `Save/SaveSectionRegistry.cs` L142; `ArchiveDeskSystem.cs` L11 | LIVE |
| E15 | Panels: `ArchiveDeskPanel`, `ChroniclePanel`, `IronCenotaphMemorialPanel`. | `src/UI/` | LIVE |
| E16 | Whether a survivor's death exposes an event to clear/degrade an oral record (survivor fate ledger). | `Survivors/SurvivorFateSystem.cs` | **VERIFY (P0)** |
| E17 | Where `ProjectCanonicalSources` can accept an optional overlay without a signature break. | `ShelterArchiveSystem.cs` L136; callers `Main.ShelterArchiveProjection.Integration.cs` L21, CLI | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Journal entries & authors | `JournalSystem` | read-only |
| Memorial wall & rites | `MemorialSystem` | read-only; recopy source |
| Archive timeline | `ShelterArchiveSystem` (projection) | optional overlay parameter (Gaps as tagged Events) |
| Transcription | `ArchiveDeskSystem` | copy job (additive job field), additive `custody[]` in `ArchiveDeskState` — **DEC-RK-01** |
| Retention | `RetentionPolicyCatalog` | none (memorial protection confirmed, not changed) |
| Individual memory | `MemoryDecaySystem` | read-only clarity query |
| Evidence | `JusticeSystem` | `AddEvidence` calls only |
| Custody rows, Keeper chair, Gaps | — | `RecordCustodyLedger` (pure Core), nested in the archive-desk save |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Records/RecordCustody.cs` (new, pure), `Records/RecordMedia.cs` (new), `Records/RecordRisks.cs` (new), `ArchiveDeskSystem.cs` (additive nested DTO field + copy-job field only), `Shelter/ShelterArchiveSystem.cs` (additive optional overlay param on `ProjectCanonicalSources` only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `record_media.json`, `record_places.json`, `record_risks.json`, `keeper_report_lines.json`, `record_dispute_options.json`
**Host:** `src/Main.ShelterArchiveProjection.Integration.cs` (`INT`, pass overlay), `src/Host/ArchiveDeskHostSession.cs` (`INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `ArchiveDeskPanel` (Keeper report, custody, copy) and `ChroniclePanel` (Gaps) — **DEC-RK-07**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Records/RecordCustodyTests.cs`, `RecordRisksTests.cs`, `RecordCopyTests.cs`, `Ashfall.Core.Tests/Save/RecordCustodySaveTests.cs`; extend archive-desk and shelter-archive tests

## 5. Packages

### RK-P0 — Premise audit (Auditor; read-only)
- Close E12, E16, E17: room/sump read APIs; death event for oral records; overlay parameter feasibility; enumerate every reader of `ArchiveDeskState`, `ProjectCanonicalSources`; confirm memorials are unprunable; confirm fade units (per day vs per fraction); foreman signs DEC-RK-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; overlay signature decided.

### RK-P1 — Custody model (Core, pure + nested DTO)
- `RecordCustody { recordKey, kind, medium, placeId, condition, copyOf?, copies[], state, lostDay?, cause? }`; nested additive `custody[]` in `ArchiveDeskState`; default empty; record keys are journal `KnowledgeKey` or memorial survivor id.
- **Accept:** round-trip; old saves load; no rows → identical archive projection; ledger holds no record body text.

### RK-P2 — Keeper chair & decay (Core)
- Keeper = `archivistId` not on duty roster (existing rule); condition falls by ink fade (Paper), place modifier and neglect multiplier (empty chair); durable media by data.
- **Accept:** table-driven; chair empty → faster decay per data; deterministic; conservation — condition never rises without a copy.

### RK-P3 — Risks (Core, seeded)
- Damp (sump/flood + room condition), Fire (`ShelterFireHazardSystem`), Breach/Theft (defense/siege/QW signals — soft), Neglect, Time; each moves state one step per roll; fork keyed `(day, recordKey)`.
- **Accept:** same seed → same steps; no risk fires without its signal; memorial pages never reach permanent Lost (recopy from `MemorialSystem`).

### RK-P4 — Gaps in the projection (Core)
- `ProjectCanonicalSources(journal, memorial, custody?)`: each Lost record emits a tagged Event (`gap`, `cause:*`, `medium:*`); enum unchanged.
- **Accept:** overlay null → byte-identical projection; each Lost record → one Gap; deterministic ordering.

### RK-P5 — Copying (Core + host)
- Copy job on the existing transcription queue (`copyOf`), consuming ink/slate/working drum, half a Keeper-day; resets condition on the new medium; second place = redundancy.
- **Accept:** ink consumed atomically (existing CR3-04 pattern); a record survives loss of one copy; busy archivist blocks as today.

### RK-P6 — Oral accounts (Core, read-only queries)
- Oral custody rows read the bearer's Memory Decay clarity and death event (E16); Forgotten or dead → Lost unless transcribed.
- **Accept:** uses only the public clarity query; no new decay maths; bearer death is a deterministic loss.

### RK-P7 — Reading, Dispute, Seal, Evidence (Core + host)
- Reading: bounded morale/cohesion/legitimacy change via public APIs; Dispute: Amend/Append/Strike (Strike → Gap + discovery risk); Seal: copy into a time capsule; Evidence: `JusticeSystem.AddEvidence` weight from condition (capped by data).
- **Accept:** no direct mutation of relationships, legitimacy or justice; Gap adds no evidence; a discovered Strike subtracts a bounded amount.

### RK-P8 — Presentation & chronicle
- Extend `ArchiveDeskPanel` and `ChroniclePanel`; optional `recordQuality` input to `EpilogueChronicleInput` for presentation-only slide variants (**DEC-RK-09**).
- **Accept:** presenter tests; panels hold no authority; epilogue unchanged when the input is absent.

### RK-P9 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no custody rows, no Keeper → journal, memorial, archive projection identical on a saved corpus.
3. Conservation: no record gains condition except by a copy; a copy never overwrites its source.
4. Determinism: identical loss steps and Gaps on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-decay with a copy in progress and a Gap present.
6. Memorial pages are never permanently lost.
7. No writes to journal, memorial, retention, justice, legitimacy or relationships except via public APIs.

## 7. Cross-plan boundaries
- **The Reconstruction Tree:** capability vs accounts; a shared fragment source is read-only.
- **The Ration Wars:** the Book is a record; no slant.
- **The Long Siege / The Quiet War:** breach, theft or a leak are signals, consumed via their public seams.
- **The Deep Works:** a dry held drift is a Place (soft).
- **Shelter Governance:** the Assembly may order a Strike; the Keeper may refuse.
- **Year Two:** Chronicle/Verdict may read `recordQuality`; a second-shelter copy survives the first.
- **Radio Free Ashfall:** a broadcast is a copy medium; no new store.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-RK-01 | Custody overlay nests in `ArchiveDeskState` (archive-desk save); no new section. | architecture | Yes; confirm in P0 |
| DEC-RK-02 | Provenance is the existing journal `AuthorId`; no author or slant is added. | rule | Yes |
| DEC-RK-03 | Gaps are tagged Event entries; `ArchiveEntryType` unchanged. | architecture | Yes |
| DEC-RK-04 | Four media (Paper, Slate, Drum, Oral). | scope | Yes |
| DEC-RK-05 | Empty Keeper chair accelerates decay and blocks copying. | design | Yes |
| DEC-RK-06 | Memorials are never permanently lost; they recopy from the wall. | rule | Yes |
| DEC-RK-07 | No new routed panel; extend archive-desk and chronicle panels. | UI | Yes |
| DEC-RK-08 | Capability (Tree) vs accounts (this plan). | boundary | Yes |
| DEC-RK-09 | Chronicle `recordQuality` is presentation-only. | design | Yes |
| DEC-RK-10 | Evidence clue weight capped by data; Gaps add none. | rule | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Custody`, `Keeper`, `Gap`, `Provenance`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on archive/journal/memorial paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing archive-desk, shelter-archive, memorial, retention and justice tests (list from P0 selector)
- [ ] `--shelter-archive-selftest` and archive-desk selftests (VERIFY args in `HostCli.ShelterArchive.cs`)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a Gap cannot be represented without changing `ArchiveEntryType` or the projector's contract; memorial protection cannot be guaranteed; the overlay would need a second record store; retention would need to change; any path overlaps a live claim.
