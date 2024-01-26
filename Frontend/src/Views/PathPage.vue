<template>
  <div class="container">
    <h5>
      Pat
      <span>h</span>
    </h5>

    <p>Create and Design Your Path Here</p>
    <div class="d-flex">
      <button class="btn btn-danger mr-2">Delete All</button>
      <router-link to="/Path/New" class="btn btn-success"
        >Create Path</router-link
      >
    </div>
    <div class="card bg-light">
      <div class="card-header">List Path</div>
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
        <table class="table table-striped">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Start</th>
              <th scope="col">Goal</th>
              <th scope="col">Distance</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="paths.length > 0">
            <tr v-for="path in paths" :key="path.id">
              <td>{{ path.id }}</td>
              <td>{{ path.name }}</td>
              <td>{{ path.start }}</td>
              <td>{{ path.goal }}</td>
              <td>{{ path.distance }}</td>
              <td colspan="">
                <div class="d-flex">
                  <button
                    id="detail"
                    class="material-symbols-outlined"
                    @click="detailPath(path)"
                  >
                    visibility
                  </button>
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editPath(path)"
                  >
                    edit
                  </button>

                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deletePath(path)"
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

const paths = ref([]);
const router = useRouter(); // Initialize the router

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/paths";

const fetchPath = async () => {
  try {
    const response = await axios.get(apiUrl);
    paths.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const deletePath = async (path) => {
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

const editPath = (path) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-path", // replace 'edit-map' with the name of your edit route
    params: { pathId: path.id }, // adjust this based on the structure of your route
  });
};

// Call fetchMaps on component mount
onMounted(() => {
  fetchPath();
});
</script>

<style scoped>
#delete {
  color: #9f0000;
}
#edit {
  color: #2032ff;
}
#detail {
  color: #00751f;
}
.d-flex .material-symbols-outlined {
  border: none;
  background: none;
  justify-content: space-between;
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
</style>
