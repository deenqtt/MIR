<template>
  <div class="container">
    <router-link
      v-for="card in cards"
      :to="`/${card.title}`"
      class="card"
      :key="card.title"
    >
      <div
        class="card-body"
        @mouseenter="showIcon(card)"
        @mouseleave="hideIcon(card)"
        @click="redirectTo(card.title)"
      >
        <p v-if="!card.hover">{{ card.title }}</p>
        <i v-if="card.hover" :class="card.icon"></i>
      </div>
    </router-link>
  </div>
</template>

<script setup>
import { ref } from "vue";

const cards = ref([
  { title: "System", hover: false, icon: "fas fa-cogs" },
  { title: "Lorem", hover: false, icon: "fas fa-globe" },
  { title: "Lorem", hover: false, icon: "fas fa-chart-bar" },
  { title: "Lorem", hover: false, icon: "fas fa-envelope" },
  { title: "Another Page", hover: false, icon: "fas fa-info-circle" },
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
  box-shadow: 1px 1px 5px rgba(0, 0, 0, 0.3);
  border-radius: 8px;
  cursor: pointer;
  position: relative;
  transition: transform 0.3s ease;
}
.card:hover {
  transform: scale(1.05); /* Scale up on hover */
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
  background-color: rgba(
    255,
    255,
    255,
    0.7
  ); /* Semi-transparent background to make the text more visible */
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
}
.card-body i {
  opacity: 0;
  transition: opacity 0.3s ease;
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
