CREATE TRIGGER SetDateUpdated
    ON Products
    AFTER UPDATE
              AS
BEGIN
UPDATE Products
SET DateUpdated = GETUTCDATE()
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.Id
END