<template>
  <div class="container">
    <div class="bag1">
    <!-- Existing cards -->
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
    <!-- New card for robot information -->
    <div class="card robot-card">
      <div class="card-body">
      
      
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faRobot } from '@fortawesome/free-solid-svg-icons'

const cards = ref([
  {
    title: "Error Handling",
    hover: false,
    icon: "fas fa-cogs",
    // tooltip: "Tooltip for System",
  },
  {
    title: "Battery",
    hover: false,
    icon: "fa-solid fa-battery-quarter",
    // tooltip: "Tooltip for Lorem",
  },
  {
    title: "Docking",
    hover: false,
    icon: "fas fa-chart-bar",
    // tooltip: "Tooltip for Lorem",
  },
  {
    title: "Mqtt",
    hover: false,
    icon: "fas fa-chart-bar",
    // tooltip: "Tooltip for Lorem",
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

const faRobotIcon = faRobot;
</script>

<style scoped>
/* Existing styles */

.bag1 .card {
  background-color: #f7eedd;
}
.bag1 {
  margin-top: 20px;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  gap: 20px;
}

.bag1 .card {
  width: 200px;
  height: 100px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.bag1 .card:hover {
  transform: scale(1.05);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}
.bag1 .card-body {
  display: flex;
  align-items: center;
  justify-content: center;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: #ace2e1;
  transition: background-color 0.3s ease;
}
.bag1 .card:hover .card-body {
  background-color: rgba(255, 255, 255, 0);
}
.bag1 .card-body p {
  margin: 0;
  transition: opacity 0.3s ease;
  font-family: "Poppins", sans-serif;
  color: #333;
}
.bag1 .card-body i {
  opacity: 0;
  transition: opacity 0.3s ease;
  color: #008dda;
}
.bag1 .card:hover .card-body p {
  opacity: 0;
}
.bag1 .card:hover .card-body i {
  opacity: 1;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%) scale(2);
  font-size: 24px;
}

/* New styles for robot card */
.robot-card {
  display: flex;
  margin-top: 50px;
  width: auto; /* Card width */
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Card shadow */
  border-radius: 8px; /* Card border radius */
  cursor: pointer; /* Cursor style */
}

.robot-card .card:hover{
  height: 200px;
}







</style>
