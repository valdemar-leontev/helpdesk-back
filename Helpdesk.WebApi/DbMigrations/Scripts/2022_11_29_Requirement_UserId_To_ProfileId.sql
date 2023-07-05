alter table business.requirement
    add if not exists profile_id int null;

update business.requirement r
    set profile_id = p.id
from business.profile p
where p.user_id = r.user_id;

alter table business.requirement
    alter column profile_id set not null;

alter table business.requirement
    add constraint fk_requirement_profile_profile_id foreign key (profile_id)
    references business.profile (id) on delete cascade;

create index ix_requirement_profile_id
    on business.requirement (profile_id);

drop index if exists business.ix_requirement_user_id;

alter table business.requirement
    drop constraint if exists fk_requirement_user_user_id;

alter table business.requirement
    drop column if exists user_id;