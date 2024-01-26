<template>
  <aside
    :class="`${is_expanded ? 'is-expanded' : ''} ${
      is_mobile ? 'is-mobile' : ''
    }`"
  >
    <h3>Menu</h3>
    <div class="menu">
      <div class="menu1">
        <router-link to="/dashboard" class="button">
          <span class="material-symbols-outlined">dashboard</span>
        </router-link>
      </div>
      <router-link to="/maps" class="button">
        <span class="material-symbols-outlined">tune</span>
      </router-link>
      <router-link to="/monitoring" class="button">
        <span class="material-symbols-outlined">monitoring</span>
      </router-link>
      <router-link to="/system" class="button">
        <span class="material-symbols-outlined">settings</span>
      </router-link>
    </div>

    <div class="flex"></div>

    <div class="menu">
      <router-link to="/robot" class="button">
        <span class="material-symbols-outlined">help</span>
      </router-link>
      <button class="tombol">
        <span class="material-symbols-outlined">logout</span>
      </button>
    </div>
  </aside>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from "vue";

const is_expanded = ref(false);
const is_mobile = ref(false);

const checkWindowSize = () => {
  is_mobile.value = window.innerWidth <= 1024;
};

const ToggleMenu = () => {
  is_expanded.value = !is_expanded.value;
};

onMounted(() => {
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
  margin-left: 4px;
  background: none;
  color: #fff;
  border: none;
  display: flex;
  align-items: center;
  padding: 1rem 1rem;
  font-size: 2rem;
}
.menu-toggle-wrap {
  display: none; /* hide the menu toggle for larger screens */
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

  background-color: var(--dark);

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

      padding: 1rem 1rem; /* Adjust padding for vertical and horizontal spacing */

      .material-symbols-outlined {
        font-size: 2rem;
        color: var(--light);
        transition: 0.2s ease-in-out;
        margin-right: 1rem;
      }

      .text {
        color: var(--light);
        transition: 0.2s ease-in-out;
      }

      &:hover {
        background-color: var(--dark-alt);
      }

      &.router-link-exact-active {
        border-right: 3px solid var(--primary);

        .material-icons,
        .text {
          color: var(--primary);
        }
      }
    }
  }

  &.is-expanded {
    width: var(--sidebar-width);

    .menu-toggle-wrap {
      top: -3rem;

      .menu-toggle {
        transform: rotate(-180deg);
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
}
.menu {
  align-content: center;
  border-right: 3px solid transparent; /* Initially transparent border */
}

router-link-exact-active {
  border-right-color: var(--primary); /* Set border color on active state */
}
</style>
