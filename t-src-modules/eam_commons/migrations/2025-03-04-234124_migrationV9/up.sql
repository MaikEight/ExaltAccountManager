ALTER TABLE EamAccount ADD COLUMN orderId INTEGER;
-- Set the default value of the new column to the value of the id column
UPDATE EamAccount SET orderId = id;