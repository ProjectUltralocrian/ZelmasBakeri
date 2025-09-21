CREATE PROCEDURE [dbo].[RegisterReview]
	@Name nvarchar(100),
	@Message nvarchar(1000),
	@CreatedAt DateTime2(7)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Reviews (Name, Message, CreatedAt) VALUES (@Name, @Message, @CreatedAt);
	SELECT SCOPE_IDENTITY() AS LastInsertedID;
END