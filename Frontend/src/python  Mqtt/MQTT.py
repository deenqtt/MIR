from flask import Flask, jsonify, request
from flask_cors import CORS
import paho.mqtt.client as mqtt
import requests

app = Flask(__name__)
CORS(app)  # Aktifkan CORS

mqtt_client = None
topic = "Batt1"

# Fungsi untuk menangani koneksi ke MQTT broker
def connect_to_mqtt(broker_address, port, new_topic):
    global mqtt_client, topic

    # Hentikan dan hapus koneksi dan langganan dari konfigurasi sebelumnya
    if mqtt_client:
        mqtt_client.unsubscribe(topic)
        mqtt_client.disconnect()
        mqtt_client = None
        print("Disconnected from previous MQTT Broker and unsubscribed from topic", topic)

    # Buat koneksi baru ke MQTT broker
    mqtt_client = mqtt.Client()
    mqtt_client.on_connect = on_connect
    mqtt_client.on_message = on_message
    mqtt_client.connect(broker_address, port)

    # Langganan ke topik yang baru
    mqtt_client.subscribe(new_topic)
    topic = new_topic

    mqtt_client.loop_start()
    print("Connected to new MQTT Broker:", broker_address)
    print("Subscribed to new topic:", new_topic)

# Fungsi yang dipanggil saat terhubung ke MQTT broker
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT Broker!")
    else:
        print("Failed to connect, return code", rc)

# Fungsi yang dipanggil saat menerima pesan dari MQTT broker
def on_message(client, userdata, msg):
    global percentage
    print(f"Received message '{msg.payload.decode()}' from topic '{msg.topic}'")
    try:
        # Coba untuk mengonversi pesan menjadi integer dan tetapkan ke nilai persentase
        percentage = int(msg.payload.decode())
        print("Percentage updated to:", percentage)
    except ValueError:
        print("Invalid percentage value received")
# Fungsi untuk memproses pesan yang diterima
def process_message(message):
    try:
        # Konversi pesan angka menjadi persentase
        value = float(message)
        percentage = int(value * 100)
        send_percentage_to_frontend(percentage)
    except ValueError:
        print("Invalid message format: message is not a number")

# Fungsi untuk mengirim persentase ke frontend menggunakan REST API
def send_percentage_to_frontend(percentage):
    # Anda dapat menyesuaikan endpoint dan metode HTTP sesuai kebutuhan
    # Misalnya, menggunakan POST request untuk mengirim persentase ke frontend
    # Anda juga bisa menambahkan autentikasi atau validasi lain jika diperlukan
    # Contoh sederhana tanpa autentikasi:
    try:
        requests.post('http://localhost:5173/Battery/update_percentage', json={'percentage': percentage})
        print("Percentage sent to frontend:", percentage)
    except Exception as e:
        print("Failed to send percentage to frontend:", str(e))


percentage = 0  # Inisialisasi persentase

# Route untuk mendapatkan persentase
@app.route('/get_percentage')
def get_percentage():
    global percentage
    return jsonify({'percentage': percentage})
# Route untuk memeriksa koneksi ke MQTT broker
@app.route('/check_mqtt_connection')
def check_mqtt_connection():
    global mqtt_client
    if mqtt_client:
        connected = mqtt_client.is_connected()
    else:
        connected = False
    return jsonify({'connected': connected})

# Route untuk mengubah konfigurasi MQTT
@app.route('/change_mqtt_config', methods=['POST'])
def change_mqtt_config():
    data = request.json
    broker_address = data['broker_address']
    port = data['port']
    new_topic = data['topic']
    connect_to_mqtt(broker_address, port, new_topic)
    print("MQTT configuration updated: Broker Address =", broker_address, ", Port =", port, ", Topic =", new_topic)
    return jsonify({'status': 'success'})

# Tambahkan route untuk mengambil konfigurasi MQTT default
@app.route('/default_mqtt_config')
def default_mqtt_config():
    global topic
    return jsonify({'broker_address': "52.74.91.79", 'port': 1883, 'topic': topic})

if __name__ == '__main__':
    # Koneksi pertama ke default MQTT broker saat aplikasi dijalankan
    connect_to_mqtt("52.74.91.79", 1883, topic)
    app.run(debug=True)
