from flask import Flask, jsonify
from flask_cors import CORS
import paho.mqtt.client as mqtt

app = Flask(__name__)
CORS(app)  # Aktifkan CORS

# Inisialisasi koneksi MQTT
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT Broker!")
        client.connected = True
        # Subscribe ke topik "Batt1" setelah koneksi berhasil dilakukan
        client.subscribe("Batt1")
    else:
        print("Failed to connect, return code %d\n", rc)
        client.connected = False

# Fungsi untuk menangani pesan yang diterima
def on_message(client, userdata, msg):
    print(f"Received message '{msg.payload.decode()}' from topic '{msg.topic}'")

mqtt_client = mqtt.Client()
mqtt_client.connected = False
mqtt_client.on_connect = on_connect
mqtt_client.on_message = on_message  # Menambahkan fungsi on_message
mqtt_client.connect("52.74.91.79", 1883)

# Route untuk memeriksa koneksi MQTT
@app.route('/check_mqtt_connection')
def check_mqtt_connection():
    return jsonify({'connected': mqtt_client.connected})

if __name__ == '__main__':
    mqtt_client.loop_start()
    app.run(debug=True)
