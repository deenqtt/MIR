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
                    class="material-symbols-outlined"
                    @click="detailPath(user)"
                  >
                    visibility
                  </button>
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editPath(user)"
                  >
                    edit
                  </button>

                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deleteUser(user)"
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

const users = ref([]);
const router = useRouter(); // Initialize the router

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/users";

const fetchUser = async () => {
  try {
    const response = await axios.get(apiUrl);
    users.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const deleteUser = async (user) => {
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

const editPath = (user) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-user", // replace 'edit-map' with the name of your edit route
    params: { userId: user.id }, // adjust this based on the structure of your route
  });
};

// Call fetchMaps on component mount
onMounted(() => {
  fetchUser();
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
  color: #000000;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #1920ff;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 100; /* Memberikan jarak atas antara h5 dan p */
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
