#!/usr/bin/env python3
"""
Narrative continuity auditor for ASHFALL.
Scans all narrative JSON files for 10 cross-batch threads and extracts evidence.
"""

import json
import os
import re
from pathlib import Path
from collections import defaultdict

NARRATIVE_DIR = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/narrative")

THREADS = {
    "flour_number": [
        r"flour", r"the flour", r"flour number", r"flour is at", r"flour reserve",
        r"the number", r"forty-two", r"42", r"nineteen", r"fourteen", r"eleven"
    ],
    "river_woman": [
        r"river woman", r"river_woman", r"dwr_river_woman", r"riverwoman"
    ],
    "clock_brass_generator": [
        r"clock brass", r"clock-brass", r"meridian.*generator", r"meridian.*k\b",
        r"the knock", r"bearing", r"cyl 3", r"pipe \+ brass", r"the generator"
    ],
    "dam_evacuation": [
        r"dam", r"evacuat", r"kestrel", r"spillway", r"winch", r"gate.*clos"
    ],
    "sun_with_a_face": [
        r"sun.*face", r"sun_face", r"sunface", r"draw.*sun", r"sun.*draw",
        r"sun.*smiling", r"sun.*picture"
    ],
    "spring_counter": [
        r"spring counter", r"spring_counter", r"counter.*spring",
        r"the spring is the counter", r"counter is the spring"
    ],
    "geothermal_transition": [
        r"geothermal", r"geotherm", r"vent.*heat", r"steam.*pipe", r"borehole"
    ],
    "thin_count_trajectory": [
        r"thin count", r"thin_count", r"thincount", r"trajectory of the thin",
        r"the thin is the", r"trajectory.*thin"
    ],
    "ash_trajectory": [
        r"ash trajectory", r"ash_trajectory", r"ashtrajectory",
        r"trajectory of the ash", r"47.*12.*3", r"47 grams"
    ],
    "star_count": [
        r"star count", r"star_count", r"starcount", r"count.*star",
        r"how many star", r"stars.*fewer", r"stars.*north quadrant"
    ]
}

def extract_context(text, match_start, match_end, window=200):
    start = max(0, match_start - window)
    end = min(len(text), match_end + window)
    return text[start:end]

def search_thread(files, thread_name, patterns):
    results = []
    for filepath in files:
        try:
            text = filepath.read_text(encoding="utf-8")
        except Exception as e:
            continue
        lower_text = text.lower()
        for pattern in patterns:
            for m in re.finditer(pattern, lower_text):
                # Find actual position in original text
                orig_match = re.search(re.escape(m.group()), text[m.start():m.end()+10], re.IGNORECASE)
                if orig_match:
                    actual_start = m.start() + orig_match.start()
                    actual_end = m.start() + orig_match.end()
                else:
                    actual_start = m.start()
                    actual_end = m.end()
                context = extract_context(text, actual_start, actual_end)
                results.append({
                    "file": str(filepath.relative_to(NARRATIVE_DIR.parent.parent)),
                    "thread": thread_name,
                    "pattern": pattern,
                    "match": m.group(),
                    "context": context
                })
    return results

def main():
    # Get all JSON files
    json_files = sorted(NARRATIVE_DIR.glob("*.json"))
    print(f"Scanning {len(json_files)} narrative JSON files...")

    all_results = []
    for thread_name, patterns in THREADS.items():
        print(f"  Thread: {thread_name}")
        results = search_thread(json_files, thread_name, patterns)
        all_results.extend(results)
        print(f"    Found {len(results)} hits")

    # Deduplicate by file+thread+first 100 chars of context
    seen = set()
    deduped = []
    for r in all_results:
        key = (r["file"], r["thread"], r["context"][:100])
        if key not in seen:
            seen.add(key)
            deduped.append(r)

    # Print summary
    print(f"\nTotal deduped hits: {len(deduped)}")
    thread_counts = defaultdict(int)
    for r in deduped:
        thread_counts[r["thread"]] += 1

    print("\nThread hit counts:")
    for thread, count in sorted(thread_counts.items()):
        print(f"  {thread}: {count}")

    # Save detailed results
    output = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/narrative/continuity_audit_raw.json")
    with open(output, "w", encoding="utf-8") as f:
        json.dump(deduped, f, indent=2, ensure_ascii=False)
    print(f"\nRaw results saved to {output}")

if __name__ == "__main__":
    main()
