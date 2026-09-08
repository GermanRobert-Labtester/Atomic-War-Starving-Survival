# Moral Flag Faction-Reaction Handoff

`moral_choice_faction_reactions.json` is currently keyed by threshold event ID and contains dialogue/journal payloads. Its loader has no `requires_flag` field, and `Main.MoralChoice` requests reactions by event ID.

No unsupported flag predicates were added. Live integrations: 0/3. Staged candidates:

- `flag_broke_treaty` → affected accord faction reaction.
- `flag_sabotaged_rival` → rival faction reaction.
- `flag_preserved_archive` → knowledge-keeper reaction.

Reaction knowledge must remain plausible; the faction reaction system, not the flag store, should own one-shot reaction delivery and standing effects.
