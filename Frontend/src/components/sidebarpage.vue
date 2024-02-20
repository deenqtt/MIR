<template>
  <aside
    :class="`${is_expanded ? 'is-expanded' : ''} ${
      is_mobile ? 'is-mobile' : ''
    }`"
  >
    <br /><br />
    <div class="menu">
      <div class="menu1">
        <router-link
          to="/dashboard"
          class="button"
          :class="{ active: $route.path === '/dashboard' }"
        >
          <span
            class="fa-solid fa-house"
            data-toggle="tooltip"
            data-bs-placement="right"
            title="Dashboard"
          ></span>
        </router-link>
      </div>
      <router-link
        to="/maps"
        class="button"
        :class="{ active: $route.path === '/maps' }"
      >
        <span
          class="fa-solid fa-sliders"
          data-toggle="tooltip"
          data-bs-placement="right"
          title="Setup"
        ></span>
      </router-link>
      <!-- Sisipkan event mouseenter dan mouseleave untuk menampilkan dan menyembunyikan tooltip -->
      <router-link
        to="/monitoring"
        class="button"
        :class="{ active: $route.path === '/monitoring' }"
      >
        <span
          class="fa-solid fa-chart-simple"
          data-toggle="tooltip"
          data-bs-placement="right"
          title="Monitoring"
        ></span>
      </router-link>
      <router-link
        to="/system"
        class="button"
        :class="{ active: $route.path === '/system' }"
        v-bind:data-tooltip="'System'"
        @mouseenter="tooltip.show = true"
        @mouseleave="tooltip.show = false"
      >
        <span
          class="fa-solid fa-gears"
          data-toggle="tooltip"
          data-bs-placement="right"
          title="System"
        ></span>
      </router-link>
    </div>

    <div class="flex"></div>

    <div class="menu">
      <router-link
        to="/help"
        class="button"
        :class="{ active: $route.path === '/help' }"
      >
        <span
          class="fa-solid fa-circle-question"
          data-toggle="tooltip"
          data-bs-placement="right"
          title="Help"
        ></span>
      </router-link>
      <button class="tombol" @click="logout">
        <span
          class="fa-solid fa-right-from-bracket"
          data-toggle="tooltip"
          data-bs-placement="right"
          title="Logout"
        ></span>
      </button>
    </div>
  </aside>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from "vue";
import { useRouter } from "vue-router";
const router = useRouter();
const is_expanded = ref(false);
const is_mobile = ref(false);
// Tambahkan konfigurasi untuk tooltip menggunakan Alpine.js
const tooltip = ref({
  show: false,
});

const checkWindowSize = () => {
  is_mobile.value = window.innerWidth <= 660;
};

const ToggleMenu = () => {
  is_expanded.value = !is_expanded.value;
};

const logout = () => {
  localStorage.setItem("authenticated", false);
  router.push({ name: "Login" });
};

const onButtonHover = () => {
  console.log("Button hovered");
};

onMounted(() => {
  $(document).ready(function () {
    $("#element").tooltip("update");
    $('[data-toggle="tooltip"]').tooltip();
  });
  checkWindowSize();
  window.addEventListener("resize", checkWindowSize);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", checkWindowSize);
});
</script>

<style lang="scss" scoped>
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

/* Styling for mobile view */
.is-mobile {
  width: 30px;
  height: 100%;
  z-index: 100;
  overflow-y: auto;

  .menu {
    display: none;
    flex-direction: column;
  }

  .button {
    border-right: none !important;
  }

  h3,
  .text {
    display: inline-block;
    margin-left: 1rem;
  }
}

aside {
  display: flex;
  flex-direction: column;

  background-color: #000000;

  width: calc(2rem + 32px);
  overflow: hidden;
  min-height: 100vh;
  padding: 1rem;

  transition: 0.2s ease-in-out;

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
