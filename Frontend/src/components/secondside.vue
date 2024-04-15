<template>
  <div>
    <header class="navbar navbar-expand-lg bg-light">
      <div id="navigation" class="collapse navbar-collapse">
        <ul class="navbar-nav">
          <div class="nav1">
            <li class="nav-item">
              <i class="navbar-brand" href="#">GSPE Company</i>
            </li>
          </div>
          <div class="form-check form-switch">
            <input
              class="form-check-input"
              type="checkbox"
              id="flexSwitchCheckDefault"
            v-model="mqttConnected" disabled
            />
        <p>MQTT ({{ mqttStatus }})</p>
            <div id="history-list"></div>
          </div>

          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <li class="nav-item">
              <button class="play-pause-btn" @click="togglePlayPause">
                <span
                  class="fa-solid"
                  data-toggle="tooltip"
                  data-bs-placement="bottom"
                  title="Start/Pause?"
                >
                  <i v-if="isPlaying" class="fa-solid fa-pause"></i>
                  <i v-else class="fa-solid fa-play"></i>
                </span>
              </button>
            </li>

            <select
              class="form-control form-control-sm"
              v-model="selectedMission"
              id="missionSelect"
            >
              <option v-for="name in missionOptions" :key="name" :value="name">
                {{ name }}
              </option>
            </select>
            <div class="dropdown">
              <a
                class="nav-link"
                href="#"
                id="robotDropdown"
                role="button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <span
                  class="fa-solid fa-robot"
                  data-toggle="tooltip"
                  data-bs-placement="right"
                  title="Select Robot"
                ></span>
              </a>
              <div
                class="dropdown-menu robot-dropdown-menu"
                aria-labelledby="robotDropdown"
              >
                <a
                  class="dropdown-item"
                  v-for="robot in robotOptions"
                  :key="robot"
                  @click="selectRobot(robot)"
                >
                  {{ robot }}
                </a>
              </div>
            </div>

            <li class="nav-item dropdown" id="joystick">
              <a
                class="nav-link"
                href="#"
                id="joystickDropdown"
                role="button"
                :aria-expanded="dropdownOpen ? 'true' : 'false'"
                @click="toggleDropdownJosytick"
              >
                <span
                  class="fa-solid fa-gamepad"
                  data-toggle="tooltip"
                  data-bs-placement="right"
                  title="Joystick"
                ></span>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="joystickDropdown"
                :class="{ show: dropdownOpen1 }"
                style="
                  width: 300px;
                  margin-right: 100px;
                  height: 200px;
                  overflow: auto;
                "
              >
                <div class="controller">
                  <button @click="move('up')" class="btn up">Up</button>
                  <div>
                    <button @click="move('left')" class="btn left">Left</button>
                    <button @click="move('right')" class="btn right">
                      Right
                    </button>
                  </div>
                  <button @click="move('down')" class="btn down">Down</button>
                  <button @click="clickAction" class="btn click">Click</button>
                </div>
              </div>
            </li>

            <!-- Elemen notifikasi -->
            <li class="nav-item dropdown" id="notif">
              <a
                class="nav-link notif"
                href="#"
                id="notificationsDropdown"
                role="button"
                :aria-expanded="dropdownOpen ? 'true' : 'false'"
                @click="toggleDropdown"
              >
                <div class="notification-container">
                  <span
                    class="fa-solid fa-bell"
                    id="notificationIcon"
                    data-toggle="tooltip"
                    data-bs-placement="right"
                    title="Notification"
                    @click.stop="toggleDropdown"
                  ></span>

                  <!-- Menampilkan titik hijau hanya jika ada notifikasi yang belum dibaca -->
                  <span
                    class="notification-dot"
                    v-if="hasUnreadNotifications && notifications.length > 0"
                  ></span>
                </div>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="notificationsDropdown"
                :class="{ show: dropdownOpen }"
              >
                <!-- Menampilkan notifikasi dari state Vuex -->
                <a
                  v-for="(notification, index) in notifications"
                  :key="index"
                  class="dropdown-item"
                  href="#"
                  @click="removeNotifications(index)"
                >
                  {{ notification }}
                </a>
              </div>
            </li>
          </div>
        </ul>
      </div>
    </header>

    <nav
      :style="{ width: is_expanded ? 'var(--sidebar-width)' : '30px' }"
      id="sidenav-5"
      data-te-sidenav-init
      data-te-sidenav-hidden="false"
      data-te-sidenav-accordion="true"
    >
      <div class="menu-toggle-wrap" style="background-color: #fff">
        <button @click="toggleMenu">
          <span
            :class="[
              'fa-solid',
              is_expanded ? 'fa-angles-right' : 'fa-angles-left',
            ]"
          ></span>
        </button>
      </div>

      <div class="sidebar-content">
        <h2>{{ pageTitle }}</h2>
        <h5 v-if="subMenu.length > 0"></h5>

        <router-link
          v-for="(item, index) in subMenu"
          :key="index"
          :to="`/${item.toLowerCase()}`"
          class="submenu-item"
          :class="{ 'active-submenu': isSubMenuActive(item) }"
        >
          <h5>{{ item }}</h5>
        </router-link>
      </div>
    </nav>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, watch, onMounted, toRefs, computed, reactive } from "vue";
import { useRouter } from "vue-router";
import { useStore } from "vuex";

// Inisialisasi variabel untuk menyimpan status koneksi MQTT
const mqttConnected = ref(false);
const mqttStatus = ref('Connecting...');
// Fungsi untuk melakukan permintaan ke API Python
const checkMQTTConnection = async () => {
  try {
    const response = await axios.get('http://localhost:5000/check_mqtt_connection'); // Ganti URL dengan URL API Python Anda
    mqttConnected.value = response.data.connected;
      mqttStatus.value = response.data.connected ? 'Connected' : 'Disconnected';
  } catch (error) {
    console.error('Error checking MQTT connection:', error);
  }
};

// Ketika komponen dimuat, lakukan permintaan ke API untuk memeriksa koneksi MQTT
onMounted(() => {
  checkMQTTConnection();
    setInterval(checkMQTTConnection, 5000);
});


const missionOptions = ref([]);
const activeSubMenu = ref("");
const store = useStore();
const selectedMission = ref(null);
const selectedRobot = ref(null);
const robotOptions = ref([]);
const filteredMissions = ref([]);
const isDropdownVisible = ref(false);
const hoveredJoystick = ref(false);
const hoveredNotif = ref(false);
const dropdownOpen = ref(false);
const dropdownOpen1 = ref(false);

const removeNotifications = (notificationIndex) => {
  // Remove the clicked notification from the list
  store.dispatch("removeNotification", notificationIndex);
};

// Dapatkan nilai boolean apakah ada notifikasi yang belum dibaca
const hasUnreadNotifications = computed(() => {
  return store.state.unreadNotifications > 0;
});
const move = (direction) => {
  console.log(`Move to ${direction}`);
};
const markNotificationsAsReadAndRemoveLocalStorage = () => {
  // Mark all notifications as read in Vuex store
  store.state.notifications.forEach((notification) => {
    notification.read = true;
  });

  // Remove the flag indicating unread notifications from local storage
  localStorage.removeItem("hasUnreadNotifications");
};
const addNotification = (notification) => {
  // Menambahkan notifikasi ke dalam array notifications
  store.commit("addNotification", notification);

  // Set a flag in local storage to indicate that there are unread notifications
  localStorage.setItem("hasUnreadNotifications", true);
};

const notifications = computed(() => {
  return store.state.notifications;
});

const router = useRouter();

const toggleDropdown = () => {
  dropdownOpen.value = !dropdownOpen.value;
};

const toggleDropdownJosytick = () => {
  dropdownOpen1.value = !dropdownOpen1.value;
};
const selectRobot = async (robotName) => {
  store.commit("setSelectedRobot", robotName);
  selectedRobot.value = robotName;

  try {
    console.log(`Selected Robot: ${robotName}`);
    await fetchMissions(selectedRobot.value);
  } catch (error) {
    console.error(`Error fetching missions for ${robotName}:`, error);
  }
};

const fetchMissions = async (robot) => {
  try {
    const response = await axios.get(
      `http://localhost:5258/missions/filtered-by-robot?robot=${robot}`
    );

    console.log("API Response:", response.data);

    // Ensure that response.data is not undefined before attempting to filter
    const missions = response.data ?? [];

    console.log(`Missions for ${robot}:`, missions);

    // Filter missions based on the selected robot and exclude missions with ID 0
    const filteredMissions = missions.filter(
      (mission) => mission.robot === robot && mission.id !== 0
    );

    console.log(`Filtered Missions for ${robot}:`, filteredMissions);

    missionOptions.value = filteredMissions.map((mission) => mission.name); // Use the correct property name for mission name
  } catch (error) {
    console.error(`Error fetching missions for ${robot}:`, error);
  }
};

const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};

watch(selectedRobot, () => {
  console.log("Selected Robot changed, fetching filtered missions...");
  // You can fetch the filtered missions here or update as needed
});

const isSubMenuActive = (item) => {
  const routeName = router.currentRoute.value.name;
  const isActive = routeName && routeName.toLowerCase() === item.toLowerCase();
  if (isActive) {
    activeSubMenu.value = item;
  }
  console.log(`Route: ${routeName}, Active Submenu: ${activeSubMenu.value}`);
  return isActive;
};

// Deklarasi state
const state = reactive({
  is_expanded: localStorage.getItem("is_expanded") === "true",
  pageTitle: "",
  subMenu: [],
});

// Mendapatkan referensi ke variabel dari state
const { is_expanded, pageTitle, subMenu } = toRefs(state);
const toggleMenu = () => {
  is_expanded.value = !is_expanded.value;
  localStorage.setItem("is_expanded", is_expanded.value);
};

const updatePageTitle = () => {
  const routeName = router.currentRoute.value.name;
  const commonSubMenu = [
    "Add Robot",
    "Maps",
    "Path",
    "Mission",
    "Footprint",
    "Modul",
    "User",
  ];

  if (routeName === "Dashboard") {
    pageTitle.value = "Dashboard";
    subMenu.value = ["Dashboard"];
  } else if (routeName === "Setup" || commonSubMenu.includes(routeName)) {
    pageTitle.value = "Setup";
    subMenu.value = commonSubMenu;
  } else if (routeName === "Activity") {
    pageTitle.value = "Monitoring";
    subMenu.value = ["Activity", "Error Log"];
  } else if (routeName === "Settings") {
    pageTitle.value = "System";
    subMenu.value = ["Settings", "Error"];
  } else if (routeName === "Robot") {
    pageTitle.value = "Robot";
    subMenu.value = ["Robot"];
  }
};

watch(
  () => router.currentRoute.value.name,
  () => {
    updatePageTitle();
  }
);

const isPlaying = ref(false);

const togglePlayPause = () => {
  isPlaying.value = !isPlaying.value;
  if (isPlaying.value) {
    startScrolling();
  } else {
    stopScrolling();
  }
};
onMounted(() => {
  $(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
  });
  console.log("Component mounted");
  updatePageTitle();
  fetchRobots();
  fetchMissions();
});
</script>

<style lang="scss" scoped>
.form-check-input:checked {
  background-color: #28a745; /* Warna hijau saat tombol dicentang (checked) */
  border-color: #28a745; /* Warna garis pinggir hijau */
}

.form-check-input:checked + .form-check-label {
  color: #28a745; /* Warna teks hijau saat tombol dicentang (checked) */
}
.form-check p {
  width: 200px;
}
.form-check {
  margin-left: 20px;
  margin-top: 20px;
  font-family: "Poppins", sans-serif;
  font-weight: 600;
  width: 200px;
}
.submenu-item .fa-submenu {
  margin-right: -10px;
}

.robot-dropdown-menu {
  max-height: 200px; /* Atur ketinggian maksimum dropdown di sini */
  overflow-y: auto; /* Aktifkan scrolling vertikal */
  overflow-x: hidden; /* Sembunyikan scrolling horizontal */
}

/* Styling for dropdown menu items */
/* Keyframes untuk animasi dropdown */
@keyframes dropdownAnimation {
  from {
    transform: translateY(-50%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
/* CSS untuk menampilkan titik biru di atas ikon lonceng (bell) */
.notification-container {
  position: relative;
}

/* CSS untuk menampilkan titik biru di atas ikon lonceng (bell) */
.notification-dot {
  position: absolute;
  width: 10px;
  height: 10px;
  background-color: rgb(0, 255, 51);
  border-radius: 50%;
  top: -5px;
  right: -5px;
}

.notification-container {
  position: relative;
}

/* CSS untuk menampilkan titik biru di atas ikon lonceng (bell) */
.notification-dot {
  position: absolute;
  width: 10px;
  height: 10px;
  background-color: rgb(0, 255, 51);
  border-radius: 50%;
  top: -5px;
  right: -5px;
}

/* Styling for dropdown menu */
.dropdown-menu {
  animation: dropdownAnimation 0.3s ease-out forwards; /* Menggunakan animasi */
}

/* Untuk mengatur posisi dropdown menu */
.dropdown-menu.show {
  transform-origin: top; /* Titik awal transfromasi */
  top: 100%; /* Menggeser dropdown ke bawah */
}
#notif .dropdown-menu {
  margin-left: -70px;
}
.dropdown-menu {
  margin-left: -55px;
  box-shadow: 1px 1px 2px #000;
}

.controller {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.controller .btn {
  margin: 10px;
  padding: 10px 20px;
  font-size: 12px;
  border: none;
  cursor: pointer;
}

.controller .up {
  background-color: #237ab5;
  color: #ffffff;
  width: auto;
}

.controller .left {
  background-color: #e74c3c;
  color: #ffffff;
  width: auto;
}

.controller .right {
  background-color: #2ecc71;
  color: #ffffff;
  width: auto;
}

.controller .down {
  background-color: #ec9d1e;
  color: #ffffff;
  width: auto;
}

.controller .click {
  background-color: #9b59b6;
  color: #ffffff;
  width: auto;
}

.form-control {
  width: 180px;
  height: 20px;
}
button {
  border: hidden;
}
input {
  height: 20px;
  background-color: #d2d2d2;
}
#navbarSupportedContent {
  gap: 60px;
  margin-left: 100px;
  margin-top: 10px;
}
.submenu-item.active-submenu {
  h5 {
    position: relative;
    margin-top: 0;
    padding: 5px 20px;
    background-color: #f0f0f0;
    color: #000000;

    &:before,
    &:after {
      content: "";
      position: absolute;
      left: 0;
      right: 0;
      height: 1px;
      background-color: #000000;
    }

    &:before {
      top: -1px;
    }

    &:after {
      bottom: -1px;
    }
  }
}

.fa-solid {
  font-size: 18px;
  color: #000000;
}

// Tambahkan aturan ini untuk menyimpan garis di atas dan bawah submenu saat submenu aktif
.submenu-item.active-submenu:first-child h5:before,
.submenu-item.active-submenu:last-child h5:after {
  content: "";
  position: absolute;
  left: 0;
  right: 0;
  height: 1px;
  background-color: #000000;
}

.submenu-item.active-submenu:first-child h5:before {
  top: -1px;
}

.submenu-item.active-submenu:last-child h5:after {
  bottom: -1px;
}
.submenu-item {
  text-decoration: none;
  color: #000000;
  position: relative;

  h5 {
    position: relative;
    margin-top: 0; /* Remove the margin-top */
    padding: 5px 20px; /* Adjust the padding as needed */

    &:hover {
      &:before,
      &:after {
        content: "";
        position: absolute;
        left: 0;
        right: 0;
        height: 1px;
        background-color: #000000; // Set the color for the underline
      }

      &:before {
        top: -1px;
      }

      &:after {
        bottom: -1px;
      }
    }
  }
}

.navbar .nav2 .nav-item {
  margin-left: -50px;
  margin-right: 25px;
}
.navbar .nav3 .nav-item {
  margin-right: 20px;
}
.fa-solid .nav1 .nav-item {
  font-size: 30px;
  margin-right: 20px;
}

header {
  margin-top: -10px;
  position: fixed;
  top: 0;
  left: 15;
  width: 100%;
  z-index: 1000;
  padding: 10px;
  border-bottom: 1px solid #000000; /* Tambahkan garis bawah di sini */
}

nav {
  margin-top: 50px;
  display: flex;
  flex-direction: column;
  background-color: #ffffff;
  overflow: hidden;
  padding: 1rem;
  width: var(--sidebar-width);
  z-index: 899;
  .menu-toggle-wrap {
    margin-left: -50px;
    display: flex;
    justify-content: flex-end;
    margin-bottom: 1rem;
    position: relative;
    top: 0;
    transition: 0.8s ease-in-out;

    .menu-toggle {
      transition: 0.8s ease-in-out;
    }
  }
}

.menu-toggle-wrap {
  display: flex;
  justify-content: center;
}

.is-expanded nav {
  .menu-toggle-wrap {
    top: -3rem;

    .menu-toggle {
      transform: rotate(-180deg);
    }
  }
}

button {
  background-color: #f8f8f8;
  margin-top: 1px;
  width: 20px;
  height: 50px;
}
h2 {
  font-family: "Poppins", sans-serif;
  font-size: 20px;
  margin-left: 30px;
  font-weight: 700;
  margin-top: -20px;
}
.navbar-brand {
  margin-left: 50px;
  margin-top: 15px;
  font-family: "Poppins", sans-serif;
  font-weight: 700;
  color: #000000;
}
h5 {
  font-size: 14px;
  font-family: "Poppins", sans-serif;
  font-weight: 300;
  margin-left: 20px;
  padding: 20px;
  margin-top: -30px;
  margin-bottom: 20px;
}
.submenu-item {
  text-decoration: none;
  color: #000000;
}
.menu-toggle-wrap button {
  background: #ffffff;
}
</style>
