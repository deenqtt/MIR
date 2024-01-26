// store.js
import { createStore } from "vuex";

export default createStore({
  state() {
    return {
      designData: null, // Your form data will be stored here
    };
  },
  mutations: {
    updateFormData(state, newData) {
      state.formData = { ...state.formData, ...newData };
    },
    setDesignData(state, data) {
      state.designData = data;
    },
  },
  actions: {
    async saveDesignData({ commit, state }) {
      try {
        const response = await axios.post("http://localhost:5258/footprints", {
          formData: state.formData, // Assuming formData is part of your state
          lines: state.designData.lines,
        });

        // Commit mutation to update the state
        commit("setDesignData", null);

        return response.data;
      } catch (error) {
        console.error("Failed to save design data:", error);
        throw error;
      }
    },
  },
});
