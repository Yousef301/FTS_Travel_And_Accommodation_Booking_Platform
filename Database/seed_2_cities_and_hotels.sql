-- Step 1: Insert Cities
INSERT INTO Cities (Id, Name, Country, CreatedAt, ModifiedAt)
VALUES 
    (NEWID(), 'New York', 'USA', '2020-01-01', NULL),
    (NEWID(), 'London', 'UK', '2020-01-01', NULL),
    (NEWID(), 'Tokyo', 'Japan', '2020-01-01', NULL),
    (NEWID(), 'Paris', 'France', '2020-01-01', NULL),
    (NEWID(), 'Berlin', 'Germany', '2020-01-01', NULL),
    (NEWID(), 'Sydney', 'Australia', '2020-01-01', NULL),
    (NEWID(), 'Toronto', 'Canada', '2020-01-01', NULL),
    (NEWID(), 'Rome', 'Italy', '2020-01-01', NULL),
    (NEWID(), 'Dubai', 'UAE', '2020-01-01', NULL),
    (NEWID(), 'Hong Kong', 'China', '2020-01-01', NULL),
    (NEWID(), 'S�o Paulo', 'Brazil', '2020-01-01', NULL),
    (NEWID(), 'Moscow', 'Russia', '2020-01-01', NULL),
    (NEWID(), 'Istanbul', 'Turkey', '2020-01-01', NULL),
    (NEWID(), 'Buenos Aires', 'Argentina', '2020-01-01', NULL),
    (NEWID(), 'Cairo', 'Egypt', '2020-01-01', NULL);

-- Step 2: Declare variables for hotel seeding
DECLARE @CityId UNIQUEIDENTIFIER;
DECLARE @CityName NVARCHAR(100);
DECLARE @HotelName NVARCHAR(100);
DECLARE @Owner NVARCHAR(100);
DECLARE @Address NVARCHAR(255);
DECLARE @Description NVARCHAR(255);
DECLARE @PhoneNumber NVARCHAR(20);
DECLARE @Email NVARCHAR(100);
DECLARE @Count INT;
DECLARE @CreatedAt DATETIME = '2020-01-01';

-- Step 3: Insert Hotels
DECLARE city_cursor CURSOR FOR
SELECT Id, Name FROM Cities;

OPEN city_cursor;
FETCH NEXT FROM city_cursor INTO @CityId, @CityName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Count = ABS(CHECKSUM(NEWID())) % 4 + 1;

    WHILE @Count > 0
    BEGIN
        SET @HotelName = @CityName + ' Hotel ' + CAST(@Count AS NVARCHAR(10));
        SET @Owner = @HotelName + ' Owner';
        SET @Address = CAST(ABS(CHECKSUM(NEWID())) % 1000 AS NVARCHAR(10)) + ' Random St, Unit ' + CAST(ABS(CHECKSUM(NEWID())) % 100 AS NVARCHAR(10)) + ', ' + @CityName;
        SET @Description = 'A great hotel in ' + @CityName + ' offering exceptional service.';
        SET @PhoneNumber = '555-' + RIGHT('0000' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS NVARCHAR(4)), 4);
        SET @Email = LOWER(REPLACE(@HotelName, ' ', '_')) + '@example.com';

        INSERT INTO Hotels (Id, CityId, Name, Owner, Address, Description, PhoneNumber, Email, Rating, CreatedAt)
        VALUES 
            (NEWID(), @CityId, @HotelName, @Owner, @Address, @Description, @PhoneNumber, @Email, 0, @CreatedAt);

        SET @Count = @Count - 1;
    END

    FETCH NEXT FROM city_cursor INTO @CityId, @CityName;
END

CLOSE city_cursor;
DEALLOCATE city_cursor;
