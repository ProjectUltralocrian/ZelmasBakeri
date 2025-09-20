CREATE PROCEDURE [dbo].[GetAllOrderDetails]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        o.Id,
        o.CustomerId,
        c.Name AS CustomerName,
        c.Email AS CustomerEmail,
        STRING_AGG(ol.CakeId, ',') AS CakeIdsString,
        o.Date,
        o.Comments
    FROM Orders o
    INNER JOIN Orderlines ol ON o.Id = ol.OrderId
    INNER JOIN Customers c ON c.Id = o.CustomerId
    GROUP BY o.Id, o.CustomerId, o.Date, o.Comments, c.Name, c.Email
    ORDER BY o.Date;
END