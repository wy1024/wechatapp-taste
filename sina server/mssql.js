var mssql = require('mssql')

const config = {
    user: 'taste',
    password: 'Wechat2018',
    server: 'taste.database.windows.net',
    database: 'taste',
    pool: {
        max: 10,
        min: 0,
        idleTimeoutMillis: 40000
    },
    options: {
      encrypt: true
    }
  }
  
  const connection = mssql.connect(config, function (err) {
    if (err)
        throw err; 
  });

  module.exports = connection;