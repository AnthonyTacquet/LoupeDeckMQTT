# LoupeDeckMQTT
## General Settings
This is an example setting:  
Name: Send MQTT command  
Hostname: 192.168.1.51  
Port: 1883  
Topic Publish: loopdeck/in  
Publish Payload: Button is pressed  
Topic Subscribe Base: loopdeck/out  
Base Text: Output  

Explenation:  
The hostname and port settings are default settings for a MQTT connection.  
When the button is pressed the text from "Topic Payload" will be send to the "Topic Publish" Topic.  
The "Topic Subscribe Base" setting is used for recieving payloads from the broker.  
This is a base topic because only using the following subtopics will have affect on the button.  
- /text (loopdeck/out/text): when sending a string to this topic the recieved text will be displayed next to the "Base Text" on the button   
- /image (loopdeck/out/image): when sending a string (Base64) of the image to this topic the recieved image will be displayed on top of the text, when there is no text (empty string) send to the /text topic the image will be displayed in big*  
- /text_color (loopdeck/out/text_color): when sending a string with format (Red,Green,Blue,Alpha OR Example: 24,80,120,254) is send to this topic the text color will be changed.  

*IMPORTANT  
When a text is set the image can only be 32x32  
When the text is empty the image can be 64x64  
## Publish to a topic button
__The documention is not finished, for any question you can contact me__
## Publish to a topic adjust button
__The documention is not finished, for any question you can contact me__
## Subscribe to a topic button
__The documention is not finished, for any question you can contact me__