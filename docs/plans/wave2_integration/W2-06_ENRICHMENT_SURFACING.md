# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 6 OF 6

# ENRICHMENT & CONTENT SURFACING INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-06 · part A of B
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-01 (maintenance), W2-02 (bugs), W2-03 (gameplay), W2-04 (environments), W2-05 (locations)
**Plan-unblocking annex:** Annex U at the end — deliberately separated per the Wave 2 rule.

---

## 0. How to read this plan

This plan makes the content that already exists **reachable, legible, and
alive**: narrative files surfaced into play, survivor voices, item lore,
culture (radio/music/echoes), environmental storytelling, journals, factions,
collectibles, and the small human details that make a shelter feel inhabited.
It authors prose and surfaces; it does not add simulation authority. Every new
line has a consumer, and every consumer reads a real owner.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Surface & Revive | wire existing prose into existing surfaces; revive dead data; no new mechanics |
| **B** | Voice & Texture | systematic voicing per surface (survivors, items, factions, places) on existing owners |
| **C** | Living Archive | a coherent, queryable body of in-world text with archival structure and continuity governance |

**Level 2:** ten points, each A/B/C (Appendix A).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Surface & Revive | 1–10 | — | — |
| B Voice & Texture | 1,5 | 2,3,4,6,7,8,9 | 10 |
| C Living Archive | — | 1,3 | 2,4,5,6,7,8,9,10 |

### 0.3 The Wave 2 rule for this plan

> **Content must be consumed.** A line of prose that no surface can show is not
> enrichment; it is debt. This plan authors and surfaces together, and every
> tranche proves reachability with the content-utilization gate.

### 0.4 Voice discipline (non-negotiable)

ASHFALL's tone is restrained, human, and fictional: no real countries, wars,
people, copied text, or slurs; no glorified violence; dignity for the ill, the
disabled, the dead, and the young; scarcity without misery-porn. Every tranche
gets a narrative review before commit.

### 0.5 Vocabulary

| Term | Meaning |
|---|---|
| surface | a place the player can read text |
| consumer | code that fetches and displays content |
| reachability | content is fetchable by a live consumer |
| dead data | authored content with no consumer |
| voice | the consistent register of a speaker/place |
| continuity | facts agree across files |
| archive | the structured body of in-world text |

---

## 1. Executive summary

ASHFALL has an enormous text body: **221 JSON files carrying prose-ish keys**
(`text`/`body`/`description`), including narrative quests (20 files),
encounters (7), radio (10), events (10), echoes (1), dialogue (1), plus
environmental atmosphere texts, deep lore, vinyl records, and item
descriptions. The culture and archive systems already exist:

- `EchoSystem`/`EchoCatalog`, `VinylRecordCatalog`, `VinylMoraleSystem`,
  `CulturalArchiveVaultSystem`, `CultureCreationSystem`.
- `RadioScriptbookCatalog`, `RadioBroadcastCatalog`, `FactionRadioEngine`,
  `PsyOpsSystem`, `RadioRecordingSystem`.
- `ArchiveDeskSystem`, `ArchiveInkCatalogLoader`, `PaperPrintingCatalog`,
  `LibraryStudySystem`.
- Narrative owners: `NarrativeEncounterSystem`, `MicroLocationEncounterLoader`,
  `OralLorePerformanceSystem`, `TradeCaravanCatalog`, `TravelEncounterSystem`,
  `QuestSystem` (via quest files), `JournalSystem`, `MemorialSystem`.
- A content-utilization runtime collector exists
  (`ContentUtilizationRuntimeCollector`) and a `--content-utilization-selftest`.

The gap is **surfacing and reachability**:

1. 221 prose files — how many are actually reachable in play? The wave-1
   programs repeatedly found "presence is not reachability" (the repository's
   own principle). Point 1 audits and revives.
2. Survivor identity/voice: dossier fields exist across multiple catalogs
   (`survivors.json`, `year_of_ash_survivors.json`, `starting_survivors.json`,
   `expansion_survivor_fields.json`, `deep_lore_survivor_fields.json`,
   `antigravity_survivor_fields.json`) — voicing is thin (Point 2).
3. Item lore/provenance: Plan 190/XP-07 is unsigned; the surfaces that could
   show item history are unbuilt (Point 3).
4. Culture content (vinyl/echoes/radio) exists with owners; the player-facing
   texture is uneven (Point 4).
5. Environmental storytelling prose (`environmental_atmosphere_expansion.json`,
   `environmental_texts_expansion_05.json`) may lack consumers (Point 5).
6. Journals/memorial/chronicle record events; the **stories** around them are
   thin (Point 6).
7. Faction texture: communiqués, treaties, radio voices exist; consistency
   across files is unaudited (Point 7; the narrative-continuity skill exists
   for this).
8. Collectibles/archive: matrices exist; display surfaces need voicing (Point 8).
9. Lived-in detail (notes, logs, graffiti, item descriptions) is the largest
   volume opportunity and the easiest to overproduce (Point 9).
10. Content-utilization honesty: the authoring workflow must prevent dead data
    (Point 10).

---

## 2. Verified current state (content evidence)

| Evidence | Count / note |
|---|---|
| JSON files with prose keys | 221 |
| Quest files | 20 |
| Encounter files | 7 |
| Radio files | 10 |
| Event files | 10 |
| Echo files | 1 |
| Dialogue files | 1 |
| Vinyl/culture systems | `VinylRecordCatalog`, `VinylMoraleSystem`, `CultureCreationSystem`, `CulturalArchiveVaultSystem` |
| Archive/print systems | `ArchiveDeskSystem`, `ArchiveInkCatalogLoader`, `PaperPrintingCatalog`, `UndergroundPrintingPressPanel` |
| Radio owners | `RadioScriptbookCatalog`, `RadioBroadcastCatalog`, `FactionRadioEngine`, `PsyOpsSystem`, `NvisCommunicationsSystem` |
| Narrative owners | `NarrativeEncounterSystem`, `MicroLocationEncounterLoader` (28 encounters), `OralLorePerformanceSystem`, `TravelEncounterSystem` |
| Environmental texts | `environmental_atmosphere_expansion.json`, `environmental_texts_expansion_05.json` |
| Survivor catalogs | `survivors.json`, `year_of_ash_survivors.json`, `starting_survivors.json`, `expansion_survivor_fields.json`, `deep_lore_survivor_fields.json`, `antigravity_survivor_fields.json` |
| Journals/records | `JournalSystem`, `MemorialSystem`, `MedicalRecordLog` (day/kind/id only, **never free text**) |
| Utilization tooling | `ContentUtilizationRuntimeCollector`, `--content-utilization-selftest` |
| Localisation | `assets/l10n/` (strings.csv, template.pot, `extract_l10n_inventory.py`) — string freeze is UNBLOCK-03 D22 |

**Critical boundaries already decided elsewhere:**

- `MedicalRecordLog` explicitly **never stores free text** (privacy rule in its
  class comment) — this plan must not add free-text notes there.
- Plan 190 instance-lore stays mapped until signed (XP-07) — Point 3 respects
  that boundary and authors only the surfaces that need no schema.
- String freeze (D22/UNBLOCK-03) governs UI strings; prose in data catalogs is
  not UI strings, but any new **interface** text must follow the freeze once
  declared.

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Auditing and reviving unreachable prose.
- Voicing survivor/item/faction/place surfaces through existing owners.
- Journal/chronicle/memorial storytelling on existing records.
- Culture surfaces (echoes, vinyl, radio, archive) content and texture.
- Environmental storytelling consumption (with W2-04 values).
- Lived-in detail authored into existing catalogs with consumers.
- Continuity governance and the authoring workflow (with content-utilization
  gating).

### 3.2 Non-goals

- New narrative systems (Rule 5).
- Gameplay values/tuning (W2-03).
- Environment mechanics (W2-04); location tiers (W2-05).
- Medical/save free-text storage (forbidden).
- Schema changes (UNBLOCK-01/02; XP-07 signature for provenance).
- UI string freeze workarounds (UNBLOCK-03 governs).

### 3.3 Rules

1. **Consumer first.** A tranche names its surface and consumer before
   authoring; no orphan prose.
2. **Owner truth.** Text may reference a value (day, place, name) only by
   reading it from the owner; templates over fabrication.
3. **Tone review.** Every tranche passes narrative review.
4. **Continuity.** Facts (names, dates, places, factions) agree across files;
   the continuity audit is part of the tranche.
5. **Volume budget.** Bounded tranches; no 50k-word dumps into unreachable
   catalogs.
6. **Dignity rules.** No real-world references; no slurs; care with death,
   illness, and children.
7. **Localisation readiness.** New UI-adjacent strings follow the key
   discipline as declared by UNBLOCK-03.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Narrative reachability audit & revival | B |
| 2 | Survivor voice and identity | B |
| 3 | Item lore and provenance surfaces | B |
| 4 | Culture: echoes, vinyl, radio, archive | B |
| 5 | Environmental storytelling consumption | A |
| 6 | Journal, chronicle, and memorial storytelling | B |
| 7 | Faction texture and continuity | B |
| 8 | Collectibles and archive display | B |
| 9 | Lived-in detail at bounded volume | B |
| 10 | Content-utilization honesty and authoring workflow | B |

### 4.2 Selection sheet

```text
PLAN 6 — ENRICHMENT
Plan Path: [ ] A Surface & Revive  [ ] B Voice & Texture (default)  [ ] C Living Archive

01 narrative reachability . [A] [B] [C]   default B
02 survivor voice ......... [A] [B] [C]   default B
03 item lore/provenance ... [A] [B] [C]   default B
04 culture surfaces ....... [A] [B] [C]   default B
05 environmental stories .. [A] [B] [C]   default A
06 journal/chronicle ...... [A] [B] [C]   default B
07 faction texture ........ [A] [B] [C]   default B
08 collectibles/archive ... [A] [B] [C]   default B
09 lived-in detail ........ [A] [B] [C]   default B
10 utilization honesty ..... [A] [B] [C]   default B
```

---

## 5. Decision Point 1 — Narrative reachability audit & revival (default B)

### 5.1 The design question

Which of the 221 prose files can the player actually encounter? The audit must
produce a reachability map and a revival list.

### 5.2 Path A — Audit and report

- For each prose-bearing catalog: find its consumer(s), the trigger conditions,
  and the observable surface.
- Classify: reachable / conditionally reachable (flag-gated) / unreachable.
- Publish `docs/narrative/REACHABILITY_AUDIT.md` with counts and the
  unreachable list.

### 5.3 Path B — Revive with existing consumers

- For each unreachable set with an obvious existing consumer (a narrative
  encounter owner, radio schedule, archive desk, journal), wire the content to
  that consumer's existing trigger mechanism.
- For content with no possible consumer, either author a minimal consumer hook
  through an existing owner or **archive the file** with a manifest (never
  leave it looking live).
- Prove each revival with the content-utilization selftest (the content appears
  in the utilization report).

### 5.4 Path C — Narrative graph

Path B, plus a read-only narrative graph (which file leads to which, gating
conditions, continuity edges) generated from data, used by authors and by a
continuity gate. This is the foundation for the Living Archive path.

### 5.5 Acceptance

- Reachability map complete for all prose catalogs.
- Every revived set demonstrably reachable.
- Unreachable-and-archived sets listed with reasons.
- No new narrative system.

---

## 6. Decision Point 2 — Survivor voice and identity (default B)

### 6.1 The design question

Survivors have dossier fields, traits, and backgrounds, but the game rarely
**speaks in their voice**. Voicing should make individuals distinct without a
second identity store.

### 6.2 Path A — Voice inventory

- Find every existing survivor text surface (detail panel, journal lines,
  dialogue, radio, memorial) and the identity fields each reads.
- Identify fields with no surface (dead identity data).

### 6.3 Path B — Voiced surfaces on existing owners

- Author voice fragments keyed to identity facts that already exist:
  background/profession (from enrichment/identity owners), traits
  (`development_traits.json`), mood/mental state (`SurvivorMentalHealthRecord`),
  relationships, and keepsakes.
- Surfaces: survivor detail (a line of voice), journal entries at milestones,
  memorial lines, and dialogue choices where a dialogue owner exists.
- Selection is deterministic (hash of survivor id + context) so it is stable
  across sessions, and reads current state so lines fit the situation.
- No fabricated biography: if a field is absent, no line claims it.

### 6.4 Path C — Voice profiles with growth

Path B, plus authored voice evolution (a survivor's register changes with
trauma/recovery) via the same selection mechanism reading state — still read
only, no new store.

### 6.5 Acceptance

- No identity field used by a line without the field existing.
- Deterministic selection tested (same id+context → same line).
- Tone review passed.
- No new survivor system.

---

## 7. Decision Point 3 — Item lore and provenance surfaces (default B)

### 7.1 The design question

Items have descriptions; what they lack is **history**. Plan 190/XP-07
(provenance) is unsigned, so this point authors only the surfaces that need no
schema.

### 7.2 Path A — Surface inventory

- Find item-description surfaces (inspection model, inventory detail) and how
  they read the catalog.
- Identify items with descriptions that never surface.

### 7.3 Path B — Static lore surfaces (no schema)

- Author item-family lore entries (a class of tool, a uniform, a ration) shown
  by the existing inspection surface based on item family/tags — no per-instance
  state, so no schema change.
- Named unique items (if any exist in the catalog) may carry authored history
  text as static data.
- Conditions/first-found discoveries may add a line through existing
  discovery/journal owners.

### 7.4 Path C — Provenance (requires XP-07 signature)

If XP-07 is signed: instance provenance fields and reveal conditions (Plan 190's
mapped scope) plus a HISTORY surface. This plan then authors the text corpus
for the signed mechanic; the mechanic itself stays with the XP-07 package.

### 7.5 Acceptance

- Item lore reachable through the existing inspection model.
- No per-instance state without a signature.
- Family selection deterministic and tested.

---

## 8. Decision Point 4 — Culture surfaces (default B)

### 8.1 The design question

Echoes, vinyl, radio, and the archive exist with owners; the player-facing
texture (what they say, when, and why) is the enrichment target.

### 8.2 Path A — Audēit per surface

- Inventory each culture owner's content and its trigger.
- List content that never plays (dead culture).

### 8.3 Path B — Voiced culture

- Author content sets that use each owner correctly:
  echoes at places/events (`EchoCatalog`), vinyl at downtime
  (`VinylMoraleSystem`), radio programs/broadcasts/psyops
  (`RadioScriptbookCatalog`, `FactionRadioEngine`), archive entries
  (`CulturalArchiveVaultSystem`).
- Ensure a distribution: each campaign phase has something to hear/read.
- Continuity across radio voices and factions (Point 7).

### 8.4 Path C — Culture arcs

Authored cultural storylines (a song that spreads, a broadcast that changes a
faction's stance through existing owners). Content-heavy; the mechanics stay
with existing owners.

### 8.5 Acceptance

- Every culture owner has a phase-appropriate set.
- Distribution audited (no phase without content).
- No lore that contradicts faction/name canon.

---

## 9. Decision Point 5 — Environmental storytelling consumption (default A)

### 9.1 The design question

`environmental_atmosphere_expansion.json` and related texts describe places and
momentos; W2-04 provides the values. This point makes sure the prose is
consumed at the right moments (approach, weather, time of day).

### 9.2 Path A — Consumer check

- Verify which owners read the atmosphere texts and under what conditions.
- List texts with no trigger.

### 9.3 Path B — Trigger alignment

- Align text `type`/`tags`/`weather`/`time_phase` fields with existing owners'
  context (e.g., approach text read by the expedition arrival surface).
- Prove each trigger with the utilization gate.

### 9.4 Path C — Adaptive atmosphere

Text selection reads environment fields (weather, ash, rad band) from W2-04's
read models for richer matching — still read-only.

### 9.5 Acceptance

- Every atmosphere text has a trigger or is archived.
- Matching uses authored fields, not code heuristics.

---

## 10. Decision Point 6 — Journal, chronicle, memorial storytelling (default B)

### 10.1 The design question

The journal records events; the memorial remembers the dead; the chronicle
archives milestones. Enrichment adds the **story around the record** without
free-text storage.

### 10.2 Path A — Surface inventory

- Find journal/memorial/chronicle surfaces and their record sources.
- List records that read as bare facts with no narrative line.

### 10.3 Path B — Templated narrative lines

- Author deterministic templates (per event kind, survivor state, day context)
  rendered by the existing surfaces from existing records — no new record
  fields.
- The medical record's no-free-text rule is respected: narrative lines live in
  the journal/archive surfaces, never in the medical log.
- Memorial lines select from authored templates by identity facts.

### 10.4 Path C — Story threads

Authored multi-entry journal arcs (a survivor's recovery told across days)
assembled from existing event records. Content-heavy; still no new store.

### 10.5 Acceptance

- Each templated line reads real record fields.
- No free text in restricted stores.
- Journal knowledge-key dedup respected (one entry per meaningful event).

---

## 11. Decision Point 7 — Faction texture and continuity (default B)

### 11.1 The design question

Factions speak through communiqués, radio, treaties, and art. Texture should be
distinct; continuity must hold across files.

### 11.2 Path A — Continuity audit

- Run the continuity method across faction-bearing files: names, claims,
  relationships, dates.
- List contradictions.

### 11.3 Path B — Voiced factions + continuity gate

- Author voice sets per faction (radio register, communiqué style, treaty
  language) through existing owners.
- A continuity gate: a checked list of canon facts (faction names, leadership
  titles, treaty partners) referenced by content files must agree; violations
  fail the gate.

### 11.4 Path C — Faction arcs

Authored faction storylines through existing war/radio/treaty owners. Content
via the narrative team.

### 11.5 Acceptance

- No canon contradiction in the checked list.
- Each faction has a distinct register (reviewed).
- No new faction authority.

---

## 12. Decision Point 8 — Collectibles and archive display (default B)

### 12.1 The design question

Collectibles and the archive have matrices and owners; display surfaces should
tell the player what they found and why it matters.

### 12.2 Path A — Display audit

- Verify each collectible category has a display surface and a source note.
- List items with no provenance text.

### 12.3 Path B — Authored display text

- Author per-collectible provenance/curiosity text shown by the existing
  display owner (collectible matrix → panel).
- The archive desk surfaces a short contextual note per archived entry.

### 12.4 Path C — Archive volumes

Authored archive sets (a collection that tells a story when complete) through
the existing vault owner.

### 12.5 Acceptance

- Every collectible displayed with authored text.
- No new collection store.

---

## 13. Decision Point 9 — Lived-in detail at bounded volume (default B)

### 13.1 The design question

Notes, logs, graffiti, item descriptions, and small texts make the shelter
inhabited. This is the highest-volume, highest-repetition risk.

### 13.2 Path A — Inventory and gaps

- List existing detail surfaces and count coverage.
- Identify the emptiest high-traffic surfaces.

### 13.3 Path B — Bounded authored sets

- Author per-surface sets with a strict volume cap per tranche (e.g., 30–60
  entries), each tagged to a context and consumed by an existing surface.
- Repetition control: selection avoids recently shown entries per campaign
  (using the journal's knowledge-key pattern or a small rotation state — read
  only where possible).
- Tone and dignity review per set.

### 13.4 Path C — Archival structure

Organise the total body of in-world text into an archive with indices (by
place, era, faction, theme), generated from data for authors and for a possible
in-game archive browser (only if such a surface is signed).

### 13.5 Acceptance

- Every tranche within the volume cap.
- Repetition control tested.
- Reachability proven.

---

## 14. Decision Point 10 — Utilization honesty and authoring workflow (default B)

### 14.1 The design question

Dead data accumulates unless the workflow prevents it. The content-utilization
selftest exists; the workflow must use it as a gate.

### 14.2 Path A — Baseline report

- Run the utilization selftest; record the current reachability baseline.
- Publish the dead-data list.

### 14.3 Path B — Consumer-first workflow

- A tranche template: surface → consumer → trigger → content → utilization
  proof.
- Rule: no content merged without appearing in the utilization report.
- A shrinking allowlist for known-dead legacy data (freeze now, drain later).
- Continuity + tone checklists attached to the template.

### 14.4 Path C — Living archive governance

Path B, plus the narrative graph (Point 1 C) and an authoring guide generated
from the archive (voice bibles per faction/place, canon index) so future
content is written against current truth.

### 14.5 Acceptance

- New content always reachable.
- Dead-data list shrinking or explicitly archived.
- Authoring template used by every tranche.

---

## 15. Execution phases

### N0 — Content premise freeze (1 day)

- Run the utilization selftest and reachability audit; record baseline.
- Produce `P0_CONTENT_PREMISE.md`.

### N1 — Revival (Point 1)

- Wire unreachable sets to existing consumers; archive the rest with reasons.

### N2 — Voice (Points 2 + 3)

- Survivor voice fragments + item-family lore surfaces.

### N3 — Culture (Point 4)

- Echo/vinyl/radio/archive sets with phase distribution.

### N4 — Story surfaces (Points 5 + 6)

- Atmosphere triggers + journal/memorial templates.

### N5 — Factions and collectibles (Points 7 + 8)

- Voice sets + continuity gate; collectible display text.

### N6 — Lived-in detail (Point 9)

- Bounded sets with repetition control.

### N7 — Workflow (Point 10)

- Templates, allowlist, archive governance.

### 15.1 Ordering constraints

- N0 first (know what is dead before authoring more).
- N1 before all authoring (do not add to a dead pile).
- N5 continuity gate before faction content merges.
- N7 last (it codifies the others).

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | reachability map + utilization proof per revival |
| 2 | deterministic selection test + identity-field existence check |
| 3 | inspection-surface test; no schema change grep |
| 4 | phase distribution table; owner trigger tests |
| 5 | trigger alignment proof |
| 6 | template tests; restricted-store grep clean |
| 7 | continuity gate green; register review notes |
| 8 | display text coverage |
| 9 | volume caps; repetition test |
| 10 | utilization baseline → no dead additions |

Commands:

```bash
godot --headless --path . -- --content-utilization-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Narrative
bash scripts/run_test.sh Ashfall.Core.Tests/Culture   # if present
bash scripts/ci/doc-link-gate.sh
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | volume overproduction | H | M | caps + consumer-first |
| 2 | tone drift | M | H | review per tranche |
| 3 | continuity contradictions | M | H | gate + canon index |
| 4 | prose in restricted stores | L | H | grep + review |
| 5 | UI string freeze violations | M | M | follow UNBLOCK-03 when declared |
| 6 | selection repetition | M | M | rotation/knowledge-key pattern |
| 7 | provenance work without XP-07 | M | H | surface-only until signed |
| 8 | culture content contradicting factions | M | M | continuity gate |
| 9 | authoring races W2-04/05 | M | M | claims + surfaces named |
| 10 | dead data grows again | M | M | utilization gate + allowlist drain |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| N0 | `W2-06-N0-PREMISE` | premise + baseline |
| N1 | `W2-06-N1-REVIVAL` | consumer wirings + archive manifest |
| N2 | `W2-06-N2-VOICE` | survivor/item content + selection + surfaces |
| N3 | `W2-06-N3-CULTURE` | culture catalogs + owners' content |
| N4 | `W2-06-N4-STORY` | atmosphere triggers + templates |
| N5 | `W2-06-N5-FACTIONS-COLLECTIBLES` | voice sets + continuity gate + display text |
| N6 | `W2-06-N6-DETAIL` | bounded detail sets |
| N7 | `W2-06-N7-WORKFLOW` | templates + allowlist + archive governance |

Coordination: W2-03/04/05 name surfaces; this plan writes prose. UNBLOCK-03
governs strings. XP-07 signature gates provenance mechanics.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | revert wirings; keep audit | dead data remains (documented) |
| 2 | remove voice sets | survivors remain silent |
| 3 | remove family lore | items remain bare |
| 4 | remove sets | culture stays thin |
| 5 | keep audit | atmosphere texts unread |
| 6 | remove templates | records stay bare |
| 7 | keep audit | contradictions uncaught |
| 8 | remove display text | collectibles bare |
| 9 | remove sets | shelter feels empty |
| 10 | keep baseline | dead data can grow |

---

## 20. DoD and handoff

**Path A:** baseline, reachability map, revivals wired, dead sets archived, no
new authoring beyond revival needs.

**Path B:** all of A, plus voiced survivors/items/factions, culture sets,
story surfaces, collectible text, bounded detail, and the workflow gate.

**Path C:** all of B, plus the narrative graph and archive governance as signed
packages.

**Handoff:** outcome, files, contract (surfaces + triggers), utilization proof,
tone/continuity notes, limitations, untouched shared paths, Annex U.

### 20.1 First safe step

> N0 only: run the utilization baseline and reachability audit. Do not author
> new content until the dead-data picture exists.

---

# PART B — WORKED ENRICHMENT DESIGNS, SCENARIOS, ANNEX U

---

## B.1 The consumer-first tranche template

```markdown
### Tranche <id>
- Surface: <panel/journal/radio/map/etc.>
- Consumer: <owner/file that fetches>
- Trigger: <condition/event>
- Content: <files + counts>
- Utilization proof: <selftest output>
- Tone review: <name/date>
- Continuity check: <facts referenced, canon index version>
- Volume: <entries added vs cap>
- Revert: <files>
```

A tranche that cannot fill the first three lines is not ready.

---

## B.2 Survivor voice selection sketch

```csharp
// Deterministic, reads existing identity state, never fabricates.
public static string VoiceLineFor(
    SurvivorId id, VoiceContext ctx, IIdentityFacts facts, IReadOnlyList<Line> pool)
{
    var candidates = pool.Where(l => l.Matches(ctx) && l.Requires.IsSubsetOf(facts.Keys));
    if (candidates.Count == 0) return null;             // silence over fabrication
    var hash = StableHash.Of($"{id.Value}:{ctx}:{facts.Version}"); // djb2/x33
    return candidates[(int)(hash % (uint)candidates.Count)].Text;
}
```

Rules: no line whose `Requires` facts are absent; stable selection; silence is
acceptable and preferable to an invented biography.

---

## B.3 Item-family lore surface

```text
Surface: item inspection model (existing)
Consumer: inventory detail panel (existing)
Selection: item.family/tags → family lore set → deterministic entry
No per-instance state (no schema change)
```

Named uniques (if present) carry authored static history in the catalog row.

---

## B.4 Culture phase distribution

| Campaign phase (days) | Echoes | Vinyl | Radio | Archive |
|---|---|---|---|---|
| 0–30 | place echoes | 2–3 records | 2 programs + news | 3 entries |
| 31–90 | event echoes | 3–4 | 3 programs + faction news | 4 entries |
| 91–180 | rare/lore echoes | 2–3 | 3 programs + psyops window | 5 entries |

The table is the audit contract: a phase without content is a gap.

---

## B.5 Journal template example

```json
{
  "id": "journal_template_water_first_restored",
  "event_kind": "needs_restored",
  "requires": ["survivor_id", "day"],
  "lines": [
    "Day {day}: {name} drank like someone remembering what water was for.",
    "Day {day}: The first cup tasted of rust and relief. {name} said nothing.",
    "Day {day}: {name} held the cup with both hands, as if it might be taken back."
  ],
  "tone": "restrained"
}
```

Rendered from real fields (`day`, `name`); the surface is the existing journal,
with knowledge-key dedup.

---

## B.6 Continuity gate shape

```json
// docs/narrative/CANON_FACTS.json (generated/checked)
{
  "factions": ["black_flotilla","merchant","holdfast","…"],
  "titles": { "black_flotilla": ["Admiral","Quartermaster"], "…": [] },
  "treaties": [["black_flotilla","merchant","non_aggression"]],
  "places": ["loc_holdfast","…"]
}
```

The gate scans content files for faction/title/place references and fails on
unknown names or contradictory claims (e.g., a title the canon list lacks).

---

## B.7 Scenario walks

### B.7.1 Scenario — Path A, one week

1. N0 baseline + reachability map (2 days).
2. N1 revival wiring for the top unreachable sets (2 days).
3. N7 allowlist + template (1 day).
Outcome: the dead pile is known, the cheapest revivals are live, and future
content cannot join the pile silently.

### B.7.2 Scenario — Path B, one month

1. N0 (1 day).
2. N1 revivals + archive manifest (3 days).
3. N2 survivor voice + item family lore (5 days).
4. N3 culture sets + distribution (5 days).
5. N4 atmosphere triggers + journal templates (4 days).
6. N5 faction voices + continuity gate + collectible text (5 days).
7. N6 bounded detail sets (3 days).
8. N7 workflow codification (2 days).
Outcome: the shelter speaks, remembers, broadcasts, and archives — all through
existing owners.

### B.7.3 Scenario — Path C, a season

Path B plus the narrative graph and archive governance, enabling future
content to be written against generated canon.

### B.7.4 Scenario — tone review rejects a set

The set returns for revision; nothing merges. Tone review is not advisory.

### B.7.5 Scenario — an unreachable file is beloved content

Revive, don't delete: find or author an existing-owner consumer (e.g., schedule
it as a radio broadcast). Archival is the last resort, never the first.

---

## B.8 Foreman Q&A

**Q1. Another content plan?**
It is the opposite of addition: first it finds what is dead, then it adds only
to living surfaces.

**Q2. Who writes the prose?**
The narrative team via the repo's writing skills; this plan defines surfaces,
counts, and gates.

**Q3. Does provenance arrive early?**
No. Point 3 authors family lore only; instance provenance waits for XP-07.

**Q4. Will the journal contradict records?**
Templates read record fields; no invented facts.

**Q5. What about the medical log?**
It stays free-text-free. Narrative lives in journal/archive surfaces.

**Q6. How is repetition prevented?**
Rotations and knowledge keys; tested.

**Q7. What is the smallest approval?**
N0: the utilization baseline and dead-data list.

**Q8. What is the largest?**
Path C's archive governance, requiring a large but bounded authoring effort.

**Q9. How does this help unblocking?**
It revives the content the expansion plans (12–31) assume exists, and it gives
W2-03/04/05 their prose surfaces.

**Q10. What proves enrichment is done?**
The utilization report shows the dead list shrinking and every new tranche
reachable.

**Q11. Can we skip tone review to go faster?**
No.

**Q12. Does this plan touch UI strings?**
Only as UNBLOCK-03's freeze permits; data prose is separate.

---

## B.9 Annex U — Plan-unblocking (separately)

### U.1 What W2-06 releases

| Blocked item | Release mechanism | Gate |
|---|---|---|
| Expansions 12–31 | The content each assumes becomes reachable/voiced | all phases |
| EN-05 signal continuity | Radio/echo/archive content sets + continuity gate | N3/N5 |
| Plan 42/46 survivor voice/metrics | Voice fragments + templated lines provide the content the certifications need | N2/N4 |
| D11 semantic content | Content grouping supports the semantic-kind routing work | N1 |
| XP-07 (if signed) | Prose corpus for provenance surfaces ready in advance | N2 |
| Plan 190 | Family lore ships without schema; instance half waits for signature | N2 |
| W2-03/04/05 | Named surfaces receive their prose | all |
| E1/Plan 53 census | Utilization baseline feeds content governance | N0/N7 |

### U.2 Signatures needed

```text
[ ] I authorize N0 utilization baseline and reachability audit.
[ ] I authorize N1 revival wirings (existing consumers only).
[ ] I authorize N2 survivor voice + item family lore (no schema).
[ ] I authorize N3 culture content sets.
[ ] I authorize N4 atmosphere triggers + journal templates.
[ ] I authorize N5 faction voices + continuity gate + collectible text.
[ ] I authorize N6 bounded lived-in detail (cap: ____).
[ ] I authorize N7 consumer-first workflow + dead-data allowlist.
[ ] XP-07 provenance: [ ] not yet [ ] signed (then N2-C corpus).
```

### U.3 What W2-06 never touches for unblocking

- Provenance mechanics (XP-07/Plan 190 signature).
- Schemas/save sections (UNBLOCK-01/02).
- Environment/location mechanics (W2-04/05).
- Gameplay values (W2-03).
- UI string freeze (UNBLOCK-03).
- Medical log storage rules.

### U.4 The reachability-release rule

Content presence releases nothing. Only utilization-proven reachability does.

---

## B.10 Appendices

### B.10.1 Selection sheet

```text
ASHFALL WAVE 2 · PLAN 6 (ENRICHMENT) · SELECTION
Date: ______  Foreman: ______  HEAD: ______

PLAN PATH: [ ] A Surface & Revive  [ ] B Voice & Texture (default)  [ ] C Living Archive

01 reachability ....... [A] [B] [C]   default B
02 survivor voice ..... [A] [B] [C]   default B
03 item lore .......... [A] [B] [C]   default B
04 culture ............ [A] [B] [C]   default B
05 environmental ...... [A] [B] [C]   default A
06 journal/chronicle .. [A] [B] [C]   default B
07 factions ........... [A] [B] [C]   default B
08 collectibles ....... [A] [B] [C]   default B
09 detail ............. [A] [B] [C]   default B
10 utilization ........ [A] [B] [C]   default B

Signature: ________________
```

### B.10.2 Tone checklist (every tranche)

```text
[ ] No real countries/wars/people
[ ] No copied text or recognizable paraphrase of existing fiction
[ ] No slurs or dehumanizing language
[ ] Illness/disability treated with dignity
[ ] Death and children handled with restraint
[ ] Scarcity without gratuitous misery
[ ] Restrained, human register
[ ] Facts checked against the canon index
```

### B.10.3 Glossary

| Term | Meaning |
|---|---|
| reachability | content fetchable by a live consumer |
| dead data | authored content with no consumer |
| voice | consistent register of a speaker/place |
| continuity gate | machine check of canon facts |
| utilization proof | selftest evidence of reachability |
| canon index | the checked fact list |
| rotation | selection memory preventing repetition |
| archive manifest | record of intentionally archived content |

### B.10.4 What "done" looks like (default Path B)

| Point | Done when |
|---|---|
| 1 | reachability map complete; revivals proven |
| 2 | voice surfaces live with deterministic selection |
| 3 | family lore reachable; no schema change |
| 4 | phase distribution met |
| 5 | every atmosphere text triggered or archived |
| 6 | journal templates live; restricted stores clean |
| 7 | continuity gate green; faction registers distinct |
| 8 | collectible display text complete |
| 9 | caps met; repetition controlled |
| 10 | workflow used; dead list shrinking |

---

## B.11 Final statement for W2-06

ASHFALL is rich in written content and uneven in its delivery. This plan makes
the writing reach the player: it audits 221 prose files, revives what is
stranded, voices survivors and factions, gives items identity, fills the
journal and the archive, and installs the consumer-first workflow so no future
line is written into silence.

Recommended: **Plan Path B** with Point 5 at Path A. Start with N0 — the
utilization baseline — because enrichment begins with knowing what is already
dead.

---

**End of W2-06.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

*Document control: W2-06 · Wave 2 · HEAD 5be1a30a · companion to W2-01…W2-05.*---

# PART C — ENRICHMENT PLAYBOOK, VOICE GUIDES, AND PER-POINT CHECKLISTS

---

## C.1 The content tranche workflow (canonical)

```text
Tranche lifecycle:
1. SURFACE   — name the panel/journal/radio/map/archive surface.
2. CONSUMER  — name the owner/file that fetches content.
3. TRIGGER   — the authored condition (event, tag, phase, flag).
4. CONTENT   — author within the volume cap.
5. PROOF     — run --content-utilization-selftest; the new content appears.
6. REVIEW    — tone checklist + continuity gate.
7. COMMIT    — one tranche, one diff, one revert.
```

Any tranche that cannot complete steps 1–3 is deferred, not merged.

---

## C.2 Voice guides

### C.2.1 The ASHFALL register (all content)

- Short sentences. Concrete nouns. Few adjectives.
- Emotion implied by action and object, not stated.
- No exclamation-mark enthusiasm; no irony that punches down.
- Weather, ash, rust, hunger, and quiet are the shared vocabulary.
- People are competent and tired; help is practical.

### C.2.2 Survivor voice (point 2)

Voice is built from *facts the game has*: profession, background, traits,
mental state, relationship, keepsake. Examples of the mapping:

| Fact (owner) | Voice tendency | Example fragment |
|---|---|---|
| profession medic | clinical, gentle | "Hold still. This part is the same everywhere." |
| trait stoic | terse | "It'll keep. We won't." |
| state: insomnia | circular, quiet | "The third hour is the one that talks." |
| relationship high | warm, specific | "You still owe me a card game." |
| keepsake present | anchored | "The photograph stays in the tin." |

No line may claim a fact the survivor does not have.

### C.2.3 Faction voice (point 7)

| Faction (from current data) | Register | Avoid |
|---|---|---|
| black_flotilla | naval, transactional, polite threat | pirate cliché |
| merchant | ledger language, indirect | greed caricature |
| holdfast | civic, protective, tired | propaganda |
| others per canon | per the canon index | real-world analogs |

### C.2.4 Item family voice (point 3)

Item lore is written as **object history**, not stat description:
- What it was made for;
- What it survived;
- What it costs now.
No fabricated provenance for instances (that waits for XP-07).

---

## C.3 Per-point checklists

### C.3.1 Point 1 — reachability

```text
[ ] Prose catalogs enumerated (221 baseline; verify at N0)
[ ] Consumer identified per catalog or classified unreachable
[ ] Reachability report published
[ ] Revivals wired to existing consumers
[ ] Utilization proof per revival
[ ] Archive manifest for intentionally unreachable sets
[ ] No new narrative system
```

### C.3.2 Point 2 — survivor voice

```text
[ ] Voice surface named (detail/journal/memorial/dialogue)
[ ] Identity fact source named (existing owner)
[ ] Fragments authored; Requires-fact tags present
[ ] Deterministic selection test (same id+context → same line)
[ ] Absent-fact silence test (no fabricated biography)
[ ] Tone review passed
```

### C.3.3 Point 3 — item lore

```text
[ ] Inspection surface confirmed
[ ] Family/tag selection authored
[ ] No per-instance state (grep for new fields)
[ ] Unique named items (if any) carry static lore
[ ] Reachability test
[ ] XP-07 boundary noted (instance half deferred)
```

### C.3.4 Point 4 — culture

```text
[ ] Phase distribution table met (all four surfaces × three phases)
[ ] Owner trigger tests (echo at place, vinyl at downtime, radio schedule)
[ ] Continuity check against factions/canon
[ ] No dead culture entries
```

### C.3.5 Point 5 — environmental stories

```text
[ ] Every atmosphere text has a trigger or is archived
[ ] Trigger alignment uses authored fields (type/tags/weather/time_phase)
[ ] Matching reads W2-04 values where required
[ ] Utilization proof
```

### C.3.6 Point 6 — journal/chronicle/memorial

```text
[ ] Template set authored per event kind
[ ] Fields rendered from existing records only
[ ] Knowledge-key dedup respected
[ ] Restricted-store grep clean (no free text in the medical log)
[ ] Memorial templates read identity facts
[ ] Tone review
```

### C.3.7 Point 7 — factions

```text
[ ] Canon index versioned (factions/titles/treaties/places)
[ ] Continuity gate flips on a deliberate contradiction (negative test)
[ ] Voice sets per faction authored and reviewed
[ ] No new faction authority
```

### C.3.8 Point 8 — collectibles/archive

```text
[ ] Each collectible category has display text
[ ] Archive desk notes authored
[ ] Display reads the existing owner
[ ] No new collection store
```

### C.3.9 Point 9 — lived-in detail

```text
[ ] Volume cap per tranche stated and respected
[ ] Context tags present on every entry
[ ] Repetition control tested (no immediate repeats)
[ ] High-traffic surfaces prioritized
[ ] Tone review
```

### C.3.10 Point 10 — utilization honesty

```text
[ ] Baseline recorded (dead list)
[ ] Allowlist created for legacy dead data (shrink-only)
[ ] Tranche template in use
[ ] No content merged without utilization proof
[ ] Canon index and tone checklist attached to the template
```

---

## C.4 Worked content examples

### C.4.1 Journal template (memorial)

```json
{
  "id": "journal_template_memorial_watched",
  "event_kind": "survivor_memorialized",
  "requires": ["name", "day", "role"],
  "lines": [
    "Day {day}: We said {name}'s name and nothing else. {role} was enough for today.",
    "Day {day}: The chair by the stove is still {name}'s. Nobody has moved it.",
    "Day {day}: {name} would have hated the fuss. We kept it small."
  ]
}
```

### C.4.2 Radio scriptbook (faction news)

```json
{
  "id": "radio_broadcast_merchant_ledger_31",
  "station": "merchant",
  "schedule": { "min_day": 31, "weight": 1.0 },
  "script": [
    "This is the ledger speaking. Routes north are open; routes east are not.",
    "Water remains the only currency we all understand.",
    "Trade is not charity. Bring something worth carrying."
  ]
}
```

### C.4.3 Environmental approach text

```json
{
  "id": "atm_loc_approach_thermal_plant",
  "location": "geothermal_plant_ruins",
  "type": "location_description",
  "tags": ["approach", "thermal", "steam", "hazard"],
  "weather": "any",
  "time_phase": "any",
  "text": "The road surface has buckled here, pushed up from below by something that still ticks."
}
```

(The example text is the verified existing row; its consumer must be proved at
N0.)

### C.4.4 Item family lore

```json
{
  "id": "lore_family_pressure_lamp",
  "applies_to_tags": ["lamp", "pressure", "kerosene"],
  "lines": [
    "They were made to outlive the men who carried them, and mostly did.",
    "The glass is the first thing to go. Keep a spare; keep two.",
    "Every one you find was somebody's last light before the grid quit."
  ]
}
```

No instance provenance; the selection is family-based.

---

## C.5 Continuity governance detail

### C.5.1 The canon index fields

```json
{
  "version": 1,
  "factions": ["black_flotilla", "merchant", "holdfast"],
  "titles": {
    "black_flotilla": ["Admiral", "Quartermaster"],
    "merchant": ["Factor", "Broker"],
    "holdfast": ["Steward", "Warden"]
  },
  "treaties": [["black_flotilla", "merchant", "non_aggression"]],
  "places": ["loc_holdfast"],
  "era_terms": ["Year of Ash", "the Long Winter"]
}
```

### C.5.2 The gate

```python
# scan content files for capitalized faction/title/place tokens
# unknown token or a treaty pair not in the index → failure with file:line
```

### C.5.3 The rule

The index is *checked-in data*: changing canon is a reviewable diff, not an
accident. W2-05's location names and W2-07-era content (expansions) must
conform.

---

## C.6 Volume governance

| Surface | Tranche cap | Reason |
|---|---|---|
| survivor voice fragments | 40 | selection variety without volume |
| item family lore | 20 families | coverage |
| radio scripts | 12 | schedule capacity |
| echoes | 16 | trigger rarity |
| archive entries | 20 | desk capacity |
| journal templates | 30 event kinds | coverage |
| lived-in detail | 40 | repetition risk |
| environmental texts | 8 per place type | trigger specificity |

Caps are per tranche, not per plan; multiple tranches are allowed with
utilization proof between them.

---

## C.7 The dead-data drain plan (point 10)

1. N0 records the dead list.
2. Allowlist holds legacy dead entries (shrink-only).
3. Each wave-2 tranche revives or archives at least as many entries as it adds.
4. W2-01's maintenance gates keep the utilization report current.
5. When the allowlist is empty, the workflow is self-sustaining.

This is enrichment's contribution to the repository's truth discipline: written
words are not exempt from reachability.

---

## C.8 Scenarios (extended)

### C.8.1 Scenario — content exists but has no owner file

Author a consumer through an existing owner (journal/radio/archive) rather than
creating a new system. If impossible, archive with a manifest and record the
reason.

### C.8.2 Scenario — the tone review and the content plan disagree

The review wins. Escalate the plan, not the text.

### C.8.3 Scenario — a voice line would contradict a survivor's state

The `Requires` gate prevents it. If the line is still desired, the state owner
must first track the fact (a gameplay change → W2-03, not this plan).

### C.8.4 Scenario — radio content overlaps a faction's stance change

Coordinate with the faction/psyops owner; content follows the mechanic, never
the reverse.

### C.8.5 Scenario — an expansion wants a place's prose now

W2-05 supplies the place identity; this plan writes the prose when the surface
exists. Expansion authors request tranches through the same template.

---

## C.9 Foreman one-pager

- **What:** revive dead prose, voice survivors/factions/items, fill journal and
  culture surfaces, govern continuity and volume.
- **Choose:** Plan Path (default B) + ten point paths.
- **First:** N0 utilization baseline.
- **Smallest:** N0 + N7 (baseline + workflow).
- **Largest:** Path C archive governance.
- **Never:** free text in restricted stores, new narrative systems, unproven
  content, tone shortcuts.
- **Unblocking:** Annex U, signed, separate.

---

## C.10 Final checklist

```text
[ ] N0 baseline + reachability map
[ ] Revivals proved by utilization
[ ] Voice/survivor surfaces live
[ ] Item family lore reachable
[ ] Culture phase distribution met
[ ] Atmosphere triggers aligned
[ ] Journal templates live; restricted stores clean
[ ] Continuity gate green
[ ] Collectible text complete
[ ] Detail within caps; repetition controlled
[ ] Workflow in use; allowlist shrinking
[ ] Tone reviews recorded
[ ] Annex U signatures recorded
```

**End of W2-06 enrichment playbook.**

*Document control: W2-06 · Wave 2 · HEAD 5be1a30a · proposal only.*---

# PART D — CONTENT CORPUS PLAN AND TONE BIBLE

This part makes the enrichment work concrete: a corpus plan by surface, the
tone bible, the continuity rules, and the authoring economics.

---

## D.1 Corpus plan by surface

### D.1.1 Surface inventory and target volumes

| Surface | Owner | Current | Target | Tranche cap |
|---|---|---|---|---|
| survivor voice | detail/journal/memorial | thin | 40 fragments | 40 |
| item family lore | inspection | descriptions only | 20 families × 3 lines | 20 families |
| journal templates | journal | some | 30 event kinds × 3 variants | 30 kinds |
| radio scripts | radio owners | 10 files | 12 per phase | 12 |
| echoes | echo catalog | 1 file | 16 | 16 |
| vinyl | vinyl catalog | existing | phase coverage | 8 |
| archive entries | archive desk | existing | 20 | 20 |
| environmental texts | atmosphere catalogs | 2 files | 8 per place type | 8 |
| lived-in detail | existing surfaces | sparse | 40 per surface per tranche | 40 |
| collectible notes | collectible display | minimal | 1 per category | — |

Targets are ceilings with purpose, not quotas to fill.

### D.1.2 Index of existing prose catalogs (verified categories)

| Category | Files |
|---|---|
| quests | 20 |
| encounters | 7 |
| radio | 10 |
| events | 10 |
| echoes | 1 |
| dialogue | 1 |
| environmental | 2 |
| deep lore / vinyl / caravan / travel | several |
| total prose-bearing JSON | 221 |

The corpus plan begins by sorting these into reachable/conditional/unreachable
(Point 1).

---

## D.2 The tone bible (canonical)

### D.2.1 Principles

1. **Restraint.** The prose trusts the reader. It never explains the emotion it
   wants.
2. **Concreteness.** Ash, rust, water, batteries, cloth. Abstract nouns are
   rare.
3. **Dignity.** Every person, including the dying and the dead, keeps their
   dignity.
4. **Practicality.** Hope is a behaviour (a repaired pump), not a speech.
5. **Silence.** Some moments have no line; the game is allowed to say nothing.

### D.2.2 Register examples

| Situation | Good | Avoid |
|---|---|---|
| first clean water | "The cup tasted of rust and relief." | "Finally, we had won!" |
| a death | "We said her name and nothing else." | grimdark spectacle |
| a faction broadcast | "Trade is not charity. Bring something worth carrying." | propaganda parody |
| a hazard | "The third hour is the one that talks." | horror cliché |

### D.2.3 Hard prohibitions

- No real countries, conflicts, leaders, or groups.
- No copied text or close paraphrase from existing fiction.
- No slurs, dehumanization, or group stereotypes.
- No sexual content involving minors; no torture detail; no suicide method
  detail.
- No real-world religion presented as villainy; fictional beliefs only.
- No mockery of disability, illness, or grief.

### D.2.4 Sensitivity rules

| Topic | Rule |
|---|---|
| children | present as people; never as horror props or combatants |
| illness/mental health | clinical and humane; no "madness" shorthand |
| disability/prosthetics | capability framing; dignity |
| death | restrained; name-and-silence over spectacle |
| scarcity | systemic, not misery-porn |
| interrogation/violence | consequences first; no titillation |

---

## D.3 Continuity rules

1. **Names.** Use the canon index (W2-06 B.6); no invented factions/titles.
2. **Places.** Use location ids/names from W2-05's authority; unknown places
   are errors.
3. **Dates/eras.** Use `Year of Ash`, `the Long Winter`, and the season ids from
   W2-04's axis.
4. **Technology.** No anachronisms beyond the established salvage-tech level.
5. **Numbers.** Prose may reference numbers only from owners (day counts etc.)
   via templates.

Violations are caught by the continuity gate and by review; both must pass.

---

## D.4 Authoring economics

| Metric | Guidance |
|---|---|
| words per voice fragment | 8–25 |
| words per journal line | 10–30 |
| words per radio script | 40–120 |
| words per environmental text | 30–80 |
| review time | ≤ 1 hour per 40 fragments with the checklist |
| tranche size | 1 surface × 1 phase × cap |
| reject rate expectation | 10–20% on tone/continuity (budget for it) |

Volume discipline comes from the caps; quality from the reviews; reachability
from the utilization gate.

---

## D.5 Surface-by-surface plans

### D.5.1 Survivor detail (voice)

- One line under the dossier, selected by facts.
- Silence when no facts match.
- No biographies invented.

### D.5.2 Journal (templates)

- Templates per event kind with field placeholders.
- Knowledge-key dedup respected; no repeat entries.
- Memorial templates read identity facts.

### D.5.3 Memorial (lines)

- Name-and-silence register; 2–4 variants per context.
- Never a score; never a resource reference.

### D.5.4 Radio (scripts)

- Per station/faction register (W2-06 B.2.3).
- Schedule fields authored (day windows, weights).
- No real-world news analogies.

### D.5.5 Echoes

- Tied to places/events; short; present-tense impressions.
- Triggered by existing ownership (EchoCatalog).

### D.5.6 Archive

- Each archived entry has a note; entries read as documents, not exposition.
- Archive sets may tell a story across entries (Path C).

### D.5.7 Item families

- Object history, not stat text; three lines per family.
- No instance provenance until XP-07.

### D.5.8 Environmental texts

- Approach/dwell/leave phases; tags for weather/time.
- Match the location's authored character from W2-05's tier/role.

### D.5.9 Lived-in detail

- Notes, labels, chalk marks, torn pages; short.
- High-traffic surfaces first; repetition control mandatory.

---

## D.6 The archive governance model (Path C)

```
Corpus → indices (place, era, faction, theme) → canon index → voice bibles
       → authoring template → utilization gate → continuity gate
```

Deliverables:

- `docs/narrative/CORPUS_INDEX.md` (generated).
- `docs/narrative/CANON_FACTS.json` (checked).
- `docs/narrative/VOICE_BIBLES/<faction>.md` (authored, reviewed).
- The authoring template with tone/continuity/utilization steps.

---

## D.7 Extended scenarios

### D.7.1 Scenario — the corpus exceeds the caps

Caps exist because unreachable volume is debt. The plan prefers 40 excellent
fragments over 400 average ones; overflow is scheduled, not merged.

### D.7.2 Scenario — two factions' voices converge

Review catches it; one faction's register is sharpened. The voice bibles make
the difference explicit.

### D.7.3 Scenario — a template wants a fact the game lacks

The fact must be tracked by an owner first (W2-03/04/05 change), then the
template; never fabricate.

### D.7.4 Scenario — continuity and player experience disagree

Example: canon says a faction is destroyed but content treats it as active? The
canon index wins; content is corrected or the canon fact is updated with a
reviewable diff.

### D.7.5 Scenario — a revival changes game balance

If wiring a narrative event changes weights/outcomes, the change belongs to
W2-03; revivals preserve existing triggers' weights.

---

## D.8 The content one-pager

- **What:** reachability audit, revival, voice, culture, story surfaces,
  continuity, volume discipline.
- **First:** N0 utilization baseline.
- **Smallest:** N0 + N7 (baseline + workflow).
- **Largest:** Path C archive governance.
- **Never:** dead data, tone shortcuts, restricted-store free text, fabricated
  facts.
- **Unblocking:** Annex U, signed, separate.

---

## D.9 Final checklist

```text
[ ] Corpus indexed; reachability known
[ ] Tone bible acknowledged by every author
[ ] Canon index checked in
[ ] Tranche template in use
[ ] Caps respected
[ ] Reviews recorded
[ ] Utilisation proof per tranche
[ ] Continuity gate green
[ ] Dead allowlist shrinking
[ ] Annex U signatures recorded
```

**End of W2-06 content corpus plan.**

*Document control: W2-06 · Wave 2 · HEAD 5be1a30a · proposal only.*