# Feature / Task Plan: Power, Paper and Place I — Paper and Power (permits, forged papers and audits) & The Treaty Table (negotiate treaties, embargoes and tolls)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Open points are in §24 (Expansion backlog) and §25 (Open Mysteries).

> **Subjects covered (2 of the 16 in this batch):**
> 25. **Paper and Power** — permits, forged papers and audits. (Prefix `PP`.)
> 26. **The Treaty Table** — negotiate treaties, embargoes and tolls. (Prefix `TT`.)
>
> **Companions that already exist and are extended, not replaced:** `RegionalTreatySystem`, `DiplomaticSummitSystem`, `FactionDiplomacySystem` (three treaty owners), `FactionEmbargoLedger`, the faction-standing owner, `JusticeSystem`, the `BureaucraticDocument*` lore catalog, `CensusClaimSystem`, the crossing/currents faction data, and the design bibles `docs/expansions/expansion_03_nobodys_charter_plan.md` (Nobody's Charter), `expansion_11_*` (Long Line), `expansion_the_holdfast_plan.md`. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Sister plans (read-only cross-references, all hooks ship dark):** `convoy-wars-and-inside-a-house-2026-09-29.md` (its *Checkpoint* threat rows are the first consumer of this plan's tolls and papers), `iron-road-and-siege-year-2026-09-29.md` (rail certificates; the Siege *Terms Year*), `.ai/plans/long-line-freight-2026-09-29.md` (carrier's bills), `.ai/plans/shelter-governance-2026-09-29.md` if present (VERIFY).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, data, ledger or other plan. Paths are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **8b**, **14b** and **25** carry story texture. Sample lines are content candidates for JSON rows, never code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — The Stamp and the Table

> *"Everybody in the wasteland can be killed, and almost everybody can be cheated. The people who
> last are the ones who learn which of those two the other party has chosen — and carry the right
> piece of paper for it."*

The world after the fall did not lose its **paper**. It lost its *courts*, its *printing*, its *inspectors* — and kept the habit. A clerk with a stamp and a lamp outlasts a warlord with a truck, because the clerk decides who may pass, and the warlord needs to pass. Caravanserais have market law. The Fleet's port has inspectors who are "polite, thorough, armed." Reconstruction offices file. The rail guild certifies. A quarantine post clears or does not.

These two subjects are paired because they are the same thing at two scales. **Paper and Power** is the *individual* scale: the piece of paper in the survivor's pocket, the sheet of stock and the steady hand that makes a false one, the day a clerk comes to look at the books. **The Treaty Table** is the *collective* scale: the long table where two powers agree, in writing, on who may pass, who pays, and who is shut out. A treaty is paper with an army behind it; a permit is a treaty of one.

**The binding tone rule (inherited from the corpus).** Cold, exhausted, human, restrained; specificity over adjectives; the game never tells the player how to feel (Nobody's Charter tone lock). Bureaucracy here is not comedy and not tyranny-as-cartoon. It is *ordinary* — which is why it is frightening. Forgery is not a heist; it is a night with a lamp, stock and ink, and a very steady hand. A toll is not robbery; it is a rate.

**Tone & register.**

- **Paper and Power is written in the voice of the counter.** The stamp coming down. The clerk who does not look up. The line that says *valid until the eleventh*. The small shame of a good forgery and the smaller pride.
- **The Treaty Table is written in the voice of the minutes.** Clauses, in order. What was asked, what was given, what was written down and what was left off the record on purpose.

**What the two share.** *Legibility as leverage.* To be legible to a power (papers in order, terms honoured) is to be safe and *governable*. To be illegible (forged, unlisted, embargoed) is to be free and *hunted*. Neither is free: the game prices both.

**The second layer.** Paper is how a frightened world agrees to be calm. The stamp, the rate, the
clause in order — each is a small machine for converting distrust into procedure, and each works
exactly as well as the hands that keep it. What makes this plan cold rather than comic is its
refusal to exempt anyone: the clerk is ordinary, the forgery is a craft, the toll is a rate, and
the treaty ends on a date nobody rings a bell for. Legibility is leverage, and leverage is
exercised by people who also have papers to get in order. And the eleventh arrives for the clerk
too: the rate is published, and nobody standing in the line is exempt from the line.

---

## 1. Goal & Outcome

### 1.1 Paper and Power (PP)

> *Design intent: at a checkpoint on day 140, the player should hold their breath — because they
> know exactly which of the three papers in their satchel is not what it says.*

- **Goal:** Add a **Papers layer** over the existing faction, caravan and expedition seams: a **Paper Ledger** of documents the shelter, its survivors and its caravans hold (permits, licences, passes, certificates); **Authorities** (data rows tied to existing factions) that issue, recognise and check papers; **Forgery and Amendment** as two human acts with a craft quality; a deterministic **Check** that decides Accepted / Queried / Refused / Detected; and **Audits** that compare what the shelter *declared* against what the existing owners *record*.
- **Outcome (observable):**
  1. A **Papers surface** (an existing panel; chosen at P0) lists every paper held: kind, issuer, holder, scope, expiry, origin (Issued / Bought / Forged / Amended / Found) and **derived status** (Valid, Expiring, Expired, Void, Revoked).
  2. At any **checkpoint or gate** (plan 2 *Checkpoint* rows; shelter gates; rail posts) that asks for a paper, the shelter may **present** one; the result is deterministic for a given (paper, authority, day).
  3. A **forged or amended** paper can be made by a survivor with `paper_stock`, time and skill; its **Craft** quality is derived from real inputs and decides detection odds.
  4. **Detection** has consequences through existing owners only (faction standing, a `FactionEmbargoLedger` request, chronicle) — never a new penalty system.
  5. An **Audit** by an authority samples the shelter's declarations, compares them to owner reads (trade, water, census, salvage, treaty terms) and returns findings from *Clean* to *Struck*; the player can **set the books right** or **tidy** them (falsify a line, at risk).
  6. Save/load round-trips; a legacy save loads as "no papers held, no audit history" and every existing faction, caravan, expedition and treaty behaviour equals today's.
- **Non-Goals (PP):** no minigame; no real-world documents, seals, agencies or countries; no new currency; no change to any faction-standing, trade or expedition arithmetic; no new crime system (a `Forgery` justice route is an *unsigned option*, §22); no new routed panel; no Unity.
- **"Done" (PP):** §19 PP acceptance passes via `bin/run-scoped-tests`; existing treaty, embargo, caravan, justice and census tests unchanged; handoff lists untouched shared paths.

### 1.2 The Treaty Table (TT)

> *Design intent: the player should be able to answer, for any road they use, "who let us on it,
> what did it cost, and what happens to us if that agreement ends?"*

- **Goal:** Fold the three existing treaty owners and the political embargo ledger into **one Treaty Table**: a single **Instrument list** (with provenance), a derived **Position** per faction (what they want, what they will give, how much leverage each side has — *read, never stored*), and three new **clause kinds** expressed as *data mapped onto existing owner effects*: **Toll Schedules**, **Embargo Clauses**, and **Pass Clauses** (which issue papers, the seam to PP). Store only what the household *did*: which schedules exist and which clauses were enacted.
- **Outcome (observable):**
  1. The **Treaty Table surface** lists every instrument in force or on offer — from `RegionalTreatySystem`, `DiplomaticSummitSystem` and `FactionDiplomacySystem` — as one list with a provenance chip, life stage (Fresh, Settled, Strained, Lapsing, Lapsed) and its clauses.
  2. For each faction, a **Position card** shows wants/offers (from authored faction data), trust/standing (existing owners), and a derived **Leverage** band (Weak, Even, Strong).
  3. A **Toll Schedule** exists per post: authority, rate (permille), exemptions by treaty or paper. The convoy plan's Checkpoint rows read the *effective* toll through one read hook; with no schedule the authored data row applies unchanged.
  4. An **Embargo Clause** enacts a `FactionEmbargoLedger.TryAddEmbargo` with source `treaty:<id>:<clause>` and is lifted when the clause ends; the **Embargo Board** shows every active embargo with a source chip (debt, treaty, …).
  5. A **Pass Clause** issues a `safe_conduct` paper on ratification; a treaty that lapses or is broken makes dependent papers **Void** (derived, not stored).
  6. Save/load round-trips; a legacy save loads with no schedules and no clause log, and every treaty, embargo and trade behaviour equals today's.
- **Non-Goals (TT):** no fourth treaty system; no change to any treaty, summit or diplomacy arithmetic; no change to `FactionEmbargoLedger` semantics; no weather-embargo rework (`TradeEmbargoSystem` is weather-driven and out of scope); no real-world nations or wars; no new routed panel; no Unity.
- **"Done" (TT):** §19 TT acceptance passes; existing treaty and embargo tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**A treaty is where papers come from; a paper is how a treaty is shown.** One Pass Clause issues one paper kind; one paper carries a `BasisId` (a treaty id, an authority licence, or none); a paper whose basis is no longer in force is **Void** — computed on read. Toll exemptions are granted by *presenting* a valid paper or being a signatory. There is one Paper Ledger and one Treaty Table state; neither keeps a copy of the other's facts (Rule 5).

---

## 1b. Texture, Mystery & Voice

**Paper: the object matters.**

A permit is not a boolean. It has a stamp that is slightly off-centre, a line of authority, a date and a corner that has been folded so often the fold is the most reliable part. The Papers surface renders each as a *line of description*, never as an icon alone: *"Passage pass, Fleet port. Valid to day 152. One corner soft from folding."* A forged pass reads differently only to the person who made it: *"Passage pass, Fleet port. Valid to day 160. The stamp is yours."*

**The clerk.**

The check never shows a roll. It shows a **clerk**: *"The inspector turned it over, read the back, which had nothing on it, and turned it again."* Accepted, Queried, Refused and Detected each have several such lines. **The player learns the odds only by living them.**

**Audits are ordinary.**

An audit is not a raid. It is a person with a book who asks for yours. The Audit Book shows the *lines*: what you said, what the owner records, the gap. A gap is not a crime until someone reads it. **Tidying** a line is the smallest form of the same act as forging a pass.

**The Treaty Table: minutes.**

The Table never lies about who holds leverage. It shows a **band** (Weak, Even, Strong) and the three reasons in plain words: *"They need your water. You need their road. Neither has anywhere else to go."* Ratification is written as minutes: *"Agreed: cart toll to two in the hundred, signatories exempt. Not agreed: the north gate. Not recorded: why."*

**Tolls are rates, not robbery.**

*"Two in the hundred on cargo, paid at the post, with a receipt."* Whether that is a fair rate or an insult is the player's judgement; the game states only the rate and who set it.

**What the player is never told.**

- **Who signs the forged pass authority's real papers.** Authority rows list an *issuer* role, never a person.
- **Which clause was left off the record.** Every ratification line ends with a *not recorded* clause; it is never revealed.
- **Whether an audit was random.** The seeded sample is never shown as a sample; it reads as a clerk's choice.
- **What the stamp is made of.** The seal tool is a *thing*; its origin is not explained.

**Voice — sample fragments (content candidates for `paper_lines.json`, `table_minutes.json`).**

> "Accepted. The stamp came down without a sound, which is how you know it was a good one." — check, Accepted (PP)

> "The inspector turned it over and read the back, which had nothing on it. Then he read the front again." — check, Queried (PP)

> "You have a good hand. It was still a hand." — Detected, forger's line (PP)

> "The audit sampled four lines of nine. Three were clean. The fourth was yours." — Audit Book (PP)

> "Agreed: two in the hundred, signatories exempt. Not agreed: the north gate. Not recorded: why." — minutes (TT)

> "They need your water. You need their road. Neither has anywhere else to go." — Position card (TT)

> "The treaty ended on the thirtieth. Nobody rang anything. Twelve papers went quiet in twelve satchels." — lapse line (TT)

**Design texture beats.**

- **Paper is described, never iconised (PP).** Every state has a sentence.
- **The clerk, not the roll (PP).** Outcomes are read as a person's behaviour.
- **Tidying is forging's small sibling (PP).** The same craft/scrutiny rule covers both.
- **Minutes, with a silence (TT).** Every ratification has a clause "not recorded".
- **A treaty ends quietly (TT).** Lapse is a fact in twelve places, not a fanfare.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §8b/
§14b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the counter leaves lying around.**

> "Pass, Fleet port. Valid to day 152. One corner soft from folding — the fold is the most reliable part."

> "Seal tool, wrapped in cloth. Its origin is not explained, and the cloth is older than the lamp."

> "Audit Book: four lines sampled of nine. The sample is never shown as a sample."

**What the table leaves lying around.**

> "Minutes, ratification. Last clause: not recorded. It is always the last clause."

> "Toll schedule: two in the hundred, receipted. The receipt is the whole civilisation."

> "Treaty, lapsed on the thirtieth. Twelve papers went quiet in twelve satchels."

**Scenes the player may piece together.**

> "The inspector reads the back, which has nothing on it. The forgery is perfect, and the reading was never about the paper."

> "The leverage band reads Strong and gives its reasons in plain words. The plainness is the threat."

**Held silences (texture, not register rows).**

- Who fills the issuer roles. Authority rows list a *role*, never a person — and every role is filled by somebody who goes home at the end of the day. Texture only.
- What is left off the record. Every ratification has one unrecorded clause (§1b); it is never revealed and no future pass may write it in.

**Fourth pass — valid until the eleventh (texture only; §25 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Cold, not comic: everything here is done by somebody ordinary, at a
counter, under a date. The prose should keep the *valid until* construction wherever it can —
every document in this plan is a machine with a calendar in it — and let the small shames and
smaller prides stay unreceipted. Legibility is leverage, and the most frightening artefact is a
rate that was published in advance so nobody could claim surprise.

**What the counter leaves lying around.**

> "Pass book, third column: issuing *role*, not name. The clerk who does not look up is in the
> second column of a different book."

> "Forgery, good. The small shame and the smaller pride are both on the counter for a moment, and
> neither is receipted."

**What the table leaves lying around.**

> "Treaty, lapsed on the thirtieth. The bell nobody rings is the only ceremony the schedule keeps."

**Held silences (texture, not register rows).**

- Who keeps the seal tool wrapped. Its origin is not explained and the cloth is older than the lamp
  (§1c); the keeping is a habit with no author and must stay that way. Texture only.
- What the unexamined lines of the audit would show. The sample is never shown as a sample (§1c);
  the rest of the page is unexamined on purpose and the plan declines to be the tenth line.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `RegionalTreatySystem` (data `regional_treaties.json`, 5 treaties, e.g. `road_iron_charter`: 30 scrap, +10% economy discount, 15% raid-pressure relief on the coastal route, faction `hydro_barons`, compliance every 30 days) owns Propose / Ratify(scrapCost) / BreakTreaty / IsActive / GetActiveEffects / GetRaidPressureModifier / GetTradeDiscount / GetSupplyPriceRelief; status enum Proposed, Ratified, Active, Violated, Suspended, Expired. | `RegionalTreatySystem.cs` L9–388; `Data/regional_treaties.json` | LIVE |
| E2 | `DiplomaticSummitSystem` (data `diplomatic_treaties.json`, **8 frameworks**: non-aggression compact, aquifer water sharing, demilitarised trade corridor, prisoner repatriation, patrol stand-down, scuttle debt amnesty, quarantine of the low fields, meteor-watch collaboration) owns `TryScheduleSummit`, `AdvanceNegotiation(summitId, offerConcession)`, `TryRatifyTreaty`, hostage **guarantees** (`TryExchangeGuarantee`, `TryReleaseGuarantee`), DMZ (`IsArmedPatrolAllowed`), violations (`ReportArmedPatrol`, `ReportRaidAgainstSignatory`), `TickDay`, save `DiplomaticSummitSave`. Frameworks carry `minimum_signatories`, `required_concessions`, `duration_days` (compact: 30), `stability_rating` (55), `violation_tolerance` (1), `violation_penalty_standing` (−12). | `Diplomacy/DiplomaticSummitSystem.cs`; `Data/diplomatic_treaties.json` | LIVE |
| E3 | `FactionDiplomacySystem` (data `treaty_templates.json`, **6 types**: non-aggression, trade alliance, mutual defence, intelligence sharing, tribute, vassalage; e.g. non-aggression base 60 days, reputation requirement 20) owns per-faction `DiplomaticRelationState` (trust, level), envoys, `ProposeTreaty`, `DispatchMission`, `ViolateTreaty`, `TickDay`. | `Diplomacy/FactionDiplomacySystem.cs`; `Data/treaty_templates.json` | LIVE |
| E4 | Three treaty owners overlap (`treaty_non_aggression_compact`, template `non_aggression`, regional treaties) and **all three are instantiated by the host** (2026-09-29). | `Main.ShelterSocial.cs` L110; `Main.FlagshipInstitutions.cs` L183; `Host/DiplomacyHostSession.cs` L47 | **RESOLVED (§2b)** |
| E5 | `FactionEmbargoLedger` is the canonical **political** embargo authority: `TryAddEmbargo(factionId, scope, startDay, durationDays, sourceId)`, idempotent by `sourceId`, day-derived expiry, `IsEmbargoed(factionId, day)`, `ActiveEmbargoes(day)`, save state. Today the requester is debt consequences. | `FactionEmbargoLedger.cs` | LIVE |
| E6 | `TradeEmbargoSystem` (`trade_embargoes.json`) is **weather-driven** (fallout storm blocks caravans, price multipliers) — a different concept sharing the word "embargo". | `Economy/TradeEmbargoSystem.cs` | LIVE (finding) |
| E7 | `foundry_treaty_consequences.json` holds per-treaty met/violated outcomes (standing delta, market modifiers) for the Foundry faction — a consequence-policy pattern to follow, not a fourth treaty system. | `Data/foundry_treaty_consequences.json` | LIVE |
| E8 | **No permit, licence, pass, forgery or audit concept exists in Core or Data.** Grep finds "toll" only as encounter text (`TravelEncounterHeadlessDemo` "garrison toll"), the Toll faction (`IronRaidersSystem`), and an id prefix (`toll_`) in integrity rules; "permit" only as English. | grep over `Assets/Ashfall.Core` | LIVE (finding) |
| E9 | `BureaucraticDocumentCatalog` holds authored **lore documents** with a `BureaucraticDocumentTruthClass` (HistoricalCanonicalRecord, ContemporaneousAuthoredRecord, TemplateCompatibleRecord, FlavorOnlyArtifact, UnsafeUnresolved) and journal-based discovery; "no field is a simulation delta". It is **not** a permit system. | `Narrative/BureaucraticDocumentCatalog.cs` | LIVE |
| E10 | `JusticeSystem` has `CrimeType` {Theft, Assault, Murder, Hoarding, Sabotage, Desertion} — **no forgery**; evidence, trial and punishment machinery exists. | `Narrative/JusticeSystem.cs` | LIVE |
| E11 | `CensusClaimSystem` owns a survivor census ledger (`UpsertLedger`, `listed` flag) and levy orders (`IssueLevy`, `HonourLevy`, `SubstituteLevy`, `RefuseLevy`) — the Holdfast bible's Office. | `CensusClaimSystem.cs` | LIVE |
| E12 | Faction ids exist (`faction_railway_guild`, `faction_hydro_barons`, `faction_salt_freeholders`, `faction_ordnance_foundry`, `faction_rebuilders`, `faction_central_garrison`, `faction_scavengers`, `faction_black_ops`, and crossing factions `faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`, `faction_the_water_committee`, `faction_the_quarantine_post`, `faction_the_smugglers_court`). | `Data/faction_lore.json`, `Data/crossing_factions.json` | LIVE |
| E13 | An item `paper_stock` exists in `items.json`. The paper-making and printing catalogs are lore prose entries, not gameplay inputs. | `Data/items.json`; `Narrative/Paper*Catalog.cs` | LIVE |
| E14 | The plan-2 *Checkpoint* threat rows (`toll_permille`, `refusal_strength`; black flotilla port, caravanserai, Holdfast) are the first consumer of tolls and papers. | `convoy-wars-and-inside-a-house-2026-09-29.md` §6.8, §7 | LIVE (cross-plan) |
| E15 | Write authority: `FactionWarSystem.ModifyStanding` (id normalised via `FactionStandingIdResolver.ToSystemsId`); `FactionBranchCoordinator.ModifyStanding` for PRPF/military only; host split at `Main.PsyOps.cs` L60–71. | `YearOfAsh/FactionWarSystem.cs` L88; `Factions/FactionBranchCoordinator.cs` L421 | **RESOLVED (§2b)** |
| E16 | No forger skill; `skill_steady_hands` and `skill_crafting` exist in `skills.json`; per-survivor level read to confirm at coding. | `Data/skills.json`; `Survivors/SkillProgressionSystem.cs` L110 | **RESOLVED as composite (§2b)** |
| E17 | Owners for trade shipments, water yield and expedition logs exist; exact public reads for the Observed column are unconfirmed (narrowed at pass 1 to one read per template). | — | **VERIFY (P0), narrowed** |
| E18 | `CulturalArchiveVaultSystem.TryRecordChronicleEntry` and `JournalSystem.TryAddRawEntry`. | `Culture/CulturalArchiveVaultSystem.cs` L385; `Journal/JournalSystem.cs` L281 | **RESOLVED (§2b)** |
| E19 | Sections `regional_treaty` and `diplomatic_summits` exist in the registry; Paper Ledger nests beside faction-embargo state in the expansion host. | `Save/SaveSectionRegistry.cs` L120, L229 | **RESOLVED (§2b)** |
| E21 | `FactionEmbargoLedger` exposes only `TryAddEmbargo`, `IsEmbargoed(factionId, day)`, `ActiveEmbargoes(day)` and capture/restore: **there is no removal call**, and `IsEmbargoed` takes no scope, so an embargo suspends a faction's trade **wholesale** for its window; `scope` is a label. | `FactionEmbargoLedger.cs` L53–123 | LIVE (finding; re-verify P0) |
| E20 | Difficulty scalars are read at owner sites; this plan changes none. | `difficulty_presets.json` | LIVE |

**Six findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E8 — there is no papers concept at all.** PP is a genuinely new *small* state owner (the Paper Ledger), not a layer over an existing one. Everything else it touches is a read or a request through an existing owner.
2. **E4 — three treaty owners.** The Table is a **fold with provenance**, exactly as the Remembrance Calendar folds three remembrance owners: no owner is edited to reconcile them, and any reconciliation is a `KNOWN_DEBT` item for the integrator, not this plan.
3. **E5 vs E6 — "embargo" names two different things.** Only `FactionEmbargoLedger` is political. The Table reads and requests through it; the weather system is untouched, and the UI must never merge the two boards without labelling.
4. **E9 — `BureaucraticDocument` is lore, not law.** PP must not extend that catalog into a permit system; it may *cite* it (a lore document can be the basis of a `Found` paper, DEC-PP-09) and nothing more.
5. **E5/E21 — the political ledger is blunt.** It cannot lift an embargo early and ignores scope, so every embargo this plan requests is a fixed-window, faction-wide trade closure. The plan says so on the Board and bounds clauses by the treaty term; an early-lift call is an unsigned option (DEC-TT-05).
6. **E10 — no forgery crime.** Detected forgery routes through standing, the embargo ledger and the chronicle. A justice route is an unsigned option (DEC-PP-06), because adding an enum value to a shared owner is a decision, not a convenience.

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

**Confirmed:** E1–E3, E5–E14 read as written. What the open items resolved to:

| # | Open item | Result | Edit made |
|---|---|---|---|
| E4 | Does the host run all three treaty owners? | **Yes, all three are instantiated**: `RegionalTreatySystem` (`Main.ShelterSocial.cs` L110, `RegionalTreatyHostSession`), `DiplomaticSummitSystem` (`Main.FlagshipInstitutions.cs` L183), `FactionDiplomacySystem` (`DiplomacyHostSession.cs` L47). The overlap is therefore live, not theoretical. | The Treaty Table is a **reading and a filing surface over the three**, never a fourth owner; a paper cites the owner that granted it |
| E15 | Which is the standing write authority? | Two exist, split by faction kind. `FactionWarSystem.ModifyStanding(factionId, delta)` normalises the id through `FactionStandingIdResolver.ToSystemsId` and is the general write; `FactionBranchCoordinator.ModifyStanding` routes only PRPF and the military branch. The host already picks between them (`Main.PsyOps.cs` L60–71; `Main.Narrative.cs` L500 calls `FactionWar` directly). | Paper events write through `FactionWarSystem.ModifyStanding` and copy the `Main.PsyOps` branch rule for PRPF/military (DEC-PP-14) |
| E16 | "Forger's hand" read | **No forger skill exists.** `skills.json` carries `skill_steady_hands`, `skill_crafting`, `skill_workshop_sense` and others; none names writing or forgery. `SkillProgressionSystem.GetSkill(id)` returns a *definition*; the per-survivor level read must be confirmed at coding time. | Forger's hand is a **composite read** of `skill_steady_hands` and `skill_crafting` (DEC-PP-15); no skill is added by this plan |
| E18 | Chronicle writer | `CulturalArchiveVaultSystem.TryRecordChronicleEntry` (fixed `summary_key`, deduped) and `JournalSystem.TryAddRawEntry` (free text, per-key dedupe). | Treaty milestones with fixed wording → chronicle keys; papers with a named bearer or a day → journal |
| E19 | Save homes | `regional_treaty` and `diplomatic_summits` are registry sections; `faction_diplomacy` state lives with `DiplomacyHostSession`. | Treaty Table records nest in `regional_treaty`; the Paper Ledger nests beside the faction-embargo state in the expansion host (never a new section) |
| — | Faction ids in the tolls table | `faction_rebuilders` **is real** (`caravans.json`, `faction_war_events.json`, `holdfast_factions.json`). `faction_salt_freeholders` **is real** (`year_of_ash_*`, `quests_npc_arcs.json`). The flotilla is **`faction_black_flotilla`** (`faction_territory.json`, `characters.json`) — not "the fleet", which is a separate id (`faction_the_fleet`). | the three VERIFY marks in the authority rows cleared; flotilla id fixed |
| E17 | Audit reads (trade, water, expedition log) | Owners exist but exact public reads for the Observed column are **not yet confirmed line for line**. | left open, narrowed to a P0 checklist item: one read per audit template, listed with its owner |

**What did not change:** the Paper Ledger's derive-don't-store rule, the six authored authorities, the audit templates, the tone rule, and the principle that dispatch and diplomacy owners speak first.

**One consequence worth stating plainly.** Three systems already promise the player that a peace can be signed, and the host runs all three. A paper that says *"under the compact"* has to say which compact. The Treaty Table's first honest act is a column that names the owner.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Regional treaties (scrap-paid, route effects) | `RegionalTreatySystem` | none |
| Summits, frameworks, guarantees, DMZ, violations | `DiplomaticSummitSystem` | none |
| Trust, envoys, missions, treaty templates | `FactionDiplomacySystem` | none |
| Political embargoes (start/end/dedupe) | `FactionEmbargoLedger` | requests via `TryAddEmbargo` only |
| Weather embargoes | `TradeEmbargoSystem` | none |
| Faction standing | existing standing owner (P0 resolves E15) | requests only |
| Crimes, evidence, trials | `JusticeSystem` | none (option DEC-PP-06) |
| Census, levy | `CensusClaimSystem` | read-only for audit lines |
| Lore documents | `BureaucraticDocumentCatalog` | read-only |
| Inventory, items | inventory owner | consumes `paper_stock` through its command |
| **Papers held, their origin, craft, revocation** | — | `PaperLedger` (stored) |
| **Authorities: scrutiny, recognised kinds, issue fees, audit cadence** | — | `authorities.json` (data) + `AuthorityCatalog` |
| **Paper check outcome** | — | `PaperCheck` (pure, deterministic) |
| **Audit book and history** | — | `AuditEngine` (derives lines) + `AuditHistory` (stored, bounded) |
| **Treaty Table view, Position, Leverage, life stage** | — | pure read models over the owners |
| **Toll schedules** | — | `TollScheduleState` (stored) + `ITollSchedule` read hook |
| **Clause log (which clauses were enacted)** | — | `ClauseLog` (stored, idempotence and history) |

**Non-duplication statement.** Every treaty term, embargo window, standing value and trade figure stays with its owner. The plan stores five kinds of *human or historical fact*: **a paper exists** (with its origin and craft), **a paper was revoked**, **an audit happened** (and what it found), **a toll schedule was set**, and **a clause was enacted**. Everything else — paper status, check odds, audit lines, leverage, treaty life stage, effective toll — is derived on read. **No new standing, embargo, trade, crime or currency authority is created.**

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (PP):** `Assets/Ashfall.Core/Paper/PaperLedger.cs` (new: state + rules), `Paper/PaperKind.cs`, `Paper/PaperStatus.cs` (pure derivation), `Paper/AuthorityCatalog.cs` (loader), `Paper/PaperCheck.cs` (pure, deterministic), `Paper/Forgery.cs` (craft quality), `Paper/AuditEngine.cs` (derive lines, severity), `Paper/AuditHistory.cs`, `Paper/PaperCatalogLoader.cs`.

**Core (TT):** `Assets/Ashfall.Core/Diplomacy/Table/TreatyTableView.cs` (fold, pure), `Table/TreatyPosition.cs` (Position, Leverage), `Table/TreatyLifeStage.cs` (pure), `Table/TollSchedule.cs` (state + rules), `Table/ITollSchedule.cs` (read hook), `Table/ClauseLog.cs`, `Table/ClauseCatalogLoader.cs`, `Table/EmbargoBoard.cs` (read). Additive nested DTO in the faction-diplomacy save (`INT`, home chosen at P0) and the paper-ledger home (`INT`).

**Data (PP):** `paper_kinds.json`, `authorities.json`, `audit_line_templates.json`, `paper_lines.json`, `forgery_inputs.json`.
**Data (TT):** `treaty_clauses.json`, `treaty_positions.json`, `toll_schedules.json`, `table_minutes.json`.

**Host:** `src/Main.SubsystemComposition.cs` (`INT`: day-owner and probe registration), the checkpoint/gate seam in the caravan/expedition host (`INT`: a single `IPaperGate` call site), `src/Main.Campaign.cs` (`INT`: briefing line for audits and lapses).

**Presentation (both):** extend an existing diplomacy/faction panel for the Table and an existing inventory or faction panel for Papers; no new routed panel (DEC-PP-08, DEC-TT-08).

**Tests:** `Ashfall.Core.Tests/Paper/*` (Ledger, Status, Check, Forgery, Audit, Save); `Ashfall.Core.Tests/Diplomacy/Table/*` (Fold, Position, LifeStage, Toll, ClauseLog, Embargo, Save); parity guards extend existing treaty, embargo, summit, diplomacy, justice and census tests unchanged.


---

# PART ONE — PAPER AND POWER

## 5. The Paper Ledger (the one new stored owner)

### 5.1 What a paper is

A **paper** is a document a holder can *present* to an authority as proof: of the right to pass, trade, draw, settle, salvage, or of a fact (cleared of quarantine, certified fit to run). It is **stored** because its existence, origin and craft are historical facts about what the shelter did; its **status** is never stored.

```csharp
public sealed class PaperLedgerState
{
    public int SchemaVersion = 1;
    public List<PaperRecord> Papers = new();       // bounded: <= 48 live records
    public Dictionary<string, int> HeatByAuthority = new();   // authority id -> detected-forgery count (history)
    public List<string> ChronicleKeys = new();     // idempotence guard
}
public sealed class PaperRecord
{
    public string PaperId;          // "paper:<kind>:<issued_day>:<n>"
    public string KindId;           // paper_kinds.json
    public string IssuerId;         // authority id whose name is on it
    public string HolderType;       // "shelter" | "survivor" | "caravan" | "vehicle"
    public string HolderId;         // null for shelter
    public string Origin;           // "issued" | "bought" | "forged" | "amended" | "found"
    public int    IssuedDay;
    public int    ExpiryDay;        // -1 = no expiry; derived from kind lifetime at creation, stored as a day
    public string ScopeId;          // route, zone, faction, site, or null
    public string BasisId;          // treaty id | authority licence | lore document id | null
    public int    Craft;            // 0..100; meaningful only for forged/amended; -1 otherwise
    public string ForgedBy;         // survivor id, if origin forged/amended
    public bool   Revoked;          // human/authority act
    public int    RevokedDay = -1;
    public string RevokedReason;    // "detected" | "recalled" | "surrendered"
    public int    DeclaredAmount;   // for licences with declared quantity (units), else 0
    public string DeclaredItemId;   // e.g. "clean_water"
}
```

**Home:** nested in a single existing save owner (DEC-PP-02, decided at P0). A legacy save without the field loads as **no papers held, no heat** and every checkpoint behaviour equals today's (there was no paper check to fail).

### 5.2 Derived status (never stored)

`PaperStatus.Of(paper, day, treatyReader, authorityCatalog)`:

| Status | Rule |
|---|---|
| **Revoked** | `Revoked == true` |
| **Void** | `BasisId` names a treaty/licence that the owner reports **not in force** (E1–E3: not Active/Ratified; or expired) |
| **Expired** | `ExpiryDay >= 0 && day > ExpiryDay` |
| **Expiring** | `ExpiryDay >= 0 && ExpiryDay - day <= 5` |
| **Valid** | otherwise |

Order of precedence: Revoked, Void, Expired, Expiring, Valid. **Void** is the seam to the Treaty Table (§1.3): a paper resting on a treaty goes quiet when the treaty does, with no write and no event.

### 5.3 Rules

- **P-1** A paper is created by exactly one of: **Issue** (authority act, §6), **Buy** (a broker; price from data, item cost through the inventory owner), **Forge** (§7), **Amend** (§7), **Find** (loot or a discovery table; may cite a lore document as basis).
- **P-2** Papers are never deleted. A spent or expired paper may be **surrendered** (archived) but the record stays for history (bounded: when > 48, oldest *archived* records are summarised into `HeatByAuthority` and a count; live papers are never dropped).
- **P-3** A paper is held by a **holder**: the shelter, a survivor, a caravan or a vehicle. Presenting a paper requires the *presenter* to be the holder or to act for it (a caravan presents its own papers; a survivor presents theirs; the shelter's papers are presented by whoever leads).
- **P-4** `Craft` exists only on forged/amended papers; authentic papers have `Craft = -1` and can **never be Detected** as forged (they can still be Expired, Void, Refused for scope).
- **P-5** `ScopeId` is validated at creation against existing route/zone/faction/site ids (§18 validator rules).
- **P-6** One paper cannot have two bases. An amended paper keeps its original `BasisId` (an amendment does not make a treaty say what it does not).

---

## 6. Authorities and the twelve paper kinds

### 6.1 Authorities (data rows tied to existing factions)

An **authority** is a data row: who checks, how hard, what they recognise, what they issue, at what fee, and how often they audit. Rows bind to existing faction ids (E12); *working labels* below are the row ids.

| Authority id | Faction ref (E12) | Scrutiny | Issues | Recognises | Audit cadence | Detected-forgery response |
|---|---|---|---|---|---|---|
| `auth_holdfast_office` | Reconstruction Office (`faction_rebuilders`, verified §2b) | 55 | settlement charter, census entry, water draw permit | own + rail certificates | 90 days | standing −15; papers recalled |
| `auth_caravanserai_law` | `faction_salt_freeholders` (verified §2b) | 35 | trade licence, passage pass | trade licence, passage pass, carrier's bill | 60 days | standing −8; 14-day embargo |
| `auth_fleet_port` | black flotilla (`faction_black_flotilla`, verified §2b) | 80 | passage pass (harbour), salvage claim | passage pass, safe conduct, salvage claim | 45 days | standing −20; 30-day embargo |
| `auth_rail_guild` | `faction_railway_guild` | 60 | rail certificate, carrier's bill | rail certificate, carrier's bill, trade licence | 60 days | standing −12; certificate revoked |
| `auth_water_committee` | `faction_the_water_committee` | 50 | water draw permit | water draw permit, settlement charter | 30 days | standing −10; 14-day embargo (faction-wide; the ledger is scope-blind, E21) |
| `auth_quarantine_post` | `faction_the_quarantine_post` | 90 | quarantine clearance | quarantine clearance | none (event) | standing −25; 45-day embargo |

Numbers are **data** (DEC-PP-04). **Scrutiny** (0–100) is the authority's own thoroughness; it is never scaled by difficulty (X-6).

### 6.2 The twelve paper kinds (`paper_kinds.json`)

| Kind id | Purpose | Issued by | Default lifetime (days) | Scope | Fee | Notes |
|---|---|---|---|---|---|---|
| `passage_pass` | pass a post or port | caravanserai, fleet port | 30 | route or post | 4 scrap | the everyday paper |
| `trade_licence` | trade in a market | caravanserai, rail guild | 60 | market/region | 10 scrap | carries a declared cargo line |
| `salvage_claim` | claim a site | fleet port | 45 | site id | 12 scrap | blocks rival claims by paper only |
| `settlement_charter` | right to hold the shelter as a place | Holdfast office | 365 | shelter | 20 scrap | Void if its basis lapses |
| `water_draw_permit` | draw from a shared source | water committee, Holdfast office | 90 | source id | 6 clean water | carries a declared draw |
| `quarantine_clearance` | proof of a clean bill | quarantine post | 21 | person/caravan | 5 scrap | short-lived on purpose |
| `carriers_bill` | consignment note for freight | rail guild, caravanserai | 14 | consignment | 3 scrap | ties to Long Line Freight |
| `rail_certificate` | a segment certified to run | rail guild | 60 | segment | (plan 1) | read from Iron Road certificates |
| `safe_conduct` | passage under a treaty | treaty clause | treaty term | faction/zone | 0 | basis = treaty id; Void when it lapses |
| `muster_paper` | house service credential | a House (plan 2) | service term | house | 0 | read from `HouseServiceState` |
| `census_entry` | a person listed | Holdfast office | none | person | 0 | mirrors `CensusClaimSystem.listed` |
| `broker_letter` | a bought introduction | a broker | 30 | faction | 15 scrap | recognised by no authority; only unlocks a Buy |

`rail_certificate`, `muster_paper` and `census_entry` are **mirrors**: the Paper Ledger *lists* them so the player has one Papers view, but their truth stays with the Iron Road ledger, `HouseServiceState` and `CensusClaimSystem`. A mirror is a **read**, never a copy (DEC-PP-03).

### 6.3 Issuing

`Issue(kind, authority, holder, scope, day)` requires: the authority issues the kind; the fee is paid through the inventory owner's existing command; the holder meets the kind's **standing floor** (e.g. trade licence needs standing ≥ Neutral with the authority's faction, read from the standing owner); no live paper of the same kind and scope is already held (renewal is a new issue, superseding: the old one is not deleted). Result: a new `PaperRecord` with `Origin = "issued"`, `Craft = -1`, `ExpiryDay = day + lifetime`.

### 6.4 Worked example — a caravan's day-one papers

Day 12. The Holdfast Salt Run caravan (plan 2) will pass the caravanserai and the Fleet port. The player issues, at the caravanserai (fee 4 scrap each): a `passage_pass` (route `salt_run`, expiry day 42) and a `trade_licence` (market `caravanserai`, declared item `salt_block`, amount 40, expiry day 72), and at the Fleet port a `passage_pass` (harbour, expiry day 42, fee 4 scrap). Cost: 4 + 10 + 4 = 18 scrap; three records; Papers surface shows: *"Passage pass, caravanserai. Valid to day 42."*, *"Trade licence, caravanserai. Declares 40 salt block. Valid to day 72."*, *"Passage pass, Fleet port (harbour). Valid to day 42."*

---

## 7. Forgery and amendment (two human acts, one craft rule)

### 7.1 The acts

- **Forge(kind, authority, survivor)** — make a paper that *looks issued* by an authority that never issued it. Requires: `paper_stock` ×1 (inventory command), a **forger** survivor, one **survivor-day**, a **seal source** (see below), optional inputs for quality. Result: `Origin = "forged"`, `IssuerId = authority`, `Craft = quality`.
- **Amend(paper, changes, survivor)** — alter a **genuine** paper (extend `ExpiryDay`, widen `ScopeId`, raise `DeclaredAmount`). Requires the same inputs. Result: `Origin = "amended"`, original `BasisId` kept, `Craft = quality`. Amending an already-forged paper is refused (no double forgery: P-7).
- **Seal source:** a **seal tool** item (inventory), or a *lifted impression* from a genuine paper of the same authority held in the ledger. Without one, quality is capped at 30 (a plain hand, no stamp).

### 7.2 Craft (0–100), deterministic from real inputs

`Craft = clamp( 10 + 6*Skill + 10*InkGood + 8*SealTool + 5*CareDays + 6*SamplePaper − 12*Rush , 0, 100 )`

| Term | Meaning |
|---|---|
| `Skill` | the forger's relevant skill/trait read (0..10), E16 |
| `InkGood` | 1 if an ink-grade item is spent, else 0 |
| `SealTool` | 1 if a seal tool is used, else 0 |
| `CareDays` | extra survivor-days spent, 0..3 (each costs a survivor-day) |
| `SamplePaper` | 1 if a genuine paper of the same authority and kind is held (a model) |
| `Rush` | 1 if made in under a day's care (under time pressure), −12 |

No randomness: the same inputs give the same craft. All terms are data (`forgery_inputs.json`, DEC-PP-05).

### 7.3 Worked example — a Fleet port pass

Day 30. A forger with Skill 4 wants a harbour `passage_pass`; the held genuine one (§6.4) is a sample; a seal tool is in the workshop; good ink is available; two extra care days.
`Craft = 10 + 6·4 + 10·1 + 8·1 + 5·2 + 6·1 − 0 = 10 + 24 + 10 + 8 + 10 + 6 = 68`.
A hasty version (no tool, no ink, no sample, no care, rushed): `10 + 24 + 0 + 0 + 0 + 0 − 12 = 22` and capped at 30 by the missing seal source → **22**.

### 7.4 Rules

- **F-1** A forger is a **survivor doing a day of work**; that day is consumed through the existing assignment/duty owner. A survivor who is sick or away cannot forge.
- **F-2** `paper_stock` is consumed on **making**, not on detection.
- **F-3** A forgery is a private fact: no authority "knows" it exists until a check **Detects** it (§8) or an audit line catches it (§10).
- **F-4** No forged paper is ever silently discarded; if detected it is **Revoked** with reason `detected` and remains in the ledger.
- **F-5** The **forger's identity** is stored on the record (`ForgedBy`) so consequences and story lines can name them; it is never shown to an authority unless a check reports it (§8.3).

---

## 8. The Check (deterministic, one decision per paper-authority-day)

### 8.1 Interface

`PaperCheck.Evaluate(PaperCheckRequest, ...)` returns `CheckResult { Outcome, Reason, DetectChance, ClerkLineId }`.

```csharp
public sealed class PaperCheckRequest
{
    public string AuthorityId;
    public string PaperId;          // null = presenting nothing
    public string PostId;           // where (for scope checks)
    public string RequiredKindId;   // what the post asks for
    public int    Day;
    public int    HeatOverride = -1;
}
public enum CheckOutcome { Accepted, Queried, Refused, Detected }
```

### 8.2 Decision order (first match wins)

1. **No paper presented** → **Refused** (`no_paper`).
2. Paper kind is not one the authority **recognises** for `RequiredKindId` → **Refused** (`not_recognised`).
3. Status is **Revoked / Void / Expired** → **Refused** (`revoked`, `void`, `expired`). These are *deterministic*, no roll: a genuine-but-dead paper is simply dead.
4. **Scope mismatch** (paper scope does not cover `PostId`) → **Refused** (`scope`).
5. **Genuine** (`Craft = -1`) and status Valid/Expiring → **Accepted**. (Expiring is accepted with a *Queried* flourish line only; the outcome remains Accepted.)
6. **Forged or amended**: compute `Detect` and roll once (below): rolled below `Detect` → **Detected**; else if `Detect >= 25` and the roll is at least `Detect` but below `Detect + 10` → **Queried** (the clerk hesitated; the caller may *press on* or *withdraw*); else → **Accepted**.

### 8.3 Detection chance (percent)

`Detect = clamp( Scrutiny − 0.6·Craft + 4·Heat , 3 , 95 )`

- `Scrutiny` from the authority row (§6.1).
- `Heat` = `HeatByAuthority[authority]`, capped at 5 (each detected forgery makes that authority warier; history, stored).
- The **roll** is `NextInt(100)` from the stream `paper_check:<paper_id>:<authority_id>:<day>` (a `CampaignStreamIds` fork). A given (paper, authority, day) always yields the same outcome, across save/load and repeated presentation, so re-presenting on the same day cannot be used to re-roll.

### 8.4 Worked examples (Craft 68 from §7.3; Craft 22 for the hasty one)

| Paper | Authority | Scrutiny | Heat | Detect |
|---|---|---|---|---|
| Craft 68 | Fleet port | 80 | 0 | `80 − 40.8 + 0 = 39.2` → **39%** |
| Craft 68 | Fleet port | 80 | 2 | `80 − 40.8 + 8 = 47.2` → **47%** |
| Craft 22 | Fleet port | 80 | 0 | `80 − 13.2 = 66.8` → **67%** |
| Craft 68 | Caravanserai | 35 | 0 | `35 − 40.8 = −5.8` → clamped **3%** |
| Craft 22 | Caravanserai | 35 | 0 | `35 − 13.2 = 21.8` → **22%** |
| Craft 68 | Quarantine post | 90 | 0 | `90 − 40.8 = 49.2` → **49%** |

The lesson written into the table: **a good forgery is a good bet at a soft post and a coin-toss at a hard one.** The player is never shown these numbers; they learn them by living (§1b). The Papers surface may show a *qualitative* line for a forged paper the player made ("*a steady hand; the stamp is yours*"), never the percent (DEC-PP-07).

### 8.5 Outcomes and their consequences (through existing owners only)

| Outcome | What happens |
|---|---|
| **Accepted** | the caller proceeds; a chronicle line only for a *first* accepted forgery at an authority (idempotent) |
| **Queried** | the caller chooses **Press on** (re-evaluates once with `Detect + 15`) or **Withdraw** (no penalty, the post stays closed for the day) |
| **Refused** | the caller cannot pass; the consequence is the **post owner's own** (plan 2's Checkpoint: pay toll, refuse → Ambush strength; a shelter gate: entry denied) — the check adds nothing |
| **Detected** | (1) paper `Revoked` (`detected`); (2) standing delta from the authority row through the standing owner (E15); (3) `HeatByAuthority[authority]++`; (4) if the row lists an embargo, `FactionEmbargoLedger.TryAddEmbargo(faction, scope, day, days, "paper:<paper_id>")`; (5) a chronicle line; (6) *optional* `IJusticeSink.OnForgeryDetected(forgerId, authorityId)` (default no-op, DEC-PP-06) |

**No other penalty is added.** A player who is Detected at the caravanserai is *poorer, warier-watched and slightly less welcome*, not ruined.

### 8.6 Worked example — the Fleet port, day 44

The Salt Run caravan arrives at the Fleet port (plan 2 *Checkpoint*, `toll_permille` 150). It presents the **forged** harbour pass (Craft 68; the genuine one expired on day 42). Order: recognised; status **Valid** (forged record expiry day 60); scope ok; forged → `Detect = 39%`. Roll for `paper_check:paper:passage_pass:30:0:auth_fleet_port:44` is 71 → not detected; `Detect >= 25` and the roll is 32 above (`71 − 39`) — not below `Detect + 10 = 49` → **Accepted**. Chronicle (first accepted forgery at this authority): *"The inspector read the pass twice, which is what he does with all of them. Then he stamped it."* The caravan pays its 15% toll as usual (plan 2), passes, and the ledger notes nothing else.

Two weeks later, the same forged pass at the same port on day 58 rolls 22 → **Detected**. Consequences: paper Revoked; standing −20 through the owner; `HeatByAuthority["auth_fleet_port"] = 1`; embargo 30 days `sourceId "paper:paper:passage_pass:30:0"`; chronicle: *"You have a good hand. It was still a hand."* Note that the *same* paper passed once and failed once, because the roll is per (paper, authority, day) — not per paper.

---

## 8b. Texture — the counter

**The clerk's lines** (`paper_lines.json`, keyed by outcome and authority temperament; 4–6 each):

- Accepted, hard post: "He read it twice, which is what he does with all of them. Then he stamped it."
- Accepted, soft post: "She did not look up. The stamp came down without a sound."
- Queried: "He turned it over and read the back, which had nothing on it. Then he read the front again."
- Refused, expired: "It says the eleventh. It is the twelfth. He was almost apologetic."
- Refused, scope: "It is a good pass for a different road."
- Detected: "There was a pause of a kind you do not forget. Then two people came in from the next room."
- Detected, forger's line: "You have a good hand. It was still a hand."

**The workshop at night.** Forging is written as *a night's work*, not a feat: a lamp, a straight edge, a sheet of stock, a stamp that is a shade too dark and has to be pressed lighter. The line the forger's day produces (`forge_lines`): *"Marisol worked until the lamp guttered. It looks right. It looks right the way a photograph of a thing looks right."*

**The small shame.** A survivor asked to forge for the shelter's sake may carry it. That is an *optional* hook to the survivor-stress/conscience owners (§20 hooks) — a **line and a chronicle key**, never a new stat.

---

## 9. Presenting papers (where the check happens)

### 9.1 One gate seam

The host exposes a single `IPaperGate.Present(postId, requiredKindId, holder, day)` call. Callers:

| Caller | When | On Refused |
|---|---|---|
| Convoy plan *Checkpoint* (plan 2) | a caravan reaches a post | existing rule: pay toll or take Ambush strength |
| Shelter gates / trade visitors | a trader wants to enter or trade | entry refused with a reason line |
| Rail posts (plan 1 railhead) | a consignment moves | consignment held at the post |
| Water source draw | a draw is attempted | draw refused for the day; the water owner's own rules apply |
| Expedition to a claimed site | a party enters a claimed site | party warned; claim dispute line |

**Each caller keeps its own consequence.** The gate returns an outcome; the caller decides what refusal means. If no caller is registered (default `NullPaperGate`), every present returns `Accepted` and **nothing in the game changes** — the plan ships dark.

### 9.2 Who presents what (auto-selection)

`PaperSelector.BestFor(holder, requiredKind, postId, day)` chooses among held papers: prefer **genuine Valid** over Expiring over forged over amended; among equals, the one with the latest expiry. The player may override in the surface (a "choose paper" control) *before* presenting. The selector never picks a Revoked/Void/Expired paper unless nothing else exists (so the player sees the refusal reason honestly).

### 9.3 Buying and finding

`Buy(kind, broker)` costs the data-listed items and yields `Origin = "bought"` with `Craft = -1` (a bought paper is *genuine as far as the authority can tell*, because it was made by someone at the source — a deliberately grey market, DEC-PP-10). A bought paper may still be **Detected** if the authority row marks the broker as *compromised* (data flag, per broker) — modelled as a `Craft` equal to the broker row's `quality` so it uses the same formula, and the record's `Origin` stays `"bought"`.

---

## 10. Audits (declared versus recorded)

### 10.1 What an audit is

An **audit** is an authority looking at the shelter's own **declarations** and comparing them to what existing owners **record**. It is an event with a **Book** (a list of lines), each line reading `declared` vs `observed` and yielding a small **finding**. It is *ordinary*: a clerk with a ledger.

### 10.2 Declarations (what the shelter has said)

Declarations live *on papers* (the licence's `DeclaredItemId` + `DeclaredAmount`) and in **standing declarations** (headcount claimed to an office; water draw claimed to a committee), both stored in the ledger as part of the paper or a small `Declaration` list (≤ 12) on the state. A declaration is a **human act** (the player states a figure when applying) and is stored because it is a historical fact — *what was said*, not what was true.

### 10.3 The line templates (`audit_line_templates.json`)

| Template | Declared (stored) | Observed (read) | Owner read (E17, VERIFY) |
|---|---|---|---|
| `line_trade_volume` | licence's declared cargo amount | units actually shipped in the term | trade/caravan ledger |
| `line_water_draw` | permit's declared daily draw | measured draw over the term | `DeepWellSystem`/`WaterTreatmentSystem` |
| `line_headcount` | census entries listed | roster count | `CensusClaimSystem` ledger vs roster |
| `line_salvage_sites` | claimed site ids | sites actually visited | expedition state |
| `line_treaty_quota` | treaty required concession | delivered concession | treaty owner compliance |
| `line_toll_paid` | tolls the shelter says it paid | tolls recorded as paid | toll receipts (TT) |
| `line_papers_in_order` | none | any Void/Expired papers held in use | Paper Ledger itself |
| `line_forgery_check` | none | any paper with `Origin` forged/amended presented in the term | Paper Ledger + present log |

Each template maps a **gap** to a **finding severity**: `gap_pct` bands (`0–5` clean, `6–15` noted, `16–30` fined, `> 30` suspended, and a `Struck` band for forgery). Bands are data (DEC-PP-04).

### 10.4 Running an audit

1. **Trigger:** cadence (authority row, e.g. Holdfast office 90 days, with a **seeded jitter** ±10 days from stream `audit_when:<authority>:<n>`), or an event (an embargo begins; a complaint; a Detected forgery — the audit is *scheduled*, not instant).
2. **Sample:** the authority's **thoroughness** (= Scrutiny/25, rounded up) picks *how many lines* it reads: e.g. Scrutiny 55 → 3 lines; Scrutiny 80 → 4; Scrutiny 35 → 2. Which lines: a seeded pick (`audit_sample:<authority>:<day>`) from the templates the shelter has declarations for; lines with a **known** gap are *not* more likely (the audit does not know), so honesty is not a sure defence and luck is not a sure defence.
3. **Findings:** for each sampled line: `gap = |declared − observed| / max(declared,1)`; severity from bands; a `Struck` finding only from `line_forgery_check`.
4. **Outcome (worst finding decides)** — through existing owners: **Clean** (standing + 1, if the owner allows), **Noted** (a chronicle line), **Fined** (fee in items via inventory; standing unchanged), **Suspended** (a paper of the audited kind is Revoked, reason `recalled`; a 14-day embargo request (source `audit:<authority>:<day>`; scope is a label only, E21)), **Struck** (as Detected, §8.5).
5. **Record:** an `AuditRecord` (authority, day, lines sampled, findings, outcome) is appended to `AuditHistory` (bounded ≤ 24; oldest summarised to counts).

### 10.5 Setting the books, and tidying

Before the audit day the player may open the **Audit Book** (the surface shows the *declared vs observed* for every template with a declaration — the player can *see their own gaps*):

- **Set the books right** (honest): amend a **declaration** to the true figure (a permit's declared draw is updated). Costs a survivor-day of *clerking* (assignment owner). Effect: that line's gap becomes 0; a chronicle line records the correction.
- **Tidy** (falsify): keep the declaration but **doctor the evidence**. Requires the forger inputs of §7; produces a **tidied line** with `Craft` from §7.2. When sampled, a tidied line runs the same **Detect** formula against the authority's Scrutiny; not detected → the line reads as clean (gap 0); detected → severity **Struck** for the whole audit.
- **Do nothing:** the gap stands.

### 10.6 Worked example — the Holdfast office audits the Salt Run

Day 91. The Holdfast office (Scrutiny 55 → 3 lines) audits the shelter. Declarations on file: a `trade_licence` declaring 40 salt block (observed shipped 52); a `water_draw_permit` declaring 6/day (observed 6); a census with 8 entries (roster 9). Templates with declarations: volume, water, headcount → the sample picks all three (only three exist).

- Volume: `|40 − 52|/40 = 30%` → **fined** band (16–30).
- Water: `0%` → clean.
- Headcount: `|8 − 9|/8 = 12.5%` → **noted** band (6–15).

Worst finding: **Fined**. Consequence: a fee in items (data: 6 scrap), a chronicle line: *"The clerk wrote 40 and 52 on the same line and drew a small box around the difference."* Had the player *set the books* on day 89 (declared 52), volume would be 0% and the worst finding would be **Noted** (headcount). Had they **tidied** the volume line with Craft 60 against Scrutiny 55: `Detect = 55 − 36 + 4·0 = 19%`; roll 47 → not detected → the line reads clean → worst finding **Noted**; risk taken, 19%.

### 10.7 Rules

- **A-1** An audit reads only **declared** lines; an authority never audits what the shelter never said.
- **A-2** Every gap comes from an owner read; no gap is invented. If an owner read is unavailable, that template is **skipped** with `read_unavailable` in the Book, never guessed (R-6 style).
- **A-3** Determinism: audit timing and sample are seeded through named forks; the same seed and state give the same audit.
- **A-4** A player can **Decline** an audit once per authority per year at a standing cost (data): the authority records the refusal as a `Noted`-level finding and reschedules in 30 days. (DEC-PP-11.)
- **A-5** An audit never **ends** a treaty or removes a treaty benefit directly; it can only Revoke papers and request embargoes, which the Table then shows.

---

## 11. Papers in the world (worked scenes)

### 11.1 The cheap pass and the hard port

The caravan's passage pass to the caravanserai (Scrutiny 35) can be a *Craft 40* forgery at ~11% detect risk; the Fleet port (Scrutiny 80) needs a *Craft 80+* paper to get the risk under 32%. **The player pays for quality with days, ink and seal tools, not with dice.** That is the whole economy of forgery.

### 11.2 Void papers

Day 200. A `safe_conduct` (basis: `treaty_non_aggression_compact`, 30 days) is held by three caravans. The compact lapses (E2 `duration_days` 30, no renewal). On day 201 the Papers surface lists all three as **Void** with the line *"The treaty this rests on has ended."* No write occurred; no event fired; nothing was revoked. If the treaty is renewed on day 205 they return to **Valid** on their own (their `ExpiryDay` is the treaty's *old* term end unless re-issued; DEC-PP-12 decides whether a renewal re-issues papers or simply revives them — default: **re-issue** by a Pass Clause on the new term).

### 11.3 The lifted impression

A forger holding a genuine Fleet port pass has a **sample** (+6). She also has a lifted impression from a **found** pass of the same authority (a dead courier's; `Origin = found`): treated as a seal source, lifting the no-stamp cap. Result: the harbour pass of §7.3 without a seal tool — `Craft = 10 + 24 + 10 + 0 + 10 + 6 = 60` (no tool term, impression as source). Detect at Fleet port heat 0: `80 − 36 = 44%`.


---

# PART TWO — THE TREATY TABLE

## 12. The Table view (one list, three owners, provenance kept)

### 12.1 What it folds

`TreatyTableView.Build(day)` reads, and never writes:

| Owner | Instruments it contributes | Provenance chip |
|---|---|---|
| `RegionalTreatySystem` | the 5 regional treaties (status, effects, ratification cost, compliance interval) | **Regional** |
| `DiplomaticSummitSystem` | the 8 summit frameworks and any scheduled summit, active treaty, guarantee and violation | **Summit** |
| `FactionDiplomacySystem` | the 6 treaty templates as *offerable* types, active `ActiveTreatyRecord`s, missions | **Diplomacy** |
| `FactionEmbargoLedger` | active political embargoes (a *restriction*, not a treaty; shown on the Embargo Board, §16) | **Embargo** |
| Toll schedules (TT) | posts with a set rate | **Toll** |

A row is `{ InstrumentId, DisplayName, Provenance, Parties[], Status, Stage, DaysLeft, Clauses[], Costs[], Effects[] }`.

### 12.2 Overlap handling (finding E4)

Three instruments overlap in concept (the non-aggression family: `treaty_non_aggression_compact` in the summit owner, `non_aggression` in diplomacy, and regional charters with raid-pressure relief). The Table **never merges** them. It shows each with its provenance chip and a shared **family chip** (`peace`, `trade`, `water`, `defence`, `debt`, `watch`) from `treaty_clauses.json: family_map`. If two rows with the same parties and family are **active at once**, the Table adds one plain line — *"Two agreements say much the same thing. Neither knows about the other."* — and a `KNOWN_DEBT` pointer for the integrator; it changes nothing.

### 12.3 Life stage (derived, never stored)

`TreatyLifeStage.Of(instrument, day)`:

| Stage | Rule |
|---|---|
| **Fresh** | less than 20% of term elapsed |
| **Settled** | 20% to term end minus 5 days, no strain |
| **Strained** | recorded violations ≥ `violation_tolerance` (summit) or status Violated/Suspended (regional) but not yet broken |
| **Lapsing** | 5 days or fewer left |
| **Lapsed** | status Expired, or term ended |
| **Broken** | status Violated beyond tolerance, or `BreakTreaty` called |

The stage is a **word** on the row; it carries no arithmetic. Terms, tolerance and durations stay with the owners.

### 12.4 Worked example — three rows on day 60

| Instrument | Provenance | Term | Stage | Note |
|---|---|---|---|---|
| Compact of Non-Aggression | Summit | 30 days from day 45 (`duration_days` 30) | Settled (15 of 30 elapsed = 50%) | tolerance 1, violations 0 |
| Road Iron Charter | Regional | ratified day 40, compliance every 30 days | Settled | +10% barter, 15% raid relief on the coastal route |
| Non-Aggression Pact | Diplomacy | base 60 days from day 20 | Settled (40 of 60) | trust requirement 20 met |

Table footer: *"Two agreements say much the same thing. Neither knows about the other."* (families `peace` overlap.)

---

## 13. Position and Leverage (derived)

### 13.1 Position card

For each faction, a **Position** is a read of:

| Field | Source |
|---|---|
| Wants / Offers | authored faction data (`treaty_positions.json`, per faction; a sentence each, plus tags) |
| Trust | `FactionDiplomacySystem.DiplomaticRelationState.trust` |
| Standing | the standing owner (E15) |
| Dependence (theirs on us / ours on them) | trade reads (trade discount/price relief owners, route dependence) |
| Alternatives | other instruments the faction holds with rivals (Table fold) |
| Threat | raid pressure and recent violations (owners) |

### 13.2 Leverage (a band, never a number on the card)

`Leverage = 50 + Need + Reach + Standing + Alternatives + Threat`, each term clamped to −20..+20 by data thresholds:

| Term | + (in the player's favour) | − |
|---|---|---|
| Need | they depend on something we supply (water, salt, repair) | we depend on them (road, fuel, medicine) |
| Reach | our route or fleet can hurt them | theirs can hurt us |
| Standing | our standing above Neutral with them | below |
| Alternatives | they have no other partner for this | they have several |
| Threat | they are under raid pressure we relieve | we are under theirs |

**Bands:** Weak < 40, Even 40–60, Strong > 60 (data, DEC-TT-04). The Position card shows the band and the **three largest terms in words**. The number is never displayed.

### 13.3 Worked example — the Fleet port

Data reads (illustrative): the port needs salt (Need +15); our road reaches their sea lane, theirs reaches ours (Reach 0); standing Neutral (0); they trade with two other partners (Alternatives −10); no raid pressure (Threat 0). `Leverage = 50 + 15 + 0 + 0 − 10 + 0 = 55` → **Even**. Card: *"They need your salt. They have two other partners for everything else. Neither of you can shut the other out."*

### 13.4 Rules

- **L-1** Leverage is recomputed on read; nothing is stored.
- **L-2** Any missing read makes that term `0` and the card says *"cannot say"* for it (R-6 style), never a guess.
- **L-3** Leverage **never changes** what an owner will accept. It only selects which authored ask-rung the Table offers for the *new clause kinds* (§14.4). Owner terms remain the owner's.

---

## 14. Clauses — three new kinds, expressed through existing owners

### 14.1 The clause catalog (`treaty_clauses.json`)

A **clause** is a data row that a treaty may carry; enacting it produces *one* call into an existing owner (or into the small TT state for tolls). Clauses are attached to treaty families, not to a fourth treaty system.

| Clause kind | Enacted through | Stored fact (TT) |
|---|---|---|
| **Toll clause** | `TollScheduleState` | a `TollScheduleRecord` |
| **Embargo clause** | `FactionEmbargoLedger.TryAddEmbargo` | a `ClauseLog` entry |
| **Pass clause** | `PaperLedger.Issue` (`safe_conduct`) | a `ClauseLog` entry + the papers |
| *(existing)* guarantee, DMZ, concession | the summit owner | none (owner-held) |

### 14.2 Enactment and idempotence

When a treaty is **ratified** (existing owner call succeeds), the host offers the Table the *ratification event*; the Table then enacts the clauses attached to that treaty's family by id (`clause_id`), exactly once (`ClauseLog` key `clause:<treaty_id>:<clause_id>:<ratified_day>`). Re-loading a save never re-enacts. **Ratification itself is untouched; the Table only reacts.**

### 14.3 The three kinds in detail

**Toll clause** — sets or lowers the rate at a named post for signatories (§15). Terms: `post_id`, `rate_permille` (bounded by the post's floor and ceiling), `exempt_multiplier` (0 or 50 for signatories).

**Embargo clause** — *requests* a political embargo against a named faction for `days` (bounded by the treaty term). **The ledger is what it is:** it has `TryAddEmbargo` and day-derived expiry, and **no removal call**, and `IsEmbargoed(factionId, day)` is **scope-blind** (E21). So an embargo clause is a *faction-wide trade suspension for a fixed window*, the `scope` string is a **label** only (shown on the Board), and an embargo **cannot be lifted early** in v1 (DEC-TT-05 records the option of one additive `TryLift(sourceId)` on the ledger, unsigned; default: none). The clause catalog therefore only offers embargoes with `days <= term remaining`, and the preview says plainly: *"This cannot be undone before it ends."*

**Pass clause** — on ratification, issues `count` `safe_conduct` papers (holder: the shelter or a named caravan) with `BasisId = treaty id`, `ExpiryDay = treaty term end`, `ScopeId = the treaty's zone or faction`. A treaty that later lapses or is broken makes them **Void** by the PP status rule; no revocation write.

### 14.4 The ask ladder (for the new clause kinds only)

Each faction temperament (data) has three **rungs** per clause kind: **Opening** (what they ask first), **Middle**, **Floor** (what they will accept at worst). The Table picks the rung by Leverage: **Weak → Opening, Even → Middle, Strong → Floor**. The rung sets the *values of the new clause* (toll rate, embargo days, pass count) — not any term the owner already holds. The **summit owner's own negotiation** (`AdvanceNegotiation`, concessions, `TryRatifyTreaty`) remains the act that ratifies; the Table's ladder is a **preview and a parameter source**. If P0 finds no way to attach clause values to a ratification without changing the owner, the ladder degrades to *advice lines only* (DEC-TT-09).

| Clause kind | Temperament | Opening | Middle | Floor |
|---|---|---|---|---|
| Toll | **Hard** (Fleet port) | keep 15% for all | 10%, signatories 5% | 7%, signatories exempt |
| Toll | **Market** (caravanserai) | 6% for all | 4%, signatories 2% | 2%, signatories exempt |
| Toll | **Guild** (rail guild) | 5% + fee per consignment | 3% | 1.5% |
| Embargo | **Hard** | 45 days | 30 | 14 |
| Embargo | **Market** | 21 | 14 | 7 |
| Pass | **any** | 1 paper, 14-day | 2 papers, term | 4 papers, term, renewable |

### 14.5 Worked example — "The Salt Run Accord"

Day 100. The player, at Even leverage with the caravanserai (market temperament), ratifies the **Demilitarised Trade Corridor** framework through the summit owner (existing act). The Table, reacting to the ratification event, enacts the attached clauses at the **Middle** rung:

1. **Toll clause:** post `post_caravanserai` rate 40‰ (4%), signatory exempt multiplier 50 → 20‰ for the shelter.
2. **Pass clause:** 2 `safe_conduct` papers for the Salt Run caravan, `BasisId` the treaty, expiry the term end.
3. **Embargo clause:** none (not offered by this family).

Minutes line: *"Agreed: four in the hundred at the caravanserai, two for the signatory. Two passes for the road. Not recorded: the north gate."* `ClauseLog` gets three keys. On day 130 the corridor lapses (term 30): the two passes go **Void** on their own; the toll falls back to the data row (60 in plan 2's row is *toll_permille 60* = 6%).

---

## 14b. Texture — the table

**Who sits.** The Table never draws a person, but its lines always imply one: *"Their factor read the clause aloud in the flat voice of someone who has read it aloud before."* The minutes speak for the room.

**What is not recorded.** Every ratification line ends with a clause left off, on purpose: *"Not recorded: the north gate."* *"Not recorded: whose water it was."* The player is never told what was left off. The phrase is authored (`table_minutes.json`), never derived, and is the plan's small deliberate silence.

**A quiet ending.** A treaty that lapses does not announce itself. *"The corridor treaty ended on the thirtieth. Nobody rang anything. Two papers went quiet in two satchels."* The player sees it in the row's stage and in the Papers surface.

**Embargo is a door, not a speech.** *"Their trade has been closed to us for fourteen days. Nobody said why. Everyone knows."* The board shows the **source chip** (debt, treaty, paper) so the player can tell who shut the door.

**A toll is a number with a name on it.** *"Four in the hundred, at the caravanserai, set by their council, waived for signatories."* A player who *sets* a toll at their own post sees their own name on the line.

---

## 15. Toll Schedules (small stored state, one read hook)

### 15.1 Shape

```csharp
public sealed class TollScheduleState
{
    public int SchemaVersion = 1;
    public List<TollScheduleRecord> Records = new();       // <= 24
    public List<TollReceipt> Receipts = new();             // bounded ring, <= 40
}
public sealed class TollScheduleRecord
{
    public string PostId;
    public string AuthorityId;
    public int    RatePermille;        // bounded by the post's floor/ceiling (toll_schedules.json)
    public int    SetDay;
    public string SetBy;               // "treaty:<id>" | "player" | "data"
    public List<string> ExemptTreatyIds = new();   // signatories of these get exempt_multiplier
    public List<string> ExemptKindIds = new();     // presenting these valid papers gets exempt_multiplier
    public int    ExemptMultiplierPct;             // 0 = free, 50 = half
}
public sealed class TollReceipt { public string PostId; public int Day; public string PartyId; public int Units; public string ItemId; }
```

### 15.2 The read hook

```csharp
public interface ITollSchedule
{
    // The rate the caller should apply at `postId` for this party today.
    int EffectiveTollPermille(string postId, TollParty party, int day);
}
public sealed class TollParty { public string FactionId; public IReadOnlyList<string> TreatyIds; public IReadOnlyList<string> ValidPaperKinds; }
```

**Default `NullTollSchedule`** returns the **authored data row** for the post (plan 2's `toll_permille`), so with no schedule the game is unchanged. The convoy plan's *Checkpoint* handler calls the hook once per arrival; it does not need to know any of the above.

### 15.3 Effective rate

`effective = base_or_scheduled × (party is exempt ? ExemptMultiplierPct/100 : 1)`, rounded to a whole permille; a party is **exempt** when one of its `TreatyIds` is in `ExemptTreatyIds` **and** the treaty is in force (owner read), or it holds a **valid** paper of a kind in `ExemptKindIds` (PP status). Bounded by the post's `floor` and `ceiling`. A **Void** or **Revoked** paper grants nothing.

### 15.4 Units and rounding

`toll_units = max(1, floor(cargo_units × effective / 1000))` when `effective > 0 && cargo_units > 0`, else 0. The toll is paid in the cargo's own goods (as in plan 2), through the inventory owner's command. Example: 40 salt blocks at 60‰ = `floor(2.4) = 2`; at 150‰ = 6; at 20‰ = `floor(0.8) = 0 → max(1) = 1`.

### 15.5 Who may set a rate

- **Data** sets the rate for every post (authored).
- A **treaty** may set or lower it (Toll clause; never raise above the post ceiling).
- The **player** may set the rate only at a post the **shelter holds** (`ITollPostReader.Holds(postId)`, default: none; Holdfast gate and the Iron Road *Railhead* when plan 1's state is Running are the first candidates, DEC-TT-06), within the post's floor/ceiling. A player-set toll credits receipts to the shelter's inventory as a `TollReceipt` and never touches the authority's own posts.
- Raising a toll changes **nothing else**: no standing effect, no reprisal. (Reprisal is the convoy plan's *Road War*, which reads `RoadMemory`; TT does not duplicate it.)

### 15.6 Worked example — the Railhead toll

Day 320. Plan 1's Railhead is Running and the shelter holds `post_railhead`. The player sets 30‰ (floor 10, ceiling 80). A hydro-barons freight party (treaty: none; no paper) carries 120 units: `floor(120 × 30 / 1000) = 3` units credited; `TollReceipt(post_railhead, 320, faction_hydro_barons, 3, "coal")`. A rail-guild consignment holding a valid `carriers_bill` (in `ExemptKindIds` at 50%): `effective = 30 × 0.5 = 15‰`, `floor(120 × 15/1000) = 1` unit. The Table shows the receipts strip and the current schedule; the minutes read: *"Three in a hundred and a half at the railhead. The Guild pays half and carries its own bill."*

---

## 16. The Embargo Board (read model)

### 16.1 What it shows

Every active political embargo from `FactionEmbargoLedger.ActiveEmbargoes(day)`: faction, scope label, start, end, days left, and a **source chip** parsed from `sourceId`:

| Source prefix | Chip |
|---|---|
| `debt:` | Debt |
| `treaty:` | Treaty |
| `paper:` | Paper |
| `audit:` | Audit |
| other | Other |

### 16.2 Rules

- **B-1** The board **never merges** with the weather board (`TradeEmbargoSystem`); the two are labelled *Political* and *Weather* wherever both appear.
- **B-2** The board shows *scope* as a label and, next to it, the fixed line *"Trade with this faction is closed for the window."* — the ledger's actual behaviour (E21).
- **B-3** A future scope-aware ledger would only change this line; the Table would not.

---

## 17. The Treaty Book (history) and a worked lifecycle

### 17.1 The Book

A pure read model over the owners' own violation and status records plus `ClauseLog`: for each instrument ever held, *when ratified, which clauses, which violations, when it ended and how*. **No new record is created for owner-held facts** (violations, penalties, guarantees); the Book joins them.

### 17.2 Lifecycle, day 100 to day 190 (one worked timeline)

| Day | Event | Owner action | Table/TT record |
|---|---|---|---|
| 100 | Corridor framework ratified | `TryRatifyTreaty` (existing) | Toll + Pass clauses enacted; `ClauseLog` ×3; minutes line |
| 104 | Salt Run passes caravanserai | convoy Checkpoint reads `ITollSchedule` → 20‰ | receipt |
| 118 | A signatory's patrol enters the DMZ | `ReportArmedPatrol` (existing) | stage: Strained |
| 121 | second patrol | violation count exceeds tolerance 1 | the owner sets the status; the Table shows Broken only if the owner says so |
| 130 | term ends | owner marks Expired | stage: Lapsed; two passes **Void** (derived) |
| 131 | Salt Run at caravanserai | toll hook: no schedule in force → data row 60‰ | Toll falls back |
| 135 | player asks a new summit | `TryScheduleSummit` (existing) | none |
| 140 | second corridor ratified | existing | clauses re-enacted (new `ratified_day`, new keys), two new passes |

### 17.3 Rules

- **T-1** The Table reads violation and status only from owners.
- **T-2** A lapse never edits a paper or a schedule; it changes what the derived reads say.
- **T-3** Renewal is **re-ratification** (an existing act) and *re-issues* Pass clause papers (DEC-PP-12); old papers stay in the ledger as Void.


---

# PART THREE — DEPTH, CATALOGS, HOOKS, ACCEPTANCE, DELIVERY

## 17b. Presentation spec (no new routed panel)

**Papers tab** (an existing faction or inventory panel, chosen at P0):

```
PAPERS                                                      Day 44
HELD (5)                     Origin    Status     Expires   Basis
 Passage pass  caravanserai  issued    Valid      d42→d60?  —
 Trade licence caravanserai  issued    Valid      d72       —
 Passage pass  Fleet harbour forged    Valid      d60       —      "The stamp is yours."
 Safe conduct  Salt corridor issued    Void       d130      Compact (ended)
 Census entry  (mirror)      mirror    Valid      —         Holdfast census
[Issue…] [Buy…] [Forge…] [Amend…] [Surrender]           Heat: Fleet port 1
```

**Audit Book** (opened from the Papers tab or the briefing line "The Holdfast office will call on day 91"):

```
AUDIT BOOK — Holdfast office (calls in 7 days)
Declared vs recorded
 Trade volume   said 40   recorded 52   gap 30%   [Set the books] [Tidy…]
 Water draw     said 6/d  recorded 6/d  gap 0%
 Headcount      said 8    roster 9      gap 12.5% [Set the books]
[Decline the audit (once a year, standing cost)]
```

**Treaty Table tab** (an existing diplomacy panel):

```
TREATY TABLE                                                Day 100
INSTRUMENTS   Provenance  Stage     Left   Family
 Compact of Non-Aggression  Summit   Settled  12d   peace
 Road Iron Charter          Regional Settled  —     trade
 Non-Aggression Pact        Diplomacy Settled 20d   peace
   "Two agreements say much the same thing. Neither knows about the other."
POSITION  Fleet port — Even   "They need your salt. They have two other partners."
TOLLS     caravanserai 40‰ (signatory 20‰)   railhead 30‰ (held)
EMBARGOES (Political)  faction_x  closed 14d  [Treaty]   (Weather board is separate)
MINUTES   "Agreed: four in the hundred… Not recorded: the north gate."
```

**Every control** is keyboard and controller reachable with visible focus; Back closes the tab; controls meet the a11y height floor already in Core; status and stage are always **words**, never colour alone. **The odds are never shown as a percent** (DEC-PP-07); a forged paper's line is qualitative.

---

## 17c. Authored content

### 17c.1 Position cards (`treaty_positions.json`, six factions; Wants / Offers / Temperament)

| Faction (E12) | Wants | Offers | Temperament |
|---|---|---|---|
| `faction_railway_guild` | steady freight, certified segments, no wildcat carriers | carrier's bills, rail certificates, halved tolls | Guild |
| `faction_hydro_barons` | scrap, coastal route safe from raiders, clean water | route access, barter discount (as `road_iron_charter`) | Market |
| `faction_salt_freeholders` | protection for their carts, salt moved on time | passage passes, market licences, a night's lodging | Market |
| `faction_ordnance_foundry` | brine pipe, coal, iodine (foundry accords) | tooling, standing, a quiet lane | Hard |
| `faction_rebuilders` | a count of the living, reports, order | settlement charters, census entries, water permits | Hard |
| `faction_central_garrison` | levies, obedience, a clear road | safe conduct, protection, patience | Hard |

Position lines (the "why" on the card) are authored per pair: *"They need your salt. They have two other partners for everything else."*

### 17c.2 Audit prose (`paper_lines.json`, by severity)

- **Clean:** "Nothing to draw a box around."
- **Noted:** "The clerk wrote two numbers on the same line and did not draw anything. It was worse."
- **Fined:** "The clerk wrote 40 and 52 on the same line and drew a small box around the difference."
- **Suspended:** "He stamped the back of it with a mark you have not seen before and put it in his own satchel."
- **Struck:** "They did not raise their voices. That was the first thing you noticed."
- **Set the books:** "You wrote the true figure over the false one and initialled it. It felt like taking a splinter out."
- **Tidy:** "The line reads clean. It looks the way a room looks after someone has tidied it."
- **Decline:** "You said no, and were thanked for your candour, which is not what it sounds like."

### 17c.3 "Not recorded" clauses (`table_minutes.json`; twelve, used at random by seeded pick per ratification)

*the north gate; whose water it was; who went first; what was said in the corridor; the second cart; the price of the fuel; the name of the man in the doorway; the reason for the delay; who paid for the lamp; what the scribe was told to leave out; the count of the dead; the weather that day.*

**Rule T-N:** exactly one "not recorded" clause per ratification, chosen by the seeded stream `minutes:<treaty_id>:<ratified_day>`; never explained.

### 17c.4 Embargo lines

- Debt: "Their trade is closed to us for fourteen days. There is a paper in a drawer that says why."
- Treaty: "Closed for thirty days by our own signature. It cannot be undone before it ends."
- Paper: "Closed for fourteen days after a pass was found to be false. Nobody has said whose."
- Audit: "Closed for fourteen days after the clerk's second visit."

---

## 17d. Edge cases and rules

- **X-1** A paper presented to an authority that does not recognise its kind is Refused (`not_recognised`), never Detected, even if forged: detection is only for recognised kinds.
- **X-2** Two papers of the same kind and scope may be held (a renewal in advance); the selector prefers the later expiry; the older remains until it expires.
- **X-3** A **holder who dies** leaves a survivor's paper *held by the shelter* as `Found` (origin becomes `found`, craft kept); it may be surrendered.
- **X-4** A caravan destroyed: its papers stay in the ledger with holder unchanged; status derived normally; they may be **found** by a raider (a hook for the convoy plan; default none).
- **X-5** The shelter with zero standing floors: `Issue` refuses with `standing_floor` and the reason line; nothing else changes.
- **X-6** Difficulty presets **never** scale Scrutiny, Craft terms, thresholds, tolls, leverage bands or audit sampling.
- **X-7** Determinism: the only randomness is (a) the check roll, (b) audit timing and sample, (c) "not recorded" clause choice, (d) coined broker names if used. All through named `CampaignStreamIds` forks; none from clock or hash order.
- **X-8** Save bounds: ≤ 48 live papers, ≤ 12 declarations, ≤ 24 audit records, ≤ 24 toll records, ≤ 40 receipts, ≤ 64 clause-log keys.
- **X-9** An authority whose faction is **defeated or dissolved** (year-two profile, plan 1 hooks) stops issuing and recognising; papers it issued become **Void** by a `IssuerActive` read (default `NullIssuerReader` = always active).
- **X-10** A treaty ratified with **no** attached clauses does nothing extra; the Table lists it and its stage only.
- **X-11** A forged `safe_conduct` (basis: a treaty) is checked against the treaty status *first*: if the basis is not in force it is **Void** and Refused deterministically before any roll.
- **X-12** Refusal is never a game-over. Every refusal leaves the caller with its owner's existing options (pay, go round, turn back).

---

## 17e. Two hundred days on the Salt Run (one worked timeline, both parts)

Fixture: the plan-2 Salt Run (caravanserai, Fleet port, depot, Holdfast), default difficulty, one forger (Skill 4).

| Day | Event | Stored change | Player sees |
|---|---|---|---|
| 12 | issues passage pass + trade licence (caravanserai), harbour pass (Fleet) | 3 papers, 18 scrap | Papers tab |
| 30 | forges harbour pass, Craft 68 (as §7.3) | 1 forged paper, 1 stock, 2 survivor-days | "The stamp is yours." |
| 42 | genuine harbour pass expires | none (derived) | Expired |
| 44 | Fleet check on forged pass: Accepted | ClauseLog none; chronicle key `firstaccept:auth_fleet_port` | first-accept line |
| 58 | same forged pass: Detected | paper Revoked; Heat 1; standing −20; embargo 30d `paper:…` | detected line |
| 60 | Fleet trade closed 30 days (ledger) | ledger record | Embargo board: Paper chip |
| 91 | Holdfast office audits: volume gap 30% | AuditRecord; fee 6 scrap | Fined line |
| 100 | Corridor ratified (existing) | 3 clause keys; toll 40‰/20‰; 2 passes | minutes |
| 104 | toll at caravanserai read via hook | receipt | 20‰ applied |
| 130 | corridor lapses | none | passes Void; toll back to 60‰ |
| 140 | second corridor | new keys, 2 new passes | minutes, "not recorded: the weather that day" |
| 190 | Fleet audits (45-day cadence; Heat 1) | AuditRecord Clean | Clean line |

---

## 17f. Interaction with existing owners (what reads what, what requests what)

| Interaction | Direction | Note |
|---|---|---|
| Faction standing | PP requests deltas | through the resolved write owner (E15); no new standing store |
| `FactionEmbargoLedger` | PP and TT request | `TryAddEmbargo` only; source ids `paper:`, `audit:`, `treaty:` |
| Inventory | PP/TT request | fees, `paper_stock`, tolls, fines — through existing commands |
| Assignment/duty | PP requests | forger day, clerking day |
| Treaty owners | TT reads | status, terms, violations; ratification event triggers clause enactment |
| `CensusClaimSystem` | PP reads | headcount line; `census_entry` mirror |
| Iron Road ledger | PP reads | `rail_certificate` mirror |
| `HouseServiceState` | PP reads | `muster_paper` mirror |
| `JusticeSystem` | optional sink | `IJusticeSink.OnForgeryDetected` (default no-op) |
| Convoy Checkpoint | calls IPaperGate, ITollSchedule | each keeps its own consequence |
| Chronicle | append-only | idempotent keys |

No owner changes behaviour. The only writes are to the Paper Ledger, the Toll Schedule state and the Clause Log, plus **requests** the owners already support.

---

## 18. Catalog specs and validator rules

| File | Rows | Key fields |
|---|---|---|
| `paper_kinds.json` | 12 | `kind_id`, `label`, `issuers[]`, `lifetime_days`, `scope_kind`, `fee`, `standing_floor`, `mirror_of?` |
| `authorities.json` | 6+ | `authority_id`, `faction_ref`, `scrutiny`, `issues[]`, `recognises[]`, `audit_cadence_days`, `detected_response{standing_delta, embargo_days, revoke}` |
| `audit_line_templates.json` | 8 | `template_id`, `declared_source`, `observed_read`, `bands{clean,noted,fined,suspended}` |
| `forgery_inputs.json` | ~10 | `input_id`, `craft_delta`, `item_id`, `cap_without_seal` |
| `paper_lines.json` | ~60 | `outcome`/`severity`, `temperament`, `text` |
| `treaty_clauses.json` | ~20 | `clause_id`, `kind` (toll/embargo/pass), `family`, `params`, `family_map` |
| `treaty_positions.json` | 6 + pair lines | `faction_id`, `wants`, `offers`, `temperament`, `pair_lines[]` |
| `toll_schedules.json` | ~8 posts | `post_id`, `authority_id`, `base_permille`, `floor`, `ceiling`, `holdable` |
| `table_minutes.json` | ~40 | minutes lines by outcome; the 12 "not recorded" clauses |

**Validator rules (added to the integrity pipeline; row-level failure output):**

- **PP-1** every `authority.faction_ref` resolves in `faction_lore.json` or `crossing_factions.json`; every `issues`/`recognises` kind exists in `paper_kinds.json`.
- **PP-2** every paper `scope_kind` resolves to an existing route/zone/faction/site/segment id space; mirrors reference an owner that exists.
- **PP-3** `scrutiny` 0–100; `audit_cadence_days` ≥ 7 or 0 (event-only); `detected_response.embargo_days` ≥ 0.
- **PP-4** every audit template's `observed_read` names a registered probe; bands strictly increasing.
- **PP-5** forgery `craft_delta`s bounded −20..+20; every `item_id` resolves in `items.json`.
- **PP-6** every paper kind and authority has at least 3 clerk lines per outcome (Accepted, Queried, Refused, Detected).
- **PP-7** all text ≤ 200 chars; no placeholder tokens; no real places, agencies or people.
- **TT-1** every clause `family` is in the closed family list; every toll `post_id` resolves to a convoy Checkpoint row or a holdable post.
- **TT-2** toll `floor <= base <= ceiling`; permille 0–1000.
- **TT-3** every embargo clause `days` ≤ the family's term (or is marked `term_bounded`).
- **TT-4** every faction in `treaty_positions.json` has all three rungs for every clause kind it can be offered.
- **TT-5** exactly 12 "not recorded" clauses, unique, ≤ 48 chars.
- **TT-6** tone deny-list: no line attributes mind or wish to *the treaty*, *the table*, *the paper* or *the stamp* (only to people); small verb deny-list (`wanted`, `decided`, `knew`, `forgave`) unless the subject is a person.

---

## 19. Acceptance criteria

**PP**

- **PP-A1** `PaperStatus` follows §5.2 for every combination (parametrised), including **Void** from a treaty basis.
- **PP-A2** `Issue` enforces fee, standing floor and supersession; `Forge` and `Amend` consume `paper_stock` and a survivor-day; double amendment is refused.
- **PP-A3** `Craft` matches the §7.3 worked examples exactly (68, 22, 60).
- **PP-A4** `Detect` matches the §8.4 table; the roll is stable across save/load and repeat presentation on one day.
- **PP-A5** The decision order of §8.2 holds: authentic papers are never Detected; dead papers are Refused without a roll; Queried band behaves as specified.
- **PP-A6** Detected applies exactly the §8.5 consequences through owner calls and nothing else.
- **PP-A7** Audit: sample size follows Scrutiny; findings follow bands; the §10.6 example reproduces (Fined, then Noted after set-the-books, then Noted after a passed tidy).
- **PP-A8** `NullPaperGate` leaves every caller unchanged (parity).
- **PP-A9** Round-trip save; legacy save loads with no papers; existing treaty, embargo, caravan, justice and census tests unchanged.

**TT**

- **TT-A1** The Table fold lists every instrument from the three owners with correct provenance and never mutates an owner.
- **TT-A2** Life stage follows §12.3 across boundaries (parametrised).
- **TT-A3** Leverage matches §13.3 and yields the right band and rung; a missing read yields `0` and "cannot say".
- **TT-A4** Clause enactment is exactly-once per `ClauseLog` key, across save/load; ratification is untouched.
- **TT-A5** Effective toll follows §15.3–15.4, including exemptions, floors, ceilings and the `max(1, …)` rule; `NullTollSchedule` returns the authored row.
- **TT-A6** An embargo clause calls `TryAddEmbargo` idempotently, is bounded by the term, and the Board shows source chips; the two embargo boards never merge.
- **TT-A7** A lapsed treaty makes dependent `safe_conduct` papers Void with no write.
- **TT-A8** Round-trip save; legacy save loads with no schedules or log; existing treaty and embargo tests unchanged.

---

## 20. Cross-plan hooks (ship dark; `Null*` defaults)

| Hook | Direction | Default | Purpose |
|---|---|---|---|
| `IPaperGate` | callers to PP | `NullPaperGate` (Accepted) | one present-a-paper seam |
| `ITollSchedule` | Checkpoint to TT | `NullTollSchedule` (authored row) | effective toll |
| `ITollPostReader` | shelter-held posts | `NullTollPostReader` (none) | player-set tolls |
| `IJusticeSink` | Detected forgery | no-op | optional justice route (DEC-PP-06) |
| `IIssuerReader` | is the authority active | `NullIssuerReader` (true) | dissolved factions |
| `IConscienceSink` | a forger's day | no-op | optional survivor line/stress hook |
| `IBrokerReader` | brokers and their quality | `NullBrokerReader` | Buy |
| Convoy Wars (plan 2) | Checkpoint calls gate and toll hook | none | first consumer |
| Iron Road (plan 1) | rail certificate mirror; Railhead as holdable post | none | read-only |
| Long Line Freight | carrier's bill | none | consignment paper |
| Siege Year (plan 1) | the Terms Year as a Table instrument | none | read-only |
| Inside a House (plan 2) | muster paper mirror | none | read-only |

Every hook is optional; the plan is complete with all defaults.

---

## 21. Packages

| Pkg | Scope | Depends |
|---|---|---|
| P0 | Premise audit: E4, E15, E16, E17, E18, E19, E21; resolve standing write owner; choose save homes; record findings | none |
| P1 | Catalogs and loaders (paper + treaty) + validators PP-1..7, TT-1..6 | P0 |
| P2 | `PaperLedger` + `PaperStatus` + Issue/Buy + save DTO | P1 |
| P3 | `Forgery` (Craft) + `PaperCheck` (Detect, roll, outcomes) + consequences via owners | P2 |
| P4 | `IPaperGate` host seam (`INT`) + Papers surface | P3 |
| P5 | `AuditEngine` + Audit Book + `AuditHistory` | P3 |
| P6 | `TreatyTableView` + Position + Leverage + life stage | P1 |
| P7 | `TollSchedule` + `ITollSchedule` + receipts | P6 |
| P8 | `ClauseLog` + clause enactment (toll, embargo, pass) on ratification event (`INT`) | P7, P2 |
| P9 | Embargo Board + Treaty Table surface + save parity + tone review + handoff | all |

PP (P2..P5) and TT (P6..P7) may proceed in parallel after P1; P8 is the join (Pass clause issues papers).

---

## 22. Decision register (all unsigned; foreman/user signature required)

- **DEC-PP-01** A single Paper Ledger is the one new stored owner for PP. *Recommend yes.*
- **DEC-PP-02** Save home for the Paper Ledger. *Decide at P0.*
- **DEC-PP-03** `rail_certificate`, `muster_paper`, `census_entry` are read-only mirrors, not copies. *Recommend yes.*
- **DEC-PP-04** Scrutiny values, audit bands, cadence. *Tunable data.*
- **DEC-PP-05** Craft terms and caps. *Tunable data.*
- **DEC-PP-06** Optional `Forgery` justice route (additive enum value on `JusticeSystem`). *Default: none; unsigned.*
- **DEC-PP-07** Odds never shown as a percent. *Recommend yes.*
- **DEC-PP-08** No new routed panel; extend existing surfaces.
- **DEC-PP-09** A `Found` paper may cite a lore document as basis. *Recommend yes, read-only.*
- **DEC-PP-10** Bought papers are genuine-as-far-as-checked unless the broker is flagged. *Recommend yes.*
- **DEC-PP-11** Declining an audit: once per authority per year, at a standing cost. *Recommend yes.*
- **DEC-PP-12** Renewal re-issues Pass clause papers (old stay Void). *Recommend yes.*
- **DEC-PP-13** An authority's Detected response may include a faction-wide embargo (the ledger is scope-blind) or none, per row. *Per-row data.*
- **DEC-PP-14** Paper events write standing through `FactionWarSystem.ModifyStanding`, copying the `Main.PsyOps` branch rule for PRPF/military (§2b).
- **DEC-PP-15** "Forger's hand" is a composite read of `skill_steady_hands` and `skill_crafting`; no skill is added (§2b).
- **DEC-TT-01** Table is a fold with provenance; no fourth treaty owner. *Recommend yes.*
- **DEC-TT-02** Toll schedules are a small stored state; the authored row is the default.
- **DEC-TT-03** Clause enactment reacts to the ratification event; ratification is untouched.
- **DEC-TT-04** Leverage weights and band cuts. *Tunable data.*
- **DEC-TT-05** Additive `TryLift(sourceId)` on `FactionEmbargoLedger`. *Default: none; embargoes run out their days.*
- **DEC-TT-06** Player-set tolls only at shelter-held posts, within floor and ceiling.
- **DEC-TT-07** Two active instruments in one family produce a plain footer line only.
- **DEC-TT-08** No new routed panel; extend the existing diplomacy surface.
- **DEC-TT-09** If clause values cannot attach to ratification without changing the owner, the ladder is advice lines only.

---

## 23. Test plan and risks

**Focused tests (`bin/run-scoped-tests`), 40–50 total:**

- Status: one parametrised table over Revoked/Void/Expired/Expiring/Valid.
- Issue/Forge/Amend: inputs consumed, refusals with reasons, no double amendment.
- Craft: the three worked examples (68, 22, 60) and cap-without-seal.
- Check: decision order, Detect table, roll stability across load and repeat, Queried band.
- Consequences: Detected applies exactly standing, embargo request, heat, revoke, chronicle.
- Audit: sample size by Scrutiny, bands, the §10.6 example (three variants), decline.
- Fold: three owners, provenance, no mutation, overlap footer.
- Life stage: boundaries.
- Leverage: the §13.3 example, missing-read path.
- Clauses: exactly-once enactment, save/load, toll/embargo/pass.
- Toll: effective rate, exemptions, floors and ceilings, rounding rule, Null default.
- Embargo board: source chips; weather board separate.
- Void seam: lapse makes `safe_conduct` Void with no write.
- Validators: rows PP-1..7, TT-1..6 (catalog-level, row failure output).
- Save: round-trip and legacy load for the three stored DTOs; parity guard runs existing treaty, embargo, summit, diplomacy, justice and census tests unchanged.

| Risk | Likelihood | Mitigation |
|---|---|---|
| Three treaty owners collide in the Table | Med | Fold with provenance; debt note; never edit owners |
| Ledger is scope-blind and cannot lift early | High | State it (E21); clause bounded by term; DEC-TT-05 |
| Forgery feels like a minigame | Med | Deterministic craft; day of work; no dice UI |
| Players re-roll checks by re-presenting | Med | Roll seeded by (paper, authority, day) |
| Audits feel like punishment | Med | Only declared lines; honest correction cheap; decline once a year |
| A second embargo/standing/trade store appears | Low | Requests only; Rule 5 review at P9 |
| Papers plan ships without a caller | Med | `NullPaperGate`; the convoy Checkpoint is first consumer, hook-gated |
| Save bloat | Low | Bounds X-8 |

---

## 24. Expansion backlog

- Scope-aware embargo ledger (with `TryLift`), then per-route embargoes.
- Forged papers as clues (the Long Inquest may read them; hook name reserved, not built).
- A registry of stamps and seals as collectibles.
- Couriers and letters carrying papers (letter-delivery seam).
- Audits of *other* shelters (a treaty partner's books).
- Counter-audit: the player audits an authority's toll receipts.
- Paper-making as a gameplay chain (the paper catalogs are lore today).
- Treaty arbitration by a neutral Current (crossing arbitration seam).
- Multi-party summits with side letters.
- Player-authored clauses (bounded text, sanitised).

---

## 25. Open Mysteries and Deliberate Silence

- Who signs the authorities' papers is never stated.
- Every ratification leaves one clause "not recorded", never revealed.
- Whether an audit was random is never shown; the sample reads as a clerk's choice.
- Where the seal tool came from is never explained.
- Whether the forger's hand is admired, feared or simply used is left to the survivors' own lines.
- Whether two treaties that say the same thing were signed by people who knew is never asked.
- What the blank line in the minutes would have said, had it been written, is the table's secret.

---

## 26. Pre-flight, verification and stop conditions

**Pre-flight (before any edit):** read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`; confirm claims for every path in section 4; complete P0 and record findings.

**Verification:** run only focused targets via `bin/run-scoped-tests` for the changed files; run `bin/ashfall-dev validate-config` for the new catalogs; a Godot headless check only if a panel or the gate seam is touched (15 FPS); never the full suite without `RUN FULL TESTS`.

**Stop and report to the foreman if:**

- a path in section 4 is claimed by another owner;
- P0 shows there is no single write owner for a standing delta (E15) and a request cannot be routed without a new authority;
- clause enactment cannot hang off the ratification event without editing a treaty owner;
- any change would alter a treaty term, standing, embargo, trade or crime value directly;
- any line would attribute mind or wish to a treaty, a table, a paper or a stamp.

**Handoff:** outcome, files, contract, commands and results, limitations, shared paths intentionally untouched, per `AI_AGENT_WORKFLOW.md`. On integration, mark this file FULLY INTEGRATED at the top (multiple times) and move it to `.ai/plans/integrated/<category>/`.
