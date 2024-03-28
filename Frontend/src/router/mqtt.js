import mqtt from "mqtt";

const brokerUrl = "wss://59.74.91.79"; // URL broker MQTT

// Fungsi untuk menghubungkan ke server MQTT
const connect = () => {
  console.log("Connecting to MQTT server...");
  const client = mqtt.connect(brokerUrl); // Menghubungkan ke broker MQTT
  // Tangani event ketika koneksi berhasil dibuat
  client.on("connect", () => {
    console.log("Connected to MQTT broker");
  });

  // Tangani event ketika pesan diterima
  client.on("message", (topic, message) => {
    console.log(
      `Received message on topic: ${topic}, Message: ${message.toString()}`
    );
  });

  // Subscribe ke topik tertentu
  const topic = "test";
  client.subscribe(topic);

  // Publish pesan ke topik tertentu
  client.publish(topic, "Hello MQTT");
};

// Fungsi untuk mematikan koneksi dari server MQTT
const disconnect = () => {
  // Logika untuk memutus koneksi dari server MQTT
  console.log("Disconnecting from MQTT server...");
  // Tambahkan logika untuk memutus koneksi dari server MQTT jika diperlukan
};

// Fungsi untuk melakukan subscribe pada topik MQTT tertentu
const subscribe = (topic) => {
  // Logika untuk melakukan subscribe pada topik MQTT
  console.log("Subscribing to topic:", topic);
  // Tambahkan logika untuk subscribe ke topik MQTT jika diperlukan
};

// Fungsi untuk melakukan unsubscribe dari topik MQTT tertentu
const unsubscribe = (topic) => {
  // Logika untuk melakukan unsubscribe dari topik MQTT
  console.log("Unsubscribing from topic:", topic);
  // Tambahkan logika untuk unsubscribe dari topik MQTT jika diperlukan
};

// Export fungsi-fungsi yang dibutuhkan
export { connect, disconnect, subscribe, unsubscribe };
