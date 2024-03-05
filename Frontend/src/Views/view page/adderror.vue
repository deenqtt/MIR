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
                  v-model="newError.Robotname"
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
import { ref, onMounted } from "vue";
import axios from "axios";
import Swal from "sweetalert2";
import { useStore } from "vuex";
const store = useStore();
const robotOptions = ref([]);
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/errors";
const newError = ref({
  Robotname: "",
  Date: "",
  Explained: "",
});

const onSubmit = async () => {
  try {
    if (
      !newError.value.Robotname ||
      !newError.value.Date ||
      !newError.value.Explained
    ) {
      // Show an alert or set an error message indicating that the form is incomplete
      await Swal.fire("Please fill all fields", "", "warning");
      store.commit("addNotification");
      return;
    }

    const response = await axios.post(apiUrl, newError.value);
    console.log(response.data);

    await Swal.fire("Success!", "Error Successfully Sent!", "success");

    // Clear the form fields after successful submission
    newError.value = {
      Robotname: "",
      Date: "",
      Explained: "",
    };

    // Refetch errors after adding a new one
    fetchError();
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create error: " + error.message;
  }
};

const fetchError = async () => {
  try {
    const response = await axios.get(apiUrl);
    errors.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
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
onMounted(() => {
  fetchError();
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

  text-align: center; /* Menempatkan gambar ke kanan */
}

.error-image {
  width: 400px; /* Mengurangi ukuran gambar */
  height: 300px;
  border-radius: 20px;
}
</style>
