﻿--  1. Creating business.subdivision_link_subdivision 

drop table if exists business.subdivision_link_subdivision;

create table if not exists business.subdivision_link_subdivision
(
    id integer generated by default as identity constraint pk_subdivision_link_subdivision primary key,
    subdivision_id integer not null constraint fk_subdivision_link_subdivision_subdivision_subdivision_id references dictionaries.subdivision on delete cascade,
    subdivision_parent_id integer constraint fk_subdivision_link_subdivision_subdivision_subdivision_parent references dictionaries.subdivision
);

alter table business.subdivision_link_subdivision
    owner to postgres;

create unique index if not exists ix_subdivision_link_subdivision_subdivision_id
    on business.subdivision_link_subdivision (subdivision_id);

create index if not exists ix_subdivision_link_subdivision_subdivision_parent_id
    on business.subdivision_link_subdivision (subdivision_parent_id);

-- 2. Creating business.profile_link_subdivision 

drop table if exists business.profile_link_subdivision;

create table if not exists business.profile_link_subdivision
(
    id integer generated by default as identity constraint pk_profile_link_subdivision primary key,
    profile_id integer not null constraint fk_profile_link_subdivision_profile_profile_id references business.profile on delete cascade,
    subdivision_id integer not null constraint fk_profile_link_subdivision_subdivision_subdivision_id references dictionaries.subdivision on delete cascade,
    is_head boolean not null
);

alter table business.profile_link_subdivision
    owner to postgres;

create unique index if not exists ix_profile_link_subdivision_profile_id
    on business.profile_link_subdivision (profile_id);

create unique index if not exists ix_profile_link_subdivision_profile_id_subdivision_id
    on business.profile_link_subdivision (profile_id, subdivision_id);

create index if not exists ix_profile_link_subdivision_subdivision_id
    on business.profile_link_subdivision (subdivision_id);

-- 3. Deleting subdivisionId and supervisorId from business.profile

alter table business.profile 
    drop column if exists subdivision_id,
    drop column if exists supervisor_id;