namespace Loupedeck.MQTTPlugin.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Loupedeck.MQTTPlugin.More;

    public class MQTTAdjustments : ActionEditorAdjustment
    {
        private ManualResetEvent mqttClientThreadFinished = new ManualResetEvent(false);
        private MqttData _data = new MqttData();
        private SubscribeActions _actions = new SubscribeActions();
        private string _text = "Not Loaded (Press to load)";
        private string _baseText = "";
        private bool _start = false;

        // Initializes the command class.
        public MQTTAdjustments() : base (true)
        {

            this.DisplayName = "Publish To A Topic";
            this.Description = "Publish To A Topic";
            this.GroupName = "";

            _actions.myEvents += Recieved;

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "hostname", labelText: "Hostname:")
                    .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "port", labelText: "Port:", description: "Default: 1883"));

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_publish", labelText: "Topic Publish:")
                .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_subscribe", labelText: "Topic Subscribe Base:")
            .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "base_text", labelText: "Base Text:", description: "Text that will always be displayed."));
        }

        protected override Boolean ApplyAdjustment(ActionEditorActionParameters actionParameters, Int32 diff)
        {
            this._text = "" + diff;
            this.AdjustmentValueChanged();

            _data.MqttPublishAdjustmentAsync(actionParameters, _text);

            if (_start)
                return true;

            _start = true;
            _text = "Loaded, Waiting...";
            this.ActionImageChanged();

            _data.MqttSubscribeAsync(actionParameters, mqttClientThreadFinished, _actions.CreateFunction());

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
            $"{actionEditorActionParameters.Parameters.GetValue("base_text")}\n{_text}";


    }
}