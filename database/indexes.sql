-- indexes.sql

CREATE INDEX idx_manufacture_date ON catalog_parts(manufacture_date);
CREATE INDEX idx_material ON catalog_parts(material);
CREATE INDEX idx_part_type ON catalog_parts(part_type);
CREATE INDEX idx_weight_kg ON catalog_parts(weight_kg);
CREATE INDEX idx_long_description_trgm ON catalog_parts USING gin (long_description gin_trgm_ops);
CREATE INDEX idx_registration_timestamp ON catalog_parts(registration_timestamp);
CREATE INDEX idx_size_meters ON catalog_parts(size_meters);