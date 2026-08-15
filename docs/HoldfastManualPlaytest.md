# Holdfast Manual Playtest Checklist

**Environment:** Desktop Godot 4.7.1+ (.NET), launch via `godot --path .` or editor Play.  
**Estimated time:** 10–12 minutes.  
**Prerequisite:** Delete `user://holdfast_*` saves if you want a clean first impression.

---

## 1. Launch & Navigation

- [ ] **Launch** the project. No errors in the Output panel.
- [ ] The main menu is visible with the title "ASHFALL · ATOMIC WAR: STARVING SURVIVAL".
- [ ] Click **"Holdfast: open terminal"**.
- [ ] The terminal window appears with the title "THE HOLDFAST · QUARTERMASTER TERMINAL".
- [ ] Header buttons **SAVE**, **RELOAD**, **CLOSE [Esc]** are visible.
- [ ] The feedback line beneath the header is non-empty.
- [ ] Tab bar shows: **Status**, **Factions**, **Supplies**, **Inventory**, **Trade**.

---

## 2. Status Tab

- [ ] **Status** tab is selected by default.
- [ ] Status text contains:
  - [ ] A multi-line status summary (ice road, brine, census).
  - [ ] "Holdfast catalog: 3 factions · 35 locations · 40 items · 10 quests".
  - [ ] "Selected counterparty: faction_the_office" (or another active faction).
  - [ ] "Available value: 100".

---

## 3. Factions Tab

- [ ] Click **Factions** tab.
- [ ] The faction list shows exactly **3** entries.
- [ ] Entries are sorted alphabetically by ID.
- [ ] At least one faction shows **[ACTIVE]** and at least one shows **[DORMANT]**.
- [ ] Click **faction_the_office** (or the first active faction).
- [ ] The details panel on the right shows:
  - [ ] Faction display name and `[ACTIVE]`.
  - [ ] Id, Alignment, Region, Trust.
  - [ ] Wants and Offers lists.
  - [ ] Signature quote.
  - [ ] Access rule.

---

## 4. Supplies Tab

- [ ] Click **Supplies** tab.
- [ ] The supply list shows exactly **40** entries.
- [ ] Each entry shows: `DisplayName · Type · stock N`.
- [ ] Click any item (e.g., **item_fume_rag**).
- [ ] Details panel shows:
  - [ ] Display name, Id, Category, Description.
  - [ ] Unit value, merchant stock, player holdings.
  - [ ] Stack max, weight.
  - [ ] A line of marginalia (flavor text) beneath the hard data.

---

## 5. Inventory Tab

- [ ] Click **Inventory** tab.
- [ ] Summary shows:
  - [ ] "Available value: 100" (or current value if you have traded).
  - [ ] "Stacks: N · weight X.X/Y kg".
- [ ] If you have traded, your held items appear here with counts.
- [ ] If you have not traded, the list shows "Nothing stored. The shelves are bare."

---

## 6. Trade Tab — Buy

- [ ] Click **Trade** tab.
- [ ] The item selector dropdown lists all 40 items.
- [ ] The details panel shows the selected item's unit value, stock, and holdings.
- [ ] **Quantity** spin box allows values up to the available stock or holdings.
- [ ] Set quantity to **2**.
- [ ] Click **BUY SELECTED**.
- [ ] Feedback line shows a success message mentioning the item and quantity.
- [ ] **Inventory** tab now shows the purchased item with the correct count.
- [ ] Status line "Available value" has decreased by `unit value × quantity`.

---

## 7. Trade Tab — Sell

- [ ] Return to **Trade** tab.
- [ ] Select an item you currently hold.
- [ ] Set quantity to **1** (or a valid held amount).
- [ ] Click **SELL SELECTED**.
- [ ] Feedback line shows a success message.
- [ ] **Inventory** tab no longer shows that item (or shows reduced count).
- [ ] "Available value" has increased.

---

## 8. Trade Tab — Failure States

For each failure below, confirm:
- [ ] The feedback line shows a specific, non-dramatic message.
- [ ] **Inventory** tab and "Available value" are **unchanged** after the failed action.

| # | Failure | How to trigger | Expected message fragment |
|---|---------|---------------|---------------------------|
| 8a | Invalid quantity | Set quantity to **0**, click BUY | "Quantity must be at least one" |
| 8b | Insufficient funds | Set value to 1 (fresh ledger), buy expensive item | "Available value is below the listed worth" |
| 8c | Insufficient stock | Buy all stock of an item, then try to buy 1 more | "no stock at that quantity" |
| 8d | Insufficient inventory | Sell an item you do not hold | "No holdings of this item" |
| 8e | Unknown item | (Not normally reachable via UI) — skip |
| 8f | Unknown faction | (Not normally reachable via UI) — skip |
| 8g | Restricted | Select a dormant faction, attempt buy | "remains unavailable under current access rules" |

> **Note:** 8e, 8f, and 8g are guarded by the UI and are covered by the headless runtime test. Manual verification of 8a–8d is sufficient.

---

## 9. Save & Reload

- [ ] Perform at least one successful buy or sell.
- [ ] Click **SAVE**.
- [ ] Feedback line confirms save committed.
- [ ] Click **RELOAD**.
- [ ] Feedback line confirms state reloaded.
- [ ] **Inventory** tab and "Available value" match the pre-reload state.
- [ ] Trade a few more items to confirm the terminal is still functional after reload.

---

## 10. Fresh Ledger

- [ ] Click **NEW LEDGER**.
- [ ] Feedback line warns: "Press again within 3 seconds to archive current ledger and start fresh."
- [ ] Click **NEW LEDGER** again within 3 seconds.
- [ ] Feedback line confirms: "New ledger started. Prior records archived..."
- [ ] "Available value" is reset to the starting value.
- [ ] Inventory is empty.

---

## 11. Keyboard Navigation

- [ ] Press **Tab** to move focus between controls.
- [ ] Press **Enter** to activate the focused button.
- [ ] With focus on the Trade tab:
  - [ ] Press **B** to buy the selected item.
  - [ ] Press **S** to sell the selected item.
- [ ] Press **Esc** to close the terminal and return to the main menu.

---

## 12. Dispatch Log

- [ ] Scroll to the bottom of the terminal.
- [ ] The **DISPATCH LOG** section is visible.
- [ ] After a successful trade, a new dispatch line appears (quartermaster's voice).
- [ ] After a rejected trade, a dispatch line explains the refusal in-universe.
- [ ] After save/reload, a dispatch line notes the ledger was reopened.

---

## Automated Coverage Note

The following checks are verified headlessly by `--holdfast-runtime-ui-test` and do not need manual repetition:

- All 40 items render through the supplies and trade selectors.
- All 3 factions render through the faction selector.
- Post-reload rendering matches pre-reload.
- Failure-message matrix: InsufficientFunds, InsufficientStock, InsufficientInventory, UnknownItem, UnknownFaction, UnavailableOrRestricted.
- Save/reload round-trip preserves player value, inventory, and merchant stock.

---

*End of checklist.*
