# Plan 21 — Phantom Memory & Heirloom World Layer

> **Theme:** The `PhantomMemoryEngine` (memento/heirloom triggers) is one of ASHFALL's most
> distinctive systems — objects that carry memory of the dead — yet it has **7 trigger entries**
> and no heirloom items of its own. This plan turns it into a pervasive world layer.
>
> **Key evidence (verified):** `phantom_triggers.json` = **7 items**; **no**
> `phantom_heirlooms.json` exists; `Assets/Ashfall.Core/Phantoms/` holds only
> `PhantomTriggerDto.cs` (the engine is elsewhere — `PhantomMemoryEngine.cs`); registry flags
> "expand heirloom artifact triggers" as a top safe extension.

---

## Task 21A — Phantom trigger catalog expansion (7 → 30)

**Goal:** Expand the phantom-trigger catalog so scavenged objects routinely carry authored
memory echoes, making loot emotionally loaded instead of purely material.

**Files:** `phantom_triggers.json` (primary), `items.json` (objects that carry triggers),
read-only `PhantomMemoryEngine.cs`, `Phantoms/PhantomTriggerDto.cs`.

**Substeps:**
1. Read `PhantomMemoryEngine` + `PhantomTriggerDto` to learn the trigger schema (object id, memory text, trigger condition, emotional payload, survivor-affinity).
2. Read the 7 existing triggers to lock voice — these are memory *fragments*, second-person, restrained, devastating by understatement.
3. Design trigger taxonomy: personal mementos (a wedding band, a lunch pail), work objects (a foreman's whistle, a nurse's fob watch), pre-war ordinary (a bus ticket, a recipe card).
4. Author 8 personal-memento triggers tied to existing `item_*` keepsakes.
5. Author 8 work-object triggers tied to trade/industrial items (foundry, farm, medical).
6. Author 7 ordinary-object triggers — the most affecting because the object is mundane.
7. Add survivor-affinity keys where a trigger lands harder for a survivor with a matching backstory (uses `SurvivorRelationsSystem`/trait match).
8. Decide which triggers grant a small morale/guilt effect vs. pure lore (morale → `NeedsSystem`, guilt → `GuiltInsomniaSystem`; keep payloads within existing fields).
9. Validate all `item_`/trait refs; data-integrity selftest; narrative-continuity (no canon clash).
10. xUnit: trigger fires on object acquisition, affinity match amplifies, effect applies, save round-trip of seen-triggers.

**Next steps:** the most affecting triggers become final-wish objects (06A); a "returned the
memento" resolution (give it to the right NPC in 20B) for closure.

---

## Task 21B — Heirloom items & inheritance chains

**Goal:** Create a real heirloom item class — named objects that persist, accrue history, and
can be passed down — feeding both the phantom engine and New Game+ legacy (15C).

**Files:** new `phantom_heirlooms.json` (or extend items with an heirloom tag via
`expansion_item_tags.json`), `items.json`, read-only `PhantomMemoryEngine.cs`,
`GenerationalLineageExtension.cs`, `LegacyInheritanceSystem` (15C, if built).

**Substeps:**
1. Confirm whether heirlooms are a distinct catalog or an item tag (`expansion_item_tags.json` has 67 tags — check for an heirloom/keepsake tag first; reuse before creating).
2. Design heirloom schema: named object, provenance chain (who held it, when, fate), memory triggers per holder.
3. Author 12 heirlooms: a grandfather's dosimeter, a mother's recipe tin, a regiment lighter, a midwife's satchel, a lighthouse keeper's logbook.
4. Give each a 2–3 generation provenance chain (pre-war origin → exchange survivor → current) written as phantom memory fragments (21A voice).
5. Wire heirloom inheritance: on a survivor's death, the heirloom passes to a bonded/kin survivor via `GenerationalLineageExtension`.
6. Add per-holder memory unlocks — the object "remembers" differently depending on whose hands held it.
7. Mark 2–3 heirlooms as New Game+ legacy candidates (15C inheritance set).
8. Validate ids/tags; data-integrity selftest.
9. xUnit: heirloom provenance append on inheritance, memory unlock per holder, determinism.
10. Save round-trip: heirloom state (holder, provenance) survives save/load and NG+.

**Next steps:** heirloom display in the memorial/decor wall (12C); a "family heirloom completed"
chronicle line in the epilogue (15A).

---

## Task 21C — Confession & secret world-objects

**Goal:** Expand `confession_secrets.json` (**8 entries**) into a system of discoverable
confessions/secrets — hidden truths about NPCs and factions that create moral leverage.

**Files:** `confession_secrets.json` (extend), `characters.json` (20B NPCs), faction data,
read-only: locate the consumer of `confession_secrets.json` (grep for the loader), `MoralBranchingSystem.cs`.

**Substeps:**
1. Find and read the confession/secret consumer system; learn the schema (subject, secret, discovery method, leverage/consequence).
2. Read the 8 existing secrets to lock the tone (damning but human, never cartoonish).
3. Author 8 NPC-personal secrets tied to the 20B named NPCs (a hoarded stash, a wartime desertion, a hidden kin, a betrayed partner).
4. Author 6 faction secrets (a famine covered up, a rigged census, a poisoned well blamed on a rival) tied to 16C treaty politics.
5. Author 4 bunker-internal secrets (someone is skimming rations; a sealed room) that surface via 12B social events.
6. For each secret, define the discovery path (document find 17B, overheard radio 11B, a confession at a deathbed 09C).
7. Define the leverage options: expose (faction standing + guilt), blackmail (resource gain + moral hardening), keep (trust gain) — using existing standing/morale/hardening hooks.
8. Ensure secrets produce/consume real `flag_` ids; no orphans (dialog-graph lint).
9. Data-integrity selftest + narrative-continuity.
10. xUnit: secret discovery → leverage choice → consequence (standing/morale/hardening); determinism.

**Next steps:** secrets as Verdict testimony (15B); a blackmail-economy risk (discovered
blackmail → feud); confession booth as a shelter room function.
