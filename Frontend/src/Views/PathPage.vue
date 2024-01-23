<template>
  <div class="container">
    <h5>
      Pat
      <span>h</span>
    </h5>

    <p>Create and Design Your Path Here</p>
    <div class="d-flex">
      <button class="btn btn-danger mr-2">Delete All</button>
      <router-link to="/Path/New" class="btn btn-success"
        >Create Path</router-link
      >
    </div>
    <div class="card bg-light">
      <div class="card-header">List Path</div>
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <form class="form-inline my-2 my-lg-0">
            <input
              class="form-control mr-sm-2"
              type="search"
              placeholder="Search By Name"
              aria-label="Search"
            />
          </form>
        </div>
        <table class="table table-striped">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Start</th>
              <th scope="col">Goal</th>
              <th scope="col">Distance</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.d-flex {
  align-self: flex-end;
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
  margin-left: -7px;
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
}
th {
  font-size: 13px;
}
</style>
