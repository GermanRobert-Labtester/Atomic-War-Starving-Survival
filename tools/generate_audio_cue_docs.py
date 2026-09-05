#!/usr/bin/env python3
"""Generate authoritative markdown documentation for all audio cues from audio_cues.json."""

import json
from collections import Counter

def main():
    with open('Assets/StreamingAssets/Data/audio_cues.json') as f:
        data = json.load(f)

    cues = sorted(data['cues'], key=lambda c: (c['bus'], c['id']))

    md = [
        '# Authoritative Audio Cue Catalog',
        '',
        f'**Total Cues Authored:** {len(cues)}  ',
        '**Data Authority:** `Assets/StreamingAssets/Data/audio_cues.json` (`schema_version: 1`)  ',
        '**Runtime Accessor:** `AtomicWar.GodotApp.Audio.AudioCueCatalog`  ',
        '',
        '---',
        '',
        '## 1. Cue Manifest by Audio Bus',
        '',
        '| Cue ID | Bus | Primary Resource | Alt Samples | Vol (dB) | Cooldown (s) | Loop | Fallback Cue |',
        '|---|---|---|---|---|---|---|---|'
    ]

    for c in cues:
        alt_count = len(c.get('resource_paths', []))
        res = c.get('resource_path', '').replace('res://assets/audio/', '')
        fallback = f"`{c.get('fallback_cue_id')}`" if c.get('fallback_cue_id') else '—'
        loop = '✓' if c.get('loop') else '—'
        md.append(f"| `{c['id']}` | `{c['bus']}` | `{res}` | {alt_count} | {c['default_volume_db']} | {c['cooldown_seconds']} | {loop} | {fallback} |")

    md.append('')
    md.append('---')
    md.append('')
    md.append('## 2. Audio Bus Allocation Summary')
    md.append('')

    bus_counts = Counter(c['bus'] for c in cues)
    for b, cnt in sorted(bus_counts.items()):
        md.append(f'- **{b}:** {cnt} cues')

    with open('docs/audio/AUDIO_CUE_CATALOG.md', 'w') as f:
        f.write('\n'.join(md) + '\n')

    print(f'Generated docs/audio/AUDIO_CUE_CATALOG.md with {len(cues)} cues.')

if __name__ == '__main__':
    main()
