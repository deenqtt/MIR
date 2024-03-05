<template>
  <div class="canvass">
    <div class="d-flex">
      <button class="btn btn-outline-primary" @click="clearCanvas">
        Clear Canvas
      </button>
      <button class="btn btn-outline-success" @click="submitForm">
        Save Image
      </button>
      <label class="btn btn-outline-info">
        Select Drawing Mode
        <select v-model="drawingMode">
          <option value="line">Line</option>
          <option value="rectangle">Rectangle</option>
          <option value="circle">Circle</option>
        </select>
      </label>
      <label>
        Line Thickness:
        <input type="number" v-model="lineThickness" min="1" />
      </label>
      <router-link to="/footprint" class="btn btn-light btn-sm"
        >Back</router-link
      >
    </div>
    <br />
    <div class="card bg-light">
      <div class="card-body form-flex">
        <div class="form-group">
          <label for="Name">Name</label>
          <input
            type="text"
            class="form-control form-control-sm"
            placeholder="Input Your Path Name"
            v-model="newFoot.Name"
          />
        </div>
        <div class="form-group">
          <label for="Select">Select Robot</label>
          <select
            class="form-control form-control-sm"
            v-model="newFoot.Robotname"
          >
            <option value="" disabled selected>Select Robot</option>
            <option v-for="name in robotOptions" :key="name" :value="name">
              {{ name }}
            </option>
          </select>
        </div>
      </div>
    </div>
    <br />
    <div class="card bg-light">
      <div class="card-body">
        <canvas
          ref="canvasRef"
          style="width: 100%"
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
import { ref, onMounted, onBeforeUnmount } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";

const apiUrl = "http://localhost:5258/footprints";
let canvas;
let ctx;
let drawing = false;
let lines = ref([]);
let currentLine = [];
let xAxisLength = ref(0);
let yAxisLength = ref(0);
let drawingMode = ref("line");
let lineThickness = ref(5); // Default line thickness
const newFoot = ref({
  Name: "",
  Robotname: "",
});

const router = useRouter();

const robotOptions = ref([]);
const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};
const submitForm = async () => {
  if (validateForm()) {
    const dataURL = canvas.toDataURL("image/png");
    const imageBlob = await fetch(dataURL).then((res) => res.blob());

    const formData = new FormData();
    formData.append("file", imageBlob, "drawn_image.png");
    formData.append("name", newFoot.value.Name);
    formData.append("robotname", newFoot.value.Robotname);

    // Log request headers
    console.log("Request Headers:", Object.fromEntries(formData.entries()));

    try {
      const response = await axios.post(apiUrl, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      console.log("Image uploaded successfully!");
      router.push("/footprint"); // Redirect after successful submission
    } catch (error) {
      console.error("Error uploading image:", error);
    }
  }
};

const validateForm = () => {
  if (
    !newFoot.value.Name ||
    !newFoot.value.Robotname ||
    lines.value.length === 0
  ) {
    console.error("Please fill in all required fields and draw on the canvas.");
    return false;
  }
  return true;
};

const startDrawing = (event) => {
  drawing = true;
  const rect = canvas.getBoundingClientRect();
  const x = (event.clientX - rect.left) * (canvas.width / rect.width);
  const y = (event.clientY - rect.top) * (canvas.height / rect.height);
  currentLine = [{ x, y, type: drawingMode.value }];
};

const drawLines = () => {
  ctx.lineWidth = lineThickness;
  ctx.lineCap = "round";
  ctx.strokeStyle = "black";

  lines.value.forEach((line) => {
    if (line.length >= 2) {
      ctx.beginPath();
      ctx.moveTo(line[0].x, line[0].y);

      for (let i = 1; i < line.length; i++) {
        ctx.lineTo(line[i].x, line[i].y);
      }

      ctx.stroke();
    }
  });
};

const draw = (event) => {
  if (!drawing) return;

  const rect = canvas.getBoundingClientRect();
  const x = (event.clientX - rect.left) * (canvas.width / rect.width);
  const y = (event.clientY - rect.top) * (canvas.height / rect.height);

  const start = currentLine[currentLine.length - 1];

  switch (drawingMode.value) {
    case "line":
      // Draw straight lines
      currentLine.push({ x, y, type: drawingMode.value });
      break;
    case "rectangle":
      // Draw rectangles
      currentLine = [
        { x: start.x, y: start.y, type: drawingMode.value },
        { x, y: start.y, type: drawingMode.value },
        { x, y, type: drawingMode.value },
        { x: start.x, y, type: drawingMode.value },
        { x: start.x, y: start.y, type: drawingMode.value },
      ];
      break;
    case "circle":
      // Draw circles
      const radius = Math.sqrt((x - start.x) ** 2 + (y - start.y) ** 2);
      const circumference = 2 * Math.PI * radius;
      const points = 100;
      const scalingFactor = 0.5; // Adjust this factor as needed

      currentLine = Array.from({ length: points + 1 }, (_, index) => {
        const angle = (index / points) * 2 * Math.PI;
        return {
          x: start.x + scalingFactor * radius * Math.cos(angle),
          y: start.y + scalingFactor * radius * Math.sin(angle),
          type: drawingMode.value,
        };
      });
      break;
  }

  ctx.clearRect(0, 0, canvas.width, canvas.height);
  drawLines();
};
const stopDrawing = () => {
  if (drawing) {
    drawing = false;
    lines.value.push(currentLine);
    updateAxisLength();
    drawAxes();
    drawLines();
  }
};

const clearCanvas = () => {
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  lines.value = [];
  xAxisLength.value = 0;
  yAxisLength.value = 0;
};
const drawAxes = () => {
  // Comment out or remove the following lines to eliminate X and Y axis lines
  // ctx.lineWidth = 1;
  // ctx.strokeStyle = "#ccc";
  //
  // // Draw X-axis
  // ctx.beginPath();
  // ctx.moveTo(0, canvas.height / 2);
  // ctx.lineTo(canvas.width, canvas.height / 2);
  // ctx.stroke();
  //
  // // Draw Y-axis
  // ctx.beginPath();
  // ctx.moveTo(canvas.width / 2, 0);
  // ctx.lineTo(canvas.width / 2, canvas.height);
  // ctx.stroke();
  // Draw measurement numbers on X-axis
};

const updateAxisLength = () => {
  const lastLine = lines.value[lines.value.length - 1];
  if (lastLine) {
    const deltaX = Math.abs(lastLine[lastLine.length - 1].x - lastLine[0].x);
    const deltaY = Math.abs(lastLine[lastLine.length - 1].y - lastLine[0].y);

    xAxisLength.value = deltaX;
    yAxisLength.value = deltaY;
  }
};
const createShape = (event) => {
  const rect = canvas.getBoundingClientRect();
  const x = (event.clientX - rect.left) * (canvas.width / rect.width);
  const y = (event.clientY - rect.top) * (canvas.height / rect.height);

  // Determine the shape type based on a condition (e.g., even or odd click)
  const shapeType = event.detail % 2 === 0 ? "square" : "circle";

  // Create a shape based on the shape type
  if (shapeType === "square") {
    const size = 50; // Set the size of the square
    lines.value.push([
      { x: x - size / 2, y: y - size / 2 },
      { x: x + size / 2, y: y - size / 2 },
      { x: x + size / 2, y: y + size / 2 },
      { x: x - size / 2, y: y + size / 2 },
      { x: x - size / 2, y: y - size / 2 },
    ]);
  } else if (shapeType === "circle") {
    const radius = 25; // Set the radius of the circle
    lines.value.push([
      { x, y: y - radius },
      { x: x + radius, y },
      { x, y: y + radius },
      { x: x - radius, y },
      { x, y: y - radius },
    ]);
  }

  // Update and redraw the canvas
  updateAxisLength();
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  drawLines();
};
onMounted(() => {
  canvas = document.querySelector("canvas");
  ctx = canvas.getContext("2d");
  drawAxes();
  fetchRobots();
});

onBeforeUnmount(() => {
  canvas.removeEventListener("mousedown", startDrawing);
  canvas.removeEventListener("mouseup", stopDrawing);
  canvas.removeEventListener("mousemove", draw);
});
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

.form-flex {
  display: flex;
  justify-content: space-between;
}

.form-group {
  flex: 0 0 48%; /* Adjust width as needed */
}
</style>
