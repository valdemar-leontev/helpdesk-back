alter table dictionaries.position
    drop column if exists code;

alter table dictionaries.question_type
    drop column if exists code;

alter table dictionaries.requirement_category
    drop column if exists code;

alter table dictionaries.requirement_category_type
    drop column if exists code;

alter table dictionaries.requirement_state
    drop column if exists code;

alter table dictionaries.subdivision
    drop column if exists code;