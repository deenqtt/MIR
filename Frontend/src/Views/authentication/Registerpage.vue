<template>
  <div class="container">
    <div class="register-page">
      <h2>REGISTER</h2>
      <img src="../image/route.png" alt="" class="logo" />
      <div class="card-body shadow-lg bg-light">
        <div class="content">
          <div class="image-container">
            <!-- Placeholder for image -->

            <img src="../image/Logo.png" alt="Your Image" class="logo-img" />
          </div>
          <div class="form-container">
            <form @submit.prevent="createUser">
              <div class="input">
                <input
                  class="form-control"
                  placeholder=" "
                  v-model="newUser.username"
                />
                <label class="input__label">Username</label>
              </div>
              <br />
              <div class="input">
                <input
                  :type="showPassword ? 'text' : 'password'"
                  class="form-control"
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
              <br />
              <div class="input">
                <input
                  class="form-control"
                  placeholder=" "
                  v-model="newUser.phone"
                />
                <label class="input__label">Phone</label>
              </div>

              <br />
              <button type="submit" class="btn btn-success">Register</button>
              <div class="massage">
                <span v-if="phoneValidationError" class="error-message">{{
                  phoneValidationError
                }}</span>
                <br />
                <span
                  >Have an account??
                  <router-link to="/auth/login" class="router"
                    >Login</router-link
                  ></span
                >
              </div>
            </form>
            <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
            <span v-if="passwordValidationError" class="error-message">{{
              passwordValidationError
            }}</span>
          </div>
        </div>
      </div>
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
.container {
  margin-top: -20px;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: Poppins, sans-serif; /* Menyusun item secara vertikal di tengah */
}
.register-page {
  width: 800px;
  text-align: center; /* Posisikan teks ke tengah */
}

.card-body {
  display: flex;
  flex-direction: column; /* Menjadikan card-body tata letak kolom */
  align-items: center; /* Menyusun item di tengah */
  border-radius: 10px;
}

h2 {
  margin-top: 10px;
  font-weight: 800;
  margin-bottom: 10px; /* Memberi jarak atas */
  background: linear-gradient(to right, #1c3db5 0%, #330867 100%);
  background-clip: text;
  color: transparent;
}

.content {
  display: flex;
}

.image-container {
  flex: 1;
  padding: 20px; /* Tambahkan padding agar terlihat lebih baik */
}

.image-container img {
  max-width: 100%;
  height: auto;
}

.form-container {
  flex: 1;
  padding: 20px; /* Tambahkan padding agar terlihat lebih baik */
}

.input {
  position: relative;
  margin-bottom: 20px; /* Tambahkan margin bottom untuk menambahkan ruang antar elemen input */
}

.form-control:focus + .input__label,
.form-control:not(:placeholder-shown) + .input__label {
  transform: translateY(-30px);
  padding-left: 1rem;
  font-size: 12px;
  color: #6c6c6c;
}

.toggle-password {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  right: 45px;
  background-color: transparent;
  border: none;
  cursor: pointer;
  z-index: 5; /* Tambahkan z-index yang lebih tinggi */
}

.error-message {
  color: red;
  margin-top: 10px;
}

.btn {
  color: #000;
  font-family: "Poppins", sans-serif;
  height: 35px;
  font-weight: 600;
  font-size: 12px;
  border-radius: 4px;
  width: 248px;
  margin-top: -20px;
  margin-left: -40px;
}

label {
  font-family: "Poppins", sans-serif;
  margin: 8px;
  font-weight: 500;
}
.massage {
  margin-right: 40px;
}
p {
  margin: 10px;
  margin-left: -80px;
  font-family: "Poppins", sans-serif;
  cursor: pointer;
  color: #000000;
}
p {
  margin-left: -35px;
}
.router {
  text-decoration: none;
  color: #000000;
}

.router:hover {
  color: #ff0000;
}

@media (max-width: 768px) {
  .container {
    flex-direction: column;
  }

  .login-page {
    width: 100%; /* Atur lebar sesuai kebutuhan */
  }

  .image-container,
  .form-container {
    padding: 10px; /* Ubah padding untuk tampilan seluler */
  }

  .content {
    flex-direction: column; /* Tampilan kolom untuk layar kecil */
  }

  input,
  button {
    width: calc(100% - 20px); /* Ubah lebar input untuk tampilan seluler */
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
.input {
  position: relative;
  margin-bottom: 20px;
}

.form-control {
  width: calc(100% - 40px); /* Sesuaikan lebar input */
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
  font-size: inherit;
  padding-left: 10px;
  position: absolute;
  pointer-events: none;
  transition: 0.3s;
  color: #6c6c6c;
}
.logo-img {
  max-width: 300px; /* Sesuaikan ukuran gambar */
  max-height: 300px; /* Sesuaikan ukuran gambar */
  -webkit-filter: drop-shadow(5px 5px 5px #666666);
  filter: drop-shadow(5px 5px 5px #6e6e6e);
  transition: transform 0.3s ease; /* Menambahkan efek transisi */
}

.logo-img:hover {
  transform: scale(1.1); /* Memperbesar gambar saat hover */
}
.logo {
  width: 100px;
  height: 100px;
  position: absolute;
  top: 50%; /* Letakkan di tengah vertikal */
  left: 150px; /* Geser ke kiri sejauh 50px */
  transform: translateY(-50%); /* Geser ke atas sejauh setengah tinggi */
  z-index: 1; /* Meletakkan di belakang card */

  background-size: cover;
  border-radius: 50%; /* Membuat bentuk lingkaran */
}
</style>
