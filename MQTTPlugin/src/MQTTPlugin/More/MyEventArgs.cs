namespace Loupedeck.MQTTPlugin.More
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MyEventArgs : EventArgs
    {
        public SubscriptionType Type { get; }
        public string Value { get; }

        public MyEventArgs(SubscriptionType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
