<template>
  <div>
    <header class="navbar navbar-expand-lg bg-white navbar-light">
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
                <span class="material-symbols-outlined">
                  {{ isPlaying ? "pause" : "play_arrow" }}
                </span>
              </button>
            </li>
            <form class="form-inline my-2 my-lg-0">
              <div class="scrolling-text-container" v-if="isPlaying">
                <div class="scrolling-text">{{ scrollingText }}</div>
              </div>
              <input
                class="form-control"
                type="text"
                placeholder="…"
                readonly
              />
            </form>

            <li class="nav-item dropdown" id="robot">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="robotDropdown"
                role="button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <span class="material-symbols-outlined">robot</span>
              </a>
              <div class="dropdown-menu" aria-labelledby="robotDropdown">
                <!-- Add your robot dropdown content here -->
                <a class="dropdown-item" href="#">Robot 1</a>
                <a class="dropdown-item" href="#">Robot 2</a>
                <a class="dropdown-item" href="#">Robot 3</a>
              </div>
            </li>

            <li class="nav-item dropdown" id="joystick">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="joystickDropdown"
                role="button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <span class="material-symbols-outlined">joystick</span>
              </a>
              <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                <!-- Add your joystick dropdown content here -->
                <div class="joy-stick">
                  <button class="btn btn-danger">STOP</button>
                </div>
              </div>
            </li>

            <li class="nav-item dropdown" id="notif">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="notificationsDropdown"
                role="button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <span class="material-symbols-outlined">notifications</span>
              </a>
              <div
                class="dropdown-menu"
                aria-labelledby="notificationsDropdown"
              >
                <!-- Add your notifications dropdown content here -->
                <a class="dropdown-item" href="#">Notification 1</a>
                <a class="dropdown-item" href="#">Notification 2</a>
                <a class="dropdown-item" href="#">Notification 3</a>
              </div>
            </li>
          </div>
        </ul>
      </div>
    </header>

    <nav :style="{ width: is_expanded ? 'var(--sidebar-width)' : '30px' }">
      <div class="menu-toggle-wrap">
        <button class="menu-toggle" @click="toggleMenu">
          <span class="material-symbols-outlined">
            {{
              is_expanded
                ? "keyboard_double_arrow_left"
                : "keyboard_double_arrow_right"
            }}
          </span>
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
import { ref, onMounted, watch, toRefs } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const isSubMenuActive = (item) => {
  const routeName = router.currentRoute.value.name;
  return routeName && routeName.toLowerCase() === item.toLowerCase();
};
const { is_expanded, pageTitle, subMenu } = toRefs({
  is_expanded: ref(localStorage.getItem("is_expanded") === "true"),
  pageTitle: ref(""),
  subMenu: ref([]), // Use an array to store multiple submenus
});

const toggleMenu = () => {
  is_expanded.value = !is_expanded.value;
  localStorage.setItem("is_expanded", is_expanded.value);
};

onMounted(() => {
  // Set initial page title and submenu based on the current route
  updatePageTitle();
});

const updatePageTitle = () => {
  const routeName = router.currentRoute.value.name;
  if (routeName === "Dashboard") {
    pageTitle.value = "Dashboard";
    subMenu.value = ["Dashboard"];
  }
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
  } else if (
    routeName === "Setup" ||
    routeName === "Maps" ||
    routeName === "Path" ||
    routeName === "Mission" ||
    routeName === "Footprint" ||
    routeName === "Modul" ||
    routeName === "User"
  ) {
    pageTitle.value = "Setup";
    subMenu.value = commonSubMenu;
  } else if (
    routeName === "Monitoring" ||
    routeName === "System" ||
    routeName === "Help"
  ) {
    pageTitle.value = routeName;
    subMenu.value = [""];
  }
};

watch(
  () => router.currentRoute.value.name,
  () => {
    // Watch for changes in the route and update the page title and submenu accordingly
    updatePageTitle();
  }
);
const scrollingText = ref(""); // bind scrollingText to the input field
const isPlaying = ref(false); // track play/pause state

const togglePlayPause = () => {
  isPlaying.value = !isPlaying.value;
  // Implement logic to start/stop scrolling text based on isPlaying value
  if (isPlaying.value) {
    startScrolling();
  } else {
    stopScrolling();
  }
};
</script>

<style lang="scss" scoped>
.dropdown-menu {
  margin-left: -40px;
}
.joy-stick {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-family: Arial, sans-serif;
}
.joy-stick .btn {
  width: auto;
  background-color: #ff0000;
  height: 40px;
}
button {
  border: hidden;
}
input {
  height: 20px;
  background-color: #d2d2d2;
}
#navbarSupportedContent {
  gap: 40px;
  margin-left: -200px;
  margin-top: 10px;
}
.submenu-item.active-submenu {
  h5 {
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

.nav1 .nav-item {
  font-size: 30px;
  margin-right: 550px;
}
header {
  margin-top: -10px;
  position: fixed;
  top: 0;
  left: 60;
  width: 100%;
  z-index: 1000;

  padding: 10px; /* Adjust the padding as needed */
  border-bottom: #000000 solid 1px;
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

.menu-toggle {
  background-color: #ffffff;
  color: #000000;
  border: none;
  padding: 10px;
  cursor: pointer;
}

.is-expanded nav {
  .menu-toggle-wrap {
    top: -3rem;

    .menu-toggle {
      transform: rotate(-180deg);
    }
  }
}

.is-expanded .menu-toggle {
  background-color: #fff;
  color: #fffdfd;
}

button {
  background-color: #fff;
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
  margin-left: 20px;
  margin-top: 10px;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
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
</style>
