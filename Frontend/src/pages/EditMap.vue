<template>
  <div class="container">
    <h5>
      Edit
      <span>Map</span>
    </h5>

    <router-link to="/Maps" class="btn btn-light"> Back</router-link>
    <div class="card bg-light">
      <div class="card-header">
        <div class="d-flex justify-content-between align-items-center">
          <div class="d-flex align-items-center">
            <button class="material-symbols-outlined">open_with</button>
            <div class="save-all-icon">
              <button class="material-symbols-outlined">save</button>
              <p>Save All</p>
            </div>
            <button class="nav-item dropdown" id="robot">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="robotDropdown"
                role="button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <button class="material-symbols-outlined">
                  emergency_home
                </button>
              </a>
              <div class="dropdown-menu" aria-labelledby="robotDropdown">
                <!-- Add your robot dropdown content here -->
                <a class="dropdown-item" href="#">Robot 1</a>
                <a class="dropdown-item" href="#">Robot 2</a>
                <a class="dropdown-item" href="#">Robot 3</a>
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

<script setup></script>

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
  font-weight: 500;
  color: #193867;
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
