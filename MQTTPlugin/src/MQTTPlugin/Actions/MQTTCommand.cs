namespace Loupedeck.MQTTPlugin.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MQTTnet;
    using MQTTnet.Client;

    public class MQTTCommand : ActionEditorCommand
    {
        private ManualResetEvent mqttClientThreadFinished = new ManualResetEvent(false);
        MqttData data = new MqttData();

        // Initializes the command class.
        public MQTTCommand()
        {

            this.DisplayName = "Publish To A Topic";
            this.Description = "Publish To A Topic";
            this.GroupName = "";


            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "hostname", labelText: "Hostname:"/*,"Select Scene name"*/)
                    .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "port", labelText: "Port:", description: "Default: 1883"));

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_publish", labelText: "Topic Publish:")
                .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "payload", labelText: "Publish Payload:")
                .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_subscribe", labelText: "Topic Subscribe Base:")
            .SetRequired());
        }

        // This method is called when the user executes the command.
        protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
        {
            data.MqttPublishAsync(actionParameters);

            // PluginLog.Info($"Counter value is {this._counter}"); // Write the current counter value to the log file.
            return true;
        }

        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override string GetCommandDisplayName(ActionEditorActionParameters actionEditorActionParameters) =>
            $" To Do ";

       
       
    }
}
