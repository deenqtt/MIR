<template>
  <div class="container">
    <div class="centered-content" v-if="!errors.length && !errorOccurred">
      <img src="../image/robotsearch.png.png" alt="Robot" class="" />
      <p class="">
        Nothing error for {{ selectedRobot ? selectedRobot : "this robot" }},
        all good.
      </p>
    </div>
    <div v-else>
      <h5>Error <span>Log</span></h5>
      <p class="p">Monitoring Error Robot</p>
      <br />
      <div class="error">
        <label for="robotDropdown">Error List</label>
        <div class="dropdown">
          <button
            class="btn dropdown-toggle"
            type="button"
            id="robotDropdown"
            href="#"
            role="button"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
            @click="toggleDropdown"
          >
            {{ selectedRobot ? selectedRobot : "" }}
          </button>
          <div
            class="dropdown-menu robot-dropdown-menu"
            aria-labelledby="robotDropdown"
            :class="{ show: isDropdownOpen }"
          >
            <a
              class="dropdown-item"
              v-for="robot in robotOptions"
              :key="robot"
              @click="selectRobot(robot)"
            >
              {{ robot }}
            </a>
          </div>
        </div>
      </div>
      <div class="card bg-light">
        <div class="card-body">
          <div v-if="!filteredErrors.length" class="image-message-container">
            <img
              src="../image/robotsearch.png.png"
              alt="Robot"
              class="center-image error-image"
            />
            <p class="message below-image-message">
              Nothing error for
              {{ selectedRobot ? selectedRobot : "this robot" }}, all good.
            </p>
          </div>
          <table class="table table-hover" v-else>
            <thead class="thead-dark">
              <tr>
                <th>Robot Name</th>
                <th>Error Description</th>
                <th class="text-end">Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(error, index) in filteredErrors" :key="index">
                <td>{{ error.robotname }}</td>
                <td>{{ error.explained }}</td>
                <td class="justify-content-end text-end">
                  <button @click="markAsDone(index)" class="btn btn-success">
                    Done
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, onBeforeMount } from "vue";
import axios from "axios";
import Swal from "sweetalert2";

const errors = ref([]);
const errorOccurred = ref(false);
const robotOptions = ref([]);
const selectedRobot = ref(null);
const apiUrl = "http://localhost:5258/errors";

const fetchErrors = async () => {
  try {
    const response = await axios.get(apiUrl);
    errors.value = response.data;
    errorOccurred.value = errors.value.length > 0;
  } catch (error) {
    console.error("Error fetching errors:", error);
  }
};

const markAsDone = async (index) => {
  try {
    const confirmed = await Swal.fire({
      title: "Are you sure?",
      text: "Do you want to mark this error as done?",
      icon: "question",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, mark it as done",
    });

    if (confirmed.isConfirmed) {
      // Kirim permintaan ke server untuk menandai error sebagai selesai
      const errorId = errors.value[index].id; // Anggap error memiliki properti id
      await axios.delete(`${apiUrl}/${errorId}`);

      // Hapus error dari daftar
      errors.value.splice(index, 1);
      // Perbarui nilai errorOccurred jika tidak ada error lagi
      errorOccurred.value = errors.value.length > 0;

      Swal.fire(
        "Marked as Done!",
        "The error has been marked as done.",
        "success"
      );
    }
  } catch (error) {
    console.error("Error marking error as done:", error);
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

const selectRobot = (robot) => {
  selectedRobot.value = robot;
};

const filteredErrors = computed(() => {
  if (!selectedRobot.value) return errors.value;
  return errors.value.filter(
    (error) => error.robotname === selectedRobot.value
  );
});

onBeforeMount(() => {
  fetchErrors();
  fetchRobots();
});
</script>

<style scoped>
.image-message-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  margin-bottom: 20px; /* Ganti sesuai kebutuhan */
}

.below-image-message {
  margin-top: 10px; /* Jarak antara gambar dan pesan */
}
.card {
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}
.container {
  font-family: "Poppins", sans-serif;
}
.error {
  display: flex;
}
.error .dropdown {
  margin-top: -5px;
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
.p {
  font-family: "Poppins", sans-serif;
  font-weight: 400;
  color: #a1a1a1;
  margin-top: -5px;
  display: inline-block;
  position: relative;
}

.p::after {
  content: "";
  position: absolute;
  left: 0;
  bottom: -2px; /* Sesuaikan dengan ketebalan garis */
  width: 100%;
  border-bottom: solid 1px #000;
}

.centered-content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  max-width: 100%;
  max-height: 100%;
  margin-top: 100px;
}

.centered-content img {
  filter: drop-shadow(2px 4px 6px rgba(0, 0, 0, 0.5));
}

.message {
  font-family: "Poppins", sans-serif;
  text-align: center;
  margin-top: 20px;
}

.error-message {
  font-family: "Poppins", sans-serif;
  text-align: center;
  margin-top: 20px;
}
.table {
  margin-top: 20px;
}
.table {
  margin-top: 20px;
  background-color: rgba(255, 255, 255, 0.5); /* Ubah nilai opasitas di sini (0.5 adalah 50%) */
}

.btn-success {
  background-color: #28a745;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-success:hover {
  background-color: #218838;
}

/* Style untuk gambar */
.error-image {
  width: 400px; /* Mengurangi ukuran gambar */
  height: 300px;
  border-radius: 20px;
}
</style>
