<template>
  <div class="container">
    <h5>
      Edit
      <span>Map</span>
    </h5>

    <button @click="confirmBack" class="btn btn-light">Back</button>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex justify-content-between align-items-center">
          <div class="d-flex align-items-center">
            <button class="material-symbols-outlined">open_with</button>
            <div class="save-all-icon">
              <button class="material-symbols-outlined">save</button>
              <p>Save All</p>
            </div>
            <button class="nav-item dropdown" id="forbiddenZoneDropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="forbiddenZoneDropdownMenu"
                role="button"
                data-bs-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <button class="material-symbols-outlined">
                  emergency_home
                </button>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="forbiddenZoneDropdownMenu"
              >
                <a class="dropdown-item" href="#" @click="changeIcon('tembok')"
                  >Tembok</a
                >
                <a class="dropdown-item" href="#" @click="changeIcon('kaca')"
                  >Kaca</a
                >
                <!-- Tambahkan opsi lain di sini sesuai kebutuhan -->
              </div>
            </button>
          </div>
          <div class="d-flex align-items-center">
            <button class="material-symbols-outlined">zoom_in</button>
            <button class="material-symbols-outlined">zoom_out</button>
          </div>
        </div>
      </div>
      <div class="card-body">
        <canvas style="width: 100%"></canvas>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import router from "../../router";
import Swal from "sweetalert2";

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
canvas {
  border: #000 solid 1px;
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
  margin-left: -7px;
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
  border-radius: 10px;
}

.card-header {
  font-size: 15px;
}

.d-flex {
  display: flex;
}

/* Margin untuk ikon dalam .d-flex */
.d-flex > span {
  margin-right: 10px;
}

.save-all-icon {
  display: flex;
  align-items: center;
  margin-right: 20px;
  margin-left: 20px;
}
.save-all-icon p {
  margin-top: 17px;
  margin-left: 10px;
}
button {
  border: none;
}
</style>
