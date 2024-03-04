<template>
  <div class="container">
    <div class="centered-content" v-if="!errors.length && !errorOccurred">
      <img
        src="../assets/image/robotsearch.png.png"
        alt="Robot"
        class="center-image"
      />
      <p class="message">Nothing error, all good.</p>
    </div>
    <div v-else>
      <div class="card">
        <div class="card-body">
          <table class="table table-hover" v-if="errors.length">
            <thead class="thead-dark">
              <tr>
                <th>Robot Name</th>
                <th>Error Description</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(error, index) in errors" :key="index">
                <td>{{ error.robotname }}</td>
                <td>{{ error.explained }}</td>
                <td>
                  <button @click="markAsDone(index)" class="btn btn-success">
                    Done
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
          <p v-else class="error-message">Error occurred.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import Swal from "sweetalert2";
const errors = ref([]);
const errorOccurred = ref(false);

const apiUrl = "http://localhost:5258/errors";

const fetchErrors = async () => {
  try {
    const response = await axios.get(apiUrl);
    errors.value = response.data;
    errorOccurred.value = errors.value.length > 0;
  } catch (error) {
    console.error("Error fetching errors:", error);
  }
};

const markAsDone = async (index) => {
  try {
    const confirmed = await Swal.fire({
      title: "Are you sure?",
      text: "Do you want to mark this error as done?",
      icon: "question",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, mark it as done",
    });

    if (confirmed.isConfirmed) {
      // Kirim permintaan ke server untuk menandai error sebagai selesai
      const errorId = errors.value[index].id; // Anggap error memiliki properti id
      await axios.delete(`${apiUrl}/${errorId}`);

      // Hapus error dari daftar
      errors.value.splice(index, 1);
      // Perbarui nilai errorOccurred jika tidak ada error lagi
      errorOccurred.value = errors.value.length > 0;

      Swal.fire(
        "Marked as Done!",
        "The error has been marked as done.",
        "success"
      );
    }
  } catch (error) {
    console.error("Error marking error as done:", error);
  }
};

onMounted(() => {
  fetchErrors();
});
</script>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
}

.centered-content {
  max-width: 100%;
  max-height: 100%;
  margin-top: 100px;
}

.centered-content img {
  filter: drop-shadow(2px 4px 6px rgba(0, 0, 0, 0.5));
}

.message {
  font-family: "Poppins", sans-serif;
  text-align: center;
  margin-top: 20px;
}

.error-message {
  font-family: "Poppins", sans-serif;
  text-align: center;
  margin-top: 20px;
}

.btn-success {
  background-color: #28a745;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-success:hover {
  background-color: #218838;
}
</style>
