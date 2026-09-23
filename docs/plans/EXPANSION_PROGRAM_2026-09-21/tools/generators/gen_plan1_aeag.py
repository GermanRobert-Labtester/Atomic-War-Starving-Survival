import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
main_files=[f for f in os.listdir('src') if f.endswith('.cs')]
routes=sorted(set(re.findall(r'"([a-z][a-z0-9_]{2,})"', rd('src/Main.PlayerSurfaces.cs'))))
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
# AE surface decision queue
rows=[]
for r in orph:
    a=r['auth'][0]; tk=tokens(a)
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    attach=[f for f in main_files if f.startswith('Main.') and dom and dom in f.lower()]
    rt=[x for x in routes if any(w in x for w in tk)]
    if not attach and not rt: rows.append((a,r['file']))
hdr_ae=f"""# PLAN-ORPHAN-SEAL-01 — Appendix AE: Surface Decision Queue

**Generated:** 2026-09-21. Orphans with **neither a candidate host partial nor a
candidate surface route** (Appendices G and N): {len(rows)} of 99. These cannot
be wired mechanically — each needs an EP-01 surface decision before a package
can claim it.
**Options per decision:** (a) add a route through the surface owner (needs that
owner's claim), (b) attach to an existing partial by domain even without a name
match (verify the seam first), (c) ship headless-only with a recorded reason,
(d) decide the system is Core-only and register it per Plan 11's rules.
**Rule:** the decision is recorded in the package — not left implicit.

| Authority | File | Decision options |
|---|---|---|
"""
body_ae=''
for a,f in rows:
    body_ae+=f"| `{a}` | `{f.replace('Assets/Ashfall.Core/','')}` | a / b / c / d — decide at premise check |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AE_SURFACE_DECISIONS.md','w',encoding='utf-8').write(hdr_ae+body_ae)
# AF global seal order
yt=rd(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md')
batches=[]
for m in re.finditer(r'### Batch (\d+) — (\d+) members · risk sum (\d+) · dominant region `([^`]+)`\n\n(.*?)(?=\n### |\Z)', yt, re.S):
    members=[x for x in re.findall(r'\| `([^`]+)` \|', m.group(5)) if any(s in x for s in ('System','Engine','Coordinator','Manager'))]
    batches.append({'n':int(m.group(1)),'risk':int(m.group(3)),'region':m.group(4),'members':members})
ab=rd(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AB_BATCH_PLAN_LINKS.md')
plan_texts={}
for base,_,fs in os.walk('docs/plans'):
    if 'EXPANSION_PROGRAM' not in base: continue
    for f in fs:
        if f.endswith('.md') and 'APPENDIX' not in f.upper() and not f.startswith('README'):
            plan_texts[f.replace('.md','')]=rd(os.path.join(base,f))
score={}
for b in batches:
    unblock=0
    for a in b['members']:
        unblock+=sum(1 for p,t in plan_texts.items() if re.search(r'\b'+re.escape(a)+r'\b',t))
    score[b['n']]=(unblock*3 - b['risk'])
order=sorted(batches,key=lambda b:(-score[b['n']],b['n']))
hdr_af="""# PLAN-ORPHAN-SEAL-01 — Appendix AF: Global Seal Order

**Generated:** 2026-09-21. A recommended global order over Appendix Y's ten
batches: batches that **unblock the most programme plans per unit of risk**
first (`score = 3 × plan-mentions − risk sum`). Within a batch, Appendix Y's
risk order applies; across batches, this order.
**This is a recommendation, not a claim schedule** — the foreman may reorder
for staffing or for a programme deadline.

"""
body_af=''
for i,b in enumerate(order,1):
    body_af+=f"### Priority {i:02d} — Batch {b['n']:02d} (score {score[b['n']]})\n\n"
    body_af+=f"- Region: `{b['region']}` · members {len(b['members'])} · risk sum {b['risk']}\n"
    body_af+=f"- First three members by risk: {', '.join('`'+x+'`' for x in b['members'][:3])}\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AF_SEAL_ORDER.md','w',encoding='utf-8').write(hdr_af+body_af)
# AG loader gaps
data_files=[f for f in os.listdir('Assets/StreamingAssets/Data') if f.endswith('.json')]
core_files=[os.path.join(b,f) for b,_,fs in os.walk('Assets/Ashfall.Core') for f in fs if f.endswith('.cs')]
def catalogs(a):
    tk=tokens(a); return [f for f in data_files if tk and sum(1 for w in tk if w in f.lower())>=max(1,len(tk)-1)][:4]
hdr_ag="""# PLAN-ORPHAN-SEAL-01 — Appendix AG: Loader Gap List

**Generated:** 2026-09-21. For orphan-matched catalogs where **no Core file
mentions the filename** — the data exists but nothing loads it by that name.
Appendix M recorded loader references; this is the zero-loader subset.
**Reading a row:** either the loader constructs the path dynamically (common),
the catalog is consumed through a different name, or the catalog is genuinely
unloaded. The package's first job is to classify its own rows; a genuinely
unloaded catalog is a finding for its content owner, not a wiring task.

| Authority | Catalog | Loader references |
|---|---|---:|
"""
body_ag=''; gaps=0
for r in orph:
    a=r['auth'][0]
    for c in catalogs(a):
        refs=sum(1 for cf in core_files if c in rd(cf))
        if refs==0:
            gaps+=1
            body_ag+=f"| `{a}` | `{c}` | 0 |\n"
hdr_ag+=f"\n**{gaps} catalog rows have zero loader references.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AG_LOADER_GAPS.md','w',encoding='utf-8').write(hdr_ag+body_ag)
for n in ['AE_SURFACE_DECISIONS','AF_SEAL_ORDER','AG_LOADER_GAPS']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('surface decisions:',len(rows),'batches ordered:',len(order),'loader gaps:',gaps)
