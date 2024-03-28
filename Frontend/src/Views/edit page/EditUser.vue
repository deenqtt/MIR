<template>
  <div class="container">
    <h5>
      Edit
      <span>User</span>
    </h5>
    <div>
      <button @click="confirmBack" class="btn btn-secondary btn-back">
        Back
      </button>
    </div>
    <div class="card bg-light">
      <div class="card-header">Edit User</div>
      <div class="card-body">
        <form @submit.prevent="updateUser">
          <div class="form-group">
            <label for="name">Username</label>
            <input
              type="text"
              class="form-control"
              v-model="selectedUser.username"
            />
          </div>
          <div class="form-group">
            <label for="phone">Phone</label>
            <input
              type="text"
              class="form-control"
              v-model="selectedUser.phone"
            />
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <input
              type="password"
              class="form-control"
              v-model="selectedUser.password"
            />
          </div>
          <div>
            <button type="submit" class="btn btn-primary">Update</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import Swal from "sweetalert2";
const router = useRouter();
const userId = ref(null);
const selectedUser = ref({
  id: null,
  username: "",
  phone: "",
  password: "",
});

const apiUrl = "http://localhost:5258/users";

const fetchUser = async (userId) => {
  try {
    const response = await axios.get(`${apiUrl}/${userId}`);
    selectedUser.value = response.data;
  } catch (error) {
    console.error("Failed to fetch user:", error);
  }
};

const updateUser = async () => {
  try {
    if (userId.value) {
      await axios.put(`${apiUrl}/${userId.value}`, selectedUser.value);
      console.log("User updated:", selectedUser.value);
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Success!", "User Has Been Edited.", "success");
      router.go(-1); // Navigate back to the user list page after the update
    } else {
      console.error("User ID is undefined");
    }
  } catch (error) {
    console.error("Error updating user:", error);
  }
};

const confirmBack = async () => {
  const confirmMessage =
    "Are you sure you want to go back? Any unsaved changes will be lost.";
  const confirmed = await Swal.fire({
    title: "Sure To Go Back?",
    text: confirmMessage,
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Yes",
    cancelButtonText: "No",
  });

  if (confirmed.isConfirmed) {
    router.go(-1); // Navigate back one step
  }
};

onMounted(() => {
  userId.value = router.currentRoute.value.params.id;

  if (userId.value) {
    console.log("Fetching user with ID:", userId.value);
    fetchUser(userId.value);
  } else {
    console.error("User ID is undefined");
  }
});
</script>

<style scoped>
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column;
  position: relative; /* Menetapkan posisi relatif untuk elemen container */
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
  margin-top: 20px;
  margin-right: 40px;
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}

.card-header {
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
}

.btn-back {
  position: absolute;
  top: 10px;
  right: 10px;
  margin-right: 20px;
}
</style>
