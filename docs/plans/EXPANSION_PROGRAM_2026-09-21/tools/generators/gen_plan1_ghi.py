import os,re,json,subprocess
root=os.getcwd()
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# host partials + save sections + CLI flags
main_files=sorted(os.path.basename(p) for p in [os.path.join('src',f) for f in os.listdir('src')] if p.endswith('.cs'))
save_txt=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
sections=re.findall(r'new\("([a-z0-9_]+)",\s*"(\w+)",\s*"(\w+)",\s*"([a-z0-9_]+)"', save_txt)
cli_txt=rd('Assets/Ashfall.Core/HostCliRegistry.cs')+rd('src/Host/HostCli.cs')
route_txt=rd('src/Main.PlayerSurfaces.cs')
# G
hdr_g="""# PLAN-ORPHAN-SEAL-01 — Appendix G: Host Integration Point Map

**Generated:** 2026-09-21. For each of the 99 host-unreachable authorities, the
**most plausible existing host attachment points**: a `Main.*` partial with a
matching domain word, registry save sections whose key/file matches, and any
existing CLI flag with the domain word. These are *candidates*: a package
re-verifies the real seam per Plan 1 §3 and Appendix C patterns — the map
exists to make that verification fast and to show where no candidate exists.
**Reading a row:** `—` in a column means the audit found no name match; that is
a finding, not a failure. A row with no host partial and no save section is
likely Core-only or needs a new surface decision (EP-01).

| Authority | Candidate `src/Main.*` partials | Registry sections | CLI flags |
|---|---|---|---|
"""
body_g=''
for r in orph:
    a=r['auth'][0]
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    key=re.sub(r'(system|engine|coordinator|manager)$','',a.lower())
    main_hits=[f for f in main_files if f.startswith('Main.') and (dom in f.lower() or key[:6] in f.lower())]
    sec_hits=[s[0] for s in sections if dom in s[0].lower() or dom in s[3].lower() or key[:8] in s[0].lower()]
    cli_hits=sorted(set(re.findall(r'--[a-z0-9-]*'+re.escape(dom[:6])+r'[a-z0-9-]*', cli_txt)))
    body_g+=f"| `{a}` | {', '.join('`'+h+'`' for h in main_hits[:3]) or '—'} | {', '.join('`'+s+'`' for s in sec_hits[:3]) or '—'} | {', '.join('`'+c+'`' for c in cli_hits[:3]) or '—'} |\n"
# route matches summary
route_hits=len(re.findall(r'"[a-z0-9_]+"', route_txt))
body_g+=f"\n**Player-surface route ids present:** {route_hits} string ids in `src/Main.PlayerSurfaces.cs`; the route column was omitted per-system because route names do not track authority names — each package picks the route at bind time (Appendix C, panel route pattern).\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-G_HOST_INTEGRATION_POINTS.md','w',encoding='utf-8').write(hdr_g+body_g)
# H
hdr_h="""# PLAN-ORPHAN-SEAL-01 — Appendix H: API Surface & Complexity Census

**Generated:** 2026-09-21. Per orphan file: line count, public method count,
public property count, constructor count, static member count, and declared
base type or interfaces. Used to size seal packages (a 2,000-line engine with
no ctor is not a small package) and to spot static state that fights the host
session pattern.

| Authority | Lines | Public methods | Public props | Ctors | Static members | Base/interfaces |
|---|---:|---:|---:|---:|---:|---|
"""
body_h=''
for r in orph:
    t=rd(r['file']); a=r['auth'][0]
    lines=t.count('\n')+1
    pm=len(re.findall(r'public\s+(?:static\s+)?(?:async\s+)?[\w<>,\[\]\.\?]+\s+\w+\s*\(', t))
    pp=len(re.findall(r'public\s+[\w<>,\[\]\.\?]+\s+\w+\s*\{\s*get', t))
    ct=len(re.findall(r'public\s+'+re.escape(a)+r'\s*\(', t))
    st=len(re.findall(r'\bstatic\b', t))
    m=re.search(r'class\s+'+re.escape(a)+r'\s*(?::\s*([^\{]+))?', t)
    base=(m.group(1).strip() if m and m.group(1) else '—')
    body_h+=f"| `{a}` | {lines} | {pm} | {pp} | {ct} | {st} | {base[:70]} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-H_API_SURFACE.md','w',encoding='utf-8').write(hdr_h+body_h)
# I
hdr_i="""# PLAN-ORPHAN-SEAL-01 — Appendix I: Provenance & Staleness Ledger

**Generated:** 2026-09-21. Last commit (short hash, date) per orphan file plus
line count, giving a staleness signal: an orphan untouched since the Unity era
is a different risk from one touched during the Godot campaign. Re-run this
ledger before promoting any seal package; a changed hash invalidates that
package's premise check.
**Note:** dates are repository history, not gameplay claims.

| Authority | Last commit | Date | Lines |
|---|---|---|---:|
"""
body_i=''
rows_i=[]
for r in orph:
    try:
        out=subprocess.run(['git','log','-1','--format=%h|%ad','--date=short','--',r['file']],
                           capture_output=True,text=True,timeout=20).stdout.strip()
        h,d=(out.split('|')+['?','?'])[:2] if out else ('?','?')
    except Exception:
        h,d='?','?'
    lines=rd(r['file']).count('\n')+1
    rows_i.append((r['auth'][0],h,d,lines))
for a,h,d,l in sorted(rows_i,key=lambda x:x[2]):
    body_i+=f"| `{a}` | `{h}` | {d} | {l} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-I_PROVENANCE.md','w',encoding='utf-8').write(hdr_i+body_i)
for n in ['G_HOST_INTEGRATION_POINTS','H_API_SURFACE','I_PROVENANCE']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
