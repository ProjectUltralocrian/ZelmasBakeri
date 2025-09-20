CREATE PROCEDURE [dbo].[RegisterOrder]
	@CustomerId int,
	@Date datetime2(7),
	@Comments nvarchar(1000)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Orders (CustomerId, Date, Comments) VALUES (@CustomerId, @Date, @Comments);
	SELECT SCOPE_IDENTITY() AS LastInsertedID;
END
