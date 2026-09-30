# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 41

STATUS: APPROVED BY USER

Lane: Item art (tranche 41)

---

## 0. Framing — Fifteen Voices in a Sleeve

> *"A cassette is a voice that agreed to be kept. A casualty list is a voice that did not."*

This tranche illustrates the shelter's **memory media**: five paper-and-metal artifacts (a casualty
list, an evacuation route map, a civil defence poster, a child's drawing, scavenged dog tags) and
ten tapes, each from a distinct archive with its own sleeve, symbol, palette and wear cue. Nothing
here is a weapon or a resource. Everything here is *testimony with a physical body*.

The visual spec's hardest line is **"abstract unreadable markings"** — and it is the same ethic as
the corpus's held silences. The artifacts must look *specific* and read as *unreadable*: evidence
without content, a name you cannot quite make out, a waveform you cannot unspool. Wear and framing
are the only authorship permitted. The 26 px silhouette test is the discipline: if the artifact
cannot be recognised at the size of a fingernail, its grief does not ship.

**Tone & register.** Curatorial, restrained, forensic. The vocabulary is the archive: *sleeve,
symbol, wear cue, silhouette, framing, variant*. Prose should read like a conservator's notes on
evidence that nobody is left to claim.

**The second layer.** These fifteen icons are the only place in the inventory where the game's past
is *physically present* — a tape you can carry is a person who once spoke into a machine. The
tranche's insistence on distinct per-family sleeves is what keeps ten tapes from collapsing into
one prop: each archive keeps its own handwriting, even illegibly.

**Texture (second prose pass — commentary only).**

- The child's drawing is the tranche's quiet centre: the one artifact whose unreadable markings were always going to be unreadable.
- Dog tags *scavenged* — the adjective is doing narrative work the icon must not contradict.
- Ten cassettes from ten archives means ten palettes that must disagree gently at 64 px and agree strongly at 26 px.

*Deliberate limits: the Visual specification and Verification sections below are this tranche's
register — binding and deliberately unenlarged. No new open items are created here.*

---

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_document_casualty_list`, `item_document_evacuation_route_map`, `item_document_civil_defense_poster`, `item_document_child_drawing`, `item_dog_tags_scavenged`, `cassette_greenhouse_tapes_2`, `cassette_field_hospital_7_2`, `cassette_evacuation_train_2`, `cassette_station_14_2`, `cassette_teachers_recordings_1`, `cassette_quarantine_tapes_1`, `cassette_checkpoint_kilo_1`, `cassette_saint_maren_1`, `cassette_family_bunker_1`, `cassette_free_radio_1`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and lack direct or normalized-prefix art in the current `AssetRegistry.GetItem` item roots. No selected ID has a semantic alias or conflicting exact art claim; prior cassette claims concern catalog prose only. Similar catalog objects already illustrated use other IDs, so these variants need distinct wear and framing. `potassium_iodide` was excluded because its prefixed `item_potassium_iodide` art already exists. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` files reach the current presentation seam. Root owns fifteen new JPEGs and import sidecars, fifteen editable SVG sources under `docs/visual/sources/tranche41/`, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

Draw five worn paper or metal artifacts and ten tapes from distinct archives. Each tape needs a family-specific sleeve, symbol, palette, and wear cue. Use opaque charcoal backgrounds, restrained fictional materials, clear 26 px silhouettes, and abstract unreadable markings. No real insignia, recognizable people, or copied marks.

## Verification

Confirm fifteen opaque 512×512 JPEGs; inspect full-size, 64 px, and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars and SVG sources; check the scoped diff. Art-only additions do not require gameplay tests.
