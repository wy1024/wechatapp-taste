CREATE TABLE [dbo].[Restaurants]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Location] NVARCHAR(MAX) NULL, 
    [Phone] NVARCHAR(20) NULL
)
