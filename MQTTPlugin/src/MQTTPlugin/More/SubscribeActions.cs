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
                string payload = "";
                if (e.ApplicationMessage.Payload != null)
                    payload = System.Text.Encoding.Default.GetString(e.ApplicationMessage.Payload);

                if (topic.EndsWith("/text"))
                    myEvents(this, new MyEventArgs(SubscriptionType.TEXT, payload));
                else if (topic.EndsWith("/image"))
                    myEvents(this, new MyEventArgs(SubscriptionType.IMAGE, payload));
                else if (topic.EndsWith("/text_color"))
                    myEvents(this, new MyEventArgs(SubscriptionType.TEXT_COLOR, payload));

                return Task.CompletedTask;
            };

        }
    }
}
