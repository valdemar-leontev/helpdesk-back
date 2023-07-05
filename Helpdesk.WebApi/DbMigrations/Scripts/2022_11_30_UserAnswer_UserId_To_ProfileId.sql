alter table business.user_answer
    add if not exists profile_id int null;

update business.user_answer r
    set profile_id = p.id
from business.profile p
where p.user_id = r.user_id;

alter table business.user_answer
    alter column profile_id set not null;

alter table business.user_answer
    add constraint fk_user_answer_profile_profile_id foreign key (profile_id)
    references business.profile (id) on delete cascade;

create index ix_user_answer_profile_id
    on business.user_answer (profile_id);

drop index if exists business.ix_user_answer_user_id;

alter table business.user_answer
    drop constraint if exists fk_user_answer_user_user_id;

alter table business.user_answer
    drop column if exists user_id;