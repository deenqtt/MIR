import Swal from "sweetalert2";
import { ref } from "vue";

import * as Paho from "paho-mqtt";

console.log(Paho);

let client = null;

export let mqttHost = ref(localStorage.getItem("mqttHost") || "192.168.2.119");
export let mqttPort = ref(parseInt(localStorage.getItem("mqttPort")) || 9000);
export let mqttTopic = ref(localStorage.getItem("mqttTopic") || "Batt");
export let batteryPercentage = ref(0);

function onConnectionLost(responseObject) {
  if (responseObject.errorCode !== 0) {
    console.log("onConnectionLost:" + responseObject.errorMessage);
  }
}

function onMessageArrived(message) {
  console.log("Received message:", message.payloadString);
  console.log("onMessageArrived:" + message.payloadString);
  try {
    const payload = JSON.parse(message.payloadString);
    if (typeof payload === "number") {
      batteryPercentage.value = payload;
      updateBatteryIcon(payload);
    }
  } catch (error) {
    console.error("Error parsing MQTT message payload:", error);
  }
}

export const mqttConnected = ref(false);

export function connectClient(options) {
  if (!client || !client.isConnected()) {
    client = new Paho.Client(
      mqttHost.value,
      mqttPort.value,
      `${new Date().getTime() + Math.round(Math.random() * 1000)}`
    );

    client.onConnectionLost = onConnectionLost;
    client.onMessageArrived = onMessageArrived;

    client.connect({
      onSuccess: () => {
        console.log(
          "Connected to MQTT on",
          mqttHost.value,
          "port",
          mqttPort.value,
          "topic",
          mqttTopic.value
        );
        client.subscribe(mqttTopic.value);
        mqttConnected.value = true;
        if (options && options.onSuccess) {
          options.onSuccess();
        }
      },
      onFailure: () => {
        console.error("Failed to connect to MQTT");
        if (options && options.onFailure) {
          options.onFailure();
        }
      },
    });
  }
}

export function updateMqttSettings(host, port, topic) {
  console.log(
    "Updating MQTT settings to host",
    host,
    "port",
    port,
    "topic",
    topic
  );
  mqttHost.value = host;
  mqttPort.value = port;
  mqttTopic.value = topic;

  // Simpan nilai host, port, dan topic ke localStorage
  localStorage.setItem("mqttHost", host);
  localStorage.setItem("mqttPort", port);
  localStorage.setItem("mqttTopic", topic);

  // Selanjutnya, sambungkan ulang ke MQTT broker dengan pengaturan yang baru
  if (client && client.isConnected()) {
    console.log("Disconnecting from the current host...");
    client.disconnect();
    Swal.fire({
      icon: "info",
      title: "Updating MQTT settings",
      text: "Disconnecting from the current host...",
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      willOpen: () => {
        Swal.showLoading();
      },
    });
    setTimeout(() => {
      console.log("Connecting to the new host...");
      connectClient({
        onSuccess: () => {
          Swal.fire({
            icon: "success",
            title: "MQTT settings updated",
            text: "Connected to the new host",
          });
        },
        onFailure: () => {
          Swal.fire({
            icon: "error",
            title: "Failed to connect to the new host",
          });
        },
      });
    }, 2000);
  } else {
    Swal.fire({
      icon: "info",
      title: "Updating MQTT settings",
      text: "Connecting to the new host...",
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      willOpen: () => {
        Swal.showLoading();
      },
    });
    console.log("Connecting to the new host...");
    connectClient({
      onSuccess: () => {
        Swal.fire({
          icon: "success",
          title: "MQTT settings updated",
          text: "Connected to the new host",
        });
      },
      onFailure: () => {
        Swal.fire({
          icon: "error",
          title: "Failed to connect to the new host",
        });
      },
    });
  }
}
