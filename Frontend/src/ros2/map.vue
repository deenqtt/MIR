<template>
  <div>
    <h3>Navigation Map</h3>
    <div id="nav"></div>
  </div>
</template>

<script setup>
import Config from "../data/config";
import * as ROSLIB from "roslib";

let connected = false;
let ros = null;
let viewer2 = null;
let nav = null;

const initConnection = () => {
  ros = new ROSLIB.Ros();
  
  setInterval(() => {
    reconnect();
  }, 5000);

  ros.on("connection", () => {
    console.log("[map] Connection established successfully");
    connected = true;
    mapView();
  });

  ros.on("close", (error) => {
    console.log(error);
    console.log("Connection closed");
    connected = false;
  });

  reconnect();
};

const reconnect = () => {
  if (!connected) {
    console.log('Reconnecting');
    try {
      ros.connect(`ws://${Config.ROSBRIDGE_SERVER_IP}:${Config.ROSBRIDGE_SERVER_PORT}`).onerror( function (e){
        console.log("Error caught by connect: ");
        console.log(e);
      });
    } catch (error) {
      console.log("Connection problem");
    }
  }
};

const mapView = () => {
  if (connected) {
    try {
      viewer2 = new ROS2D.Viewer({
        divID : 'nav',
        width : 750,
        height : 550
      });
      nav = new NAV2D.OccupancyGridClientNav({
        ros : ros,
        rootObject : viewer2.scene,
        viewer : viewer2,
        serverName : '/move_base',
        topic : "/map"
      });
      console.log("Nav setup complete")
    } catch (error) {
      console.log("Nav error");
      console.log(error);
    }
  }
};

initConnection();
</script>
