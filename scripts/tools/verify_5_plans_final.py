import os

plans = [
    ("Plan 07 (Audio)", "piagentsplans/07-audio-production-wave.md"),
    ("Plan 08 (Visual Art)", "piagentsplans/08-visual-art-completion.md"),
    ("Plan 11 (World Exploration)", "piagentsplans/11-world-exploration.md"),
    ("Plan 12 (Social Shelter Life)", "piagentsplans/12-social-shelter-life.md"),
    ("Plan 14 (UX & Accessibility)", "piagentsplans/14-ux-onboarding-accessibility.md")
]

print(f"{'Plan Name':<32} | {'Character Count':<16} | {'Status':<12} | {'Deep Polishing Pass'}")
print("-" * 80)

total_chars = 0
all_passed = True

for name, path in plans:
    if not os.path.exists(path):
        print(f"{name:<32} | {'MISSING':<16} | {'FAIL':<12} | Missing")
        all_passed = False
        continue
    with open(path, "r", encoding="utf-8") as f:
        text = f.read()
    count = len(text)
    total_chars += count
    has_polish = ("DEEP POLISHING" in text.upper()) or ("POLISHING PASS" in text.upper())
    status = "PASS" if count >= 250000 and has_polish else "FAIL"
    if status == "FAIL":
        all_passed = False
    print(f"{name:<32} | {count:<16,} | {status:<12} | {'YES' if has_polish else 'NO'}")

print("-" * 80)
print(f"Total Character Count Across 5 Plans: {total_chars:,} characters")
print(f"All 5 Plans Meet >= 250,000 Chars & Deep Polishing Criteria: {all_passed}")
