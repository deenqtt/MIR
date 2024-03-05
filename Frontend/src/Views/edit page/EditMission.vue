<template>
  <div class="container">
    <h5>
      Nama
      <span>Mission</span>
    </h5>
    <br />
    <div class="d-flex">
      <button @click="confirmBack" class="btn btn-light">Back</button>
      <button @click="confirmSave" class="btn btn-success">Save</button>
    </div>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex">
          <label for="logicDropdown">Logic</label>
          <select
            class="form-control form-control-sm"
            id="logicDropdown"
            v-model="selectedLogic"
          >
            <option value="IF">If</option>
            <option value="Else">Else</option>
            <option value="Timer">Timer</option>
          </select>
        </div>
        <div class="d-flex">
          <label for="pathDropdown">Path</label>
          <select
            class="form-control form-control-sm"
            v-model="selectedPath"
            id="missionSelect"
          >
            <option v-for="name in pathOptions" :key="name" :value="name">
              {{ name }}
            </option>
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
import axios from "axios";
import { ref, watch, onMounted } from "vue";
import Swal from "sweetalert2";
import router from "../../router";
const selectedLogic = ref("");
const selectedPath = ref("");
const selectedOptions = ref([]);
const pathOptions = ref([]);
const fetchPath = async () => {
  try {
    const response = await axios.get("http://localhost:5258/paths");
    pathOptions.value = response.data.map((path) => path.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};
const removeCard = (index) => {
  selectedOptions.value.splice(index, 1);
};
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
const updateUser = async () => {
  try {
    if (userId.value) {
      await axios.put(`${apiUrl}/${userId.value}`, selectedUser.value);
      console.log("User updated:", selectedUser.value);
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
  console.log("Component mounted");
  fetchPath();
});
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
