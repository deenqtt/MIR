<template>
  <div class="container">
    <h5>
      Edit
      <span>Path</span>
    </h5>
    <button @click="clearCanvas" class="btn btn-light">Hapus Canvas</button>
    <div class="d-flex">
      <button @click="confirmBack" class="btn btn-light">Back</button>
      <button @click="updatePath" class="btn btn-success">Save</button>
    </div>
    <div class="card bg-light">
      <div class="card-body form-flex">
        <form @submit.prevent="updatePath">
          <div class="form-group">
            <label for="Start">Start</label>
            <input
              type="text"
              class="form-control"
              v-model="selectedPath.start"
              required
            />
            <div class="input-group-append">
              <button class="material-symbols-outlined" @click="setStartPoint">
                near_me
              </button>
            </div>
          </div>
          <div class="form-group">
            <label for="Goal">Goal</label>
            <input
              type="text"
              class="form-control"
              v-model="selectedPath.goal"
              required
            />
            <div class="input-group-append">
              <button class="material-symbols-outlined" @click="setGoalPoint">
                near_me
              </button>
            </div>
          </div>
          <div class="form-group">
            <label for="Distance">Distance</label>
            <input
              type="text"
              class="form-control"
              v-model="selectedPath.distance"
              required
            />
          </div>
        </form>
      </div>
    </div>

    <br />

    <div class="card bg-light">
      <div class="card-header">
        <p>Select Your Start And Goal</p>
      </div>
      <div class="card-body">
        <canvas
          style="width: 100%; height: 100%"
          ref="canvas"
          @click="addPoint"
        ></canvas>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref, reactive } from "vue";
import { useRouter } from "vue-router"; // Import useRouter
import Swal from "sweetalert2";
const router = useRouter();
const canvas = ref(null);
const ctx = ref(null);
const startPoint = ref(null);
const goalPoint = ref(null);
const distance = ref(0);

const apiUrl = "http://localhost:5258/paths";

const pathId = ref(null); // Accessing route params directly from $route
const selectedPath = ref({
  id: null,
  Start: "",
  Goal: "",
  Distance: "",
});

const fetchPath = async (pathId) => {
  try {
    const response = await axios.get(`${apiUrl}/${pathId}`);
    selectedPath.value = response.data;
  } catch (error) {
    console.error("Failed to fetch user:", error);
  }
};

const updatePath = async () => {
  try {
    if (pathId.value) {
      await axios.put(`${apiUrl}/${pathId.value}`, selectedPath.value);
      console.log("Path updated:", selectedPath.value);
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

onMounted(() => {
  pathId.value = router.currentRoute.value.params.id;

  if (pathId.value) {
    console.log("Fetching path with ID:", pathId.value);
    fetchPath(pathId.value);
  } else {
    console.error("User ID is undefined");
  }
});

onMounted(() => {
  fetchPath();
  ctx.value = canvas.value.getContext("2d");
});

function addPoint(event) {
  const rect = canvas.value.getBoundingClientRect();
  const x = Math.round(event.clientX - rect.left);
  const y = Math.round(event.clientY - rect.top);
  if (startPoint.value === null) {
    startPoint.value = { x, y };
  } else if (goalPoint.value === null) {
    goalPoint.value = { x, y };
    calculateDistance();
  }
  draw();
}

function calculateDistance() {
  const dx = goalPoint.value.x - startPoint.value.x;
  const dy = goalPoint.value.y - startPoint.value.y;
  distance.value = Math.round(Math.sqrt(dx * dx + dy * dy));
}

function draw() {
  ctx.value.clearRect(0, 0, canvas.value.width, canvas.value.height);
  if (startPoint.value) {
    ctx.value.beginPath();
    ctx.value.arc(startPoint.value.x, startPoint.value.y, 5, 0, Math.PI * 2);
    ctx.value.fillStyle = "blue"; // Warna biru untuk titik awal
    ctx.value.fill();
  }
  if (goalPoint.value) {
    ctx.value.beginPath();
    ctx.value.arc(goalPoint.value.x, goalPoint.value.y, 5, 0, Math.PI * 2);
    ctx.value.fillStyle = "red"; // Warna merah untuk titik tujuan
    ctx.value.fill();
  }
  if (startPoint.value && goalPoint.value) {
    ctx.value.beginPath();
    ctx.value.moveTo(startPoint.value.x, startPoint.value.y);
    ctx.value.lineTo(goalPoint.value.x, goalPoint.value.y);
    ctx.value.strokeStyle = "green";
    ctx.value.lineWidth = 2;
    ctx.value.stroke();
  }
}

function setStartPoint() {
  startPoint.value = null;
  goalPoint.value = null;
}

function setGoalPoint() {
  startPoint.value = null;
  goalPoint.value = null;
}

function clearCanvas() {
  ctx.value.clearRect(0, 0, canvas.value.width, canvas.value.height);
  startPoint.value = null;
  goalPoint.value = null;
  distance.value = 0;
}
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
</script>

<style scoped>
.btn-light {
  margin-top: -80px;
}
canvas {
  border: #000 solid 1px;
  height: 100px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column;
}

h5 {
  margin-top: 20px;
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 100;
}

.btn {
  text-align: center;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: 30px;
  align-self: flex-end;
  margin-right: 40px;
  margin-top: -10px;
  margin-bottom: 10px;
  border-radius: 10px;
}

.card {
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}

.card-header {
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header p {
  font-size: 12px;
  font-weight: 500;
  margin-top: -20px;
  margin-bottom: -20px;
}

.card-header button {
  border: none;
}

.form-flex {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.form-group {
  flex: 1;
  margin-right: 10px;
  margin-bottom: 10px;
}

.form-group select {
  border-radius: 10px;
  background-color: #b4b4b4;
}

.input-group {
  position: relative;
  width: 100%;
}

.with-button {
  border-radius: 0.25rem 0 0 0.25rem;
}

.input-group-append {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  z-index: 2;
}

.input-group-append .material-symbols-outlined {
  border-radius: 0 0.25rem 0.25rem 0;
  background-color: #b4b4b4;
  margin-right: 9px;
  border: none;
}
</style>
