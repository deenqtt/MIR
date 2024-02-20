<template>
  <div class="container">
    <h5>
      Foot
      <span>prints</span>
    </h5>

    <p>Create and Design Your 2D Here</p>
    <div class="d-flex">
      <button
        class="btn btn-success"
        v-if="!showCreateForm"
        @click="showCreateForm = true"
      >
        Create New
      </button>
    </div>
    <h5 class="text-title text-dark" v-if="!showCreateForm">List Footprint</h5>
    <div class="card bg-light" v-if="!showCreateForm">
      <div class="card-body">
        <div class="d-flex align-items-center">
          <p class="mr-2">Search</p>
          <div class="group">
            <svg class="icon" aria-hidden="true" viewBox="0 0 24 24">
              <g>
                <path
                  d="M21.53 20.47l-3.66-3.66C19.195 15.24 20 13.214 20 11c0-4.97-4.03-9-9-9s-9 4.03-9 9 4.03 9 9 9c2.215 0 4.24-.804 5.808-2.13l3.66 3.66c.147.146.34.22.53.22s.385-.073.53-.22c.295-.293.295-.767.002-1.06zM3.5 11c0-4.135 3.365-7.5 7.5-7.5s7.5 3.365 7.5 7.5-3.365 7.5-7.5 7.5-7.5-3.365-7.5-7.5z"
                ></path>
              </g>
            </svg>
            <input placeholder="Search" type="search" class="input" />
          </div>
        </div>

        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">#ID</th>
              <th scope="col">Name</th>
              <th scope="col">Robot</th>

              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody v-if="footprints.length > 0">
            <tr v-for="footprint in footprints" :key="footprint.id">
              <td>{{ footprint.id }}</td>
              <td>{{ footprint.name }}</td>
              <td>{{ footprint.robotname }}</td>

              <td colspan="">
                <div class="d-flex">
                  <button
                    id="edit"
                    class="material-symbols-outlined"
                    @click="editPath(footprint)"
                  >
                    edit
                  </button>

                  <br />
                  <button
                    id="delete"
                    class="material-symbols-outlined"
                    @click="deleteDesign(footprint)"
                  >
                    delete
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- <div class="canvass" v-if="showCreateForm">
      <div class="color-options">
        <div
          v-for="(color, index) in getColorOptions(selectedBoxType)"
          :key="index"
          @click="selectColor(color)"
          :class="{ selected: selectedColor === color }"
          :style="{ backgroundColor: color }"
        ></div>
      </div>

      <div class="preview-section" v-if="selectedColor">
        <h3>Preview:</h3>
        <div
          class="preview-box"
          :style="{
            backgroundColor: selectedColor,
            width: boxWidth,
            height: boxHeight,
          }"
        ></div>
        <button @click="downloadBox">Download Box</button>
      </div>
    </div> -->
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router"; // Import useRouter
const showCreateForm = ref(false);
import Swal from "sweetalert2";
const footprints = ref([]);
const router = useRouter(); // Initialize the router
const errorMessage = ref("");
const apiUrl = "http://localhost:5258/footprints";

let lines = ref([]);

const newFoot = ref({
  Name: "",
  Robotname: "",
});
const cancelForm = () => {
  // Reset form and navigate back to the list view
  newFoot.value = {
    Name: "",
    Robotname: "",
  };
  showCreateForm.value = false;
};
const robotOptions = ref([]);
const fetchDesign = async () => {
  try {
    const response = await axios.get(apiUrl);
    footprints.value = response.data;
  } catch (error) {
    errorMessage.value = "Failed to fetch maps: " + error.message;
  }
};

const deleteDesign = async (footprint) => {
  // Gunakan SweetAlert untuk konfirmasi penghapusan
  const confirmDelete = await Swal.fire({
    title: "Are You Sure To Delete?",
    text: "You will not be able to return this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Delete!",
    cancelButtonText: "Cancel",
  });

  if (confirmDelete.isConfirmed) {
    try {
      // Hapus pengguna jika pengguna mengkonfirmasi
      await axios.delete(`${apiUrl}/${footprint.id}`);
      fetchMission();
      // Tampilkan pesan sukses menggunakan SweetAlert
      await Swal.fire("Congratss!", "Design Has Deleted", "success");
    } catch (error) {
      errorMessage.value = "Failed to delete user: " + error.message;
    }
  }
};

const editPath = (footprint) => {
  // Use router to navigate to "/edit" and pass the map data as a parameter
  router.push({
    name: "edit-footprint",
    params: { id: footprint.id },
  });
};
const fetchRobots = async () => {
  try {
    const response = await axios.get("http://localhost:5258/robots");
    robotOptions.value = response.data.map((robot) => robot.name);
  } catch (error) {
    console.error("Error fetching robot names:", error);
  }
};
// const submitForm = async () => {
//   if (validateForm()) {
//     const dataURL = canvas.toDataURL("image/png");
//     const imageBlob = await fetch(dataURL).then((res) => res.blob());

//     const formData = new FormData();
//     formData.append("file", imageBlob, "drawn_image.png");
//     formData.append("name", newFoot.value.Name);
//     formData.append("robotname", newFoot.value.Robotname);

//     // Log request headers
//     console.log("Request Headers:", Object.fromEntries(formData.entries()));

//     try {
//       const response = await axios.post(apiUrl, formData, {
//         headers: {
//           "Content-Type": "multipart/form-data",
//         },
//       });

//       console.log("Image uploaded successfully!");
//       router.push("/footprint"); // Redirect after successful submission
//     } catch (error) {
//       console.error("Error uploading image:", error);
//     }
//   }
// };

// const validateForm = () => {
//   if (
//     !newFoot.value.Name ||
//     !newFoot.value.Robotname ||
//     lines.value.length === 0
//   ) {
//     console.error("Please fill in all required fields and draw on the canvas.");
//     return false;
//   }
//   return true;
// };
// const boxTypes = {
//   small: {
//     width: "50px",
//     height: "50px",
//     colors: ["#FF0000", "#00FF00", "#0000FF"],
//   },
//   medium: {
//     width: "100px",
//     height: "100px",
//     colors: ["#FFA500", "#008000", "#000080"],
//   },
//   large: {
//     width: "150px",
//     height: "150px",
//     colors: ["#800080", "#FFFF00", "#008080"],
//   },
// };

// const selectedBoxType = ref("small");
// const selectedColor = ref(boxTypes[selectedBoxType.value].colors[0]);

// const selectColor = (color) => {
//   selectedColor.value = color;
// };

// const getColorOptions = (boxType) => {
//   return boxTypes[boxType].colors;
// };

// const boxWidth = ref(boxTypes[selectedBoxType.value].width);
// const boxHeight = ref(boxTypes[selectedBoxType.value].height);

// // Watch for changes in selectedBoxType and update boxWidth and boxHeight
// watch(
//   () => selectedBoxType.value,
//   () => {
//     boxWidth.value = boxTypes[selectedBoxType.value].width;
//     boxHeight.value = boxTypes[selectedBoxType.value].height;
//   }
// );

onMounted(() => {
  // canvas.getContext("2d");

  // drawAxes();
  fetchRobots();
  fetchDesign();

  // canvas.addEventListener("mousedown", startDrawing);
  // canvas.addEventListener("mouseup", stopDrawing);
  // canvas.addEventListener("mousemove", draw);
});
</script>

<style scoped>
.group {
  display: flex;
  line-height: 28px;
  align-items: center;
  position: relative;
  max-width: 190px;
}

.group .input {
  margin-top: 10px;
  width: 100%;
  height: 30px;
  line-height: 28px;
  padding: 0 1rem;
  padding-left: 2.5rem;
  border: 2px solid transparent;
  border-radius: 8px;
  outline: none;
  background-color: #ffffff;
  color: #0d0c22;
  transition: 0.3s ease;
  box-shadow: 1px 1px 2px #000;
  margin-bottom: 10px;
}

.group .input::placeholder {
  color: #9e9ea7;
}
.mr-2 {
  margin-top: 10px;
}
.input:focus,
input:hover {
  outline: none;
  border-color: rgba(0, 10, 196, 0.4);
  background-color: #fff;
  box-shadow: 0 0 0 4px rgba(49, 13, 228, 0.1);
}

.group .icon {
  position: absolute;

  left: 0.6rem;
  fill: #9e9ea7;
  width: 1rem;
  height: 1rem;
}
.d-flex .material-symbols-outlined {
  border: none;
  background: none;
}

.d-flex {
  align-self: flex-end;
}
input {
  height: 20px;
  margin-top: -25px;
  font-size: 12px;
}
.container {
  font-family: "Poppins", sans-serif;
  display: flex;
  flex-direction: column; /* Mengatur tata letak elemen dalam satu kolom */
}

h5 {
  font-size: 25px;
  font-weight: 700;
  color: #0800ff;
  margin-top: 20px;
}

span {
  font-size: 25px;
  font-weight: 700;
  color: #000000;
  margin-left: -7px;
}

p {
  margin-top: -10px;
  font-size: 12px;
  font-weight: 500; /* Memberikan jarak atas antara h5 dan p */
}
.container .btn {
  text-align: center;
  width: 120px;
  color: #000;
  font-size: 12px;
  font-weight: 600;
  height: 30px;
  align-self: flex-end;
  margin-right: 40px;
  margin-top: -60px;
  margin-bottom: 50px;
}
.card {
  margin-right: 40px;
  box-shadow: 5px 10px 8px #779bff;
  border-radius: 10px;
}

th {
  font-size: 13px;
}
.text-title {
  color: #656565;
  margin-top: -30px;
}

.container {
  display: flex;
  flex-direction: column;
}
.canvass canvas {
  border: 1px solid #000;
  margin-bottom: 10px;
  cursor: crosshair;
}
.canvass .btn {
  width: auto;
}

.canvass .form-flex {
  display: flex;
  justify-content: space-between;
}

.canvass .form-group {
  flex: 0 0 48%; /* Adjust width as needed */
}
</style>
