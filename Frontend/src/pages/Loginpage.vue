<template>
  <div class="container">
    <h2>LOGIN</h2>

    <div class="card-body bg-dark">
      <form @submit.prevent="loginUser">
        <label for="username">Username</label>
        <input
          v-model="login.username"
          type="text"
          class="form-control"
          placeholder="Input Username"
          required
        />

        <label for="password">Password</label>
        <input
          v-model="login.password"
          type="password"
          placeholder="Input Password"
          required
          class="form-control"
        />
        <br />
        <button type="submit" class="btn btn-success">LOGIN</button>
        <p>
          Not Have Account? Please
          <router-link to="/authentication" class="router"
            >Register</router-link
          >
        </p>
        <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
      </form>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref } from "vue";
import { useRouter } from "vue-router";

const apiUrl = "http://localhost:5258";
const router = useRouter();
const login = ref({ username: "", password: "" });
const errorMessage = ref("");

const loginUser = async () => {
  try {
    // Make a POST request to your backend API for user authentication
    const response = await axios.post(`${apiUrl}/login`, {
      username: login.value.username,
      password: login.value.password,
    });

    const user = response.data; // Assuming the backend sends the user data on successful login

    if (user) {
      console.log("Login successful!");
      // Redirect to another page or perform additional actions
      router.push("/dashboard"); // Adjust the route as needed
    } else {
      errorMessage.value = "Invalid username or password.";
    }
  } catch (error) {
    errorMessage.value = "Invalid username or password.";
    console.error("An error occurred during login:", error);
    // Handle any network or server-related errors
  }
};
</script>

<style scoped>
/* Your existing styles go here */

.error-message {
  color: red;
  margin-top: 10px;
  margin-left: 35px;
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
}
input {
  font-size: 12px;
  font-family: "Poppins", sans-serif;
  font-weight: 500;
  opacity: 20%;
  width: 300px;
  height: 30px;
  margin-left: 10px;
  border-radius: 10px;
  margin-right: 10px;
}
.btn {
  color: #000;
  font-family: "Poppins", sans-serif;
  height: 30px;
  font-weight: 600;
  font-size: 12px;
  border-radius: 10px;
  margin-left: 10px;
}

p {
  margin: 10px;
  margin-left: 10px;
  font-family: "Poppins", sans-serif;
  cursor: pointer;
  color: #ababab;
}
.router {
  text-decoration: none;
  color: #ababab;
  margin-left: 5px;
}
.router:hover {
  color: #ff0000;
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

  p {
    display: none;
  }
  .error-message {
    display: block;
    font-size: 12px;
    margin-left: 30px;
  }
  .btn {
    height: auto;
    padding: 10px;
    text-align: center;
    font-size: 14px;
    margin: 10px;
  }
}
</style>
