CREATE TABLE [dbo].[Dishes]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [RestaurantId] INT NOT NULL, 
    [CuisineId] INT NOT NULL, 
    [Name] NVARCHAR(MAX) NOT NULL, 
	[Description] NVARCHAR(MAX) NULL,
    [Flavors] NVARCHAR(MAX) NULL, 
    [Ingredients] NVARCHAR(MAX) NULL, 
    [Category] NVARCHAR(MAX) NULL, 
    [Price] FLOAT NOT NULL, 
    [Image] VARBINARY(MAX) NULL, 
    CONSTRAINT [FK_Dishes_ToRestaurants] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants]([id]),
    CONSTRAINT [FK_Dishes_ToCuisine] FOREIGN KEY ([CuisineId]) REFERENCES [Cuisine]([id]) 
)
