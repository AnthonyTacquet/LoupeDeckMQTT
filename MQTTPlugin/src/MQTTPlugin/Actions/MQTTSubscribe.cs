namespace Loupedeck.MQTTPlugin.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MQTTnet.Client;

    using MQTTnet;
    using Loupedeck.MQTTPlugin.More;

    public class MQTTSubscribe : ActionEditorCommand
    {
        private ManualResetEvent mqttClientThreadFinished = new ManualResetEvent(false);
        private MqttData _data = new MqttData();
        private SubscribeActions _actions = new SubscribeActions();
        private string _text = "Not Loaded (Press to load)";
        private bool _start = false;

        // Initializes the command class.
        public MQTTSubscribe()
        {
            this.DisplayName = "Subscribe To A Topic";
            this.Description = "Subscribe To A Topic";
            this.GroupName = "";

            _actions.myEvents += Recieved;

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "hostname", labelText: "Hostname:")
                    .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "port", labelText: "Port:", description: "Default: 1883"));

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_subscribe", labelText: "Topic Subscribe Base:")
            .SetRequired()); ;
        }

        // This method is called when the user executes the command.
        protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
        {
            if (_start)
                return true;

            _start = true;
            _text = "Loaded, Waiting...";
            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.

            _data.MqttSubscribeAsync(actionParameters, mqttClientThreadFinished, _actions.CreateFunction());

            // PluginLog.Info($"Counter value is {this._counter}"); // Write the current counter value to the log file.
            return true;
        }

        private void Recieved(object source, MyEventArgs e)
        {
            if (e.Type == SubscriptionType.TEXT)
                _text = e.Value;

            this.ActionImageChanged();

        }

        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override string GetCommandDisplayName(ActionEditorActionParameters actionEditorActionParameters) =>
            $"{_text}";
        /*
        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, Int32 imageWidth, Int32 imageHeight) 
        {
        }*/



    }
}

