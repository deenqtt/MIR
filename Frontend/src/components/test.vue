<template>
  <div class="controller">
    <button @click="move('up')" class="btn up">Up</button>
    <div>
      <button @click="move('left')" class="btn left">Left</button>
      <button @click="move('right')" class="btn right">Right</button>
    </div>
    <button @click="move('down')" class="btn down">Down</button>
    <button @click="clickAction" class="btn click">Click</button>
  </div>
</template>

<script setup>
const move = (direction) => {
  console.log(`Move to ${direction}`);
};

const clickAction = () => {
  console.log("Click");
};
</script>

<style scoped>
.controller {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
}

.btn {
  margin: 5px;
  padding: 10px 20px;
  font-size: 16px;
  border: none;
  cursor: pointer;
}

.up {
  background-color: #3498db;
  color: #ffffff;
}

.left {
  background-color: #e74c3c;
  color: #ffffff;
}

.right {
  background-color: #2ecc71;
  color: #ffffff;
}

.down {
  background-color: #f39c12;
  color: #ffffff;
}

.click {
  background-color: #9b59b6;
  color: #ffffff;
}
</style>
