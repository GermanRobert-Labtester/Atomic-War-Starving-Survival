import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
PLANS=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-MUTATION-HEREDITY-81.md',['Medical'],['mutat','gene','heredit','congenital','dna'],'Medical','MH-81','PLAN-PANDEMIC-PUBLIC-HEALTH-47','docs/plans/EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-PANDEMIC-PUBLIC-HEALTH-47.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-NOMADS-CARAVAN-CULTURE-82.md',['Economy','Visitors','Settlements'],['nomad','caravan','migration','visitor'],'Economy','NC-82','PLAN-FACTION-BRANCH-TRUTH-171','docs/plans/EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-FACTION-BRANCH-TRUTH-171.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-DEEP-STRATA-83.md',['Excavation','Subterranean','Shelter'],['subterran','strata','deepwell','tunnel','geotherm'],'Shelter','DS-83','PLAN-AQUIFER-MONITORING-TRUTH-164','docs/plans/EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-AQUIFER-MONITORING-TRUTH-164.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-ANCIENT-RUINS-VAULTS-84.md',['Archaeology','Narrative','Expeditions'],['ruin','vault','relic','ancient','archaeo'],'Narrative','AR-84','PLAN-ARCHAEOLOGY-TRUTH-152','docs/plans/EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-ARCHAEOLOGY-TRUTH-152.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-HOST-CLI-CONTRACT-86.md',[],['hostcli','commandline','descriptor'],'Host','HC-86','PLAN-HOST-COMPOSITION-GOVERNANCE-71','docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-HOST-COMPOSITION-GOVERNANCE-71.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-SAVE-MIGRATION-CORRIDOR-87.md',['Save'],['envelope','schema','slot','migration'],'Save','SM-87','PLAN-SAVE-GOVERNANCE-12','docs/plans/EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-SAVE-GOVERNANCE-12.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-DETERMINISM-CROSS-HOST-89.md',['Random','Save'],['rng','determin','checksum','replay'],'Save','DH-89','PLAN-DETERMINISM-REPLAY-13','docs/plans/EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-DETERMINISM-REPLAY-13.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-DATA-SCHEMA-COVERAGE-90.md',['IO'],['schema','normaliz','loadresult'],'IO','DS2-90','PLAN-DATA-AUTHORITY-14','docs/plans/EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-DATA-AUTHORITY-14.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-HOST-EVENT-ARCHIVE-91.md',['Telemetry','Journal'],['telemetry','recorder','journalcorpus','metric'],'Telemetry','HE-91','PLAN-TELEMETRY-PRIVACY-58','docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-TELEMETRY-PRIVACY-58.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-MOD-CONTENT-BOUNDARY-92.md',['Mods'],['mod','layering','compat'],'Mods','MB-92','PLAN-CONTENT-PIPELINE-QA-77','docs/plans/EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-CONTENT-PIPELINE-QA-77.md'),
]
main_files=[f for f in os.listdir('src') if f.endswith('.cs')]
data_pairs=[(f,os.path.join(b,f)) for b,_,fs in os.walk('Assets/StreamingAssets/Data') for f in fs if f.endswith('.json')]
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
for plan_path,dirs,tokens,region,pid,auth_name,auth_path in PLANS:
    plan_txt=rd(plan_path)
    base_name=os.path.basename(plan_path).replace('.md','')
    app=f"{base_name}_APPENDIX-A_SCAFFOLD.md"
    if app in plan_txt: print('linked already:',base_name); continue
    auth_txt=rd(auth_path)
    auth_pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', auth_txt)[:3]
    pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', plan_txt)[:6]
    files=[]
    for root in ['Assets/Ashfall.Core']+[os.path.join('Assets/Ashfall.Core',d) for d in dirs]:
        if not os.path.isdir(root): continue
        for b,_,fs in os.walk(root):
            for f in fs:
                if not f.endswith('.cs'): continue
                bn=f[:-3].lower()
                if dirs and root.startswith('Assets/Ashfall.Core/') and b==root: files.append(os.path.join(b,f))
                elif any(t in bn for t in tokens): files.append(os.path.join(b,f))
    files=sorted(set(files))
    tests=sorted(set(os.path.join(b,f) for b,_,fs in os.walk('Ashfall.Core.Tests') for f in fs if f.endswith('.cs') and any(t in f.lower() for t in tokens)))
    cats=[(f,p) for f,p in data_pairs if any(t in f.lower() for t in tokens)][:6]
    partials=[f for f in main_files if any(t in f.lower() for t in tokens)]
    b_=f"""# {base_name} — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant).
**Scaffolding authority:** [`{auth_name}`]({os.path.relpath(auth_path,os.path.dirname(plan_path))}) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
"""
    for code,desc in auth_pkgs:
        b_+=f"| `{code}` | {desc[:110]} |\n"
    b_+=f"\n## 2. Source inventory ({len(files)} files)\n\n| File | Lines |\n|---|---:|\n"
    for p in files[:40]: b_+=f"| `{p.replace('Assets/Ashfall.Core/','')}` | {rd(p).count(chr(10))+1} |\n"
    if len(files)>40: b_+=f"\n… and {len(files)-40} more.\n"
    b_+="\n## 3. Data bindings\n\n"
    if cats:
        b_+="| Catalog | Records/shape |\n|---|---|\n"
        for f,p in cats: b_+=f"| `{f}` | {rec_count(p)} |\n"
    else: b_+="No catalog name-matched; verify loader paths before claiming a data dependency.\n"
    b_+=f"\n## 4. Host attachment\n\n- Candidate host partials: {', '.join('`'+x+'`' for x in partials[:4]) or 'none — new attachment or headless-only'}\n"
    b_+=f"- Proposed method names: `Setup{region}{pid.split('-')[0]}Scaffold` / `Save{region}{pid.split('-')[0]}Scaffold` (Plan 1 Appendix AI: collision-free)\n"
    b_+=f"- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.\n"
    b_+="\n## 5. Test scaffold (proposed, not created)\n\n"
    cls=pid.replace('-','')
    b_+=f"Proposed file: `Ashfall.Core.Tests/{region}/{cls}ScaffoldTests.cs`\n\n```csharp\n// One fixture per package, plus one authority-conformance fixture.\npublic class {cls}ScaffoldTests\n{{\n"
    for code,desc in pkgs:
        b_+=f"    [Fact] public void {re.sub(r'[^A-Za-z0-9]','',code)}_TODO() {{ /* {desc[:80]} */ }}\n"
    b_+=f"    [Fact] public void {cls}_AuthorityConformance_TODO() {{ /* pattern parity with {auth_name} */ }}\n""}}\n```\n"
    b_+=f"\n## 6. Commands\n\n- `bash scripts/run_test.sh Ashfall.Core.Tests/{region}/`{' (create the region if absent)' if not os.path.isdir('Ashfall.Core.Tests/'+region) else ''}\n"
    b_+="- Regenerate, do not edit (Plan 1 Appendices AJ/AM).\n- `python3 scripts/ci/generate-docs-index.py --check` if docs change.\n"
    b_+="\n## 7. Scaffolding rules\n\n1. No production file before a claim.\n2. The authority's patterns win over new ones; a deviation is recorded with a reason.\n3. Every fixture maps to a package; a package without a fixture is a finding.\n"
    open(os.path.join(os.path.dirname(plan_path),app),'w',encoding='utf-8').write(b_)
    made.append((app,len(b_)))
    lines=plan_txt.splitlines(keepends=True); out=[]; ins=False
    for ln in lines:
        if not ins and ln.startswith('**Non-goals:**'):
            out.append(f"**Implementation scaffold:** [`{app}`]({app}) — paired with `{auth_name}` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).\n\n")
            ins=True
        out.append(ln)
    if ins: open(plan_path,'w',encoding='utf-8').write(''.join(out))
    print(f"{base_name}: files={len(files)} cats={len(cats)} tests={len(tests)} auth={auth_name} -> {len(b_)} chars, linked={ins}")
