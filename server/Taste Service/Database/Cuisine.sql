﻿CREATE TABLE [dbo].[Cuisine]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Taste] NVARCHAR(MAX) NULL
)
