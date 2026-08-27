import re

def process_file():
    input_path = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/ai-art/SEAART_QUEUE.md'
    output_path = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/ai-art/WEB_NANO_BANANA_PRO.md'

    with open(input_path, 'r', encoding='utf-8') as f:
        content = f.read()

    items = content.split('## ')
    out_blocks = []

    for item in items[1:]:
        if '**Model:** Nano Banana 2' in item:
            lines = item.strip().split('\n')
            title_line = lines[0]

            new_lines = [f"## {title_line}"]

            for line in lines[1:]:
                if line.startswith('**Platform:**'):
                    new_lines.append('**Platform:** Web  ')
                elif line.startswith('**Model:**'):
                    new_lines.append('**Model:** Nano Banana Pro  ')
                elif line.startswith('**Reference:**') or line.startswith('**Reason for model choice:**'):
                    continue # Skip these
                elif line.startswith('> '):
                    # Rewrite the prompt text
                    text = line[2:].strip()
                    # Remove "Match the referenced... exactly."
                    text = re.sub(r'Match the referenced.*?exactly\.\s*', '', text)
                    # Remove "Change only the stated fill level; preserve container geometry, cap and label block."
                    text = re.sub(r'Change only.*?block\.\s*', '', text)
                    # Remove "Apply only the specified break; preserve every undamaged part."
                    text = re.sub(r'Apply only.*?part\.\s*', '', text)
                    text = re.sub(r'Add only.*?materials\.\s*', '', text)

                    new_lines.append(f"> {text}")
                else:
                    new_lines.append(line)

            # Clean up multiple empty lines
            cleaned_lines = []
            for line in new_lines:
                if line.strip() == '' and (not cleaned_lines or cleaned_lines[-1].strip() == ''):
                    continue
                cleaned_lines.append(line)

            out_blocks.append('\n'.join(cleaned_lines))

    with open(output_path, 'a', encoding='utf-8') as f:
        f.write('\n\n' + '\n\n'.join(out_blocks) + '\n')

if __name__ == '__main__':
    process_file()
