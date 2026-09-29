# ASHFALL — THE RECONSTRUCTION TREE
### Research and rebuild lost knowledge · What is known is only what someone still knows

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/reconstruction-tree-2026-09-29.md`
**Family:** "New ways to play" — `docs/expansions/expansion_new_ways_to_play_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. No magic; no real institutions, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The research tree in ASHFALL is a shopping list. Sixty-two nodes across six categories; you pick one, wait a handful of days (never more than eighteen), and a thing is *known*. It stays known forever. Nobody has to teach it, nobody can forget it, and a death in the workshop costs a pair of hands but never a page.

The world the game describes is one in which the opposite is true. **Knowledge is a thing people carry, and people die.** A water engineer who is the only one who knows how to service the still is not a statistic; she is a single point of failure with a name. The Reconstruction Tree makes that true in play: knowledge has **bearers**, **fragments**, and a **fragile middle**, and *rebuilding what was lost* is a campaign, not a queue.

The player's promise: **you will come to count what the shelter knows the way you count food — and to know who to keep alive, who to make teach, and what to write down before it is too late.**

### 1.2 Pillars

1. **Knowledge has bearers.** A capability is held by people (a skill), by texts (a manual), and by practice (a workshop). Lose all three and it is gone.
2. **Lost is a state, not a lock.** Some knowledge the world *had* and has forgotten. It comes back from fragments — a salvaged manual, an archive, an elder's testimony, a diver's find.
3. **Rebuilding is uncertain.** A reconstruction can fail, and a failure teaches something.
4. **Teaching is play.** Apprentices, classroom hours, and the radio spread what is known; that is the defence against loss.
5. **Ship dark.** Existing research completes and stays known exactly as today unless a node is flagged bearer-dependent.

### 1.3 Not this

Not a second research system. Not a punishing "tech rot" applied to existing saves. Not a crafting-recipe sprawl. No new currency.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | **62 knowledge nodes** in six categories (engineering 16, survival 12, medical 10, science 9, combat 8, scavenging 7); each has `days_to_complete` (max 18), `prerequisites`, `breakthrough_item`. **31 are roots** — the tree is wide and shallow. | `research_knowledge.json` | LIVE |
| F2 | 30 unlocks (items 8, shelter 7, recipe 4, expedition 4, combat 4, medical 3) bridge a completed node to a consumer. | `research_unlocks.json`; `Research/ResearchUnlockBridge.cs` | LIVE |
| F3 | `ResearchSystem`: one **active** research at a time, days remaining, eligibility, available/locked/completed lists, manual unlocks, research points (add/spend), blueprint progress with `discoveryState`, save/restore; section `research`. | `Research/ResearchSystem.cs`, `ResearchState.cs`; `Save/SaveSectionRegistry.cs` L138 | LIVE |
| F4 | A capability is `IsManualUnlocked(id)`; 19 call sites read it. **Nothing revokes it.** | grep `HasCapability(`/`IsManualUnlocked(` | LIVE (19 refs) / **GAP: no loss** |
| F5 | Acquisition sources are enumerated (direct research, library manual study, autopsy finding, workshop reverse-engineering, narrative reward, field discovery) but the record is metadata only. | `Research/KnowledgeAcquisitionSource.cs` | LIVE (metadata) |
| F6 | Tech salvage: pre-war technologies with complexity, research points, scrap yields, possible blueprints, required tags/skill discipline, base success, **catastrophic-failure chance**, preservation value, research notes. | `Research/TechSalvageCatalog.cs`; `tech_salvage.json` | LIVE |
| F7 | Pre-war archives with decryption projects (progress, researcher, solvent); archive desk, library study, cultural tomes, archive inks. | `Research/PrewarArchiveDecryptionSystem.cs`; `prewar_archives.json`, `library_manuals.json`, `cultural_archive_tomes.json`; sections `prewar_archives`, `library_study`, `archive_desk` | LIVE |
| F8 | **Skills decay to dormant** and can be stopped bunker-wide by an Archivist perk. | `Survivors/SkillProgressionSystem.cs` L56, L71, L240–257 | LIVE |
| F9 | Education: teacher + subject assignment, daily sessions, graduation, a **shelter knowledge** counter (`AdjustShelterKnowledge`); apprenticeship/curriculum engine (literacy, vocational certification, manual transcription yield). | `Education/SurvivorEducationSystem.cs`, `ApprenticeshipCurriculumEngine.cs` | LIVE |
| F10 | Survivor death runs through a legacy system. | `Survivors/SurvivorDeathLegacySystem.cs` | LIVE |
| F11 | UI: `ResearchPanel`, `ResearchAtlasPanel`, `LibraryStudyPanel`, `ArchiveDeskPanel`. | `src/UI/` | LIVE |
| F12 | Research is host-wired (research host session, unlock bridge, several composition files). | `src/Host/ResearchHostSession.cs`; `src/Main.ResearchUnlock.cs` | LIVE |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **"Rebuild lost knowledge" vs "research never lapses" (F4).** Systems win: the completed-node capability stays the authority. Loss is **opt-in**: only nodes a catalog flags as *bearer-dependent* can lapse, so no existing save changes behaviour.
- **One active research (F3).** Parallel reconstruction is desirable but changes the owner. **DEC-RT-04** asks whether to add a bounded *bench* count inside `ResearchSystem` or to keep one active project and make *fragments* the parallel track.

---

# PART II — THE STORY

## 3. Three ways to know

Every bearer-dependent node is held in up to three ways:

| Held in | What it is | It is lost when |
|---|---|---|
| **A person** | a survivor whose relevant skill is above dormant | they die, leave, or the skill decays |
| **A page** | a manual/tome in the archive desk or library | the page burns, floods, or is never transcribed |
| **A practice** | the workshop *still doing it* (a recurring use) | nobody has used it for a season |

A node with **two or more** holds is **Secure**. With **one** it is **Fragile**. With **none** it is **Lapsed** — the shelter *had* it and does not. Lapse is a state on top of the completed node, and it is reversible.

## 4. Fragments and Reconstruction

**A fragment** is a small, named, authored piece of a lost node: a stained diagram, a half-remembered ratio, a dead man's margin note, a diver's photograph of a control panel. Fragments come from salvage, archives, elders, expeditions, dives (*The Drowned Coast*), and the radio (*Radio Free Ashfall* mailbag).

**A reconstruction project** is the ordinary research project with three additions:
1. It needs **fragments** — *n* of *m* — before it can begin.
2. It needs a **reconstructor** with the right skill discipline.
3. It ends in a **trial**: a check that can fail. A failed trial does not lose the fragments; it costs days and may teach a *lesson* (a small permanent bonus to the next attempt). A **catastrophic** failure (a small authored chance, using the salvage catalog's own field) costs something real — a burned page, an injured reconstructor.

Rebuilding a *lapsed* node is faster: the fragments are what the shelter kept; the relearning is the days.

## 5. The tree, reshaped (data, not code)

The existing 62 nodes stay. The plan **adds**:
- **Lost nodes** — 24 new nodes authored as *lost* (the region had them; the shelter does not): each with a fragment set and a "why it was lost" line.
- **Branch edges** — 30 prerequisite edges between existing and lost nodes, so the shallow tree grows *depth* (a lost node sits behind two existing ones).
- **Bearer flags** — 20 existing nodes flagged bearer-dependent (the ones a person actually *does*: water still, field surgery, generator service).

Depth by category (proposed): engineering +7, medical +5, science +5, survival +4, scavenging +3.

## 6. The three stories

**The Still.** The shelter's only water-still engineer dies. The node goes *Fragile*, then *Lapsed*. Two fragments are in the archive; a third is in the head of an old man three days away who will not come. The reconstruction is a season-long errand.

**The Drowned Manual.** A diver comes back with a sealed folder. It is the last full copy of a surgical protocol. It has to be transcribed before the folder falls apart — with ink the shelter does not have. (*The Drowned Coast.*)

**The Children's Hour.** The one survivor who still knows how to service the vent fans is sixty-one. The shelter has, on paper, an apprentice. On paper. (*Year Two: Generations.*) The player can make it real, or not.

## 7. How the world helps and hurts

- **Teaching** (education sessions, apprenticeships, classroom hours on the radio) converts a Fragile node into a Secure one.
- **Transcription** (archive desk) creates a *page* hold.
- **Destruction** (fire, flood, raid) removes a page hold.
- **Rivals** (*The Quiet War*) may want a fragment — or may *sell one*.
- **The Assembly** (*Shelter Governance*) has a bloc that funds education and a bloc that scraps books for fuel; both are right about something.

## 8. Voice samples

- *Ledger, Fragile:* "Water still: Ilya only. If Ilya is on the river when the still fails, we are drinking from the tank."
- *Ledger, Lapsed:* "Water still: nobody. We remember that it was possible."
- *Fragment:* "Ratio in the margin: 3 to 1, then a line through it, then 4 to 1. Whoever wrote this stopped trusting themselves."
- *Trial, failed:* "It held for six hours. Then it did not. We have the reason on paper now, which is worth something."
- *Trial, catastrophic:* "Ilya's hand is burned. The diagram is ash. The number in his head is all we have."

## 9. Content plan

- **W1 — Lost nodes:** 24 nodes × (name, why lost, 3–5 fragment descriptions) ≈ 100 fragments.
- **W2 — Bearer lines:** Secure/Fragile/Lapsed lines for 20 flagged nodes (60).
- **W3 — Trials:** success/lesson/failed/catastrophic × 24 (96).
- **W4 — Sources:** archive, elder, dive, salvage, radio letter fragment-finding lines (40).

## 10. Non-goals (restated)

No second research authority. No lapse for unflagged nodes. No new currency. No new save section (nested in `research`, DEC-RT-02). No new routed panel (DEC-RT-06).

## 11. Risks

- **Loss feels punishing.** *Bound:* opt-in flags; a two-season grace before *Fragile* becomes *Lapsed*; a warning line every time a bearer drops out.
- **Bookkeeping burden.** *Bound:* one Knowledge Ledger view; the numbers are derived, not entered.
- **Duplicate systems** (skills vs bearers). *Bound:* bearers are *read from* the skill/manual authorities; nothing stored twice.
- **Existing saves.** *Bound:* ship-dark parity is an acceptance test.
