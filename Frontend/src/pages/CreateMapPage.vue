<template>
  <div class="container">
    <h5>
      Created
      <span>Maps</span>
    </h5>
    <p class="p">Create and Design Your Map Here</p>
    <router-link class="btn btn-light btn-sm bg-danger" to="/maps"
      >Back</router-link
    >
    <div class="card bg-light">
      <div class="card-body">
        <p class="text-dark">Name</p>
        <input
          type="text"
          placeholder="Input Name Maps"
          class="form-control form-control-sm"
        />
        <div class="flex">
          <p class="text-dark">
            Site
            <input
              type="text"
              placeholder="Input Site"
              class="form-control form-control-sm"
            />
          </p>
          <br />
          <p class="created">
            Created By<input
              type="text"
              placeholder="Input Creator"
              class="form-control form-control-sm"
            />
          </p>
        </div>
      </div>
    </div>
    <br />
    <div class="d-flex">
      <router-link to="/Maps/Created/New" class="btn btn-success"
        >Create Map</router-link
      >
      <button class="btn btn-light">Cancel</button>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.container {
  font-family: "Poppins", sans-serif;
}
.flex {
  display: flex;
  margin-top: 10px;
}
.flex .created {
  margin-left: 20px;
  font-size: 13px;
}
.text-dark {
  font-size: 13px;
}
.flex .created input {
  width: 400px;
}
.flex .text-dark input {
  width: 500px;
}
.d-flex .btn {
  font-size: 12px;
  color: #000;
  font-weight: 500;
  border-radius: 8px;
  margin: 10px;
}
h5 {
  font-size: 25px;
  font-weight: 500;
  color: #0060f1;
}

span {
  font-size: 25px;
  font-weight: 500;
  color: #000;
}
input {
  border-radius: 10px;
  font-size: 12px;
  background-color: #d3d3d3;
  opacity: 100%;
}
.container .p {
  font-weight: 100;
  font-size: 12px;
}
.card {
  margin-top: 8px;
  border-radius: 10px;
  box-shadow: 1px 1px 4px #000;
}
</style>
