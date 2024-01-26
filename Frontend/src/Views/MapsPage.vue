<template>
  <div class="container">
    <h5>
      Map
      <span>s</span>
    </h5>

    <p>Create and Design Your Map Here</p>
    <router-link to="/Maps/Created" class="btn btn-success">
      Create Map</router-link
    >
    <div class="card bg-light">
      <div class="card-header">List Map</div>
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <form class="form-inline my-2 my-lg-0">
            <input
              class="form-control mr-sm-2"
              type="search"
              placeholder="Search By Name"
              aria-label="Search"
            />
          </form>
        </div>
        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Created By</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="maps.length > 0">
            <tr v-for="map in maps" :key="map.id">
              <td>{{ map.id }}</td>
              <td>{{ map.name }}</td>

              <td>{{ map.creator }}</td>
              <td colspan="">
                <div class="d-flex">
                  <button
                    class="material-symbols-outlined"
                    @click="detailMap(map)"
                  >
                    visibility
                  </button>
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editMaps(map)"
                  >
                    edit
                  </button>

                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deleteMaps(map)"
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
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router"; // Import useRouter

const maps = ref([]);
const router = useRouter(); // Initialize the router

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

const deleteMaps = async (map) => {
  const confirmDelete = window.confirm(
    "Are you sure you want to delete this map?"
  );
  if (confirmDelete) {
    try {
      await axios.delete(`${apiUrl}/${map.id}`);
      fetchMaps();
      window.alert("Map successfully deleted!");
    } catch (error) {
      errorMessage.value = "Failed to delete map: " + error.message;
    }
  }
};

const editMaps = (map) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-map", // replace 'edit-map' with the name of your edit route
    params: { mapId: map.id }, // adjust this based on the structure of your route
  });
};

// Call fetchMaps on component mount
onMounted(() => {
  fetchMaps();
});
</script>

<style scoped>
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
  color: #193867;
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
.card-header {
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
}
th {
  font-size: 13px;
}
.d-flex .material-symbols-outlined {
  margin-left: 20px;
  margin-right: -20px;
  border: none;
  background: none;
}
#delete {
  color: #e30000;
}
#edit {
  color: #0569ff;
}
</style>
