var express = require('express')
var app = express()
var mssql = require('mssql')
var connection = require("./mssql");

app.get('/', function (req, res) {
  res.send('Hello World');
})

app.listen(process.env.PORT || 5050);
 
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