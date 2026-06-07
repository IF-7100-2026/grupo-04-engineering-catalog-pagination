-- schema.sql

CREATE EXTENSION IF NOT EXISTS "pg_trgm";

CREATE TABLE IF NOT EXISTS catalog_parts (
    part_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    manufacture_date date NOT NULL,
    registration_timestamp timestamp without time zone NOT NULL,
    weight_kg integer NOT NULL,
    size_meters numeric(10,2) NOT NULL,
    part_type character varying(50) NOT NULL,
    material character varying(50) NOT NULL,
    long_description text NOT NULL
);