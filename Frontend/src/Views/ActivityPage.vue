<template>
  <div class="container">
    <div class="heading">
      <h5>Monitoring<span class="span">ing</span></h5>
      <div class="dropdown">
        <button
          class="btn dropdown-toggle"
          type="button"
          id="robotDropdown"
          href="#"
          role="button"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
          @click="toggleDropdown"
        ></button>
        <div
          class="dropdown-menu robot-dropdown-menu"
          aria-labelledby="robotDropdown"
          :class="{ show: isDropdownOpen }"
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
    </div>
    <div class="card bg-light-subtle" v-if="selectedRobots.length">
      <div class="card-body">
        <div
          v-for="robot in selectedRobots"
          :key="robot"
          class="hover-card"
          @mouseover="expandCard(robot)"
          @mouseleave="resetCard"
        >
          <div v-if="robot">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Robot {{ robot }}</h5>
                <ul>
                  <li
                    v-for="activity in getActivities(robot)"
                    :key="activity.id"
                  >
                    {{ activity.description }}
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="centered-content" v-if="!selectedRobots.length">
      <img
        class="robot-image"
        src="../assets/image/roboterror.png"
        alt="Robot Image"
      />
      <h5 class="message">Please select a robot to see activities</h5>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const robotOptions = ref([]);
const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};

const selectedRobots = ref([]);
let isDropdownOpen = false;

const expandCard = (robot) => {
  const hoverCard = document.querySelector(
    `.hover-card[data-robot="${robot}"]`
  );
  if (hoverCard) {
    const card = hoverCard.querySelector(".card");
    if (card) {
      card.style.height = "auto"; // Set height to auto on hover
    }
  }
};

const resetCard = () => {
  const cards = document.querySelectorAll(".hover-card .card");
  cards.forEach((card) => {
    card.style.height = ""; // Reset height to default on mouse leave
  });
};

const toggleDropdown = () => {
  isDropdownOpen = !isDropdownOpen;
};

const selectRobot = (robot) => {
  selectedRobots.value.push(robot);
  isDropdownOpen = false;
};

const getActivities = (robotName) => {
  const robot = robots.value.find((r) => r.name === robotName);
  return robot ? robot.activities : [];
};

const robots = ref([
  {
    id: 1,
    name: "Robot 1",
    activities: [
      { id: 1, description: "Activity 1 for Robot 1" },
      { id: 2, description: "Activity 2 for Robot 1" },
      { id: 3, description: "Activity 3 for Robot 1" },
    ],
  },
  {
    id: 2,
    name: "Robot 2",
    activities: [
      { id: 1, description: "Activity 1 for Robot 2" },
      { id: 2, description: "Activity 2 for Robot 2" },
      { id: 3, description: "Activity 3 for Robot 2" },
    ],
  },
]);

onMounted(() => {
  fetchRobots();
});
</script>

<style scoped>
.container {
  font-family: "Poppins", sans-serif;
}
.card {
  width: 100%;
  margin-top: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: height 0.3s ease; /* Transisi untuk perubahan tinggi kartu */
}

.card-body {
  padding: px;
}

.card-title {
  margin-bottom: 4px;
  font-size: 15px;
  color: #000;
}

.centered-content {
  text-align: center;
  margin-top: 20px;
}

.robot-image {
  width: 200px;
  height: auto;
  margin-bottom: 20px;
  filter: drop-shadow(2px 4px 6px rgba(0, 0, 0, 0.5));
}

.message {
  font-size: 20px;
  font-family: "Poppins", sans-serif;
}

.heading {
  display: flex;
  align-items: center;
}

h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
  margin-right: 10px; /* Tambahkan margin kanan agar tidak terlalu dekat dengan dropdown */
}

.span {
  font-size: 25px;
  font-weight: 700;
  color: #000;
}

.dropdown {
  position: relative;
  margin-top: 20px; /* Sesuaikan margin atas sesuai kebutuhan */
}

.dropdown-toggle {
  background-color: transparent;
  color: #000;
  border: none;
  padding: 0.5rem 1rem;
  cursor: pointer;
  border-radius: 0.25rem;
  outline: none;
}

.dropdown-menu {
  position: absolute;
  background-color: #fff;
  border: 1px solid rgba(0, 0, 0, 0.125);
  list-style: none;
  padding: 0;
  margin: 0;
  display: none;
}

.dropdown-menu.show {
  display: block;
}

.dropdown-menu li {
  padding: 0.5rem 1rem;
}

.dropdown-menu a {
  text-decoration: none;
  color: #000;
  cursor: pointer;
}

.dropdown-menu a:hover {
  background-color: #f8f9fa;
}
.hover-card {
  position: relative;
}

.hover-card:hover .card {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Efek bayangan saat hover */
}
</style>
