import { createStore } from "vuex";

export default createStore({
  state: {
    selectedRobot: "",
    robotOptions: [],
    filteredMissions: [],
    notifications: [], // Menambahkan properti notifications untuk menyimpan pesan notifikasi
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
    },
    markAllNotificationsAsRead(state) {
      // Mengubah properti read dari semua notifikasi menjadi true
      state.notifications.forEach((notification) => {
        notification.read = true;
      });
    },
  },
});
