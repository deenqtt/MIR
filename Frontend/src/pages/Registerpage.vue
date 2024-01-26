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
          @input="validatePassword"
        />

        <label for="Phone">Phone</label>
        <input
          v-model="newUser.phone"
          type="text"
          class="form-control"
          placeholder="Input Number (e.g., +621234567890)"
          required
        />
        <span v-if="phoneValidationError" class="error-message">{{
          phoneValidationError
        }}</span>
        <br />
        <button type="submit" class="btn btn-success">Register</button>
        <span
          >Have an account??
          <router-link to="/" class="span">Login</router-link></span
        >
      </form>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
      <span v-if="passwordValidationError" class="error-message">{{
        passwordValidationError
      }}</span>
    </div>
  </div>
</template>
<script setup>
import { onMounted, ref, watchEffect } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const users = ref([]);
const newUser = ref({ username: "", password: "", phone: "" });
const errorMessage = ref("");
const phoneValidationError = ref("");

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

const validatePhoneNumber = () => {
  const phone = newUser.value.phone || "";
  const isValid = /^(?:\+62|0)[1-9]\d*$/.test(phone);

  if (!isValid) {
    phoneValidationError.value = "I Please use +62 or start with 0.";
  } else {
    phoneValidationError.value = "";
  }
};

watchEffect(() => {
  validatePhoneNumber();
});

const createUser = async () => {
  // Check phone number format before creating the user
  validatePhoneNumber();
  if (phoneValidationError.value) {
    return;
  }
  // Check if username or phone already exists
  const usernameExists = users.value.some(
    (user) => user.username === newUser.value.username
  );
  const phoneExists = users.value.some(
    (user) => user.phone === newUser.value.phone
  );

  if (usernameExists || phoneExists) {
    errorMessage.value = "Username or phone already registered.";
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
const passwordValidationError = ref("");

const validatePassword = () => {
  const password = newUser.value.password || "";

  // Password strength criteria

  const isLengthValid = password.length >= 8;

  // Evaluate password strength
  if (!isLengthValid) {
    passwordValidationError.value =
      "Password must contain at least 8 characters long.";
  } else {
    passwordValidationError.value = "";
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
