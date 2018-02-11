CREATE TABLE [dbo].[OrderDetails]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Datetime] DATETIME NOT NULL, 
    [RestaurantId] INT NOT NULL
	CONSTRAINT [FK_OrderDetails_ToRestaurant] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants]([id]), 
    [Details] NVARCHAR(MAX) NOT NULL

)
