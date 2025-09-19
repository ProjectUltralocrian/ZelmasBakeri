CREATE TABLE [dbo].[OrderLines]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[OrderId] INT NOT NULL,
	[CakeId] INT NOT NULL,
	[Quantity] INT NOT NULL, 
    CONSTRAINT [FK_OrderLines_Order] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]),
    CONSTRAINT [FK_OrderLines_Cake] FOREIGN KEY ([CakeId]) REFERENCES [Cakes]([Id])
)
