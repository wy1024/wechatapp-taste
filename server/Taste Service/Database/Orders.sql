CREATE TABLE [dbo].[Orders]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [OrderDetailsId] INT NOT NULL
	CONSTRAINT [FK_Orders_ToUser] FOREIGN KEY ([Userid]) REFERENCES [Users]([id]),
	CONSTRAINT [FK_Orders_ToOrderDetails] FOREIGN KEY ([OrderDetailsId]) REFERENCES [OrderDetails]([id])
)
