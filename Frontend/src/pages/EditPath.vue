<template>
  <div class="container">
    <h5>
      Edit
      <span>Path</span>
    </h5>
    <button @click="clearCanvas" class="btn btn-light">Hapus Canvas</button>
    <div class="d-flex">
      <button @click="confirmBack" class="btn btn-light">Back</button>
      <button @click="Save" class="btn btn-success">Save</button>
    </div>
    <div class="card bg-light">
      <div class="card-body form-flex">
        <div class="form-group">
          <label for="Start">Start</label>
          <input
            type="text"
            readonly
            class="form-control with-button"
            v-model="newPath.Start"
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
            readonly
            class="form-control with-button"
            v-model="newPath.Goal"
          />
          <div class="input-group-append">
            <button class="material-symbols-outlined" @click="setGoalPoint">
              near_me
            </button>
          </div>
        </div>
        <div class="form-group">
          <label for="Distance">Distance</label>
          <input type="text" readonly class="form-control" v-model="distance" />
        </div>
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

import router from "../router";
const canvas = ref(null);
const ctx = ref(null);
const startPoint = ref(null);
const goalPoint = ref(null);
const distance = ref(0);
const newPath = ref({ Start: "", Goal: "" });

onMounted(() => {
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
