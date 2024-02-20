<template>
  <div class="container">
    <h5>
      Mission
      <span>s</span>
    </h5>

    <p>Create and Design Your Mission Here</p>
    <div class="d-flex">
      <button
        v-if="!showCreateForm"
        class="btn btn-success"
        @click="showCreateForm = true"
      >
        Create New
      </button>
    </div>
    <div class="card bg-light" v-if="!showCreateForm">
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <div class="group">
            <svg class="icon" aria-hidden="true" viewBox="0 0 24 24">
              <g>
                <path
                  d="M21.53 20.47l-3.66-3.66C19.195 15.24 20 13.214 20 11c0-4.97-4.03-9-9-9s-9 4.03-9 9 4.03 9 9 9c2.215 0 4.24-.804 5.808-2.13l3.66 3.66c.147.146.34.22.53.22s.385-.073.53-.22c.295-.293.295-.767.002-1.06zM3.5 11c0-4.135 3.365-7.5 7.5-7.5s7.5 3.365 7.5 7.5-3.365 7.5-7.5 7.5-7.5-3.365-7.5-7.5z"
                ></path>
              </g>
            </svg>
            <input
              placeholder="Search"
              type="search"
              class="input"
              v-model="searchTerm"
            />
          </div>
        </div>
        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Site</th>
              <th scope="col">Group</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody>
            <!-- Use the original missions array if searchTerm is empty -->
            <tr
              v-for="mission in searchTerm ? filteredMissions : missions"
              :key="mission.id"
            >
              <td>{{ mission.id }}</td>
              <td>{{ mission.name }}</td>
              <td>{{ mission.site }}</td>
              <td>{{ mission.group }}</td>
              <td colspan="">
                <div class="d-flex">
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editMission(mission)"
                  >
                    edit
                  </button>
                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deleteMission(mission)"
                  >
                    delete
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="d-flex button-flex">
      <button
        type="submit"
        @click="submitForm"
        class="btn btn-success"
        v-if="showCreateForm"
      >
        Create Mission
      </button>
      <button
        v-if="showCreateForm"
        type="button"
        class="btn btn-light"
        @click="cancelForm"
      >
        Back
      </button>
    </div>
    <form class="form" v-if="showCreateForm" @submit.prevent="submitForm">
      <div class="card bg-light">
        <div class="card-body">
          <label id="name" class="text-dark">Name</label>
          <input
            type="text"
            placeholder="Input Name Mission"
            class="form-control form-control-sm"
            v-model="newMission.Name"
          />
          <label id="site" class="text-dark"> Site</label>
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

          <label id="group" class="created"> Group</label
          ><input
            type="text"
            placeholder="Input Group"
            class="form-control form-control-sm"
            v-model="newMission.Group"
          />

          <label class="select">Select Robot</label>
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
    </form>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
const showCreateForm = ref(false);
const missions = ref([]);
const router = useRouter();
const searchTerm = ref("");
const mapOptions = ref([]);
import Swal from "sweetalert2";
import store from "../store";
const robotOptions = ref([]);
const newMission = ref({
  Name: "",
  Site: "",
  Group: "",
  Robot: "",
});
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/missions";
const filteredMissions = ref([]);
const editMission = (mission) => {
  // Log the username before navigating to the edit page
  console.log("Editing user:", mission.name);

  // Use router to navigate to "/edit" and pass the user data as a parameter
  router.push({
    name: "edit-mission",
    params: { id: mission.id },
  });
};
const fetchMission = async () => {
  try {
    console.log("Fetching missions...");

    // Log URL dan parameter yang dikirim
    console.log("URL:", apiUrl);
    console.log("Parameters:", searchTerm.value);

    const response = await axios.get(apiUrl, {
      params: { robot: searchTerm.value }, // Menambahkan parameter robot jika diperlukan
    });

    console.log("Response:", response.data);

    missions.value = response.data;
  } catch (error) {
    console.error("Error fetching missions:", error);

    if (error.response) {
      // The request was made and the server responded with a status code
      console.error("Response Status:", error.response.status);
      console.error("Response Data:", error.response.data);
    } else if (error.request) {
      // The request was made but no response was received
      console.error("No response received. Request details:", error.request);
    } else {
      // Something happened in setting up the request that triggered an Error
      console.error("Error during request setup:", error.message);
    }

    errorMessage.value = "Failed to fetch missions. See console for details.";
  }
};

const deleteMission = async (mission) => {
  // Gunakan SweetAlert untuk konfirmasi penghapusan
  const confirmDelete = await Swal.fire({
    title: "Are You Sure To Delete?",
    text: "You will not be able to return this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Delete!",
    cancelButtonText: "Cancel",
  });

  if (confirmDelete.isConfirmed) {
    try {
      // Hapus pengguna jika pengguna mengkonfirmasi
      await axios.delete(`${apiUrl}/${mission.id}`);
      fetchMission();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Congratss!", "PMission Has Deleted", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

// Filter missions based on search term

const updateFilteredMissions = () => {
  if (searchTerm.value.trim() === "") {
    // If the search term is empty, use the original missions array
    filteredMissions.value = [...missions.value];
  } else {
    // Otherwise, filter based on the search term
    filteredMissions.value = missions.value.filter((mission) =>
      mission.group.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
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
    store.commit("addNotification", "Mission  created!");
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
const cancelForm = () => {
  // Reset form and navigate back to the list view
  newMission.value = {
    Name: "",
    Site: "",
    Group: "",
    Robot: "",
  };
  showCreateForm.value = false;
};
// Call fetchMissions and updateFilteredMissions on component mount
onMounted(() => {
  fetchMission();

  fetchMaps();
  fetchRobots();
});

// Watch for changes in the searchTerm and update filteredMissions
watch(searchTerm, updateFilteredMissions);
</script>

<style scoped>
.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}

.group .input {
  margin-top: 10px;
  margin-bottom: 10px;
  width: 100%;
  height: 30px;
  line-height: 28px;
  padding: 0 1rem;
  padding-left: 2.5rem;
  border: 2px solid transparent;
  border-radius: 8px;
  outline: none;
  background-color: #ffffff;
  color: #0d0c22;
  transition: 0.3s ease;
  box-shadow: 1px 1px 2px #000;
}

.group .input::placeholder {
  color: #9e9ea7;
}
.mr-2 {
  margin-top: 10px;
}
.input:focus,
input:hover {
  outline: none;
  border-color: rgba(0, 10, 196, 0.4);
  background-color: #fff;
  box-shadow: 0 0 0 4px rgba(49, 13, 228, 0.1);
}

.group .icon {
  position: absolute;

  left: 0.6rem;
  fill: #9e9ea7;
  width: 1rem;
  height: 1rem;
}
.d-flex .material-symbols-outlined {
  border: none;
  background: none;
}
#delete:hover {
  color: #ad0000;
}

#edit:hover {
  color: #002aff;
}
.d-flex {
  align-self: flex-end;
}
input {
  height: 20px;
  margin-top: -25px;
  font-size: 12px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
}

h5 {
  margin-top: 20px;
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #000;
  margin-left: -7px;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 700; /* Memberikan jarak atas antara h5 dan p */
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
  margin-top: -50px;
  margin-bottom: 20px;
}
.card {
  margin-right: 40px;
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}

th {
  font-size: 13px;
}

.form #name {
  margin-bottom: 30px;
}
.form .btn {
  margin-top: 10px;
}
.form input {
  border-radius: 7px;
  padding: 8px;
  font-family: "Varela Round", sans-serif;
  font-weight: normal;
  font-size: 13px;
}
.form #group {
  margin-top: 8px;
  margin-bottom: 30px;
}
.button-flex {
  margin-top: -10px;
  margin-bottom: 20px;
}
.form #site {
  margin-top: 8px;
}
.form .select {
  margin-top: 9px;
}
</style>
