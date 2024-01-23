<script>
import Sidebar from "./components/sidebarpage.vue";
import SecondSide from "./components/secondside.vue";

export default {
  name: "App",
  components: {
    Sidebar,
    SecondSide,
  },
  computed: {
    showSidebar() {
      const exemptedPages = ["/", "/authentication"];
      const isExemptedPage = exemptedPages.includes(this.$route.path);

      return !isExemptedPage;
    },
  },
};
</script>

<template>
  <div class="app">
    <!-- sidebar -->
    <Sidebar v-if="showSidebar" />
    <SecondSide v-if="showSidebar" />

    <main>
      <!-- Content -->

      <router-view />
    </main>
  </div>
</template>

<style lang="scss" scoped>
:root {
  --primary: #4ade80;
  --primary-alt: #22c55e;
  --grey: #64748b;
  --dark: #1e293b;
  --dark-alt: #334155;
  --light: #f1f5f9;
  --sidebar-width: 800px;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: "Fira sans", sans-serif;
}

body {
  background: var(--light);
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
