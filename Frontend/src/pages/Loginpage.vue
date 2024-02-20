<template>
  <div class="container">
    <div class="login-page">
      <h2>LOGIN</h2>

      <div class="card-body shadow-lg bg-light">
        <form @submit.prevent="loginUser">
          <div class="input">
            <input
              class="input__input border-rose-600"
              placeholder=" "
              v-model="login.username"
            />
            <label class="input__label">Username</label>
          </div>
          <br /><br />
          <div class="input">
            <input
              :type="showPassword ? 'text' : 'password'"
              class="input__input"
              placeholder=" "
              v-model="login.password"
            />
            <label class="input__label">Password</label>
            <button
              class="toggle-password"
              @click.prevent="togglePasswordVisibility"
            >
              <i v-if="showPassword" class="fa-solid fa-eye-slash"></i>
              <i v-else class="fa-solid fa-eye"></i>
            </button>
          </div>
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
const showPassword = ref(false);

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};
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
.input {
  position: relative;
  font-size: 16px;
  border-radius: 5px;
}
.input {
  position: relative;
}

.toggle-password {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  right: 10px;
  background-color: transparent;
  border: none;
  cursor: pointer;
  z-index: 5; /* Tambahkan z-index yang lebih tinggi */
}

.input__input {
  padding: 25px 10px 15px 10px;
  color: #000000;
  background-color: transparent;
  position: relative;
  z-index: 3;
}

.input__label {
  display: flex;
  padding-left: 1.5rem;
  padding-bottom: 1rem;
  color: rgba(0, 0, 0, 0.75);
  width: 100%;
  left: 0;
  height: 100%;
  position: absolute;
  margin-bottom: 3px;
  text-transform: uppercase;
  top: 0;
  transition: 0.3s;
  font-weight: 300;
  align-items: center;
  letter-spacing: 1px;
  z-index: 2;
  font-size: inherit;
}

.input__input:focus + .input__label,
.input__input:not(:placeholder-shown) + .input__label {
  transform: translateY(-30px);
  padding-left: 1rem;
  font-size: 12px;
  color: #6c6c6c;
}
.error-message {
  color: red;
  margin-top: 10px;
  margin-left: 35px;
}

.login-page {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.card-body {
  color: rgb(0, 0, 0);
  border-radius: 10px;
}
h2 {
  font-family: "Poppins", sans-serif;
  font-weight: 800;
  margin-bottom: 20px;
  background: linear-gradient(to right, #142896 0%, #330867 100%);
  background-clip: text;
  color: transparent;
}

input {
  font-family: "Poppins", sans-serif;
  font-weight: 500;

  width: 300px;
  height: 30px;
  margin-left: 10px;
  border-radius: 10px;
  margin-right: 10px;

  color: #000;
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
label {
  font-family: "Poppins", sans-serif;
  margin: 8px;
  font-weight: 500;
}
p {
  margin: 10px;
  margin-left: 10px;
  font-family: "Poppins", sans-serif;
  cursor: pointer;
  color: #000000;
}
.router {
  text-decoration: none;
  color: #000000;
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
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
