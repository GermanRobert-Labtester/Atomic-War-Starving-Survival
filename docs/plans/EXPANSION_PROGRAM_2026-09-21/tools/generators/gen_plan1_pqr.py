import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
orphan_files={r['file'] for r in orph}
# P: incoming references from non-orphan files
core_files=[]; host_files=[]
for base,_,fs in os.walk('Assets/Ashfall.Core'):
    for f in fs:
        p=os.path.join(base,f)
        if f.endswith('.cs') and p not in orphan_files: core_files.append(p)
for base,_,fs in os.walk('src'):
    for f in fs:
        if f.endswith('.cs'): host_files.append(os.path.join(base,f))
hdr_p="""# PLAN-ORPHAN-SEAL-01 — Appendix P: Incoming Reference Census

**Generated:** 2026-09-21. Who is **waiting** on each orphan: references from
host (`src/**`) and from Core files that are not themselves orphaned. Appendix F
mapped orphan→orphan edges; this appendix maps reachable→orphan edges, which is
the strongest argument for sealing — a reachable consumer referencing an
unreachable authority is a partially-wired feature, not dead code.
**Findings:** {waiting} of {total} orphans are referenced by at least one
reachable file; {islands} have no inbound reference outside the orphan set.
**Use:** seal packages for the {waiting} start with their consumer list as the
integration target; islands need an EP-01 surface decision before wiring.

"""
def refs(name):
    out=[]
    for p in host_files:
        t=rd(p); c=len(re.findall(r'\b'+re.escape(name)+r'\b', t))
        if c: out.append(('HOST',os.path.relpath(p),c))
    for p in core_files:
        t=rd(p); c=len(re.findall(r'\b'+re.escape(name)+r'\b', t))
        if c: out.append(('CORE',os.path.relpath(p,'Assets/Ashfall.Core'),c))
    return out
waiting=0
body_p=''
for r in orph:
    a=r['auth'][0]; rr=refs(a)
    if rr: waiting+=1
    body_p+=f"### `{a}`\n\n"
    if not rr:
        body_p+="No reference from any reachable host or Core file. An island: the package decides the surface before wiring (EP-01).\n\n"
        continue
    body_p+='| Side | File | References |\n|:---:|---|---:|\n'
    for side,f,c in sorted(rr,key=lambda x:-x[2])[:8]:
        body_p+=f"| {side} | `{f}` | {c} |\n"
    body_p+='\n'
hdr_p=hdr_p.format(waiting=waiting,total=len(orph),islands=len(orph)-waiting)
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-P_INCOMING_REFERENCES.md','w',encoding='utf-8').write(hdr_p+body_p)
# Q: save key proposal + collisions
save_txt=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
keys=set(re.findall(r'new\("([a-z0-9_]+)"', save_txt))
aliases=set(re.findall(r'\{\s*"([a-z0-9_]+)"\s*,\s*"([a-z0-9_]+)"\s*\}', save_txt))
alias_keys={a for pair in aliases for a in pair}
def propose(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name)
    s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return s
hdr_q=f"""# PLAN-ORPHAN-SEAL-01 — Appendix Q: Save-Key Proposal & Collision Map

**Generated:** 2026-09-21 against the current registry ({len(keys)} keys,
{len(alias_keys)} alias forms). For each orphan, a proposed snake_case key (type
name minus authority suffix) checked for **exact key collision**, **alias
collision**, or near-match with an existing key.
**Rule:** an orphan with a collision does **not** get a new key — its package
fits into the existing section or adds fields to it. Only unversioned-new keys
follow Plan 87's ladder policy.
**Use:** stateful orphans (Appendix D) plan their save row from this table; a
collision row is a design constraint, not a naming problem.

| Authority | Proposed key | Collision | Nearest existing key |
|---|---|---|---|
"""
body_q=''
coll=0
for r in orph:
    a=r['auth'][0]; pk=propose(a)
    col='—'
    if pk in keys: col='**exact key**'; coll+=1
    elif pk in alias_keys: col='**alias**'; coll+=1
    near=''
    for k in sorted(keys|alias_keys):
        if k!=pk and (k.startswith(pk[:6]) or pk.startswith(k[:6])):
            near=k; break
    if col!='—': pass
    body_q+=f"| `{a}` | `{pk}` | {col} | {('`'+near+'`') if near else '—'} |\n"
hdr_q=hdr_q.replace('{coll}',str(coll)) if '{coll}' in hdr_q else hdr_q
body_q=f"\n**Collisions: {coll} orphans propose a key that already exists as a registry key or alias.**\n\n"+body_q
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Q_SAVE_KEY_COLLISIONS.md','w',encoding='utf-8').write(hdr_q+body_q)
# R: bound catalog shapes
data_dir='Assets/StreamingAssets/Data'
data_files=sorted(f for f in os.listdir(data_dir) if f.endswith('.json'))
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name)
    s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
hdr_r="""# PLAN-ORPHAN-SEAL-01 — Appendix R: Bound Catalog Shapes

**Generated:** 2026-09-21. For orphans with catalog matches (Appendix M), the
**shape** of each matched catalog: top-level keys (objects) or element keys
(arrays), so a loader can be written without opening files one by one.
**Use:** a package whose loader must read one of these files starts from this
shape; `array[N]` rows show the record count and the representative element
keys.

"""
body_r=''
for r in orph:
    a=r['auth'][0]; tk=tokens(a)
    matches=[f for f in data_files if tk and sum(1 for w in tk if w in f.lower())>=max(1,len(tk)-1)][:3]
    if not matches: continue
    body_r+=f"### `{a}`\n\n"
    for f in matches:
        try: d=json.load(open(os.path.join(data_dir,f),encoding='utf-8'))
        except Exception:
            body_r+=f"- `{f}` — unreadable\n"; continue
        if isinstance(d,dict):
            ks=', '.join('`'+k+'`' for k in list(d.keys())[:10])
            body_r+=f"- `{f}` — object keys: {ks}\n"
        elif isinstance(d,list):
            ek=', '.join('`'+k+'`' for k in list(d[0].keys())[:10]) if d and isinstance(d[0],dict) else '—'
            body_r+=f"- `{f}` — array[{len(d)}]; element keys: {ek}\n"
        else:
            body_r+=f"- `{f}` — scalar\n"
    body_r+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-R_CATALOG_SHAPES.md','w',encoding='utf-8').write(hdr_r+body_r)
for n in ['P_INCOMING_REFERENCES','Q_SAVE_KEY_COLLISIONS','R_CATALOG_SHAPES']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('waiting:',waiting,'islands:',len(orph)-waiting,'key collisions:',coll)
