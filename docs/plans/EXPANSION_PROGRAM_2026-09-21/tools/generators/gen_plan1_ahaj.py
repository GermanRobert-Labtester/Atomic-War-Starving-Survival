import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
reg=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
entries=re.findall(r'new\("([a-z0-9_]+)",\s*"(\w+)",\s*"(\w+)",\s*"([a-z0-9_]+)"', reg)
groups={}
for k,s,st,g in entries: groups[g]=groups.get(g,0)+1
filenames=set(re.findall(r'\{\s*"[a-z0-9_]+"\s*,\s*"([a-z0-9_\.]+\.json)"\s*\}', reg))
def propose(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name)
    return re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
# AH
hdr=f"""# PLAN-ORPHAN-SEAL-01 — Appendix AH: Lifecycle & File-Name Assignment

**Generated:** 2026-09-21. For each stateful orphan (Appendix D/AC), the closest
existing **lifecycle group** (from the registry's {len(entries)} metadata rows:
{', '.join(sorted(groups))}) and a proposed section file name following the
`*_save.json` convention.
**Use:** a package does not invent a group or a filename; it adopts the row
below or records why it deviates. A proposed filename already in the registry is
a collision to resolve before the save work starts.

| Authority | Proposed key | Suggested lifecycle group | Proposed file | File collision |
|---|---|---|---|---|
"""
body=''; collisions=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    if not re.search(r'(?:Capture|Restore|Save|Load)\w*\s*\(', t): continue
    pk=propose(a)
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    tk=tokens(a)
    cand=sorted(groups, key=lambda g: -(sum(1 for w in tk if w in g) + (3 if g==dom else 0)))
    g=cand[0] if cand else 'holdfast'
    fn=pk+'_save.json'
    col='**collides**' if fn in filenames else '—'
    if fn in filenames: collisions+=1
    body+=f"| `{a}` | `{pk}` | `{g}` | `{fn}` | {col} |\n"
hdr+=f"\n**{collisions} proposed filenames collide with existing registry files.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AH_LIFECYCLE_FILES.md','w',encoding='utf-8').write(hdr+body)
# AI: setup/save method name proposals + collision
host_setups=set()
for f in os.listdir('src'):
    if f.endswith('.cs'):
        host_setups|=set(re.findall(r'void\s+(Setup\w+)\s*\(', rd(os.path.join('src',f))))
hdr2=f"""# PLAN-ORPHAN-SEAL-01 — Appendix AI: Host Method-Name Proposals

**Generated:** 2026-09-21 against the host's current {len(host_setups)} `Setup*`
methods. For each orphan, proposed host method names following the established
conventions (`Setup<Name>` / `Save<Name>`) and whether the name is already
taken.
**Use:** the naming convention is the host's; a package proposes a name from
this table or records a reason for deviating. A collision means the package must
choose a distinct name (and the gate in Plan 71 will otherwise flag a duplicate).

| Authority | Proposed Setup name | Collides? | Proposed Save name | Collides? |
|---|---|---|---|---|
"""
def prop_setup(a):
    n=re.sub(r'(System|Engine|Coordinator|Manager)$','',a)
    return 'Setup'+n
body2=''; c1=c2=0
for r in orph:
    a=r['auth'][0]
    su=prop_setup(a); sv='Save'+re.sub(r'(System|Engine|Coordinator|Manager)$','',a)
    col1=su in host_setups; col2=sv in host_setups
    if col1: c1+=1
    if col2: c2+=1
    body2+=f"| `{a}` | `{su}` | {'**taken**' if col1 else '—'} | `{sv}` | {'**taken**' if col2 else '—'} |\n"
hdr2+=f"\n**{c1} proposed Setup names and {c2} Save names are already taken.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AI_METHOD_NAMES.md','w',encoding='utf-8').write(hdr2+body2)
# AJ: appendix maintenance map
rows=[
 ('A','Orphan dossiers','reachability audit + tests/catalog scan','after any seal batch or audit change'),
 ('B','Wave packages','manual (authoring)','when a wave package lands or splits'),
 ('C','Integration patterns','manual (authoring)','when a pattern is proven or rejected'),
 ('D','Save ownership','orphan sources (capture/restore + registry refs)','after save-registry changes'),
 ('E','Determinism audit','orphan sources (banned primitives)','after any orphan edit'),
 ('F','Dependency clusters','intra-orphan reference graph','after any orphan edit'),
 ('G','Integration points','host partials + registry + CLI flags','after host/registry growth'),
 ('H','API surface','orphan sources (size/members)','after any orphan edit'),
 ('I','Provenance','git log per orphan file','before promoting any claim'),
 ('J','Test coverage','test files referencing orphans','after test additions'),
 ('K','API signatures','orphan sources (public members)','after any orphan edit'),
 ('L','Risk scorecard','D/E/G/H/J inputs','after any input appendix refresh'),
 ('M','Catalog binding','Data files + loader scan','after data additions'),
 ('N','Surface routes','Main.PlayerSurfaces route ids','after route additions'),
 ('O','Verification commands','test regions + CLI flags','after test/flag additions'),
 ('P','Inbound references','reachability closure re-run','after any orphan seal'),
 ('Q','Save-key collisions','registry keys/aliases','after save-registry changes'),
 ('R','Catalog shapes','matched catalog JSON','after catalog edits'),
 ('S','Test regions','test files per region','after test additions'),
 ('T','Worked exemplars','derived from A–U','after any input appendix refresh'),
 ('U','Data references','JSON literals vs recursive data tree','after data additions'),
 ('V','Master worklist','L/D/Q/M/N/O inputs','after any input appendix refresh'),
 ('W','Data ids','id literals vs 4,750 data ids','after data additions'),
 ('X','Static hazards','orphan static fields','after any orphan edit'),
 ('Y','Batch plan','risk-sorted distribution','after L changes'),
 ('Z','Shared shapes','public member shapes across orphans','after any orphan edit'),
 ('AA','Time coupling','day/hour step methods','after any orphan edit'),
 ('AB','Batch-plan links','batches × plan bodies','after new plans land'),
 ('AC','Save signatures','capture/restore signatures','after any orphan edit'),
 ('AD','Batch verification','Y + O inputs','after Y or O changes'),
 ('AE','Surface decisions','G/N inputs','after host/route growth'),
 ('AF','Seal order','AB/Y/L inputs','after AB/Y/L changes'),
 ('AG','Loader gaps','M + loader scan','after data or loader changes'),
]
hdr3="""# PLAN-ORPHAN-SEAL-01 — Appendix AJ: Appendix Maintenance Map

**Generated:** 2026-09-21. A maintenance index for this kit: each appendix, what
it derives from, and the event that invalidates it. A successor agent re-runs
the listed derivation after the trigger rather than trusting a stale table.
**Rule:** regeneration always precedes claim promotion when the trigger listed
here has occurred.

| Appendix | Content | Derives from | Re-run trigger |
|---|---|---|---|
"""
body3=''.join(f"| {a} | {c} | {d} | {t} |\n" for a,c,d,t in rows)
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AJ_MAINTENANCE_MAP.md','w',encoding='utf-8').write(hdr3+body3)
for n in ['AH_LIFECYCLE_FILES','AI_METHOD_NAMES','AJ_MAINTENANCE_MAP']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('groups:',sorted(groups),'file collisions:',collisions,'setup names:',len(host_setups),'setup collisions:',c1,'save collisions:',c2)
