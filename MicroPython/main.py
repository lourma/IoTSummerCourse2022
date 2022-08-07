# main.py -- put your code here!
import ujson
global rollMax
rollMax = 45
global rollMin
rollMin = -45
global rollAll
rollAll = 0
global change
change = 4

def sub_cb(topic, msg):
    print((topic, msg))
    if topic == topic_command:
        if("sensor" in msg):
            parsed = ujson.loads(msg)
            print(parsed)
            if(parsed["sensor"] == "ROLL"):
                if(parsed["setting"] == "MIN"):
                    global rollMin
                    rollMin = float(parsed["value"])
                elif(parsed["setting"] == "MAX"):
                    global rollMax
                    rollMax = float(parsed["value"])
                elif(parsed["setting"] == "ALL"):
                    global rollAll
                    rollAll = float(parsed["value"])
                elif(parsed["setting"] == "CHG"):
                        global change
                        change = float(parsed["value"])
                elif(parsed["setting"] == "GetValues"):
                    client.publish(topic_data, b'ROLL_ALL ' + str(rollAll))
                    # client.publish(topic_data, b'{"sensor":"ROLL","setting":"MIN", "value":'+rollMin+'},'+'{"sensor":"ROLL","setting":"MAX", "value":'+rollMin+'},'+'{"sensor":"ROLL","setting":"ALL", "value":'+rollAll+'}')

        elif msg == b'red':
            pycom.rgbled(0xFF0000)
            # client.publish(topic_data, b'Pycom is ' + msg)
        elif msg == b'green':
            pycom.rgbled(0x00ff00)
            # client.publish(topic_data, b'Pycom is ' + msg)
        elif msg == b'blue':
            pycom.rgbled(0x0000ff)
            # client.publish(topic_data, b'Pycom is ' + msg)
        elif msg == b'yellow':
            pycom.rgbled(0xffff00)
            # client.publish(topic_data, b'Pycom is ' + msg)
        else:
            pycom.rgbled(0x000000)
        #  client.publish(topic_data, b'Pycom got ' + msg)

def connect_and_subscribe():
  global client_id, mqtt_server, topic_command
  client = MQTTClient(client_id, mqtt_server, port=mqtt_port, keepalive=60)
  client.set_callback(sub_cb)
  client.connect()
  client.subscribe(topic_command)
  print('Connected to %s MQTT broker, subscribed to %s topic' % (mqtt_server, topic_command))
  print('Connected to broker')
  return client

def restart_and_reconnect():
  print('Failed to connect to MQTT broker.')
  # time.sleep(10)
  # machine.reset()


try:
  py = Pycoproc()
  if py.read_product_id() != Pycoproc.USB_PID_PYSENSE:
    raise Exception('Not a Pysense')
  li = LIS2HH12(py)

  client = connect_and_subscribe()
  sent_neg = 0
  sent_pos = 0

except OSError as e:
  restart_and_reconnect()

while True:
  try:
    client.check_msg()

#Original
    if(rollAll == 1):
        if((roll + change < li.roll()) or (roll - change > li.roll()) or (pitch + change < li.pitch()) or (pitch - change > li.pitch())):
          roll = li.roll()
          pitch = li.pitch()
          client.publish(topic_data, str(roll)+":"+str(pitch))
          print(b'Sent:' + str(roll) + ':' + str(pitch))

    else:
        if((sent_neg == 0) and (rollMin > li.roll())):
          roll = li.roll()
          pitch = li.pitch()
          client.publish(topic_data, str(roll)+":"+str(pitch))
          print(b'Sent: Roll ' + str(roll) + " th:" + str(rollMin))
          sent_neg = 1

        if((sent_neg == 1) and(rollMin < li.roll())):
          sent_neg = 0

        if((sent_pos == 0) and (rollMax < li.roll())):
          roll = li.roll()
          pitch = li.pitch()
          client.publish(topic_data, str(roll)+":"+str(pitch))
          print(b'Sent: Roll ' + str(roll) + " th:" + str(rollMax))
          sent_pos = 1

        if((sent_pos == 1) and(rollMax > li.roll())):
          sent_pos = 0


    # if (time.time() - last_message) > message_interval:
    #   print('Sent: Hello #%d' % counter)
    #   outmsg = b'Hello #%d' % counter
    #
    #   client.publish(topic_data, outmsg)
    #   last_message = time.time()
    #   counter += 1
  except OSError as e:
    restart_and_reconnect()
