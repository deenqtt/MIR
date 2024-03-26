<template>
  <!-- Container div -->
  <div class="container">
    <h5>
      Pat
      <span>h</span>
    </h5>
    <p>Create and Design Your Path Here</p>
    <div class="d-flex">
      <button
        class="btn btn-danger mr-2"
        v-if="!showCreateForm"
        @click="deleteAllPaths"
      >
        Delete All
      </button>
      <button
        class="btn btn-success"
        v-if="!showCreateForm"
        @click="showCreateForm = true"
      >
        Create Path
      </button>
    </div>
    <div class="card bg-light" v-if="!showCreateForm">
      <div class="card-header">List Path</div>
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
              @input="fetchPath"
            />
          </div>
        </div>
        <table class="table table-hover">
          <thead class="thead-dark">
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Start</th>
              <th scope="col">Goal</th>
              <th scope="col">Distance</th>
              <th scope="col" class="text-end">Action</th>
            </tr>
          </thead>
          <tbody v-if="paginatedPaths.length > 0">
            <tr v-for="(path, index) in paginatedPaths" :key="path.id">
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ path.name }}</td>
              <td>{{ path.start }}</td>
              <td>{{ path.goal }}</td>
              <td>{{ path.distance }}</td>
              <td colspan="" class="justify-content-end">
                <div class="d-flex justify-content-end">
                  <button
                    class="fa-solid fa-eye"
                    id="detail"
                    @click="detailPath(path)"
                  >
                    <span>Details</span>
                  </button>
                  <button
                    class="fa-solid fa-pen-to-square"
                    id="edit"
                    @click="editPath(path)"
                  >
                    <span>Edit</span>
                  </button>
                  <br />
                  <button
                    class="fa-solid fa-eraser"
                    id="delete"
                    @click="deletePath(path)"
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
    <form class="path" @submit.prevent="submitForm" v-if="showCreateForm">
      <div class="d-flex">
        <button
          class="btn btn-success"
          v-if="!ShowPath"
          type="submit"
          @click="submitForm"
        >
          Save
        </button>
        <button class="btn btn-light" @click="cancelForm">Back</button>
      </div>
      <div class="card bg-light">
        <div class="card-body form-flex">
          <div class="form-group">
            <label id="name">Name</label>
            <input
              type="text"
              class="form-control form-control-sm"
              placeholder="Input Your Path Name"
              v-model="newPath.Name"
              required
            />
            <label id="select">Select Map</label>
            <select class="form-control form-control-sm" v-model="newPath.Map">
              <option value="" disabled selected>Select Map (Mapping)</option>
              <option v-for="name in mapOptions" :key="name" :value="name">
                {{ name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label id="Start">Start</label>
            <div class="input-group">
              <input
                type="text"
                class="form-control form-control-sm with-button"
                v-model="newPath.Start"
                required
              />
              <button
                type="button"
                class="fa-solid fa-arrow-pointer"
                @click="selectStartPoint"
              ></button>
              <pre>{{ startPoint }}</pre>
            </div>
            <br />
            <label id="Goal">Goal</label>
            <div class="input-group">
              <input
                v-model="newPath.Goal"
                id="endPoint"
                type="text"
                class="form-control form-control-sm with-button"
                required
              />
              <button
                type="button"
                class="fa-solid fa-arrow-pointer"
                @click="selectEndPoint"
              ></button>
              <pre>{{ endPoint }}</pre>
            </div>
          </div>
          <div class="form-group">
            <label id="Distance">Distance</label>
            <input
              type="text"
              class="form-control form-control-sm"
              v-model="newPath.Distance"
              required
            />{{ distance }}
          </div>
        </div>
      </div>
    </form>
    <br />
    <!-- Modal -->
    <div
      class="modal fade"
      id="pathDetailModal"
      tabindex="-1"
      role="dialog"
      aria-labelledby="pathDetailModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="pathDetailModalLabel">Path Detail</h5>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
            >
              <span aria-hidden="true" data-dismiss="modal">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <p><strong>Name:</strong> {{ selectedPath.name }}</p>
            <p><strong>Start:</strong> {{ selectedPath.start }}</p>
            <p><strong>Goal:</strong> {{ selectedPath.goal }}</p>
            <p><strong>Distance:</strong> {{ selectedPath.distance }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, reactive, computed } from "vue";
import { useRouter } from "vue-router"; // Import useRouter
const showCreateForm = ref(false);
import Swal from "sweetalert2";
import store from "../../store";
const searchTerm = ref("");
const paths = ref([]);
const router = useRouter(); // Initialize the router
const startPoint = ref("");
const endPoint = ref("");
const distance = ref(null);
const mapCanvas = ref(null);
const mapOptions = ref([]);
const newPath = ref({
  Name: "",
  Map: "",
  Start: "",
  Goal: "",
  Distance: "",
});
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/paths";
const selectedPoint = reactive({ x: null, y: null });
const isSelectingStart = ref(true);
const selectedPath = reactive({});
const submitForm = async () => {
  if (!validateForm()) {
    // Jika validasi gagal, munculkan Sweet Alert
    Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "Please fill in all fields!",
    });
    return;
  }
  try {
    const response = await axios.post(apiUrl, newPath.value);
    console.log(response.data);

    const pathsResponse = await axios.get(apiUrl);
    paths.value = pathsResponse.data;
    store.commit("addNotification", "Path created!");
    // Clear the form fields after successful submission
    newPath.value = {
      Name: "",
      Map: "",
      Start: "",
      Goal: "",
      Distance: "",
    };

    // Show success message using SweetAlert
    await Swal.fire("Success!", "Path successfully created!", "success");

    // Redirect to "/path" after success
    router.go(-1);
  } catch (error) {
    console.error(error);
    errorMessage.value = "Failed to create map: " + error.message;
  }
};
const validateForm = () => {
  // Lakukan validasi di sini
  return (
    newPath.Name &&
    newPath.Map &&
    newPath.Start &&
    newPath.Goal &&
    newPath.Distance
  );
};
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
    newPath.value = {
      Name: "",
      Map: "",
      Start: "",
      Goal: "",
      Distance: "",
    };
    showCreateForm.value = false;
  } else {
    // If user chose not to cancel, do nothing or handle as needed
  }
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
const currentPage = ref(1);
const pageSize = 5; // Jumlah item per halaman

const paginatedPaths = computed(() => {
  const startIndex = (currentPage.value - 1) * pageSize;
  return paths.value.slice(startIndex, startIndex + pageSize);
});

const totalPages = computed(() => Math.ceil(paths.value.length / pageSize));
// Update detailPath method to set selectedPath
const detailPath = (path) => {
  selectedPath.name = path.name;
  selectedPath.start = path.start;
  selectedPath.goal = path.goal;
  selectedPath.distance = path.distance;
  $("#pathDetailModal").modal("show");
};
const fetchPath = async () => {
  try {
    const response = await axios.get(apiUrl);
    const allPaths = response.data;

    // Filter paths based on the search term
    paths.value = allPaths.filter((path) =>
      path.name.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};
const deleteAllPaths = async () => {
  // Gunakan SweetAlert untuk konfirmasi penghapusan
  const confirmDelete = await Swal.fire({
    title: "Are You Sure To Delete All Path?",
    text: "You will not be able to return this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    cancelButtonText: "Cancel",
    confirmButtonText: "Delete",
  });

  if (confirmDelete.isConfirmed) {
    try {
      // Menggunakan axios untuk mengirim permintaan DELETE ke endpoint yang sesuai
      const response = await axios.delete(
        "http://localhost:5258/paths/delete-all"
      );

      // Jika penghapusan berhasil, muat ulang daftar path
      if (response.status === 200) {
        fetchPath();
        await Swal.fire("Success!", "All Paths Deleted.", "success");
      } else {
        throw new Error("Failed to delete all paths");
      }
    } catch (error) {
      // Tangani kesalahan jika permintaan gagal
      errorMessage.value = "Failed to delete all paths: " + error.message;
    }
  }
};

const deletePath = async (path) => {
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
      await axios.delete(`${apiUrl}/${path.id}`);
      fetchPath();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Berhasil!", "Path berhasil dihapus.", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

const fetchPaths = async () => {
  try {
    const response = await axios.get(apiUrl);
    paths.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const fetchMaps = async () => {
  try {
    const response = await axios.get("http://localhost:5258/maps");
    mapOptions.value = response.data.map((map) => map.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};

const canvasClick = (event) => {
  const canvas = (event && event.target) || mapCanvas.value;

  if (!canvas) {
    console.error("Canvas element is null or undefined");
    return;
  }

  const rect = canvas.getBoundingClientRect();
  // ...
};

const drawMap = () => {
  const canvas = mapCanvas.value;
  const ctx = canvas.getContext("2d");
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  // Draw fake map
  ctx.fillStyle = "#c0c0c0";
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // Draw starting point
  drawPoint(ctx, startPoint.value, "green");

  // Draw ending point
  drawPoint(ctx, endPoint.value, "red");

  // Draw line between start and end points
  if (startPoint.value && endPoint.value) {
    drawLine(ctx, startPoint.value, endPoint.value);
    calculateDistance();
  }
};

const drawPoint = (ctx, point, color) => {
  if (!point) return;
  const [x, y] = point.split(",").map(Number);
  const radius = 5; // Adjust the radius as needed
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.arc(x, y, radius, 0, 2 * Math.PI);
  ctx.fill();
  ctx.fillStyle = "black";
  ctx.fillText(`(${Math.round(x)},${Math.round(y)})`, x + 10, y - 10);
};

const drawLine = (ctx, startPoint, endPoint) => {
  const [x1, y1] = startPoint.split(",").map(Number);
  const [x2, y2] = endPoint.split(",").map(Number);

  ctx.strokeStyle = "blue";
  ctx.lineWidth = 2;
  ctx.beginPath();
  ctx.moveTo(x1, y1);
  ctx.lineTo(x2, y2);
  ctx.stroke();
};

const selectStartPoint = () => {
  isSelectingStart.value = true;
  // Ubah nilai startPoint sesuai dengan koordinat yang dipilih (mungkin menggunakan selectedPoint)
  newPath.Start = `${selectedPoint.x},${selectedPoint.y}`;
  // Panggil fungsi untuk menggambar peta ulang setelah mengubah startPoint
  drawMap();
};

const selectEndPoint = () => {
  isSelectingStart.value = false;
  newPath.Start = `${selectedPoint.x},${selectedPoint.y}`;
  // Panggil fungsi untuk menggambar peta ulang setelah mengubah startPoint
  drawMap();
};

const calculateDistance = () => {
  const [x1, y1] = startPoint.value.split(",").map(Number);
  const [x2, y2] = endPoint.value.split(",").map(Number);

  // Basic distance calculation using Pythagorean theorem
  const deltaX = x2 - x1;
  const deltaY = y2 - y1;
  const distanceInPixels = Math.sqrt(deltaX ** 2 + deltaY ** 2);

  // Assuming 1 pixel is equal to 1 meter
  distance.value = distanceInPixels.toFixed(2) + " meters";
};

const editPath = (path) => {
  // Log the username before navigating to the edit page
  console.log("Editing Path:", path.name);

  // Use router to navigate to "/edit" and pass the user data as a parameter
  router.push({
    name: "edit-path",
    params: { id: path.id },
  });
};

onMounted(() => {
  fetchPaths();
  fetchMaps();
});
</script>

<style scoped>
.btn {
  display: inline-block;
  position: relative;
  padding: 6px 12px;
  font-size: 14px;
  font-weight: 600;
  line-height: 1.5;
  text-align: center;
  text-transform: uppercase;
  vertical-align: middle;
  cursor: pointer;
  border: none;
  border-radius: 6px;
  transition: all 0.3s ease;
}

.btn span {
  display: inline-block;
}

.btn-primary {
  color: #fff;
  background-color: #007bff;
  border-color: #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
}

.btn-warning {
  color: #212529;
  background-color: #ffc107;
  border-color: #ffc107;
}

.btn-warning:hover {
  background-color: #e0a800;
  border-color: #d39e00;
}

.btn-danger {
  color: #fff;
  background-color: #dc3545;
  border-color: #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
  border-color: #bd2130;
}

.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}

.group .input {
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

.input:focus,
input:hover {
  outline: none;
  border-color: rgba(0, 10, 196, 0.4);
  background-color: #fff;
  box-shadow: 0 0 0 4px rgba(49, 13, 228, 0.1);
}

.group .icon {
  position: absolute;
  top: -1.1rem;
  left: 0.6rem;
  fill: #9e9ea7;
  width: 1rem;
  height: 1rem;
}

#name {
  color: black;
}
#select {
  color: black;
}
#Start {
  color: black;
}
#Goal {
  color: black;
}
#Distance {
  color: black;
}
#detail {
  background: #007bff;
  border-radius: 6px;
  width: auto;
  height: auto;
}
#delete span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 400;
  margin-right: 10px;
}
#edit span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}
#detail span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}
#edit {
  background: #fcee50;
  border-radius: 6px;
  width: 90px;
}
#delete {
  background: #ff838f;
  border-radius: 6px;
  width: auto;
  height: auto;
}
.canvas {
  width: 100%;
  height: 400px;
  border: 2px solid #333;
}
.canvas canvas {
  cursor: crosshair;
}
.with-button {
  width: calc(100% - 40px);
}
canvas {
  border: #000 solid 1px;
  height: 100px;
}

.container .form-inline .form-control {
  height: 30px;
  font-size: 15px;
  border-radius: 8px;
}
.d-flex .fa-solid {
  border: none;
  background: none;
  font-size: 19px;
}
.fa-pen-to-square {
  margin-left: 10px;
  margin-right: 10px;
}
.d-flex {
  align-self: flex-end;
}
.container input {
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
  margin-top: -60px;
  margin-bottom: 40px;
}
.card {
  margin-right: 40px;
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}
.card-header {
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
}
.card-header:hover {
  background: #110d79;
  transition: 3s ease-in-out linear;
  color: #fff;
}
th {
  font-size: 13px;
}
.path {
  display: flex;
  flex-direction: column;
}
.path .d-flex .btn {
  margin-top: -60px;
  margin-bottom: 40px;
}
.path .card #name {
  margin-bottom: 30px;
}
.path .card #Start {
  margin-bottom: 25px;
}
.path .card #Goal {
  margin-bottom: 25px;
}
.path .card #Distance {
  margin-bottom: 25px;
}
.path .input-group {
  position: relative;
  width: 100%;
  display: flex;
}

.path .input-group input {
  flex: 1;
}

.path .input-group button {
  height: calc(25px + 1px);
  position: absolute;
  top: 80%;
  right: 2px;
  transform: translateY(-50%);
  margin-top: -13px;
  background: none;
  border: none;
}
</style>
