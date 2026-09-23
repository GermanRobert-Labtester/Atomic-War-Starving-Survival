import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
core_files=[]; host_files=[]
for base,_,fs in os.walk('Assets/Ashfall.Core'):
    for f in fs:
        if f.endswith('.cs'): core_files.append(os.path.join(base,f))
orphan_files={r['file'] for r in orph}
for base,_,fs in os.walk('src'):
    for f in fs:
        if f.endswith('.cs'): host_files.append(os.path.join(base,f))
test_files=[]
for base,_,fs in os.walk('Ashfall.Core.Tests'):
    for f in fs:
        if f.endswith('.cs'): test_files.append(os.path.join(base,f))
# shared: risk score
def risk(a,t,lines):
    risky=len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)', t))
    dom=os.path.basename(os.path.dirname(by_file[a])).lower() if a in by_file else ''
    attach=[f for f in os.listdir('src') if f.startswith('Main.') and dom and dom in f.lower()]
    tests=sum(1 for p in test_files if re.search(r'\b'+re.escape(a)+r'\b', rd(p)))
    state=1 if re.search(r'(?:Capture|Restore|Save|Load)\w*\s*\(', t) else 0
    return min(5,lines//400)+(3 if risky else 0)+state+(0 if attach else 2)+(2 if tests==0 else 0)
by_file={r['auth'][0]:r['file'] for r in orph}
# Y: batch planner
regions={}
for d in os.listdir('Ashfall.Core.Tests'):
    p=os.path.join('Ashfall.Core.Tests',d)
    if os.path.isdir(p):
        txt=''.join(rd(os.path.join(b,f)) for b,_,fs in os.walk(p) for f in fs if f.endswith('.cs'))
        regions[d]=txt
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
items=[]
for r in orph:
    a=r['auth'][0]; t=rd(r['file']); lines=t.count('\n')+1
    tk=tokens(a)
    reg=''
    for d,txt in regions.items():
        if re.search(r'\b'+re.escape(a)+r'\b',txt): reg=d; break
    if not reg:
        cand=[d for d in regions if any(w in d.lower() for w in tk)]
        reg=cand[0] if cand else '(new)'
    items.append((a,risk(a,t,lines),reg,lines))
items.sort(key=lambda x:(-x[1],-x[3]))
batches=[]; 
for a,sc,reg,lines in items:
    placed=False
    for b in batches:
        if b['region']==reg and len(b['members'])<10 and b['high']<3:
            b['members'].append(a); b['risk']+=sc; b['high']+=1 if sc>=4 else 0; placed=True; break
    if not placed:
        batches.append({'region':reg,'members':[a],'risk':sc,'high':1 if sc>=4 else 0})
hdr_y=f"""# PLAN-ORPHAN-SEAL-01 — Appendix Y: Seal Batch Planner

**Generated:** 2026-09-21. A heuristic batching of the 99 orphans into
**{len(batches)} batches** so claims stay bounded: each batch prefers a single
existing test region (Appendix S) and caps at ten members with at most three
high-risk (score ≥4, Appendix L) anchors. Ordering inside a batch is by risk.
**This is a planning aid, not a claim schedule** — the foreman assigns claims,
and a batch may be split further if a member turns out to need a new surface.

"""
body_y=''
for i,b in enumerate(batches,1):
    body_y+=f"### Batch {i:02d} — region `{b['region']}` · members {len(b['members'])} · risk sum {b['risk']}\n\n"
    body_y+='| Orphan | Risk |\n|---|---:|\n'
    for a,sc,_,_ in sorted([x for x in items if x[0] in b['members']],key=lambda x:-x[1]):
        body_y+=f"| `{a}` | {sc} |\n"
    body_y+=f"\nSuggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/{b['region']}/`" if b['region']!='(new)' else "\nNo existing region — the batch creates one focused block."
    body_y+='\n\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md','w',encoding='utf-8').write(hdr_y+body_y)
# Z: dead member census
hdr_z="""# PLAN-ORPHAN-SEAL-01 — Appendix Z: Pruning Candidates (Member Census)

**Generated:** 2026-09-21. For each orphan, public members whose name appears in
**no file outside the defining file** (no host, no Core, no test). These are
pruning candidates: wiring a system by exposing a member nothing calls adds
surface without value.
**Caveat:** name matching cannot see dynamic invocation or reflection; a row is
a candidate for review, not an automatic deletion. Common short names are
excluded by requiring the name to be distinctive (length ≥ 6 or contains a
capital after the first character and is not a known override).

| Authority | Candidate members (unreferenced elsewhere) |
|---|---|
"""
body_z=''; total=0
external=''
for p in core_files:
    external+='\n'+rd(p)
for p in host_files:
    external+='\n'+rd(p)
for p in test_files:
    external+='\n'+rd(p)
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    members=set(re.findall(r'public\s+(?:static\s+)?(?:async\s+)?[\w<>,\[\]\.\?]+\s+(\w+)\s*\(', t)) | set(re.findall(r'public\s+(?:static\s+)?[\w<>,\[\]\.\?]+\s+(\w+)\s*\{\s*get', t))
    cand=[]
    for m in members:
        if len(m)<6 or m in ('Equals','GetHashCode','ToString','CompareTo','Clone','Dispose'): continue
        if not re.search(r'\b'+re.escape(m)+r'\b', external): cand.append(m)
    total+=len(cand)
    body_z+=f"| `{a}` | {', '.join('`'+c+'`' for c in sorted(cand)[:6]) or '—'} |\n"
hdr_z+=f"\n**Total candidate members across orphans: {total}.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Z_PRUNING_CANDIDATES.md','w',encoding='utf-8').write(hdr_z+body_z)
# AA: time-coupling census
hdr_aa="""# PLAN-ORPHAN-SEAL-01 — Appendix AA: Time-Coupling Census

**Generated:** 2026-09-21. Which orphans expose day/hour stepping methods, so a
seal package knows whether it must attach to the campaign's day loop (Plan 33)
and with what cadence. A system with `TickDay` needs a day owner; one with only
one-shot methods may not.
**Rule:** cadence comes from the method semantics, not from a new timer; the
package cites the existing day owner when wiring.

| Authority | Day-step methods | Hour-step methods | Other step/update methods |
|---|---|---|---|
"""
body_aa=''; day=hour=other=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    methods=re.findall(r'public\s+(?:static\s+)?(?:async\s+)?[\w<>,\[\]\.\?]+\s+(\w+)\s*\(', t)
    d=[m for m in methods if re.search(r'Day|Daily|NewDay',m)]
    h=[m for m in methods if re.search(r'Hour|Hourly',m)]
    o=[m for m in methods if re.search(r'Tick|Update|Advance|Step|Process',m) and m not in d and m not in h]
    if d: day+=1
    if h: hour+=1
    if o: other+=1
    body_aa+=f"| `{a}` | {', '.join('`'+m+'`' for m in d[:4]) or '—'} | {', '.join('`'+m+'`' for m in h[:3]) or '—'} | {', '.join('`'+m+'`' for m in o[:4]) or '—'} |\n"
hdr_aa+=f"\n**{day} orphans expose day-step methods · {hour} hour-step · {other} other step/update methods.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AA_TIME_COUPLING.md','w',encoding='utf-8').write(hdr_aa+body_aa)
for n in ['Y_BATCH_PLAN','Z_PRUNING_CANDIDATES','AA_TIME_COUPLING']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('batches:',len(batches),'pruning candidates:',total,'day-step orphans:',day,'hour-step:',hour)
