update business.requirement
set outgoing_number = my_query.outgoing_number_current from (
    select
      row_number() over (order by creation_date) as outgoing_number_current,
      id,
      outgoing_number as outgoing_number_original,
      creation_date
    from business.requirement
    order by creation_date
) as my_query
where business.requirement.id = my_query.id