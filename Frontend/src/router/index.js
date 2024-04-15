import { createRouter, createWebHistory } from "vue-router";
import { isAuthenticated } from "./auth";
import Register from "../Views/authentication/Registerpage.vue";
import Loginform from "../Views/authentication/Loginpage.vue";
import Dashboard from "../Views/view page/DashboardPage.vue";
import Maps from "../Views/view page/MapsPage.vue";
import Activity from "../Views/view page/ActivityPage.vue";
import System from "../Views/view page/SystemPage.vue";
import Robot from "../Views/view page/RobotPage.vue";
import Path from "../Views/view page/PathPage.vue";
import User from "../Views/view page/UserPage.vue";
import Modul from "../Views/view page/IOModulPage.vue";
import Footprint from "../Views/view page/FootprintPage.vue";
import Mission from "../Views/view page/MissionPage.vue";
import AddRobot from "../Views/view page/AddNew.vue";
import Settings from "../Views/view page/SettingsPage.vue";
import ErrorLog from "../Views/view page/Errorpage.vue";
import Error from "../Views/view page/adderror.vue";
import Battery from "../Views/setting/batterypage.vue";
import ErrorHand from "../Views/setting/errorhandling.vue";
import Docking from "../Views/setting/docking.vue";
import MQTT from "../Views/setting/MQTT.vue";
const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/auth/register",
      name: "Register",
      component: Register,
      meta: {
        requiresAuth: false,
      },
    },
    {
      path: "/",
      name: "Login",
      component: Loginform,
      meta: {
        requiresAuth: false, // Set to true for protected routes
      },
    },
    {
      path: "/Dashboard",
      name: "Dashboard",
      component: Dashboard,
      meta: { requiresAuth: true },
    },
    {
      path: "/maps",
      name: "Maps",
      component: Maps,
      meta: { requiresAuth: true },
    },
    {
      path: "/path",
      name: "Path",
      component: Path,
      meta: { requiresAuth: true },
    },
    {
      path: "/Activity",
      name: "Activity",
      component: Activity,
      meta: { requiresAuth: true },
    },
    {
      path: "/Error Log",
      name: "Error Log",
      component: ErrorLog,
      meta: { requiresAuth: true },
    },
    {
      path: "/Error",
      name: "Error",
      component: Error,
      meta: { requiresAuth: true },
    },
    {
      path: "/Add Robot",
      name: "Add Robot",
      component: AddRobot,
      meta: { requiresAuth: true },
    },
    {
      path: "/System",
      name: "System",
      component: System,
      meta: { requiresAuth: true },
    },
    {
      path: "/Robot",
      name: "Robot",
      component: Robot,
      meta: { requiresAuth: true },
    },
    {
      path: "/User",
      name: "User",
      component: User,
      meta: { requiresAuth: true },
    },
    {
      path: "/Modul",
      name: "Modul",
      component: Modul,
      meta: { requiresAuth: true },
    },
    {
      path: "/Mission",
      name: "Mission",
      component: Mission,
      meta: { requiresAuth: true },
    },
    {
      path: "/Footprint",
      name: "Footprint",
      component: Footprint,
      meta: { requiresAuth: true },
    },

    {
      path: "/settings",
      name: "Settings",
      component: Settings,
      meta: { requiresAuth: true },
    },
    {
      path: "/battery",
      name: "battery",
      component: Battery,
      meta: { requiresAuth: true },
    },
    {
      path: "/error handling",
      name: "errorhan",
      component: ErrorHand,
      meta: { requiresAuth: true },
    },
    {
      path: "/docking",
      name: "docking",
      component: Docking,
      meta: { requiresAuth: true },
    },
    {
      path: "/mqtt",
      name: "mqtt",
      component: MQTT,
      meta: { requiresAuth: true },
    },
    {
      path: "/Maps/Created/New",
      Name: "New",
      component: () => import("../Views/created page/createdMaps.vue"),
      meta: { requiresAuth: true },
    },

    {
      path: "/Mission/Created/New",
      Name: "Miss",
      component: () => import("../Views/created page/CreatedMission.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/edit-path/:id",
      name: "edit-path",
      component: () => import("../Views/edit page/EditPath.vue"),
      props: true,
      meta: { requiresAuth: true },
    },
    {
      path: "/edit-mission/:id",
      name: "edit-mission",
      component: () => import("../Views/edit page/EditMission.vue"),
      props: true,
      meta: { requiresAuth: true },
    },
    {
      path: "/edit-map/:id",
      name: "edit-map",
      component: () => import("../Views/edit page/EditMap.vue"),
      props: true,
    },
    {
      path: "/edit-user/:id",
      name: "edit-user",
      component: () => import("../Views/edit page/EditUser.vue"),
      props: true,
    },
    {
      path: "/footprint/edit/:id", // Sesuaikan sesuai kebutuhan Anda
      name: "edit-footprint",
      component: () => import("../Views/edit page/EditFoot.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/Canvas",
      name: "Canvas",
      component: () => import("../Views/test/canvas.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/coice",
      name: "c",
      component: () => import("../Views/test/choice.vue"),
      meta: { requiresAuth: true },
    },
  ],
});

// Sebelum pengguna berhasil login
router.beforeEach((to, from, next) => {
  // Jika pengguna sudah login dan mencoba mengakses halaman login
  if (isAuthenticated() && to.name === "Login") {
    // Arahkan mereka ke halaman dashboard atau halaman lain yang sesuai
    next({ name: "Dashboard" }); // Ganti "Dashboard" dengan nama halaman tujuan yang sesuai
  } else if (to.meta.requiresAuth && !isAuthenticated()) {
    // Jika route memerlukan otentikasi dan pengguna belum terotentikasi
    // Arahkan ke halaman login
    next({ name: "Login" });
  } else if (!isAuthenticated() && to.name === "Register") {
    // Jika pengguna belum login dan mencoba mengakses halaman register
    // Arahkan mereka ke halaman register
    next();
  } else {
    // Jika terotentikasi atau route tidak memerlukan otentikasi, lanjutkan navigasi
    next();
  }
});

export default router;
