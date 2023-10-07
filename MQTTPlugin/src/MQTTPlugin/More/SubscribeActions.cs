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

        public Func<MqttApplicationMessageReceivedEventArgs, Task> CreateFunction()
        {
            return e =>
            {
                string topic = e.ApplicationMessage.Topic;

                //if (topic.EndsWith("/text"))
                    

                return Task.CompletedTask;
            };

        }
    }
}
