//https://adityasridhar.com/posts/how-to-easily-use-grpc-and-protocol-buffers-with-nodejs

const grpc = require("@grpc/grpc-js");
const protoLoader = require("@grpc/proto-loader");
const PROTO_PATH = "./proto/waterlevel.proto";
const http= require("node:http");
const fs = require("node:fs");

const options = {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
  };

  let packageDefinition = protoLoader.loadSync(PROTO_PATH, options);

  let waterlevel_proto = grpc.loadPackageDefinition(packageDefinition).waterlevel;

  function main (write, timestamp=new Date().getTime()-21600000) { // 6h
      //client stub
      let client = new waterlevel_proto.WaterLevel('141.45.58.51:50051',grpc.credentials.createInsecure());
      
      client.GetData({beginning_timestamp: Math.round((timestamp/1000)).toString()}, function (err, data) {
        if (err){ write(err)};
        let waterLevelValue = JSON.parse(data.json).map(point => Number(point.value));
        let waterLevelTime = JSON.parse(data.json).map(point => { 
          let date = new Date(Number(point.timestamp)*1000);
          let hours = date.getHours().toString().padStart(2, '0');
          let minutes =date.getMinutes().toString().padStart(2, '0');
          return hours + ':' + minutes;
          });      
        write(undefined,waterLevelTime, waterLevelValue);
      });
  }

http.createServer((req, res) => 
{
  if(req.url == "/subscribe"){
    res.writeHead(200, {"Content-Type": "text/event-stream"});
    const ID = setInterval(() =>{
      main(function (err, xValue, yValue){
        if(err) res.end("data: " + JSON.stringify(err)+"\n\n");
        res.write("data: " + JSON.stringify({xValue, yValue})+"\n\n");
        console.log("Send new data");
      }, new Date().getTime()-(1000*10));
    },1000*10);
    req.on("close",()=> 
    {
      clearInterval(ID);
      if(req.complete) res.end()
    });
  }
  else{
    const html = fs.readFileSync("./index.html",{encoding: "utf8"});
    res.writeHead(200,{"Content-Type": "text/html"});
    main(function (err, xValue, yValue){
      if(err) res.end(JSON.stringify(err));
        res.end(html.replace("${{yValues}}",JSON.stringify(yValue)).replace("${{xValues}}",JSON.stringify(xValue)));
  });}
}).listen(8000);


