-- Step 1: Declare variables for room seeding
DECLARE @HotelId UNIQUEIDENTIFIER;
DECLARE @RoomNumber NVARCHAR(50);
DECLARE @RoomType NVARCHAR(50);
DECLARE @Status NVARCHAR(50) = 'Available';
DECLARE @Price FLOAT;
DECLARE @Description NVARCHAR(255);
DECLARE @MaxChildren INT;
DECLARE @MaxAdults INT;
DECLARE @CreatedAt DATETIME = '2020-01-01'; 
DECLARE @Count INT;

-- Step 2: Cursor to iterate through hotels
DECLARE hotel_cursor CURSOR FOR
SELECT Id FROM Hotels;

OPEN hotel_cursor;
FETCH NEXT FROM hotel_cursor INTO @HotelId;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Count = ABS(CHECKSUM(NEWID())) % 4 + 3;

    WHILE @Count > 0
    BEGIN
        SET @RoomNumber = 'Room ' + CAST(ABS(CHECKSUM(NEWID())) % 1000 AS NVARCHAR(10));
        SET @RoomType = CASE ABS(CHECKSUM(NEWID())) % 3
                        WHEN 0 THEN 'SingleRoom'
                        WHEN 1 THEN 'DoubleRoom'
                        ELSE 'TripleRoom'
                        END;
        SET @Price = CAST(ABS(CHECKSUM(NEWID())) % 200 + 50 AS FLOAT);
        SET @Description = 'A ' + @RoomType + ' with a comfortable stay in the hotel.';
        SET @MaxChildren = ABS(CHECKSUM(NEWID())) % 3;
        SET @MaxAdults = ABS(CHECKSUM(NEWID())) % 4 + 1;

        INSERT INTO Rooms (Id, HotelId, RoomNumber, RoomType, Status, Price, Description, MaxChildren, MaxAdults, CreatedAt)
        VALUES 
            (NEWID(), @HotelId, @RoomNumber, @RoomType, @Status, @Price, @Description, @MaxChildren, @MaxAdults, @CreatedAt);

        SET @Count = @Count - 1;
    END

    FETCH NEXT FROM hotel_cursor INTO @HotelId;
END

CLOSE hotel_cursor;
DEALLOCATE hotel_cursor;
