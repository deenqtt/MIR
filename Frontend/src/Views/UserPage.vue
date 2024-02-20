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
            <input placeholder="Search" type="search" class="input" />
          </div>
        </div>
        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">No Whatsapp</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="users.length > 0">
            <tr v-for="user in users" :key="user.id">
              <td>{{ user.id }}</td>
              <td>{{ user.username }}</td>
              <td>{{ user.phone }}</td>

              <td colspan="">
                <div class="d-flex">
                  <button
                    id="detail"
                    class="fa-solid fa-circle-info"
                    @click="detailPath(user)"
                  ></button>
                  <button
                    id="edit"
                    class="fa-solid fa-pen-to-square"
                    @click="editUser(user)"
                  ></button>

                  <br />

                  <button
                    id="delete"
                    class="fa-solid fa-user-slash"
                    data-toggle="tooltip"
                    data-bs-placement="right"
                    title="Delete"
                    @click="deleteUser(user)"
                  ></button>
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
import { onMounted } from "vue";
import { useRouter } from "vue-router";
import { ref } from "vue";

import Swal from "sweetalert2";
const router = useRouter();
const users = ref([]);
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/users";
const tooltip = ref({
  show: false,
});
const fetchUser = async () => {
  try {
    const response = await axios.get(apiUrl);
    users.value = response.data;
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
  $(document).ready(function () {
    $("#element").tooltip("update");
    $('[data-toggle="tooltip"]').tooltip();
  });
  fetchUser(); // Fetch all users instead of a specific user
});
</script>

<style scoped>
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
  color: #e00707;
}
#edit {
  color: #2032ff;
}
#detail {
  color: #00751f;
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
