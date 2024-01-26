<template>
  <div class="container">
    <h5>
      Nama
      <span>Maps</span>
    </h5>

    <p>Create and Design Your Map Here</p>
    <router-link to="/Maps" class="btn btn-light"> Back</router-link>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex">
          <span class="material-symbols-outlined"> open_with </span>
          <span class="material-symbols-outlined"> save </span>
          <p>Save All</p>
        </div>
        <button
          data-toggle="modal"
          data-target="#exampleModal"
          class="material-symbols-outlined"
        >
          widgets
        </button>
      </div>
      <div class="card-body">
        <canvas style="width: 100%"></canvas>
      </div>
    </div>
    <div
      class="modal fade"
      id="exampleModal"
      tabindex="-1"
      role="dialog"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <div class="import">
              <button class="material-symbols-outlined">cloud_download</button>
              <p>Import</p>
            </div>
            <div class="record">
              <button class="material-symbols-outlined">
                radio_button_checked
              </button>
              <p>Record Map</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Modal -->
</template>

<script setup></script>

<style scoped>
canvas {
  border: #000 solid 1px;
}
.modal-content {
  text-align: center;
}

.modal-body {
  display: flex;
  justify-content: space-around;
  align-items: center;
}

.import,
.record {
  text-align: center;
}

.modal .import .material-symbols-outlined {
  font-size: 36px; /* Sesuaikan ukuran ikon sesuai kebutuhan */
  color: #007bff; /* Sesuaikan warna ikon sesuai kebutuhan */
}
.modal .record .material-symbols-outlined {
  font-size: 36px; /* Sesuaikan ukuran ikon sesuai kebutuhan */
  color: #ff0000; /* Sesuaikan warna ikon sesuai kebutuhan */
}
.modal button {
  border: none;
  background: none;
}
.import p,
.record p {
  margin: 0;
  font-size: 14px; /* Sesuaikan ukuran font sesuai kebutuhan */
}
input {
  height: 20px;
  margin-top: -25px;
  font-size: 12px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
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
  font-weight: 100; /* Memberikan jarak atas antara h5 dan p */
}
.btn {
  text-align: center;
  width: 120px;
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
  margin: 10px;
  border-radius: 10px;
  font-size: 15px;
  display: flex; /* atau display: grid; */

  justify-content: space-between;

  align-items: center;
}
th {
  font-size: 13px;
}
.card-header span {
  margin-right: 40px;
}
.card-header p {
  font-size: 12px;
  font-weight: 500;
  margin-top: 5px;
  margin-left: -30px;
}

.card-header button {
  border: none;
  margin-top: -11px;
  justify-self: end;
}
</style>
