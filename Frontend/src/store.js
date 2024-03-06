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
  },
  actions: {
    // Fungsi untuk mengirim pesan error kepada robot yang dipilih
    async sendErrorMessageToRobot({ state }, errorMessage) {
      try {
        const robotName = state.selectedRobot;
        // Simulasikan pengiriman pesan ke robot dengan menggunakan API atau metode lainnya
        console.log(`Sending error message to ${robotName}: ${errorMessage}`);
        // Di sini Anda dapat mengimplementasikan logika untuk mengirim pesan ke robot yang dipilih
        // Misalnya, dengan menggunakan WebSocket atau HTTP request ke endpoint yang sesuai
      } catch (error) {
        console.error(`Failed to send error message to ${robotName}: ${error.message}`);
        // Handle error jika gagal mengirim pesan ke robot
      }
    },
  },
});
