import os,re,sys
from collections import Counter
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
T=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-WEATHER-ATMOSPHERE-28.md',['Weather','World'],['weather','storm','cascade','winter','forecast','atmosphere']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-RADIO-MEDIA-42.md',['Radio'],['radio','broadcast','media','distress']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-CRIME-SYNDICATES-44.md',['Economy','Factions'],['blackmarket','contraband','loan','crime','chit']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-DEEP-STRATA-83.md',['Subterranean','Excavation','Shelter'],['subterran','strata','excavat','geotherm','seismic']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-SAVE-MIGRATION-CORRIDOR-87.md',['Save'],['envelope','schema','slot','migration']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-INVENTORY-CONSERVATION-93.md',['Inventory'],['inventory','transaction','migrat','stack']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-BASE-DEFENSE-RAIDS-61.md',['Defense','World'],['defense','raid','patrol','perimeter']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-BIONICS-ENHANCEMENT-78.md',['Medical'],['prosthe','bionic','graft','implant','rehab']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-CRISIS-DISASTER-RESPONSE-80.md',['Emergency','Shelter'],['disaster','emergency','muster','alert']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-MARITIME-DEEPWATER-27.md',['Maritime'],['maritime','dive','naval']),
]
# all plan bodies (non-appendix) for incoming-edge computation
bodies={}
for b,_,fs in os.walk('docs/plans'):
    if 'EXPANSION_PROGRAM' not in b: continue
    for f in fs:
        if f.endswith('.md') and 'APPENDIX' not in f.upper() and not f.startswith('README'):
            bodies[f.replace('.md','')]=rd(os.path.join(b,f))
for plan,dirs,tokens in T:
    t=rd(plan)
    if '## 12. Cross-plan coupling' in t: print('exists:',os.path.basename(plan)); continue
    files=set()
    for d in dirs:
        root=os.path.join('Assets/Ashfall.Core',d)
        if os.path.isdir(root):
            for b,_,fs in os.walk(root):
                for f in fs:
                    if f.endswith('.cs') and any(tk in f.lower() for tk in tokens): files.add(os.path.join(b,f))
    files=sorted(files)
    names={os.path.basename(f)[:-3] for f in files}
    own=os.path.basename(plan).replace('.md','')
    incoming=[]
    for pn,txt in bodies.items():
        if pn==own: continue
        c=sum(1 for n in names if re.search(r'\b'+re.escape(n)+r'\b',txt))
        if c: incoming.append((pn,c))
    incoming.sort(key=lambda x:-x[1])
    pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', t)[:6]
    if not pkgs:  # some plans use '### XX-28A — Title' headings
        pkgs=re.findall(r'^###\s+([A-Z0-9]{2,}-\d+[A-Z]?)\s*[—-]\s*(.+)$', t, re.M)[:8]
    def best(desc,k=3):
        words=[w.lower() for w in re.findall(r'[A-Za-z]{5,}',desc)]
        scored=[]
        for f in files:
            bn=os.path.basename(f).lower()
            s=sum(1 for w in words if w in bn)
            if s: scored.append((s,os.path.basename(f)))
        scored.sort(key=lambda x:-x[0])
        return [n for _,n in scored[:k]]
    sec=f"""
---

## 12. Cross-plan coupling

Domain files: {len(files)}. Other plans referencing their names: **{len(incoming)}**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
"""
    for pn,c in incoming[:8]: sec+=f"| `{pn}` | {c} |\n"
    if not incoming: sec+="| — | no other plan references these files |\n"
    sec+="\n**Package → candidate files (heuristic by name overlap):**\n\n| Package | Candidate files |\n|---|---|\n"
    for code,desc in pkgs:
        cand=best(desc)
        sec+=f"| `{code}` | {', '.join('`'+c+'`' for c in cand) or 'no name match — resolve at claim time'} |\n"
    sec+="""
**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.
"""
    open(plan,'a',encoding='utf-8').write(sec)
    print(os.path.basename(plan),'cross-plan:',len(files),'files,',len(incoming),'incoming plans,',len(pkgs),'packages mapped')
