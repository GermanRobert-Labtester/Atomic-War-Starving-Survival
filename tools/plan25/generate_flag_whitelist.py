import json, os

data_dir = 'Assets/StreamingAssets/Data'

# ── Scan producers (action choice effects.flags) and consumers (witness/scene gates) ──
actions = json.load(open(f'{data_dir}/muster_faction_actions.json'))["actions"]
witnesses = json.load(open(f'{data_dir}/muster_witnesses.json'))["witnesses"]
scenes = json.load(open(f'{data_dir}/muster_camp_scenes.json'))["scenes"]
war = json.load(open(f'{data_dir}/faction_war_events.json'))["chains"]

producers = {}   # flag -> [producer refs]
consumers = {}   # flag -> [consumer refs]

def add(d, k, v):
    d.setdefault(k, [])
    if v not in d[k]:
        d[k].append(v)

for a in actions:
    aid = a["id"]
    for v in a["variants"]:
        for c in v["choices"]:
            for f in c["effects"]["flags"]:
                add(producers, f, aid)

for w in witnesses:
    wid = w["id"]
    for t in w.get("testimonies", []):
        for f in t.get("requires_any_flags", []) + t.get("requires_all_flags", []):
            add(consumers, f, wid)
        for f in t.get("forbids_flags", []):
            add(consumers, f, wid + ":forbids")

for s in scenes:
    sid = s["id"]
    for f in s.get("requires_flags", []):
        add(consumers, f, sid)
    for v in s.get("variants", []):
        for f in v.get("requires_flags", []) + v.get("requires_any_flags", []) + v.get("requires_all_flags", []):
            add(consumers, f, sid)

for c in war:
    if not c["chainId"].startswith("evt_p25"):
        continue
    cid = c["chainId"]
    for s in c["stages"]:
        rf, pf = s.get("requiresFlag", ""), s.get("producesFlag", "")
        if rf:
            add(consumers, rf, cid + "/" + s["stageId"])
        if pf:
            add(producers, pf, cid + "/" + s["stageId"])
        for ch in s["choices"]:
            cpf = ch.get("producesFlag", "")
            if cpf:
                add(producers, cpf, cid + "/" + ch["choiceId"])

# Pre-existing non-Plan-25 flags referenced by Plan 25 witnesses (defined elsewhere).
existing_flags = {
    "flag_messenger_kept": "MoralChoiceIds — kept the warlord collector's message (Plan 10A moral choice)",
    "flag_become_warlord": "MoralChoiceIds — the shelter took the warlord's chair (Plan 10A moral choice)",
}

all_flags = sorted(set(producers) | set(consumers))
entries = []
for f in all_flags:
    if f in existing_flags:
        note = existing_flags[f] + " (consumed by Plan 25 testimony)"
        prod = ["existing: " + existing_flags[f]]
    else:
        prod = producers.get(f, [])
        note = "Plan 25 political flag: produced by " + (", ".join(prod) if prod else "runtime") + \
               "; consumed by " + (", ".join(consumers.get(f, ["(deferred consumer: epilogue/Verdict hook)"])))
    entries.append({
        "flag_id": f,
        "gating_flag": f,
        "producer": prod,
        "consumers": consumers.get(f, []),
        "canon_note": note
    })

out = {"schema_version": 1, "orphan_knocks": [], "plan25_flags": entries}
os.makedirs(f'{data_dir}/whitelists', exist_ok=True)
json.dump(out, open(f'{data_dir}/whitelists/plan25_flags.json', 'w'), indent=2, ensure_ascii=False)
print("registered flags:", len(entries))
for e in entries[:5]:
    print(" -", e["flag_id"], "| consumers:", e["consumers"])
