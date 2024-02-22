import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

export default defineConfig({
  plugins: [vue()],
  vue: {
    compilerOptions: {
      isCustomElement: (tag) => tag.startsWith("lord-icon"),
    },
  },
});
