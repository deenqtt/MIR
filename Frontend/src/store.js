import { createStore } from "vuex";

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
    removeNotification(state, notificationIndex) {
      state.notifications.splice(notificationIndex, 1);
      state.unreadNotifications--;
    },
  },
  actions: {
    async removeNotification({ commit, state }, notificationIndex) {
      try {
        commit("removeNotification", notificationIndex);
      } catch (error) {
        console.error("Failed to remove notification:", error);
      }
    },
  },
});
