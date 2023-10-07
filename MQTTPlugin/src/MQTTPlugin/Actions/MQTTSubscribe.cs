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

    public class MQTTSubscribe : ActionEditorCommand
    {
        private ManualResetEvent mqttClientThreadFinished = new ManualResetEvent(false);
        MqttData data = new MqttData();


        // Initializes the command class.
        public MQTTSubscribe()
        {
            this.DisplayName = "Subscribe To A Topic";
            this.Description = "Subscribe To A Topic";
            this.GroupName = "";


            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "hostname", labelText: "Hostname:"/*,"Select Scene name"*/)
                    .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "port", labelText: "Port:", description: "Default: 1883"));

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_subscribe", labelText: "Topic Subscribe Base:")
            .SetRequired()); ;
        }

        protected override Boolean OnLoad()
        {
            Thread thread = new Thread(() =>
            {
                data.MqttSubscribeAsync(); // Pass the parameter to the method
            });

            thread.Start();
            return true;
            return true;
        }

        // This method is called when the user executes the command.
        protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
        {
            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.
            // PluginLog.Info($"Counter value is {this._counter}"); // Write the current counter value to the log file.

            return true;
        }

        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override string GetCommandDisplayName(ActionEditorActionParameters actionEditorActionParameters) =>
            $" To Do ";

        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, Int32 imageWidth, Int32 imageHeight) 
        {
        }



    }
}

