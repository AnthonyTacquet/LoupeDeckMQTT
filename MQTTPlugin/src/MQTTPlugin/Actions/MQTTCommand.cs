namespace Loupedeck.MQTTPlugin.Actions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Loupedeck.MQTTPlugin.More;
    using System.Drawing;
    using System.IO;
    using MQTTnet;
    using MQTTnet.Client;

    public class MQTTCommand : ActionEditorCommand
    {
        private ManualResetEvent mqttClientThreadFinished = new ManualResetEvent(false);
        private MqttData _data = new MqttData();
        private SubscribeActions _actions = new SubscribeActions();

        private string _text = "Not Loaded (Press to load)";
        private BitmapColor _color = BitmapColor.White;
        private BitmapImage _image = null;
        private string _baseText = "";

        private bool _start = false;

        // Initializes the command class.
        public MQTTCommand()
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
                new ActionEditorTextbox(name: "payload", labelText: "Publish Payload:")
                .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "topic_subscribe", labelText: "Topic Subscribe Base:")
            .SetRequired());

            this.ActionEditor.AddControl(
                new ActionEditorTextbox(name: "base_text", labelText: "Base Text:", description: "Text that will always be displayed."));

        }

        // This method is called when the user executes the command.
        protected override Boolean RunCommand(ActionEditorActionParameters actionParameters)
        {
            _data.MqttPublishAsync(actionParameters);

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
            else if (e.Type == SubscriptionType.IMAGE)
                _image = BitmapImage.FromBase64String(e.Value);
            else if (e.Type == SubscriptionType.TEXT_COLOR)
            {
                string[] vals = e.Value.Split(',');
                if (vals.Length != 4)
                    _text = "Format: Red,Green,Blue,Alpha";
                else
                    _color = new BitmapColor(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]), int.Parse(vals[3]));
            }

            this.ActionImageChanged();

        }

        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, Int32 imageWidth, Int32 imageHeight)
        {
            using (var bitmapBuilder = new BitmapBuilder(imageWidth, imageHeight))
            {                
                if (_image == null || !_start) // Only show text or nothing if no text is given
                {
                    bitmapBuilder.DrawText($"{actionParameters.Parameters.GetValue("base_text")}{_text}", color: _color);
                }
                else if (_image != null && _baseText == "" && _text == "") // Only show an image
                {
                    _image.Crop(0, 0, 64, 64); // The image can only be 64x64
                    bitmapBuilder.DrawImage(_image, (imageWidth - _image.Width) / 2, (imageHeight - _image.Height) / 2);
                }
                else // Return both text and image
                {
                    _image.Crop(0, 0, 32, 32); // The image can only be 32x32
                    bitmapBuilder.DrawImage(_image, (imageWidth - _image.Width) / 2, 10);
                    bitmapBuilder.DrawText($"{actionParameters.Parameters.GetValue("base_text")}{_text}", (imageWidth - _image.Width) / 2, 52, _image.Width, 10, color: _color);
                }

                return bitmapBuilder.ToImage();
            }
        }

    }
}
