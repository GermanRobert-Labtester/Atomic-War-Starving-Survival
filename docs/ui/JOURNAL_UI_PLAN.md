# ASHFALL — Journal UI Plan

> Target file: `Assets/_Game/UI/JournalBookUI.cs` + `Assets/_Game/Events/JournalSystem.cs`
> Companion doc: `docs/ui/UI_VISUAL_TEXT_SPEC.md` (visual/art direction in text)
> Status: DESIGN PLAN — implementation is a separate task.

## 1. Goal (2 lines)

Turn the existing diegetic journal (playthrough log + tutorial pages) into the
game's single narrative surface: a hand-annotated bunker ledger the player
opens with [J], containing the play log, survivor notes, and the item/field
text data this repo now ships. No modal popups; everything pings the strip.

## 2. What exists today (grounding)

- `JournalSystem` (Events/JournalSystem.cs): 64-entry ring, knowledge-deduped
  entries (`KnowledgeBase`), trait-voiced text via `JournalVoice.ComposeFullText`,
  unread/ping flags, `CaptureState`/`RestoreState`, `OnEntryAdded`,
  `OnNotificationPing`.
- `JournalBookUI` (UI/JournalBookUI.cs): thin MonoBehaviour mirroring the
  system; `IsOpen/HasUnread/NotificationPing`, `StatusLine` + `DetailSummary`
  text readouts consumed by the text HUD, `Push/SetEntries/ApplyUiState`.
- Item/location/survivor/event text now lives in StreamingAssets JSON and is
  imported into `ItemDefinition.description`, `LocationDefinitionSO.description`,
  `SurvivorArchetypeSO.bio`, `GameEvent.bodyText` (see `docs/ui/UI_VISUAL_TEXT_SPEC.md`
  §0 for the content audit).

## 3. Design principles

1. **One surface.** Journal = play log + item codex + survivor files + event
   history. Four tabs inside one book; no separate menus.
2. **Diegetic, not modal.** Opening the book never pauses the simulation
   behind a blocking dialog; it is an overlay with the game world still ticking
   (consistent with existing "no modal popups" rule). Save/load of `HudIsOpen`
   already exists.
3. **Text is the asset.** The book renders the JSON-authored text verbatim.
   It never paraphrases. If the writer wrote a paragraph, the book shows a
   paragraph.
4. **Read-state honesty.** Unread badge counts entries the player has not
   opened the book to see; opening the book clears it. Per-entry "new" dots
   are derived from save state, not randomness.

## 4. Layout (top to bottom)

```
+--------------------------------------------------------------+
|  [J] BUNKER LEDGER   Day 74  ·  hand-annotated  ·  [X] close  |  header strip
+--------------------------------------------------------------+
|  TAB:  LOG | ITEMS | PEOPLE | PLACES | EVENTS                 |  tab row
+--------------------------------------------------------------+
|                                                              |
|   ...scrollable content region (newest first) ...            |
|                                                              |
+--------------------------------------------------------------+
|  [J] toggle   ·   4 unread   ·   +3 today    ·  write page    |  footer strip
+--------------------------------------------------------------+
```

- Header: fixed. Title, current in-game day, close button.
- Tab row: five tabs, `Log` is default and shows exactly what
  `JournalBookUI.DetailSummary` renders today (newest first, `Day N — author`,
  text below).
- Content: one long scrolling region shared by all tabs. Tabs only change the
  sort/filter of the same list — entries are never duplicated.
- Footer: unread count, today count, hint text. Doubles as status line source
  for the collapsed HUD strip.

## 5. Tabs and their data sources

### 5.1 LOG (playthrough + tutorial)
- Source: `JournalSystem.Entries` (64 max).
- Rendering: newest first. Each entry: timestamp line
  (`Day 74 · 09:40 — author`), then the full text, then an optional
  auto-generated footer tag (`[discovery]`, `[tutorial]`, `[lore]`,
  `[survivor]`) derived from `KnowledgeKey` prefix conventions already used by
  `TryAddRawEntry` (`anchor_broadcast_*`, etc.).
- Empty state: "No pages yet. Survivors write when they learn something."
  (already exists — keep verbatim).

### 5.2 ITEMS (item codex)
- Source: `ItemCatalogSO` (imported `ItemDefinition`), filtered to items the
  survivor group has *seen* (unlocked via `KnowledgeBase` keys
  `item_seen_<id>` — this is a NEW knowledge namespace, see §7).
- Sort: by `type` (grouped: Devices, Tools, Medical, Food, Water, Materials,
  Weapons, Protective, Filter, Fuel, Comfort, Trade, Quest, Relic).
- Rendering per item:
  ```
  — name (displayName) —                     [1.2 kg · trades ~30]
    description (full JSON text, verbatim)
  ```
- Stats line derived from definition fields; only non-zero stats shown
  (`radProtection`, `durability`, `contamination`, `hungerRestore`, etc.) —
  never a bare dump.
- Locked entries show `[---]` silhouette line: "Not seen yet. The bunker has
  not logged this."

### 5.3 PEOPLE (survivor files)
- Source: `SurvivorArchetypeSO` / active survivor instances.
- One entry per survivor: name, profession, then `bio` verbatim. For living
  survivors, append a one-line status (`injured`, `rad-sick`, `on watch`,
  `out scavenging`) derived from existing need/affliction state.

### 5.4 PLACES (location field notes)
- Source: `LocationDefinitionSO` descriptions, unlocked by
  `location_visited_<id>` knowledge keys (existing pattern in
  `world_history.json` discovery fields).
- Rendering: name, `dangerLevel` + `baseRadsPerHour` shown as text
  (`"Peril: moderate · Fallout: 14 rad/h"` — never as a bar chart), then the
  description verbatim.

### 5.5 EVENTS (event history)
- Source: `GameEvent.bodyText` for events that have fired this run (tracked
  via existing event-tracker/save state; fallback: last N fired events from
  the play log).
- Rendering: title, then body text verbatim. No re-wording.

## 6. Interactions

| Input | Action |
|---|---|
| [J] | toggle book open/closed (existing) |
| [Tab] / [1-5] | switch tab |
| [Wheel]/[Up/Down] | scroll content |
| [X] / [Esc] | close book (same as [J]) |
| click item/location row | (optional Phase 2) pinned detail view with the full description |

- Opening clears `HasUnread` + `NotificationPing` (existing behaviour in
  `ApplyUiState`/`Open`).
- Every state change raises C# events: `OnTabChanged`, `OnOpened`, `OnClosed`,
  `OnEntryPushed` (all already present or trivial additions) — UI and save
  both subscribe.

## 7. New knowledge namespace (required for codex tabs)

Introduce `JournalKnowledgeKey` constants (snake_case):

```
item_seen_dosimeter        — first time an item definition is revealed
location_visited_grange_hall
survivor_met_elena_vasquez
```

These keys are fed into the existing `KnowledgeBase` via
`JournalSystem.Knowledge.Discover(key)` from the same code points that
already unlock item/location discoveries (inventory grant, location visit,
survivor recruit). Save/restore is automatic — `KnowledgeBase` is already in
`JournalSave`. The codex tabs filter on these keys; nothing else changes.

**This is a separate implementation task (JournalCodexUnlocks), not part of
this plan's commit.**

## 8. Save/load

- `JournalSave` already serializes entries, knowledge, unread flags, open
  state. Add: `ActiveTab` (int), `LastSeenIndex` (int) for per-tab unread dots.
- Backwards compatible: default `ActiveTab = 0`, `LastSeenIndex = -1` when
  absent (plain fields with defaults; no version bump needed).

## 9. Accessibility / text-mode

- The whole book is ALSO rendered as the existing `StatusLine` + `DetailSummary`
  text block (current behaviour). On text-only displays (or if the graphical
  book is disabled), `[J]` toggles the text view — the same data, no loss.
- No colour-only signals: unread is text (`· NEW`) plus a glyph.
- Min font size 14px-equivalent; no italic for body text.

## 10. Out of scope (deliberately)

- Drawing/map tabs, crafting-tree UI, quest journal with objectives — those
  belong to other surfaces (map, workbench, quest HUD).
- Pagination vs scrolling: scrolling only; the 64-entry ring caps the log.
- Any new JSON files: text lives where it already lives (StreamingAssets),
  the book only reads it.
