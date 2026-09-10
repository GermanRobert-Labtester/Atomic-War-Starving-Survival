# Plan 144 — Merge-Prefix Contract

## Source of truth

`moral_choice_chains.json` → `merge_rules`:

```json
{
  "description": "A player on Branch A can access merge quests from Branch B
    only if merge_allowed includes B. Merge quests are marked with 'merge_from'
    in their prerequisites. Merging does NOT unlock the merged branch's
    exclusive content — only the shared merge-point quests.",
  "merge_quest_prefix": "quest_moral_merge_",
  "merge_quests_require_min_progress": 5,
  "merge_never_unlocks_exclusive": true
}
```

Loaded by `MoralChoiceChainCatalogLoader` into `MoralChoiceChainData.MergeQuestPrefix`.

## Current runtime status (evidence)

- `MergeQuestPrefix` is loaded but **consumed by no runtime code** — there is
  no merge-quest discovery, no `merge_from` marker in any quest catalog, and
  no merge-eligibility logic in `MoralChoiceSystem`/`MoralBranchingSystem`.
- No quest in any canonical catalog matches the prefix today.
- The only quest ever authored under this token was the stub file's
  "Ghosts of the Wasteland" (`id` = the bare prefix string), which no loader
  ever read.

## Contract (normative from Plan 144 onward)

1. **Legal merge quest ID grammar**: `quest_moral_merge_` + non-empty
   snake_case suffix, e.g. `quest_moral_merge_<name>`. The bare prefix string
   (trailing underscore, empty suffix) is **not a quest id**.
2. **`merge_quest_prefix` application**: it is a *pattern token* used to
   recognize merge quests among catalog definitions
   (`id.StartsWith(prefix) && id != prefix && id.Length > prefix.Length`).
3. **Discovery**: actual merge quests are discovered by prefix match against
   the canonical playable catalogs (the same union the runtime loads:
   base/branching/expansion/distress). A merge quest must live in one of
   those catalogs with a full definition; it must never be manufactured by
   the prefix alone.
4. **Minimum progress**: `merge_quests_require_min_progress = 5` — branch
   progress (entry-quest resolutions) must be ≥ 5 before a merge quest from
   an allowed branch becomes accessible.
5. **`merge_allowed` semantics**: one-directional per branch
   (mercy↔listener, iron↔broken-compact). Merging grants access only to the
   shared merge-point quest; it never unlocks the merged branch's exclusive
   entry/gated quests (`merge_never_unlocks_exclusive`).
6. **Validation**: `merge_quest_prefix` is schema/prefix validation in
   `CatalogIntegrityValidator` (`PrefixPatternKeys`), NOT a quest foreign
   key. Any concrete prefix-matching id referenced anywhere must still
   resolve to a full definition via ordinary Tier-1/Tier-2 rules.

## Tests pinning the contract

See `Ashfall.Core.Tests/MoralChoice/Plan144MoralChoiceStubClosureTests.cs`:
- the prefix string itself is never a registered quest id and never
  resolvable/selectable as a quest (`MoralChoiceSystem` resolve fails);
- a prefix-matching id WITHOUT a definition is an unresolved-id error
  (never auto-blessed by the prefix);
- a stub+full definition pair for the same id is a duplicate-executable error;
- the validator accepts the shipped prefix pattern with the stub file gone;
- a malformed prefix value (no trailing underscore) is an error.
- merge access not unlocking exclusive content is a **runtime** semantic —
  deferred with the merge mechanic itself (no runtime merge code exists to
  test against; the data contract above is what implementation must follow).
