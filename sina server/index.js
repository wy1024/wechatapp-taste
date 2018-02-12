var express = require('express')
var app = express()
var mssql = require('mssql')
var connection = require("./mssql");

app.get('/', function (req, res) {
  res.send('Hello World');
})

app.listen(process.env.PORT || 5050);
 
app.get('/api/restaurants', function (req, res) {
  var request = new mssql.Request();
    request.query('select * FROM dbo.restaurants;', function (err, result) {
        if (err) 
            res.send(err);

        var data = {};
        data["user"] = result.recordset;
        res.send(data);      
    }); 
})

