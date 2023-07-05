select * from (select pr.id profileId,
                      po.id positionId,
                      po.code positionCode,
                      po.description positionDescription,
                      row_number() over ( partition by po.code, po.description
                          order by po.id ) as row_num

               from business.profile pr
                        join dictionaries.position po on pr.position_id = po.id
              ) a
where row_num > 0;

-- miracle 
do $$
    
declare
    _profileId integer;
    _positionId integer;
    _positionCode varchar;
    _positionDescription varchar;
    profile_position_duplicates  cursor for

        select * from (select pr.id profileId,
                              po.id positionId,
                              po.code positionCode,
                              po.description positionDescription,
                              row_number() over (partition by po.code, po.description
                                  order by po.id) as row_num

                       from business.profile pr
                                join dictionaries.position po on pr.position_id = po.id
                      ) a
        where row_num = 1;

begin
    open profile_position_duplicates;
    loop
        fetch profile_position_duplicates
            into _profileId, _positionId, _positionCode, _positionDescription;

        if not found then exit;
        end if ;

        -- raise notice 'Value: % % % ', _positionId, _positionCode, _positionDescription;

        update business.profile set position_id = _positionId
        where id in (
            select profileId from (select pr.id profileId,
                                          po.code positionCode,
                                          po.description positionDescription,
                                          row_number() over ( partition by po.code, po.description
                                              order by po.id ) as row_num

                                   from business.profile pr
                                            join dictionaries.position po on pr.position_id = po.id
                                  ) a
            where row_num > 1 and a.positionCode = _positionCode and a.positionDescription = _positionDescription
        );

    end loop;

end

$$ language plpgsql;

-- It will be deleted duplicates
delete from
    dictionaries.position a
    using dictionaries.position b
where
        a.id > b.id
  and a.code = b.code
  and a.description = b.description;


-- It will be created a unique constraints
alter table dictionaries.position 
    add constraint ix_position_code_description unique (code, description);
