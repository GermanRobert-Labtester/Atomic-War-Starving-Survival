import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
PLANS=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-TEXT-PACK-LOCALIZATION-88.md',['Localization'],['l10n','localis','localiz','textpack'],'Localization','TP-88','PLAN-LOCALIZATION-READINESS-52','docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-LOCALIZATION-READINESS-52.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-INVENTORY-CONSERVATION-93.md',['Inventory'],['inventory','item','stack','container'],'Inventory','IC-93','PLAN-CRAFT-QUALITY-TRUTH-112','docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CRAFT-QUALITY-TRUTH-112.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-DEPRECATED-TREE-RETIREMENT-94.md',[],['deprecated','retire'],'Performance','DR-94','PLAN-ORPHAN-SEAL-01','docs/plans/EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-SPATIAL-SIM-AUTHORITY-95.md',['World'],['route','territor','settlement','mapregion'],'World','SS-95','PLAN-WAYSTATION-NETWORK-TRUTH-153','docs/plans/EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-WAYSTATION-NETWORK-TRUTH-153.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-ECONOMY-LEDGER-TRUTH-96.md',['Economy'],['ledger','market','credit','funds','price'],'Economy','EL-96','PLAN-ECONOMY-DATA-FAMILY-TRUTH-270','docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-ECONOMY-DATA-FAMILY-TRUTH-270.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-AUDIO-MIX-AUTHORITY-97.md',['Audio'],['audio','cue','ambience','acoustic','cassette'],'Audio','AMX-97','PLAN-AUDIO-CONDITION-TRUTH-255','docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-AUDIO-CONDITION-TRUTH-255.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98.md',['Save'],['envelope','checksum','fuzz','corrupt','migration'],'Save','SF-98','PLAN-SAVE-MIGRATION-CORRIDOR-87','docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-SAVE-MIGRATION-CORRIDOR-87.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-HOTFIX-DRILL-99.md',[],['release','hotfix','changelog'],'Release','HD-99','PLAN-RELEASE-OPS-20','docs/plans/EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-RELEASE-OPS-20.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-PROGRAMME-CLOSEOUT-100.md',[],['programme','closeout','ledger'],'Governance','PC-100','PLAN-AGENT-WORKFLOW-GOVERNANCE-59','docs/plans/EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-AGENT-WORKFLOW-GOVERNANCE-59.md'),
 ('docs/plans/EXPANSION_PROGRAM_WAVE11_2026-09-21/PLAN-PLAYER-COMMAND-TRUTH-131.md',['PlayerCommand'],['command','preview','actionlog'],'PlayerCommand','PM-131','PLAN-SILENT-FAILURE-35','docs/plans/EXPANSION_PROGRAM_WAVE4_2026-09-21/PLAN-SILENT-FAILURE-35.md'),
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
def extra(pid):
    if pid=='TP-88':
        try: lz=json.load(open('artifacts/l10n-inventory.json'))
        except Exception: return ''
        per=sorted(lz.get('literal_counts_by_panel',{}).items(),key=lambda kv:-kv[1])[:6]
        t=f"\n## 3b. L10n inventory artifact\n\nRecords {lz.get('record_count')} · hardcoded {lz.get('hardcoded_literal_count')} · lookups {lz.get('localized_lookup_count')} · UI files {lz.get('ui_file_count')} · pilot {', '.join(lz.get('pilot_panels',[]))}.\n\nTop panels by literals: "+', '.join(f"`{p}` {c}" for p,c in per)+".\n"
        return t
    if pid=='DR-94':
        game=[os.path.join(b,f) for b,_,fs in os.walk('Assets/_Game') for f in fs]
        unity=0
        for root in ['Assets/Ashfall.Core','src','Ashfall.Core.Tests']:
            for b,_,fs in os.walk(root):
                for f in fs:
                    if f.endswith('.cs'):
                        t=rd(os.path.join(b,f))
                        unity+=len(re.findall(r'^(?!.*//).*?(using UnityEngine|UnityEngine\.|UNITY_5_3_OR_NEWER)',t,re.M))
        cs=rd('Ashfall.csproj'); tc=rd('Ashfall.Core.Tests/Ashfall.Core.Tests.csproj')
        return f"\n## 3b. Deprecation surface\n\n| Asset | Detail |\n|---|---|\n| `Assets/_Game/` files | {len(game)}: {', '.join('`'+os.path.basename(x)+'`' for x in sorted(game)[:5])} |\n| Non-comment Unity refs (repo) | {unity} |\n| Game csproj globs | {len(re.findall(r'<Compile Include=', cs))} |\n| Test csproj `_Game` include | {'yes' if '_Game' in tc else 'no'} |\n| `src/Bridge/` | {'present' if os.path.isdir('src/Bridge') else 'absent'} |\n"
    if pid=='HD-99':
        rows=[]
        for b,_,fs in os.walk('scripts'):
            for f in fs:
                if any(k in f.lower() for k in ('release','hotfix','changelog')):
                    p=os.path.join(b,f); rows.append((p,len(rd(p).splitlines())))
        return "\n## 3b. Release tooling\n\n| Script | Lines |\n|---|---:|\n"+''.join(f"| `{p}` | {l} |\n" for p,l in sorted(rows))+"\n"
    if pid=='PC-100':
        waves=len([d for d in os.listdir('docs/plans') if d.startswith('EXPANSION_PROGRAM')])
        plans=len([f for b,_,fs in os.walk('docs/plans') for f in fs if f.startswith('PLAN-') and f.endswith('.md') and 'APPENDIX' not in f])
        apps=len([f for b,_,fs in os.walk('docs/plans') for f in fs if 'APPENDIX' in f and f.endswith('.md')])
        return f"\n## 3b. Programme self-counts\n\n| Metric | Value |\n|---|---:|\n| Programme directories | {waves} |\n| Plan files | {plans} |\n| Appendix files | {apps} |\n| Versioned generators | {len(os.listdir('docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/generators'))} |\n"
    return ''
made=[]
for plan_path,dirs,tokens,region,pid,auth_name,auth_path in PLANS:
    plan_txt=rd(plan_path)
    base_name=os.path.basename(plan_path).replace('.md','')
    app=f"{base_name}_APPENDIX-A_SCAFFOLD.md"
    auth_txt=rd(auth_path)
    auth_pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', auth_txt)[:3]
    pkgs=re.findall(r'-\s+\*\*([A-Z0-9\-]+)\*\*\s+([^\n]+)', plan_txt)[:6]
    files=[]
    for root in ['Assets/Ashfall.Core','src']+[os.path.join('Assets/Ashfall.Core',d) for d in dirs]:
        if not os.path.isdir(root): continue
        for b,_,fs in os.walk(root):
            for f in fs:
                if not f.endswith('.cs'): continue
                bn=f[:-3].lower(); full=os.path.join(b,f)
                if dirs and root.startswith('Assets/Ashfall.Core/') and b==root: files.append(full)
                elif any(t in bn for t in tokens): files.append(full)
    files=sorted(set(files))
    cats=[(f,p) for f,p in data_pairs if any(t in f.lower() for t in tokens)][:6]
    partials=[f for f in main_files if any(t in f.lower() for t in tokens)]
    rel=lambda p: p.replace('Assets/Ashfall.Core/','').replace('src/','host:')
    b_=f"""# {base_name} — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`{auth_name}`]({os.path.relpath(auth_path,os.path.dirname(plan_path))}) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
"""
    for code,desc in auth_pkgs: b_+=f"| `{code}` | {desc[:110]} |\n"
    b_+=f"\n## 2. Source inventory ({len(files)} files, Core + host)\n\n| File | Lines |\n|---|---:|\n"
    for p in files[:40]: b_+=f"| `{rel(p)}` | {rd(p).count(chr(10))+1} |\n"
    if len(files)>40: b_+=f"\n… and {len(files)-40} more.\n"
    b_+=extra(pid)
    b_+="\n## 3. Data bindings\n\n"
    if cats:
        b_+="| Catalog | Records/shape |\n|---|---|\n"
        for f,p in cats: b_+=f"| `{f}` | {rec_count(p)} |\n"
    else: b_+="No catalog name-matched; verify paths/loaders before claiming a data dependency.\n"
    b_+=f"\n## 4. Host attachment\n\n- Candidate host partials: {', '.join('`'+x+'`' for x in partials[:5]) or 'none — new attachment or headless-only'}\n- Proposed method names: `Setup{region}{pid.split('-')[0]}Scaffold` / `Save{region}{pid.split('-')[0]}Scaffold` (Plan 1 Appendix AI: collision-free)\n- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.\n"
    b_+=f"\n## 5. Test scaffold (proposed, not created)\n\nProposed file: `Ashfall.Core.Tests/{region}/{pid.replace('-','')}ScaffoldTests.cs`\n\n```csharp\n// One fixture per package, plus one authority-conformance fixture.\npublic class {pid.replace('-','')}ScaffoldTests\n{{\n"
    for code,desc in pkgs: b_+=f"    [Fact] public void {re.sub(r'[^A-Za-z0-9]','',code)}_TODO() {{ /* {desc[:80]} */ }}\n"
    b_+=f"    [Fact] public void {pid.replace('-','')}_AuthorityConformance_TODO() {{ /* pattern parity with {auth_name} */ }}\n""}}\n```\n"
    b_+=f"\n## 6. Commands\n\n- `bash scripts/run_test.sh Ashfall.Core.Tests/{region}/`{' (create the region if absent)' if not os.path.isdir('Ashfall.Core.Tests/'+region) else ''}\n- Regenerate, do not edit (Plan 1 Appendices AJ/AM).\n- `python3 scripts/ci/generate-docs-index.py --check` if docs change.\n"
    b_+="\n## 7. Scaffolding rules\n\n1. No production file before a claim.\n2. The authority's patterns win; a deviation is recorded with a reason.\n3. Every fixture maps to a package; a package without a fixture is a finding.\n"
    open(os.path.join(os.path.dirname(plan_path),app),'w',encoding='utf-8').write(b_)
    made.append((app,len(b_)))
    lines=plan_txt.splitlines(keepends=True); out=[]; ins=False
    for ln in lines:
        if not ins and ln.startswith('**Non-goals:**'):
            out.append(f"**Implementation scaffold:** [`{app}`]({app}) — paired with `{auth_name}` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).\n\n")
            ins=True
        out.append(ln)
    if ins: open(plan_path,'w',encoding='utf-8').write(''.join(out))
    print(f"{base_name}: files={len(files)} cats={len(cats)} auth={auth_name} -> {len(b_)} chars, linked={ins}")
