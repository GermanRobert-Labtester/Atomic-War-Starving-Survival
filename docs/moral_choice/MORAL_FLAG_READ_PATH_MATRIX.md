# Moral Flag Read-Path Matrix

## Live generic readers

- `MoralChoiceSystem.HasFlag` reads the canonical moral state/shared ledger.
- `MoralChoiceSystem.EvaluateGate` already supports `requires_flag` for moral quest gates.
- `MoralChoiceSystem.RestoreState` rehydrates the shared ledger from `activeFlags`.

## Plan 125 additions

The new 15 flags are written and persisted, but the current downstream catalogs do not expose flag predicates:

| Consumer | Live flag predicate support | Plan 125 status |
| --- | --- | --- |
| Plan 109 echoes | Echo records match source quest + choice + delay + branch only | Staged; see `MORAL_FLAG_ECHO_HANDOFF.md`. |
| Plans 121–123 PONR | Branch systems use branch lock flags, moral bands, standing, and hostility | Staged; see `MORAL_FLAG_PONR_HANDOFF.md`. |
| Plan 100 reactions | Reactions are keyed by threshold event ID | Staged; see `MORAL_FLAG_FACTION_REACTION_HANDOFF.md`. |
| Plan 110 gossip | Runtime selects plain strings by moral band and section | Staged; see `MORAL_FLAG_GOSSIP_HANDOFF.md`. |
| Plan 89 epilogues | Current ending selection uses score/empathy/resolution count | Audited; see `MORAL_FLAG_EPILOGUE_HANDOFF.md`. |

No unsupported consumer fields were authored.
