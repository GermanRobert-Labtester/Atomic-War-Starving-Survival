# Moral Flag Gossip Handoff

`MoralChoiceGossipRuntime` selects plain strings from section/band pools. `moral_choice_gossip.json` has no per-line IDs, tags, or conditions, so a flag-specific line cannot be safely gated by the current runtime.

No unsupported gossip metadata was added. Live integrations: 0/3. Staged candidates:

- `flag_shared_rations` → camp chatter about a costly ration decision.
- `flag_sheltered_refugee` → greeting or chatter about a gate decision.
- `flag_ignored_distress` → private warning about an unanswered signal.

Until contextual filtering exists, these must remain thematic plain-band lines or live in an owning contextual dialogue system. Gossip never writes or mutates moral flags.
