<template>
  <div class="app" :class="{ 'with-sidebar': showSidebar }">
    <!-- sidebar -->
    <Sidebar v-if="showSidebar" />
    <SecondSide v-if="showSidebar" />
    <main :style="{ background: isExemptedPage ? '100%' : 'auto' }">
      <!-- Content -->
      <router-view />
    </main>
  </div>
</template>

<script setup>
import Sidebar from "./components/sidebarpage.vue";
import SecondSide from "./components/secondside.vue";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const showSidebar = computed(() => {
  const exemptedPages = ["/", "/auth/register"];
  return !exemptedPages.includes(router.currentRoute.value.path);
});

const isExemptedPage = computed(() => {
  const exemptedPages = ["/", "/auth/register"];
  return exemptedPages.includes(router.currentRoute.value.path);
});

const height = ref("auto"); // define the height property
</script>

<style lang="scss" scoped>
:root {
  --primary: #4ade80;
  --primary-alt: #22c55e;
  --grey: #64748b;
  --dark: #1e293b;
  --dark-alt: #334155;
  --light: #ffffff;
  --sidebar-width: 800px;
}

* {
  margin: 0;
  padding: 0;

  font-family: "Fira sans", sans-serif;
}

main {
  background: #ffffff;
}

button {
  cursor: pointer;
  appearance: none;
  border: none;
  outline: none;
  background: none;
}

.app {
  display: flex;

  main {
    margin-top: 50px;
    flex: 1 1 0;
    padding: 2rem;

    @media (max-width: 1024px) {
      padding-left: 6rem;
    }
  }
}
</style>
