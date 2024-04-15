<template>
  <div class="container">
    <br>
    <div class="d-flex">
    <h5 class="text-dark">MQTT <span class="text-primary">Configure</span></h5>
    <button class="btn btn-success" @click="goBack">Back</button></div>
    <div class="card bg-dark" style="margin-top: 20px;">
      <div class="card-body">
    <div class="form-group">
      <label for="brokerAddress" class="text-white">Broker Address:</label>
      <input type="text" class="form-control" id="brokerAddress" v-model="newBrokerAddress" required>
    </div>
    <div class="form-group">
      <label for="port" class="text-white">Port:</label>
      <input type="number" class="form-control" id="port" v-model.number="newPort" required>
    </div>
    <div class="form-group">
      <label for="topic" class="text-white">Topic:</label>
      <input type="text" class="form-control" id="topic" v-model="newTopic" required>
    </div>
    <br>
    <button class="btn btn-primary" @click="changeMQTTConfig">Save Changes</button>
    <p class="text-white">MQTT ({{ mqttStatus }})</p></div></div>
  </div>
</template>

<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';
import Swal from 'sweetalert2';
import { useRouter } from 'vue-router';

const router = useRouter();
const newBrokerAddress = ref('');
const newPort = ref(0);
const newTopic = ref('');
const mqttConnected = ref(false);
const mqttStatus = ref('Connecting...');
const goBack = () => {
  router.go(-1); // Kembali ke halaman sebelumnya
};

const fetchDefaultConfig = async () => {
  try {
    const response = await axios.get('http://localhost:5000/default_mqtt_config');
    const { broker_address, port, topic } = response.data;
    newBrokerAddress.value = broker_address;
    newPort.value = port;
    newTopic.value = topic;
  } catch (error) {
    console.error('Error fetching default MQTT configuration:', error);
  }
};

const changeMQTTConfig = async () => {
  try {
    if (!validateForm()) return; // Validasi form sebelum mengirim permintaan
    await axios.post('http://localhost:5000/change_mqtt_config', {
      broker_address: newBrokerAddress.value,
      port: newPort.value,
      topic: newTopic.value
    });
    mqttStatus.value = 'Configurations updated';
    showSuccessAlert();
  } catch (error) {
    console.error('Error updating MQTT configurations:', error);
    mqttStatus.value = 'Error updating configurations';
    showErrorAlert();
  }
};

const validateForm = () => {
  if (!newBrokerAddress.value || !newPort.value || !newTopic.value) {
    // Tampilkan pesan kesalahan jika ada input yang kosong
    Swal.fire({
      icon: 'error',
      title: 'Validation Error',
      text: 'All fields are required',
    });
    return false;
  }
  return true;
};

const showSuccessAlert = () => {
  Swal.fire({
    icon: 'success',
    title: 'Success',
    text: 'MQTT configuration updated successfully',
  });
};

const showErrorAlert = () => {
  Swal.fire({
    icon: 'error',
    title: 'Error',
    text: 'Failed to update MQTT configurations',
  });
};

const checkMQTTConnection = async () => {
  try {
    const response = await axios.get('http://localhost:5000/check_mqtt_connection');
    mqttConnected.value = response.data.connected;
    mqttStatus.value = response.data.connected ? 'Connected' : 'Disconnected';
  } catch (error) {
    console.error('Error checking MQTT connection:', error);
    mqttStatus.value = 'Error checking connection';
  }
};

onMounted(() => {
  fetchDefaultConfig();
  checkMQTTConnection();
});
</script>

<style>
  /* Tambahkan stying di sini sesuai kebutuhan */
  .container{
    font-family: 'Poppins', sans-serif;
  }

  h5{
    font-weight: 700;
  }
  .d-flex{
    display: flex;
    
    justify-content: space-between;
  }
</style>
