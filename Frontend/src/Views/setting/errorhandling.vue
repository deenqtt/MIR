<template>
  <div class="container">
    <div class="h5">
      <h5>Error <span>Handling </span></h5>

      <!-- Dropdown -->
      <div class="dropdown">
        <button
          class="btn dropdown-toggle"
          type="button"
          id="dropdownMenuButton"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        ></button>
        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
          <a class="dropdown-item" href="#">Action</a>
          <a class="dropdown-item" href="#">Another action</a>
          <a class="dropdown-item" href="#">Something else here</a>
        </div>
      </div>
    </div>
    <div class="image-container">
      <img src="../image/robotig.png" alt="Robot Image" />
      <p>Please Select Robot To See Detail Informations</p>
    </div>

    <!-- Button back -->
    <button @click="goBack" class="btn btn-primary">Back</button>
  </div>
</template>

<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

const goBack = () => {
  router.go(-1); // Kembali ke halaman sebelumnya
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center; /* Memusatkan konten secara horizontal */
  font-family: "Poppins", sans-serif;
}

h5 {
  color: #007bff; /* Warna biru */
  font-weight: 700;
}

.h5 {
  display: flex;
  flex-direction: row;
  margin-left: -900px;
  padding: 20px;
}

span {
  color: #000000; /* Warna hitam */
}

.dropdown {
  margin-top: -5px;
}

.image-container {
  text-align: center; /* Memusatkan gambar dan teks secara horizontal */
  margin-top: 20px;
}

.image-container img {
  max-width: 100%; /* Mengatur lebar gambar agar tidak melebihi lebar kontainer */
}

.image-container p {
  margin-top: 10px;
  font-size: 18px;
  color: #333; /* Warna teks */
}
</style>
