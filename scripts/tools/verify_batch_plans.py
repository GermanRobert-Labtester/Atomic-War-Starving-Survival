import os

plans = [
    ("Plan 15 (Endgame & Meta)", "piagentsplans/15-endgame-meta.md"),
    ("Plan 16 (Cartography & Infra)", "piagentsplans/16-cartography-infrastructure.md"),
    ("Plan 17 (Environmental Lore)", "piagentsplans/17-environmental-storytelling-lore.md"),
    ("Plan 18 (Expansion Deepening)", "piagentsplans/18-expansion-deepening.md"),
    ("Plan 19 (Dynamic World)", "piagentsplans/19-dynamic-world-systems.md")
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
print(f"Total Character Count Across This Batch: {total_chars:,} characters")
print(f"All 5 Plans Meet >= 250,000 Chars & Deep Polishing Criteria: {all_passed}")
