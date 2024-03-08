<template>
  <div class="container">
    <div class="d-flex">
      <h5 class="h5span"><span>Activ</span>ities</h5>
      <div class="dropdown show">
        <a
          class="dropdown-toggle"
          href="#"
          role="button"
          id="dropdownMenuLink"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        >
        </a>

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
              <div class="filter text-white" @click="toggleFilter()">
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
                <!-- Tampilkan gambar saat tidak ada aktivitas -->
                <img
                  v-if="!selectedRobot"
                  src="../image/robotsearch.png.png"
                  alt=""
                  class="no-activities-img"
                />
                <!-- Tampilkan pesan saat belum memilih robot -->
                <div v-if="!selectedRobot" class="no-activities-text">
                  <p>Please select a robot to view activities.</p>
                </div>
                <!-- Tampilkan pesan jika tidak ada aktivitas -->
                <div
                  v-else-if="activities.length === 0"
                  class="no-activities-text"
                >
                  <p>No activities available for this robot.</p>
                </div>
                <!-- Tampilkan aktivitas jika ada -->
                <div v-else>
                  <div
                    v-for="activity in activities"
                    :key="activity.id"
                    class="activity-item"
                  >
                    <!-- Deskripsi aktivitas -->
                    <div class="activity-description text-white">
                      <p class="d-flex">
                        <span class="bullet">
                          <p class="activity-time">
                            {{ formatTime(activity.date) }}
                          </p> </span
                        >{{ activity.activitiy }}
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
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

// Definisikan variabel reaktif untuk menyimpan daftar robot dan aktivitas
const robotOptions = ref([]);
const activities = ref([]); // Tambahkan definisi untuk variabel activities
const currentError = ref(""); // Tambahkan definisi untuk variabel currentError

const selectedRobot = ref(null);

// Fungsi untuk mengambil daftar nama robot dari backend
async function fetchRobots() {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
}

// Fungsi untuk mengambil aktivitas dari backend berdasarkan nama robot yang dipilih
const fetchActivities = (robotName) => {
  return axios
    .get(`http://localhost:5258/activities/robot/${robotName}`)
    .then((response) => {
      activities.value = response.data;
      currentError.value = ""; // Bersihkan pesan error jika berhasil
    })
    .catch((error) => {
      console.error("Error fetching activities:", error);
      currentError.value = "No activities found for the specified robot."; // Set pesan error
    });
};

// Fungsi yang dipanggil saat robot dipilih
function selectRobot(robot) {
  selectedRobot.value = robot;
  fetchActivities(robot); // Panggil fungsi fetchActivities dengan nama robot yang dipilih
}

// Fungsi yang dipanggil saat komponen dimuat
onMounted(() => {
  // Panggil fungsi fetchRobots saat komponen dimuat untuk mengambil daftar nama robot
  fetchRobots();
});

// Fungsi untuk memformat waktu
function formatTime(dateString) {
  const date = new Date(dateString);
  return `${date.getHours()}:${date.getMinutes()}`;
}
</script>

<style scoped>
.activity-description {
  display: flex;
  margin-top: 20px;
  margin-left: 10px;
}
.dropdown {
  margin-top: 23px;
  margin-left: 20px;
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
</style>
