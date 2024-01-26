<template>
  <div class="container">
    <h5>
      Created
      <span>Maps</span>
    </h5>
    <p class="p">Create and Design Your Map Here</p>
    <router-link class="btn btn-light btn-sm bg-danger" to="/maps"
      >Back</router-link
    >
    <form action="" @submit.prevent="submitForm">
      <div class="card bg-light">
        <div class="card-body">
          <p class="text-dark">Name</p>
          <input
            type="text"
            placeholder="Input Name Maps"
            class="form-control form-control-sm"
            v-model="newMap.Name"
            required
          />
          <div class="flex">
            <p class="text-dark">
              Site
              <input
                type="text"
                placeholder="Input Site"
                class="form-control form-control-sm"
                v-model="newMap.SiteName"
                required
              />
            </p>
            <br />
            <p class="created">
              Created By
              <select
                class="form-control form-control-sm"
                v-model="newMap.Creator"
              >
                <option value="" disabled selected>
                  Select Creator (Robot)
                </option>
                <option v-for="name in robotOptions" :key="name" :value="name">
                  {{ name }}
                </option>
              </select>
            </p>
          </div>
        </div>
      </div>
      <br />
      <div class="d-flex">
        <button type="submit" class="btn btn-success">Create Map</button>
        <button type="button" class="btn btn-light" @click="cancelForm">
          Cancel
        </button>
      </div>
    </form>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const maps = ref([]);
const robots = ref([]);
const robotOptions = ref([]);
const newMap = ref({
  Name: "",
  SiteName: "",
  Creator: "",
});

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/maps";

const fetchMaps = async () => {
  try {
    const response = await axios.get(apiUrl);
    maps.value = response.data;
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

const submitForm = async () => {
  try {
    if (!newMap.value.Name || !newMap.value.SiteName || !newMap.value.Creator) {
      // Show an alert or set an error message indicating that the form is incomplete
      window.alert("Please fill in all fields.");
      return;
    }

    const response = await axios.post(apiUrl, newMap.value);
    console.log(response.data);

    const mapsResponse = await axios.get(apiUrl);
    maps.value = mapsResponse.data;

    // Clear the form fields after successful submission
    newMap.value = {
      Name: "",
      SiteName: "",
      Creator: "",
    };

    // Redirect to "/map/created/new"
    router.push("/Maps/Created/New");

    window.alert("Map successfully created!");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create map: " + error.message;
  }
};

const cancelForm = () => {
  // Use router to navigate back to "/maps" when cancel button is clicked
  router.push("/maps");
};

onMounted(() => {
  console.log("Component mounted");
  fetchMaps();
  fetchRobots();
});
</script>
<style scoped>
.container {
  font-family: "Poppins", sans-serif;
}
.flex {
  display: flex;
  margin-top: 10px;
}
.flex .created {
  margin-left: 20px;
  font-size: 13px;
}
.text-dark {
  font-size: 13px;
}
.flex .created input {
  width: 400px;
}
.flex .text-dark input {
  width: 500px;
}
.d-flex .btn {
  font-size: 12px;
  color: #000;
  font-weight: 500;
  border-radius: 8px;
  margin: 10px;
}
h5 {
  font-size: 25px;
  font-weight: 500;
  color: #0060f1;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000;
}
input {
  border-radius: 10px;
  font-size: 12px;
  background-color: #d3d3d3;
  opacity: 100%;
}
.container .p {
  font-weight: 100;
  font-size: 12px;
}
.card {
  margin-top: 8px;
  border-radius: 10px;
  box-shadow: 1px 1px 4px #000;
}
</style>
