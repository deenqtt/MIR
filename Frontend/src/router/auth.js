import { useRouter } from "vue-router";

export function saveAuthToken(token) {
  localStorage.setItem("token", token);
}

export function removeAuthToken() {
  localStorage.removeItem("token");
}

export function isAuthenticated() {
  const token = localStorage.getItem("token");
  return !!token;
}

export function logout() {
  console.log("Logout berhasil");
  removeAuthToken();
  window.location.reload();
  const router = useRouter();
  router.push({ name: "Login" });
}
