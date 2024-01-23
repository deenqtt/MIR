<template>
  <div class="container">
    <h5>
      Created
      <span>Path</span>
    </h5>

    <p>Create and Design Your Path Here</p>
    <router-link to="/Path" class="btn btn-light"> Back</router-link>

    <div class="card bg-light">
      <div class="card-body form-flex">
        <div class="form-group">
          <label for="Name">Name</label>
          <input
            type="text"
            class="form-control"
            placeholder="Input Your Path Name"
          />
        </div>
        <div class="form-group">
          <label for="Select">Select Robot</label>
          <select
            id="Select"
            class="custom-select form-control"
            placeholder="Select Your Own Map"
          ></select>
        </div>
      </div>
    </div>

    <br />
    //Card Untuk Canvas Dibawah ini
    <div class="card bg-light">
      <div class="card-header">
        <p>Created Your Design</p>
      </div>
      <div class="card-body"></div>
      <div class="card-footer"></div>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
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
  box-shadow: 1px 2px 1px #000;
  border-radius: 10px;
}

.card-header {
  font-size: 15px;

  justify-content: space-between;
  align-items: center;
}

.card-header p {
  font-size: 12px;
  font-weight: 500;

  margin-bottom: -20px;
}

.card-header button {
  border: none;
}

.form-flex {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.form-group {
  flex: 1;
  margin-right: 10px;
  margin-bottom: 10px;
}
.form-group input {
  border-radius: 10px;
  background-color: #b4b4b4;
}
.form-group select {
  border-radius: 10px;
  background-color: #b4b4b4;
}
/* Style for input group */
.input-group {
  position: relative;
  width: 100%;
}

.with-button {
  border-radius: 0.25rem 0 0 0.25rem;
}

.input-group-append {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  z-index: 2;
}

.input-group-append .material-symbols-outlined {
  border-radius: 0 0.25rem 0.25rem 0;
  background-color: #b4b4b4;
  margin-right: 9px;
  border: none;
}
</style>
