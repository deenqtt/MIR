import { createStore } from "vuex";
import axios from "axios";

export default createStore({
  state: {
    selectedRobot: "",
    robotOptions: [],
    filteredMissions: [],
    notifications: [],
    unreadNotifications: 0,
    errorMessage: "",
  },
  mutations: {
    setSelectedRobot(state, robotName) {
      state.selectedRobot = robotName;
    },
    setRobotOptions(state, robotOptions) {
      state.robotOptions = robotOptions;
    },
    setFilteredMissions(state, missions) {
      state.filteredMissions = missions;
    },
    addNotification(state, notification) {
      state.notifications.push(notification);
      state.unreadNotifications++;
    },
    markAllNotificationsAsRead(state) {
      state.notifications.forEach((notification) => {
        notification.read = true;
      });
      state.unreadNotifications = 0;
    },
    updateUnreadNotifications(state) {
      state.unreadNotifications--;
    },
    setErrorMessage(state, errorMessage) {
      state.errorMessage = errorMessage;
    },
  },
  actions: {
    async fetchRobots({ commit }) {
      try {
        const response = await axios.get("http://localhost:5258/robots");
        const robots = response.data;
        commit("setRobots", robots);
        const robotOptions = robots.map((robot) => robot.Name);
        commit("setRobotOptions", robotOptions);
      } catch (error) {
        console.error("Failed to fetch robots:", error);
      }
    },
    async sendErrorMessageToRobot({ state, commit }, errorMessage) {
      try {
        const robotName = state.selectedRobot;
        console.log(`Sending error message to ${robotName}: ${errorMessage}`);
        const selectedRobot = state.robots.find(
          (robot) => robot.Name === robotName
        );
        if (selectedRobot) {
          commit("setRobotError", {
            robotId: selectedRobot.Id,
            hasError: true,
          });
        }
        // Simulate sending error message to robot
        // Replace this with actual logic to send error message to robot
      } catch (error) {
        console.error(
          `Failed to send error message to ${robotName}: ${error.message}`
        );
      }
    },
  },
});
