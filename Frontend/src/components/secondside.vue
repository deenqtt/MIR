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
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <li class="nav-item">
              <button class="play-pause-btn" @click="togglePlayPause">
                <span
                  class="fa-solid"
                  data-toggle="tooltip"
                  data-placement="bottom"
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
                  data-bs-placement="bottom"
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
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <span
                  class="fa-solid fa-gamepad"
                  data-toggle="tooltip"
                  data-bs-placement="bottom"
                  title="Joystick"
                ></span>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="joystickDropdown"
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
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
                @click="markNotificationsAsReadAndRemoveLocalStorage"
              >
                <div class="notification-container">
                  <span
                    class="fa-solid fa-bell"
                    id="notificationIcon"
                    data-toggle="tooltip"
                    data-bs-placement="bottom"
                    title="Notification"
                    @click="markNotificationsAsReadAndRemoveLocalStorage"
                  ></span>
                  <!-- Menampilkan titik hijau hanya jika ada notifikasi yang belum dibaca -->
                  <span
                    class="notification-dot"
                    v-if="hasUnreadNotifications"
                  ></span>
                </div>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="notificationsDropdown"
              >
                <!-- Menampilkan notifikasi dari state Vuex -->
                <a
                  v-for="notification in notifications"
                  :key="notification.id"
                  class="dropdown-item"
                  href="#"
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
      class="fixed left-15 top-0 z-[1035] h-screen w-60 -translate-x-full overflow-hidden bg-zinc-800 shadow-[0_4px_12px_0_rgba(0,0,0,0.07),_0_2px_4px_rgba(0,0,0,0.05)] data-[te-sidenav-hidden='false']:translate-x-0bg-light shadow-[0_4px_12px_0_rgba(0,0,0,0.07),_0_2px_4px_rgba(0,0,0,0.05)] data-[te-sidenav-hidden='false']:translate-x-0 dark:bg-zinc-800"
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
import { ref, onMounted, watch, toRefs, computed } from "vue";
import { useRouter } from "vue-router";
import { useStore } from "vuex";

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

// Menggunakan computed untuk mengecek apakah ada notifikasi yang belum dibaca
const hasUnreadNotifications = computed(() => {
  // Check if there are any unread notifications in the list
  return notifications.value.some((notification) => !notification.read);
});

const addNotification = (notification) => {
  // Menambahkan notifikasi ke dalam array notifications
  store.commit("addNotification", { message: notification, read: false });

  // Set a flag in local storage to indicate that there are unread notifications
  localStorage.setItem("hasUnreadNotifications", true);
};

const notifications = computed(() => {
  return store.state.notifications;
});
const router = useRouter();

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
      `http://localhost:5258/missions?robot=${robot}`
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

const { is_expanded, pageTitle, subMenu } = toRefs({
  is_expanded: ref(localStorage.getItem("is_expanded") === "true"),
  pageTitle: ref(""),
  subMenu: ref([]),
});

const toggleMenu = () => {
  is_expanded.value = !is_expanded.value;
  localStorage.setItem("is_expanded", is_expanded.value);
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

const updatePageTitle = () => {
  const routeName = router.currentRoute.value.name;
  const commonSubMenu = [
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
  } else if (["Monitoring", "System", "Help"].includes(routeName)) {
    pageTitle.value = routeName;
    // Menetapkan submenu sesuai dengan menu yang dipilih
    if (routeName === "Monitoring") {
      subMenu.value = ["Monitor"];
    } else if (routeName === "System") {
      subMenu.value = ["Setting"];
    } else if (routeName === "Help") {
      pageTitle.value = "Help";
      subMenu.value = ["Help", "AddRobot"];
    }
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
</script>

<style lang="scss" scoped>
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
  margin-left: 300px;
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
  z-index: 999; /* Set a high z-index to ensure it appears above other content */

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
