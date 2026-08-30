# Plan 120 — Crossing Factions Expansion (3 → 8 factions)

## Goal (2 lines)
Expand `crossing_factions.json` from 3 factions to 8. The Crossing expansion's
faction catalog (`CrossingCatalog.cs` confirmed live) defines factions in the
charter settlement — each with alignment, home region, trust, wants, offers,
signature quote, and access rule. 3 factions (The Scale, The Compact, The
Underwrite) is too few for a contested settlement whose entire premise is
multi-faction arbitration.

## Why (P2)
- Verified: `crossing_factions.json` has 3 entries in `actions` array. Each
  has id, display_name, alignment, home_region, is_active, trust, wants,
  offers, signature_quote, access_rule. `CrossingCatalog.cs` loads it;
  `FactionIconCatalog.cs` handles badges.
- The Crossing is the nobody's-charter expansion — a settlement where
  factions arbitrate debt, votes, and territory. 3 factions means no real
  political competition; the arbitration crises (Plan 115) have too few
  parties to create meaningful cross-faction consequences.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/crossing_factions.json` (expand `actions` 3 → 8)
- Read-only: `Assets/Ashfall.Core/CrossingCatalog.cs` (confirm schema and how
  wants/offers/access_rule resolve)

## Content grammar (per faction)
- snake_case `id` with prefix `faction_` (confirmed prefix).
- display_name: evocative faction name.
- alignment: conditional / hostile / neutral / allied.
- home_region: region id (region_crossing or specific).
- is_active: boolean.
- trust: integer starting trust (-50 to +50).
- wants: array of item ids the faction desires in trade.
- offers: array of services/boons the faction provides.
- signature_quote: 1 sentence in the faction's voice.
- access_rule: 1–2 sentences on how to maintain or lose access.

## Steps
1. Read `CrossingCatalog.cs` to confirm the schema and how wants/offers are
   resolved (item ids? service ids?).
2. Read the 3 existing factions (The Scale, The Compact, The Underwrite) to
   confirm the quality bar and avoid duplicating trade profiles.
3. Author 5 new factions:
   - `faction_the_lamplighters`: lighting/utility, controls lamp oil
     supply, wants fuel and glass, offers street lighting and night
     patrol.
   - `faction_the_granary_wardens`: food storage, controls the communal
     granary, wants grain and sacks, offers ration distribution and
     famine insurance.
   - `faction_the_water_committee`: water access, controls the well and
     filtration, wants filters and chlorine, offers clean water rights.
   - `faction_the_quarantine_post`: health/security, controls the gate
     screening, wants medicine and masks, offers disease screening and
     refugee vetting.
   - `faction_the_smugglers_court`: black market, controls the back
     channel, wants contraband and information, offers off-ledger trade
     and discreet transport.
4. Each faction: distinct alignment, trust, wants, offers, signature_quote,
   and access_rule. No two factions share identical trade profiles.
5. Cross-reference: every faction id unique; every wants/offers item id
   follows existing conventions; every home_region is valid.
6. Wire 3 factions into Plan 115 (crossing encounters — new factions appear
   in encounters and crises).
7. Wire 2 factions into Plan 98 (standing record factions — Crossing
   factions overlap with the standing record faction set).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: Crossing faction catalog loads 8 factions, all ids unique, all
   wants/offers arrays non-empty, all alignments valid.

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
- `crossing_factions.json` has 8 factions, all ids resolving, 3 wired to
  crossing encounters, 2 to standing record factions, integrity + tests
  green.

## Follow-on
- Plan 115 (crossing encounters) — new factions appear in encounters/crises.
- Plan 98 (standing record factions) — Crossing factions overlap.
- Plan 126 (crossing items) — factions want new Crossing-specific items.
- Plan 102 (foundry accords) — Crossing factions may sign treaties.
- Plan 89 (epilogues) — faction standing determines Crossing endings.
