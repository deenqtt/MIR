<template>
  <div class="container">
    <div class="d-flex">
      <button class="btn btn-outline-primary" @click="clearCanvas">
        Clear Canvas
      </button>
      <button class="btn btn-outline-success" @click="saveImage">
        Save Image
      </button>
    </div>
    <br />
    <div class="card bg-light">
      <div class="card-body">
        <canvas
          style="width: 100%"
          ref="canvas"
          @mousedown="startDrawing"
          @mousemove="draw"
          @mouseup="stopDrawing"
        ></canvas>
      </div>

      <div class="card-footer">
        <div class="d-flex">
          <p>
            X-Axis:
            <input type="text" readonly :value="xAxisLength.toFixed(2)" />
          </p>
          <p>
            Y-Axis:
            <input type="text" readonly :value="yAxisLength.toFixed(2)" />
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const apiUrl = "http://localhost:5258/footprints";
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
}
canvas {
  border: 1px solid #000;
  margin-bottom: 10px;
  cursor: crosshair;
}
.btn {
  width: auto;
}
</style>
