# ASHFALL — UI Visual Information (text spec)

> Text-only visual direction for the UI, written for artists and for
> AI-generation prompts (per AGENTS.md, generated assets go in
> `generated_AIassets/`). Complements `UI_DESIGN_PLAN_FIGMA_CANVA.md` and
> `Assets/_Game/UI/Phase11/design-tokens.json` — this file describes the
> *look* in words so any tool can render it.
> Companion doc: `docs/ui/JOURNAL_UI_PLAN.md`.

## 0. Content audit (what the game now ships)

| File | Entries | Text field | Avg words |
|---|---|---|---|
| `items.json` | 437 | `description` | 61 |
| `survivors.json` | 100 | `bio` | 104 |
| `locations.json` | 88 | `description` | 110 |
| `events.json` | 49 | `bodyText` | 127 |

All authored in house voice: cold, exhausted, human, restrained; specificity
over adjectives; no magic, no real countries, no glorified violence. The UI
must never contradict or editorialize this text.

## 1. Global look — the Bunker Ledger

The UI is a single family of forms made to look like paperwork and
hand-annotation made after the exchange, drawn on screens, not simulated
paper.

**Ink on paper, degraded by use.**

- Backgrounds: dark desaturated paper greys, not black. Tonal range roughly
  `#1c1b18` (deep ash) to `#c9c2ae` (aged paper) to `#e8e2ce` (clean sheet).
- Ink: near-black warm grey `#2a2722`. Accent ink: faded stencil red
  `#8a3b2e` for warnings, faded stencil yellow `#b98a3e` for watchfulness.
- Paper noise: a fine grain overlay (use the existing
  `phantom-memory-vignette`-style radial noise texture; keep opacity under
  12%).
- Edges: rounded corners only where the game already has them; panels look
  like stapled sheets, not glass cards. 1px borders in `#3a362e`.
- No drop shadows beyond a 2px hard offset like a pressed-down paper edge.

## 2. Type

- Family: the existing UI font; body text rendered at sizes 14-18px-equivalent.
- Hierarchy by weight and size only: title (bold, all-caps, letter-spaced),
  section header (bold), body (regular), caption (regular, +10% grey).
- Body text is left-aligned, ragged right. Justification and hyphenation are
  forbidden.
- Timestamps and id-like strings (`Day 74 · 09:40`) render in the same family,
  caption size, in the warm grey ink.

## 3. Item tooltip / codex entry (visual in text)

```
+-------------------------------------------------------+
| DOSIMETER                          [1.2 kg · trade 30]|
|                                     · device          |
|                                                       |
| A pen-sized instrument on a worn lanyard...           |
| (description verbatim, up to ~12 lines, scrolls)      |
|                                                       |
| RAD PROTECTION —         ·                        --- |  stat rows:
| DURABILITY   —           ·                        --- |  label left,
| CONTAMINATION— 0.5       ·                        --- |  value right,
| HUNGER       — 40        ·                        --- |  only non-zero
| THIRST       —           ·                        --- |  stats shown
+-------------------------------------------------------+
```

Rules:
- Name in bold caps; weight + trade value in caption on the same line.
- Type word (device/tool/food/...) in the stencil accent colour, caption size.
- Description verbatim, never truncated in the tooltip (scroll if needed).
- Stat rows: label left, value right, separated by a dotted leader line made
  of `.` characters (paper-form style). Values as numbers, no bars.
- Negative/risk stats (contamination) render in stencil red ink.
- No icons inside the tooltip except the item's own sprite at 48px, left of
  the name.

## 4. Journal book (visual in text)

Per `docs/ui/JOURNAL_UI_PLAN.md` §4. Visual translation:

- Book frame: a two-page spread illusion — central gutter line, slight
  left-page offset shadow. The spread is the header strip + content region;
  no fake page turning animation in v1.
- Header strip: `[J] BUNKER LEDGER   Day 74` in bold caps; the `[X] close`
  glyph on the right in stencil red.
- Tabs: underline-style tab labels in the paper family; the active tab's label
  in bold ink with a 2px red underline; inactive tabs in 60% grey ink.
- Content rows (log entries): timestamp caption line in warm grey; body in
  ink; 12px vertical gap between entries; a thin 1px rule between entries that
  share a day.
- Unread indicator: a 6px filled dot in stencil red before the timestamp, plus
  the text `· NEW` in the same ink. Never colour-only.
- Footer strip: caption size, `[J] toggle · 4 unread · +3 today`.

## 5. Strip HUD (collapsed journal on the main HUD)

Already implemented as text (`JournalBookUI.StatusLine`). Keep exactly this
character layout in the visual HUD:

```
JOURNAL · NEW  Day 74 · Elena  Caught in a chain-link fence…  [J]
```

- A single line, caption size, paper-family on a translucent paper-chip
  background (rect 240px tall max, right-aligned above the bottom strip).
- `· NEW` in stencil red; everything else in ink.
- The chip has a 1px border and the same grain overlay; no rounded corners
  unless the strip family already uses them.

## 6. Colour tokens (tie to Phase 11 design-tokens.json)

Reuse the existing token names; add these only if missing (values are the
paper family):

| Token | Value | Use |
|---|---|---|
| `paper_deep_ash` | `#1c1b18` | panel backgrounds |
| `paper_sheet` | `#e8e2ce` | text foreground on dark |
| `paper_aged` | `#c9c2ae` | secondary text |
| `paper_ink` | `#2a2722` | primary text on light panels |
| `paper_rule` | `#3a362e` | 1px borders, rules |
| `paper_warn_red` | `#8a3b2e` | warnings, unread dots, negative stats |
| `paper_watch_amber` | `#b98a3e` | watchfulness, today counters |

Existing tokens (radiation phases, moral, keepsake gold, terminal amber) stay
authoritative for their domains; the paper family only governs the journal
and tooltip surfaces.

## 7. Motion (text spec)

- Book open: 120ms fade + 8px downward settle (paper laid down, not slid in).
- Tab change: no animation; instant with a 1-frame ink-fade.
- Unread ping: the dot pulses twice at 10Hz on entry push, then holds steady.
- No parallax, no spring physics, no screen shake for any of these surfaces.

## 8. Accessibility

- Contrast: text on paper meets WCAG 2.2 AA against both dark and light
  variants (checked at 14px minimum).
- Colour is never the only signal (see `· NEW` text, stat labels).
- Focus/keyboard: `[J]`, `[Tab]`, `[1-5]`, `[Esc]` navigation per the journal
  plan §6; focus ring is a 2px stencil-amber outline.
- Text-mode fallback: the full book content is also available as plain text
  (existing `DetailSummary`), preserving every feature without graphics.
