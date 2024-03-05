<template>
  <div>
    <label for="name">Name:</label>
    <input type="text" id="name" v-model="name" />

    <label for="robot">Select Robot:</label>
    <select v-model="selectedRobot" id="robot">
      <option v-for="(robot, index) in robots" :key="index" :value="robot">
        {{ robot }}
      </option>
    </select>
    <label for="boxType">Select Box Type:</label>
    <select v-model="selectedBoxType" id="boxType">
      <option value="small">Small Box</option>
      <option value="medium">Medium Box</option>
      <option value="large">Large Box</option>
    </select>

    <div class="color-options">
      <div
        v-for="(color, index) in colorOptions"
        :key="index"
        @click="selectColor(color)"
        :class="{ selected: selectedColor === color }"
        :style="{ backgroundColor: color }"
      ></div>
    </div>

    <div class="preview-section" v-if="selectedColor">
      <h3>Preview:</h3>
      <canvas
        class="preview-box"
        ref="canvas"
        :width="boxWidth"
        :height="boxHeight"
      ></canvas>
      <button @click="saveBox">Save Box</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import axios from "axios";

const name = ref("");
const selectedRobot = ref("");
const robots = ["Robot 1", "Robot 2", "Robot 3"]; // Ganti dengan daftar robot yang Anda miliki
const apiUrl = "http://localhost:5258/footprints";

const selectedBoxType = ref("small");
const selectedColor = ref(null);

const getColorOptions = (boxType) => {
  const boxTypes = {
    small: ["#FF0000", "#00FF00", "#0000FF"],
    medium: ["#FFA500", "#008000", "#000080"],
    large: ["#800080", "#FFFF00", "#008080"],
  };
  return boxTypes[boxType] || [];
};

// Define computed property for colorOptions
const colorOptions = computed(() => getColorOptions(selectedBoxType.value));

const selectColor = (color) => {
  selectedColor.value = color;
};

const boxWidth = ref(50);
const boxHeight = ref(50);

const saveBox = async () => {
  // Get canvas element
  const canvas = canvasRef.value;

  // Get 2D context
  const ctx = canvas.getContext("2d");

  // Clear the canvas
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  // Set the fill color
  ctx.fillStyle = selectedColor.value;

  // Draw a filled rectangle
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // Get the data URL of the canvas
  const boxImage = canvas.toDataURL();

  try {
    const response = await axios.post(apiUrl, {
      Name: name.value,
      Robotname: selectedRobot.value,
      ImageData: boxImage,
    });
    console.log("Box saved successfully:", response.data);
  } catch (error) {
    console.error("Error saving box:", error);
  }
};
</script>

<style scoped>
.color-box {
  margin-top: 10px;
}

.color-options {
  display: flex;
  margin-top: 10px;
}

.color-options div {
  width: 30px;
  height: 30px;
  margin: 5px;
  cursor: pointer;
}

.color-options div.selected {
  border: 2px solid #000;
}

.preview-section {
  margin-top: 20px;
}

.preview-box {
  margin-top: 10px;
  border: 1px solid #000;
}

button {
  margin-top: 10px;
  padding: 5px 10px;
  cursor: pointer;
}
</style>
