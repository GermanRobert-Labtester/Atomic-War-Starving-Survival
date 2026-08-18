import os
import json

desktop_dir = '/home/robertsrff/Desktop'
os.makedirs(desktop_dir, exist_ok=True)

log_path_full = '/home/robertsrff/.gemini/antigravity-cli/brain/9a5142a0-8540-44d0-afa2-ccb128982e7c/.system_generated/logs/transcript_full.jsonl'
log_path_compact = '/home/robertsrff/.gemini/antigravity-cli/brain/9a5142a0-8540-44d0-afa2-ccb128982e7c/.system_generated/logs/transcript.jsonl'

out_md = os.path.join(desktop_dir, 'ASHFALL_SESSION_TRANSCRIPT.md')
out_jsonl = os.path.join(desktop_dir, 'ASHFALL_SESSION_TRANSCRIPT_FULL.jsonl')

source_file = log_path_full if os.path.exists(log_path_full) else log_path_compact

# 1. Copy raw JSONL
with open(source_file, 'r', encoding='utf-8') as f_in, open(out_jsonl, 'w', encoding='utf-8') as f_out:
    f_out.write(f_in.read())

# 2. Parse and format Markdown for other AIs
lines = []
with open(source_file, 'r', encoding='utf-8') as f:
    for line in f:
        line_s = line.strip()
        if line_s:
            try:
                lines.append(json.loads(line_s))
            except Exception:
                pass

with open(out_md, 'w', encoding='utf-8') as md:
    md.write('# ASHFALL: Complete Session Conversation Transcript for AI Review\n\n')
    md.write('- **Conversation ID**: `9a5142a0-8540-44d0-afa2-ccb128982e7c`\n')
    md.write(f'- **Total Trajectory Steps**: {len(lines)}\n')
    md.write('- **Game Project**: ASHFALL (2D Atomic-War Survival)\n')
    md.write('- **Export Date**: 2026-08-18\n\n')
    md.write('---\n\n')

    for step in lines:
        step_idx = step.get('step_index', '?')
        source = step.get('source', 'UNKNOWN')
        step_type = step.get('type', 'UNKNOWN')
        content = step.get('content', '')
        tool_calls = step.get('tool_calls', [])

        md.write(f'## [Step {step_idx}] Source: `{source}` | Type: `{step_type}`\n\n')

        if content:
            md.write(f'{content.strip()}\n\n')

        if tool_calls:
            md.write(f'### Tool Calls ({len(tool_calls)}):\n\n')
            for tc in tool_calls:
                name = tc.get('name', 'tool')
                args = tc.get('arguments', {})
                md.write(f'**Tool**: `{name}`\n\n')
                md.write('```json\n')
                md.write(json.dumps(args, indent=2))
                md.write('\n```\n\n')

        md.write('---\n\n')

print(f'Export completed successfully:')
print(f'-> Markdown Review File: {out_md} ({os.path.getsize(out_md)} bytes)')
print(f'-> Full Raw JSONL File: {out_jsonl} ({os.path.getsize(out_jsonl)} bytes)')
