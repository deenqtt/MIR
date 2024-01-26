<template>
  <div class="container">
    <h5>
      Created
      <span>Path</span>
    </h5>

    <p>Create and Design Your Path Here</p>
    <router-link to="/Path" class="btn btn-light"> Back</router-link>
    <form action="" @submit.prevent="submitForm">
      <button class="btn btn-success" type="submit">Save</button>

      <div class="card bg-light">
        <div class="card-body form-flex">
          <div class="form-group">
            <label for="Name">Name</label>
            <input
              type="text"
              class="form-control form-control-sm"
              placeholder="Input Your Path Name"
              v-model="newPath.Name"
            />
          </div>
          <div class="form-group">
            <label for="Select">Select Map</label>
            <select class="form-control form-control-sm" v-model="newPath.Map">
              <option value="" disabled selected>Select Map (Mapping)</option>
              <option v-for="name in mapOptions" :key="name" :value="name">
                {{ name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label for="Start">Start</label>
            <div class="input-group">
              <input
                type="text"
                readonly
                class="form-control form-control-sm with-button"
                v-model="newPath.Start"
              />
              <div class="input-group-append">
                <button class="material-symbols-outlined">near_me</button>
              </div>
            </div>
          </div>
          <div class="form-group">
            <label for="Goal">Goal</label>
            <div class="input-group">
              <input
                type="text"
                readonly
                class="form-control form-control-sm with-button"
                v-model="newPath.Goal"
              />
              <div class="input-group-append">
                <button class="material-symbols-outlined">near_me</button>
              </div>
            </div>
          </div>
          <div class="form-group">
            <label for="Distance">Distance</label>
            <input
              type="text"
              readonly
              class="form-control form-control-sm"
              v-model="newPath.Distance"
            />
          </div>
        </div>
      </div>
    </form>
    <br />

    <div class="card bg-light">
      <div class="card-header">
        <p>Select Your Start And Goal</p>
      </div>
      <div class="card-body"><canvas style="width: 100%"></canvas></div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const paths = ref([]);
const robots = ref([]);
const mapOptions = ref([]);
const newPath = ref({
  Name: "",
  Map: "",
  Start: "",
  Goal: "",
  Distance: "",
});

// const errorMessage = ref("");
const apiUrl = "http://localhost:5258/paths";

const fetchPaths = async () => {
  try {
    const response = await axios.get(apiUrl);
    paths.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const fetchMaps = async () => {
  try {
    const response = await axios.get("http://localhost:5258/maps");
    mapOptions.value = response.data.map((map) => map.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};

const submitForm = async () => {
  try {
    if (
      !newPath.value.Name ||
      !newPath.value.Map ||
      !newPath.value.Start ||
      !newPath.value.Goal
    ) {
      // Show an alert or set an error message indicating that the form is incomplete
      window.alert("Please fill in all fields.");
      return;
    }
    const response = await axios.post(apiUrl, newPath.value);
    console.log(response.data);

    const pathsResponse = await axios.get(apiUrl);
    paths.value = pathsResponse.data;

    // Clear the form fields after successful submission
    newPath.value = {
      Name: "",
      Map: "",
      Start: "",
      Goal: "",
      Distance: "",
    };

    // Redirect to "/map/created/new"
    router.push("/Path");

    window.alert("Path successfully created!");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create map: " + error.message;
  }
};

onMounted(() => {
  console.log("Component mounted");
  fetchPaths();
  fetchMaps();
});
</script>

<style scoped>
canvas {
  border: #000 solid 1px;
  height: 100px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
}

h5 {
  font-size: 25px;
  font-weight: 500;
  color: #193867;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 100; /* Memberikan jarak atas antara h5 dan p */
}

.btn {
  text-align: center;
  width: 120px;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: 30px;
  align-self: flex-end;
  margin-right: 40px;
  margin-top: -10px;
  margin-bottom: 10px;
}

.card {
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}

.card-header {
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header p {
  font-size: 12px;
  font-weight: 500;
  margin-top: -20px;
  margin-bottom: -20px;
}

.card-header button {
  border: none;
}

.form-flex {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.form-group {
  flex: 1;
  margin-right: 10px;
  margin-bottom: 10px;
}
.form-group input {
  border-radius: 10px;
  background-color: #b4b4b4;
}
.form-group select {
  border-radius: 10px;
  background-color: #b4b4b4;
}
/* Style for input group */
.input-group {
  position: relative;
  width: 100%;
}

.with-button {
  border-radius: 0.25rem 0 0 0.25rem;
}

.input-group-append {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  z-index: 2;
}

.input-group-append .material-symbols-outlined {
  border-radius: 0 0.25rem 0.25rem 0;
  background-color: #b4b4b4;
  margin-right: 9px;
  border: none;
}
</style>
