<template>
  <div class="container">
    <h5>
      Dash
      <span>boards</span>
    </h5>
    <br />
    <div v-if="selectedRobot !== ''">
      <div class="d-flex justify-content-between">
        <div class="card bg-light flex" style="width: 30%; border-radius: 15px">
          <div class="card-body flex">
            <div
              class="d-flex justify-content-end align-items-center"
              style="border-radius: 15px"
            >
              <img
                src="../image/delivery.png"
                alt=""
                style="width: 30px; margin-top: 10px; margin-right: 10px"
              />
            </div>
            <p class="selectrobot">{{ selectedRobot }}</p>
          </div>
        </div>
        <div class="card" style="width: 30%; border-radius: 15px">
          <div
            class="card-body bg-light d-flex justify-content-start align-items-start"
            style="border-radius: 15px"
          ></div>
        </div>
        <div class="card flex" style="width: 30%; border-radius: 15px">
          <div
            class="card-body bg-light d-flex justify-content-end align-items-center"
            style="border-radius: 15px; margin-right: 20px"
          >
            <i
              class="fa-solid fa-battery-full"
              style="margin-right: 10px; font-size: 50px"
            ></i>
          </div>
        </div>
      </div>
      <br />
      <div class="card bg-light">
        <div class="card-header"></div>
        <div
          class="card-body"
          style="border-radius: 0px 0px 10px 10px; box-shadow: inset"
        ></div>
      </div>
    </div>
    <div v-else class="default-content">
      <!-- Show default content when no robot is selected -->
      <div class="robot-search-container">
        <img src="../image/select.png (1).png" alt="Searching for robots..." />
        <p>Please select a robot</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useStore } from "vuex";
import { ref, onMounted } from "vue";
const store = useStore();
const selectedRobot = ref("");
onMounted(() => {
  // Watch for changes in the Vuex store and update selectedRobot
  selectedRobot.value = store.state.selectedRobot;
  store.subscribe((mutation) => {
    if (mutation.type === "setSelectedRobot") {
      selectedRobot.value = store.state.selectedRobot;
    }
  });
});
</script>

<style scoped>
.robot-search-container {
  text-align: center;
}
.default-content {
  display: flex;
  justify-content: center;
  align-items: center;
}
.robot-search-container img {
  width: 300px;
  margin-bottom: 20px;
  background: none;
  -webkit-filter: drop-shadow(5px 5px 5px #666666);
  filter: drop-shadow(5px 5px 5px #666666);
}
.container {
  font-family: "Poppins", sans-serif;
}
.bg-light {
  box-shadow: #000000 1px 1px 2px;
}
h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
}
.selectrobot {
  margin-left: 20px;
  margin-top: -20px;
  font-weight: 700;
}
span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-right: 10px;
}

.card {
  border: none;
  margin: 0;
  width: auto;
  height: 70px;
}

.card-body {
  padding: 0;
}

.card img {
  border-radius: 15px;
}
</style>
