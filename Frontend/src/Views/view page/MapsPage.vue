<template>
  <div class="container">
    <h5>
      Map
      <span>s</span>
    </h5>

    <p>Create and Design Your Map Here</p>
    <button
      v-if="!showCreateForm"
      class="btn btn-success"
      @click="showCreateForm = true"
    >
      Create Map
    </button>
    <div class="card bg-light" v-if="!showCreateForm">
      <div class="card-header">List Map</div>
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
              @input="fetchMap"
            />
          </div>
        </div>

        <table class="table table-hover">
          <thead class="thead-dark justify-content-between">
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Created By</th>
              <th scope="col" class="text-center">Action</th>
            </tr>
          </thead>
          <tbody v-if="paginatedMaps.length > 0">
            <tr v-for="(map, index) in paginatedMaps" :key="map.id">
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ map.name }}</td>
              <td>{{ map.creator }}</td>
              <td>
                <div class="d-flex justify-content-md-center">
                  <button
                    id="edit"
                    class="fa-solid fa-pen-to-square"
                    @click="editMaps(map)"
                  >
                    <span>Edit</span>
                  </button>
                  <button
                    id="delete"
                    class="fa-solid fa-delete-left"
                    @click="deleteMaps(map)"
                  >
                    <span>Delete</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>

        <nav aria-label="Page navigation example">
          <ul class="pagination">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <button class="page-link" @click="prevPage">&laquo;</button>
            </li>
            <li
              class="page-item"
              v-for="page in totalPages"
              :key="page"
              :class="{ active: currentPage === page }"
            >
              <button class="page-link" @click="changePage(page)">
                {{ page }}
              </button>
            </li>
            <li
              class="page-item"
              :class="{ disabled: currentPage === totalPages }"
            >
              <button class="page-link" @click="nextPage">&raquo;</button>
            </li>
          </ul>
        </nav>
      </div>
    </div>

    <form class="form" v-if="showCreateForm" @submit.prevent="submitForm">
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
          <br />
          <div class="flex">
            <p class="text-dark">Site</p>
            <input
              type="text"
              placeholder="Input Site"
              class="form-control form-control-sm"
              v-model="newMap.SiteName"
              required
            />

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
        <button
          type="button"
          class="btn bg-black text-white"
          @click="cancelForm"
        >
          Cancel
        </button>
      </div>
    </form>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router"; // Import useRouter
import Swal from "sweetalert2";
import store from "../../store";
const showCreateForm = ref(false);
const maps = ref([]);
const router = useRouter(); // Initialize the router
const searchTerm = ref("");
const robotOptions = ref([]);
const errorMessage = ref("");
const currentPage = ref(1);
const apiUrl = "http://localhost:5258/maps";
const pageSize = 5; // Jumlah item per halaman
const paginatedMaps = computed(() => {
  const startIndex = (currentPage.value - 1) * pageSize;
  return maps.value.slice(startIndex, startIndex + pageSize);
});
const totalPages = computed(() => Math.ceil(maps.value.length / pageSize));
const newMap = ref({
  Name: "",
  SiteName: "",
  Creator: "",
});
const cancelForm = async () => {
  const confirmCancel = await Swal.fire({
    title: "Are you sure?",
    text: "Any unsaved changes will be discarded.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, cancel",
    cancelButtonText: "No, go back",
  });

  if (confirmCancel.isConfirmed) {
    newMap.value = {
      Name: "",
      SiteName: "",
      Creator: "",
    };
    showCreateForm.value = false;
  } else {
    // If user chose not to cancel, do nothing or handle as needed
  }
};

const fetchMap = async () => {
  try {
    const response = await axios.get(apiUrl);
    const allMaps = response.data;

    // Filter paths based on the search term
    maps.value = allMaps.filter((map) =>
      map.name.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const deleteMaps = async (map) => {
  // Gunakan SweetAlert untuk konfirmasi penghapusan
  const confirmDelete = await Swal.fire({
    title: "Are You Sure To Delete?",
    text: "You will not be able to return this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Yes, delete!",
    cancelButtonText: "Cancel",
  });

  if (confirmDelete.isConfirmed) {
    try {
      // Hapus pengguna jika pengguna mengkonfirmasi
      await axios.delete(`${apiUrl}/${map.id}`);
      fetchMaps();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Success!", "Maps succesfull delete.", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};
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
    await Swal.fire("Success!", "Map successfully created!", "success");
    store.commit("addNotification", "Map successfully created!");
    // Clear the form fields after successful submission
    newMap.value = {
      Name: "",
      SiteName: "",
      Creator: "",
    };

    // Redirect to "/map/created/new"
    router.push("/Maps/Created/New");
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create map: " + error.message;
  }
};

const hideForm = () => {
  // Set showCreateForm to false to hide the form and show the card
  showCreateForm.value = false;
};
const editMaps = (map) => {
  // Log the username before navigating to the edit page
  console.log("Editing user:", map.name);

  // Use router to navigate to "/edit" and pass the user data as a parameter
  router.push({
    name: "edit-map",
    params: { id: map.id },
  });
};
const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
  }
};

const changePage = (page) => {
  currentPage.value = page;
};

// Call fetchMaps on component mount
onMounted(() => {
  fetchMap();
  fetchMaps();
  fetchRobots();
});
</script>

<style scoped>
.table thead th {
  padding-right: 20px; /* Menambahkan jarak di sebelah kanan setiap kolom */
}

.table thead th:last-child {
  padding-right: 0; /* Menghapus jarak di sebelah kanan kolom terakhir */
}

.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}

.group .input {
  margin-top: -20px;
  width: 100%;
  height: 30px;
  line-height: 28px;
  padding: 0 1rem;
  padding-left: 2.5rem;
  border: 2px solid transparent;
  border-radius: 8px;
  outline: none;
  background-color: #f3f3f4;
  color: #0d0c22;
  transition: 0.3s ease;
}

.group .input::placeholder {
  color: #9e9ea7;
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
  top: -0.8rem;
  left: 0.6rem;
  fill: #9e9ea7;
  width: 1rem;
  height: 1rem;
}

table {
  border-radius: 10px;
}
thead {
  background-color: #282d34;
}
.container .form-inline .form-control {
  height: 30px;
  margin-top: -25px;
  font-size: 14px;
}
.container {
  width: 100%;
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
}

h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
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
  font-weight: 500; /* Memberikan jarak atas antara h5 dan p */
}
.container .btn {
  text-align: center;
  width: 120px;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: 30px;
  align-self: flex-end;
  margin-right: 40px;
  margin-top: -55px;
  margin-bottom: 40px;
}
.bg-black {
  color: #ffff;
}
.card {
  margin-right: 40px;
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}
.card-header {
  margin: 10px;
  border-radius: 10px;
}
.card-header:hover {
  background-color: #002d71;
  color: #ffff;
}
th {
  font-size: 13px;
}
.btn-toolbar {
  margin-bottom: 10px;
}
.btn {
  margin-right: 10px;
}
.d-flex .fa-solid {
  border: none;
  background: none;
}
#delete {
  background: #ff8080;
  border-radius: 6px;
  margin-left: 10px;
}
#edit {
  background: #fcee50;
  border-radius: 6px;
  width: 90px;
  height: 28px;
}
#delete span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}
#edit span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}

#edit:hover {
  color: #0569ff;
}
.form input {
  border-radius: 10px;
  font-size: 12px;
  background-color: #d3d3d3;
  opacity: 100%;
}
.form .text-dark {
  margin-bottom: 30px;
}
.form .text-dark input {
  margin-top: 40px;
}
.form {
  display: flex;
  flex-direction: column;
}
.form .btn {
  margin: 10px;
  width: auto;
}
.form input {
  margin-top: -22px;
}
</style>
