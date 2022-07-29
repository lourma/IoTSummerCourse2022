# boot.py -- run on boot-up

# Complete project details at https://RandomNerdTutorials.com

import time
from umqttsimple import MQTTClient
import ubinascii
import machine
import micropython
from network import WLAN
import pycom
from myconfig import config
import ujson

from pycoproc_2 import Pycoproc
from LIS2HH12 import LIS2HH12

import gc
gc.collect()

mqtt_server = config.MQTT_BROKER
mqtt_port = config.MQTT_PORT

client_id = 'MyPyCom'

topic_server = config.TOPIC_COMMAND
topic_pycom = config.TOPIC_DATA

last_message = 0
message_interval = 5
counter = 0
inmsg =''

wlan = WLAN(mode = WLAN.STA)
wlan.connect(config.WIFI_SSID, auth=(WLAN.WPA2, config.WIFI_PASS), timeout=5000)

while wlan.isconnected() == False:
  pass
pycom.rgbled(0x010101)
print('Connection successful')
print(wlan.ifconfig())
