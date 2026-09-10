# Plan 144 — Integrity Validator Gap & Fix

## Reproduced defect (before any fix)

With `moral_choice_quest_stubs.json` temporarily removed:

```
[DATA] unresolved id 'quest_moral_merge_' at moral_choice_chains.json/merge_rules/merge_quest_prefix
DATA_INTEGRITY_SELFTEST FAIL (1) — 299 catalogs
```

Root cause chain:
1. `CatalogIntegrityValidator` Tier-1 requires every string starting with a
   known id prefix (`quest_`) in a non-id position to resolve against the
   id registry.
2. `merge_rules.merge_quest_prefix = "quest_moral_merge_"` is an id **grammar
   prefix**, not a quest id — but Tier-1 treated it as a concrete foreign key.
3. The stub file registered `quest_moral_merge_` as a definition (a full,
   never-loaded quest) purely so Tier-1 would pass — masking the defect and
   simultaneously creating divergent duplicate definitions for 9 chain ids,
   which the validator miscounted as legitimate cross-file "reuse".

## Fix (Workstream 144B + 144C)

In `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`:

### 1. Prefix-pattern key contract (144B)

New `PrefixPatternKeys = { "merge_quest_prefix" }`. Values at these keys are
NEVER registered and never enter the Tier-1 reference set. Instead a shape
check runs immediately:
- value must be non-empty,
- must end with `_`,
- must start with a known id-namespace prefix (`StartsWithAny(value, IdPrefixes)`),
otherwise:
`error: prefix-pattern key 'merge_quest_prefix' value '...' at <path> must be
a trailing-underscore id-namespace prefix (not a concrete id)`.

Deterministic: pure string checks, no ordering effects.

### 2. Duplicate executable quest definition gate (144D)

Post-pass over the registry:
- Collect every `quest_`-prefixed id registered at an entity definition
  position — path shape `file.json/<container>[N]/id` or `file.json[N]/id`.
- Group by id → distinct files.
- Classify each file's definition **executable** iff the quest object at that
  position contains any of `choices` / `stages` / `objectives` / `steps`
  (playable quest grammar) — `questline_master.json` entries never do, so
  registry-only acknowledgement remains legal.
- **Error** iff ≥2 distinct files classify the id as executable:
  `duplicate executable quest definition 'ID' defined in <fileA> and <fileB>
  — one quest id must have exactly one executable definition (Plan 144)`.
- Same id with divergent outcomes therefore cannot hide again: the gate
  fires on any cross-file executable pair regardless of content equality.
- Deterministic: files are parsed from the already-sorted file list; grouped
  reporting uses ordinal file order.

### 3. No global weakening

Tier-1 and Tier-2 foreign-key validation are untouched for every other key.
The exemption applies only to the documented prefix-grammar keys.
