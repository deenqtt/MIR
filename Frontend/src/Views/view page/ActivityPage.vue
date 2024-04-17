<template>
  <div class="container">
    <div class="d-">
      <h5 class="h5span"><span>Activities</span></h5>
      <div class="dropdown show">
        <a
          class="dropdown-toggle"
          href="#"
          role="button"
          id="dropdownMenuLink"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        ></a>

        <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
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
    <div class="card bg-dark">
      <div class="card-body">
        <div class="justify-content-between">
          <div class="content">
            <div class="d-flex last1 h5 justify-content-between">
              <div class="last text-white">
                <h5>Last Activities</h5>
              </div>
              <div class="today text-white">
                <h5>Today</h5>
              </div>
              <div class="filter text-white" @click="showFilterModal">
                <h5 style="font-size: medium; margin-right: 10px">
                  Filter
                  <i
                    class="material-icons"
                    style="font-size: medium; margin-left: 8px"
                    >calendar_today</i
                  >
                </h5>
              </div>
            </div>
            <div class="last content">
              <div class="activity text-white">
                <!-- Display image when no activities -->
                <img
                  v-if="!selectedRobot"
                  src="../image/robotsearch.png.png"
                  alt=""
                  class="no-activities-img"
                />
                <!-- Display message when no robot is selected -->
                <div v-if="!selectedRobot" class="no-activities-text">
                  <p>Please select a robot to view activities.</p>
                </div>
                <!-- Display message if no activities are available -->
                <div
                  v-else-if="activities.length === 0"
                  class="no-activities-text"
                >
                  <p>No activities available for this robot.</p>
                </div>
                <!-- Display activities if available -->
                <div v-else>
                  <!-- Display message if no activities are available for the selected time range -->
                  <div
                    v-if="activities.length === 0"
                    class="no-activities-text"
                  >
                    <p>No activities at this time.</p>
                  </div>
                  <!-- Display activities if available -->
                  <div v-else>
                    <div
                      v-for="activitie in activities"
                      :key="activitie.id"
                      class="activity-item"
                    >
                      <!-- Activity description -->
                      <div class="activity-description text-white">
                        <p class="d-flex">
                          <span class="bullet">
                            <p class="activity-time">
                              {{ formatTime(activitie.date) }}
                            </p>
                          </span>
                          {{ activitie.activity }}
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal untuk form filter -->
    <div class="modal" tabindex="-1" role="dialog" :class="{ show: showModal }">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Filter Activities</h5>
            <button
              type="button"
              class="close"
              aria-label="Close"
              @click="hideFilterModal"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <!-- Form untuk rentang waktu -->
            <form @submit.prevent="applyFilter">
              <div class="mb-3">
                <label for="startTime" class="form-label">Start Time:</label>
                <input
                  type="datetime-local"
                  class="form-control"
                  id="startTime"
                  v-model="startTime"
                />
              </div>
              <div class="mb-3">
                <label for="endTime" class="form-label">End Time:</label>
                <input
                  type="datetime-local"
                  class="form-control"
                  id="endTime"
                  v-model="endTime"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              @click="hideFilterModal"
            >
              Close
            </button>
            <button type="button" class="btn btn-primary" @click="applyFilter">
              Apply
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const robotOptions = ref([]);
const activities = ref([]);
const selectedRobot = ref(null);

// State untuk modal
const showModal = ref(false);

// State untuk rentang waktu
const startTime = ref("");
const endTime = ref("");

// Fungsi untuk menampilkan modal
const showFilterModal = () => {
  showModal.value = true;
};

// Fungsi untuk menyembunyikan modal
const hideFilterModal = () => {
  showModal.value = false;
};

// Fungsi untuk menerapkan filter berdasarkan rentang waktu
const applyFilter = () => {
  // Lakukan sesuatu dengan rentang waktu yang dipilih, misalnya memanggil fungsi fetchActivities dengan parameter waktu tertentu
  console.log("Start Time:", startTime.value);
  console.log("End Time:", endTime.value);
  // Setelah memproses filter, sembunyikan modal
  showModal.value = false;
};
async function fetchRobots() {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
}

const fetchActivities = (robotName) => {
  let url = `http://localhost:5258/activities/robot/${robotName}`;

  if (startTime.value && endTime.value) {
    url += `?startTime=${startTime.value}&endTime=${endTime.value}`;
  }

  return axios
    .get(url)
    .then((response) => {
      activities.value = response.data;
    })
    .catch((error) => {
      console.error("Error fetching activities:", error);
      activities.value = [];
    });
};

function selectRobot(robot) {
  selectedRobot.value = robot;
  fetchActivities(robot);
}

onMounted(() => {
  fetchRobots();
});
</script>

<style scoped>
.d-{
  display: flex;
}

.filter {
  cursor: pointer;
}
.activity-description {
  display: flex;
  margin-top: 20px;
  margin-left: 10px;
}
.dropdown {
  margin-top: 23px;
  margin-left: 20px;
  

}
.dropdown-toggle{
  color: #0c0c0c;
}
.h5span {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
  border: none;
  margin-bottom: 20px;
}

.h5span span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-left: -7px;
}
.last h5 {
  font-size: 15px;
}
.container {
  flex-direction: column;
  font-family: "Poppins", sans-serif;
}

.card {
  border-radius: 10px;
  border: none;
  background-color: #343a40;
  box-shadow: 1px 1px 1px #000000; /* Background color */
}
.material-symbols-outlined {
  margin-top: -4px;
  margin-left: 8px;
}

.card-body {
  display: flex;
  flex-direction: column; /* Mengatur tata letak vertikal */
}

/* Style untuk icon filter */
.material-icons {
  vertical-align: middle;
}

/* Style untuk dropdown filter */
.filter-dropdown {
  position: absolute;
  top: 30px;
  right: 0;
  background-color: #343a40; /* Background color dropdown */
  padding: 10px;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: none;
  z-index: 1;
}

/* Style untuk teks Today */
.default-content {
  display: flex;
  flex-direction: column;
  align-items: center;
}
.last1 {
  border-bottom: #000000 2px solid;
  justify-content: space-between;
}
.today {
  position: absolute;
  top: 0%;
  left: 47.5%;
  margin-top: 35px;
  background: #343a40;
}

/* Styling untuk gambar saat tidak ada aktivitas */
.no-activities-img {
  margin: auto;
  display: block;
  margin-top: 50px; /* Menengahkan gambar */
}

/* Styling untuk teks saat tidak ada aktivitas */
.no-activities-text {
  text-align: center;
  margin-top: 20px; /* Memberikan jarak ke atas */
  color: white;
}

/* Styling untuk waktu aktivitas */
.activity-time {
  color: rgba(255, 255, 255, 0.7);

  margin-left: 30px;
  margin-right: 50px;
  width: 200px;
}

/* Styling untuk bullet */
.bullet {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background-color: white;
  margin-right: 80px;
  margin-top: 2px; /* Memberikan jarak ke kanan */
}

.modal {
  display: none; /* Sembunyikan modal secara default */
  background: rgba(0, 0, 0, 0.5); /* Overlay transparan */
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  z-index: 1050; /* Lebih tinggi dari z-index untuk konten */
  overflow: hidden auto;
}

/* Style untuk memunculkan modal */
.modal.show {
  display: block;
}

/* Style untuk konten modal */
.modal-content {
  position: relative;
  display: flex;
  flex-direction: column;
  background-color: #fff;
  border: 1px solid rgba(0, 0, 0, 0.2);
  border-radius: 0.3rem;
  outline: 0;
}

/* Style untuk header modal */
.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem;
  border-bottom: 1px solid #dee2e6;
  border-top-left-radius: calc(0.3rem - 1px);
  border-top-right-radius: calc(0.3rem - 1px);
}

/* Style untuk body modal */
.modal-body {
  position: relative;
  flex: 1 1 auto;
  padding: 1rem;
  overflow: auto;
}

/* Style untuk footer modal */
.modal-footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 1rem;
  border-top: 1px solid #dee2e6;
  border-bottom-right-radius: calc(0.3rem - 1px);
  border-bottom-left-radius: calc(0.3rem - 1px);
}

/* Style untuk menutup modal */
.close {
  padding: 1rem;
  margin: -1rem -1rem -1rem auto;
  background: transparent;
  border: 0;
}
</style>
