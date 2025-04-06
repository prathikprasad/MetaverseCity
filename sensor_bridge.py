# sensor_bridge.py
import requests
from bs4 import BeautifulSoup
from flask import Flask, jsonify
import threading
import time

app = Flask(__name__)
url = "http://10.27.239.15:80"
latest_data = {"temperature": 0, "humidity": 0, "timestamp": 0}

def fetch_sensor_data():
    global latest_data
    while True:
        try:
            response = requests.get(url, timeout=5)
            soup = BeautifulSoup(response.text, 'html.parser')
            
            for p in soup.find_all("p"):
                text = p.get_text(strip=True)
                if "Temperature:" in text:
                    latest_data["temperature"] = float(text.split(":")[1].split("°")[0].strip())
                elif "Humidity:" in text:
                    latest_data["humidity"] = float(text.split(":")[1].split("%")[0].strip())
            
            latest_data["timestamp"] = time.time()
            print(f"🌞 Updated: {latest_data}")  # Terminal logging
            
        except Exception as e:
            print(f"⚠️ Sensor Error: {e}")
        time.sleep(1)

@app.route('/api/solar')
def solar_data():
    return jsonify({
        "temperature": latest_data["temperature"],
        "humidity": latest_data["humidity"],
        "timestamp": latest_data["timestamp"]
    })

if __name__ == '__main__':
    # Start sensor polling thread
    threading.Thread(target=fetch_sensor_data, daemon=True).start()
    
    # Start Flask API
    app.run(host='0.0.0.0', port=5000, debug=False)