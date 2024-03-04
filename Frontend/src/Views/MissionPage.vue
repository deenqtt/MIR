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
              @input="fetchMission"
            />
          </div>
        </div>
        <table class="table table-hover">
          <!-- Bagian kepala tabel -->
          <thead class="thead-dark">
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Site</th>
              <th scope="col">Group</th>
              <th scope="col" class="text-end">Action</th>
            </tr>
          </thead>
          <!-- Bagian badan tabel -->
          <tbody>
            <!-- Iterasi setiap item misi pada halaman aktif -->
            <tr v-for="(mission, index) in paginatedMissions" :key="mission.id">
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ mission.name }}</td>
              <td>{{ mission.site }}</td>
              <td>{{ mission.group }}</td>
              <td>
                <!-- Tombol edit -->
                <div class="d-flex justify-content-end">
                  <button
                    id="delete"
                    class="fa-solid fa-delete-left"
                    @click="deleteMission(mission)"
                  >
                    <span>Delete</span>
                  </button>

                  <button
                    id="edit"
                    class="fa-solid fa-pen-to-square"
                    @click="editMission(mission)"
                  >
                    <span>Edit</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>

        <!-- Tombol navigasi halaman -->
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
    </form>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import Swal from "sweetalert2";

const showCreateForm = ref(false);
const missions = ref([]);
const router = useRouter();
const searchTerm = ref("");
const mapOptions = ref([]);
const robotOptions = ref([]);
const newMission = ref({
  Name: "",
  Site: "",
  Group: "",
  Robot: "",
});
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/missions/all";
const apiUrll = "http://localhost:5258/missions";
const currentPage = ref(1); // Halaman saat ini
const pageSize = ref(5); // Jumlah item per halaman
// Menghitung total halaman berdasarkan jumlah item dan ukuran halaman
const totalPages = computed(() =>
  Math.ceil(missions.value.length / pageSize.value)
);
// Menghitung indeks awal item pada halaman saat ini
const startIndex = computed(() => (currentPage.value - 1) * pageSize.value);
// Menghitung indeks akhir item pada halaman saat ini
const endIndex = computed(() =>
  Math.min(startIndex.value + pageSize.value - 1, missions.value.length - 1)
);
// Memotong data misi menjadi halaman-halaman
const paginatedMissions = computed(() =>
  missions.value.slice(startIndex.value, endIndex.value + 1)
);
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
    const response = await axios.get(apiUrl);
    missions.value = response.data.filter((mission) =>
      mission.name.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  } catch (error) {
    errorMessage.value = "Failed to fetch missions: " + error.message;
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
      // Hapus misi jika pengguna mengkonfirmasi
      await axios.delete(`${apiUrll}/${mission.id}`);
      fetchMission();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Congratss!", "Mission Has Deleted", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete mission: " + error.message;
    }
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
    const response = await axios.post(apiUrll, newMission.value);
    console.log("Success:", response.data); // Log response jika input berhasil
    const missionsResponse = await axios.get(apiUrl);
    missions.value = missionsResponse.data;
    // Clear the form fields after successful submission
    newMission.value = { Name: "", Site: "", Group: "", Robot: "" };
    await Swal.fire("Success!", "Mission successfully created!", "success");
    // Redirect to "/Mission/Created/New"
    router.push("/Mission/Created/New");
  } catch (error) {
    console.error("Error:", error); // Log error jika terjadi kesalahan
    errorMessage.value = "Failed to create mission: " + error.message;
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
  $(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
  });
  fetchMission();
  fetchMaps();
  fetchRobots();
});

// Watch for changes in the searchTerm and update filteredMissions
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
#edit span,
#delete span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
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
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: auto;
  width: auto;
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
.fa-solid {
  border: none;
  border-radius: 4px;
  margin-left: 5px;
  font-size: 20px;
}
#delete span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
  margin-top: -10px;
}
#edit span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}
#delete {
  background: #ff4f4f;
}
#edit {
  background: #ffed29;
  height: 27px;
}
span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-left: -7px;
}
</style>
