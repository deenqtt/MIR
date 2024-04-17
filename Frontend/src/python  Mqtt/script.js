const socket = io();

socket.on('mqtt_message', function (data) {
    document.getElementById('mqttData').innerText = 'MQTT Data: ' + data;
});
