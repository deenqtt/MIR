<template>
  <div class="container">
    <h5>
      Foot
      <span>prints</span>
    </h5>
    <p>Create and Design Your 2D Here</p>
    <div class="d-flex">
      <button
        class="btn btn-success"
        v-if="!showCreateForm"
        @click="showCreateForm = true"
      >
        Create New
      </button>
    </div>
    <h5 class="text-title text-dark" v-if="!showCreateForm">List Footprint</h5>
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
              @input="fetchDesign"
            />
          </div>
        </div>

        <table class="table table-hover">
          <thead class="thead-dark">
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Robot</th>
              <th scope="col" class="text text-end">Action</th>
            </tr>
          </thead>
          <tbody v-if="footprints.length > 0">
            <tr
              v-for="(footprint, index) in paginatedFootprints"
              :key="footprint.id"
            >
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ footprint.name }}</td>
              <td>{{ footprint.robotname }}</td>

              <td colspan="">
                <div class="d-flex justify-content-end">
                  <button
                    id="edit"
                    class="fa-solid fa-pen-to-square"
                    @click="editDesgin(footprint)"
                  >
                    <span>Edit</span>
                  </button>

                  <br />

                  <button
                    id="delete"
                    class="fa-solid fa-delete-left"
                    @click="deleteDesign(footprint)"
                  >
                    <span>Delete</span>
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
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, computed, watch } from "vue";
import { useRouter } from "vue-router"; // Import useRouter
import Swal from "sweetalert2";
const showCreateForm = ref(false);
const searchTerm = ref("");
const footprints = ref([]);
const robotOptions = ref([]);
const router = useRouter(); // Initialize the router
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/footprints";
const currentPage = ref(1); // Halaman saat ini
const pageSize = ref(5); // Jumlah item per halaman
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

// Menghitung total halaman berdasarkan jumlah item dan ukuran halaman
const totalPages = computed(() =>
  Math.ceil(footprints.value.length / pageSize.value)
);

// Menghitung indeks awal item pada halaman saat ini
const startIndex = computed(() => (currentPage.value - 1) * pageSize.value);

// Menghitung indeks akhir item pada halaman saat ini
const endIndex = computed(() =>
  Math.min(startIndex.value + pageSize.value - 1, footprints.value.length - 1)
);

// Memotong data misi menjadi halaman-halaman
const paginatedFootprints = computed(() =>
  footprints.value.slice(startIndex.value, endIndex.value + 1)
);

let lines = ref([]);

const newFoot = ref({
  Name: "",
  Robotname: "",
});
const cancelForm = () => {
  // Reset form and navigate back to the list view
  newFoot.value = {
    Name: "",
    Robotname: "",
  };
  showCreateForm.value = false;
};

const fetchDesign = async () => {
  try {
    const response = await axios.get(apiUrl);
    // Filter data berdasarkan searchTerm
    footprints.value = response.data.filter((footprint) =>
      footprint.name.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  } catch (error) {
    errorMessage.value = "Failed to fetch footprints: " + error.message;
  }
};

const deleteDesign = async (footprint) => {
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
      await axios.delete(`${apiUrl}/${footprint.id}`);
      fetchDesign();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Congratss!", "Design Has Deleted", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

const editDesgin = (footprint) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-footprint",
    params: { id: footprint.id },
  });
};
const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};

onMounted(() => {
  fetchRobots();
  fetchDesign();
});
</script>

<style scoped>
.text {
  margin-right: -200px !important;
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
#edit {
  background: #83a4ef;
  border-radius: 4px;
}
#delete {
  background: #ff6363;
  border-radius: 4px;
  margin-left: 20px;
}
.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}

.group .input {
  margin-top: 10px;
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
  margin-bottom: 10px;
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
.d-flex .fa-solid {
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
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
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
  margin-top: -60px;
  margin-bottom: 50px;
}
.card {
  margin-right: 40px;
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}

th {
  font-size: 13px;
}
.text-title {
  color: #656565;
  margin-top: -30px;
}

.container {
  display: flex;
  flex-direction: column;
}
.canvass canvas {
  border: 1px solid #000;
  margin-bottom: 10px;
  cursor: crosshair;
}
.canvass .btn {
  width: auto;
}

.canvass .form-flex {
  display: flex;
  justify-content: space-between;
}

.canvass .form-group {
  flex: 0 0 48%; /* Adjust width as needed */
}
</style>
