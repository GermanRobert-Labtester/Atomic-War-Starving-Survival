#!/usr/bin/env python3
"""
Batch-fix event leaks in HostSessions by adding UnsubscribeAll() methods.
Handles single-line and multi-line event subscriptions.
"""
import re
from pathlib import Path

HOST_DIR = Path("src/Host")

def process_hostsession(filepath: Path):
    content = filepath.read_text()
    original = content
    
    # Find all += subscriptions (single-line pattern)
    # Match: <indent><source>.On<Event> += <handler>;
    single_line_pattern = re.compile(
        r'(\s+)(\w+(?:\.\w+)*\.On\w+)\s*\+=\s*([^;]+);',
        re.MULTILINE
    )
    
    subscriptions = []
    for match in single_line_pattern.finditer(content):
        indent = match.group(1)
        event_name = match.group(2)
        handler = match.group(3).strip()
        subscriptions.append((indent, event_name, handler))
    
    if not subscriptions:
        return False, "no subscriptions"
    
    # Find the class body start
    class_match = re.search(r'(public\s+sealed\s+class\s+\w+HostSession\s*\{)', content)
    if not class_match:
        return False, "could not find class start"
    
    # Generate field declarations
    field_declarations = "\n"
    for _, event_name, handler in subscriptions:
        base_name = re.sub(r'^\w+\.On', '', event_name)
        base_name = re.sub(r'\.', '_', base_name)
        field_name = f"_on{base_name}"
        
        # Determine field type from handler signature
        if "(" not in handler or "=>" not in handler:
            field_type = "Action?"
        elif "EventArgs" in handler:
            field_type = "System.EventHandler?"
        elif re.search(r'\([^)]*,\s*string', handler):
            field_type = "Action<string>?"
        elif re.search(r'\([^)]*,\s*\w+', handler):
            field_type = "Action<object, object>?"
        else:
            field_type = "Action?"
        
        field_declarations += f"        private {field_type} {field_name};\n"
    
    # Replace += subscriptions with field assignments
    new_content = content
    for indent, event_name, handler in subscriptions:
        base_name = re.sub(r'^\w+\.On', '', event_name)
        base_name = re.sub(r'\.', '_', base_name)
        field_name = f"_on{base_name}"
        
        old_pattern = f'{event_name} += {handler};'
        new_pattern = f'{field_name} = {handler};\n{indent}{event_name} += {field_name};'
        new_content = new_content.replace(old_pattern, new_pattern)
    
    # Generate UnsubscribeAll method
    unsubscribe_method = "\n        public void UnsubscribeAll()\n        {\n"
    for _, event_name, _ in subscriptions:
        base_name = re.sub(r'^\w+\.On', '', event_name)
        base_name = re.sub(r'\.', '_', base_name)
        field_name = f"_on{base_name}"
        unsubscribe_method += f"            {event_name} -= {field_name};\n"
    unsubscribe_method += "        }\n"
    
    # Insert UnsubscribeAll before closing brace
    new_content = new_content.rstrip()
    if new_content.endswith("}"):
        new_content = new_content[:-1] + unsubscribe_method + "}\n"
    
    if new_content != original:
        filepath.write_text(new_content)
        return True, f"added {len(subscriptions)} unsubscribe handlers"
    
    return False, "no changes made"

def main():
    changed = 0
    errors = 0
    
    for f in sorted(HOST_DIR.glob("*HostSession.cs")):
        if f.name in ["HostCli.cs", "HostCli.SelfTests.cs", "HostCli.PanelTests.cs"]:
            continue
        try:
            ok, msg = process_hostsession(f)
            if ok:
                changed += 1
                print(f"[CHANGED] {f.name}: {msg}")
            else:
                print(f"[SKIP] {f.name}: {msg}")
        except Exception as e:
            errors += 1
            print(f"[ERROR] {f.name}: {e}")
    
    print(f"\nSummary: {changed} changed, {errors} errors")

if __name__ == "__main__":
    main()
