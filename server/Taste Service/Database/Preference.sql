CREATE TABLE [dbo].[Preference]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [Ingredients] NVARCHAR(MAX) NULL, 
    [Cuisine] NVARCHAR(MAX) NULL, 
    [Dishes] NVARCHAR(MAX) NULL, 
    [Flavors] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Preference_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)
