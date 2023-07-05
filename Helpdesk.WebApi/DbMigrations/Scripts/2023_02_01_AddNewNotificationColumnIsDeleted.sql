alter table business.notification
    add column is_deleted boolean not null default false
    