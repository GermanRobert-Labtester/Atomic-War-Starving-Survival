# ASHFALL — SHELTER GOVERNANCE: THE ASSEMBLY
### Laws, courts and internal opposition · Who decides, who is judged, and who is organising in the corridor

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/shelter-governance-2026-09-29.md`
**Family:** "New ways to play" — `docs/expansions/expansion_new_ways_to_play_index.md`.
**Related, not duplicated:** Plan 159 (Shelter governance & blocs), Plan 193 (Wasteland justice), Plan 53 (ambition governance), Expansion 04 *Nobody's Charter*, Year Two's *Council of Succession* (P4c). This plan is the **player-facing Assembly** over those; §2.1 lists the reconciliations.
**Tone lock (inherited):** cold, exhausted, human, restrained. No real countries, laws, people or copied text. No glorified violence; punishments are consequences, not spectacle.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"A shelter does not need a government. It needs a room where it is possible to lose an argument
> and still stay inside."*
>
> Five political authorities already work in this game and none of them talk to each other. That is
> not a bug; it is what a shelter looks like when it is governed by institutions each invented to
> solve exactly one emergency.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The shelter already has a great deal of politics, and it has never spoken to itself. There is a **PoliticsSystem** with approval, legitimacy, elections, martial law and a *coup risk*. There is a **PolicySystem** with rations, curfew and emergency override. There is a **ShelterGovernanceEngine** with four ideological blocs, grievance in basis points, civil disputes, a stability rating. There is a **JusticeSystem** that holds trials with evidence, verdicts and six levels of punishment. There is a **LeadershipSystem** with succession and challenge. Five systems, five saves — and a player who meets them as five unrelated panels.

Shelter Governance makes them one *experience*: **the Assembly** — a place, a calendar, a set of named people. Laws are things the shelter *enacts and repeals*, not rows in a catalogue. Courts are people — an advocate, a jury, a defendant with a bunk. Opposition is *organised*: it has leaders, it escalates, and when it wins, the shelter changes hands or splits.

The promise: **you will learn to govern by consent, by fear, or by both, and you will feel each one wear.**

The **Assembly** is the room. It adds no authority — it only gives the five a shared vocabulary, a
book of statutes, a court with names, and a ladder of dissent that is **announced one sitting
ahead** before it is ever climbed.

The ladder — murmur → petition → walkout → sit-in → schism or challenge — is a slow-burning fuse,
and the announcement rule means the player always sees the shape of the trouble before the
trouble. The dread is in the seeing. And roles shape verdict *reception*, never the verdict: the
decision belongs to the justice owner. What the Assembly changes is what a verdict feels like to
receive.

### 1.2 Pillars

1. **Law is enacted.** Statutes are chosen from a book; each carries a cost in attention and a cost in consent.
2. **Courts are people.** A trial has an advocate, jurors and a defendant with relationships.
3. **Opposition is a ladder.** Murmur → petition → walkout → sit-in → schism or challenge. Each rung is visible before it is taken.
4. **Legitimacy and fear trade.** The existing law catalogue already scores both; the Assembly makes the trade-off legible.
5. **One vocabulary.** A "scope" means the same thing to the policy book, the blocs, and the law.
6. **Ship dark.** With no assembly data present, the five systems behave exactly as today.

### 1.3 Not this

Not a new political system. Not a legal-code simulator. Not real-world ideology mapped onto factions. No new save section. No new routed panel.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | **PoliticsSystem**: leader, governance mode (Appointed/Democratic/MartialLaw), approval 0–100, legitimacy 0–100, active policies, days until election, `isMartialLaw`, disputed election, **`coupRisk` 0–1**, elections/coups counters, history; `EnactPolicy`, `RepealPolicy`, `HoldElection`, `DeclareMartialLaw`, `CalculateCoupRisk`, `AdvanceDailyPolitics`, `ResolveCoup`; 6 authored political policies (cost, legitimacy impact, repeal cooldown, supporter/opponent tags). Section `settlement_politics`. | `Narrative/PoliticsSystem.cs`; `political_policies.json`; `src/Main.Politics.Integration.cs` L42–67 | LIVE |
| F2 | The daily politics call passes **`guardDeficiency = 0`** (hard-coded), so coup risk ignores guard shortage. | `src/Main.SubsystemComposition.cs` L488 | **GAP / VERIFY** |
| F3 | **PolicySystem**: scoped policies with options, attention and reversal cost, proposer rules (`leader_only`/`open_council`), decision method (`executive`/`consensus`). **Only three scopes authored**: `rations`, `curfew`, `emergency_override`. | `Governance/PolicySystem.cs`; `policies.json` | LIVE (thin) |
| F4 | **ShelterGovernanceEngine**: 4 blocs (Security & Order Vanguard, Egalitarian Commons, Free Pioneers, Heritage Archive) with influence and grievance in basis points; survivor membership; **policy consent** evaluation (supporting/opposing blocs, net consent, projected grievance); civil disputes (5 types, 4 resolutions); stability rating (grievance ≤ −40, open disputes ≤ −25, leadership modifier); daily tick. Section `shelter_governance`. | `Governance/ShelterGovernanceEngine.cs`; `shelter_governance_blocs.json`; `src/Main.ShelterGovernance.cs`; daily owner phase 5 | LIVE |
| F5 | **Vocabulary mismatch**: blocs list ~21 scope words (`curfew`, `ration_triage`, `open_admission`, `equal_rations`, `forced_labor`, …); the policy book authors `rations`, `curfew`, `emergency_override`. Only `curfew` overlaps. `EvaluatePolicyConsent` matches by exact string, so most enacted policies show **zero supporters, zero opponents, and "majority consent"**. | `shelter_governance_blocs.json`; `policies.json`; `ShelterGovernanceEngine.EvaluatePolicyConsent` L367–410 | **GAP** (by reading; VERIFY by test) |
| F6 | **JusticeSystem**: crime report → evidence with weight → trial with `TrialDecision {verdict, punishment}`; verdicts NotGuilty/Guilty/Inconclusive; six punishments (Warning, Restitution, Labor, Confinement, Banishment, Execution); crime types Theft, Assault, Murder, Hoarding, Sabotage, Desertion. Host-wired; `JusticeTribunalPanel`. | `Narrative/JusticeSystem.cs`; `src/Main.Justice.Integration.cs`; `src/UI/JusticeTribunalPanel.cs` | LIVE |
| F7 | **Four authored laws** (Theft, Assault, Sabotage→Banishment, Murder→Execution) with legitimacy impact, fear impact, deterrence and doctrine tag. **Hoarding and Desertion have no law.** | `wasteland_laws.json` | LIVE / GAP |
| F8 | **LeadershipSystem**: 5 policies (meritocratic, democratic, hereditary, elder council, martial challenge — with `challenge_threshold`); elect/designate/step down/successor/deputy; `ResolveChallenge`. Host setter exists. | `Survivors/LeadershipSystem.cs`; `leadership_policies.json`; `src/Main.SurvivorSocial.cs` L173 | LIVE (VERIFY daily integration) |
| F9 | Bloc grievance and stability have **no output beyond a rating**; nothing calls a bloc "walk out", "sit in", "split", or "challenge". | grep `Governance/` for strike/coup/revolt = 0 | **GAP** |
| F10 | `ShelterReputationSystem` scores dimensions and tags with evidence. | `Reputation/ShelterReputationSystem.cs` | LIVE |
| F11 | Year Two plans a Council of Succession ledger (`designations[]`) in an existing owner. | `.ai/plans/year-two-the-long-thaw-2026-09-29.md` P4c, DEC-Y2-07 | PROPOSED (other plan) |
| F12 | Selftests: shelter-governance host CLI and justice/politics verbs exist. | `src/Host/HostCli.ShelterGovernance.cs` | LIVE (VERIFY args) |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **Five governance owners.** Systems win: none is merged or replaced. The Assembly is a **reader and router**: it reads all five, writes only through their public commands, and stores only what none of them can (opposition state, statute list, precedent).
- **Scope vocabulary (F5).** Systems win: the policy book's scopes are the shelter's real commands, so the fix is a **scope map** (bloc scope word → one or more policy scopes) as data, not renaming either side.
- **Coup risk ignores guards (F2).** A one-line hard-coded zero. This plan does not fix it silently; DEC-SG-08 asks whether the Assembly may supply the real value.
- **Year Two's Council of Succession (F11).** Overlap risk. Boundary: Year Two owns *who is designated*; the Assembly owns *whether the shelter accepts it*.

---

# PART II — THE STORY

## 3. The Assembly

### 3.1 The calendar

The Assembly **sits** on a rhythm the shelter chooses (weekly by default). A sitting has an **agenda**: pending proposals, open disputes, open trials, opposition petitions. The player sits as the leader (or as the leader's proxy after succession) and decides in the order the agenda presents. A skipped sitting is a decision; opposition notices.

### 3.2 The Statute Book

A **statute** is a named law the shelter can enact: *what it forbids, what punishments it permits, what it costs.* The four authored laws become the **starter book**; the plan adds the two missing crime types (Hoarding, Desertion) and 8 more (curfew breach, contraband, unlawful admission, waste of water, false accusation, abandoning a post, harbouring an infiltrator, refusing quarantine).

Each statute carries: attention cost, consent (via the scope map), legitimacy impact, fear impact, **repeal cooldown**, and a *doctrine* (Merciful, Stern, Practical). Enacting a statute the blocs oppose raises grievance by the existing projected amount.

### 3.3 The Court

A trial becomes **people**:

| Role | Who | What they do |
|---|---|---|
| **Presiding** | the leader or delegate | decides verdict/punishment among those the statute permits |
| **Advocate** | a survivor with rhetoric/empathy | argues for the defendant; a good advocate can raise reasonable doubt |
| **Prosecutor** | a survivor with investigation | assembles evidence |
| **Jurors (3–5)** | survivors, weighted by bloc | their bloc membership sways verdict *reception* |
| **Defendant** | the accused | has relationships that carry into grievance |

Evidence still comes from the existing evidence model (and, when they exist, *The Quiet War*'s dossiers). The court does not invent verdict rules: it **shapes reception** — legitimacy and fear consequences — of a verdict the existing trial produced.

### 3.4 The Opposition Ladder

Each bloc has a **leader** (a named survivor) and a **rung**:

| Rung | What the bloc does | Threshold (data) |
|---|---|---|
| **Murmur** | Grievance in the corridors; small morale drag. | grievance ≥ 25% |
| **Petition** | Presents a demand to the Assembly agenda. | ≥ 40% |
| **Walkout** | Work refusal on named duties (duty roster). | ≥ 60% |
| **Sit-in** | Occupies a facility; blocks its use for days. | ≥ 75% |
| **Schism or Challenge** | The bloc leaves (a faction is born, survivors depart) — or, if the leadership policy permits, **challenges** the leader (existing challenge machinery). | ≥ 90% |

Every rung is announced a sitting ahead. **Concessions** (repeal, a statute, a reform, a punishment reduced) move grievance down; **repression** (martial law, a confinement) moves it down *now* and raises fear and coup risk *later*. The game does not choose.

### 3.5 Legitimacy versus fear

Every statute and verdict already has a **legitimacy impact** and a **fear impact**. The Assembly keeps a two-line ledger — *what the shelter obeys because it believes, what it obeys because it is afraid* — and shows both. A shelter that runs mostly on fear is stable and brittle; one that runs on legitimacy is slower and holds.

### 3.6 Precedent

A resolved case leaves a **precedent** (a small authored tag: *mercy shown*, *hoarder exiled*, *accuser fined*). Precedents shift the consent of *similar* future statutes and verdicts by bounded amounts — a shelter remembers how it has ruled.

### 3.7 Succession

Leadership policy determines what the Assembly *does* when the leader falls (election, designation, elder council, martial challenge). Year Two's Council of Succession supplies the *designation*; the Assembly decides whether it is accepted or contested. The two never share a ledger.

## 4. Four stories

**The Water Sit-in.** The Egalitarian Commons sits in the cistern room. The player can concede an equal-rations statute (Security Vanguard's grievance jumps), break the sit-in (coup risk climbs), or wait (a child gets ill in the meantime).

**The Hoarder's Trial.** The shelter has no law against hoarding. The player enacts one, in a hurry, and tries the case under it the next day. Some jurors notice.

**The Advocate.** A survivor the player likes is accused of desertion. The player has to decide whether the advocate is allowed to be good at her job.

**Schism.** A bloc leaves — with nine people, a cart, and the Heritage Archive's books. The shelter is stable again. It is also poorer by nine people and a library.

## 5. Voice samples

- *Agenda:* "Sitting nine. Pending: equal rations (Commons), curfew review (Vanguard), one trial (hoarding). Absent: the Free Pioneers."
- *Petition:* "We aren't asking for anything you haven't already promised. We are asking for it in writing."
- *Court:* "The jury is out. Two of them are the defendant's neighbours. One of them is not speaking to the other two."
- *Precedent:* "The shelter remembers that the last hoarder was fined and kept. The next will expect the same."
- *Schism:* "They left at first light with the cart. Nobody stopped them. That was the decision."

## 6. Content plan

- **W1 — Statute book:** 12 statutes × (text, doctrine, consent line, repeal line) ≈ 48 lines.
- **W2 — Court:** 5 roles × 6 lines (30); jury reception lines by bloc (16).
- **W3 — Opposition:** 4 blocs × 5 rungs × 2 lines (40) + leader voices (24).
- **W4 — Precedents:** 20 tags with two lines each (40).
- **W5 — Sittings:** agenda intros, skipped-sitting lines (24).

## 7. Non-goals (restated)

No new political authority. No new save section (nested in `shelter_governance`, DEC-SG-02). No new routed panel (DEC-SG-06). No change to trial verdict rules. No real-world ideology labels.

## 8. Risks

- **Five systems disagree.** *Bound:* the Assembly writes only through public commands; a consistency probe compares approval/legitimacy/stability each sitting.
- **Scope map hides a bug.** *Bound:* P0 proves F5 by a failing-then-passing test before the map is added.
- **Opposition frustrates players.** *Bound:* every rung is announced; concession is always possible; nothing is a surprise ending.
- **Tone.** *Bound:* punishments are consequences, not spectacle; Execution stays a rare, weighty, authored path.

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** A shelter does not need a government; it needs a room where losing an
argument is survivable. The Assembly adds no authority — it gives five owners that never spoke a
shared vocabulary, and its single innovation is *announcing trouble one sitting early enough to be
believed about it*. That scheduling rule is what separates politics from weather.

**What the expansion leaves lying around.**

> "Motion, carried. Two blocs registered grievance — two sets of people now keeping a different count."

> "Minutes margin, one word: *acceded*. The minutes are neutral. The margin is not part of the minutes."

> "Reception note after a verdict. The verdict is untouched; this is the room's receipt of it."

**Scenes the player may piece together.**

> "The walkout is announced for the next sitting. We slept badly tonight instead of next week — which the rule does not mention."

> "A schism removes named survivors through existing departure paths. The room is then a room with fewer chairs and the same agenda."

**Held silences (texture — the register below is unchanged).**

- What the opposition wants beyond its rung. A named survivor and a rung are recorded; motive is never authorised.
- What the four laws' missing pair would have covered. Two crime types have none; the gap is real and its origin unrecorded.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What the blocs actually believe.** Twenty-one scope words and grievance basis points. Doctrine is
not authored and real-world labels are forbidden.

**Why `guardDeficiency` arrives as zero.** A live gap. Until the fix is signed, the zero stands
unexplained.

**Is a precedent binding.** Bounded, decaying modifiers are applied. The legal weight of the word is
asserted by the fiction and not by the engine.

**Where the schism's departed go.** Survivors are conserved through existing departure paths and then
stopped. The departed are not tracked by design.

**Why there are exactly four laws.** Two crime types have none. The gap is real and its origin is not
recorded.

**Who decides who sits on a jury.** Roles are assigned from survivors with a seeded draw. No fiction
of appointment is authored.
