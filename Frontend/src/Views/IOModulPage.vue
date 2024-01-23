<template>
  <div class="container">
    <h5>
      I/O
      <span>Modul</span>
    </h5>

    <p>Customize I/O Here</p>

    <button class="btn btn-success">Create Path</button>

    <div class="card bg-light">
      <div class="card-header">Internal IO MIR</div>
      <div class="card-body">
        <div class="d-flex">
          <p class="input">
            Input

            <button class="btn btn-danger">1</button>
            <button class="btn btn-danger">2</button>
            <button class="btn btn-danger">3</button>
            <button class="btn btn-success">4</button>
          </p>
          <p class="output">
            Output
            <button class="btn btn-danger">1</button>
            <button class="btn btn-danger">2</button>
            <button class="btn btn-danger">3</button>
            <button class="btn btn-success">4</button>
          </p>
        </div>
      </div>
    </div>
    <br />
    <div class="card bg-light">
      <div class="card-body">
        <div class="flex">
          <div class="connect">
            <span class="material-symbols-outlined"> link </span>

            <p>Name Connection</p>
          </div>

          <p class="date">Created Date</p>

          <div class="action">
            <button class="btn btn-success">connect</button>
            <span class="material-symbols-outlined"> edit </span>
            <span class="material-symbols-outlined"> delete </span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.flex .date {
  font-weight: 500;
  margin-top: 5px;
}
.action .material-symbols-outlined {
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
  display: flex;
  margin-top: 6px;
}
.connect p {
  font-weight: 500;
}
.connect .material-symbols-outlined {
  margin-top: -12px;
}
.flex {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: -10px;
}
.material-symbols-outlined {
  size: 12px;
  margin-top: -17px;
  margin-left: -10px;
  margin-right: 10px;
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
  font-size: 25px;
  font-weight: 500;
  color: #002cae;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000000;
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
</style>
