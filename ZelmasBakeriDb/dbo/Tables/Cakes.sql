CREATE TABLE [dbo].[Cakes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] nvarchar(100) NOT NULL,
	[Description] nvarchar(200) NOT NULL,
	[Price] int NOT NULL,
	[Image] nvarchar(100)
)