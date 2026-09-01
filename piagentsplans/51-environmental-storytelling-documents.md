# Plan 51 — Environmental Storytelling Document Pack (30 documents)

## Goal (2 lines)
Add 30 environmental-storytelling documents (evacuation lists, ration records, blood
trails, barricade placement notes, hastily sealed doors, family photographs, military
maps, emergency broadcasts, vandalized propaganda, handwritten warnings, maintenance
records, shelter rejection lists, ration theft ledgers, contaminated water notices,
half-finished repairs) as readable items + journal unlocks. These tell the world's
history through physical evidence, not exposition.

## Why (P2)
- Verified: `narrative/` has 272 JSON files (documents, journals, letters, broadcasts);
  `archive_inks.json` has only 3 inks; `items.json` has 159 items but few readable
  documents. Environmental storytelling is the core of ASHFALL's tone (AGENTS.md: "show,
  don't preach") but the document catalog is thin.
- Creates the environmental-storytelling pillar: each document is found in a specific
  location type (evacuation list in a government building, ration record in a shelter,
  military map in a depot) and unlocks a journal entry that adds world history without
  exposition dumps.
- Pure DATA work — documents are items with journal-unlock effects.

## Files to touch
- `Assets/StreamingAssets/Data/items.json` (add 30 `item_document_*` entries)
- `Assets/StreamingAssets/Data/narrative/` (add 30 document JSON files — one per document,
  following the existing narrative file structure; confirm the structure first)
- Read-only: `Assets/StreamingAssets/Data/scavenging_tables.json` (Plan 46 — documents
  appear as rare entries in location-specific tables), `Assets/StreamingAssets/Data/locations.json`
  (documents are found in specific location types), `CatalogIntegrityValidator`

## Content grammar (per document)
- snake_case `id` with prefix `item_document_` (reuse `item_` prefix — documents are items).
- document_type: evacuation_list / ration_record / blood_trail_note / barricade_placement /
  sealed_door_warning / family_photograph / military_map / emergency_broadcast_transcript /
  vandalized_propaganda / handwritten_warning / maintenance_record / shelter_rejection_list /
  ration_theft_ledger / contaminated_water_notice / half_finished_repair_note / casualty_list /
  triage_record / last_letter / supply_requisition / evacuation_route_map / quarantine_notice /
  civil_defense_poster / field_report / personal_journal_fragment / death_certificate /
  supply_inventory / radio_log / child_drawing / confession / will.
- location_type: which scavenging table (Plan 46) this document appears in.
- journal_unlock: `journal_*` id — reading the document unlocks a journal entry that adds
  world history (1-3 paragraphs, grounded tone, no exposition dumps).
- faction_link: optional `faction_*` id — some documents reveal faction history (a
  requisition order from a faction, a rejection list from a faction shelter).
- quest_hook: optional — some documents hint at a quest (a map showing a hidden bunker,
  a letter mentioning a missing person, a log referencing a faction operation).
- tone: cold, exhausted, human, restrained (per AGENTS.md). No preaching, no exposition.

## Steps
1. Read `narrative/` directory structure: confirm the JSON file format for documents
   (pick 3-5 existing document files and match their schema exactly).
2. Read `items.json` to confirm the item schema; documents are items with a journal-unlock
   effect.
3. Read `scavenging_tables.json` (Plan 46) to confirm where documents slot in as rare
   entries.
4. Author 30 documents across 30 types (one per type for variety). Each document: a
   physical object (found in a specific location type) + a journal entry (the text the
   player reads). Examples:
   - Evacuation list (government building): a half-finished list of names, some crossed
     out, some added in different handwriting. Journal: who was evacuated, who was left.
   - Ration record (shelter): a ledger of food distribution, with thefts marked. Journal:
     how the shelter's food politics worked.
   - Blood trail note (hospital): a nurse's last note, left where she collapsed. Journal:
     what happened in the hospital's final days.
   - Military map (depot): a deployment map with positions marked. Journal: the military
     operation that preceded the exchange.
   - Shelter rejection list (shelter): names of people turned away from a functioning
     shelter. Journal: the moral calculus of survival.
   - Contaminated water notice (water source): a sign posted over a well. Journal: why
     the water is poisoned and what it did to the people who drank it.
   - Child's drawing (apartment): a crayon drawing of a family, found in a child's room.
     Journal: the family who lived here, told through the drawing.
   - Last letter (personal letter): a letter written but never sent. Journal: the person
     who wrote it and what they couldn't say.
5. Write each journal entry in ASHFALL's tone (cold, exhausted, human, restrained). Use
   skill `ashfall-write` for voice consistency. 1-3 paragraphs per entry. No exposition
   dumps — show through physical detail.
6. Add 30 `item_document_*` entries to `items.json` (weight, stack, value — documents are
   low-weight, low-trade-value, high-information-value).
7. Add 30 narrative JSON files to `narrative/` (one per document, matching the existing
   narrative file structure).
8. Wire 15 documents into `scavenging_tables.json` (Plan 46) as rare entries in the
   matching location-type tables.
9. Wire 5 documents to quest hooks (a map → hidden bunker location; a letter → missing
   person quest; a log → faction operation quest — feeds existing questlines).
10. Wire 3 documents to faction history (a requisition order, a rejection list, a field
    report — feeds existing 25A faction life).
11. Validate: `--data-integrity-selftest`; confirm a document found via scavenging unlocks
    its journal entry in a headless boot; confirm the journal entry text displays.
12. xUnit: document catalog loads, all `item_*` ids resolve, all `journal_*` unlocks
    resolve, journal entries display, faction links resolve, save round-trip preserves
    read state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring. The one trap is the narrative file structure
(step 1 prevents this — match existing files exactly).

## Definition of Done
- 30 `item_document_*` entries in `items.json`, 30 narrative JSON files in `narrative/`,
  15 wired into scavenging tables, 5 wired to quest hooks, 3 wired to faction history,
  journal unlocks display, save round-trip preserves read state, integrity + tests green.

## Follow-on
- Plan 46 (scavenging tables) — documents are rare loot entries.
- Existing 17A/B/C (environmental storytelling + documents + codex) — this plan is the
  primary content delivery for that pillar.
- Existing 25A (faction life) — faction-history documents deepen faction lore.
- Plan 47 (collectibles) — documents overlap with collectibles (photographs, maps).
- Existing 15B (verdict dossiers) — some documents serve as verdict evidence.
