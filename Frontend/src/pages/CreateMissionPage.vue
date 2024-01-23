<template>
  <div class="container">
    <h5>
      Created
      <span>Mission</span>
    </h5>
    <p class="p">Create and Design Your Mission Here</p>
    <router-link class="btn btn-light btn-sm bg-light" to="/mission"
      >Back</router-link
    >
    <div class="card bg-light">
      <div class="card-body">
        <label class="text-dark">Name</label>
        <input
          type="text"
          placeholder="Input Name Mission"
          class="form-control form-control-sm"
        />
        <div class="flex">
          <label class="text-dark">
            Site
            <select
              id="Select"
              class="custom-select form-control-sm"
              placeholder="Select Your Own Map"
            ></select>
          </label>
          <br />
          <label class="created">
            Group<input
              type="text"
              placeholder="Input Creator"
              class="form-control form-control-sm"
            />
          </label>
        </div>
      </div>
    </div>
    <br />
    <div class="d-flex">
      <router-link to="/Mission/Created/New" class="btn btn-success"
        >Create Mission</router-link
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
.flex .text-dark select {
  margin-top: 16px;
  width: 400px;
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
select {
  border-radius: 10px;
  font-size: 12px;
  background-color: #d3d3d3;
  opacity: 100%;
}
</style>
