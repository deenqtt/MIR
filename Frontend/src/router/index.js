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
import Add from "../Views/AddNew.vue";
const routes = [
  {
    path: "/authentication",
    name: "Register",
    component: Register,
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
    path: "/Robot",
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
    path: "/Add Robot",
    name: "AddNew",
    component: Add,
  },
  {
    path: "/Maps/Created",
    Name: "NewMap",
    component: () => import("../pages/CreateMapPage.vue"),
  },
  {
    path: "/Maps/Created/New",
    Name: "New",
    component: () => import("../log/createdMaps.vue"),
  },
  {
    path: "/Path/New",
    Name: "NewPath",
    component: () => import("../pages/CreatePathPage.vue"),
  },
  {
    path: "/Mission/New",
    Name: "NewMission",
    component: () => import("../pages/CreateMissionPage.vue"),
  },
  {
    path: "/Mission/Created/New",
    Name: "Miss",
    component: () => import("../log/CreatedMission.vue"),
  },

  {
    path: "/edit/:mapId",
    name: "edit-map",
    component: () => import("../pages/EditMap.vue"),
    props: true,
  },
  {
    path: "/edit/:pathId",
    name: "edit-path",
    component: () => import("../pages/EditPath.vue"),
    props: true,
  },
  {
    path: "/edit/:missionId",
    name: "edit-mission",
    component: () => import("../pages/EditMission.vue"),
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
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
