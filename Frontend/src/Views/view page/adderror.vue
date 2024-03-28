<template>
  <div class="container">
    <h5>Error</h5>
    <div class="card">
      <div class="card-body">
        <div class="form-container">
          <form action="" class="form" @submit.prevent="onSubmit">
            <div class="form-group">
              <label for="robotName">Robot Name:</label>
              <div class="input-group mb-3">
                <div class="input-group-prepend">
                  <label class="input-group-text" for="inputGroupSelect01"
                    >Options</label
                  >
                </div>
                <select
                  class="custom-select"
                  id="inputGroupSelect01"
                  v-model="selectedRobot"
                >
                  <option selected>Choose...</option>
                  <option
                    v-for="name in robotOptions"
                    :key="name"
                    :value="name"
                  >
                    {{ name }}
                  </option>
                </select>
              </div>
            </div>
            <div class="form-group">
              <label for="date">Date:</label>
              <input
                v-model="newError.Date"
                type="date"
                class="form-control form-control-sm"
                name="date"
                id="date"
              />
            </div>
            <div class="form-group">
              <label for="explained">Explained:</label>
              <textarea
                v-model="newError.Explained"
                type="text"
                class="form-control"
                name="explained"
                id="explained"
              />
            </div>
            <button class="btn btn-success" type="submit">Send</button>
          </form>
          <div class="image-container">
            <img
              src="../image/tetx.png"
              alt="Error Image"
              class="error-image"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import Swal from "sweetalert2";
import store from "../../store";
const selectedRobot = ref("");
const newError = ref({ Date: "", Explained: "" });
const robotOptions = ref("");
const onSubmit = async () => {
  try {
    if (
      !selectedRobot.value ||
      !newError.value.Date ||
      !newError.value.Explained
    ) {
      await Swal.fire("Please fill all fields", "", "warning");
      return;
    }

    const response = await axios.post("http://localhost:5258/errors", {
      Robotname: selectedRobot.value,
      Date: newError.value.Date,
      Explained: newError.value.Explained,
    });

    // Jika pesan kesalahan berhasil dikirim, tambahkan notifikasi ke toko Vuex
    store.commit("addNotification", {
      read: false, // Set notifikasi sebagai belum dibaca
    });

    await Swal.fire("Success!", "Error Successfully Sent!", "success");

    newError.value = { Date: "", Explained: "" };
  } catch (error) {
    console.error(error);
    await Swal.fire("Failed!", "Error Sending Failed!", "error");
  }
};

const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};
const sendErrorMessageToRobot = async (robotName, errorMessage) => {
  try {
    console.log(`Sending error message to ${robotName}: ${errorMessage}`);
    await store.dispatch("sendErrorMessageToRobot", errorMessage);
  } catch (error) {
    console.error(
      `Failed to send error message to ${robotName}: ${error.message}`
    );
  }
};
onMounted(() => {
  fetchRobots();
});
</script>

<style scoped>
.form {
  width: 50%;
}
.container {
  font-family: "Poppins", sans-serif;
}

.card {
  margin-top: 20px;
  box-shadow: 1px 2px 2px #000;
}

.form-container {
  display: flex;
}

.form-group {
  padding-right: 10px;
}

.form-control {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.image-container {
  width: 50%;
  text-align: center;
}

.error-image {
  width: 400px;
  height: 300px;
  border-radius: 20px;
}
</style>
