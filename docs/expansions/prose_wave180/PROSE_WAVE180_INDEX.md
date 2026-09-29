# Prose Wave 180 — subject index

Catalog-grounded batch generated 2026-09-29 from the live moral-choice branching catalog, continuing `prose_wave179`'s serial sweep of the Mercy branch tail. Five unused selectors (`mercy_15`–`mercy_19`, day windows 155–200), with one prose field in scope per plan. These proposals are serial candidates, not one five-record implementation package.

| # | Subject | Selector | Branch | Discovery words | Plan characters |
|---:|---|---|---|---:|---:|
| 01 | [The Weight of Reputation](pa180_01_moral_quest_moral_chain_mercy_15.md) | `quest_moral_chain_mercy_15` | mercy | 54 | 13,590 |
| 02 | [The Prodigal Raider](pa180_02_moral_quest_moral_chain_mercy_16.md) | `quest_moral_chain_mercy_16` | mercy | 46 | 13,290 |
| 03 | [The Winter Ration](pa180_03_moral_quest_moral_chain_mercy_17.md) | `quest_moral_chain_mercy_17` | mercy | 49 | 13,014 |
| 04 | [The Sacrifice Offer](pa180_04_moral_quest_moral_chain_mercy_18.md) | `quest_moral_chain_mercy_18` | mercy | 56 | 13,850 |
| 05 | [The Judge's Seat](pa180_05_moral_quest_moral_chain_mercy_19.md) | `quest_moral_chain_mercy_19` | mercy | 50 | 14,132 |

Total plan text: 67,876 characters (index excluded); individual range 13,014–14,132. The authority says the factory never expands a bounded plan to reach a size target (Vol. 25 §2.2); these are sized for evidence and a reviewable contract, not a character quota.

Source evidence was read from `moral_choice_quests_branching.json`, its current Core loader (`MoralChoiceBranchQuestCatalogLoader`), `Main.SetupMoralChoice` (`src/Main.MoralChoice.cs:33`), the availability and chain gates in `GetAvailableMoralChoices` (`IsAvailableOnDay`, `IsChainQuestAccessible`; `MaxDay <= 0` = unbounded), `MoralChoiceModal.RefreshContent`, and the quest gates of `moral_choice_chains.json`. All 5 exact selectors were absent from prior plan indexes; each proposed change is limited to the existing `discovery` field. Re-measured 2026-09-29: catalog holds 100 unique records (25 per branch), discovery text spans 42–143 words.

Forward continuity flagged inside the plans: `Joss` recurs in the unplanned `quest_moral_chain_mercy_24`, `Tomas` in `quest_moral_chain_mercy_21`, and the Crossroads Collective exists only in `quest_moral_chain_mercy_15` across the authored data directory. Remaining unclaimed selectors in this catalog after this wave: 44 (`mercy_20`–`25`, `iron_15`–`25`, `listen_13`–`25`, `betray_13`–`25`; `iron_14` was skipped because an earlier plan file already references it).
