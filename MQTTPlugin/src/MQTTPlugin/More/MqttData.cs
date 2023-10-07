namespace Loupedeck.MQTTPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls.WebParts;

    using MQTTnet.Client;

    using MQTTnet;

    public class MqttData
    {

        public async Task MqttPublishAsync(ActionEditorActionParameters actionParameters)
        {
            var mqttFactory = new MqttFactory();
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(actionParameters.Parameters.GetValue("hostname"), int.Parse(actionParameters.Parameters.GetValue("port")))
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(actionParameters.Parameters.GetValue("topic"))
                    .WithPayload(actionParameters.Parameters.GetValue("payload"))
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                await mqttClient.DisconnectAsync();
            }
        }

        public async void MqttSubscribeAsync(ActionEditorActionParameters actionParameters, ManualResetEvent mqttClientThreadFinished, Func<MqttApplicationMessageReceivedEventArgs, Task> func)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(actionParameters.Parameters.GetValue("hostname"), int.Parse(actionParameters.Parameters.GetValue("port")))
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += func;

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(SubscribeTopicWithHashTag(actionParameters.Parameters.GetValue("topic_subscribe")));
                        })
                    .Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to topic.");

                mqttClientThreadFinished.WaitOne();
            }
        }

        private string SubscribeTopicWithHashTag(string topic)
        {
            if (topic.EndsWith("/#"))
                return topic;
            else if (topic.EndsWith("/"))
                return topic + "#";
            else
                return topic + "/#";
        }
    }
}
