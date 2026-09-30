# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 35

STATUS: APPROVED BY USER

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no ID, no specification, no ownership and no verification step.
> **Visual specification** and **Verification** below remain binding.

---

## 0. Framing — Fifteen Objects

> *"Four manuals and an undertaker's register. That is what a shelter keeps when it stops being
> able to teach anyone anything."*

Read the fifteen IDs as a list and two groups fall out. The first is a **pharmacopoeia**: a
precursor base, a sterile solvent, a stimulant, a tincture, salts, a resin, an antibiotic, a
sedative, and a toxin — which is to say, the complete arc from *making* to *soothing* to
*ending*, all of it in sealed bottles on one shelf.

Note the pair at the centre: `item_chem_clarity_salts` and `item_chem_haze_resin`. Clarity and
haze. A shelter that has both on the same shelf has stopped pretending it only needs one.

The second group is **things that carry knowledge** — generator maintenance, field medicine, rough
repairs, seismology. Four manuals is exactly what a community needs to keep running and not one
word more. And then the outlier: `item_undertakers_register`, which is not a manual and not a
medicine. It is the only object in this tranche that is *about the people rather than for them*.

**Tone & register.** Pharmacist-plain, archival, unsentimental. The vocabulary is the shelf:
*precursor, solvent, tincture, salts, resin, sedative, oxidizer, register*. Prose should read like
a dispensary ledger kept by someone who has stopped asking what the compounds are for.

**The interesting constraint.** *Medicine and toxin images depict sealed fictional containers
without instructions or readable text.* The objects must be recognisable at **26 px** while
carrying no label, no dose, no warning. A bottle that has to be identified by its silhouette and
its glass is a harder drawing than a bottle with a name on it — and it is the correct one for a
world where instructions are exactly what the survivors no longer have.

---

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_medical_precursor_base`, `item_sterile_solvent_pack`, `item_chem_hyper_stim`, `item_chem_dulcimer_tincture`, `item_chem_clarity_salts`, `item_chem_haze_resin`, `item_chem_fungal_antibiotic`, `item_chem_spore_sedative`, `item_chem_choke_spore_toxin`, `item_oxidizer_reagent_flask`, `item_manual_generator_maintenance`, `item_manual_field_medicine`, `item_manual_rough_repairs`, `item_manual_seismology`, `item_undertakers_register`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and have no direct or normalized-prefix art in the current `AssetRegistry.GetItem` search roots. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing host presentation seam. Root owns only these fifteen new JPEGs and matching Godot import sidecars, five editable SVG sources under `docs/visual/sources/tranche35/`, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

One centered object or cohesive small kit per square asset, opaque near-black background, tactile grounded hand-painted realism. Distinguish precursor, solvents, stimulant, tinctures, salts, resin, fungal medicines, oxidizer, four manuals, and burial register at 26 px. Medicine and toxin images depict sealed fictional containers without instructions or readable text. No real insignia, people, copied art, or copied marks.

The built-in image service reached its usage limit after ten illustrations. The four manuals and register are locally constructed SVG illustrations rendered with Inkscape; their source files are retained for editing.

## Verification

Check fifteen opaque 512×512 JPEGs; inspect 64 px and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars; run scoped `git diff --check`. Art-only additions do not call for gameplay tests.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not content gaps and not art backlog.** They are the questions the specification
refuses to answer, and the refusal is the tone. Any later tranche that answers one must say so.

**What the undertaker's register contains.** It is drawn as an object. No page is legible, no name
appears, and none may. It is the only item in the tranche that is *about* the survivors rather
than *for* them, and it earns that by saying nothing.

**What the precursor is a precursor to.** `item_medical_precursor_base` is the base of several
compounds in this same tranche. The chain is implied by proximity on a shelf and never drawn.

**Why clarity salts and haze resin are both stocked.** Two names that negate each other, in one
inventory. Whether they are opposites, stages, or a joke someone made is not authored.

**What the four manuals teach, and to whom.** Generator maintenance, field medicine, rough repairs,
seismology — four competencies and not a fifth. Why *seismology* in particular is not explained,
and a shelter that owns that book has heard something the drawing will not show.

**What dose any of it carries.** *Sealed containers without instructions or readable text.* A
medicine you cannot read the label on is a gamble; the specification makes it a drawing instead.
The rule is a safety rule and a tone rule at the same time.
