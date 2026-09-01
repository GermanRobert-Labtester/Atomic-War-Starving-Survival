# Plan 43 — Settlements Catalog (12 living settlements)

## Goal (2 lines)
Create `settlements.json` — a new catalog of 12 functioning survivor settlements with
population, economy, allegiance, trade goods, and threats. These are not ruins to scavenge
but living communities the player can trade with, ally with, or come into conflict with,
giving the wasteland a social geography that exists independently of the player.

## Why (P2)
- Verified: no `settlements.json` exists; `locations.json` has 115 entries but they are
  almost all ruins or scavenge sites — the world has no living communities to visit.
- Settlements create the faction-territory-trade triangle: a settlement has an allegiance
  (feeds Plan 44 faction territory), a trade profile (feeds existing 16B caravans), and a
  threat level (feeds Plan 45 patrols + existing 14 raids).
- The player should feel the world is inhabited, not just abandoned. Settlements are the
  proof that other people are surviving too — some better, some worse, some hostile.

## Files to touch
- `Assets/StreamingAssets/Data/settlements.json` (CREATE — 12 settlements)
- Read-only: `Assets/StreamingAssets/Data/factions.json` (19 factions — allegiance ids must
  resolve), `Assets/StreamingAssets/Data/locations.json` (settlements may reference existing
  location ids as their physical site), `Assets/StreamingAssets/Data/items.json` (trade
  goods must resolve), `CatalogIntegrityValidator` (confirm `settlement_` prefix is accepted,
  or use `loc_` if settlements are also locations)
- Check: does an existing system consume settlement data?
  `grep -rn "settlement\|Settlement\|community" Assets/Ashfall.Core/` — if a system exists,
  match its schema; if not, this is a data-first catalog that future plans consume.

## Content grammar (per settlement)
- snake_case `id` with prefix `settlement_` or `loc_` (confirm accepted prefix — do not invent).
- name: grounded, regional, not generic (e.g. "The Sinter Works Camp", "Ferry Point",
  "St. Barrow's Mission", "The Leadworks Commune").
- population: integer (12–200); affects trade volume, defense strength, and food needs.
- allegiance: `faction_*` id (TIER-2 validation) — which faction controls this settlement.
- trade_goods: list of `item_*` ids the settlement exports (what they have surplus of).
- trade_needs: list of `item_*` ids the settlement imports (what they're short of).
- threat_level: 1–5; affects raid probability (feeds existing 14) and patrol frequency.
- attitude: friendly / neutral / wary / hostile — initial stance toward the player.
- description: 2-3 sentences of environmental storytelling (how they survive, what they
  believe, what they fear). Grounded tone, no exposition dumps.
- location_link: optional `loc_*` id if the settlement occupies an existing location site.

## Steps
1. Read `factions.json` to inventory all 19 factions; classify which would plausibly control
   settlements (not all factions are settlement-based — some are nomadic or ideological).
2. Read `locations.json` to find existing entries that could be reclassified as settlements
   (e.g. "rural_gas_station" could be a waystation settlement); do not duplicate — link.
3. Confirm whether any existing system consumes settlement data (step in Files section).
4. Author 12 settlements across 4 types: 3 trade posts (neutral, caravan-friendly), 3
   faction strongholds (allegiance-locked, defensible), 3 refugee camps (vulnerable,
   humanitarian-crisis hooks), 3 religious/ideological communities (belief-driven, friction).
5. Give each settlement: population, allegiance, trade goods/needs, threat level, attitude,
   description, and location link where applicable.
6. Cross-reference: every `faction_*` id resolves; every `item_*` id exists; every `loc_*`
   link resolves (TIER-1/TIER-2).
7. Wire 4 settlements into the caravan system (existing 16B) as caravan destinations —
   caravans travel between settlements trading goods.
8. Wire 3 settlements into Plan 32 expedition destinations as friendly trade stops (the
   player can visit to trade, not just to scavenge).
9. Validate: `--data-integrity-selftest`; confirm settlements appear in a headless boot;
   confirm caravan routes resolve between linked settlements.
10. xUnit: settlement catalog loads, allegiance resolves, trade goods/needs resolve,
    caravan route validation between linked settlements.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the prefix question (step 3): if no system consumes
settlement data yet, this catalog is data-first and future plans wire it. Confirm the
prefix is accepted by `CatalogIntegrityValidator` before authoring.

## Definition of Done
- `settlements.json` exists with 12 settlements, all ids resolving, 4 wired as caravan
  destinations, 3 wired as friendly expedition stops, integrity + tests green.

## Follow-on
- Plan 44 (faction territory) — settlements are the physical anchor for faction control.
- Plan 45 (faction patrols) — patrols originate from and defend settlements.
- Existing 16B (caravans) — settlements are caravan endpoints.
- Existing 18C (crossing refugees) — refugee camps feed the crossing expansion.
- W45 in roadmap 31 (settlements as a content pillar).
