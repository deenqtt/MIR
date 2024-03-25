<template>
  <div class="container">
    <div class="login-page" :class="{ blur: loading }">
      <h2>LOGIN</h2>
      <div class="card-body shadow-lg bg-light">
        <!-- Form login -->
        <div class="content">
          <div class="image-container">
            <!-- Tambahkan gambar di sini -->
            <img src="../image/Logo.png" alt="Logo" class="logo-img" />
          </div>
          <form @submit.prevent="loginUser" class="form-container">
            <div class="input">
              <input
                class="form-control"
                placeholder=" "
                v-model="login.username"
              />
              <label class="input__label">Username</label>
            </div>
            <br />
            <div class="input">
              <input
                :type="showPassword ? 'text' : 'password'"
                class="form-control"
                placeholder=" "
                v-model="login.password"
              />
              <label class="input__label">Password</label>
              <button
                class="toggle-password"
                @click.prevent="togglePasswordVisibility"
                :class="{ 'eye-animation': isEyeBlinking }"
              >
                <i v-if="showPassword" class="fa-solid fa-eye-slash"></i>
                <i v-else class="fa-solid fa-eye"></i>
              </button>
            </div>
            <br />
            <button type="submit" class="btn btn-success">LOGIN</button>
            <p>
              Not Have Account? Please
              <router-link to="/auth/register" class="router">
                Register
              </router-link>
            </p>
            <p v-if="errorMessage" class="error-message">
              {{ errorMessage }}
            </p>
          </form>
        </div>
      </div>
    </div>

    <!-- Loading Screen -->
    <div v-if="loading || loginSuccess" class="loading-container">
      <div class="loading"></div>
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
const isEyeBlinking = ref(false); // Menandakan apakah animasi mata sedang berlangsung
const loading = ref(false);
const loginSuccess = ref(false); // Menandakan apakah login berhasil

const loginUser = async () => {
  try {
    loading.value = true;
    const response = await axios.post(`${apiUrl}/login`, {
      username: login.value.username,
      password: login.value.password,
    });

    const token = response.data.token;

    if (token) {
      // Simpan token ke local storage
      localStorage.setItem("token", token);

      console.log("Login successful!");
      loginSuccess.value = true; // Set loginSuccess menjadi true
      setTimeout(() => {
        router.push("/dashboard"); // Arahkan pengguna ke halaman dashboard setelah 3 detik
      }, 3000); // Ubah angka sesuai dengan durasi penundaan yang diinginkan
    } else {
      errorMessage.value = "Invalid username or password.";
    }
  } catch (error) {
    errorMessage.value = "Invalid username or password.";
    console.error("An error occurred during login:", error);
  } finally {
    loading.value = false;
  }
};

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
  isEyeBlinking.value = true;
  setTimeout(() => {
    isEyeBlinking.value = false;
  }, 500); // Durasi animasi CSS
};
</script>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: Poppins, sans-serif; /* Menyusun item secara vertikal di tengah */
}
.login-page {
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
  width: 100%;
}

.image-container {
  flex: 1;
  padding: 20px;
  background: none;
  border: none; /* Tambahkan padding agar terlihat lebih baik */
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

@keyframes eye-blink {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.2);
  }
  100% {
    transform: scale(1);
  }
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
  width: 300px;
  margin-top: -20px;
  margin-left: -43px;
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

@media screen and (max-width: 768px) {
  .content {
    flex-direction: column;
    align-items: center;
  }

  .form-container {
    width: 80%; /* Adjust width for mobile */
  }

  .image-container {
    margin-bottom: 20px;
  }

  .input {
    width: 100%;
    margin-bottom: 10px;
  }
  .input__label {
    font-size: 14px; /* Increase font size for mobile */
  }

  .btn {
    width: 80%; /* Adjust width for mobile */
  }

  .error-message {
    font-size: 12px; /* Decrease font size for mobile */
  }
}

.logo-img {
  max-width: 300px; /* Sesuaikan ukuran gambar */
  max-height: 300px; /* Sesuaikan ukuran gambar */
  margin-top: -60px;

  -webkit-filter: drop-shadow(5px 5px 5px #666666);
  filter: drop-shadow(5px 5px 5px #666666);
}

.logo-img:hover {
  transform: scale(1.1); /* Memperbesar gambar saat hover */
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

.toggle-password {
  position: absolute;
  top: 50%;
  right: 45px;
  transform: translateY(-50%);
  background-color: transparent;
  border: none;
  cursor: pointer;
  z-index: 5; /* Tambahkan z-index yang lebih tinggi */
}

.blur {
  filter: blur(5px); /* Apply blur effect to the entire page */
}

.loading-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.8); /* Semi-transparent white background */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999; /* Ensure the loading screen is on top of other elements */
}

.loading {
  border: 16px solid #f3f3f3; /* Light grey */
  border-top: 16px solid #3498db; /* Blue */
  border-radius: 50%;
  width: 120px;
  height: 120px;
  animation: spin 5s linear infinite; /* Apply animation for spinning effect */
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
