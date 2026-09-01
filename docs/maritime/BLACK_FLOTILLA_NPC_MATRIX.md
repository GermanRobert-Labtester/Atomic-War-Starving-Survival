# Black Flotilla NPC Matrix (Plan 23 / Task 23A)

Authority: `Assets/StreamingAssets/Data/characters.json` (named-NPC registry;
id, display_name, profession, bio, faction, region, first_day, location_id,
wants[], offers[], will_not[], signature_quote). Baseline 54 → **60** after Plan 23.
No duplicate of any existing character; all six are referenced by the stance/radio
surfaces below. Gated by `Npcs_SixFlotillaRoles_*` tests.

| id | Name | Role (fleet) | Region / anchor | Standing-sensitive | Wants (hooks) | Offers | Will not | Disagreement / contradiction |
|---|---|---|---|---|---|---|---|
| `npc_odile_vanter` | Odile Vanter | Anchormother — political coordinator | coastal_shelf / `loc_black_flotilla_outpost` | yes (thresholds voice) | claim registry honesty; `item_marine_sealant_kit` | mooring rights, claim arbitration, convoy terms | Will not order a blockade herself; hates that she may have to | Believes the Escort Fleet's blockades starve the very keels they protect |
| `npc_cass_polder` | Quartermaster Cass Polder | Salvage chief / exchange | coastal_shelf / outpost | trade surface | dry cloth, needles, fuel | marine sealant, tools, claim tags | Refuses bell-bronze trade (burial custom) | Feuds with Halloran over inspection fees; keeps a private ledger of unpaid debts |
| `npc_jorin_hael` | Jorin Hael | Dive-Chief (Deep) | coastal_shelf / `loc_shelf_deep_berth` | yes (cooperation ≥ 55) | `item_rebreather_canister`, honest line reports | deep-service ribbon, depth tables, line lessons | Will not send a diver down without a marked line | Sent Lotte down; came up alone; never says so |
| `npc_uma_tarran` | Uma Tarran | Codekeeper / signals authority | coastal_shelf / outpost mast | code teaching at intel trust | paper_scrap, `item_fleet_log_cylinder` | band calendar, burial records | Will not broadcast open code | Reads every casualty list twice; keeps the struck-off list anyway |
| `npc_halloran_vesk` | Board Officer Halloran Vesk | Escort convoy officer | the_shelf / `loc_maritime_icebreaker_dock` approach | hostile below 0; blockade-minded | fuel, weapon serials, route intel | convoy inspection, safe-passage tickets | Will not escort unribboned deep work | Wants a hard blockade; Odile refuses to sign one |
| `npc_lotte_verrill` | Lotte Verrill | Struck-off diver (dissident) | the_drown | distrusts Deep Fleet regardless of player standing | `item_sealed_dive_lamp`, silence | uncharted wreck notes, contaminated-water caution | Will not wear the ribbon, dive claimed water | Her survival is the Deep Fleet's open wound |

## Continuity rules

- All six start alive; no Plan 23 content kills or exiles them (deserter state is
  pre-existing for Verrill). Death/defection outcomes are owned by future quest
  authority; this plan adds none.
- Radio never speaks of an NPC as active after a canonical death — none exists yet;
  broadcasts reference **roles and marks**, not live whereabouts, to stay safe.
- Location availability: outpost NPCs appear at `loc_black_flotilla_outpost`;
  Vesk at the dock approach; Verrill roams `the_drown` (no fixed location).
- Save: NPCs are static registry data; alive/dead state remains with the narrative
  flag authority (no new persistence in Plan 23).

## Radio broadcast matrix (8 authored categories)

| # | Category | Pool | Line seed |
|---|---|---|---|
| 1 | weather/current advisory | intercept_chatter | "glass falling, long swell from the south-east" — shelf marks by dark |
| 2 | salvage-claim warning | intercept_chatter | mail-steamer claim, step marks |
| 3 | convoy/escort challenge | parley_resolution | heave-to, show the bar-and-dot |
| 4 | missing-diver notice | intercept_chatter | black-ribbon code, line and air honest |
| 5 | coded deep-dive status report | trade_reaction? no — intercept | grey/black door, third ribbon |
| 6 | trade bulletin | trade_reaction | needs dry cloth/needles, pays for carry |
| 7 | standing-sensitive warning/invitation | parley_resolution | "mooring berths you tonight" (trusted) |
| 8 | aftermath (wreck shift / convoy war) | raid_warning + aftermath line | marks suspended until convoy answers |

Delivery goes through the real `FactionRadioEngine` pools (intercept_chatter,
parley_resolution, raid_warning, trade_reaction); selection is day+frequency+seed
deterministic (pinned by `Plan23FlotillaFactionDepthTests`). Two lines change meaning
once the player holds a code-ribbon context (`item_escort_challenge_ribbon`,
`item_deep_service_ribbon`) — the corpus keeps one consistent code vocabulary.
