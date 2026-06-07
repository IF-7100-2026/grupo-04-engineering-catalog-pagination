-- seed_data_generator.sql

INSERT INTO catalog_parts (
    part_id, 
    manufacture_date, 
    registration_timestamp, 
    weight_kg, 
    size_meters, 
    part_type, 
    material, 
    long_description
)
SELECT
    gen_random_uuid(),
    CURRENT_DATE - (random() * 3650)::integer,
    CURRENT_TIMESTAMP - (random() * interval '10 years'),
    (random() * 500)::integer + 1,
    (random() * 10)::numeric(10,2) + 0.1,
    'Type ' || (random() * 10)::integer,
    'Material ' || (random() * 10)::integer,
    'Auto-generated long description for testing GIN index operations on record sequence ' || gs
FROM generate_series(1, 1000000) AS gs;