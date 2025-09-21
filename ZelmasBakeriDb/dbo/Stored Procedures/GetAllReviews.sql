CREATE PROCEDURE [dbo].[GetAllReviews]
AS
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Reviews] ORDER BY [CreatedAt] DESC;
RETURN 0
