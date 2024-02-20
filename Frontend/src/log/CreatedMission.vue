<template>
  <div class="container">
    <h5>
      Nama
      <span>Mission</span>
    </h5>

    <router-link to="/Mission" class="btn btn-light"> Back</router-link>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex">
          <label for="logicDropdown">Logic</label>
          <select id="logicDropdown" v-model="selectedLogic">
            <option value="IF">If</option>
            <option value="Else">Else</option>
            <option value="Timer">Timer</option>
          </select>
        </div>
        <div class="d-flex">
          <label for="pathDropdown">Path</label>
          <select id="pathDropdown">
            <option value="option1">Option 1</option>
            <option value="option2">Option 2</option>
            <!-- Add more options as needed -->
          </select>
        </div>
      </div>
      <div class="card-body">
        <div
          v-for="(option, index) in selectedOptions"
          :key="index"
          class="card option-card"
        >
          <!-- Card content for each selected option goes here -->
          <div class="card-content">
            {{ option }}
            <button
              v-if="option === 'Timer'"
              class="settings-button"
              @click="openSettings(index)"
            >
              <i class="fa-solid fa-gear"></i>
            </button>
            <button class="delete-button" @click="removeCard(index)">
              <i class="fas fa-trash"></i>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";

const selectedLogic = ref("");
const selectedPath = ref("");
const selectedOptions = ref([]);

const removeCard = (index) => {
  selectedOptions.value.splice(index, 1);
};

const openSettings = (index) => {
  if (selectedOptions[index] === "Timer") {
    // Insert a new card with a timer input
    const timerSettingsCard = {
      type: "TimerSettings",
      duration: 0, // default duration in seconds
    };
    selectedOptions.splice(index + 1, 0, timerSettingsCard);
  }
};

watch([selectedLogic, selectedPath], () => {
  if (selectedLogic.value !== "") {
    selectedOptions.value.push(selectedLogic.value);
  }

  // Add more conditions for additional dropdowns if needed
});
</script>

<style scoped>
.settings-button {
  margin-left: 10px;
  border: none;
  background: none;
  margin-left: 750px;
}
/* Add this style to your component's style block */
.delete-button {
  background: none;
  border: none;
  cursor: pointer;
}

.card-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.option-card {
  margin-bottom: 10px; /* Adjust the margin as needed */
  padding: 15px; /* Adjust the padding as needed */
}
input {
  height: 20px;
  margin-top: -25px;
  font-size: 12px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column;
}

h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
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
  width: auto;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: 30px;
  align-self: flex-end;
  margin-right: 40px;
  margin-top: -10px;
  margin-bottom: 10px;
}
.card {
  margin-right: 40px;
  box-shadow: 1px 2px 1px #000;
}
.card-header {
  font-size: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header span {
  margin-left: 10px;
}

.card-header button {
  border: none;
  font-size: 12px;
}

/* Additional styling for dropdowns */
label {
  margin-right: 10px;
}

select {
  width: 120px; /* Adjust the width as needed */
  height: 24px;
  font-size: 12px;
}
</style>
