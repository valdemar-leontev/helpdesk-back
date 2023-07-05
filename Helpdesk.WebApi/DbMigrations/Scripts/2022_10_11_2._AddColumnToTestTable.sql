alter table tests.test 
    add column testField integer;

insert into tests.test (id, name, testfield)
values (1, 'Name 1', 1),
       (2, 'Name 2', 2);

alter table tests.test
    alter column testField set not null;

alter table tests.test
    add constraint FK_tests_test_testField_admin_user_id
    foreign key (testField) references admin.user (id);