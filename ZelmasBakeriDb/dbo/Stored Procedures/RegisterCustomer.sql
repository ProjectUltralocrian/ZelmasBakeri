CREATE PROCEDURE [dbo].[RegisterCustomer]
	@Name nvarchar(100),
	@Email varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Customers (Name, Email) VALUES (@Name, @Email);
	SELECT SCOPE_IDENTITY() AS LastInsertedID;
END
