<template>
  <div class="container">
    <h5>
      I/O
      <span>Modul</span>
    </h5>

    <button
      class="btn btn-success"
      v-if="!showForm && !showEditForm"
      @click="showConnectionForm"
    >
      Create Connection
    </button>

    <div v-if="!showForm && !showEditForm" class="card bg-light">
      <div class="card-header">Internal IO MIR</div>
      <div class="card-body">
        <div class="d-flex">
          <p class="input">
            Input
            <button
              class="btn"
              :class="{
                'btn-danger': inputButtonColors[0] === 'red',
                'btn-success': inputButtonColors[0] === 'green',
              }"
              @click="toggleInputButtonColor(0)"
            >
              1
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': inputButtonColors[1] === 'red',
                'btn-success': inputButtonColors[1] === 'green',
              }"
              @click="toggleInputButtonColor(1)"
            >
              2
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': inputButtonColors[2] === 'red',
                'btn-success': inputButtonColors[2] === 'green',
              }"
              @click="toggleInputButtonColor(2)"
            >
              3
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': inputButtonColors[3] === 'red',
                'btn-success': inputButtonColors[3] === 'green',
              }"
              @click="toggleInputButtonColor(3)"
            >
              4
            </button>
          </p>
          <p class="output">
            Output
            <button
              class="btn"
              :class="{
                'btn-danger': outputButtonColors[0] === 'red',
                'btn-success': outputButtonColors[0] === 'green',
              }"
              @click="toggleOutputButtonColor(0)"
            >
              1
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': outputButtonColors[1] === 'red',
                'btn-success': outputButtonColors[1] === 'green',
              }"
              @click="toggleOutputButtonColor(1)"
            >
              2
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': outputButtonColors[2] === 'red',
                'btn-success': outputButtonColors[2] === 'green',
              }"
              @click="toggleOutputButtonColor(2)"
            >
              3
            </button>
            <button
              class="btn"
              :class="{
                'btn-danger': outputButtonColors[3] === 'red',
                'btn-success': outputButtonColors[3] === 'green',
              }"
              @click="toggleOutputButtonColor(3)"
            >
              4
            </button>
          </p>
        </div>
      </div>
    </div>
    <br />
    <div v-if="!showForm && !showEditForm">
      <div
        class="card bg-light"
        v-for="modul in moduls"
        :key="modul.id"
        style="margin-bottom: 20px"
      >
        <div class="card-body">
          <div class="flex">
            <div class="connect">
              <i class="fa-solid fa-link"></i>
              <p>{{ modul.name }}</p>
            </div>
            <p class="date">{{ modul.date }}</p>
            <div class="action">
              <button
                class="btn"
                :class="{ 'btn-success': isConnected }"
                @click="toggleConnection"
              >
                {{ isConnected ? "Connected" : "Connect" }}
              </button>

              <button
                class="fa-solid fa-sliders"
                @click="editConnection(modul)"
              ></button>

              <button
                class="fa-solid fa-eraser"
                @click="deleteConnection(modul.id)"
              ></button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-if="showForm && !showEditForm" class="card bg-light">
      <div class="card-body">
        <form @submit.prevent="submitConnection">
          <div class="mb-3">
            <label for="connectionName" class="form-label"
              >Connection Name</label
            >
            <input
              type="text"
              class="form-control"
              id="connectionName"
              v-model="newConnection.Name"
            />
          </div>
          <div class="mb-3">
            <label for="connectionDate" class="form-label">Created Date</label>
            <input
              type="date"
              class="form-control"
              id="connectionDate"
              v-model="newConnection.Date"
            />
          </div>
          <div class="mb-3">
            <label for="portInput" class="form-label">Port Input</label>
            <select class="form-select" v-model="newConnection.PortIn">
              <option disabled value="">Please select input port</option>
              <option v-for="port in inputPorts" :key="port" :value="port">
                {{ port }}
              </option>
            </select>
          </div>
          <div class="mb-3">
            <label for="portOutput" class="form-label">Port Output</label>
            <select class="form-select" v-model="newConnection.PortOut">
              <option disabled value="">Please select output port</option>
              <option v-for="port in outputPorts" :key="port" :value="port">
                {{ port }}
              </option>
            </select>
          </div>
          <button type="submit" class="btn btn-primary">Submit</button>
        </form>
        <button class="btn btn-secondary" @click="cancelConnection">
          Back
        </button>
      </div>
    </div>
    <!-- Card untuk menampilkan form edit -->
    <div v-if="showEditForm" class="card bg-light">
      <div class="card-body">
        <form @submit.prevent="updateConnection">
          <div class="mb-3">
            <label for="connectionName" class="form-label"
              >Connection Name</label
            >
            <input
              type="text"
              class="form-control"
              id="connectionName"
              v-model="editedConnection.name"
            />
          </div>
          <div class="mb-3">
            <label for="connectionPortInput" class="form-label"
              >Port Input</label
            >
            <select
              class="form-select"
              id="connectionPortInput"
              v-model="editedConnection.portIn"
            >
              <option disabled value="">Please select input port</option>
              <option v-for="port in inputPorts" :key="port" :value="port">
                {{ port }}
              </option>
            </select>
          </div>
          <div class="mb-3">
            <label for="connectionPortOutput" class="form-label"
              >Port Output</label
            >
            <select
              class="form-select"
              id="connectionPortOutput"
              v-model="editedConnection.portOut"
            >
              <option disabled value="">Please select output port</option>
              <option v-for="port in outputPorts" :key="port" :value="port">
                {{ port }}
              </option>
            </select>
          </div>
          <button type="submit" class="btn btn-primary">Update</button>
        </form>
        <button class="btn btn-secondary" @click="cancelEdit">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import Swal from "sweetalert2";
import store from "../../store";
const moduls = ref([]);
const isConnected = ref(false);
const router = useRouter();
const successMessage = ref("");
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/moduls";
const inputPorts = ["Port 1", "Port 2", "Port 3", "Port 4"];
const outputPorts = ["Port A", "Port B", "Port C", "Port D"];
const showForm = ref(false); // Status tampilan form koneksi baru
const showEditForm = ref(false); // Status tampilan form edit koneksi
const editedConnection = ref({}); // Data koneksi yang sedang diedit
const inputButtonColors = ref(
  JSON.parse(localStorage.getItem("inputButtonColors")) || [
    "red",
    "red",
    "red",
    "red",
  ]
);
const outputButtonColors = ref(
  JSON.parse(localStorage.getItem("outputButtonColors")) || [
    "red",
    "red",
    "red",
    "red",
  ]
);
const newConnection = ref({
  name: "",
  date: "",
  portIn: "",
  portOut: "",
});
const toggleInputButtonColor = (index) => {
  inputButtonColors.value[index] =
    inputButtonColors.value[index] === "green" ? "red" : "green";
  saveColors();
};

const toggleOutputButtonColor = (index) => {
  outputButtonColors.value[index] =
    outputButtonColors.value[index] === "green" ? "red" : "green";
  saveColors();
};

const saveColors = () => {
  localStorage.setItem(
    "inputButtonColors",
    JSON.stringify(inputButtonColors.value)
  );
  localStorage.setItem(
    "outputButtonColors",
    JSON.stringify(outputButtonColors.value)
  );
};

const fetchConnect = async () => {
  try {
    const response = await axios.get(apiUrl);
    moduls.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch users: " + error.message;
  }
};

const submitConnection = async () => {
  try {
    const response = await axios.post(apiUrl, newConnection.value);
    newConnection.value = { name: "", date: "", portIn: "", portOut: "" };
    successMessage.value = "Connection created successfully!";
    // Show success notification
    await Swal.fire("Success!", "Connection created successfully!", "success");
    store.commit("addNotification", "Connection created!");
    // Redirect back to previous page
    router.go(-1);
  } catch (error) {
    errorMessage.value = "Failed to create connection: " + error.message;
  }
};

const showConnectionForm = () => {
  showForm.value = true;
};

const cancelConnection = () => {
  Swal.fire({
    title: "Are you sure?",
    text: "You are about to cancel creating a connection.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, cancel it!",
    cancelButtonText: "No, keep it",
  }).then((result) => {
    if (result.isConfirmed) {
      resetForm();
    }
  });
};

const resetForm = () => {
  showForm.value = false;
  newConnection.value = { name: "", date: "", portIn: "", portOut: "" };
};

const toggleConnection = () => {
  isConnected.value = !isConnected.value;
};

const deleteConnection = async (modulId) => {
  // Gunakan SweetAlert untuk konfirmasi penghapusan
  const confirmDelete = await Swal.fire({
    title: "Are You Sure To Delete?",
    text: "You will not be able to return this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Delete!",
    cancelButtonText: "Cancel",
  });

  if (confirmDelete.isConfirmed) {
    try {
      // Hapus pengguna jika pengguna mengkonfirmasi
      await axios.delete(`${apiUrl}/${modulId}`);
      fetchConnect();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Congratss!", "Connection Has Deleted", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

const editConnection = (modul) => {
  editedConnection.value = { ...modul }; // Salin data koneksi yang dipilih
  showForm.value = false; // Sembunyikan card koneksi
  showEditForm.value = true; // Tampilkan form edit
};

const updateConnection = async () => {
  try {
    await axios.put(
      `${apiUrl}/${editedConnection.value.id}`,
      editedConnection.value
    );
    await Swal.fire("Success!", "Connection Has Been Edited.", "success");
    router.go(-1); // Navigate back to the user list page after the update
  } catch (error) {
    errorMessage.value = "Failed to update connection: " + error.message;
  }
};

const cancelEdit = async () => {
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

onMounted(() => {
  console.log("Component Mounted");
  fetchConnect();
});
</script>

<style scoped>
.fa-solid {
  border: none;
  background: none;
}
.flex .date {
  font-weight: 500;
  margin-top: 5px;
}
.action .fa-solid {
  margin: 10px;
  margin-top: -20px;
}
.action .btn {
  height: 30px;
  font-size: 12px;
  text-align: center;
  border-radius: 10px;
  margin-top: -10px;
}
.connect {
  display: flex; /* Menjadikan elemen connect flex container */
  align-items: center; /* Menyamakan vertikal elemen di dalam container */
}
.connect p {
  font-weight: 500;
}
.connect .fa-solid {
  margin-top: -12px;
}
.flex {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: -10px;
}

.d-flex {
  margin-bottom: -10px;
}
.output .btn {
  margin-top: 10px;
  width: auto;
  gap: 10px;
}
.input .btn {
  margin-top: 10px;
  width: auto;
  gap: -20px;
  margin-left: 10px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
}

h5 {
  margin-top: 20px;
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
}

p {
  margin-left: 10px;
  font-size: 15px;
  font-weight: 500; /* Memberikan jarak atas antara h5 dan p */
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
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}
</style>
