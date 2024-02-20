import { createRouter, createWebHistory } from "vue-router";

import Register from "../pages/Registerpage.vue";
import Loginform from "../pages/Loginpage.vue";
import Dashboard from "../Views/DashboardPage.vue";
import Maps from "../Views/MapsPage.vue";
import Monitoring from "../Views/MonitoringPage.vue";
import System from "../Views/SystemPage.vue";
import Help from "../Views/RobotPage.vue";
import Path from "../Views/PathPage.vue";
import User from "../Views/UserPage.vue";
import Modul from "../Views/IOModulPage.vue";
import Footprint from "../Views/FootprintPage.vue";
import Mission from "../Views/MissionPage.vue";
import AddRobot from "../Views/AddNew.vue";
const routes = [
  {
    path: "/authentication",
    name: "Register",
    component: Register,
    meta: {
      requiresAuth: false, // Set to true for protected routes
    },
  },
  {
    path: "/",
    name: "Login",
    component: Loginform,
  },
  {
    path: "/Dashboard",
    name: "Dashboard",
    component: Dashboard,
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
    path: "/Monitoring",
    name: "Monitoring",
    component: Monitoring,
  },
  {
    path: "/System",
    name: "System",
    component: System,
  },

  {
    path: "/Help",
    name: "Help",
    component: Help,
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
    path: "/addrobot",
    name: "AddRobot",
    component: AddRobot,
  },

  {
    path: "/Maps/Created/New",
    Name: "New",
    component: () => import("../log/createdMaps.vue"),
  },
  {
    path: "/test",
    Name: "tet",
    component: () => import("../components/test.vue"),
  },

  {
    path: "/Mission/Created/New",
    Name: "Miss",
    component: () => import("../log/CreatedMission.vue"),
  },

  {
    path: "/edit-path/:id",
    name: "edit-path",
    component: () => import("../pages/EditPath.vue"),
    props: true,
  },
  {
    path: "/edit-mission/:id",
    name: "edit-mission",
    component: () => import("../pages/EditMission.vue"),
    props: true,
  },
  {
    path: "/edit-map/:id",
    name: "edit-map",
    component: () => import("../pages/EditMap.vue"),
    props: true,
  },
  {
    path: "/edit-user/:id",
    name: "edit-user",
    component: () => import("../pages/EditUser.vue"),
    props: true,
  },
  {
    path: "/footprint/edit/:id", // Sesuaikan sesuai kebutuhan Anda
    name: "edit-footprint",
    component: () => import("../pages/EditFoot.vue"),
  },
  {
    path: "/Canvas",
    name: "Canvas",
    component: () => import("../components/canvas.vue"),
  },
  {
    path: "/coice",
    name: "c",
    component: () => import("../components/choice.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});
router.beforeEach((to, from, next) => {
  console.log("Navigation guard triggered");
  const isAuthenticated = JSON.parse(localStorage.getItem("authenticated"));

  if (!isAuthenticated && to.meta.requiresAuth) {
    console.log("Redirecting to Login");
    next({ name: "Login" });
  } else if (isAuthenticated && to.name === "Login") {
    console.log("Redirecting to Dashboard");
    next({ name: "Dashboard" });
  } else {
    console.log("Continuing with navigation");
    next();
  }
});

export default router;
