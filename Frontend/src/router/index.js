import { createRouter, createWebHistory } from "vue-router";
import Register from "../pages/Registerpage.vue";
import Loginform from "../pages/Loginpage.vue";
import Dashboard from "../Views/DashboardPage.vue";
import Maps from "../Views/MapsPage.vue";
import Monitoring from "../Views/MonitoringPage.vue";
import System from "../Views/SystemPage.vue";
import Help from "../Views/HelpPage.vue";
import Path from "../Views/PathPage.vue";
import User from "../Views/UserPage.vue";
import Modul from "../Views/IOModulPage.vue";
import Footprint from "../Views/FootprintPage.vue";
import Mission from "../Views/MissionPage.vue";
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
    path: "/help",
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
    path: "/Footprint/New",
    Name: "NewMission",
    component: () => import("../pages/CreateFootPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
