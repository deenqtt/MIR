<template>
  <div class="sidebar">
    <button class="hamburger-button" @click="toggleSidebar">
      <!-- Icon bars di sini -->
      <i class="fa-solid fa-bars"></i>
    </button>
    <aside
      :class="`${is_expanded ? 'is-expanded' : ''} ${
        is_mobile ? 'is-mobile' : ''
      }`"
    >
      <!-- Tombol untuk menyembunyikan sidebar -->
      <button v-if="is_expanded" class="close-button" @click="toggleSidebar">
        <i class="fa-solid fa-times"></i>
      </button>
      <br /><br />
      <div class="menu">
        <div class="menu1">
          <router-link
            to="/dashboard"
            class="button"
            :class="{ active: $route.path === '/dashboard' }"
          >
            <lord-icon
              class="fa-solid"
              src="https://cdn.lordicon.com/wmwqvixz.json"
              trigger="hover"
              colors="primary:#ffffff"
              data-toggle="tooltip"
              data-bs-placement="right"
              title="Dashboard"
            >
            </lord-icon>
          </router-link>
        </div>
        <router-link
          to="/maps"
          class="button"
          :class="{ active: $route.path === '/maps' }"
        >
          <lord-icon
            class="fa-solid"
            src="https://cdn.lordicon.com/omiqopzf.json"
            trigger="hover"
            colors="primary:#ffffff"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="Setup"
          >
          </lord-icon>
        </router-link>
        <!-- Sisipkan event mouseenter dan mouseleave untuk menampilkan dan menyembunyikan tooltip -->
        <router-link
          to="/activity"
          class="button"
          :class="{ active: $route.path === '/activity' }"
          @click="handleNotificationClick"
        >
          <lord-icon
            class="fa-solid"
            src="https://cdn.lordicon.com/uwinmnkh.json"
            trigger="hover"
            colors="primary:#ffffff"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="Monitoring"
          >
          </lord-icon>
          <!-- Menampilkan titik hijau hanya jika ada notifikasi yang belum dibaca -->
          <span class="notification-dot" v-if="hasNewInputError"></span>
        </router-link>
        <router-link
          to="/settings"
          class="button"
          :class="{ active: $route.path === '/settings' }"
        >
          <lord-icon
            src="https://cdn.lordicon.com/ifsxxxte.json"
            trigger="hover"
            colors="primary:#ffffff"
            class="fa-solid"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="System"
          >
          </lord-icon>
        </router-link>
      </div>

      <div class="flex"></div>

      <div class="menu">
        <router-link
          to="/Robot"
          class="button"
          :class="{ active: $route.path === '/Robot' }"
        >
          <lord-icon
            class="fa-solid"
            src="https://cdn.lordicon.com/ojnjgkun.json"
            trigger="hover"
            colors="primary:#ffffff"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="Help"
          >
          </lord-icon>
        </router-link>
        <button class="tombol" @click="logout">
          <span
            class="fa-solid fa-right-from-bracket custom-tooltip"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="Logout"
          ></span>
        </button>
      </div>
      <div v-if="loading" class="loading-overlay">
        <div class="loading-spinner"></div>
      </div>
      <div v-if="is_mobile && is_expanded" class="background-overlay"></div>
    </aside>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, computed } from "vue";
import { useRouter } from "vue-router";
import { removeAuthToken } from "../router/auth";
import { useStore } from "vuex";
import store from "../store";
const loading = ref(false);
const router = useRouter();

const hasNewInputError = computed(() => {
  // Ambil nilai properti unreadNotifications dari store
  const unreadNotifications = store.state.unreadNotifications;
  // Tentukan apakah ada notifikasi yang belum dibaca
  return unreadNotifications > 0;
});
const handleNotificationClick = () => {
  // Mengosongkan jumlah notifikasi yang belum dibaca saat ikon diklik
  store.commit("markAllNotificationsAsRead");

  // Menyembunyikan ikon notifikasi saat diklik
  hasNewInputError.value = false;
};

const ToggleMenu = () => {
  is_expanded.value = !is_expanded.value;
};
const logout = async () => {
  // Menampilkan animasi loading dan menerapkan efek blur pada latar belakang
  loading.value = true;
  document.body.style.overflow = "hidden"; // Menghilangkan scroll pada latar belakang

  // Menunda navigasi ke halaman berikutnya selama 3 detik
  await new Promise((resolve) => setTimeout(resolve, 3000));

  // Membersihkan status autentikasi dari localStorage menggunakan fungsi dari auth.js
  removeAuthToken();

  // Mengarahkan pengguna kembali ke halaman login setelah selesai logout
  router.push({ name: "Login" });
};
const onButtonHover = () => {
  console.log("Button hovered");
};
// Definisikan variabel is_expanded dan is_mobile
const is_expanded = ref(false);
const is_mobile = ref(window.innerWidth <= 660); // Inisialisasi dengan nilai berdasarkan ukuran layar saat ini

// Definisikan fungsi untuk menampilkan/menyembunyikan sidebar pada tampilan ponsel
const toggleSidebar = () => {
  is_expanded.value = !is_expanded.value;
};

// Pastikan fungsi ini dipanggil ketika ukuran jendela berubah
const checkWindowSize = () => {
  is_mobile.value = window.innerWidth <= 660; // Atur batas ukuran untuk menentukan apakah itu ponsel atau bukan
};

// Dipanggil saat komponen dimuat
onMounted(() => {
  checkWindowSize();
  window.addEventListener("resize", checkWindowSize);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", checkWindowSize);
});
</script>

<style lang="scss" scoped>
/* Atur tampilan latar belakang saat sidebar diperluas pada tampilan ponsel */
.is-expanded .background-overlay {
  display: block; /* Tampilkan latar belakang saat sidebar diperluas */
}

.close-button {
  position: fixed; /* Tetapkan posisi tetap agar tombol close selalu terlihat */
  top: 20px; /* Atur posisi ke bawah dari atas */
  left: 220px; /* Atur posisi ke kiri dari kiri */
  z-index: 9999; /* Tetapkan z-index tinggi untuk menempatkan tombol di atas elemen lain */
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
}

.is-expanded {
  width: 240px; /* Atur lebar sidebar yang diperluas */
  transition: width 0.3s ease; /* Tambahkan transisi agar perubahan lebar menjadi mulus */
}

/* Atur tampilan untuk tombol close saat sidebar diperluas */
.is-expanded .close-button {
  display: block; /* Tampilkan tombol close saat sidebar diperluas */
}

.hamburger-button {
  position: fixed; /* Tetapkan posisi tetap agar tombol hamburger selalu terlihat */
  display: none; /* Atur posisi ke bawah dari atas */
  left: 10px; /* Atur posisi ke kiri dari kiri */
  z-index: 9999; /* Tetapkan z-index tinggi untuk menempatkan tombol di atas elemen lain */
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
}
.fa-times {
  color: #ffff;
}

/* Gaya untuk latar belakang blur/gelap */
.background-overlay {
  position: fixed; /* Tetapkan posisi absolut agar latar belakang menutupi seluruh layar */
  top: 0; /* Tempatkan latar belakang di bagian atas */
  left: 0; /* Tempatkan latar belakang di sisi kiri */
  width: 100%; /* Atur lebar latar belakang */
  height: 100%; /* Atur tinggi latar belakang */
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 99909; /* Tetapkan z-index sedikit lebih rendah dari sidebar agar tidak menutupinya */
  display: none; /* Sembunyikan secara default */
}
/* Gaya untuk ikon bars */
.fa-bars {
  color: #000000; /* Ubah warna ikon menjadi putih */
}
/* Atur tampilan hamburger untuk tampilan ponsel */
@media screen and (max-width: 660px) {
  .hamburger-button {
    display: block; /* Tampilkan tombol hamburger pada tampilan ponsel */
  }
  .close-button {
    display: none; /* Sembunyikan tombol close secara default */
  }
  /* Sembunyikan sidebar saat tombol hamburger tidak aktif */
  .is-mobile {
    display: none;
  }
  aside {
    position: absolute; /* Tetapkan posisi absolut agar sidebar berada di atas komponen lain */
    top: 0; /* Tempatkan sidebar di bagian atas */
    left: 0; /* Tempatkan sidebar di sisi kiri */
    bottom: 0;
    z-index: 999999; /* Biarkan sidebar menutupi seluruh ketinggian layar */
  }
  /* Atur tampilan sidebar saat tombol hamburger aktif */
  .is-mobile.is-expanded {
    display: block;
  }
}
/* Gaya untuk elemen animasi loading */
.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* Latar belakang semi-transparan */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999; /* Pastikan di atas konten lainnya */
}

.loading-spinner {
  border: 8px solid #f3f3f3; /* Light grey */
  border-top: 8px solid #3498db; /* Blue */
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite; /* Animasi putar */
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

/* Efek blur untuk latar belakang saat animasi loading aktif */
.loading-overlay + .is-expanded {
  filter: blur(5px);
}
.notification-dot {
  position: absolute;
  width: 10px;
  height: 10px;
  background-color: rgb(0, 30, 255);
  border-radius: 50%;
  top: 15px;
  right: 19px;
}

.tombol {
  margin-top: 20px;
  margin-left: 2px;
  background: none;
  color: #fff;
  border: none;
  display: flex;
  align-items: center;
  padding: 1rem 1rem;
  font-size: 1.5rem;
}
.custom-tooltip.tooltip-inner {
  background-color: blue; /* Ubah warna latar belakang menjadi biru */
}

aside {
  display: flex;
  flex-direction: column;
  background-color: #000000;

  width: calc(2rem + 32px);
  overflow: hidden;
  min-height: 100vh;
  padding: 1rem;

  .flex {
    flex: 1 1 0%;
  }

  h3,
  .button .text {
    opacity: 0;
    transition: opacity 0.3s ease-in-out;
  }

  h3 {
    color: var(--grey);
    font-size: 0.875rem;
    margin-bottom: 0.5rem;
    text-transform: uppercase;
  }

  .menu {
    margin: 0 -1rem;

    .button {
      display: flex;
      align-items: center;
      text-decoration: none;
      position: relative; /* Menjadikan posisi relatif untuk tooltip */

      padding: 1rem 1rem; /* Adjust padding for vertical and horizontal spacing */

      .fa-solid {
        font-size: 1.5rem;
        color: var(--light);
        transition: 0.2s ease-in-out;
        margin-left: 2px;
      }

      &:hover {
        background-color: var(--dark-alt);
      }

      /* Tampilan tooltip */
      &::after {
        content: attr(data-tooltip);
        position: absolute;
        top: 50%;
        left: calc(100% + 10px);
        transform: translateY(-50%);
        padding: 0.5rem;
        background-color: #000;
        color: #fff;
        border-radius: 5px;
        opacity: 0;
        visibility: hidden;
        transition: opacity 0.2s ease-in-out;
      }

      &:hover::after {
        opacity: 1;
        visibility: visible;
      }
    }
  }

  h3,
  .button .text {
    opacity: 1;
  }

  .button {
    .material-symbols-outlined {
      margin-right: 1rem;
    }
  }

  .footer {
    opacity: 0;
  }
}

.menu {
  align-content: center;
  border-right: 3px solid transparent; /* Initially transparent border */
}

/* Add style for the active menu item */
.menu .button.active {
  border-right-color: var(--primary); /* Set border color on active state */
}
.menu {
  align-content: center;
}

.menu .button {
  display: flex;
  align-items: center;
  text-decoration: none;
  padding: 1rem 1rem;
  position: relative; /* Add position relative to allow absolute positioning of the tooltip */

  .fa-solid {
    font-size: 1.5rem;
    color: #000;
    transition: 0.2s ease-in-out;
    margin-left: 2px;
  }

  &:hover {
    background-color: var(--dark-alt);
  }

  // Add style for the active menu item
  &.active {
    border-right: 3px solid var(--primary);
  }
}
.menu .button::after {
  content: attr(data-tooltip);
  position: absolute;
  top: 50%;
  left: calc(100% + 10px);
  transform: translateY(-50%);
  padding: 0.5rem;
  background-color: #000;
  color: #fff;
  border-radius: 5px;
  opacity: 0;
  visibility: hidden;
  transition: opacity 0.2s ease-in-out;
}

.menu .button:hover::after {
  opacity: 1;
  visibility: visible;
}
.menu .button .fa-solid {
  font-size: 1.5rem;
  color: #000; /* Ubah warna menjadi hitam */
  transition: 0.2s ease-in-out;
  margin-left: 2px;
}
</style>
