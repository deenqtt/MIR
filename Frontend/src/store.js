import { createStore } from "vuex";

export default createStore({
  state: {
    selectedRobot: "",
    robotOptions: [],
    filteredMissions: [],
    notifications: [], // Menambahkan properti notifications untuk menyimpan pesan notifikasi
    unreadNotifications: 0, // Menambahkan properti untuk menyimpan jumlah notifikasi yang belum dibaca
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
      // Menambah jumlah notifikasi yang belum dibaca setelah menambahkan notifikasi baru
      state.unreadNotifications++;
    },
    markAllNotificationsAsRead(state) {
      // Mengubah properti read dari semua notifikasi menjadi true
      state.notifications.forEach((notification) => {
        notification.read = true;
      });
      // Mengatur jumlah notifikasi yang belum dibaca menjadi 0 setelah semua notifikasi dibaca
      state.unreadNotifications = 0;
    },
    // Mutasi untuk mengurangi jumlah notifikasi yang belum dibaca saat notifikasi dibaca
    updateUnreadNotifications(state) {
      state.unreadNotifications--;
    },
  },
});
