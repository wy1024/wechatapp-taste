CREATE TABLE [dbo].[Orders]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
	[Datetime] DATETIME NOT NULL, 
    [RestaurantId] INT NOT NULL,
    [RestaurantName] NVARCHAR(MAX),
	[Details] NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK_OrderDetails_ToRestaurant] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants]([Id]), 
	CONSTRAINT [FK_Orders_ToUser] FOREIGN KEY ([Userid]) REFERENCES [Users]([Id]),
)
