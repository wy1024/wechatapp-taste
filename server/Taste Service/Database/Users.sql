﻿CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(MAX) NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL
)
