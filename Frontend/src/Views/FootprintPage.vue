<template>
  <div class="container">
    <h5>
      Foot
      <span>prints</span>
    </h5>

    <p>Create and Design Your 2D Here</p>
    <div class="d-flex">
      <router-link to="/Footprint/New" class="btn btn-success"
        >Create New</router-link
      >
    </div>
    <h5 class="text-title text-dark">List Footprint</h5>
    <div class="card bg-light">
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <form class="form-inline my-2 my-lg-0">
            <input
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
              <th scope="col">Robot</th>

              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="footprints.length > 0">
            <tr v-for="footprint in footprints" :key="footprint.id">
              <td>{{ footprint.id }}</td>
              <td>{{ footprint.name }}</td>
              <td>{{ footprint.robotname }}</td>

              <td colspan="">
                <div class="d-flex">
                  <button
                    id="detail"
                    class="material-symbols-outlined"
                    @click="detailPath(footprint)"
                  >
                    visibility
                  </button>
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editPath(footprint)"
                  >
                    edit
                  </button>

                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deleteDesign(footprint)"
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

const footprints = ref([]);
const router = useRouter(); // Initialize the router

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/footprints";

const fetchDesign = async () => {
  try {
    const response = await axios.get(apiUrl);
    footprints.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const deleteDesign = async (user) => {
  const confirmDelete = window.confirm(
    "Are you sure you want to delete this map?"
  );
  if (confirmDelete) {
    try {
      await axios.delete(`${apiUrl}/${user.id}`);
      fetchUser();
      window.alert("Map successfully deleted!");
    } catch (error) {
      errorMessage.value = "Failed to delete map: " + error.message;
    }
  }
};

const editPath = (footprint) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-footprint",
    params: { id: footprint.id },
  });
};

// Call fetchMaps on component mount
onMounted(() => {
  fetchDesign();
});
</script>

<style scoped>
.d-flex .material-symbols-outlined {
  border: none;
  background: none;
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
  color: #002cae;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000000;
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
.text-title {
  color: #656565;
  margin-top: -30px;
}
</style>
