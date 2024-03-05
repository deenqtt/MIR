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

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/auth/register",
      name: "Register",
      component: Register,
    },
    {
      path: "/auth/login",
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
    },
    {
      path: "/path",
      name: "Path",
      component: Path,
    },
    {
      path: "/Activity",
      name: "Activity",
      component: Activity,
    },
    {
      path: "/Error Log",
      name: "Error Log",
      component: ErrorLog,
    },
    {
      path: "/Error",
      name: "Error",
      component: Error,
    },
    {
      path: "/Add Robot",
      name: "Add Robot",
      component: AddRobot,
    },
    {
      path: "/System",
      name: "System",
      component: System,
    },
    {
      path: "/Robot",
      name: "Robot",
      component: Robot,
    },
    {
      path: "/User",
      name: "User",
      component: User,
    },
    {
      path: "/Modul",
      name: "Modul",
      component: Modul,
    },
    {
      path: "/Mission",
      name: "Mission",
      component: Mission,
    },
    {
      path: "/Footprint",
      name: "Footprint",
      component: Footprint,
    },

    {
      path: "/settings",
      name: "Settings",
      component: Settings,
    },
    {
      path: "/Maps/Created/New",
      Name: "New",
      component: () => import("../Views/created page/createdMaps.vue"),
    },
    {
      path: "/test",
      Name: "tet",
      component: () => import("../Views/test/test.vue"),
    },
    {
      path: "/Mission/Created/New",
      Name: "Miss",
      component: () => import("../Views/created page/CreatedMission.vue"),
    },
    {
      path: "/edit-path/:id",
      name: "edit-path",
      component: () => import("../Views/edit page/EditPath.vue"),
      props: true,
    },
    {
      path: "/edit-mission/:id",
      name: "edit-mission",
      component: () => import("../Views/edit page/EditMission.vue"),
      props: true,
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
    },
    {
      path: "/Canvas",
      name: "Canvas",
      component: () => import("../Views/test/canvas.vue"),
    },
    {
      path: "/coice",
      name: "c",
      component: () => import("../Views/test/choice.vue"),
    },
  ],
});

// Sebelum pengguna berhasil login
router.beforeEach((to, from, next) => {
  // Cek apakah route memerlukan otentikasi dan pengguna belum terotentikasi
  if (to.meta.requiresAuth && !isAuthenticated()) {
    // Jika tidak, arahkan ke halaman login
    next({ name: "Login" });
  } else {
    // Jika terotentikasi atau route tidak memerlukan otentikasi, lanjutkan navigasi
    next();
  }
});

export default router;
