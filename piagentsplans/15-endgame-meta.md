# Plan 15 — Endgame & Meta: Epilogue Depth, Chronicle & New Game+

> **Theme:** The ending is architecturally impressive (32-permutation epilogue matrix, Verdict
> tribunal, Muster) but the *road to it* is thin and the meta-loop (what carries between runs)
> is unexplored. This plan deepens the endgame and plants a replayability hook.
>
> **Key evidence:** `EpilogueMatrixRuntime.cs` + `EpilogueChronicleBuilder.cs` +
> `HoldfastEndings.cs` (32 permutations) live; `muster_epilogues.json`, `muster_witnesses.json`,
> `epilogue_chronicle.json` present; `MachineLogSystem`/`ReckoningSystem`/`EvidenceLedger` (Verdict) live.

---

## Task 15A — Epilogue chronicle & 32-permutation depth

**Goal:** Enrich the epilogue matrix so each of the 32 permutations reads as a distinct,
earned chronicle that *names* the player's specific choices.

**Files:** `epilogue_chronicle.json`, `muster_epilogues.json`, `muster_witnesses.json`,
read-only `EpilogueMatrixRuntime.cs`, `EpilogueChronicleBuilder.cs`, `VerdictEndingEvaluator.cs`.

**Substeps:**
1. Read `EpilogueMatrixRuntime` + `EpilogueChronicleBuilder` to learn how the 32 permutations are computed and which world flags feed them.
2. Map which flags currently have rich chronicle text vs. generic stubs.
3. Author chronicle text variants so each major flag axis (survival, mercy, justice, faction outcome, generational fate) has 2+ distinct phrasings.
4. Author witness statements (`muster_witnesses.json`) that reference *specific* player deeds (a spared warlord, a defaulted debt, a fulfilled final wish).
5. Wire fulfilled Plan 65 final wishes and vigil-managed deaths (09C) into the chronicle as named remembrances.
6. Ensure the faction-war outcome (06C) feeds a dedicated epilogue axis.
7. Add a "chronicle codex" post-ending screen that replays the full generated history (uses JournalCodex).
8. Validate ids/flags; data-integrity selftest; narrative-continuity check.
9. xUnit: permutation → correct chronicle assembly; witness selection by flags.
10. Determinism: same flags → identical chronicle across runs.

**Next steps:** shareable chronicle export (text dump); a "generations later" coda scene.

---

## Task 15B — Verdict evidence & reckoning dossier depth

**Goal:** Deepen the Verdict (machine tribunal) so the reckoning is a forensic review of the
*player's actual record*, with evidence dossiers the player can find, contest, or suppress.

**Files:** `verdict_*.json` catalogs, new evidence dossier data, read-only `MachineLogSystem.cs`,
`ReckoningSystem.cs`, `EvidenceLedger.cs`, `VerdictCensusBroadcast.cs`.

**Substeps:**
1. Read `MachineLogSystem`, `ReckoningSystem`, `EvidenceLedger` to map what evidence is recorded and how the tribunal scores it.
2. Author 12 forensic evidence dossiers (Cold War records, machine logs, survivor testimony) as discoverable data — registry §28 explicitly recommends this.
3. Author evidence that can *incriminate* and evidence that can *exculpate* the player/bunker.
4. Add a pre-verdict "gather/suppress evidence" window: dossiers found on expeditions (11A) can be submitted or destroyed (moral branching hook).
5. Wire dossier discovery into expedition loot tables at relevant sites.
6. Author 6 reckoning cross-examination beats where the tribunal cites specific player choices.
7. Ensure evidence state is captured/restored (save round-trip).
8. Validate ids; data-integrity selftest.
9. xUnit: evidence accrual, suppression flag, tribunal scoring change by submitted evidence.
10. Narrative-continuity + dialog-graph lint for the new dossier flags.

**Next steps:** a defense-advocate survivor role; contested-verdict branching ending.

---

## Task 15C — New Game+ & legacy inheritance (meta-loop)

**Goal:** Add a restrained, grounded New Game+ that carries *narrative* legacy (a journal, a
heirloom, a remembered name) rather than power — boosting replayability without breaking the
survival fantasy.

**Files:** new `LegacyInheritanceSystem` (Core, tiny), `PhantomMemoryEngine` (read — heirlooms
already exist), save/campaign envelope, new-game flow in `Main.GameFlow.cs`.

**Substeps:**
1. Read `PhantomMemoryEngine` (heirloom/memento triggers) — NG+ should *reuse* it, not duplicate.
2. Design the inheritance set: 1 heirloom item, 1 journal excerpt, 1 named memorial that persists into the next run. Nothing that confers combat/economic power.
3. Implement `LegacyInheritanceSystem` (Core, engine-agnostic, deterministic): capture a small legacy record at ending, seed it into a new campaign.
4. Store legacy in the campaign envelope / a sidecar slot file via `SaveStoreHub` (never hand-rolled envelope).
5. Wire the inherited heirloom into `PhantomMemoryEngine` triggers so it surfaces memories.
6. Author 4 legacy heirloom items + their memory-trigger texts (a parent's dosimeter, a child's drawing, a dog-eared manual).
7. Surface the inherited journal excerpt in the new run's JournalCodex prologue.
8. Confirm no determinism violation (legacy seed is fixed data, not random).
9. xUnit: legacy capture at ending → inheritance into fresh campaign → heirloom trigger fires; save round-trip.
10. Full verification: `dotnet test`, build 0/0, save-migration selftests (new envelope section must not break old saves).

**Next steps:** generational NG+ (play as the raised child from 12A); a legacy "family bunker"
that remembers multiple past runs. **Cross-tool QA applies** (new system touching saves).
