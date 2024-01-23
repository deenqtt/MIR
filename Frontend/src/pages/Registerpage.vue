<template>
  <div class="container">
    <h2>REGISTER</h2>

    <div class="card-body bg-dark">
      <form @submit.prevent="createUser">
        <label for="username">Username</label>
        <input
          v-model="newUser.username"
          type="text"
          class="form-control"
          placeholder="Input Username"
          required
        />

        <label for="password">Password</label>
        <input
          v-model="newUser.password"
          type="password"
          placeholder="Input Password"
          required
          class="form-control"
        />

        <label for="Phone">Phone</label>
        <input
          v-model="newUser.phone"
          type="text"
          class="form-control"
          placeholder="Input Number"
          required
        />
        <br />

        <button type="submit" class="btn btn-success">Register</button>
        <span
          >Have an account??
          <router-link to="/" class="span">Login</router-link></span
        >
      </form>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const users = ref([]);
const newUser = ref({ username: "", password: "", phone: "" });
const errorMessage = ref("");

const router = useRouter();
const apiUrl = "http://localhost:5258/users";

const fetchUsers = async () => {
  try {
    const response = await axios.get(apiUrl);
    users.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch users: " + error.message;
  }
};

const createUser = async () => {
  // Check if username or phone already exists
  const usernameExists = users.value.some(
    (user) => user.username === newUser.value.username
  );
  const phoneExists = users.value.some(
    (user) => user.phone === newUser.value.phone
  );

  if (usernameExists || phoneExists) {
    errorMessage.value =
      "Username or phone already registered. Please choose a different one.";
    return;
  }

  try {
    const response = await axios.post(apiUrl, newUser.value);
    // Reset newUser fields to empty after successful registration
    newUser.value = { username: "", password: "", phone: "" };
    router.push("/");
  } catch (error) {
    errorMessage.value = "Failed to create user: " + error.message;
  }
};

onMounted(() => {
  console.log("Component Mounted");
  fetchUsers();
});
</script>

<style scoped>
.error-message {
  color: red;
  margin-top: 10px;
}
.container {
  margin-top: 60px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.card-body {
  color: white;
  border-radius: 10px;
}
h2 {
  font-family: "Poppins", sans-serif;
  font-weight: 600;
  margin-bottom: 20px;
}
label {
  font-family: "Poppins", sans-serif;
  margin: 10px;
  font-weight: 500;
  margin-bottom: -5px;
}
input {
  font-size: 12px;
  font-family: "Poppins", sans-serif;
  font-weight: 500;
  opacity: 50%;
  width: 300px;
  height: 30px;
  margin-bottom: 5px;
  border-radius: 10px;
  margin: 10px;
}
.btn {
  background-color: #fff;
  color: #000;
  font-family: "Poppins", sans-serif;
  height: 30px;
  font-weight: 600;
  font-size: 12px;
  border-radius: 10px;
  margin: 5px;
  margin-left: 10px;
}
.btn:hover {
  background-color: #00dd58;
}
span {
  margin: 0;
  margin-left: 5px;
  font-family: "Poppins", sans-serif;
  cursor: pointer;
  color: #ababab;
}
.span {
  color: #fff;
  margin-left: 5px;
  text-decoration: none;
}
.span:hover {
  color: #00dd58;
}
@media (max-width: 768px) {
  .container {
    flex-direction: column;
  }
  input,
  button {
    width: 90%;
    margin-bottom: 10px;
  }
  span {
    display: none;
  }
  .btn {
    height: auto;
    padding: 10px;
    text-align: center;
    font-size: 14px;
  }
}
</style>
