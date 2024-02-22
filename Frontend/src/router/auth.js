// services/auth.js

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
  localStorage.removeItem("token");
}
