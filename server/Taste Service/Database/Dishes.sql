CREATE TABLE [dbo].[Dishes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [RestaurantId] INT NOT NULL, 
    [CuisineId] INT NOT NULL, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Flavors] NVARCHAR(MAX) NULL, 
    [Ingredients] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Dishes_ToRestaurants] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants]([id]),
    CONSTRAINT [FK_Dishes_ToCuisine] FOREIGN KEY ([CuisineId]) REFERENCES [Cuisine]([id]) 
)
