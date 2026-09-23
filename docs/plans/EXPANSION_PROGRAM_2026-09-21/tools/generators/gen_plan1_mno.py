import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
data_files=sorted(f for f in os.listdir('Assets/StreamingAssets/Data') if f.endswith('.json'))
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name)
    s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
# M: catalog binding verification
hdr_m="""# PLAN-ORPHAN-SEAL-01 — Appendix M: Catalog Binding Verification

**Generated:** 2026-09-21. Appendix A listed candidate catalogs by name match.
This appendix **verifies** the match: the file exists, its record count (where
JSON-shaped as a list or with a records/entries array), and whether any loader
in Core references that file name. `exists` without a record count means the
file is not a simple record array (an object/scene shape) and the package must
read its actual shape.
**Use:** a seal package with no verified catalog is data-blocked; a package
with a verified catalog and a loader reference has its data path proven.

"""
body_m=''
bound=0
for r in orph:
    a=r['auth'][0]; tk=tokens(a)
    matches=[]
    for f in data_files:
        fl=f.lower()
        hits=sum(1 for w in tk if w in fl)
        if tk and hits>=max(1,len(tk)-1): matches.append(f)
    matches=matches[:5]
    body_m+=f"### `{a}`\n\n"
    if not matches:
        body_m+="No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.\n\n"
        continue
    body_m+='| Catalog | Exists | Records | Loader reference |\n|---|:---:|---:|---|\n'
    for f in matches:
        p=os.path.join('Assets/StreamingAssets/Data',f)
        exists=os.path.exists(p)
        rec='—'
        try:
            d=json.load(open(p,encoding='utf-8'))
            if isinstance(d,list): rec=str(len(d))
            elif isinstance(d,dict):
                for k in ('records','entries','items','rows','definitions'):
                    if isinstance(d.get(k),list): rec=str(len(d[k])); break
                else: rec='object'
        except Exception: rec='unreadable'
        loaders=[]
        for base,_,fs in os.walk('Assets/Ashfall.Core'):
            for cf in fs:
                if cf.endswith('.cs') and f in rd(os.path.join(base,cf)): loaders.append(cf)
        body_m+=f"| `{f}` | {'yes' if exists else '**no**'} | {rec} | {len(loaders)} file(s){': `'+loaders[0]+'`' if loaders else ''} |\n"
    body_m+='\n'
    bound+=1
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-M_CATALOG_BINDING.md','w',encoding='utf-8').write(hdr_m+body_m)
# N: route candidates
route_txt=rd('src/Main.PlayerSurfaces.cs')
routes=sorted(set(re.findall(r'"([a-z][a-z0-9_]{2,})"', route_txt)))
hdr_n=f"""# PLAN-ORPHAN-SEAL-01 — Appendix N: Player-Surface Route Candidates

**Generated:** 2026-09-21. `src/Main.PlayerSurfaces.cs` declares
**{len(routes)} quoted surface route ids**. This appendix matches each orphan's
domain token against the declared route ids, so a package knows whether a
surface already exists or must be added (Appendix C's panel-route pattern).
**Reading a row:** a route match is a name coincidence until the package opens
the route; `—` means no route name in the current surface registry shares the
domain word — the package then either adds one through the surface owner or
ships the seal headless-only with a stated reason.
**Rule:** a new route id is an edit to the surface owner's registry and needs
that owner's claim; routes are not created per orphan by reflex.

| Authority | Candidate route ids |
|---|---|
"""
body_n=''
for r in orph:
    tk=tokens(r['auth'][0])
    hits=[rt for rt in routes if any(w in rt for w in tk)]
    body_n+=f"| `{r['auth'][0]}` | {', '.join('`'+h+'`' for h in hits[:4]) or '—'} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-N_SURFACE_ROUTES.md','w',encoding='utf-8').write(hdr_n+body_n)
# O: verification command set
test_dirs=sorted(d for d in os.listdir('Ashfall.Core.Tests') if os.path.isdir(os.path.join('Ashfall.Core.Tests',d)))
cli=rd('Assets/Ashfall.Core/HostCliRegistry.cs')+rd('src/Host/HostCli.cs')
flags=sorted(set(re.findall(r'--[a-z][a-z0-9-]*-selftest', cli)))
hdr_o=f"""# PLAN-ORPHAN-SEAL-01 — Appendix O: Verification Command Set

**Generated:** 2026-09-21. For each orphan, the **concrete focused commands a
seal package can cite** in its acceptance section: an existing test region (if
any) and an existing headless selftest flag whose name matches the domain
({len(test_dirs)} test regions and {len(flags)} `--*-selftest` flags currently exist).
**Use:** a package that cannot cite a command runs its own focused test block
per `TEST_POLICY.md`; this table prevents inventing commands that do not exist.
**Note:** a flag match is name-level; the package confirms the flag's current
behavior before citing it (Plan 23's selftest truth).

| Authority | Test region | Headless flag candidate | Suggested focused command |
|---|---|---|---|
"""
body_o=''
for r in orph:
    a=r['auth'][0]; tk=tokens(a)
    tdirs=[d for d in test_dirs if any(w in d.lower() for w in tk)]
    fl=[f for f in flags if any(w in f for w in tk)]
    region='`Ashfall.Core.Tests/'+tdirs[0]+'/'+'`' if tdirs else '—'
    cmd=f"`bash scripts/run_test.sh Ashfall.Core.Tests/{tdirs[0]}/`" if tdirs else ('`godot --headless --path . -- '+fl[0]+'`' if fl else 'new focused block required')
    body_o+=f"| `{a}` | {region} | {', '.join('`'+f+'`' for f in fl[:2]) or '—'} | {cmd} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-O_VERIFICATION_COMMANDS.md','w',encoding='utf-8').write(hdr_o+body_o)
for n in ['M_CATALOG_BINDING','N_SURFACE_ROUTES','O_VERIFICATION_COMMANDS']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('catalog-bound orphans:',bound,'routes:',len(routes),'flags:',len(flags),'test regions:',len(test_dirs))
