namespace Loupedeck.MQTTPlugin.More
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MQTTnet.Client;

    public class SubscribeActions
    {

        public delegate void MyEventHandler(object source, MyEventArgs args);
        public event MyEventHandler myEvents;
        public Func<MqttApplicationMessageReceivedEventArgs, Task> CreateFunction()
        {
            return e =>
            {
                string topic = e.ApplicationMessage.Topic;

                if (topic.EndsWith("/text"))
                {
                    string payload = System.Text.Encoding.Default.GetString(e.ApplicationMessage.Payload);
                    myEvents(this, new MyEventArgs(SubscriptionType.TEXT, payload));
                }


                return Task.CompletedTask;
            };

        }
    }
}
