<template>
  <div class="container">
    <h5>
      Robot
      <span>Management</span>
    </h5>

    <div class="card bg-light">
      <div class="card-header">List Of Robot</div>
      <div class="card-body">
        <table class="table">
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
          <tbody v-if="robots.length > 0">
            <tr v-for="robot in robots" :key="robot.id">
              <td>{{ robot.id }}</td>
              <td>{{ robot.name }}</td>
              <td>{{ robot.serialnumber }}</td>
              <td>{{ robot.ip }}</td>
              <td>{{ robot.domainId }}</td>
              <td>
                <button
                  class="btn btn-primary btn-sm"
                  @click="editRobot(robot)"
                >
                  Edit
                </button>
                <br />
                <button
                  class="btn btn-danger btn-sm"
                  @click="deleteRobot(robot)"
                >
                  Delete
                </button>
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

const robots = ref([]);

const errorMessage = ref("");
const apiUrl = "http://localhost:5258/robots";

const fetchRobots = async () => {
  try {
    const response = await axios.get(apiUrl);
    robots.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch robots: " + error.message;
  }
};
const deleteRobot = async (robot) => {
  const confirmDelete = window.confirm(
    "Are you sure you want to delete this robot?"
  );
  if (confirmDelete) {
    try {
      await axios.delete(`${apiUrl}/${robot.id}`);
      fetchRobots();
      window.alert("Robot successfully deleted!");
    } catch (error) {
      errorMessage.value = "Failed to delete robot: " + error.message;
    }
  }
};

onMounted(() => {
  console.log("Component mounted");
  fetchRobots();
});
</script>

<style scoped>
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
