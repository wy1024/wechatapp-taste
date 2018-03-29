var express = require('express')
var app = express()
var mssql = require('mssql')
var connection = require("./mssql");
var bodyParser = require('body-parser')
var jsonParser = bodyParser.json()

app.get('/', function (req, res) {
  res.send('Hello World');
})

app.listen(process.env.PORT || 5050);
 
// restaurants
app.get('/api/restaurant/all', function (req, res) {
  var request = new mssql.Request();
  request.query('select * FROM dbo.restaurants;', function (err, result) {
      if (err) {
				res.send(err);
				return;
			}

      var data = result.recordset;
      res.send(data);      
  }); 
});

app.post('/api/restaurant/:restaurantId', function (req, res) {
	var restaurantId = req.params.restaurantId;
	if (!restaurantId) {
		res.send("Invalid restaurantId");
		return;
	}

  var request = new mssql.Request();
  request.query('select * FROM dbo.restaurants where Id = ' + restaurantId, function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

      var data = result.recordset;
      res.send(data);      
  }); 
});

// menu
app.post('/api/menu/:restaurantId', function (req, res) {
	var restaurantId = req.params.restaurantId;
	if (!restaurantId) {
		res.send("Invalid restaurantId");
		return;
	}

  var request = new mssql.Request();
  request.query('select * FROM dbo.Dishes where RestaurantId = ' + restaurantId, function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

			// need to group dishes into categories
			var record = result.recordset;
			var createdCategories = [];
			var map = {};
			for(i=0; i < record.length; i++) {
				var item = record[i];
				var c = item.Category;
				if (c in map) {
					var list = map[c];
					list.push(item);
				} else {
					var list = [];
					list.push(item);
					map[c] = list;
					createdCategories.push(c);
				}
			}

			var allFormattedResults = [];
			for(i=0; i<createdCategories.length; i++){
				var mapCategoryItem = map[createdCategories[i]];
				var formattedResult = {
					name: createdCategories[i],
					foods: mapCategoryItem
				}
				allFormattedResults.push(formattedResult);
			}
			var finalRes = {
				goods: allFormattedResults
			}
      res.send(finalRes);      
  }); 
});

//orders
app.post('/api/orders/:userId', function (req, res) {
	var userid = req.params.userId;
	if (!userid) {
		res.send("Invalid userid");
		return;
	}

  var request = new mssql.Request();
  request.query('SELECT Orders.DateTime, Orders.Details FROM Orders JOIN Users ON Users.Id = Orders.UserId WHERE Users.UserId = \'' + userid + '\'', function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

      var data = result.recordset;
      res.send(data);      
  }); 
});

app.post('/api/order/:orderId', function (req, res) {
	var orderId = req.params.orderId;
	if (!orderId) {
		res.send("Invalid orderId");
		return;
	}

  var request = new mssql.Request();
  request.query('SELECT Orders.DateTime, Orders.Details FROM Orders JOIN Users ON Users.Id = Orders.UserId WHERE Orders.Id = ' + orderId, function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

      var data = result.recordset;
      res.send(data);      
  }); 
});

app.post('/api/submit/order', jsonParser, function (req, res) {
	var restaurantid = req.body.restaurantId;
	var userid = req.body.userId;
	var restaurantname = req.body.restaurantName
	var details = req.body.details;
	// DishId : "", Quantity: "4"

	if (!restaurantid) {
		res.send("Invalid restaurantid");
		return;
	} else if (!userid) {
		res.send("Invalid userid");
		return;
	} else if (!restaurantname) {
		res.send("invalid restaurantname");
		return;
	} else if (!details) {
		res.send("invalid details")
		return;
	}

	var detailsString = JSON.stringify(details);
	var d = new Date();
	var dateString = ("00" + (d.getMonth() + 1)).slice(-2) + "/" + 
	("00" + d.getDate()).slice(-2) + "/" + 
	d.getFullYear() + " " + 
	("00" + d.getHours()).slice(-2) + ":" + 
	("00" + d.getMinutes()).slice(-2) + ":" + 
	("00" + d.getSeconds()).slice(-2) + ":000";

	var insertStatement = `INSERT INTO dbo.Orders VALUES (${userid}, '${dateString}', ${restaurantid}, '${restaurantname}', '{details:${detailsString}}')`;
	console.log(insertStatement);
  var request = new mssql.Request();
  request.query(insertStatement, function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

      res.send("success");      
  }); 
});

// preference
app.post('/api/preference/:userId', function (req, res) {
	var userId = req.params.userId;
	if (!userId) {
		res.send("Invalid userId");
		return;
	}

	var request = new mssql.Request();
  request.query('select * FROM dbo.preference where UserId = ' + userId, function (err, result) {
			if (err) {
				res.send(err);
				return;
			}

			var data = result.recordset[0];
			
			var cuisines = data.Cuisine.split(',').map((word) => {
				return "'" + word + "'";
			}).join(',');
			var dishes = data.Dishes.split(',').map((word) => {
				return "'" + word + "'";
			}).join(',');

			var request2 = new mssql.Request();
			var query2 = 'select * FROM dbo.cuisine where Id in (' + cuisines + ')';

			request2.query(query2, function (err2, result2) {
				if (err2) {
					res.send(err2);
					return;
				}
				var cuisineDetails = [];
				var data2 = result2.recordset;
				for(i=0;i<data2.length;i++){
					cuisineDetails.push(data2[i]);
				}

				var request3 = new mssql.Request();
				var query3 = 'select * FROM dbo.dishes where Name in (' + dishes + ')';
				request3.query(query3, function (err3, result3) {
					if (err3) {
						res.send(err3);
						return;
					}
					var dishesDetails = [];
					var data3 = result3.recordset;
					for(i=0;i<data3.length;i++){
						dishesDetails.push(data3[i]);
					}

					
					var preference = {
						userId: userId,
						ingredients: data.Ingredients,
						cuisine: cuisineDetails,
						dishes: dishesDetails,
						flavors: data.Flavors
					}
					
					res.send(preference);
				});

			});   
	}); 
});

app.get('/api/testtt', function (req, res) {
	var http = require("https");

	var options = {
	  "method": "GET",
	  "hostname": "parseapi.back4app.com",
	  "port": null,
	  "path": "/classes/ReferSalesList",
	  "headers": {
	    "x-parse-application-id": "pemuRp7OIvuUvCw2zuUjAfKD8ITOD1gccjDFspu6",
	    "x-parse-rest-api-key": "qjPTK8ZN9ENIQaMiqdQPhDyunZNiKwpHF1AY3LWH",
	    "cache-control": "no-cache",
	    "postman-token": "b2cb915d-d678-9320-50ef-b7a5d1d53fd8"
	  }
	};

	var request = http.request(options, function (result) {
	  var chunks = [];

	  result.on("data", function (chunk) {
	    chunks.push(chunk);
	  });

	  result.on("end", function () {
	    var body = Buffer.concat(chunks);
	    res.send(body.toString());
	  });
	});

	request.end();
})


// wx login stuff
// 微信登录
app.post('/user/getOpenId/:code', function (req, res) {
	// 取到传入的code参数
	var code = req.params.code;
	//code: 081nSMli15zTzz0TT6mi1rPAli1nSMlY
	// 设定小程序appid appsecret
	var APPID = 'wx9114b997bd86f8ed';
	var SECRET = 'd27551c7803cf16015e536b192d5d03b';
	// 拼接请求地址
	var url = 'https://api.weixin.qq.com/sns/jscode2session?appid='+APPID+'&secret='+SECRET+'&js_code='+code+'&grant_type=authorization_code';
	// 向微信请求openid等信息
	var request = require('request');
	request(url, function (error, response, body) {
			// {"session_key":"ZPp6HkQCI6ZCfkTIyseIPQ==","expires_in":7200,"openid":"ox-0I0VkzCYSFllCNgVWQ4PffMM0"}
			res.send(body);
	})
});