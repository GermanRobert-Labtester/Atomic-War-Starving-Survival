# Moral Faction Reaction Matrix

## 1. Faction Voice Architecture

ASHFALL's factions are not archetypal "good vs evil" tropes. Each possesses specific institutional motivations, survival doctrines, and lenses through which player actions are judged:

- **Peacekeepers**:
  - *Core Lens*: Order, treaty compliance, civilian protection, predictability, procedural consequence.
  - *Attitude*: Pragmatic law enforcers under resource strain. They reward cooperation with logistics contracts and perimeter defenses, but post bounties and revoke parley rights when ruthlessness endangers the colony.
- **Raiders**:
  - *Core Lens*: Leverage, martial reputation, fear, usefulness, territorial risk tolerance.
  - *Attitude*: Tribal opportunists and combat veterans. They respect violence and ruthlessness as indicators of power, but draw hard border boundaries when a monster becomes too dangerous to negotiate with.
- **Knowledge Keepers**:
  - *Core Lens*: Historical records, empirical evidence, taxonomy, institutional memory, archive security.
  - *Attitude*: Scholarly preservationists. They classify player actions impartially across historical registries (e.g. Master Roll vs Red Cartulary), offering restricted archive access to community builders and barring vaults against destructive actors.

---

## 2. Event Coverage Matrix

| Event ID | Faction | Speaker | Location | Lines | Narrative Focus |
|---|---|---|---|:---:|---|
| `moral_event_bounty_issued` | Peacekeeper | Peacekeeper Sergeant Veill | Outpost notice board | 5 | Bounty notice posted; description circulated to patrols. |
| `moral_event_bounty_issued` | Peacekeeper | Anonymous Peacekeeper recruit | Camp perimeter | 3 | Junior sentry nervousness; moral comparison to raiders. |
| `moral_event_bounty_issued` | Raider | Raider lookout, unnamed | Raider territory border | 4 | Professional respect for high bounty; mercenary calculation. |
| `moral_event_bounty_issued` | Knowledge Keeper | Knowledge Keeper archivist | Ruined library entrance | 4 | Classification between dangerous and lost; guarding records. |
| `moral_event_contract_taken` | Peacekeeper | Peacekeeper Captain Osa | Headquarters, back room | 5 | Standing contract offer; safehouse clearance. |
| `moral_event_contract_taken` | Raider | Raider captain, scarred woman | Outpost, edge of territory | 4 | Caution around Peacekeeper proxy; shifting border dynamics. |
| `moral_event_contract_taken` | Knowledge Keeper | Knowledge Keeper elder | Archive meeting hall | 3 | Recognition of constructive action; restricted shelf access. |
| `moral_event_contract_raised` | Peacekeeper | Peacekeeper Captain Osa | Headquarters, private quarters | 6 | Full logistics partnership; leadership recognition warning. |
| `moral_event_contract_raised` | Raider | Raider lieutenant | Raider border checkpoint | 4 | Standing offer of defection; mutual respect between commanders. |
| `moral_event_contract_raised` | Knowledge Keeper | Knowledge Keeper council leader | Archive council chamber | 4 | Seat at the long table as living witness to history. |
| `moral_event_patrol_defense` | Peacekeeper | Peacekeeper patrol leader | Outside player's shelter | 4 | Perimeter rotation deployment; defending east approach. |
| `moral_event_patrol_defense` | Raider | Raider ridge scout | Overlooking shelter perimeter | 4 | Scout reconnaissance; recognition that hitting gates means hitting blue coats. |
| `moral_event_patrol_defense` | Knowledge Keeper | Knowledge Keeper surveyor | Outer perimeter marker | 4 | Formal logging of permanent armed node on valley map. |
| `moral_event_legend_positive` | Peacekeeper | Peacekeeper Captain Osa | Headquarters | 4 | Honest warning about becoming a myth and the danger of falling. |
| `moral_event_legend_positive` | Raider | Raider caravan broker | Crossroads trade camp | 4 | Scavengers refusing attack contracts due to sheer reputation. |
| `moral_event_legend_positive` | Knowledge Keeper | Chief Chronicler Kaelen | Sanctum of the Master Roll | 4 | Dedicated archive volume; indexing depositions to separate fact from myth. |
| `moral_event_legend_positive` | Civilian | Camp elder & young survivor | Central fire & perimeter | 8 | Moral lesson to children; newborn named in honor of player. |
| `moral_event_legend_negative` | Peacekeeper | Peacekeeper Captain Osa | Headquarters, via radio | 4 | Bounty upgraded to dead-or-alive; immediate fire on sight. |
| `moral_event_legend_negative` | Raider | Raider warlord | Raider stronghold, throne room | 5 | Territorial partition at the river; fifty guns ready if crossed. |
| `moral_event_legend_negative` | Knowledge Keeper | Chief Chronicler Kaelen | Sanctum of the Master Roll | 5 | Inscription into the Red Cartulary; iron doors barred against player scouts. |
| `moral_event_legend_negative` | Civilian | Camp mother | Camp interior, children's area | 4 | Whispered instructions to children to hide when player passes. |

---

## 3. Plan 95 Journal Voice Integration

Threshold reactions feed player self-reflection into `JournalSystem` via `src/Main.MoralChoice.cs`:
1. **Contract Raised** (`moral_event_contract_raised`):
   > *"The Peacekeepers raised my contract to full support — logistics, medical, passage through every corridor they hold. Captain Osa says I've become something the wasteland rarely produces. The Knowledge Keepers offered me a seat at their long table."*
2. **Bounty Issued** (`moral_event_bounty_issued`):
   > *"The Peacekeepers have issued a bounty with my face on it. The sergeant put it up himself. Every patrol knows my description now."*
3. **Additional Sealed Reactions**: All other threshold events similarly include concise, tone-anchored first-person reflections that register upon event firing without numeric morality disclosure.
