<template>
  <div class="container">
    <router-link
      v-for="card in cards"
      :to="`/${card.title}`"
      class="card"
      :key="card.title"
      data-bs-toggle="tooltip"
      :title="card.tooltip"
    >
      <div
        class="card-body"
        @mouseenter="showIcon(card)"
        @mouseleave="hideIcon(card)"
        @click="redirectTo(card.title)"
      >
        <p v-if="!card.hover" class="card-title">{{ card.title }}</p>
        <i v-if="card.hover" :class="card.icon"></i>
      </div>
    </router-link>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";

const cards = ref([
  {
    title: "System",
    hover: false,
    icon: "fas fa-cogs",
    tooltip: "Tooltip for System",
  },
  {
    title: "Lorem",
    hover: false,
    icon: "fas fa-globe",
    tooltip: "Tooltip for Lorem",
  },
  {
    title: "Lorem",
    hover: false,
    icon: "fas fa-chart-bar",
    tooltip: "Tooltip for Lorem",
  },
  {
    title: "Lorem",
    hover: false,
    icon: "fas fa-envelope",
    tooltip: "Tooltip for Lorem",
  },
  {
    title: "Another Page",
    hover: false,
    icon: "fas fa-info-circle",
    tooltip: "Tooltip for Another Page",
  },
  {
    title: "Card 6",
    hover: false,
    icon: "fas fa-heart",
    tooltip: "Tooltip for Card 6",
  },
  {
    title: "Card 7",
    hover: false,
    icon: "fas fa-star",
    tooltip: "Tooltip for Card 7",
  },
  {
    title: "Card 8",
    hover: false,
    icon: "fas fa-user",
    tooltip: "Tooltip for Card 8",
  },
  {
    title: "Card 9",
    hover: false,
    icon: "fas fa-bell",
    tooltip: "Tooltip for Card 9",
  },
  {
    title: "Card 10",
    hover: false,
    icon: "fas fa-code",
    tooltip: "Tooltip for Card 10",
  },
]);

const showIcon = (card) => {
  card.hover = true;
};

const hideIcon = (card) => {
  card.hover = false;
};

const redirectTo = (title) => {
  // Redirect logic here, for example using Vue Router
  router.push({ path: `/${title}` });
};

onMounted(() => {
  // Activate tooltips
  const tooltipTriggerList = [].slice.call(
    document.querySelectorAll('[data-bs-toggle="tooltip"]')
  );
  const tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
  });
});
</script>

<style scoped>
.container {
  margin-top: 20px;
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}
.card {
  width: 200px;
  height: 100px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.card:hover {
  transform: scale(1.05); /* Scale up on hover */
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}
.card-body {
  display: flex;
  align-items: center;
  justify-content: center;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.95); /* Semi-transparent background */
  transition: background-color 0.3s ease;
}
.card:hover .card-body {
  background-color: rgba(
    255,
    255,
    255,
    0
  ); /* Transparent background on hover */
}
.card-body p {
  margin: 0;
  transition: opacity 0.3s ease;
  font-family: "Poppins", sans-serif;
  color: #333; /* Adjust the color as needed */
}
.card-body i {
  opacity: 0;
  transition: opacity 0.3s ease;
  color: #007bff; /* Adjust the color as needed */
}
.card:hover .card-body p {
  opacity: 0; /* Hide title on hover */
}
.card:hover .card-body i {
  opacity: 1; /* Show icon on hover */
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%) scale(2); /* Scale up icon on hover */
  font-size: 24px;
}
</style>
