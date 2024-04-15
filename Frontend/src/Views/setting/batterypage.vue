<template>
  <div class="container">
    <div class="h5 d-flex">
      <h5>Battery <span>Info </span></h5>

      <!-- Dropdown -->
      <div class="dropdown">
        <button
          class="btn dropdown-toggle"
          type="button"
          id="dropdownMenuButton"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        ></button>
        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
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

    <div v-if="!selectedRobot">
      <div class="image-container">
        <img src="../image/robotsearch.png.png" alt="Robot Image" />
        <p>Please Select Robot To See Detail Informations</p>
      </div>

      <!-- Button back -->
      <button @click="goBack" class="btn btn-primary">Back</button>
    </div>
    <!-- Container untuk menampilkan informasi baterai -->
    <div v-if="selectedRobot">
      <div class="d-flex d1 ">
      <div class="card">
        <div class="card-body">
      <h1 class="txt">Dynamic Percentage:</h1>
      <h2 class="txt">{{ percentage }}%</h2></div>
    </div>
  </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { batteryPercentage } from "../../router/mqtt"; // Import fungsi dan variabel dari mqtt.js
const robotOptions = ref([]);
const router = useRouter();
const selectedRobot = ref(null);
const percentage = ref(0);
async function fetchRobots() {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
}
const selectRobot = (robot) => {
  selectedRobot.value = robot; // Menetapkan nama robot yang dipilih
};
const goBack = () => {
  router.go(-1); // Kembali ke halaman sebelumnya
};

const fetchPercentage = async () => {
  try {
    const response = await fetch('http://localhost:5000/get_percentage');
    const data = await response.json();
    percentage.value = data.percentage;
  } catch (error) {
    console.error('Error fetching percentage:', error);
  }
};
onMounted(() => {
  fetchPercentage();
  setInterval(fetchPercentage, 5000); // Polling setiap 5 detik
  fetchRobots();
});
</script>

<style scoped>
.txt{
  font-size: 1rem;
}

.d1 .card{
  width: 100%; 
  border-radius: 7px;
  align-items: center;
  justify-content: center;
}
.d-flex {
  display: flex;
  justify-content: space-between;
}
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-family: "Poppins", sans-serif;
}

.h5 {
  display: flex;
  flex-direction: row;
  margin-left: -900px;
  padding: 20px;
}

.dropdown {
  margin-top: -5px;
}

.image-container {
  text-align: center;
  margin-top: 20px;
}

.image-container img {
  max-width: 100%;
}

.image-container p {
  margin-top: 10px;
  font-size: 18px;
  color: #333;
}

.battery-info {
  display: flex;
  justify-content: center; /* Mengatur posisi tengah */
  align-items: center; /* Mengatur posisi tengah */
  margin-top: 20px; /* Atur margin atas */
  width: 100%; /* Lebar penuh */
  max-width: 600px; /* Lebar maksimum */
}
h5 {
  color: #007bff; /* Warna biru */
  font-weight: 700;
}
.card {
  width: 100%;
  margin-top: -30px;
  border-radius: 30px;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1); /* Tambahkan box shadow */
}
.card1 {
  margin-top: 30px;
  border-radius: 10px;
}
.card-header {
  text-align: center;
  border-radius: 30px 30px 0px 0px;
}

.card-footer {
  margin-top: 20px; /* Atur margin atas */
}

.battery-container {
  display: flex;
  flex-direction: column;
  align-items: center; /* Mengatur posisi tengah */
}
.material-icons {
  font-size: 3rem;
  rotate: 90deg;
}
.battery-percentage {
  font-size: 2rem; /* Ubah ukuran teks persentase */
}
</style>
