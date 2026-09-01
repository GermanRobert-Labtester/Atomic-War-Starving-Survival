# Plan 98 — Standing Record Factions Expansion (1 → 8 factions)

## Goal (2 lines)
Expand `standing_record_factions.json` from 1 verified faction to 8. The
Standing Record faction system (`StandingRecordCatalog.cs` confirmed live)
defines factions in the Standing Record expansion — each has alignment, home
region, trust, wants, offers, signature quote, access rule, and badge asset.
1 faction ("The Overlay") is far too few for a faction-territory expansion.

## Why (P2)
- Verified: `standing_record_factions.json` has 1 entry (id, display_name,
  alignment, home_region, is_active, trust, wants, offers, signature_quote,
  access_rule, badge_asset_id). `StandingRecordCatalog.cs` is confirmed in
  Core. `FactionIconCatalog.cs` handles badges.
- Creates the Standing-Record-faction pillar: the Standing Record expansion
  needs multiple competing factions with territories, trade preferences, and
  access rules. 1 faction means no faction interaction, no territorial
  conflict, no trade dynamics.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/standing_record_factions.json` (expand 1 → 8)
- Read-only: `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs`
  (confirm schema and how wants/offers/access_rule resolve)

## Content grammar (per faction)
- snake_case `id` with prefix `faction_` (confirmed prefix).
- display_name: evocative faction name ("The Overlay", "The Scale",
  "The Compact").
- alignment: conditional / hostile / neutral / allied.
- home_region: region id (all_regions or a specific region).
- is_active: boolean.
- trust: integer starting trust level (-50 to +50).
- wants: array of item ids the faction desires in trade.
- offers: array of services/boons the faction provides.
- signature_quote: 1 sentence in the faction's voice defining its
  philosophy.
- access_rule: 1–2 sentences describing how to maintain (or lose) faction
  access.
- badge_asset_id: asset id for the faction badge (empty string acceptable
  until art is produced).

## Steps
1. Read `StandingRecordCatalog.cs` to confirm the schema and how wants/offers
   are resolved (item ids? service ids?).
2. Read the existing faction ("The Overlay") to confirm the quality bar.
3. Author 7 new factions:
   - `faction_the_scale`: trade-focused, controls water access, wants brass
     and tools, offers water rights and safe passage.
   - `faction_the_compact`: cooperative, manages land records, wants paper
     and ink, offers cadastral maps and dispute resolution.
   - `faction_the_underwrite`: protection-focused, controls fuel depot,
     wants weapons and armor, offers security contracts.
   - `faction_the_cutters`: road maintenance, controls ice road, wants iron
     and coal, offers haulage and road access.
   - `faction_the_fleet`: maritime, controls the dock, wants rope and tar,
     offers barge transport and fishing rights.
   - `faction_the_rebuilders`: agricultural, controls the grain silo, wants
     seeds and tools, offers food supply and crop knowledge.
   - `faction_the_garrison`: military remnant, controls the checkpoint,
     wants ammunition and intelligence, offers patrols and safe passage.
4. Each faction: distinct alignment, home_region, trust, wants, offers,
   signature_quote, and access_rule. No two factions should have identical
   trade profiles.
5. Cross-reference: every faction id unique; every wants/offers item id
   follows existing conventions; every home_region is a valid region.
6. Wire 3 factions into Plan 44 (faction territory map — Standing Record
  factions control territories).
7. Wire 2 factions into Plan 45 (faction patrol encounters — garrison and
  cutters patrol their territories).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: standing record faction catalog loads 8 factions, all ids unique,
   all wants/offers arrays non-empty, all alignments valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is wants/offers resolution (step 1): confirm
whether they are item ids or service ids before authoring.

## Definition of Done
- `standing_record_factions.json` has 8 factions, all ids resolving, 3 wired
  to territory map, 2 wired to patrol encounters, integrity + tests green.

## Follow-on
- Plan 44 (faction territory) — factions control territories.
- Plan 45 (faction patrols) — garrison and cutters patrol.
- Plan 43 (settlements) — factions govern settlements.
- Plan 92 (faction dialogue) — factions have overheard dialogue.
- Plan 89 (muster epilogues) — faction standing determines endings.
