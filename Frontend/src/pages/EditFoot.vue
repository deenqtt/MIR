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
const canvasRef = ref(null);
// Inisialisasi
const xAxisLength = ref(0);
const yAxisLength = ref(0);

const context = canvas.getContext("2d");
const canvas = canvasRef.value;

if (canvas) {
  const context = canvas.getContext("2d");
  // Lakukan sesuatu dengan context
} else {
  console.error("Canvas element is null or undefined.");
}

onMounted(() => {
  loadImage();
});

const loadImage = async () => {
  try {
    // Mendapatkan ID gambar dari parameter rute
    const imageId = this.$route.params.imageId;

    const response = await axios.get(`${apiUrl}/${imageId}/image`, {
      responseType: "arraybuffer",
    });

    const canvas = canvasRef.value;
    const context = canvas.getContext("2d");
    const img = new Image();
    img.onload = () => {
      context.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
    const blob = new Blob([response.data], { type: "image/png" });
    img.src = URL.createObjectURL(blob);
  } catch (error) {
    console.error("Error loading image:", error);
    displayNoImageMessage();
  }
};

const displayNoImageMessage = () => {
  const canvas = canvasRef.value;
  const context = canvas.getContext("2d");
  context.font = "20px Arial";
  context.fillText("Nothing designed here, please create", 10, 50);
};
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
