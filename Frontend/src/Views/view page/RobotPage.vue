<template>
  <!-- Container div -->
  <div class="container">
    <!-- Judul -->
    <h5>
      Robot
      <span>Management</span>
    </h5>

    <!-- Card untuk menampilkan daftar robot -->
    <div class="card bg-light">
      <div class="card-header">List Of Robot</div>
      <div class="card-body">
        <table class="table table-hover">
          <!-- Header tabel -->
          <thead>
            <tr>
              <th>#</th>
              <th>Name</th>
              <th>Serial Number</th>
              <th>Ip</th>
              <th>Domain Id</th>
              <th>Action</th>
            </tr>
          </thead>
          <!-- Isi tabel -->
          <tbody v-if="paginatedRobots.length > 0">
            <tr v-for="(robot, index) in paginatedRobots" :key="robot.id">
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ robot.name }}</td>
              <td>{{ robot.serialnumber }}</td>
              <td>{{ robot.ip }}</td>
              <td>{{ robot.domainId }}</td>
              <td>
                <div class="d-flex">
                  <!-- Tombol untuk membuka modal edit -->
                  <button
                    class="fa-solid fa-file-pen"
                    data-toggle="modal"
                    data-target="#editRobot"
                    @click="setEditRobot(robot)"
                  ></button>
                  <!-- Tombol untuk menghapus robot -->
                  <button
                    class="fa-solid fa-eraser"
                    @click="deleteRobot(robot)"
                  ></button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
        <!-- Pagination -->
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
import { onMounted, ref, reactive, computed } from "vue";
import Swal from "sweetalert2";

const robots = ref([]);
const apiUrl = "http://localhost:5258/robots";
const errorMessage = ref("");
const editRobotData = ref({
  id: null,
  name: "",
  serialnumber: "",
  ip: "",
  domainId: "",
});
const currentPage = ref(1);
const pageSize = 5; // Jumlah item per halaman

const paginatedRobots = computed(() => {
  const startIndex = (currentPage.value - 1) * pageSize;
  return robots.value.slice(startIndex, startIndex + pageSize);
});

const totalPages = computed(() => Math.ceil(robots.value.length / pageSize));

const fetchRobots = async () => {
  try {
    const response = await axios.get(apiUrl);
    robots.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch robots: " + error.message;
  }
};

const deleteRobot = async (robot) => {
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
      // Panggil endpoint backend untuk menghapus robot
      await axios.delete(`http://localhost:5258/robots/${robot.id}`);

      // Refresh daftar robot setelah penghapusan
      fetchRobots();

      // Tampilkan pesan sukses
      await Swal.fire("Success!", "Robot successfully deleted.", "success");
    } catch (error) {
      // Tangani kesalahan jika gagal menghapus robot
      errorMessage.value = "Failed to delete robot: " + error.message;
    }
  }
};

const setEditRobot = (robot) => {
  editRobotData.value = { ...robot };
};

const editRobot = async () => {
  try {
    await axios.put(`${apiUrl}/${editRobotData.value.id}`, editRobotData.value);
    fetchRobots();
    await Swal.fire("Success!", "Changes saved successfully!", "success");
    $("#editRobot").modal("hide");
  } catch (error) {
    errorMessage.value = "Failed to save changes: " + error.message;
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

onMounted(() => {
  fetchRobots();
});
</script>

<style scoped>
.fa-solid {
  border: none;
  background: none;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column;
}

h5 {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-top: 20px;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
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

th {
  font-size: 13px;
}

.modal-title {
  color: #0800ff;
}

.modal-title span {
  color: #000;
}

.modal-content {
  border-radius: 10px;
}

.modal-body input {
  margin-bottom: 10px;
}

.modal-footer {
  justify-content: flex-end;
}

.btn-primary {
  background-color: #0800ff;
  border-color: #0800ff;
}

.btn-primary:hover {
  background-color: #0600cc;
  border-color: #0600cc;
}

.btn-outline-primary {
  color: #0800ff;
  border-color: #0800ff;
}

.btn-outline-primary:hover {
  color: #ffffff;
  background-color: #0800ff;
  border-color: #0800ff;
}

.btn-outline-danger {
  color: #ff0000;
  border-color: #ff0000;
}

.btn-outline-danger:hover {
  color: #ffffff;
  background-color: #ff0000;
  border-color: #ff0000;
}
</style>
