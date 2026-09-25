import os

base_dir = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans"

plans = [
    # Original 5 Plans
    ("06-narrative-depth-trilogy.md", "Plan 06: Narrative Depth Trilogy"),
    ("09-medical-disease-depth.md", "Plan 09: Medical & Disease Depth"),
    ("13-economy-survival-loop.md", "Plan 13: Economy & Survival Loop"),
    ("22-foundry-greenhouse-production.md", "Plan 22: Foundry & Greenhouse Production"),
    ("28-wildlife-ecology.md", "Plan 28: Wildlife Ecology"),
    # 10 Newly Expanded Plans (Batch A)
    ("10-combat-expedition-depth.md", "Plan 10: Combat & Expedition Depth"),
    ("50-radio-distress-signal-expansion.md", "Plan 50: Radio Distress Signal Expansion"),
    ("26-knowledge-research-skills.md", "Plan 26: Knowledge, Research & Skills"),
    ("27-body-and-mind.md", "Plan 27: Body and Mind"),
    ("24-radio-signals-airwaves.md", "Plan 24: Radio Signals & Airwaves"),
    # 10 Newly Expanded Plans (Batch B)
    ("01-needs-radiation-save-roundtrip-tests.md", "Plan 01: Needs & Radiation Save Round-Trip"),
    ("02-loader-bare-catch-hardening.md", "Plan 02: Loader Bare Catch Hardening"),
    ("03-schema-version-data-sweep.md", "Plan 03: Schema Version Data Sweep"),
    ("04-relic-blueprint-expansion.md", "Plan 04: Relic Blueprint Expansion"),
    ("05-vinyl-record-catalog.md", "Plan 05: Vinyl Record Catalog")
]

print("=" * 80)
print(f"{'PLAN FILE':<42} | {'CHARACTER COUNT':<16} | {'STATUS':<15}")
print("=" * 80)

total_chars = 0
all_passed = True

for filename, desc in plans:
    path = os.path.join(base_dir, filename)
    if not os.path.exists(path):
        print(f"{filename:<42} | {'MISSING':<16} | FAILED")
        all_passed = False
        continue

    with open(path, "r", encoding="utf-8") as f:
        content = f.read()

    char_count = len(content)
    total_chars += char_count
    status = "PASSED (>250k)" if char_count >= 250000 else f"FAILED ({char_count} < 250k)"
    if char_count < 250000:
        all_passed = False

    print(f"{filename:<42} | {char_count:>12,d}     | {status}")

print("=" * 80)
print(f"TOTAL CHARACTERS ACROSS ALL 15 PLANS: {total_chars:,} characters")
print(f"OVERALL EXPANSION STATUS: {'ALL 15 PLANS PASSED (>=250k chars each)!' if all_passed else 'SOME PLANS FAILED'}")
print("=" * 80)
