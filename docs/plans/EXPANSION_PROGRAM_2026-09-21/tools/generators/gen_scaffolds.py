import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
BASE='docs/plans/'
PLANS=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-COMBAT-DEPTH-62.md', ['Combat'], ['combat','weapon','ballistic','breach','tactical'], 'Combat', 'CD-62'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-MENTAL-HEALTH-THERAPY-64.md', ['Psychology','Needs','Sanatorium','Medical'], ['therap','mental','psych','sanator'], 'Psychology', 'MH-64'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-COLLECTIBLES-RELICS-67.md', ['Collectibles','Narrative','World'], ['collectible','relic','heirloom','trophy'], 'Collectibles', 'CR-67'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-LABOUR-PROFESSIONS-68.md', ['DutyRoster','Survivors','Economy'], ['profession','labour','labor','rolecatalog','duty'], 'DutyRoster', 'LP-68'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-CARTOGRAPHY-LANDMARKS-70.md', ['World','Exploration'], ['cartograph','landmark','mapcatalog','route'], 'World', 'CL-70'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-THREADING-ASYNCHRONY-72.md', [], ['async','thread','concurrent','background'], 'Performance', 'TA-72'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-DEV-TOOLING-TRUTH-75.md', ['Performance'], ['console','overlay','inspector','demo','debugtool'], 'Performance', 'DT-75'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-CONTENT-PIPELINE-QA-77.md', ['Content'], ['content','validat','gate','utilization'], 'Content', 'CP-77'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-BIONICS-ENHANCEMENT-78.md', ['Medical'], ['prosthe','bionic','graft','implant','rehab'], 'Medical', 'BE-78'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-AUTONOMOUS-MACHINES-79.md', ['Crafting','Foundry','Shelter','AdvancedMachinery'], ['robot','machine','autom','drone','sentry'], 'Crafting', 'AM-79'),
]
main_files=[f for f in os.listdir('src') if f.endswith('.cs')]
data_pairs=[]
for base,_,fs in os.walk('Assets/StreamingAssets/Data'):
    for f in fs:
        if f.endswith('.json'): data_pairs.append((f,os.path.join(base,f)))
def rec_count(p):
    try:
        d=json.load(open(p,encoding='utf-8'))
        if isinstance(d,list): return str(len(d))
        if isinstance(d,dict):
            for k in ('records','entries','items','rows','definitions','waves'):
                if isinstance(d.get(k),list): return str(len(d[k]))
            return 'object('+str(len(d))+' keys)'
    except Exception: return 'unreadable'
    return '—'
made=[]
for plan_path, dirs, tokens, region, pid in PLANS:
    plan_txt=rd(plan_path)
    if not plan_txt: print('MISSING PLAN',plan_path); continue
    base_name=os.path.basename(plan_path).replace('.md','')
    app=f"{base_name}_APPENDIX-A_SCAFFOLD.md"
    if app in plan_txt: print('already linked',base_name); continue
    # source inventory
    files=[]
    roots=['Assets/Ashfall.Core']+[os.path.join('Assets/Ashfall.Core',d) for d in dirs]
    for root in set(roots):
        if not os.path.isdir(root): continue
        for b,_,fs in os.walk(root):
            for f in fs:
                if not f.endswith('.cs'): continue
                bn=f[:-3].lower()
                if dirs and root.startswith('Assets/Ashfall.Core/') and b==root:
                    files.append(os.path.join(b,f))
                elif any(t in bn for t in tokens):
                    files.append(os.path.join(b,f))
    files=sorted(set(files))
    tests=[]
    for b,_,fs in os.walk('Ashfall.Core.Tests'):
        for f in fs:
            if f.endswith('.cs') and any(t in f.lower() for t in tokens): tests.append(os.path.join(b,f))
    tests=sorted(set(tests))
    cats=[(f,p) for f,p in data_pairs if any(t in f.lower() for t in tokens)][:6]
    partials=[f for f in main_files if any(t in f.lower() for t in tokens)]
    # extract plan packages for fixtures
    pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', plan_txt)[:6]
    hdr=f"""# {base_name} — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory ({len(files)} files)

| File | Lines |
|---|---:|
"""
    body=hdr
    for p in files[:40]:
        body+=f"| `{p.replace('Assets/Ashfall.Core/','')}` | {rd(p).count(chr(10))+1} |\n"
    if len(files)>40: body+=f"\n… and {len(files)-40} more.\n"
    body+="\n## 2. Data bindings\n\n"
    if cats:
        body+="| Catalog | Records/shape |\n|---|---|\n"
        for f,p in cats: body+=f"| `{f}` | {rec_count(p)} |\n"
    else: body+="No catalog name-matched the domain tokens; verify loader paths before claiming a data dependency.\n"
    body+="\n## 3. Host attachment\n\n"
    body+=f"- Candidate host partials: {', '.join('`'+x+'`' for x in partials[:4]) or 'none — new attachment or headless-only\n'}\n"
    body+=f"- Proposed setup method: `Setup{region}Scaffold` · proposed save method: `Save{region}Scaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)\n"
    body+=f"- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.\n"
    body+="\n## 4. Test scaffold (proposed, not created)\n\n"
    body+=f"Proposed file: `Ashfall.Core.Tests/{region}/{pid.replace('-','')}ScaffoldTests.cs`\n\n```csharp\n// Scaffold skeleton — derived from this plan's packages below.\n// Fixtures assert one package each; replace the TODO bodies at claim time.\npublic class {pid.replace('-','')}ScaffoldTests\n{{\n"
    for code,desc in pkgs:
        name=re.sub(r'[^A-Za-z0-9]','',code)
        body+=f"    [Fact] public void {name}_TODO() {{ /* {desc[:80]} */ }}\n"
    body+="}\n```\n"
    body+="\n## 5. Commands\n\n"
    body+=f"- `bash scripts/run_test.sh Ashfall.Core.Tests/{region}/`{' (create the region if absent)' if not os.path.isdir('Ashfall.Core.Tests/'+region) else ''}\n"
    body+=f"- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).\n"
    body+=f"- `python3 scripts/ci/generate-docs-index.py --check` if docs change.\n"
    body+="\n## 6. Scaffolding rules\n\n"
    body+="1. No production file is created before a claim; this appendix is documentation.\n"
    body+="2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).\n"
    body+="3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.\n"
    body+="4. The scaffold is regenerated, not edited — see Appendix AM.\n"
    out=os.path.join(os.path.dirname(plan_path), app)
    open(out,'w',encoding='utf-8').write(body)
    made.append((out,len(body)))
    # link into plan
    lines=plan_txt.splitlines(keepends=True); outl=[]; ins=False
    for ln in lines:
        if not ins and ln.startswith('**Non-goals:**'):
            outl.append(f"**Implementation scaffold:** [`{app}`]({app}) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).\n\n")
            ins=True
        outl.append(ln)
    if ins: open(plan_path,'w',encoding='utf-8').write(''.join(outl))
    print(f"{base_name}: files={len(files)} tests={len(tests)} cats={len(cats)} partials={len(partials)} pkgs={len(pkgs)} -> {len(body)} chars, linked={ins}")
