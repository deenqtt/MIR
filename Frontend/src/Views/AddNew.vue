<template>
  <div>
    <div class="container">
      <h5>
        Add
        <span>Robot</span>
      </h5>
      <!-- form submit  -->
      <form @submit.prevent="submitForm">
        <div class="card bg-light-subtle">
          <div class="card-body">
            <div class="d-flex">
              <div class="form-group2">
                <label for="robotName">Nama Robot</label>
                <input
                  type="text"
                  class="form-control form-control-sm"
                  id="robotName"
                  v-model="newRobot.Name"
                  required
                />
              </div>
              <div class="form-group1">
                <label for="serialNumber">Serial Number</label>
                <input
                  type="text"
                  class="form-control form-control-sm"
                  id="serialNumber"
                  v-model="newRobot.Serialnumber"
                  required
                />
              </div>
            </div>

            <div class="d-flex">
              <div class="form-group2">
                <label for="ipAddress">IP Address</label>
                <input
                  type="text"
                  class="form-control form-control-sm"
                  id="ipAddress"
                  v-model="newRobot.Ip"
                  required
                />
              </div>
              <div class="form-group1">
                <label for="domainId">Domain ID</label>
                <input
                  type="text"
                  class="form-control form-control-sm"
                  id="domainId"
                  v-model="newRobot.DomainId"
                  required
                />
              </div>
            </div>
          </div>
        </div>

        <button type="submit" class="btn btn-primary">Submit</button>
        <button @click="clearForm" class="btn btn-danger">Clear Form</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useStore } from "vuex";
import Swal from "sweetalert2";
const store = useStore(); // Mengakses store Vuex di dalam setup
const apiUrl = "http://localhost:5258/robots";
const errorMessage = ref("");
const robots = ref([]);
const newRobot = ref({
  Name: "",
  Serialnumber: "",
  Ip: "",
  DomainId: "",
});

const fetchRobots = async () => {
  try {
    const response = await axios.get(apiUrl);
    robots.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch robots: " + error.message;
  }
};
//submit
const submitForm = async () => {
  try {
    const response = await axios.post(apiUrl, newRobot.value);
    console.log(response.data);

    const robotsResponse = await axios.get(apiUrl);
    robots.value = robotsResponse.data;

    newRobot.value = {
      Name: "",
      Serialnumber: "",
      Ip: "",
      DomainId: "",
    };

    // Tampilkan sweet notification untuk memberi tahu pengguna bahwa robot berhasil disimpan
    await Swal.fire("Success!", "Robot successfully saved!", "success");

    // Kirim pesan notifikasi ke store Vuex
    store.commit("addNotification", "Robot successfully saved!");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to save robot: " + error.message;
  }
};
//clear form submit
const clearForm = () => {
  // Kosongkan nilai pada newRobot
  newRobot.value = {
    Name: "",
    Serialnumber: "",
    Ip: "",
    DomainId: "",
  };

  // Tampilkan sweet notification untuk memberi tahu pengguna bahwa form berhasil dikosongkan
  Swal.fire("Success!", "Form successfully cleared!", "success");
};

onMounted(() => {
  console.log("Component mounted");
  fetchRobots();
});
</script>

<style scoped>
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column;
}
input {
  border-radius: 20px;
  font-weight: 700;
  width: 350px;
  opacity: 90%;
  color: #000;
}
h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
  margin-bottom: 20px;
}
span {
  color: #000;
  font-weight: 700;
}
label {
  margin-top: -10px;
  font-size: 12px;
}
.card {
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
  width: auto;
  margin-bottom: 20px;
}
.card .form-group2 {
  font-weight: 500;
  margin-left: 20px;
}
.form-group1 {
  margin-left: 100px;
}
.container .btn {
  margin: 10px;
  color: #000;
  font-weight: 700;
}
</style>
