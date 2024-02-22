<template>
  <div class="container">
    <h2>REGISTER</h2>

    <div class="card-body shadow-lg bg-light">
      <form @submit.prevent="createUser">
        <div class="input">
          <input
            class="input__input"
            placeholder=" "
            v-model="newUser.username"
          />
          <label class="input__label">Username</label>
        </div>

        <div class="input">
          <input
            :type="showPassword ? 'text' : 'password'"
            class="input__input"
            placeholder=" "
            v-model="newUser.password"
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

        <div class="input">
          <input class="input__input" placeholder=" " v-model="newUser.phone" />
          <label class="input__label">Phone</label>
        </div>
        <span v-if="phoneValidationError" class="error-message">{{
          phoneValidationError
        }}</span>
        <br />
        <button type="submit" class="btn btn-success">Register</button>
        <span
          >Have an account??
          <router-link to="/auth/login" class="span">Login</router-link></span
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
const showPassword = ref(false);

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};
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

  if (isValid) {
    phoneValidationError.value = "";
  } else {
    phoneValidationError.value = "Please use +62 or start with 0.";
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
    router.push("/auth/login");
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
    passwordValidationError.value = "Password must 8 characters long.";
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
.input {
  position: relative;
  font-size: 16px;
  border-radius: 5px;
}

.input__input {
  padding: 25px 10px 15px 10px;
  color: #000000;
  background-color: transparent;
  position: relative;
  z-index: 3;
}

.input__label {
  font-family: "Poppins", sans-serif;

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
}
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.card-body {
  color: rgb(0, 0, 0);
  border-radius: 10px;
  box-shadow: -10px 4px 21px 1px rgba(0, 0, 0, 0.71);
  -webkit-box-shadow: -10px 4px 21px 1px rgba(0, 0, 0, 0.71);
  -moz-box-shadow: -10px 4px 21px 1px rgba(0, 0, 0, 0.71);
}
h2 {
  font-family: "Poppins", sans-serif;
  font-weight: 800;
  margin-bottom: 20px;
  background: linear-gradient(to right, #1c3db5 0%, #330867 100%);
  background-clip: text;
  color: transparent;
}
label {
  font-family: "Poppins", sans-serif;
  margin: 8px;
  font-weight: 500;
}
input {
  font-family: "Poppins", sans-serif;
  width: 300px;
  height: 30px;
  margin-bottom: 5px;
  border-radius: 10px;
  margin: 10px;
  color: #000;
  font-weight: 700;
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
  color: #000000;
}
.span {
  color: #000000;
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
