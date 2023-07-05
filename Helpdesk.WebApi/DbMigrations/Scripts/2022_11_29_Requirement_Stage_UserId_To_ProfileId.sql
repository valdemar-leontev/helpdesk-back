alter table business.requirement_stage
    add if not exists profile_id int;

update business.requirement_stage r
    set profile_id = p.id
from business.profile p
where p.user_id = r.user_id;

alter table business.requirement_stage
    alter column profile_id set not null;

alter table business.requirement_stage
    add constraint fk_requirement_stage_profile_profile_id foreign key (profile_id)
    references business.profile (id) on delete cascade;

create index ix_requirement_stage_profile_id
    on business.requirement_stage (profile_id);

drop index if exists business.ix_requirement_stage_user_id;

alter table business.requirement_stage
    drop constraint if exists fk_requirement_stage_user_user_id;

alter table business.requirement_stage
    drop column if exists user_id;