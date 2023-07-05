alter table business.requirement_comment
    drop column notification_id

----

alter table business.requirement_comment
    drop constraint fk_requirement_comment_requirement_requirement_id;

alter table business.requirement_comment
    add constraint fk_requirement_comment_requirement_requirement_id
        foreign key (requirement_id) references business.requirement
            on delete cascade;

----

alter table business.requirement_link_notification
    drop constraint fk_requirement_link_notification_notification_notification_id;

alter table business.requirement_link_notification
    add constraint fk_requirement_link_notification_notification_notification_id
        foreign key (notification_id) references business.notification
            on delete cascade;