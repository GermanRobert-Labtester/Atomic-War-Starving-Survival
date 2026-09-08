# Feedback Accessibility & Usability Rules

## 1. Multi-Modal Severity Communication

Never rely solely on color to convey severity. Every feedback message must present a textual severity prefix and distinct visual treatment:

| Severity | Color Token | Hex Anchor | Prefix | Display Duration |
|---|---|---|---|---|
| `critical` | `Theme.Critical` | `#C84B31` / Red | `[CRITICAL]` | 5.0s |
| `error` | `Theme.Critical` | `#C84B31` / Red | `[ERROR]` | 4.0s |
| `warning` | `Theme.Entropy` | `#E09F3E` / Amber | `[WARNING]` | 4.0s |
| `success` | `Theme.Warm` | `#D4A373` / Green-gold | `[SUCCESS]` | 3.5s |
| `info` | `Theme.Pale` | `#EDEDED` / Neutral | `[INFO]` | 3.0s |

---

## 2. Layout & Presentation Rules

1. **Screen Positioning:**
   - Toast overlay is anchored at the top-right / upper center of the viewport, below `GameHudOverlay` so meters and menu buttons remain visible.
2. **Stacking & Capacity:**
   - Maximum 4 concurrent visible toasts. Additional messages are queued and dequeued as older toasts dismiss.
3. **Typography & Readability:**
   - Font: `BarlowCondensed` / `Theme.FontSizeBody` (minimum 14px for body text, 12px for metadata).
   - High-contrast background panels (`#1A1A1A` with 90% opacity, distinct borders).
   - Automatic text wrapping enabled; cards expand vertically to accommodate multi-line messages without clipping.
4. **Pause on Hover/Focus:**
   - If mouse hovers over a toast or keyboard focus is active on it, auto-dismiss timers are paused to give the player sufficient reading time.
5. **Dismissal Controls:**
   - Each toast card provides a close button (`×`) that can be activated via mouse click or keyboard shortcut.
