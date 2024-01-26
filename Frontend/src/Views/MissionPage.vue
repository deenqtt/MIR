<template>
  <div class="container">
    <h5>
      Mission
      <span>s</span>
    </h5>

    <p>Create and Design Your Mission Here</p>
    <div class="d-flex">
      <router-link to="/Mission/New" class="btn btn-success"
        >Create New</router-link
      >
    </div>
    <div class="card bg-light">
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <form class="form-inline my-2 my-lg-0">
            <input
              v-model="searchTerm"
              class="form-control mr-sm-2"
              type="search"
              placeholder="Search By Group"
              aria-label="Search"
            />
          </form>
        </div>
        <table class="table table-striped">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Site</th>
              <th scope="col">Group</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="filteredMissions.length > 0">
            <tr v-for="mission in filteredMissions" :key="mission.id">
              <td>{{ mission.id }}</td>
              <td>{{ mission.name }}</td>
              <td>{{ mission.site }}</td>
              <td>{{ mission.group }}</td>

              <td colspan="">
                <div class="d-flex">
                  <button
                    id="detail"
                    class="material-symbols-outlined"
                    @click="detailMission(mission)"
                  >
                    visibility
                  </button>
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
          <tbody v-else>
            <tr>
              <td colspan="5">No missions found</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";

const missions = ref([]);
const router = useRouter();
const searchTerm = ref(""); // Step 1

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/missions";

const fetchMission = async () => {
  try {
    const response = await axios.get(apiUrl);
    missions.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch missions: " + error.message;
  }
};
const deleteMission = async (mission) => {
  const confirmDelete = window.confirm(
    "Are you sure you want to delete this map?"
  );
  if (confirmDelete) {
    try {
      await axios.delete(`${apiUrl}/${mission.id}`);
      fetchMission();
      window.alert("Map successfully deleted!");
    } catch (error) {
      errorMessage.value = "Failed to delete map: " + error.message;
    }
  }
};

const editMission = (mission) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-mission", // replace 'edit-map' with the name of your edit route
    params: { missionId: mission.id }, // adjust this based on the structure of your route
  });
};
// Filter missions based on search term
const filteredMissions = ref([]);

const updateFilteredMissions = () => {
  filteredMissions.value = missions.value.filter((mission) =>
    mission.group.toLowerCase().includes(searchTerm.value.toLowerCase())
  );
};

// Call fetchMissions and updateFilteredMissions on component mount
onMounted(() => {
  fetchMission();
});

// Watch for changes in the searchTerm and update filteredMissions
watch(searchTerm, updateFilteredMissions);
</script>

<style scoped>
.d-flex .material-symbols-outlined {
  border: none;
  background: none;
}
#delete {
  color: #ad0000;
}
#detail {
  color: #000;
}
#edit {
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
  font-size: 25px;
  font-weight: 500;
  color: #235196;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000;
  margin-left: -7px;
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
  margin-right: 40px;
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}

th {
  font-size: 13px;
}
</style>
