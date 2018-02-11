CREATE TABLE [dbo].[Restaurants]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Location] NVARCHAR(MAX) NULL, 
    [Phone] NVARCHAR(20) NULL, 
    [Owner] NVARCHAR(40) NULL, 
    [Image] VARBINARY(MAX) NOT NULL
)
