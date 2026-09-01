# Muster Testimony Style Guide (Plan 25 · 25B.7)

> The Muster is testimony, not a narrator scoring screen. These rules are contract for every authored variant in `muster_witnesses.json` v2.

## Length
- One variant: **40–110 words**. It must read aloud in a gathering without losing the room.
- No variant may exceed ~130 words; if it needs more, the testimony is doing exposition's job — cut it.

## Specificity
- Name **one concrete act** the player did or failed to do (a paid toll, a refused appeal, a blacklist, a chosen strategy letter's consequence).
- Anchor in a place or object the campaign knows (`loc_*` ids, real items, real faction institutions — the claim ledger, the rate card, the neutral-ground pot).
- Testimony may reference campaign facts only through what the witness could know. No omniscience.

## Voice
- First person, plain, tired. Cold, exhausted, human, restrained (repo tone rules).
- Dialect only where faction canon established one (Guild ledger-speak, Hydro accounting idiom, Raider spare codes). Never slur or comic register.
- The witness is wrong sometimes. Let them be wrong about motive while right about fact.

## Moral stance
- No omniscient moral judgment. A helped-variant is gratitude for a thing done, **not** a verdict that the player is good.
- A failed-variant names the harm and the cost. It is allowed to be unfair; it is not allowed to be generic.
- Variants may contradict each other across witnesses. Contradiction is the point — the Muster is a reckoning by people, not a scoreboard.
- No witness calls the player a hero. No witness calls the player a monster. They say what happened to them.

## Variants
- Minimum two per witness: **helped** (what the player did that mattered) and **failed** (what the player did, enabled, or refused to do).
- Complicated variants are preferred where the campaign can produce them (helped the person, harmed their faction/community).
- Selection is first-match on real flags (v2 `requires_*_flags`); the unconditional variant is the "never encountered their issue" case and must stand alone as honest, not as filler.

## Forbidden
- Praise/condemnation adjectives stacked on the player ("brave", "cruel", "selfless").
- Recap narration ("you arrived, you built, you chose...").
- Faction lore recitals (culture belongs in the codex, not testimony).
- Threats of mechanical consequence inside testimony prose (tone only — mechanics live in effects).
- Real countries, wars, people; magic; glorified violence (repo content rules).

## Mechanical contract recap
- `day_min` gates availability; `subject_id` + `IWitnessEligibility` gate alive/dead; flag conditions gate variant; `priority` then id ordinal gate order.
- Dead subjects never testify. Absence is handled by the fallback variant, representation, or epilogue remembrance — never resurrection.
- Results persist (`witness_id → variant_id, delivered_day`) for Plan 15A/15B consumption.
