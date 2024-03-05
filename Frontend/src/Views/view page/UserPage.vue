<template>
  <div class="container">
    <h5>
      User
      <span>Management</span>
    </h5>

    <p>Manage User Here</p>

    <div class="card bg-light">
      <div class="card-header">Edit And Delete User</div>
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
              @input="fetchUser"
            />
          </div>
        </div>
        <table class="table table-hover">
          <thead class="thead-dark text-white">
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">No Whatsapp</th>
              <th scope="col" class="text-end">Action</th>
            </tr>
          </thead>
          <tbody v-if="paginatedUsers.length > 0">
            <tr v-for="(user, index) in paginatedUsers" :key="user.id">
              <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
              <td>{{ user.username }}</td>
              <td>{{ user.phone }}</td>

              <td colspan="">
                <div class="d-flex justify-content-end">
                  <button
                    id="edit"
                    class="fa-solid fa-pen-to-square"
                    @click="editUser(user)"
                  >
                    <span>Edit</span>
                  </button>

                  <br />

                  <button
                    id="delete"
                    class="fa-solid fa-user-slash"
                    data-toggle="tooltip"
                    data-bs-placement="right"
                    title="Delete"
                    @click="deleteUser(user)"
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
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import Swal from "sweetalert2";

const searchTerm = ref("");
const router = useRouter();
const users = ref([]);
const apiUrl = "http://localhost:5258/users";
const errorMessage = ref("");
const currentPage = ref(1);
const pageSize = 5; // Jumlah item per halaman

const paginatedUsers = computed(() => {
  const startIndex = (currentPage.value - 1) * pageSize;
  return users.value.slice(startIndex, startIndex + pageSize);
});

const totalPages = computed(() => Math.ceil(users.value.length / pageSize));
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
const fetchUser = async () => {
  try {
    const response = await axios.get(apiUrl);
    users.value = response.data.filter((user) =>
      user.username.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  } catch (error) {
    errorMessage.value = "Failed to fetch users: " + error.message;
  }
};

const deleteUser = async (user) => {
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
      await axios.delete(`${apiUrl}/${user.id}`);
      fetchUser();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Berhasil!", "Pengguna berhasil dihapus.", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

const editUser = (user) => {
  // Log the username before navigating to the edit page
  console.log("Editing user:", user.username);

  // Use router to navigate to "/edit" and pass the user data as a parameter
  router.push({
    name: "edit-user",
    params: { id: user.id },
  });
};

onMounted(() => {
  fetchUser(); // Fetch all users instead of a specific user
});
</script>

<style scoped>
#edit span {
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
  color: #000;
}
#delete span {
  color: #000;
  font-family: "Poppins", sans-serif;
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500;
}
.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}
.mr-2 {
  margin-top: 1px;
}
.group .input {
  margin-top: 1px;
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

.input:focus,
input:hover {
  outline: none;
  border-color: rgba(0, 10, 196, 0.4);
  background-color: #fff;
  box-shadow: 0 0 0 4px rgba(49, 13, 228, 0.1);
}

.group .icon {
  position: absolute;
  top: 0.6em;
  left: 0.6rem;
  fill: #9e9ea7;
  width: 1rem;
  height: 1rem;
}
#delete {
  color: #000;
  background: #ff6060;
  width: auto;
  height: auto;
  border-radius: 4px;
}
#edit {
  color: #000000;
  background: #2f8ac3;
  width: auto;
  height: 25px;
  border-radius: 4px;
}

.d-flex .fa-solid {
  border: none;
  background: none;
  margin: 5px;
  font-size: 1rem;
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
  color: #000000;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 500; /* Memberikan jarak atas antara h5 dan p */
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
  background-color: #02007c;
  color: #fff;
}
th {
  font-size: 13px;
}
</style>
