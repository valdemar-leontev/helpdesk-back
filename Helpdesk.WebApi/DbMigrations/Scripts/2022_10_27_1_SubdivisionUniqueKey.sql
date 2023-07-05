alter table dictionaries.subdivision
    add constraint ix_subdivision_code_description unique (code, description);