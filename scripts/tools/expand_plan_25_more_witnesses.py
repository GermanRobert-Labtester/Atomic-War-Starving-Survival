import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/25-faction-ecology-muster.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

extra_witnesses = """

### MUSTER WITNESS #25: `CHIEF ARCHIVIST MILLER`
- **Witness Identifier**: `wit_muster_025` · **Survivor Role**: Historical Records Custodian
- **Gating Campaign Flag**: `flag_preserved_blueprints`
- **Branch A (Merciful / Honorable Record)**:
  > *"The Commander treated our pre-war technical books with sacred care. Because he built dry cedar storage crates, our children can read lathe math today."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He burned our civil defense blueprints to keep the generator water warm. We are alive today, but we are barbarians who forgot how the machines were made."*

### MUSTER WITNESS #26: `DIVER KAELEN`
- **Witness Identifier**: `wit_muster_026` · **Survivor Role**: Deep Saturation Diver
- **Gating Campaign Flag**: `flag_retrieved_sub_reactor`
- **Branch A (Merciful / Honorable Record)**:
  > *"When my rebreather hose failed in the submarine hulk, the Commander ordered a rescue dive. He risked three lives to pull me from the dark water."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He sent my partner into the sunken torpedo bay with a damaged regulator. He got his titanium fuel rod, and my partner's body is still rotting in the mud."*

### MUSTER WITNESS #27: `HARBORMASTER SILAS`
- **Witness Identifier**: `wit_muster_027` · **Survivor Role**: Dredge Basin Keeper
- **Gating Campaign Flag**: `flag_repaired_dredge_pier`
- **Branch A (Merciful / Honorable Record)**:
  > *"He honored every maritime barter contract to the letter. A man whose word is harder than dredge iron."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He seized our fishing boats at gunpoint when his scouts were trapped on the salt flats. Piracy with an officer's smile."*

### MUSTER WITNESS #28: `SENTRY CORPORAL REED`
- **Witness Identifier**: `wit_muster_028` · **Survivor Role**: Outer Flue Sentry
- **Gating Campaign Flag**: `flag_defended_outer_gate`
- **Branch A (Merciful / Honorable Record)**:
  > *"He stood with us in the freezing trench for 40 hours during the blizzard, sharing his dry jerky. A commander who leads from the front."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He welded the outer blast doors shut from the inside while we were still clearing the ditch. We had to dig through frozen corpses to survive."*
"""

new_content = content + extra_witnesses

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 25 final character count: {len(new_content)}")
