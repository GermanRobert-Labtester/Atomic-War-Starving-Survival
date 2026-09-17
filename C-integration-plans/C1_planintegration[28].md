# C1 — Flagship Integration Plan [28]: Shelter Identity, Naming, Origin & Community Reputation Projection

> **Output:** `C1_planintegration[28].md`
>
> **Source baseline:** Plan 166 — Shelter Identity & Naming System
>
> **Primary mission:** give the player's shelter a durable name, origin, optional motto/emblem, and historically grounded community identity that appears consistently across onboarding, UI, journal, feedback, factions, archives, governance, quests, and epilogues.
>
> **Primary architectural rule:** shelter identity owns **identity facts**. It does not own faction standing, settlement standing, infamy, trade prices, refugee flow, rumor propagation, governance, archive history, or dialogue state when those already have canonical authorities.
>
> **Primary continuity rule:** the source proposes a `ShelterReputation` DTO with per-faction/per-settlement scores and a new `infamy` meter. That is high-risk duplication given Plan 139 faction consequences, Plan 147 NPC memory, Plan 155 underground heat/public criminality, Plan 131 rumor/intelligence, Plan 159 governance, and existing `FactionStanceEngine` / `FactionBranchCoordinator`. The flagship therefore converts “reputation” into a **derived community reputation/read model** over existing evidence unless repository audit proves a genuinely missing canonical fact.
>
> **Mandatory execution order:** 166A authority/reputation overlap audit → 166B persistent identity/name/origin contract → 166C origin effects through existing shelter systems → 166D name propagation/tokenization + archive/governance integration → 166E derived known-for/community reputation projection → 166F UI, persistence, old-save compatibility, determinism, accessibility, reachability and CI → 166G optional emblem/legacy extensions.
>
> **Critical scope rule:** shelter naming is low-risk; shelter origin modifiers are medium/high-risk because they can duplicate room, shielding, defense, ventilation, supplies, education, and access systems; a new global reputation/infamy system is high-risk and should be rejected by default unless a source-of-truth audit proves existing faction/public-history systems cannot express the required behavior.
>
> **Guardrails:** no second faction reputation table; no second settlement-standing table; no new global infamy meter by default; no origin bonus applied by hidden global multiplier if a real system owns the underlying fact; no room/layout mutation from identity code if shelter-layout authority exists; no starting-item injection outside canonical inventory/bootstrap transaction; no journal strings manually replacing `"the shelter"` everywhere; no dynamic format-string injection without localization/token safety; no rename rewriting historical archive records destructively; no player-chosen name used as persistence key; no name-based file paths; no wall-clock/GUID/unseeded RNG; no identity-owned market pricing; no identity-owned refugee spawning; no identity-owned faction dialogue runtime; no emblem image generator required for MVP.

---

# 0. Mission

ASHFALL's shelter is currently mechanically real but narratively generic.

The source baseline reports that searches for:

```text
ShelterName
shelter_identity
ShelterCustom
```

return no existing identity system, while journal, feedback, and UI refer to the community generically as:

```text
"the shelter"
```

That creates a continuity problem.

The player may spend:
- weeks feeding survivors;
- building rooms;
- governing;
- trading;
- surviving raids;
- making faction enemies;
- burying the dead;
- documenting history;

yet the place itself lacks a stable identity.

The target architecture is:

```text
ONBOARDING / CAMPAIGN START
        │
        ├── origin selection
        └── shelter naming
              │
              ▼
ShelterIdentity Authority
              │
              ├── stable identity ID
              ├── current display name
              ├── name history
              ├── origin ID
              ├── founding day
              ├── founder reference if real
              ├── motto optional
              └── emblem selection optional
              │
              ├────────► localization token resolver
              ├────────► journal / feedback
              ├────────► UI headers
              ├────────► archive / history
              ├────────► governance rename action
              └────────► dialogue read model

EXISTING WORLD HISTORY / SOCIAL AUTHORITIES
              │
              ├── faction standing
              ├── NPC memories
              ├── rumors/intelligence
              ├── moral history
              ├── governance history
              ├── trade history
              ├── combat history
              ├── medical aid history
              └── isolation/contact history
              │
              ▼
CommunityIdentityProjection
              │
              ├── known-for tags
              ├── public descriptors
              ├── faction-specific known facts
              └── archive/epilogue identity summary
```

The system should answer:

> What is this community called, where did it come from, how has its identity changed, and what has the world come to know it for?

It should not answer:

> What is faction X's standing score?
> What price should faction X charge?
> How many refugees arrive?
> How infamous is the player according to a second global meter?

Those remain other systems.

---

# 1. Source-Evidence Interpretation

## 1.1 Name/origin are genuinely missing

This is the cleanest part of the plan.

The source reports no existing shelter-name/identity matches and generic shelter text throughout the UI/journal.

Therefore a small persistent identity authority is justified.

## 1.2 Per-faction reputation duplicates existing faction state

The source proposes:

```text
reputationByFaction
```

But ASHFALL already has:
- faction standing/trust;
- combat political consequences;
- trade stance;
- diplomacy;
- rumor awareness.

A second per-faction shelter-reputation score would create conflicting truths.

Default decision:
- **do not add it**.

## 1.3 Per-settlement reputation also needs an authority audit

If settlements already have:
- trust;
- standing;
- memory;
- influence;
then reuse.

If they do not, Plan 166 should not invent a broad settlement diplomacy system merely to support shelter naming.

## 1.4 Known-for tags are useful if derived

Tags such as:
- traders;
- raiders;
- healers;
- hermits

are valuable as **summaries of existing history**.

They should be derived from:
- real trade events;
- combat/raid history;
- medical aid;
- isolation/contact patterns.

Do not mutate a tag counter manually from scattered call sites.

## 1.5 Infamy is probably already represented elsewhere

The source proposes:
- cruel/betraying actions;
- faction hostility;
- refugee fear;
- ending impact.

Those overlap:
- MoralChoice;
- faction standing;
- Plan 139;
- Plan 155 heat/public criminality;
- Plan 147 memories;
- rumor;
- epilogue history.

Default:
- reject new global `infamy` state.

Use a derived descriptor if needed.

## 1.6 Origin effects must be real, not decorative multipliers

Example:

```text
Government Bunker:
+radiation shielding
+air filtration
-space
-surface access
```

Each line must map to a real existing subsystem.

If no surface-access model exists:
- do not fake it as arbitrary stat.

## 1.7 Rename history must preserve the past

If the shelter was named:
- “Ash Home”
then renamed:
- “The Haven”,

historical records should be able to say:
- name at time of event;
- current name;
- formerly known as.

Do not rewrite every old event to the new name unless presentation explicitly chooses current-name rendering.

---

# 2. Non-Negotiable Shelter Identity Invariants

## INV-166.1 — One stable shelter identity

The community has a stable internal identity independent of display name.

## INV-166.2 — Display name is not an ID

Rename does not change:
- save keys;
- references;
- history IDs.

## INV-166.3 — Origin is immutable after campaign start by default

Origin records what the shelter **was**.

Rename/governance does not alter it.

## INV-166.4 — Name history is append-only

Past names remain historically queryable.

## INV-166.5 — No second faction reputation

Faction standing/trust remains canonical.

## INV-166.6 — No second settlement reputation without explicit ADR

## INV-166.7 — Known-for is derived from real history

No scattered direct tag mutation.

## INV-166.8 — Public knowledge is information-gated

A faction/NPC may refer to the shelter by name only if:
- they know the community;
- identity information has propagated through an existing channel where relevant.

## INV-166.9 — Origin effects use existing authorities

No identity-owned radiation, defense, inventory, room, ventilation, morale, or education state.

## INV-166.10 — Starting effects apply exactly once

Origin starting supplies/layout cannot reapply on load.

## INV-166.11 — Name propagation is tokenized

No mass hand-edits that bake current name into authored strings.

## INV-166.12 — Custom user text is presentation-safe

Name/motto:
- length-limited;
- escaped;
- safe in save, journal, localization, UI.

## INV-166.13 — Rename is governance/history action, not save migration

## INV-166.14 — Rename cost uses real authority

If rename has:
- governance approval;
- morale/social consequence;
it goes through those systems.

## INV-166.15 — No forced rename penalty without system evidence

The source's “morale penalty” is a candidate, not an invariant.

## INV-166.16 — Archive stores identity provenance

Plan 162 should be able to reference:
- current identity;
- historical name;
- origin.

## INV-166.17 — Identity does not own dialogue

It supplies text tokens/read model.

## INV-166.18 — Old saves remain playable without forced blocking prompt unless onboarding design explicitly supports deferred completion

## INV-166.19 — Identity calculation requires no RNG

Naming, origin, token resolution, and known-for derivation are deterministic.

## INV-166.20 — Identity exists headlessly

No UI dependency.

---

# 3. Definition of Done

Plan 166 closes only when:

- repository confirms no existing canonical shelter identity authority;
- faction/settlement/reputation overlap is audited;
- one stable shelter identity ID exists;
- shelter name is persisted and validated;
- origin selection is persisted;
- name history persists across rename;
- motto/emblem are optional and cannot block campaign start;
- origin definitions are data-driven;
- every origin effect maps to a live existing system or is removed/deferred;
- origin starting effects are exactly-once;
- origin layout effects route through canonical shelter/layout initialization;
- name appears through reusable token/localization substitution in journal, feedback, UI and dialogue where supported;
- no mass fragile string replacement is required;
- archive can resolve shelter name/origin at event/current time;
- governance can expose rename only if Plan 159 governance route exists;
- rename history remains stable;
- known-for tags are derived from canonical history/events;
- no second faction reputation score exists;
- no new global infamy meter exists without approved ADR;
- faction-specific behavior continues to use faction standing/trust;
- trade-price effects continue to use market/faction authorities;
- refugee effects continue to use visitor/admission authority;
- old saves receive safe default identity;
- old saves do not crash or lose access to UI/journal;
- capture/restore is idempotent;
- headless tests pass;
- all origin IDs/effect adapter IDs/asset IDs/localization keys validate;
- `--shelter-identity-selftest` exists or equivalent;
- generated identity/origin matrices are drift-gated;
- UI/accessibility/text-scale checks pass;
- exported/headless build renders name tokens consistently.

---

# 4. Phase P0 — Identity & Reputation Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
current shelter-root/state object
shelter save section
onboarding journey
campaign start/bootstrap
journal template system
feedback template system
localization/token resolver
UI header surfaces
FactionBranchCoordinator
FactionStanceEngine
settlement relationship state
Plan 131 rumor/intelligence
Plan 139 combat political history
Plan 147 NPC memory
Plan 155 heat/underground history
Plan 159 governance
Plan 162 archive status
MarketSystem
visitor/refugee system
room/layout authority
radiation/filtration/ventilation authority
defense authority
inventory bootstrap
```

## P0.2 Build shelter identity authority matrix

Create:

`docs/shelter/SHELTER_IDENTITY_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
identity owns?
projection/adaptor?
status
```

Rows:
- shelter internal ID;
- current display name;
- name history;
- origin;
- founder;
- motto;
- emblem;
- faction standing;
- settlement standing;
- public reputation;
- known-for;
- infamy;
- trade pricing;
- refugee flow;
- archive history;
- governance rename;
- journal naming;
- dialogue naming;
- room layout;
- radiation shielding;
- ventilation;
- defense;
- starting inventory.

## P0.3 Reputation ADR

Create:

`docs/architecture/ADR_SHELTER_IDENTITY_VS_REPUTATION.md`

Answer:

```text
What does shelter identity own?
What does faction standing own?
Does settlement standing exist?
What public-history facts exist?
Can known-for be derived?
Is any independent global reputation fact actually missing?
```

Default result:

```text
NO new reputationByFaction
NO new reputationBySettlement
NO new infamy
```

## P0.4 Audit text-token architecture

Find whether content supports:
- named placeholders;
- localization interpolation;
- runtime format args.

Do not introduce ad-hoc `string.Replace("the shelter", name)` everywhere.

## P0.5 Baseline token inventory

Search all player-facing text for:
- "the shelter";
- "your shelter";
- generic shelter labels.

Classify:
- should remain generic;
- should use current shelter name;
- should use historical name;
- should use “the shelter” grammatically.

## P0.6 Baseline old-save behavior

Capture current:
- onboarding;
- save load;
- journal header;
- feedback;
- faction screen.

---

# TASK 166A — Persistent Identity, Naming & Name History

# 166A.0 Goal

Create one small identity authority that can persist the shelter's stable identity and player-facing name.

## 166A.1 Proposed file

If no current owner:

`Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`

## 166A.2 Identity state

Recommended:

```text
schema_version
shelter_id
current_name
origin_id
founding_day
founder_survivor_id optional
motto optional
emblem_id optional
emblem_variant optional
name_history[]
identity_completed
```

No faction reputation state.

## 166A.3 Stable `shelter_id`

Use one canonical campaign shelter identity.

Example:
- fixed `player_shelter` if exactly one player shelter exists;
or
- deterministic campaign entity ID.

Do not derive from name.

## 166A.4 Current name

UTF-safe string.

## 166A.5 Validation

Define:
- min/max grapheme length;
- trim rules;
- control-character rejection;
- newline policy;
- markup escaping;
- whitespace normalization.

## 166A.6 Do not restrict to ASCII

Support localized player names.

## 166A.7 Reserved names

Only block if:
- UI/system collision;
- profanity policy exists.

Do not invent a giant censor list in this plan.

## 166A.8 Default name

Old saves:
- localized `"Shelter"` display default;
or project-specific neutral label.

## 166A.9 No-name state

Internally:
- name may be defaulted immediately
rather than nullable.

This simplifies token resolution.

## 166A.10 Founding day

Use campaign day.

If old save:
- campaign start day/default.

## 166A.11 Founder survivor

Only populate if onboarding has a canonical founder.

Do not invent one.

## 166A.12 Motto

Optional player text.

## 166A.13 Motto validation

Shorter than name/description.

No effect on gameplay by default.

## 166A.14 Emblem

Use catalog enum/ID.

Do not save raw image bytes.

## 166A.15 Emblem color

Use predefined palette ID where possible.

Avoid arbitrary user-generated shader/material state.

## 166A.16 Name history entry

Suggested:

```text
name
effective_day
source_decision_id
previous_name
reason_tag
```

## 166A.17 Initial naming

Creates first history entry.

## 166A.18 Rename

Appends new entry.

## 166A.19 No destructive rewrite

Archive/event history may choose current or historical display, but original name-at-event remains recoverable.

## 166A.20 Rename idempotence

Same governance/action ID:
- one rename.

## 166A.21 Rename to same name

Reject/no-op.

## 166A.22 Rename cooldown

Only if needed to prevent spam/confusion.

Prefer governance procedure/history cost over arbitrary timer.

## 166A.23 Rename cost

Audit Plan 159.

Possible:
- governance decision;
- relationship/identity event.

Do **not** hardcode morale penalty unless there is a semantic reason.

## 166A.24 Identity-completed flag

Use only if onboarding can be incomplete.

## 166A.25 Headless naming

Tests can set name through API.

## 166A.26 CaptureState

Versioned.

## 166A.27 RestoreState

No events emitted during restore.

## 166A.28 Old-save migration

Default fields safely.

## 166A.29 Reference stability

Changing name never invalidates:
- quests;
- saves;
- archives;
- faction references.

## 166A.30 API

Suggested:

```text
GetIdentity()
SetInitialName(...)
Rename(...)
SetMotto(...)
SetEmblem(...)
```

## 166A.31 Read model immutable

No collection mutation leak.

## 166A.32 Semantic events

Only:
- shelter_named;
- shelter_renamed;
- shelter_identity_customized.

No event on every read.

## 166A.33 Generated identity contract

Create:

`docs/shelter/SHELTER_IDENTITY_CONTRACT.md`

### 166A DoD

The shelter has a stable internal identity, safe player-facing name, immutable origin reference, and append-only rename history with no reputation duplication.

---

# TASK 166B — Origin Selection & Exactly-Once Starting Effects

# 166B.0 Goal

Give campaign origin meaningful starting-state differences only through systems that already own those differences.

## 166B.1 Origin catalog

Create:

`Assets/StreamingAssets/Data/shelter_origins.json`

Versioned.

## 166B.2 Origin DTO

Recommended:

```text
id
display_key
description_key
flavor_key
effect_refs[]
layout_profile_id optional
starting_bundle_id optional
tags[]
```

Avoid free-form generic `startingBonuses` maps.

## 166B.3 Typed effects

Origin effects must reference known adapter types.

Examples:

```text
shelter_layout_profile
starting_inventory_bundle
radiation_shielding_profile
ventilation_profile
surface_access_profile
defense_profile
community_space_profile
```

Only if real consumers exist.

## 166B.4 Six source origins

Audit each:

1. Government Bunker
2. Mining Facility
3. School Basement
4. Private Vault
5. Improvised Cellar
6. Military Outpost

Each must pass a real-effect review.

## 166B.5 Government Bunker

Candidate:
- radiation shielding;
- air filtration;
- cramped space;
- surface access.

Map every one to a real system.

If surface access has no authority:
- omit/defer that drawback.

## 166B.6 Mining Facility

Candidate:
- mining access;
- underground space;
- ventilation;
- water access.

Do not invent mining-access stat if no mining topology exists.

## 166B.7 School Basement

Candidate:
- community space;
- defense drawback;
- education bonus.

If education system absent:
- remove/defer education bonus.

## 166B.8 Private Vault

Candidate:
- starting supplies;
- luxury items;
- small space;
- social/morale drawback.

Do not create generic permanent morale penalty if space/social systems already model the cause.

## 166B.9 Improvised Cellar

Candidate:
- surface access;
- poor shielding;
- fewer starting supplies.

Use real state.

## 166B.10 Military Outpost

Candidate:
- defense;
- weapon cache;
- limited medical/community resources.

Use canonical inventory/defense.

## 166B.11 Origin power budget

Each origin should have:
- strengths;
- weaknesses;
- roughly comparable overall starting value.

## 166B.12 No objectively dominant origin

Simulate.

## 166B.13 Starting item bundle

Use canonical bootstrap inventory transaction.

## 166B.14 Exactly once

Track bootstrap application ID outside repeated load path.

## 166B.15 Layout profile

Use shelter-layout/room initialization authority.

## 166B.16 No post-start origin reapplication

Origin is historical identity.

## 166B.17 Origin cannot be renamed

Name may change; origin stays.

## 166B.18 Origin discovery vs choice

Source says choose during onboarding.

If fiction instead supports “discover/remember origin”:
- separate flavor reveal from mechanical origin.

Do not let later discovery change starting effects.

## 166B.19 Origin flavor

Journal/archive presentation.

## 166B.20 Origin tag use

Quests/dialogue may query origin ID/tag.

## 166B.21 No origin-owned quest state

## 166B.22 Old saves

Use:
- `legacy_shelter` / `unspecified`
origin
unless migration can infer a real origin safely.

Do not guess.

## 166B.23 Origin integrity

Validate:
- effect adapters;
- bundle IDs;
- layout IDs;
- localization;
- assets.

## 166B.24 Origin tests

Per origin:
- chosen;
- effect set applied once;
- save/load;
- no duplicate bundle;
- no missing adapter.

## 166B.25 Generated origin matrix

Create:

`docs/shelter/SHELTER_ORIGIN_MATRIX.md`

### 166B DoD

Every shipped origin has real, exactly-once starting consequences backed by existing shelter/inventory/environment systems, and unsupported flavor bonuses are removed rather than simulated generically.

---

# TASK 166C — Naming Propagation, Localization & Historical Rendering

# 166C.0 Goal

Make the shelter name appear consistently without scattering fragile string replacement logic.

## 166C.1 Introduce canonical identity token

Example:

```text
{shelter_name}
```

Use current localization/token conventions.

## 166C.2 Additional tokens

Only if needed:

```text
{shelter_origin}
{shelter_motto}
{shelter_former_name}
```

## 166C.3 Token resolver

Central service reads:
- ShelterIdentity read model.

## 166C.4 No `string.Replace("the shelter", ...)`

Avoid grammar bugs and accidental replacements.

## 166C.5 Journal templates

Audit which entries should use:
- name;
- generic phrase;
- historical name.

## 166C.6 Feedback messages

Parameterized tokens.

## 166C.7 UI headers

Display current name.

## 166C.8 Faction dialogue

Use name only if dialogue context supports identity knowledge.

## 166C.9 NPC dialogue

Same.

## 166C.10 Archive

Store/resolve:
- name-at-event;
- current name;
- origin.

## 166C.11 Plan 162 integration

Preferred archive event metadata:

```text
shelter_identity_id
shelter_name_at_event
```

Current UI can additionally render:
- “now The Haven”.

## 166C.12 No mass historical rewrite on rename

## 166C.13 Epilogue

Use final/current name;
optionally mention prior names.

## 166C.14 Save slot UI

If save-list UI supports:
- show shelter name.

Do not use it as filename.

## 166C.15 Window/title/header

Only player-facing safe contexts.

## 166C.16 Notifications

Tokenized.

## 166C.17 Tooltips

No excessive repetition.

## 166C.18 Grammatical support

Some languages require case/declension.

Do not assume English phrase insertion works universally.

For Latvian/other localization:
- templates should be authored around raw name token;
- avoid automated grammatical inflection unless localization framework supports it.

## 166C.19 Escaping

Player text cannot inject:
- BBCode;
- RichText tags;
- formatting markup
unless explicitly escaped.

## 166C.20 Name length overflow tests

Test:
- short;
- max length;
- Unicode;
- wide glyphs.

## 166C.21 Motto line-wrap

Bounded.

## 166C.22 Token missing fallback

If identity unavailable:
- localized `"the shelter"`/`"Shelter"` fallback.

## 166C.23 Headless token resolution

Pure.

## 166C.24 Generated usage audit

Create:

`docs/shelter/SHELTER_NAME_PROPAGATION_AUDIT.md`

Rows:
- surface;
- template;
- token;
- historical/current;
- test.

### 166C DoD

The shelter name appears consistently through a single safe tokenization path and rename does not corrupt history or localization.

---

# TASK 166D — Governance Rename, Archive, Onboarding & Campaign Integration

# 166D.0 Goal

Place identity creation/change at the correct lifecycle points.

## 166D.1 Onboarding order

Recommended:

```text
campaign setup
→ origin selection
→ origin preview
→ initial name
→ optional emblem/motto
→ confirm
→ apply origin exactly once
→ enter campaign
```

Adjust to current onboarding.

## 166D.2 Origin preview

Show real consequences before confirm.

## 166D.3 No fake bonus text

Preview generated from effect adapters/data.

## 166D.4 Confirmation transaction

Initial identity + origin application atomic where possible.

## 166D.5 Cancel/back

No duplicated origin effects.

## 166D.6 Resume interrupted onboarding

State machine-safe.

## 166D.7 Existing saves

Do not force full onboarding.

Use:
- default identity;
- optional rename/customization prompt later
if desired.

## 166D.8 Rename via governance

If Plan 159 is live:
- expose rename as governance action/decision.

## 166D.9 Informal/leader-rule rename

Mode procedure applies.

## 166D.10 Council/assembly rename

Only if governance design says community name requires approval.

Do not hardcode universal vote.

## 166D.11 Rename consequence

Record:
- governance decision;
- identity event;
- archive event.

## 166D.12 Morale impact

Default:
- none.

If naming has community attachment support:
- use relationships/morale event explicitly.

## 166D.13 Rename spam protection

Use:
- governance procedure;
- optional cooldown;
- history,
not arbitrary huge resource tax.

## 166D.14 Archive first entry

“The Naming”/founding record if archive live.

## 166D.15 Plan 162 absent

Publish adapter interface and defer.

Do not create second archive.

## 166D.16 Founding event

Record:
- origin;
- name;
- day.

## 166D.17 Emblem/motto event

One semantic update event.

## 166D.18 Quest hooks

Identity setup should not require quest runtime if onboarding can do it directly.

## 166D.19 “The Name” quest

Default:
- unnecessary.

Use onboarding milestone/event unless quest architecture specifically needs it.

## 166D.20 “The Origin” quest

Likewise.

## 166D.21 Later identity quests

Can query:
- known-for tags;
- archive milestones;
- governance state.

Quest runtime owns them.

## 166D.22 Save-slot label update

On rename:
- display metadata may refresh;
- save identity stays stable.

### 166D DoD

Shelter identity is established once during onboarding, can be changed through the existing governance lifecycle, and remains historically consistent through archive and save metadata.

---

# TASK 166E — Derived Community Reputation & Known-For Projection

# 166E.0 Goal

Make the shelter develop a recognizable public identity without creating duplicate reputation truth.

## 166E.1 Reject source `ShelterReputation` DTO as written by default

Do not store:

```text
reputationByFaction
reputationBySettlement
infamy
```

unless the P0 ADR proves a missing source of truth.

## 166E.2 Community reputation read model

Suggested:

```text
known_for_tags[]
public_descriptors[]
faction_view_summaries[]
settlement_view_summaries[] if authority exists
history_milestones[]
```

Derived.

## 166E.3 Known-for rules

Data-driven:

`community_identity_rules.json`

## 166E.4 Rule fields

Suggested:

```text
tag
source_event_kinds[]
window_days optional
threshold
minimum_distinct_sources
visibility_requirement
retention_policy
priority
```

## 166E.5 Source events

Reuse canonical event/history records.

Examples:

### Traders
- completed meaningful trade volume/events.

### Raiders
- offensive raid/combat history.

### Healers
- medical aid/treatment/rescue events.

### Hermits
- sustained low external-contact history.

## 166E.6 No direct scattered increment calls

Do not:
- `traderScore += 1`
from every trade call site.

## 166E.7 Threshold derivation

Compute from event history/aggregates.

If retention compaction removes raw events:
- maintain canonical aggregate counters in history/telemetry authority, not identity if possible.

## 166E.8 Tag activation

Deterministic.

## 166E.9 Tag loss

Some tags:
- persistent milestone;
others:
- recent-behavior descriptor.

Data says which.

## 166E.10 Conflicting tags

Possible:
- traders + raiders.

Do not force one personality archetype.

## 166E.11 Tag cap

UI can show top N; state may have more derived truths.

## 166E.12 Tag priority

By:
- salience;
- recency;
- strength.

## 166E.13 Faction-specific reputation

Use existing faction standing/trust summary.

Example projection:

```text
Garrison: Hostile
Free Traders: Trusted
```

Identity UI can display it without owning it.

## 166E.14 Settlement view

Only if settlement authority exists.

## 166E.15 Public knowledge

A known-for tag may have:
- internal historical truth;
- public known state.

Use Plan 131 if information asymmetry matters.

## 166E.16 Faction dialogue

Can condition on:
- known-for tag known to faction;
- actual faction standing.

## 166E.17 No automatic trade discount from tag

Trade price still controlled by:
- faction stance;
- market;
- dealer/NPC relationship.

## 166E.18 No automatic refugee multiplier from tag

Visitor/refugee system may consume an explicit identity descriptor if its design supports.

Otherwise defer.

## 166E.19 No global infamy meter

If UI wants “notorious”:
- derive from:
  - public negative rumors;
  - hostile faction count;
  - severe moral/combat history;
according to explicit read-model rules.

## 166E.20 Moral history vs public reputation

A cruel act unknown to anyone:
- may affect morality;
- should not automatically affect public reputation.

## 166E.21 Underground heat vs notoriety

Plan 155 heat:
- enforcement scrutiny.

Do not treat it as shelter infamy.

## 166E.22 NPC personal memory

Plan 147:
- personal.

Does not become community known-for unless shared.

## 166E.23 Combat faction consequence

Plan 139:
- political standing.

Can be source evidence for faction view, not duplicated.

## 166E.24 Reputation event

Emit only when a derived tag crosses active/inactive threshold:
- community_identity_tag_gained;
- community_identity_tag_lost.

## 166E.25 No daily tag spam

## 166E.26 Archive

Record meaningful new identity tag.

## 166E.27 Epilogue

Use final:
- name;
- origin;
- strongest known-for tags;
- governance;
- major history.

## 166E.28 Determinism

Same history:
- same tags.

## 166E.29 No RNG

Reputation derivation should not require `ISeededRng`.

## 166E.30 Generated reputation rules matrix

Create:

`docs/shelter/COMMUNITY_IDENTITY_RULE_MATRIX.md`

### 166E DoD

The shelter can become known as a trading hub, raider stronghold, healing refuge, or isolated community through real historical behavior without introducing a duplicate reputation or infamy system.

---

# TASK 166F — UI, Persistence, Accessibility, Validation & CI

# 166F.0 Goal

Make shelter identity visible and robust across new/old campaigns, long names, renames, origin choices, history, and exports.

## 166F.1 UI surface decision

Likely:
- shelter header;
- identity/detail section;
- onboarding.

Do not create a large standalone panel if current shelter overview can host it.

## 166F.2 Identity summary

Show:

```text
Name
Origin
Motto optional
Emblem
Founded
Known for
Faction relations summary
```

## 166F.3 Reputation wording

Label:
- “Known for”
- “How others see us”

Do not imply a hidden numeric reputation if none exists.

## 166F.4 Origin tooltip

Generated from actual effects.

## 166F.5 Name-history view

Optional archive/history subsection.

## 166F.6 Rename UI

Shows:
- current;
- new;
- governance procedure/cost;
- historical effect.

## 166F.7 Emblem selector

MVP:
- curated emblem IDs;
- curated colors.

No procedural image generation required.

## 166F.8 Motto

Optional.

## 166F.9 Accessibility

- keyboard name entry;
- screen-reader label;
- no color-only emblem meaning;
- text scaling;
- high-contrast emblem fallback.

## 166F.10 Unicode tests

Test:
- Latin;
- Latvian diacritics;
- Cyrillic;
- CJK if font supports;
- combining characters;
- emoji policy.

## 166F.11 Save-state size

Tiny.

## 166F.12 Name history retention

Bounded naturally by rename rate.

No compaction required unless abuse possible.

## 166F.13 Old-save matrix

Cases:

1. no identity section;
2. legacy default name;
3. named new save;
4. origin-selected save;
5. renamed save;
6. emblem/motto;
7. unknown removed emblem ID;
8. origin schema older version.

## 166F.14 Missing emblem

Fallback.

## 166F.15 Missing origin definition

Keep raw ID/history and display:
- unknown/legacy origin
without crash.

## 166F.16 Restore idempotence

No:
- origin bundle reapply;
- naming event replay;
- rename event replay;
- known-for gain replay.

## 166F.17 Data integrity

Validate:
- origin IDs;
- effect adapters;
- layouts;
- bundles;
- emblems;
- localization;
- identity rules.

## 166F.18 Selftest

Create:

```text
--shelter-identity-selftest
```

## 166F.19 Selftest cases

At least:
1. old save default;
2. select origin;
3. origin exact-once effects;
4. initial naming;
5. Unicode name;
6. invalid control chars;
7. rename;
8. historical name;
9. tokenized journal;
10. tokenized feedback;
11. faction/dialogue token;
12. archive name-at-event;
13. known-for projection;
14. no reputation duplication;
15. headless.

## 166F.20 Source-scan authority gate

Detect:
- `reputationByFaction` duplicate storage;
- `reputationBySettlement` duplicate storage;
- global `infamy` state;
- hardcoded market discount;
- direct refugee multiplier;
- direct origin-owned room/defense/radiation state.

## 166F.21 Content acceptance

Origin definitions:
- EFFECT_PRODUCED.

Known-for rules:
- derived/selectable.

## 166F.22 Reachability

Each origin:
- selectable in onboarding;
- applies effects.

Each emblem:
- selectable if shipped.

Each known-for tag:
- attainable or deliberately rare.

## 166F.23 Snapshot tests

- default shelter;
- max-length name;
- renamed shelter;
- six origins;
- multiple known-for tags;
- narrow resolution;
- enlarged text.

## 166F.24 Headless

Identity fully usable.

## 166F.25 Exported-build parity

Token resolution and fonts work in export.

## 166F.26 Generated docs

Create:
- `SHELTER_IDENTITY_ARCHITECTURE.md`;
- `SHELTER_IDENTITY_AUTHORITY_MATRIX.md`;
- `SHELTER_ORIGIN_MATRIX.md`;
- `SHELTER_NAME_PROPAGATION_AUDIT.md`;
- `COMMUNITY_IDENTITY_RULE_MATRIX.md`;
- `ADR_SHELTER_IDENTITY_VS_REPUTATION.md`.

### 166F DoD

Shelter identity is safe across onboarding, save/load, rename, localization, archive history, UI scaling, headless execution, and exported builds.

---

# TASK 166G — Optional Advanced Identity Features

# 166G.0 Goal

Keep presentation-heavy follow-ons from destabilizing the core identity contract.

## 166G.1 Procedural emblem generator

Default:
- DEFER.

Curated emblems are sufficient.

## 166G.2 Custom uploaded emblem

Out of scope unless file/import safety system explicitly supports user content.

## 166G.3 Identity competitions

Follow-on only if other named settlements/shelters have comparable public identity.

## 166G.4 New Game+ identity

Separate persistence/design layer.

## 166G.5 Dynamic motto generation

Not necessary.

## 166G.6 Famous shelter legacy

Epilogue/archive can derive from:
- identity;
- history;
- known-for;
- governance.

## 166G.7 External map label

If world map displays player shelter:
- use current name token.

## 166G.8 Radio callsign

Potential separate field only if radio system needs a short identifier.

Do not overload shelter name.

## 166G.9 Banner/flag world asset

Only if world/rendering supports.

### 166G DoD

Advanced identity presentation remains additive and does not complicate the underlying save/reference model.

---

# 5. Identity State Model

```text
LEGACY / UNNAMED
      │
      ▼
ORIGIN SELECTED
      │
      ▼
INITIAL NAME SET
      │
      ├── motto/emblem optional
      │
      ▼
ACTIVE IDENTITY
      │
      ├── known-for evolves from history
      ├── archive records milestones
      └── governance may rename
              │
              ▼
        RENAMED IDENTITY
              │
              └── history preserved
```

---

# 6. Identity vs Reputation Contract

Identity owns:

```text
name
origin
motto
emblem
name history
founding metadata
```

Existing systems own:

```text
faction standing
settlement standing
NPC personal trust
morality
heat
trade price
refugee flow
rumor
```

Community reputation is primarily a **projection**.

---

# 7. Stable ID Contract

Never use:

```text
shelter_name
```

as:
- save key;
- map key;
- quest target ID;
- archive foreign key.

Use stable `shelter_id`.

---

# 8. Name Validation Contract

Validation handles:

```text
grapheme length
leading/trailing whitespace
control characters
markup escaping
empty result
```

No lossy ASCII-only sanitization.

---

# 9. Name History Contract

Every rename produces:

```text
old name
new name
day
source decision
```

History is append-only.

---

# 10. Historical Rendering Contract

Archive/event can render:

```text
Name at event
Current name
```

depending on context.

Do not globally rewrite past.

---

# 11. Origin Contract

Origin is:

```text
historical campaign starting condition
```

not:
- a permanent generic bonus bag detached from current state.

If origin gave:
- extra supplies,
those supplies are consumed normally.

If it gave:
- a room profile,
the current shelter later evolves normally.

---

# 12. Origin Effect Adapter Contract

Each origin effect:

```text
effect_ref
→ typed adapter
→ canonical system
→ exactly-once bootstrap
```

No arbitrary key/value modifier maps.

---

# 13. Community Identity Tag Contract

Known-for tags are **derived summaries**.

Example:

```text
trade history
→ trader tag

medical aid history
→ healer tag
```

No direct tag mutation from UI.

---

# 14. Public Knowledge Contract

Internal historical truth:

```text
we have raided three convoys
```

does not automatically mean:

```text
every faction knows us as raiders
```

Plan 131/information system determines external knowledge.

---

# 15. Faction Dialogue Contract

Dialogue may use:

```text
shelter name
faction standing
known public identity tag
```

Memory/identity does not own final dialogue selection.

---

# 16. Trade Contract

Identity can display:
- “Known as traders”.

It cannot directly set:
- 10% discount.

Faction/market systems own prices.

---

# 17. Refugee Contract

Identity can expose descriptor/tag.

Visitor/refugee authority may use it only through an explicit adapter.

No direct spawn-rate multiplier inside identity.

---

# 18. Governance Contract

Rename may be a governance action.

Governance decides:
- authorization/procedure.

Identity executes:
- the rename record.

---

# 19. Archive Contract

Archive receives:
- stable shelter ID;
- name-at-event;
- origin ID;
- current identity read.

No duplicate historical store.

---

# 20. Journal Contract

Journal templates request:
- current/historical shelter token.

Journal owns the entry.

---

# 21. Feedback Contract

Feedback templates use runtime identity token.

No hand-coded name concatenation in each producer.

---

# 22. Persistence Matrix

| Fact | Owner |
|---|---|
| stable shelter ID | identity |
| current name | identity |
| name history | identity |
| origin | identity |
| motto/emblem | identity |
| faction standing | faction |
| NPC personal trust | NPC memory |
| public rumor | rumor/intel |
| moral band | moral-choice |
| underground heat | black market |
| trade prices | market |
| refugee flow | visitor system |
| archive events | archive |
| governance decisions | governance |
| known-for tags | derived identity projection / historical aggregates |

---

# 23. Old-Save Migration

Safe default:

```text
shelter_id = player_shelter
current_name = localized "Shelter"
origin_id = legacy_unspecified
name_history = []
motto = null
emblem = default
```

No origin effects retroactively applied.

---

# 24. Exactly-Once Origin Bootstrap

Use stable source:

```text
origin_bootstrap:<campaign_id>:<origin_id>
```

or existing bootstrap transaction ID.

Reload must not:
- respawn supplies;
- recreate rooms;
- reapply shielding.

---

# 25. Failure Injection Matrix

## N166.1 Rename changes shelter persistence key
Expected: identity stability gate fails.

## N166.2 Origin supplies reappear after reload
Expected: exactly-once gate fails.

## N166.3 Identity stores `reputationByFaction`
Expected: authority source-scan fails.

## N166.4 Identity stores global `infamy`
Expected: ADR/authority gate fails.

## N166.5 Journal uses naive `string.Replace("the shelter", name)`
Expected: propagation/static gate fails.

## N166.6 Name contains RichText markup and alters UI
Expected: escaping test fails.

## N166.7 Max-length Unicode name clips onboarding
Expected: snapshot/accessibility gate fails.

## N166.8 Rename rewrites historical archive name
Expected: history test fails.

## N166.9 School origin grants education bonus with no education consumer
Expected: origin-effect integrity fails.

## N166.10 Military origin sets defense stat directly in identity
Expected: authority gate fails.

## N166.11 Public “raiders” tag appears despite no information spread
Expected: information-gating test fails.

## N166.12 Old save receives retroactive origin weapon cache
Expected: migration test fails.

---

# 26. Determinism Contract

Identity itself is deterministic.

Same:

```text
campaign state
+ chosen origin
+ player text
+ history
+ information state
```

must yield same:
- identity;
- name history;
- origin effects;
- known-for projection;
- external descriptor visibility.

No RNG needed.

---

# 27. Long-Horizon Metrics

Track:

```text
renames
origin distribution
known-for tags active
tag gain/loss
public-known tags by faction
identity history size
token failures
fallback-token usage
origin application duplicates
```

---

# 28. Balance Guardrails

Origin choices should create:

```text
different starting texture
different constraints
different early priorities
```

not:
- permanent winner/loser builds.

Known-for should create:
- narrative recognition;
- selective system reactions through existing authorities.

Not a hidden extra stat layer.

---

# 29. Origin Balance Matrix

For each origin compare:

```text
starting inventory value
room/capacity value
environmental safety
defense
access/logistics
early resource burden
```

Normalize approximately.

---

# 30. Rename Guardrails

Rename should be:
- possible;
- historically meaningful;
- not exploitable.

Do not:
- reset faction state;
- reset known-for;
- reset heat;
- reset NPC memory.

The world remembers the same community.

---

# 31. Identity Continuity Across Rename

All external references use:
- shelter ID.

Display name changes.

Rumor/faction/NPC systems may optionally remember old name as alias if they model that.

Do not fork identity.

---

# 32. UI Acceptance

## Onboarding
- six origins or reduced honest set;
- real effect preview;
- name entry;
- optional motto/emblem.

## Shelter header
- current name.

## Identity detail
- origin;
- founded;
- known-for;
- emblem/motto.

## Faction screen
- canonical faction relationship;
- no duplicate identity reputation score.

## Archive
- name history.

---

# 33. Accessibility

- keyboard text entry;
- visible focus;
- text-scale safe;
- no emblem-color-only meaning;
- Unicode safe;
- screen-reader labels.

---

# 34. Localization

Names are player text.

Surrounding grammar remains localized.

Origin names/descriptions:
- localization keys.

Known-for tags:
- localization keys.

---

# 35. Content Acceptance

Origins:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Known-for rules:
- loaded;
- queried;
- selected/visible when real history meets criteria.

---

# 36. Reachability

For every origin:
- onboarding can select;
- effects apply once.

For every known-for tag:
- source history can occur;
- projection can activate;
- at least one display/consumer uses it.

---

# 37. Performance Guardrails

Identity:
- constant-time reads.

Known-for:
- event-driven aggregates or indexed history;
- no full campaign-log scan every frame.

Token resolution:
- cheap;
- no repeated disk/catalog loads.

---

# 38. CI / Gate Set

Recommended:

```text
shelter_identity_authority_single
shelter_name_validation
shelter_name_history
shelter_origin_integrity
shelter_origin_exactly_once
shelter_origin_authority
shelter_identity_no_duplicate_reputation
shelter_identity_token_propagation
shelter_identity_archive_history
shelter_identity_known_for_derivation
shelter_identity_information_gate
shelter_identity_old_save
shelter_identity_ui_access
```

---

# 39. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --shelter-identity-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 40. Recommended Commit Breakdown

```text
166A-1 identity/reputation authority audit
166A-2 reputation ADR
166A-3 shelter identity state / stable ID
166A-4 name validation + Unicode safety
166A-5 name history / rename idempotence
166A-6 motto/emblem minimal state
166A-7 old-save migration
166A-8 identity tests/docs

166B-1 origin catalog/schema
166B-2 origin effect adapter registry
166B-3 Government Bunker / Cellar effects
166B-4 Mining / School effects
166B-5 Vault / Military effects
166B-6 bootstrap transaction
166B-7 origin balance tests
166B-8 generated origin matrix

166C-1 shelter-name token resolver
166C-2 journal propagation
166C-3 feedback propagation
166C-4 UI headers/save labels
166C-5 dialogue knowledge gating
166C-6 historical rendering/archive adapter
166C-7 localization/escaping
166C-8 propagation audit/tests

166D-1 onboarding origin/name flow
166D-2 atomic confirmation/resume
166D-3 governance rename adapter
166D-4 archive founding/rename events
166D-5 quest-hook disposition
166D-6 existing-save optional customization
166D-7 save-slot display
166D-8 lifecycle tests/docs

166E-1 community identity rules schema
166E-2 history-event adapters/aggregates
166E-3 traders/healers tags
166E-4 raiders/hermits tags
166E-5 public knowledge/Plan-131 integration
166E-6 faction-view projection
166E-7 archive/epilogue descriptors
166E-8 anti-duplication tests

166F-1 identity UI
166F-2 onboarding/name snapshots
166F-3 Unicode/max-length/accessibility
166F-4 old-save matrix
166F-5 deterministic/headless selftest
166F-6 source-scan authority gates
166F-7 content acceptance/reachability
166F-8 final ship/no-ship report

166G-1 advanced emblem/legacy disposition
```

---

# 41. Risk Register

## R166.1 “Reputation” duplicates faction standing

Mitigation:
- explicit ADR;
- derived read model only.

## R166.2 Origins bypass real systems with magic bonuses

Mitigation:
- typed effect adapters;
- no consumer = no effect.

## R166.3 Naming propagation causes localization bugs

Mitigation:
- tokenized templates;
- no raw replacement.

## R166.4 Rename corrupts history

Mitigation:
- stable shelter ID;
- append-only name history;
- name-at-event.

## R166.5 Origin starting bundles duplicate on load

Mitigation:
- exactly-once bootstrap ID.

## R166.6 Known-for tags become arbitrary grind meters

Mitigation:
- derive from actual event history.

## R166.7 Custom text breaks UI/markup

Mitigation:
- grapheme limits;
- escaping;
- snapshots.

## R166.8 Old saves are forced into new onboarding

Mitigation:
- default identity;
- optional later customization.

---

# 42. Acceptance Checklist

## P0

- [ ] no existing shelter identity owner confirmed
- [ ] shelter state/save owner audited
- [ ] onboarding audited
- [ ] journal templates audited
- [ ] feedback templates audited
- [ ] localization/token resolver audited
- [ ] UI headers audited
- [ ] faction standing/trust audited
- [ ] settlement relationship state audited
- [ ] Plan 131 rumor audited
- [ ] Plan 139 combat history audited
- [ ] Plan 147 NPC memory audited
- [ ] Plan 155 heat/history audited
- [ ] Plan 159 governance audited
- [ ] Plan 162 archive status audited
- [ ] MarketSystem audited
- [ ] visitor/refugee authority audited
- [ ] layout/room authority audited
- [ ] shielding/ventilation audited
- [ ] defense audited
- [ ] inventory bootstrap audited
- [ ] identity authority matrix
- [ ] identity-vs-reputation ADR
- [ ] generic shelter text inventory

## 166A

- [ ] stable shelter ID
- [ ] name not used as ID
- [ ] versioned identity state
- [ ] current name
- [ ] Unicode validation
- [ ] grapheme length
- [ ] markup/control safety
- [ ] default name
- [ ] founding day
- [ ] founder only if canonical
- [ ] motto optional
- [ ] emblem ID only
- [ ] curated color/palette
- [ ] name-history entries
- [ ] initial naming recorded
- [ ] rename append-only
- [ ] no destructive history
- [ ] rename idempotence
- [ ] same-name no-op
- [ ] rename procedure/cost grounded
- [ ] identity-completed semantics
- [ ] headless naming
- [ ] CaptureState
- [ ] RestoreState
- [ ] old-save migration
- [ ] reference stability
- [ ] semantic events bounded
- [ ] identity contract docs

## 166B

- [ ] versioned origin catalog
- [ ] typed effect refs
- [ ] no generic modifier maps
- [ ] six origins audited
- [ ] Government Bunker effects real
- [ ] Mining Facility effects real
- [ ] School Basement effects real
- [ ] Private Vault effects real
- [ ] Improvised Cellar effects real
- [ ] Military Outpost effects real
- [ ] unsupported bonuses removed/deferred
- [ ] origin value budget
- [ ] no dominant origin
- [ ] starting inventory canonical
- [ ] bootstrap exactly once
- [ ] layout authority reused
- [ ] origin immutable
- [ ] flavor vs mechanical origin separated
- [ ] quest predicates only
- [ ] legacy unspecified origin
- [ ] integrity
- [ ] per-origin tests
- [ ] generated matrix

## 166C

- [ ] canonical shelter-name token
- [ ] origin/motto tokens only if needed
- [ ] centralized resolver
- [ ] no naive string replacement
- [ ] journal tokenized
- [ ] feedback tokenized
- [ ] UI headers
- [ ] faction dialogue knowledge-gated
- [ ] NPC dialogue knowledge-gated
- [ ] archive name-at-event
- [ ] Plan-162 adapter
- [ ] no historical rewrite
- [ ] epilogue current/final name
- [ ] save-slot label
- [ ] notifications
- [ ] grammar/localization considered
- [ ] player markup escaped
- [ ] max-length/wide glyph tests
- [ ] motto wrapping
- [ ] fallback token
- [ ] headless resolver
- [ ] propagation audit generated

## 166D

- [ ] onboarding order integrated
- [ ] real origin effect preview
- [ ] no fake bonus text
- [ ] atomic confirm
- [ ] back/cancel no duplicate effects
- [ ] interrupted onboarding resume
- [ ] old save not forced through onboarding
- [ ] governance rename adapter
- [ ] governance mode procedure respected
- [ ] rename consequence history
- [ ] morale penalty not hardcoded by default
- [ ] rename spam protection grounded
- [ ] archive founding record
- [ ] archive adapter defers if Plan 162 absent
- [ ] founding event
- [ ] customization event
- [ ] onboarding not needlessly implemented as quest
- [ ] future quests use canonical quest runtime
- [ ] save-slot display updates

## 166E

- [ ] no source-style ShelterReputation duplicate DTO
- [ ] derived community reputation read model
- [ ] known-for rules in data
- [ ] real source events
- [ ] traders tag derived
- [ ] raiders tag derived
- [ ] healers tag derived
- [ ] hermits tag derived
- [ ] no scattered direct increments
- [ ] deterministic thresholds
- [ ] retention classes
- [ ] conflicting tags allowed
- [ ] UI top-N priority
- [ ] faction view reads standing authority
- [ ] settlement view only if real
- [ ] public knowledge gated
- [ ] faction dialogue uses known tag + standing
- [ ] no tag-owned price discount
- [ ] no tag-owned refugee multiplier
- [ ] no global infamy meter
- [ ] morality/public reputation separated
- [ ] Plan-155 heat separated
- [ ] Plan-147 personal memory separated
- [ ] Plan-139 political standing separated
- [ ] tag-gained/lost semantic events
- [ ] no daily event spam
- [ ] archive identity milestones
- [ ] epilogue projection
- [ ] deterministic/no RNG
- [ ] generated rule matrix

## 166F

- [ ] identity surface reused/new panel justified
- [ ] identity summary
- [ ] reputation wording accurate
- [ ] origin tooltip generated
- [ ] name-history view
- [ ] rename UI
- [ ] curated emblem selector
- [ ] motto optional
- [ ] accessibility
- [ ] Unicode test set
- [ ] small save state
- [ ] old-save matrix
- [ ] missing emblem fallback
- [ ] missing origin fallback
- [ ] restore idempotence
- [ ] data integrity
- [ ] shelter-identity selftest
- [ ] source-scan anti-duplication gate
- [ ] content acceptance
- [ ] reachability
- [ ] snapshots
- [ ] headless
- [ ] exported-build parity
- [ ] generated docs

## 166G

- [ ] procedural emblem generator deferred
- [ ] custom-upload emblem out of core scope
- [ ] identity competitions follow-on
- [ ] New Game+ identity separate
- [ ] dynamic motto generation unnecessary
- [ ] famous-shelter legacy derived
- [ ] map label integration if supported
- [ ] radio callsign separate if needed
- [ ] world banner only if rendering supports

---

# 43. Ship / No-Ship Gate

**SHIP** only if:

```text
shelter_identity_authorities == 1
AND shelter_display_name_used_as_persistence_id == false
AND duplicate_faction_reputation_state == 0
AND duplicate_settlement_reputation_state_without_adr == 0
AND global_infamy_meter_added == false
AND origin_effects_without_real_consumer == 0
AND origin_starting_effect_reapplications == 0
AND identity_owned_market_price_state == false
AND identity_owned_refugee_flow_state == false
AND identity_owned_faction_dialogue_state == false
AND naive_shelter_string_replacement_paths == 0
AND unsafe_player_markup_injection == 0
AND rename_destroys_historical_name == false
AND rename_resets_world_relationship_state == false
AND old_save_forced_onboarding_breakage == false
AND shelter_identity_old_save == pass
AND shelter_identity_save_roundtrip == pass
AND shelter_origin_exactly_once == pass
AND shelter_identity_token_propagation == pass
AND shelter_identity_known_for_derivation == pass
AND shelter_identity_information_gate == pass
AND shelter_identity_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND exported_build_identity_smoke == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 44. Implementer Handoff

1. Confirm there is still no canonical shelter identity owner.
2. Create the smallest stable identity state first: shelter ID, name, origin, name history.
3. Never use player name as an internal key.
4. Reject the source's duplicate per-faction/per-settlement reputation tables unless the ADR proves they are genuinely missing concepts.
5. Reject global infamy by default; derive public descriptors from existing political, moral, rumor, combat, and underground histories.
6. Audit all six origin effects against real system seams before authoring values.
7. Remove/defer any origin bonus with no real consumer.
8. Apply starting inventory/layout/environment effects through canonical bootstrap systems exactly once.
9. Tokenize shelter-name rendering centrally; do not mass-replace “the shelter.”
10. Preserve historical names across rename.
11. Keep origin immutable and historical.
12. Make motto/emblem optional presentation state.
13. Integrate rename through Plan 159 governance where appropriate, but do not invent a universal morale penalty.
14. Let Plan 162 archive own archive history; pass shelter identity metadata to it.
15. Derive `Traders`, `Raiders`, `Healers`, `Hermits`, and future known-for tags from canonical event history.
16. Gate external knowledge of identity/reputation through Plan 131 where information asymmetry matters.
17. Keep trade prices, refugee flow, faction standing, NPC memory, morality, and heat in their existing systems.
18. Give old saves a safe default identity without retroactive origin bonuses.
19. Test Unicode, max-length names, markup escaping, text scaling, and exported-build fonts.
20. Close only when the shelter's name and origin are visible throughout the campaign without creating a second reputation simulation.

---

# 45. Final Outcome

When this plan is complete, ASHFALL's shelter stops being an anonymous container.

At campaign start, the player establishes where the community came from and what it is called. The origin has real consequences only where real systems support them: a bunker can begin with stronger shielding because the shelter-environment authority says so; a military site can begin with equipment because canonical bootstrap inventory delivers it; a cramped vault can have less usable space because the actual room/layout authority represents that constraint.

The name then follows the community everywhere it should.

Journal entries, feedback, shelter headers, archive records, faction dialogue, save labels, and epilogue text can all resolve the same shelter identity through a central token path. The player can rename the community later without changing its internal identity or erasing history. Old records can still know what the shelter was called when an event occurred.

The community can also develop a reputation without adding another reputation system.

If the player becomes heavily trade-oriented, the shelter can become known as a trading hub because the campaign's real trade history supports that description. If it repeatedly raids others, “raiders” can emerge from combat history. If it treats outsiders, “healers” can come from medical-aid records. If it withdraws from the world, “hermits” can emerge from sustained contact history.

Those descriptors do not secretly replace faction standing, NPC memory, morality, black-market heat, market prices, or refugee logic. They summarize the shelter's real history and can be consumed by those systems only through explicit adapters.

Most importantly, the community keeps its identity through change.

Governments can change.
Leaders can die.
The shelter can be renamed.
Its public reputation can shift.

But its founding origin and historical continuity remain intact.

The result is a shelter that is no longer simply “the shelter.”

It is a named place with a provenance, a history, and a reputation earned by what the player actually did.
