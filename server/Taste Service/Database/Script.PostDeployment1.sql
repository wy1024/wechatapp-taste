/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


  INSERT INTO dbo.Restaurants VALUES ('HS4VTV8MXDMDM', 'Coffee shop', 'Seattle', '1234567', 'Jason', NULL);
  INSERT INTO dbo.Restaurants VALUES ('some other id pepper', 'Hot pepper', 'SF', '234567', 'Tommy', NULL);

  INSERT INTO dbo.Cuisine VALUES(N'川菜', N'辣');
  INSERT INTO dbo.Cuisine VALUES(N'甜品', N'甜');

  INSERT INTO dbo.Dishes VALUES (1, 2, 'Latte', 'A good cup of latte', 'Refreshing', 'Latte beans', 'Coffee', 4.2, NULL);
  INSERT INTO dbo.Dishes VALUES (1, 2, 'Mocha', 'Elegant mocha', 'Refreshing', 'Mocha beans', 'Coffee', 3.9, NULL);
  INSERT INTO dbo.Dishes VALUES (2, 1, N'宫保鸡丁', N'好吃宫保鸡丁！', N'香辣,脆口', N'鸡肉,花生,土豆', N'炒菜', 10.5, NULL);
  INSERT INTO dbo.Dishes VALUES (2, 1, N'鱼香肉丝', '', N'香辣', N'猪肉,胡萝卜,酱油', N'炒菜', 11.5, NULL);
  INSERT INTO dbo.Dishes VALUES (2, 2, N'甜豆花', N'豆花是甜的', N'甜,柔软', N'豆腐,豆花,糖,红豆, Latte beans', N'甜品', 5, NULL);
  INSERT INTO dbo.Dishes VALUES (2, 2, N'抹茶冰激凌', N'清凉夏日', N'冰凉,甜腻, Refreshing', N'抹茶,冰激凌', N'甜品', 4.8, NULL);

  INSERT INTO dbo.Users VALUES ('jasonwang', 'Jason Wang');
  INSERT INTO dbo.Users VALUES ('hongyao', 'Hongyao Zhang');

  INSERT INTO dbo.Orders VALUES (1, '2007-05-08 12:35:29.123', 1, 'Coffee shop','{Details:[{DishId: "2", Quantity: "5"}, {DishId: "1", Quantity: "1"}]}');
  INSERT INTO dbo.Orders VALUES (1, '2007-05-08 12:35:29.123', 1, 'Coffee shop','{Details:[{DishId: "2", Quantity: "5"}, {DishId: "1", Quantity: "1"}]}');
  INSERT INTO dbo.Orders VALUES (2, '2007-05-08 12:35:29.123', 1, 'Coffee shop','{Details:[{DishId: "1", Quantity: "1"}, {DishId: "1", Quantity: "1"}]}');
