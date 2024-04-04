<template>
  <div class="container">
    <div class="d-flex">
      <h5>Customize MQTT</h5>
      <button class="btn btn-success">Back</button>
    </div>
    <div class="card bg-light">
      <div class="card-body">
        <label for="mqttHost">Host:</label>
        <input
          class="form-control"
          type="text"
          id="mqttHost"
          v-model="mqttHost"
        />
        <label class="form-cotrol" for="mqttPort">Port:</label>
        <input
          class="form-control"
          type="number"
          id="mqttPort"
          v-model.number="mqttPort"
        />
        <label for="mqttTopic">Topic:</label>
        <input
          class="form-control"
          type="text"
          id="mqttTopic"
          v-model="mqttTopic"
        />
        <br />
        <button class="btn btn-primary" @click="updateMqttSettingss">
          Update MQTT Settings
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import {
  updateMqttSettings,
  mqttHost,
  mqttPort,
  mqttTopic,
} from "../../router/mqtt";
const mqttHostValue = mqttHost;
const mqttPortValue = mqttPort;
const mqttTopicValue = mqttTopic;
const updateMqttSettingss = () => {
  updateMqttSettings(mqttHostValue.value, mqttPortValue.value, mqttTopicValue.value);
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  margin-top: 20px;
  font-family: "Poppins", sans-serif;
}
.container h5 {
  font-weight: 500;
  margin-bottom: 20px;
}
.d-flex {
  justify-content: space-between;
}
.btn-success {
  margin-bottom: 10px;
}
</style>
