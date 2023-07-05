alter table business.requirement
    drop column if exists outgoing_number;
        
alter table business.requirement
    add column outgoing_number integer;

with t as (
    select r.id,
           r.requirement_category_id,
           r.creation_date,
           row_number() over (partition by requirement_category_id  order by creation_date) as num
    from business.requirement r
)
update business.requirement set outgoing_number = t.num
    from t
where t.id = business.requirement.id;

alter table business.requirement
    alter column outgoing_number set not null;