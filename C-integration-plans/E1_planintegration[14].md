---
PLAN_ID: E1-14
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 14
STATUS: READY_FOR_EXECUTION_WHEN_PLAN_34_AND_PLAN_149_CONTRACTS_PASS
SOURCE_PLAN: "Plan 175 — Meta Profile & New Game+ Orchestration"
SEQUENCE_FILENAME: "E1_planintegration[14].md"
PREVIOUS_FILENAME: "E1_planintegration[13].md"
NEXT_FILENAMES:
  - "E1_planintegration[15].md"
  - "E1_planintegration[16].md"
CATEGORY: LINK+META_PROFILE+REWARDS+NEW_GAME_PLUS+SETTINGS_BOUNDARY
PRIMARY_INTENT: "Create exactly one product-level cross-campaign reward/profile authority that imports finalized Plan 34 completion records and Plan 149 completed-achievement ids once, persists currency/prestige/unlocks outside campaign slots, and emits explicit new-campaign bootstrap selections without duplicating achievement or difficulty logic."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_PROFILE_STORE: true
ONE_REWARD_OWNER: true
SECOND_ACHIEVEMENT_ENGINE_FORBIDDEN: true
SECOND_DIFFICULTY_ENGINE_FORBIDDEN: true
LIVE_PROFILE_DEPENDENCY_AFTER_BOOTSTRAP_FORBIDDEN: true
CAMPAIGN_SLOT_META_STATE_FORBIDDEN: true
RANDOM_META_REWARD_ROLLS_FORBIDDEN: true
RUNTIME_RISK: MEDIUM_HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: VERY_HIGH
MIGRATION_RISK: HIGH
---

# E1 Plan Integration [14] — Meta Profile, Verified Completion Import, Achievement Rewards, Prestige, Currency, and New Game+ Bootstrap

> **Sequence rule:** this file is `E1_planintegration[14].md`.
> The next files are `E1_planintegration[15].md`, `E1_planintegration[16].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 175 into an implementation-grade cross-campaign meta-profile programme.

The source plan is unusually disciplined about ownership and that discipline should be preserved:

- Plan 34 owns difficulty presets and creation of `CampaignCompletionRecord`.
- Plan 149 owns achievement definitions, evaluation, and run-local achievement state.
- E1-14 consumes finalized completion facts and completed achievement IDs only after the campaign/epilogue
  boundary is stable.
- E1-14 owns the product-level profile store, import receipts, meta currency, prestige, profile unlocks,
  reward-purchase state, and the selection of unlocked New Game+ bootstrap options.
- New campaigns still begin as fresh campaign envelopes.
- Difficulty comes from an existing Plan 34 preset, not a meta-profile multiplier engine.
- Achievement logic is never reevaluated by the profile.

The highest integration risk is cross-campaign ownership drift. ASHFALL already has or is planning a
generational/legacy record layer. A meta profile must not become a second campaign-history simulator, and the
legacy archive must not become a second reward/currency owner.

Therefore E1-14 defines a hard separation:

**campaign history explains what happened; meta profile owns account/product-level unlocks and rewards.**

Where E1-5 legacy history exists, E1-14 should reference its campaign IDs or summary references rather than
copying full lineage, faction, memorial, survivor, site, or epilogue records.

## 1. Source Intent Preserved

Plan 175 asks for:

- one `MetaProgressionSystem`;
- one versioned/checksummed profile store;
- campaign summaries;
- completed achievement IDs;
- unlocked profile items;
- meta currency;
- prestige;
- source campaign IDs for idempotent imports;
- `meta_unlockables.json`;
- pure deterministic currency/prestige calculations;
- import after epilogue settles;
- exact-once reward/unlock resolution;
- New Game+ setup using existing Plan 34 difficulty presets;
- explicit bootstrap inputs;
- challenge rules owned by Plan 34;
- profile/store isolation from campaign slots;
- duplicate-import, corruption, migration, purchase, input-application, and headless tests.

E1-14 retains all of those requirements and adds the lifecycle, transaction, schema, migration, exploit,
bootstrap-isolation, legacy-boundary, and balance rules needed to make them safe.

## 2. Core Architecture Thesis

```text
Active campaign
      |
      v
Plan 34 finalizes CampaignCompletionRecord
      |
      +--> Plan 149 has finalized completed achievement IDs
      |
      v
Epilogue/ending settles
      |
      v
Meta import transaction
      |
      +--> validate source campaign ID
      +--> reject duplicate import
      +--> resolve reward definitions
      +--> calculate deterministic currency/prestige
      +--> add unlocks
      +--> record import receipt
      |
      v
Versioned MetaProfile store
      |
      +--> campaign import summaries/receipts
      +--> completed achievement IDs
      +--> unlocked profile items/options
      +--> currency
      +--> prestige
      +--> purchase ledger
      |
      v
New Game+ setup UI
      |
      +--> select Plan 34 difficulty preset
      +--> select unlocked start options
      |
      v
Pure NewCampaignBootstrapInput
      |
      v
Fresh campaign envelope
```

After campaign initialization, active campaign simulation must not query or mutate the meta profile.

## 3. Non-Negotiable Rules

- Plan 34 remains the sole owner of difficulty preset definitions and `CampaignCompletionRecord` creation.
- Plan 149 remains the sole owner of achievement definitions/evaluation/run-local completion.
- E1-14 does not evaluate achievement conditions.
- E1-14 does not derive campaign completion from save state.
- E1-14 imports only finalized facts.
- Every source campaign can be imported at most once.
- Meta currency/prestige calculations are deterministic pure functions.
- No random reward rolls.
- Meta currency is owned only by MetaProfile.
- Prestige is owned only by MetaProfile.
- Profile unlocks are owned only by MetaProfile.
- Campaign slots never contain authoritative meta currency/prestige/unlock data.
- New campaigns do not hold a live mutable reference to MetaProfile.
- New Game+ options are copied into explicit bootstrap input and then validated by campaign owners.
- Difficulty modifiers are Plan 34 preset behavior, not profile behavior.
- Ironman/scarcity/challenge tags remain Plan 34 concerns.
- Profile rewards cannot directly mutate an existing campaign.
- Reward definitions reference source facts; they do not duplicate achievement/completion definitions.
- E1-5 legacy/history remains separate from reward ownership.
- Full historical campaign records should not be copied into MetaProfile if a canonical legacy archive already owns them.
- Clean Start must remain available.
- New Game+ must be opt-in.
- Profile corruption must never corrupt campaign slots.
- Campaign corruption must not silently rewrite profile rewards.
- Profile writes are atomic/checksummed according to existing save-service conventions.
- All migration steps are versioned and idempotent.
- All purchases are transactional and idempotent.
- Reward application is auditable by provenance.
- Deleting a campaign save after successful import must not revoke profile rewards unless product design explicitly defines revocation.
- Re-importing a copied/rolled-back campaign with the same campaign ID must not grant rewards twice.
- Cloned campaign IDs must be detected through stable completion/import provenance where possible.

## 4. Relationship to E1-5 Legacy / Generational Continuation

E1-5 and E1-14 are complementary but must not collapse into one ambiguous store.

### E1-5 owns

- immutable campaign history;
- lineage;
- memorials;
- historical faction memory;
- unresolved obligations;
- remembered sites;
- narrative continuation seeds;
- campaign-history presentation.

### E1-14 owns

- verified campaign import receipt;
- completed achievement IDs imported from Plan 149;
- meta currency;
- prestige;
- profile unlocks;
- profile purchases;
- New Game+ option entitlements/selections;
- product-level reward state.

### Shared key

Both may reference the same stable `campaignId`.

E1-14 should store the minimum campaign summary required for reward audit/UI. If E1-5 already stores the
canonical narrative campaign summary, E1-14 stores a reference or compact receipt rather than duplicating the
full record.

## 5. Acceptance Slices

### Slice A — Profile store and exact-once import
One completion record + achievement set imports once and persists safely.

### Slice B — Deterministic rewards
Currency, prestige, and automatic unlocks resolve from data.

### Slice C — Reward purchases
Optional spendable unlocks with transactional ledger.

### Slice D — New Game+ bootstrap
Select an existing difficulty preset plus unlocked profile options and create a fresh campaign.

### Slice E — Presentation
Profile/history/reward UI, source provenance, migration diagnostics.

Do not start with a large prestige tree, dozens of currencies, or a separate meta difficulty system.


---

## E1-14A — Premise verification and cross-campaign authority audit

**Goal:** Verify Plan 34 completion, Plan 149 achievements, save-service patterns, legacy/archive state, campaign IDs, settings/profile paths, and bootstrap contracts before creating the profile.

### Required substeps

1. Inspect Plan 34 implementation and locate the canonical `CampaignCompletionRecord` definition, finalization event, difficulty preset catalog, challenge tags, and campaign-start difficulty input.
2. Inspect Plan 149 achievement definitions, evaluator, run-local state, completed-achievement export/finalization, and achievement ID catalog.
3. Inspect save services, checksum/versioning helpers, atomic-write patterns, profile/settings paths, campaign-slot paths, backup/recovery conventions, and corruption handling.
4. Inspect campaign ID creation and stability across save/load/copy.
5. Inspect epilogue/ending finalization ordering.
6. Inspect E1-5/legacy campaign history implementation if present.
7. Inspect current campaign bootstrap/new-game setup flow.
8. Search for existing meta progression, prestige, currency, New Game+, profile unlock, account/profile, or permanent reward state.
9. Map every Plan 175 field to one owner.
10. Create `docs/meta/META_PROFILE_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence.
12. Set `PREMISE_VERIFIED_AT` to current HEAD and block implementation if Plan 34/149 final facts are not stable.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14B — Meta-profile ownership ADR

**Goal:** Define exactly one profile/reward authority and its boundary with campaign history, achievements, difficulty, settings, and active campaign state.

### Required substeps

1. Write an ADR comparing `MetaProgressionSystem`, a generic PlayerProfile service, and extension of an existing profile/settings service.
2. Define profile-owned fields: profile schema/version, imported campaign receipts, imported completed achievement IDs, currency, prestige, unlocked reward IDs, purchase ledger, entitlement metadata, and optional cosmetic preferences if they belong there.
3. Explicitly exclude achievement definitions/evaluation, difficulty rules/modifiers, active campaign state, lineage, survivor history, inventory, standing, campaign resources, and live quest state.
4. Define E1-5 legacy archive boundary.
5. Define user settings versus profile progression boundary.
6. Define New Game+ bootstrap as a value object copied from profile selections.
7. Define post-bootstrap severance: campaign runs without live profile dependency.
8. Define feature flags for purchases and New Game+ independently.
9. Require second-tool review.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14C — Profile store path and isolation contract

**Goal:** Place the profile outside campaign slots and guarantee isolation in both directions.

### Required substeps

1. Identify the existing product-level/profile save root.
2. Create exactly one profile file/store according to current save-service pattern.
3. Do not place authoritative profile state inside any campaign slot.
4. Do not use per-campaign copies as writeable fallback profile state.
5. Define profile path naming and ownership.
6. Define read/write permissions and lifecycle.
7. Define behavior when no profile exists.
8. Define campaign-load behavior when profile is missing/corrupt.
9. Add tests proving campaign save bytes are unchanged by profile-only reward purchases.
10. Add tests proving profile save bytes are unchanged by ordinary in-campaign saves.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14D — Versioned checksummed profile schema

**Goal:** Define a minimal durable profile DTO with explicit versioning and corruption detection.

### Required substeps

1. Define profile ID/version, creation/update metadata where existing save conventions support it, import receipts, achievement ID set, unlock ID set, currency balance, lifetime earned/spent counters if useful, prestige, purchase records, and optional compact campaign summaries.
2. Use deterministic stable ordering for serialized collections where project conventions need it.
3. Define checksum coverage.
4. Define atomic write/replace behavior.
5. Define backup/recovery behavior from existing save service.
6. Define max reasonable sizes/retention.
7. Do not persist derived values that can be recomputed unless historical audit requires them.
8. Add serialization round-trip tests.
9. Add checksum mismatch/corruption tests.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14E — Campaign import receipt contract

**Goal:** Represent exact-once consumption of finalized campaign facts.

### Required substeps

1. Define `campaignId`, completion record version/hash/reference, imported timestamp or sequence, difficulty preset ID, ending/completion facts needed for reward audit, imported achievement IDs or achievement-set fingerprint, calculated currency delta, prestige delta, unlock IDs granted, and import transaction ID.
2. Keep the receipt compact.
3. Reference E1-5 legacy record ID if available instead of copying full historical details.
4. Define uniqueness on source campaign ID.
5. Define how a legitimate regenerated completion record with the same campaign ID is handled.
6. Define whether import hash mismatch after prior import is warning-only or corruption evidence.
7. Add tests for one import, duplicate identical import, duplicate conflicting import, and missing optional legacy reference.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14F — Completion handoff ordering

**Goal:** Import only after Plan 34 and Plan 149 facts are final and the epilogue boundary has settled.

### Required substeps

1. Identify exact end-of-campaign ordering.
2. Ensure Plan 34 `CampaignCompletionRecord` is immutable/finalized before import.
3. Ensure Plan 149 completed achievement IDs are finalized before import.
4. Ensure epilogue/ending processing that may grant final achievements runs before import if architecture requires it.
5. Do not scan the campaign afterward to 'find missed achievements'.
6. Create one explicit `ImportCampaignCompletion(...)` command/transaction.
7. Define crash-safe handoff.
8. Add tests for import too early, successful post-epilogue import, crash before profile write, crash after profile write, and repeated end-screen entry.
9. Emit clear audit diagnostics.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14G — Exact-once import transaction

**Goal:** Make campaign reward ingestion idempotent across retries, reloads, copied saves, and UI re-entry.

### Required substeps

1. Validate source campaign ID.
2. Check import receipt before calculating/applying rewards.
3. Validate completion record contract/version.
4. Validate achievement IDs originate from Plan 149 catalog.
5. Resolve deterministic reward result.
6. Apply currency/prestige/unlocks inside one profile transaction.
7. Append import receipt.
8. Write profile atomically.
9. Return existing receipt/result on safe duplicate request.
10. Reject or quarantine conflicting duplicate payloads.
11. Add idempotency/property tests.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14H — Achievement ID import boundary

**Goal:** Store completed achievement IDs without ever re-evaluating their conditions.

### Required substeps

1. Consume the finalized Plan 149 completed-ID set.
2. Validate IDs against the Plan 149 catalog.
3. Union newly imported IDs into the profile's completed-achievement set.
4. Do not duplicate achievement title/description/criteria in MetaProfile.
5. Do not query campaign facts to re-check achievement eligibility.
6. Define behavior for retired/removed achievement IDs in old profiles.
7. Preserve historical completed IDs even if an achievement becomes unavailable, subject to catalog migration policy.
8. Add tests for known, unknown, duplicate, retired, and renamed IDs.
9. Ensure achievement UI can still distinguish run-local evaluation from permanent profile completion.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14I — Completion fact import boundary

**Goal:** Consume Plan 34 completion facts as trusted finalized inputs rather than re-deriving difficulty/end-state.

### Required substeps

1. Identify exactly which completion fields reward definitions may reference.
2. Examples may include ending ID, completion status, difficulty preset ID, challenge tags, days survived, or explicitly exported scoring facts.
3. Do not read raw campaign save to infer these values.
4. Validate difficulty preset ID against Plan 34.
5. Do not copy Plan 34 difficulty modifier definitions into profile data.
6. Add tests for valid record, unknown preset, incomplete record, unsupported version, and retired ending.
7. Document supported reward condition facts.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14J — Deterministic currency calculation

**Goal:** Calculate meta currency as a pure auditable function of finalized imported facts.

### Required substeps

1. Define one pure `CalculateMetaCurrency(completion, achievements, rewardCatalogVersion)` function or equivalent.
2. Do not use RNG.
3. Define base completion award and optional achievement/difficulty/challenge contributions only if product design approves them.
4. Use bounded integer/fixed-point arithmetic.
5. Clamp/validate nonnegative and maximum values.
6. Record result in import receipt.
7. Version calculation policy if future balance changes should not retroactively alter old receipts.
8. Add golden tests.
9. Test repeated import returns no additional currency.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14K — Prestige calculation contract

**Goal:** Keep prestige deterministic, interpretable, and distinct from spendable currency.

### Required substeps

1. Define what prestige means: lifetime completion/status score, rank, unlock tier, or cosmetic progression.
2. Prefer lifetime/non-spendable progression unless product design says otherwise.
3. Define a pure calculation/update policy.
4. Do not derive prestige from random events.
5. Define caps/tier thresholds.
6. Define whether balance changes affect only future imports.
7. Record awarded delta/source.
8. Add tests for multiple campaigns, cap/tier boundaries, migration, and duplicate import.
9. Prevent prestige from becoming an undisclosed difficulty multiplier.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14L — Meta unlockable catalog schema

**Goal:** Create `meta_unlockables.json` as reward definitions that reference owner catalogs instead of duplicating them.

### Required substeps

1. Define unlock ID, localization keys, unlock kind, source condition references, currency price if purchasable, prestige requirement if used, bootstrap option target, cosmetic/content target, mutual-exclusion/selection rules, and availability/version metadata.
2. Reference Plan 149 achievement IDs directly.
3. Reference Plan 34 completion fact predicates through a small allowed condition vocabulary.
4. Do not copy achievement criteria.
5. Do not copy difficulty modifiers.
6. Define automatic versus purchasable unlocks.
7. Define reward target owner/catalog ID.
8. Validate all references.
9. Version catalog semantics.
10. Add duplicate/invalid/circular dependency tests.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14M — Unlock-condition resolver

**Goal:** Resolve profile entitlements from finalized imported facts and existing profile state.

### Required substeps

1. Evaluate only meta reward conditions, not achievement criteria.
2. Allow predicates such as completed achievement ID, completed campaign, specific ending/completion fact, prestige threshold, or prior unlock dependency.
3. Keep resolver pure.
4. Define ordering/fixed-point iteration if unlocks can depend on other unlocks.
5. Reject cycles.
6. Return newly granted unlock IDs.
7. Apply once within import/purchase transaction.
8. Add tests for single condition, AND/OR conditions if supported, dependency, duplicate grant, and retired reward.
9. Do not inspect active campaign state.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14N — Automatic reward grant transaction

**Goal:** Grant automatic unlocks exactly once with provenance.

### Required substeps

1. Resolve automatic rewards during import.
2. Write unlock ID into profile set.
3. Record source campaign/achievement/reward rule provenance.
4. Do not create duplicate inventory/items inside a completed campaign.
5. Do not auto-apply New Game+ options to the next campaign.
6. Expose newly unlocked results for post-run UI.
7. Add tests for one reward, multiple rewards, already unlocked, catalog update, and duplicate import.
8. Keep reward ownership entirely profile-side.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14O — Meta currency purchase ledger

**Goal:** If spendable rewards exist, implement purchases as atomic profile transactions.

### Required substeps

1. Define purchase request ID, unlock ID, quoted price/catalog version, currency before/after, purchase sequence, and result.
2. Validate reward is purchasable and not already owned.
3. Validate prestige requirement if applicable.
4. Validate balance.
5. Deduct currency and grant unlock atomically.
6. Persist purchase record/provenance.
7. Make retry idempotent.
8. Add tests for success, insufficient funds, already owned, stale price/catalog, duplicate request, corruption recovery, and migration.
9. Do not create campaign resources from the purchase.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14P — Currency conservation and auditability

**Goal:** Guarantee the profile currency ledger explains every unit earned and spent.

### Required substeps

1. Track lifetime earned and spent if useful for integrity.
2. Assert `balance = opening + earned - spent + explicit_migration_adjustments`.
3. Do not allow negative balance.
4. Define migration adjustments explicitly.
5. Record campaign import source for earnings and purchase ID for spending.
6. Add profile integrity validator.
7. Add fuzz/property tests over random import/purchase sequences.
8. Provide debug audit summary.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14Q — Prestige tier and reward dependency gate

**Goal:** Allow prestige to gate profile content without making it a hidden stat bonus.

### Required substeps

1. Define tiers/ranks only if player-facing value exists.
2. Use prestige requirements in unlockable catalog.
3. Do not multiply campaign stats based on prestige automatically.
4. Do not make high prestige silently reduce difficulty.
5. Show clear requirement and source in UI.
6. Add tests for below threshold, exact threshold, above threshold, migration, and removed tier.
7. Keep prestige progression bounded.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14R — New Game+ entitlement model

**Goal:** Represent what the profile permits the player to choose without copying active campaign state.

### Required substeps

1. Define unlock kinds eligible as start options: cosmetic, starting item package, starting knowledge option, survivor archetype option, starting site/context option, or other explicitly approved bootstrap adapters.
2. Each entitlement references one bootstrap-option ID owned by campaign initialization content.
3. Do not store active campaign values in profile.
4. Do not allow arbitrary free-form stat modifiers.
5. Define mutually exclusive option groups.
6. Define selection count/cost/budget if design needs it.
7. Add tests for locked/owned option, invalid target, mutually exclusive pair, and retired option.
8. Keep high-power options gated by E1-5/NG+ balance policy.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14S — E1-5 mechanical legacy reconciliation

**Goal:** Prevent profile New Game+ rewards from bypassing the earlier records-before-bonuses campaign-boundary architecture.

### Required substeps

1. Review E1-5 mechanical inheritance gate.
2. Classify each profile start option as cosmetic, narrative, knowledge, resource, stat, facility, standing, or other mechanical category.
3. Cosmetic/narrative options may pass with minimal balance risk.
4. Mechanical start advantages require explicit New Game+ design approval and caps.
5. Do not represent inherited survivor/shelter state as profile unlocks.
6. Do not import E1-5 lineage traits into profile reward ownership.
7. Keep Clean Start baseline intact.
8. Add parity/power-budget tests for every enabled mechanical start option.
9. Document why profile reward entitlement differs from legacy historical inheritance.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14T — Plan 34 difficulty preset integration

**Goal:** Select only existing difficulty presets and challenge tags through Plan 34 APIs.

### Required substeps

1. Query Plan 34 catalog for available presets.
2. Display preset name/description/owned tuning values through its read model.
3. Store selected preset ID in New Game+ setup input.
4. Do not copy preset multipliers into MetaProfile.
5. Do not create profile difficulty modifiers.
6. Validate preset still exists at campaign start.
7. Define fallback if a saved setup draft references a retired preset.
8. Add tests for normal, locked-by-product-rule if any, retired, challenge-tagged, and default preset.
9. Keep ironman/scarcity rules in Plan 34.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14U — New Game+ setup value object

**Goal:** Create one explicit immutable bootstrap selection object detached from the live profile.

### Required substeps

1. Define selected difficulty preset ID, selected profile option IDs, optional continuation/history mode ID if E1-5 integration permits, and setup version.
2. Include provenance/source unlock IDs for UI/debug where helpful.
3. Do not include profile currency balance or prestige as live references.
4. Validate selections against a snapshot of profile entitlements before campaign creation.
5. Freeze/copy the validated selection.
6. Pass into campaign initializer.
7. Add serialization only if setup drafts are intentionally persisted outside campaign state.
8. Add tests for validation, copy, mutation isolation, and retired option.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14V — Fresh-campaign bootstrap application

**Goal:** Apply selected profile options exactly once during new-campaign composition through owning authorities.

### Required substeps

1. Create typed bootstrap adapters for each approved option kind.
2. Starting items go through Inventory initialization.
3. Starting knowledge goes through knowledge/research initialization.
4. Cosmetics go through presentation/profile selection.
5. Starting survivor/site options go through their canonical creation authority.
6. Difficulty preset goes through Plan 34.
7. Do not leave MetaProgressionSystem registered as a live campaign dependency.
8. Record applied bootstrap option IDs in the campaign envelope only if needed for audit/replay, not as entitlement truth.
9. Make application idempotent.
10. Add tests for each shipped option type and duplicate bootstrap call.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14W — Post-bootstrap profile severance

**Goal:** Prove active gameplay does not depend on or mutate the profile.

### Required substeps

1. After campaign composition, remove/avoid profile service from simulation dependency graph.
2. Do not query profile currency/prestige during gameplay.
3. Do not unlock profile rewards mid-campaign except through a deferred end-of-run/import path unless product design explicitly creates account-level live achievements—and if so route them separately.
4. Changing/deleting/corrupting profile during an active campaign must not alter campaign state.
5. Add dependency-graph/static tests if possible.
6. Add runtime test loading campaign after profile file is temporarily unavailable.
7. Document bootstrap-only dependency.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14X — Clean Start and New Game+ parity policy

**Goal:** Keep Clean Start fully supported while allowing intentional optional NG+ advantages.

### Required substeps

1. Define Clean Start as no profile mechanical options plus selected Plan 34 baseline preset.
2. Allow cosmetics/profile UI without changing simulation.
3. Define New Game+ as an explicit mode if mechanical start options are enabled.
4. Clearly label mechanical advantages/challenge interactions.
5. Do not quietly apply owned unlocks.
6. Measure starting resource/stat/knowledge/capability delta.
7. Ensure core endings/content remain reachable on Clean Start unless explicitly designed otherwise.
8. Add automated baseline parity tests.
9. Add UI test that player can deselect all optional advantages.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14Y — Challenge preset interaction

**Goal:** Resolve conflicts between profile start options and Plan 34 challenge presets explicitly.

### Required substeps

1. Allow Plan 34 preset/tags to declare prohibited bootstrap option categories if necessary.
2. Example: scarcity challenge may forbid resource packages; ironman may be orthogonal.
3. Plan 34 owns the restriction semantics.
4. Meta setup UI consumes allowed/prohibited result.
5. Do not duplicate challenge multipliers.
6. Show conflict reason before campaign creation.
7. Add tests for compatible, prohibited, partially compatible, and retired challenge tags.
8. Prevent hidden fallback that silently changes the selected preset.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14Z — Profile campaign-summary boundary

**Goal:** Store only the summary needed for reward provenance and profile UX.

### Required substeps

1. Define compact fields such as campaign ID, completion timestamp/day, ending ID, Plan 34 preset ID, import reward summary, achievement count/list reference, and legacy record reference if present.
2. Do not copy full survivor roster, inventory, faction histories, memorials, or site records.
3. Prefer navigation/linkage to E1-5 campaign history if available.
4. Define retention behavior.
5. Add tests for profile with 1, 10, 100 campaign receipts.
6. Keep import receipts permanent enough to preserve idempotency.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AA — Profile history and reward UI

**Goal:** Present campaign imports, currency, prestige, unlocks, and provenance without becoming a second legacy chronicle.

### Required substeps

1. Show current currency, prestige/tier, unlocked reward count, and recent/new unlocks.
2. Show compact completed-campaign receipts.
3. Link to full campaign history/legacy UI where available.
4. Show reward source: achievement, ending/completion fact, purchase, migration.
5. Do not display duplicated achievement criteria.
6. Do not display copied difficulty formulas.
7. Add snapshot tests for empty profile, first completion, multiple campaigns, max prestige, no currency, and corrupt/recovered profile.
8. Keep history lists paginated/bounded.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AB — New Game+ setup UI

**Goal:** Expose profile entitlements and existing difficulty presets with explicit source and tradeoff information.

### Required substeps

1. Reuse new-game setup surface.
2. Show selected Plan 34 preset.
3. Show unlocked/locked profile options.
4. Show source requirement/unlock provenance.
5. Show conflicts from Plan 34 challenge policy.
6. Show whether an option changes starting power or is cosmetic/narrative.
7. Allow Reset to Clean Start.
8. Do not show unavailable internal reward IDs.
9. Add keyboard/gamepad navigation.
10. Add interaction tests for select/deselect/conflict/start.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AC — Reward catalog integrity validation

**Goal:** Validate every meta reward against its source and target catalogs.

### Required substeps

1. Validate referenced Plan 149 achievement IDs.
2. Validate Plan 34 completion fact names/predicate vocabulary.
3. Validate target bootstrap option/cosmetic/catalog IDs.
4. Validate prices/prestige thresholds.
5. Validate dependency graph/cycles.
6. Validate unlock-kind consumer exists.
7. Validate no reward embeds achievement criteria or difficulty multipliers.
8. Add data-integrity-selftest coverage.
9. Fail fast with actionable diagnostics.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AD — Profile corruption rejection and recovery

**Goal:** Handle profile corruption safely without contaminating campaign data or silently inventing rewards.

### Required substeps

1. Use checksum validation.
2. Reject invalid profile before applying mutations.
3. Use existing backup/recovery pattern if available.
4. Do not rebuild rewards by scanning all campaign saves automatically unless an explicit recovery tool is separately designed.
5. Allow campaign play with a missing/corrupt profile if product flow supports Clean Start.
6. Provide clear user/developer error state.
7. Add tests for truncated file, checksum mismatch, invalid version, negative currency, duplicate receipts, and impossible purchase ledger.
8. Keep campaign slots untouched.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AE — Old-profile migration framework

**Goal:** Support schema evolution without replaying old campaigns or duplicating rewards.

### Required substeps

1. Define sequential migrations by schema version.
2. Each migration is deterministic and idempotent.
3. Do not recalculate old import rewards unless migration explicitly records a policy adjustment.
4. Preserve import receipt uniqueness.
5. Preserve purchased/unlocked reward IDs.
6. Map retired/renamed unlock IDs through explicit migration table.
7. Record migration adjustments to currency if ever necessary.
8. Add fixtures for every supported historical version.
9. Validate post-migration checksum.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AF — No-profile / first-run bootstrap

**Goal:** Define clean first-run behavior.

### Required substeps

1. Create default profile lazily or at product startup according to save-service conventions.
2. Initialize zero currency/prestige unless design says otherwise.
3. Initialize no imported campaigns/achievements.
4. Grant only explicitly free/default profile unlocks from catalog policy.
5. Do not require a completed campaign to start a normal game.
6. Add first-run tests.
7. Ensure failure to create profile surfaces cleanly without corrupting campaign initialization.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AG — Duplicate campaign and copied-save defenses

**Goal:** Prevent profile farming through copied saves and repeated end-state imports.

### Required substeps

1. Use stable campaign ID as primary idempotency key.
2. Include completion-record fingerprint/hash/reference for conflict detection.
3. Define policy for a campaign save copied before completion then finished along two branches with the same campaign ID.
4. Prefer rejecting second conflicting completion under the same immutable campaign identity.
5. If branching campaigns are officially supported, require branch/run IDs from the campaign authority rather than inventing them in profile code.
6. Test copied completed save, copied pre-completion save, rollback, cloud conflict if applicable, and manual duplicate import.
7. Do not award based solely on filename/slot ID.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AH — Import concurrency and atomicity

**Goal:** Protect profile integrity if multiple completion/import requests arrive close together.

### Required substeps

1. Audit whether concurrent profile writes are possible on desktop/cloud flows.
2. Use save-service locking/serialization.
3. Perform read-validate-mutate-write atomically.
4. Reject stale write version where supported.
5. Ensure two different campaign imports cannot overwrite each other.
6. Ensure duplicate same-campaign imports converge to one receipt.
7. Add concurrency tests if infrastructure supports them.
8. Document single-process assumptions if locking is intentionally simple.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AI — Cloud/profile synchronization boundary

**Goal:** Integrate with any existing cloud/save sync without inventing merge semantics locally.

### Required substeps

1. Audit cloud sync/provider if present.
2. Identify whether profile store participates.
3. Use provider conflict policy.
4. Do not merge currency/unlocks naïvely by max/sum.
5. If no cloud sync exists, document local-only assumption.
6. Add conflict fixture only if supported.
7. Keep campaign-slot cloud state separate from profile authority.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AJ — Meta economy balance

**Goal:** Ensure currency/prestige/reward prices create long-term goals without forcing grind or power creep.

### Required substeps

1. Estimate currency earned per standard completion and per achievement tier.
2. Estimate cost of purchasable rewards.
3. Measure campaigns required for key unlocks.
4. Separate cosmetic and mechanical reward pricing.
5. Check whether high difficulty is mandatory for progression.
6. Check whether repeated easy completions dominate optimal currency farming.
7. Cap exploitative multipliers.
8. Ensure no negative balance.
9. Simulate 1, 3, 5, 10, and 20 campaign progression.
10. Document target progression curves.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AK — Mechanical New Game+ power budget

**Goal:** Bound starting advantages so NG+ changes openings without invalidating survival systems.

### Required substeps

1. Classify all mechanical profile options by resource/stat/knowledge/facility/survivor/site impact.
2. Define a start-power budget or mutually exclusive loadout policy if multiple options can stack.
3. Measure equivalent resource value.
4. Measure early survival difficulty.
5. Check interaction with Plan 34 presets.
6. Prevent prestige/currency from becoming direct in-campaign stat multipliers.
7. Require explicit approval for permanent stat bonuses.
8. Prefer horizontal options and alternate starts over pure power.
9. Add baseline vs NG+ simulations.
10. Keep all mechanical options removable/disableable without invalidating profile history.

### Meta-profile invariants

- Plan 34 owns completion facts and difficulty rules.
- Plan 149 owns achievement definitions/evaluation.
- Meta profile imports finalized facts once and owns only rewards/profile state.
- Campaign slots contain no authoritative profile currency/prestige/unlock state.
- New Game+ creates explicit bootstrap input and then severs the live profile dependency.
- Reward calculations and purchases are deterministic/idempotent.
- E1-5 historical legacy is not duplicated into the reward profile.
- Clean Start remains available.

### Negative tests

- Meta profile re-evaluates an achievement condition.
- Meta profile defines or applies a second difficulty multiplier.
- The same campaign grants currency twice.
- A campaign save stores authoritative meta currency/prestige.
- A profile purchase mutates an already-running campaign.
- A New Game+ campaign queries profile state during simulation.
- A full E1-5 legacy record is duplicated into the reward profile.
- Profile corruption damages a campaign slot.

### Acceptance evidence

- [ ] Exact-once import tests pass.
- [ ] Achievement/difficulty ownership tests pass.
- [ ] Currency/prestige conservation passes.
- [ ] Profile save/migration/corruption tests pass.
- [ ] NG+ bootstrap isolation passes.
- [ ] Save-slot isolation passes.
- [ ] Balance/performance evidence is captured.

---

## E1-14AL — Reward purchase and refund policy

**Goal:** Define whether purchases are permanent, refundable, or consumable at the profile level.

### Required substeps

1. Default unlock purchases to permanent entitlement unless product design says otherwise.
2. Do not make start-option selection consume the entitlement by default.
3. S

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
