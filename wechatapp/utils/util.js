function formatTime(date) {
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var day = date.getDate()

  var hour = date.getHours()
  var minute = date.getMinutes()
  var second = date.getSeconds()


  return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':')
}

function formatNumber(n) {
  n = n.toString()
  return n[1] ? n : '0' + n
}

module.exports = {
  formatTime: formatTime
}

const API_URL = "http://tasteservice.azurewebsites.net/api/menu";

function fetchData(){
  var promise = new Promise(function(resolve, reject){
    wx.request({
      url: API_URL,
      data: {
        id:"2",
      },
      header: {
        'Content-Type': 'application/json'
      },
      success: resolve,
      fail: reject
    })
  });
  return promise;
}
