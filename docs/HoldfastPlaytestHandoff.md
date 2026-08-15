# Holdfast Playtest Handoff

**Environment:** Desktop Godot 4.7.1+ (.NET), display available (`$DISPLAY=:0`).  
**Launch command:** `godot --path .` (or press Play in the Godot editor).  
**Estimated time:** 10–12 minutes.  
**Checklist:** `docs/HoldfastManualPlaytest.md`  
**Automated coverage:** All 40 items, 3 factions, failure matrix, save/reload, and New Ledger are verified headlessly by `--holdfast-runtime-ui-test`.

---

## Quick-start

1. Open a terminal in the project root.
2. Run `godot --path .`.
3. When the main menu appears, click **"Holdfast: open terminal"**.
4. Work through `docs/HoldfastManualPlaytest.md` checkbox by checkbox.
5. Record outcomes in the template below.

---

## Findings template

| Step | Expected | Observed | Severity | Screenshot / notes |
|------|----------|----------|----------|-------------------|
| 1. Launch | No errors in Output panel | | | |
| 2. Status tab | Catalog counts correct | | | |
| 3. Factions tab | 3 entries, sorted, active/dormant | | | |
| 4. Supplies tab | 40 entries, details populate | | | |
| 5. Inventory tab | Value + stacks shown | | | |
| 6. Buy | Value decreases, inventory updates | | | |
| 7. Sell | Value increases, inventory updates | | | |
| 8a. Invalid quantity | "Quantity must be at least one" | | | |
| 8b. Insufficient funds | "Available value is below..." | | | |
| 8c. Insufficient stock | "no stock at that quantity" | | | |
| 8d. Insufficient inventory | "No holdings of this item" | | | |
| 9. Save/reload | State restored, trade continues | | | |
| 10. New ledger | Two-press confirms, state resets | | | |
| 11. Keyboard | Tab/Enter/B/S/Esc work | | | |
| 12. Dispatch log | Quartermaster entries appear | | | |

**Severity:** Blocker / Major / Minor / Cosmetic

---

## Notes for the tester

- The terminal uses a dark phosphor palette. If text is hard to read, adjust the Godot editor's theme contrast settings.
- "New ledger" requires two quick presses (within ~3 seconds) of the header button. If the timer expires, press once more to re-arm.
- Save files live under `user://` (`~/.local/share/godot/...`). Delete `holdfast_s1_save.json` and `holdfast_trade_save.json` for a clean start.
- Quarantined saves appear as `holdfast_s1_save.json.corrupt-<timestamp>` in the same directory.

---

*Handoff prepared by the release audit pass.*
