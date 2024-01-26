<template>
  <div>
    <div
      class="container"
      :style="{ marginLeft: is_expanded ? 'var(--sidebar-width)' : '30px' }"
    >
      <h5>
        Add
        <span>Robot</span>
      </h5>
      <form @submit.prevent="submitForm">
        <div
          class="card bg-light-subtle"
          :style="{
            width: is_expanded
              ? 'calc(100% - var(--sidebar-width))'
              : 'calc(100% - 30px)',
          }"
        >
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

        <button type="submit" class="btn btn-danger">Clear Form</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";

const robots = ref([]);
const newRobot = ref({
  Name: "",
  Serialnumber: "",
  Ip: "",
  DomainId: "",
});

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/robots";

const fetchRobots = async () => {
  try {
    const response = await axios.get(apiUrl);
    robots.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch robots: " + error.message;
  }
};

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

    // Tampilkan notifikasi sukses (gunakan library notifikasi atau buat sendiri)
    // Contoh sederhana dengan menggunakan window.alert
    window.alert("Robot successfully saved!");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to save robot: " + error.message;
  }
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
  background-color: #bababa;
  width: 350px;
}
h5 {
  font-size: 25px;
  font-weight: 500;
  color: #193867;
}
span {
  color: #000;
}
label {
  margin-top: -10px;
  font-size: 12px;
}
.btn {
  width: auto;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  margin-top: 20px;
  margin-left: 40px;
}
.card {
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}
.card .form-group2 {
  font-weight: 500;
  margin-left: 20px;
}
.form-group1 {
  margin-left: 100px;
}
</style>
