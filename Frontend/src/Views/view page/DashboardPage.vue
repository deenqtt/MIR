<template>
  <div class="container">
    <h5>
      Dash
      <span>boards</span>
    </h5>
    <br />
    <div v-if="selectedRobot !== ''">
      <div class="d-flex">
        <div class="card-name">
          <div class="card bg-dark">
            <div class="card-body">
              <img src="../image/ai.png" alt="roboticon" />

              {{ selectedRobot }}
            </div>
          </div>
        </div>
        <div class="informations">
          <div class="d-flex">
            <div class="card bg-light">
              <!-- Add background to the image container -->

              <img src="" alt="" class="colorized-image" />
            </div>
            <div class="card bg-primary-subtle">
              <!-- Add background to the image container -->

              <img src="../image/half.png" alt="" class="colorized-image" />
            </div>
          </div>
        </div>
      </div>
      <br />
      <div class="condition">
        <div class="card bg-dark">
          <div class="card-body">
            <img src="../image/info.png" alt="info" />
            Condition In Mission ?
          </div>
        </div>
      </div>
      <br />
      <br />
      <br />
      <div class="card-maps">
        <div class="card">
          <div class="card-header">
            <button class="fa-solid fa-arrow-left arrow-icons"></button
            ><button class="fa-solid fa-arrow-right"></button>
          </div>
          <div class="card-body">
            <canvas class="canvas" style="width: 100%"></canvas>
          </div>
        </div>
      </div>
    </div>
    <div v-else class="default-content">
      <!-- Show default content when no robot is selected -->
      <div class="robot-search-container">
        <img src="../image/select.png (1).png" alt="Searching for robots..." />
        <p>Please select a robot</p>
      </div>
    </div>
  </div>
</template>
<script setup>
import { useStore } from "vuex";
import { ref, onMounted } from "vue";
const store = useStore();
const selectedRobot = ref("");
onMounted(() => {
  // Watch for changes in the Vuex store and update selectedRobot
  selectedRobot.value = store.state.selectedRobot;
  store.subscribe((mutation) => {
    if (mutation.type === "setSelectedRobot") {
      selectedRobot.value = store.state.selectedRobot;
    }
  });
});
</script>

<style scoped>
.default-content {
  display: flex;
  justify-content: center; /* Untuk mengatur konten secara horizontal di tengah */
  align-items: center; /* Untuk mengatur konten secara vertikal di tengah */
  /* Menjadikan div seluruh tinggi halaman */
}

.robot-search-container {
  text-align: center; /* Untuk mengatur teks dan gambar berada di tengah secara horizontal */
}

.robot-search-container img {
  width: 300px; /* Atur ukuran gambar sesuai kebutuhan */
  margin-bottom: 20px;
  background: none;
  -webkit-filter: drop-shadow(5px 5px 5px #666666);
  filter: drop-shadow(5px 5px 5px #666666); /* Jarak antara gambar dan teks */
}

.canvas {
  border: solid 1px #000000;
}
.arrow-icons {
  margin-right: 20px;
}
.fa-solid {
  border: none;
  background: none;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Menyusun elemen dalam satu kolom */
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
  color: #000000;
  margin-left: -7px;
}
.container .card {
  border-radius: 20px;

  width: 500px;
  color: #fff;
}
img {
  width: 8%;
  margin-right: 20px;
  background-color: #acacac;
  padding: 3px;
  border-radius: 10px;
}
.d-flex {
  gap: 20px;
}
.d-flex .informations .bg-light {
  width: 200px;
  height: 200px;
}
.d-flex .informations .bg-primary-subtle {
  width: 200px;
}
.condition {
  margin-top: -140px;
}
.bg-light {
  margin-right: 20px;
}

.colorized-image {
  width: 50%;
  margin: auto;
  margin-top: -6px;
  filter: grayscale(100%);
  background: none;
  /* Adjust the filter values as needed to achieve the desired color effect */
}
.card-maps .card {
  width: auto;
  color: #000000;
}
.informations {
  display: flex;
  flex-direction: column;
  align-items: flex-end; /* Align to the right */
}
.informations .d-flex {
  gap: 20px;
}
.informations .bg-light {
  width: 200px;
  height: 200px;
}
.informations .bg-primary-subtle {
  width: 200px;
}
</style>
