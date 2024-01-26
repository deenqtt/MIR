<template>
  <div class="container">
    <h5>
      Created
      <span>Mission</span>
    </h5>
    <p class="p">Create and Design Your Mission Here</p>
    <router-link class="btn btn-light btn-sm bg-light" to="/mission"
      >Back</router-link
    >
    <form action="" @submit.prevent="submitForm">
      <div class="card bg-light">
        <div class="card-body">
          <label class="text-dark">Name</label>
          <input
            type="text"
            placeholder="Input Name Mission"
            class="form-control form-control-sm"
            v-model="newMission.Name"
          />
          <div class="flex">
            <label class="text-dark">
              Site
              <select
                class="form-control form-control-sm"
                v-model="newMission.Site"
              >
                <option value="" disabled selected>Select Map (Mapping)</option>
                <option
                  v-for="siteName in mapOptions"
                  :key="siteName"
                  :value="siteName"
                >
                  {{ siteName }}
                </option>
              </select>
            </label>

            <label class="created">
              Group<input
                type="text"
                placeholder="Input Group"
                class="form-control form-control-sm"
                v-model="newMission.Group"
              />
            </label>
          </div>
          <p class="text-dark">Select Robot</p>
          <select
            class="form-control form-control-sm"
            v-model="newMission.Robot"
          >
            <option value="" disabled selected>Select Robot</option>
            <option v-for="name in robotOptions" :key="name" :value="name">
              {{ name }}
            </option>
          </select>
        </div>
      </div>
      <br />
      <div class="d-flex">
        <button type="submit" class="btn btn-success">Create Mission</button>
        <button class="btn btn-light">Cancel</button>
      </div>
    </form>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const missions = ref([]);
const mapOptions = ref([]);
const robotOptions = ref([]);
const newMission = ref({
  Name: "",
  Site: "",
  Group: "",
  Robot: "",
});

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/missions";

const fetchMission = async () => {
  try {
    const response = await axios.get(apiUrl);
    paths.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};
const submitForm = async () => {
  try {
    if (
      !newMission.value.Name ||
      !newMission.value.Group ||
      !newMission.value.Robot ||
      !newMission.value.Site
    ) {
      // Show an alert or set an error message indicating that the form is incomplete
      window.alert("Please fill in all fields.");
      return;
    }
    const response = await axios.post(apiUrl, newMission.value);
    console.log(response.data);

    const missionsResponse = await axios.get(apiUrl);
    missions.value = missionsResponse.data;

    // Clear the form fields after successful submission
    newMission.value = {
      Name: "",
      Site: "",
      Group: "",
      Robot: "",
    };

    // Redirect to "/map/created/new"
    router.push("/Mission/Created/New");

    window.alert("Path successfully created!");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create map: " + error.message;
  }
};
const fetchMaps = async () => {
  try {
    const response = await axios.get("http://localhost:5258/maps");
    mapOptions.value = response.data.map((map) => map.siteName);
  } catch (error) {
    console.error("Error fetching site names:", error);
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

// const submitForm = async () => {
//   try {
//     if (
//       !newPath.value.Name ||
//       !newPath.value.Map ||
//       !newPath.value.Start ||
//       !newPath.value.Goal
//     ) {
//       // Show an alert or set an error message indicating that the form is incomplete
//       window.alert("Please fill in all fields.");
//       return;
//     }
//     const response = await axios.post(apiUrl, newPath.value);
//     console.log(response.data);

//     const pathsResponse = await axios.get(apiUrl);
//     paths.value = pathsResponse.data;

//     // Clear the form fields after successful submission
//     newPath.value = {
//       Name: "",
//       Map: "",
//       Start: "",
//       Goal: "",
//       Distance: "",
//     };

//     // Redirect to "/map/created/new"
//     router.push("/Path");

//     window.alert("Path successfully created!");
//   } catch (error) {
//     console.error(error);
//     errorMessage.value = "Failed to create map: " + error.message;
//   }
// };

onMounted(() => {
  console.log("Component mounted");
  fetchMission();
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
  margin-top: 16px;
}
.text-dark {
  font-size: 13px;
}
.flex .created input {
  width: 400px;
}
.flex .text-dark select {
  width: 400px;
  border-radius: 8px;
  margin: 10px;
}
.flex .created .d-flex .btn {
  font-size: 12px;
  color: #000;
  font-weight: 500;
  border-radius: 8px;
  margin: 10px;
}
.flex .text-dark {
  margin-top: 10px;
  border-radius: 8px;
  font-size: 12px;
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
select {
  border-radius: 10px;
  font-size: 12px;
  background-color: #d3d3d3;
  opacity: 100%;
}
</style>
