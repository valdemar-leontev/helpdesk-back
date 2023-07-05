alter table business.requirement_template
    alter column description type varchar(256),
    alter column name type varchar(256);

alter table business.variant
    alter column description type varchar(256);

alter table business.question
    alter column description type varchar(256);



alter table dictionaries.position
    alter column code type varchar(32),
    alter column description type varchar(256);

alter table dictionaries.question_type
    alter column code type varchar(32),
    alter column description type varchar(256);

alter table dictionaries.requirement_category
    alter column description type varchar(256);

alter table dictionaries.requirement_category_type
    alter column description type varchar(256);

alter table dictionaries.requirement_state
    alter column code type varchar(32),
    alter column description type varchar(256);

alter table dictionaries.role
    alter column code type varchar(32),
    alter column description type varchar(256);

alter table dictionaries.subdivision
    alter column code type varchar(32),
    alter column description type varchar(256);