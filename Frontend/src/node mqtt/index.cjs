const express = require('express');
const mqtt = require('mqtt');
const http = require('http');
const WebSocket = require('ws');

const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

let mqttClient;
let mqttConnected = false;

// Endpoint untuk menerima koneksi WebSocket dari klien web
wss.on('connection', function connection(ws) {
  if (mqttConnected) {
    // Jika sudah terhubung ke broker MQTT, beritahu klien web
    ws.send('connected');
  }

  // Tangani pesan dari klien web dan teruskan ke broker MQTT
  ws.on('message', function incoming(message) {
    if (mqttClient && mqttConnected) {
      mqttClient.publish('topic', message.toString());
    }
  });
});

// Buat koneksi TCP ke broker MQTT
mqttClient = mqtt.connect('ws://broker.emqx.io:8083/mqtt');

// Tangani kejadian saat terhubung ke broker MQTT
mqttClient.on('connect', function () {
  console.log('Connected to MQTT broker');
  mqttConnected = true;
});

// Tangani kejadian jika koneksi ke broker MQTT ditutup
mqttClient.on('close', function () {
  console.log('Connection to MQTT broker closed');
  mqttConnected = false;
});

server.listen(8080, function listening() {
  console.log('WebSocket-to-TCP proxy berjalan di port 8080');
});
