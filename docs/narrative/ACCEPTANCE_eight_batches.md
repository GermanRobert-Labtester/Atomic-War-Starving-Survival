# Narrative Acceptance Check — Eight Diegetic-Content Batches

**Slice:** The eight creative-writing batches from commit `0118d212` (atmosphere, radio, journals, bureaucratic, letters, medical, engineering, graffiti), post-continuity-fixes.

**Method:** `ashfall-narrative-check` — adapted for ambient/diegetic flavor text (no quest state machines, flags, choices, or runtime effects), focused on reachability-as-world-text, mechanical text/data alignment, tone, and fictional-world compliance.

**Boundary:** `ashfall-narrative-continuity` (already run; see `docs/narrative/CONTINUITY_REPORT.md`) owns broad ID/flag/contradiction graph audits. This check focuses on whether each batch is legible, mechanically aligned, tonally compliant, and fictionally clean.

---

## Slice Map

All eight batches are **ambient/diegetic flavor text** — no entry conditions, exit conditions, flags, choices, or runtime system effects. There are no branches to trace and no state machines to complete. "Reachability" for these slices is "discoverable as world text," not "gated by a quest state."

| Batch | Schema mirrored | Loader match | Reachability |
|---|---|---|---|
| environmental_atmosphere_expansion | environmental_texts_expansion_05 | DATA_ONLY (no Core loader for `environmental_texts`) | ambient world text |
| radio_distress_signals_expansion | radio_distress_signals | DATA_ONLY (mirrors shipped schema) | ambient radio |
| narrative/journals_expansion | journal_entries_expansion_05 | DATA_ONLY (mirrors shipped schema; diverges from runtime `JournalEntry` — see Finding 1) | ambient journal |
| narrative/bureaucratic_documents_expansion | bunker_shift_schedules_and_notices | DATA_ONLY | ambient document |
| narrative/letters_expansion | unsent_letters_batch_2 | DATA_ONLY | ambient letter |
| narrative/medical_documents_expansion | dweller_medical_casebook | **LOADABLE** (superset; `DwellerMedicalCatalog`-compatible) | ambient medical |
| narrative/engineering_logs_expansion | bunker_maintenance_logs_batch_2 | DATA_ONLY | ambient log |
| narrative/graffiti_expansion | bunker_graffiti_postings | **LOADABLE** (exact match; `BunkerGraffitiCatalog`-compatible) | ambient graffiti |

---

## Branch / Reachability Findings

**No blocking reachability findings.** There are no dead ends, impossible conditions, duplicate choices, or effects-with-no-consumer because there are no choices or effects. All content is ambient.

**CONTENT_DECISION (owner):** Wiring the DATA_ONLY batches into their respective runtime systems (JournalSystem, RadioTuner, environmental-text display, document viewer, letter discovery, maintenance-log viewer) is a separate `ashfall-implement` task and is **not required for the content to be valid data authority**. The two LOADABLE batches (medical, graffiti) could be wired to `DwellerMedicalCatalog` and `BunkerGraffitiCatalog` with no schema change (medical is a superset; graffiti is an exact match).

---

## Mechanical Text/Data Alignment Findings

### Finding 1 — Journals schema diverges from runtime `JournalEntry` (CONTENT_DECISION, not blocking)

**Evidence:**
- My `narrative/journals_expansion.json` mirrors `journal_entries_expansion_05.json`: `id`/`title`/`bodyText`/`day`/`type`/`author`/`tags`.
- The Core runtime `Assets/Ashfall.Core/Journal/JournalEntry.cs` expects: `Id`/`Text`/`Timestamp`/`AuthorName`/`AuthorId`/`KnowledgeKey`/`Day`/`Hour`.
- The shipped `journal_entries_batch_1.json` uses the loader-matching schema (`text`/`timestamp`/`author_name`/`author_id`/`knowledge_key`/`day`/`hour`).

**Assessment:** My file matches the *data-authority* schema of the shipped `expansion_05` file (which is itself DATA_ONLY — not loaded into the runtime `JournalSystem`), so it is no more nor less wired than the shipped file it mirrors. If the journals batch is ever wired to the runtime `JournalSystem`, it would need either a schema adapter or a reformat to the `JournalEntry`/`batch_1` schema. This is a known divergence between two parallel journal schemas in the existing project, not a new defect.

**Resolution:** Owner decision. Either (a) leave as DATA_ONLY ambient text matching `expansion_05`, or (b) reformat to the `batch_1`/`JournalEntry` schema if runtime loading is desired. No action required for the content to be valid.

### Finding 2 — All prose is free of mechanical game-ID references (PASS)

**Evidence:** Searched all eight files for `\b(item_|loc_|location_|faction_|survivor_|quest_|npc_|disease_|event_|recipe_|flag_|trait_|echo_|radio_|broadcast_|enc_|narrative_)[a-z_]+`. The only hits were false positives: `location_description` (a `type` vocabulary value in atmosphere) and `radio_broadcasts`/`survivor_community`/`survivor_drift`/`survivor_isolated` (schema-key names and `outcome_type` vocabulary values in radio). **No dangling game-ID references in any prose field.** The prose never names an `item_*`, `loc_*`, or `faction_*` that would need to resolve against the catalog.

### Finding 3 — Medical and graffiti schemas are loader-compatible (PASS)

**Evidence:**
- `narrative/medical_documents_expansion.json` is a strict superset of `DwellerMedicalCaseEntry` (adds `doc_type`); the extra field would be ignored by `DwellerMedicalCatalog`. Loadable as-is.
- `narrative/graffiti_expansion.json` is an exact field-match to `BunkerGraffitiEntry`. Loadable as-is.

---

## Tone & Fictional-World Compliance

### Tone — restrained ASHFALL tone (PASS)

- **No melodramatic apocalypse speeches, no poetic monologues everywhere, no nihilism for its own sake.** The recurring "the form is the form / the margin is the X" voice is restrained and institutional, not declamatory.
- **Emotional range is varied:** aching, ashamed, blunt, bureaucratic, calm, fierce, fretful, grateful, grieving, guilty, haunted, homesick, inconsolable, practical, proud, quiet, self-loathing, simple, tender, urgent, weary, wry (22 distinct tones across letters alone). Not monotonous despair.
- **Materiality throughout:** concrete physical details (boot-leather gaskets, clock-brass shims, bicycle-inner-tube patches, a dirt-cornered requisition, a candle by an empty bed). Not generic "the world ended" language.
- **Humor is restrained and situational:** the weevil/hardtack joke, the wet-gloves stoker rule, "TUESDAY IS THE BREAD," "the boot is the gasket." No Marvel-style quipping.

### Fictional-world compliance (PASS)

- **No real countries/wars/people:** searched for `russia|soviet|china|america|usa|ukraine|germany|nato|warsaw|leningrad|moscow|washington|putin|biden|stalin|hitler|nazi|communist|capitalist|cold war|world war|wwii|ww2|nuclear war` → **zero hits** across all eight files. (The existing `survivor_letters_lost_kin.json` Leningrad reference was deliberately avoided by choosing the cleaner `unsent_letters_batch_2` schema.)
- **No supernatural confirmation:** the two `supernatural_atmosphere` entries (the choir, the radio loop) are explicitly ambiguous — "The acoustics of the old cistern explain it, if you want them to. The faithful... did not want them to. You may choose. The sound does not care which you choose." The numbers-loop radio arc cross-references the dam but never explains *how* it knows. This is the sanctioned ambiguity (the Cult may believe; the world does not confirm).
- **No magic/fantasy:** the single "spell" hit is idiomatic ("the asking is the part that breaks the spell" — a warm-meal entry), not magic. No ghosts/demons/spells/prophecies/chosen-ones.
- **No glorified violence / gore for spectacle:** the single "gore" hit is the medical file's own description stating "No gore for spectacle." The autopsy reports are clinical (marrow cavities, consolidated lungs); the horror is in the margin note, not the viscera. The worst document (the river woman's death) is devastating because the nurse held a hand that didn't hold back — not because of pathology.

---

## Findings Summary

| # | Finding | Severity | Status |
|---|---|---|---|
| 1 | Journals schema diverges from runtime `JournalEntry` | CONTENT_DECISION | Owner decision (leave as DATA_ONLY or reformat for runtime) |
| 2 | All prose free of mechanical game-ID references | PASS | No action |
| 3 | Medical/graffiti schemas are loader-compatible | PASS | No action (wiring is a separate task) |
| 4 | Tone — restrained, varied, material | PASS | No action |
| 5 | No real countries/wars/people | PASS | No action |
| 6 | No supernatural confirmation (ambiguous only) | PASS | No action |
| 7 | No magic/fantasy | PASS | No action |
| 8 | No glorified violence / gore spectacle | PASS | No action |

**No BLOCKING findings.** One CONTENT_DECISION (Finding 1) deferred to owner.

---

## Quality Gate

- ✅ Every blocking finding includes a reproducible path or exact missing edge — **none** (no blocking findings).
- ✅ Tone judgments identify the relevant rule and quoted context (above).
- ✅ No continuity issue is relabeled as a style preference — the continuity contradictions were resolved in `CONTINUITY_REPORT.md` (Findings 1 & 2 fixed: Rima age, Dima→Kolya); the remaining schema divergence (Finding 1 here) is a mechanical-alignment issue, not a continuity issue.

**Conclusion:** The eight batches pass the narrative acceptance check. They are valid data-authority ambient text, tonally and fictionally compliant, free of mechanical ID references, and two are directly loadable by existing Core catalogs. The one CONTENT_DECISION (journals schema) is a pre-existing project-level divergence between two parallel journal schemas, not a new defect, and is deferred to the owner.
