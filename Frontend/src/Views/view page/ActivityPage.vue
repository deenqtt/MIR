<template>
  <div class="container">
    <div class="d-flex">
      <h5 class="h5span"><span>Activities</span></h5>
      <div class="dropdown show">
        <a
          class="dropdown-toggle"
          href="#"
          role="button"
          id="dropdownMenuLink"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        ></a>

        <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
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
    <div class="card bg-dark">
      <div class="card-body">
        <div class="justify-content-between">
          <div class="content">
            <div class="d-flex last1 h5 justify-content-between">
              <div class="last text-white">
                <h5>Last Activities</h5>
              </div>
              <div class="today text-white">
                <h5>Today</h5>
              </div>
              <div class="filter text-white" @click="showFilterModal">
                <h5 style="font-size: medium; margin-right: 10px">
                  Filter
                  <i
                    class="material-icons"
                    style="font-size: medium; margin-left: 8px"
                    >calendar_today</i
                  >
                </h5>
              </div>
            </div>
            <div class="last content">
              <div class="activity text-white">
                <!-- Display image when no activities -->
                <img
                  v-if="!selectedRobot"
                  src="../image/robotsearch.png.png"
                  alt=""
                  class="no-activities-img"
                />
                <!-- Display message when no robot is selected -->
                <div v-if="!selectedRobot" class="no-activities-text">
                  <p>Please select a robot to view activities.</p>
                </div>
                <!-- Display message if no activities are available -->
                <div
                  v-else-if="activities.length === 0"
                  class="no-activities-text"
                >
                  <p>No activities available for this robot.</p>
                </div>
                <!-- Display activities if available -->
                <div v-else>
                  <!-- Display message if no activities are available for the selected time range -->
                  <div
                    v-if="activities.length === 0"
                    class="no-activities-text"
                  >
                    <p>No activities at this time.</p>
                  </div>
                  <!-- Display activities if available -->
                  <div v-else>
                    <div
                      v-for="activitie in activities"
                      :key="activitie.id"
                      class="activity-item"
                    >
                      <!-- Activity description -->
                      <div class="activity-description text-white">
                        <p class="d-flex">
                          <span class="bullet">
                            <p class="activity-time">
                              {{ formatTime(activitie.date) }}
                            </p>
                          </span>
                          {{ activitie.activity }}
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal for time filter -->
    <div
      class="modal fade"
      id="filterModal"
      tabindex="-1"
      role="dialog"
      aria-labelledby="filterModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="filterModalLabel">
              Filter Activities by Time
            </h5>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
              @click="hideFilterModal"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <!-- Input to select time range -->
            <label for="startTime">Start Time:</label>
            <input
              type="time"
              id="startTime"
              name="startTime"
              v-model="startTime"
            />
            <label for="endTime">End Time:</label>
            <input type="time" id="endTime" name="endTime" v-model="endTime" />
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-dismiss="modal"
            >
              Close
            </button>
            <button type="button" class="btn btn-primary" @click="applyFilter">
              Apply Filter
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const robotOptions = ref([]);
const activities = ref([]);
const selectedRobot = ref(null);
const startTime = ref("");
const endTime = ref("");

async function fetchRobots() {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
}

const fetchActivities = (robotName) => {
  let url = `http://localhost:5258/activities/robot/${robotName}`;

  if (startTime.value && endTime.value) {
    url += `?startTime=${startTime.value}&endTime=${endTime.value}`;
  }

  return axios
    .get(url)
    .then((response) => {
      activities.value = response.data;
    })
    .catch((error) => {
      console.error("Error fetching activities:", error);
      activities.value = [];
    });
};

function selectRobot(robot) {
  selectedRobot.value = robot;
  fetchActivities(robot);
}

function formatTime(dateString) {
  const date = new Date(dateString);
  return `${date.getHours()}:${date.getMinutes()}`;
}

function showFilterModal() {
  $("#filterModal").modal("show");
}

function hideFilterModal() {
  $("#filterModal").modal("hide");
}

function applyFilter() {
  if (selectedRobot.value) {
    fetchActivities(selectedRobot.value);
  }
  hideFilterModal();
}
onMounted(() => {
  fetchRobots();
});
</script>

<style scoped>
.activity-description {
  display: flex;
  margin-top: 20px;
  margin-left: 10px;
}
.dropdown {
  margin-top: 23px;
  margin-left: 20px;
}
.h5span {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
  border: none;
  margin-bottom: 20px;
}

.h5span span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-left: -7px;
}
.last h5 {
  font-size: 15px;
}
.container {
  flex-direction: column;
  font-family: "Poppins", sans-serif;
}

.card {
  border-radius: 10px;
  border: none;
  background-color: #343a40;
  box-shadow: 1px 1px 1px #000000; /* Background color */
}
.material-symbols-outlined {
  margin-top: -4px;
  margin-left: 8px;
}

.card-body {
  display: flex;
  flex-direction: column; /* Mengatur tata letak vertikal */
}

/* Style untuk icon filter */
.material-icons {
  vertical-align: middle;
}

/* Style untuk dropdown filter */
.filter-dropdown {
  position: absolute;
  top: 30px;
  right: 0;
  background-color: #343a40; /* Background color dropdown */
  padding: 10px;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: none;
  z-index: 1;
}

/* Style untuk teks Today */
.default-content {
  display: flex;
  flex-direction: column;
  align-items: center;
}
.last1 {
  border-bottom: #000000 2px solid;
  justify-content: space-between;
}
.today {
  position: absolute;
  top: 0%;
  left: 47.5%;
  margin-top: 35px;
  background: #343a40;
}

/* Styling untuk gambar saat tidak ada aktivitas */
.no-activities-img {
  margin: auto;
  display: block;
  margin-top: 50px; /* Menengahkan gambar */
}

/* Styling untuk teks saat tidak ada aktivitas */
.no-activities-text {
  text-align: center;
  margin-top: 20px; /* Memberikan jarak ke atas */
  color: white;
}

/* Styling untuk waktu aktivitas */
.activity-time {
  color: rgba(255, 255, 255, 0.7);

  margin-left: 30px;
  margin-right: 50px;
  width: 200px;
}

/* Styling untuk bullet */
.bullet {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background-color: white;
  margin-right: 80px;
  margin-top: 2px; /* Memberikan jarak ke kanan */
}
</style>
