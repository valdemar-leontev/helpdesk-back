import os

def toggle_line(
    path: str,
    name: str,
    original: str,
    current: str,
    forward: bool = False
):
    if forward is False:
        original, current = current, original

    with open(f'{path}{name}', mode='r', encoding='utf-8') as source_file:
        lines = source_file.readlines()

        with open(f'{path}{name}_changed', mode='w', encoding='utf-8') as destination_file:

            for line in lines:
                if original in line:
                    destination_file.write(line.replace(original, current))
                else:
                    destination_file.write(line)

    os.remove(f'{path}{name}')
    os.rename(f'{path}{name}_changed', f'{path}{name}')