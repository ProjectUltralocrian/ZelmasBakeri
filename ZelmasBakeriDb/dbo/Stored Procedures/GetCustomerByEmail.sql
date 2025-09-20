CREATE PROCEDURE [dbo].[GetCustomerByEmail]
	@Email nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
	*
	FROM Customers
	WHERE Email = @Email;
END	
