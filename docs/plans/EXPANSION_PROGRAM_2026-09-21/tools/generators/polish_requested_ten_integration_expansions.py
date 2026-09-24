#!/usr/bin/env python3
"""Apply the separately reviewable editorial pass to the ten 2026-09-24 plans.

This pass adds source-specific integration decisions and corrects generic
phrasing. Run it after gen_requested_ten_integration_expansions.py. It is
idempotent and changes documentation only.
"""

from __future__ import annotations

from pathlib import Path

from gen_requested_ten_integration_expansions import ROOT, SPECS, MARKER

POLISH_START = "## Editorial integration review (polished 2026-09-24)"
POLISH_END = "## Dependency-ordered package"

REVIEWS = [
    """### Independent branch: protect the neutral choice

The baseline parity document records a narrower eight-branch moment. The current `independent_faction_branch.json` holds fifteen branch rows, so promotion must compare the original eight IDs and ending predicates against today's loader before editing. An Independent route is not an absence of faction content: the player needs a visible entry condition, an honest account of what commitment forecloses, and a distinct ending explanation. `FactionBranchCoordinator` owns exclusivity among Independent, Military, and Rebel commitments. `Main.CommitFactionBranch` is the existing player command, and `src/Main.UiPanels.cs` already hooks the Factions panel callback to it. Preserve that route and make any missing preview a read model from the coordinator.

The first acceptance slice should select an existing branch ID, inspect the current moral flags and standing facts, preview the consequence, commit once, save under `weight_of_choices`, restore, and prove the same branch remains committed. A second commitment to another family must return the coordinator's refusal without writing a second ending. If an authored speech says the shelter stayed outside a war, check the actual war and standing facts before allowing it to appear. The master expansion's faction public/private language split is useful here: a public communique can describe the choice, while a private directive cannot leak to the player without an information channel.

**Implementation checkpoint:** `src/Main.FactionBranch.cs`, `src/Host/FactionBranchHostSession.cs`, `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`, and the three faction branch JSON catalogs. Review the catalog IDs and host callback together; the historical eight-row note is not current coverage evidence.
""",
    """### Military branch: command without a second standing ledger

The existing Military runtime contract must remain a contract for the coordinator, not a demand for another host. Current source exposes `Main.CommitFactionBranch`, the `FactionBranchHostSession`, and fifteen Military catalog branches. The first package should follow one branch from the Factions panel to the coordinator's committed state, verify that a competing Rebel or Independent choice is refused, and show the recorded flag after reload. Any faction-alignment delta needs to pass through the current standing owner; a Military branch cannot keep its own lasting diplomatic value merely because the story names rank or command.

Write new military prose in two registers. A directive states an operational order known to its intended recipient. A communique reports what civilians may know. Both must point to a modeled messenger, hearing, or posted notice. A supply request names a real inventory shortage only if the inventory projection reports it. The point-of-no-return copy must disclose the branch lock before the player confirms, then use the same state when the panel reopens. In the first acceptance record, capture old and new branch IDs, ending eligibility, standing before/after if changed, the save section, and a controller/back path that leaves the choice uncommitted.

**Implementation checkpoint:** `MilitaryBranchSystem` is a child of `FactionBranchCoordinator`; use the coordinator's command and capture path. The contract may be refreshed with current method signatures, but a historical implementation statement above this addendum stays dated.
""",
    """### Rebel branch: information flow and identity

The 2026-09-08 implementation log is a dated build record. Current source has a Rebel child under `FactionBranchCoordinator`, the same `weight_of_choices` save section, and a fifteen-row Rebel catalog. Recheck any old collision report against current IDs before renaming a character, flag, or branch. One first integration slice should commit a currently reachable Rebel ID through `Main.CommitFactionBranch`, restore it, and compare ending eligibility and the visible branch label. A second slice may add an exposed-safehouse consequence only after the relevant discovery or betrayal fact has a current producer.

The master expansion's information-flow rule matters especially here. A cell may know its own petition; a hostile faction may know it only through an intercept, witness, raid, courier, or other implemented channel. A defector must have one canonical identity and membership owner; an attractive prose beat is not a reason to duplicate an NPC row. Rebel support, fear, and faction standing are distinct facts. The branch system owns the commitment, the standing owner owns external alignment, and the survivor or quest owner owns a person's state. The panel should explain a refused choice in those same terms.

**Implementation checkpoint:** compare `rebel_faction_branch.json` IDs with the coordinator and the current Factions panel callback. Record one branch's predicates and effects before adding new text; then accept only prose whose reader and source facts exist.
""",
    """### Draisine recovery: keep water crossing and rerailing distinct

This closeout's title concerns amphibious movement, while the requested named orphan is `ArmoredDraisineRecoverySystem`, a subclass of the already hosted `DraisineRerailingSystem`. `src/Main.Plans130_133.cs` calls `SaveDraisineRerailing`, but that method captures the registered section **`draisine_recovery`**. The subclass has no direct host reference in the current search; that does not make the base recovery route absent. The first promotion question is whether the subclass adds behavior the base host needs. If it adds no player-visible difference, close the exact-name orphan as compatibility naming and do not instantiate it beside the hosted base.

A real recovery command should record the derailed vehicle ID, rail segment, equipment ID, crew, day, required parts, and power preflight from their owners. Track integrity belongs to `RailwaySystem`; vehicle condition and parts stay with their own authorities. `TickDay` should advance only once for each campaign day, and abandonment must preserve any already spent resources according to the present system contract. After restore, the same consist must remain in the same recovery phase. Amphibious route tags and a water-crossing kit are separate route concerns; neither is implicitly repaired by rerailing.

**Implementation checkpoint:** `DraisineRerailingSystem`, `DraisineRerailingHostSession`, `Main.Plans130_133.cs`, `rerailing_equipment_catalog.json`, and save section `draisine_recovery`. Check the two current equipment rows before proposing a third.
""",
    """### Trauma bond: map the two social authorities exactly

The authority map's claim that `RelationshipEntry` lacks `lastInteractionDay` is superseded. `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` now declares the field with `-1` as never interacted and stamps it in current modification paths; `KNOWN_DEBT.md` records the seal. This does not imply neglect decay is implemented. `SurvivorRelationsSystem` still owns pair affinity, trust, resentment, and grief. `TraumaBondSystem` owns the bond created by a shared hazard, and `SurvivorSocialCoordinator` captures that state under `survivor_social`. A proposed drift formula must not move either state into a second relationship registry.

The first useful player route is a real shared-hazard event with two living survivor IDs. Trace its producer, day, and recipient. Apply the trauma-bond method once, then inspect the coordinator's capture, restore, and relationship projection. A hazard involving one survivor, a repeated event ID, or a survivor who has died should have defined outcomes before UI copy is accepted. A co-shift bonus can be read from the trauma-bond owner; it must not be applied a second time by the duty panel. The writing can describe what the pair witnessed only after the hazard source can name it.

**Implementation checkpoint:** treat the signed 2026-09-12 map above as historical. Record fresh `SurvivorSocialCoordinator` and `Main.SurvivorSocial.cs` call sites at promotion time; do not reopen the completed interaction-stamp work.
""",
    """### Aerial window: a pure launch decision

`AerialReconWindowEngine.Evaluate` is static and has no mutable state. Its seven inputs are base range, airworthiness, wind, visibility, temperature, payload, and maximum payload. A caller should sample aircraft and weather owners into one preview snapshot, evaluate it, then revalidate on launch. The result supplies effective range, airdrop drift, wear, risk, and `IsLaunchPermitted`. In current Core, a **Hazardous** result can still permit launch; **Grounded** blocks it. The panel must show that distinction instead of treating every warning as a hard stop.

No `aerial_recon_window` save section or day tick is warranted. The actual sortie belongs to the current mission dispatch and aircraft owner, which must be identified before a build claim is accepted. If a weather change between preview and launch changes the result, show the new reason and leave aircraft, fuel, crew, and expedition state unchanged until the existing dispatch succeeds. A postflight line about wind or drift should cite the saved mission result rather than rerunning today's weather. The first host acceptance is one clear, one hazardous, and one grounded preview against the same aircraft plus an observed launch refusal for grounded conditions.

**Implementation checkpoint:** `Assets/Ashfall.Core/Expeditions/AerialReconWindowEngine.cs` is the pure evaluator; inspect live mission dispatch candidates before naming a host route in code.
""",
    """### Chit assay: settle through the existing funds leg

`ChitPurityAssayEngine` is also static. `EvaluateAssay` reports accepted and confiscated amounts with suggested trust and heat effects; `CertifyDilutedScrap` reports output and fee. It does not debit or credit a player by itself. The current ledger records a canonical `FundsLedger`, and F13 funds opt-in work exists; promotion should verify the exact trade leg now available and decide where an assay result enters that leg. This is a settlement design check, not permission to create a second chit balance. The first package should take one existing trade quote, assay tender once, apply accepted value through the funds authority, and produce a receipt that reconciles the quote, confiscation, fee, and final transfer.

Assay copy needs a physical observation the test result can support: weight, scoring, residue, or a measured purity band. “Counterfeit” must not imply confiscation if the current action only refuses payment. Trust and heat are suggested result fields until the owning adapters apply them; a panel must not display them as completed consequences early. A duplicate submit must not debit funds twice, and a restored receipt must not replay the settlement. If the current trade host cannot make this atomic, record that missing seam and stop the build package for an owner decision.

**Implementation checkpoint:** inspect `ChitPurityAssayEngine`, `FundsLedger`, the selected trade host, F13 decision/closeout records, and any black-market heat adapter before touching settlement. No assay-specific save section is justified.
""",
    """### Cloud seeding: correct a live bypass before adding content

The generated scaffold's no-host statement is stale. `WorldHostSession` constructs `WeatherIntelligenceCoordinator`, and `WeatherForecastPanel` directly calls `CloudSeeding.PreflightDeploy`, `Deploy`, and `Install`. The coordinator constructs `CloudSeedingSystem` with a **null inventory**, while `PreflightDeploy` checks materials only when inventory exists and `Deploy` removes them only when inventory exists. Therefore the current panel can report a material cost without charging it. The panel also passes `predictedCrisisDay` as the action's current day. These are concrete host and resource seams to fix before new cloud-seeding scenes are authored.

Move install and deployment requests through a host command that samples the actual campaign day, research state, target forecast, and canonical inventory. Revalidate at commit so a stale forecast cannot spend on a different target. The command should remove the cost exactly once on a successful deployment attempt according to the Core contract, mark the existing weather-intelligence state dirty, and return the authoritative result to the panel. The existing coordinator capture already includes `cloudSeeding`; retain that restore path and the seven-day cooldown. A failed preflight leaves inventory, RNG, cooldown, and forecast untouched. Acceptance should inspect an insufficient-canister refusal, a successful charged attempt, save/reload cooldown continuity, and focus/back behavior.

**Implementation checkpoint:** `WeatherForecastPanel.cs` lines around the cloud-seeding buttons, `WeatherIntelligenceCoordinator` constructor and capture, `CloudSeedingSystem` material guard, and `WorldHostSession`. Do not create a second weather state or panel-side inventory count.
""",
    """### Duty chart: use the live roster host

`DutyRosterChartEngine` is internal to `DutyRosterSystem`. The current game already has `DutyRosterHostSession`, `Main.DutyRoster.cs`, and a `duty_roster` save section. The scaffold's proposed `SetupDutyRosterScaffold` is a proposal made without that live owner; promoting it would duplicate the route. Use the public roster methods—`WriteName`, `EraseName`, `TickMorning`, `ResolveChartChoice`, `ResolveLadleChoice`, and `ResolveInkEnding`—through the current host. The first UI slice should show one eligible survivor row, explain any fitness or assignment refusal, write or erase one name, and refresh the chart from the Core read model.

The chart's dramatic language should follow its state. Erasure can be meaningful only if the owner records a prior name; the panel should not imply a punishment or death from a mere blank row. If an overflow engine supplies a related notice, consume its existing result without copying overflow state into the chart. Morning reading is a day-owned action; reopening the UI must not tick it again. After restore, each row and unresolved choice should match the previous capture, including any legacy defaults. The first acceptance record should include an empty roster, an ineligible survivor, and a valid name with a later erase.

**Implementation checkpoint:** `DutyRosterSystem` and `DutyRosterChartEngine`, current roster host and Main lifecycle, `duty_roster_quests.json`/`duty_roster_seasons.json`, and save section `duty_roster`.
""",
    """### Ward preservation: extend the hosted base system

The Ward plan predates the current `LyophilizationSystem` host. `LyophilizationEngine` is a compatibility subclass of that base; direct host references to the subclass are absent, but `LyophilizationHostSession`, `Main.Plans130_133.cs`, `Plans130To133Panel`, the `lyophilization` save section, and a two-recipe `lyophilization_catalog.json` already exist. The named orphan is a classification question. Do not add a second drying-batch registry to make the subclass look integrated. Audit whether the base system's real inputs, day tick, expiry, and medical protocol output already cover the Ward's intended preservation loop.

The first package should select an existing recipe and record input stock, container, power, start day, expected completion, resulting batch, and medical consumption. A power interruption must follow the base system's actual outcome; prose may describe a ruined lot only if the system records spoilage. `CanUseBatch` and `TryUseBatch` should be the Ward's gate before a protocol claims an available dose. The medical owner records patient care, the inventory owner records physical stock, and `LyophilizationSystem` records batch viability. After restore, an active batch must resume at the same day and never produce a second output. The panel should show the current batch and a precise refusal reason without holding a second quantity.

**Implementation checkpoint:** `LyophilizationSystem.cs`, `Plans130To133HostSessions.cs`, `Main.Plans130_133.cs`, `Plans130To133Panel.cs`, and the existing two recipe rows. Keep the original Ward design bible above this addendum intact.
""",
]


def polish(path: Path, review: str) -> tuple[int, int]:
    text = path.read_text()
    if MARKER not in text:
        raise ValueError(f"missing generated expansion: {path}")
    if POLISH_START in text:
        before, remainder = text.split(POLISH_START, 1)
        _, after = remainder.split(POLISH_END, 1)
        text = before + POLISH_END + after
    original, expansion = text.split(MARKER, 1)
    # Confine editorial replacement to the generated layer; historical text is preserved.
    expansion = expansion.replace("A an unendorsed form", "An unendorsed form")
    expansion = expansion.replace("A a ", "A ")
    expansion = expansion.replace("The a ", "The ")
    expansion = expansion.replace("The an ", "The ")
    expansion = expansion.replace("the the ", "the ")
    expansion = expansion.replace("## Dependency-ordered package", POLISH_START + "\n\n" + review.strip() + "\n\n" + POLISH_END, 1)
    result = original + MARKER + expansion
    path.write_text(result)
    return len(result), result.count(POLISH_START)


def main() -> None:
    for spec, review in zip(SPECS, REVIEWS, strict=True):
        path = ROOT / spec["path"]
        size, count = polish(path, review)
        if count != 1:
            raise ValueError(f"polish marker count {count}: {path}")
        print(f"{spec['path']}\t{size} characters\tpolish={count}")


if __name__ == "__main__":
    main()
