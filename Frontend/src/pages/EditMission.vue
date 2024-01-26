<template>
  <div class="container">
    <h5>
      Edit
      <span>Mission</span>
    </h5>

    <router-link to="/Mission" class="btn btn-light"> Back</router-link>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex">
          <label for="logicDropdown">Logic</label>
          <select id="logicDropdown" v-model="selectedLogic">
            <option value="option1">Option 1</option>
            <option value="option2">Option 2</option>
            <!-- Add more options as needed -->
          </select>
        </div>
        <div class="d-flex">
          <label for="pathDropdown">Path</label>
          <select id="pathDropdown" v-model="selectedPath">
            <option value="option1">Option 1</option>
            <option value="option2">Option 2</option>
            <!-- Add more options as needed -->
          </select>
        </div>
      </div>
      <div class="card-body">
        <div class="d-flex align-items-center"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";

const selectedLogic = ref(""); // Holds the selected value for Logic dropdown
const selectedPath = ref(""); // Holds the selected value for Path dropdown
</script>

<style scoped>
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
  font-weight: 500;
  color: #193867;
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
