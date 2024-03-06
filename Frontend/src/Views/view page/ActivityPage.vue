<template>
  <div class="container">
    <h5 class="h5span"><span>Activ</span>ities</h5>
    <div class="card bg-dark">
      <div class="card-body">
        <div class="d-flex justify-content-between">
          <div class="list">
            <div class="d-flex align-items-center">
              <h5 class="list-robot text-white" style="font-size: 15px">
                List Robot
              </h5>
              <span class="material-symbols-outlined text-white">
                arrow_drop_down
              </span>
            </div>
            <div class="ul text-white">
              <li
                v-for="robot in robotOptions"
                :key="robot"
                @click="changeActivity(robot)"
              >
                {{ robot }}
              </li>
            </div>
          </div>

          <div class="content">
            <div class="d-flex h5 justify-content-between">
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
                <div class="filter-dropdown" v-show="showFilter">
                  <!-- Dropdown content here -->
                  <datepicker @select="selectDate"></datepicker>
                </div>
              </div>
            </div>
            <div class="last content">
              <div class="activity text-white">
                <div class="activity-item">
                  <div class="bullet" v-if="currentActivity"></div>
                  <div class="activity-description text-white">
                    <p v-if="currentActivity">{{ currentActivity }}</p>
                    <!-- Tampilkan gambar dan pesan default saat tidak ada aktivitas yang dipilih -->
                    <div v-else class="default-content">
                      <img
                        src="../image/robotsearch.png.png"
                        alt="Robot Image"
                        style="width: 150px; margin-top: 10px"
                      />
                      <p>Please select a robot to view its activities.</p>
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
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import { useStore } from "vuex";
const store = useStore();
const showFilter = ref(false);

const robotOptions = ref([]);

// Fetch robots function
const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};
const currentActivity = computed(() => store.state.errorMessage); // Mengambil nilai pesan error dari store
function toggleFilter() {
  showFilter.value = !showFilter.value;
}

function selectDate(date) {
  // Implement your logic when date is selected
}

function changeActivity(robot) {
  // Memperbarui pesan error berdasarkan robot yang dipilih
  const message = `Message for ${robot} - This is the message to be sent to the selected robot.`;
  currentActivity.value = message;
  window.location.reload();
  // Simpan pesan error ke local storage
  localStorage.setItem("errorMessage", message);
}
onMounted(() => {
  fetchRobots();
  // Cek apakah ada pesan yang tersimpan di local storage
  const savedMessage = localStorage.getItem("errorMessage");
  if (savedMessage) {
    // Set nilai pesan error dari local storage ke state Vuex
    store.commit("setErrorMessage", savedMessage);
  }
});
</script>

<style scoped>
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
  margin-right: 600px;
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
  box-shadow: 1px 1px 1px #000000;
  height: max-content; /* Background color */
}
.material-symbols-outlined {
  margin-top: -4px;
  margin-left: 8px;
}
.ul {
  list-style: none;
  padding: 8px;
  margin: 0px 0px 10px;
}
.ul li {
  margin: 8px;
  opacity: 30%;
  cursor: pointer;
  margin-left: -10px;
}
li:hover {
  opacity: 100%;
}
.list {
  border-right: 2px solid #838383; /* Menambahkan garis di samping kanan */
  margin-right: 10px; /* Mengatur jarak antara teks dan garis */
  padding-right: 50px; /* Memberikan ruang antara list dan border */
  flex: 1; /* Memberikan fleksibilitas agar list dapat berubah ukuran sesuai kebutuhan */
}
.card-body {
  display: flex;
  flex-direction: column; /* Mengatur tata letak vertikal */
}
.last {
  flex-direction: column;
  margin-top: 10px; /* Menambahkan jarak antara last activities dan content */
}

.filter {
  margin-top: 10px; /* Menambahkan jarak antara last activities dan filter */
  position: relative;
}
.h5 {
  border-bottom: 2px solid #dddbdb;
}
.activity {
  margin-top: 20px;
}
.activity-item {
  display: flex;
  align-items: center;
  padding-left: 20px; /* Menambahkan padding ke kiri untuk memberikan spasi ke kanan pada aktivitas pertama */
  position: relative; /* Menetapkan posisi relatif untuk mengatur garis */
}
.bullet {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background-color: #fff; /* Menjadikan warna bulet putih */
  margin-right: 20px;
}
.activity-description p {
  margin: 0;
}
.activity-item:not(:last-child) {
  margin-bottom: 20px;
}

/* Menambahkan garis di antara bulet-bulet */
.activity-item::after {
  content: "";
  position: absolute;
  top: 50%;
  left: 10px; /* Sesuaikan dengan posisi bulet */
  height: calc(100% + 20px); /* Sesuaikan dengan tinggi div parent */
  width: 2px;
  background-color: #fff; /* Warna garis */
  z-index: -1; /* Menetapkan lapisan z di bawah bulet */
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

.filter:hover .filter-dropdown {
  display: block;
}

/* Style untuk teks Today */
.today {
  font-family: "Poppins", sans-serif;
  font-weight: 100;
  position: absolute;
  top: 23%; /* Tetapkan posisi atas */
  justify-content: center;
  align-items: center;
  display: flex;
  width: 100px;
  left: 59%;
  background: #343a40;
  transform: translateX(-50%);
}

.default-content {
  display: flex;
  flex-direction: column;
  margin-left: 200px;
  align-items: center;
}
</style>
