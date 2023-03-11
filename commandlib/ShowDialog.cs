using System;
using System.Windows.Forms;

namespace commandlib
{
    public class ShowDialog : ActionBase
    {
        public enum DialogType
        {
            Info,
            Warning,
            Error
        }

        #region helpers

        public static ShowDialog GetCommandFrom(string text)
        {
            return new ShowDialog()
            {
                Name = $"ShowDialog: {text}",
                Message = text,
                MessageType = DialogType.Info
            };
        }

        #endregion

        #region property

        public string Message { get; set; }

        public DialogType MessageType { get; set; } = DialogType.Info;

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            Console.WriteLine($"\tShow message dialog : {this.Message} ");

            ShowDialogInternal();

        }

        #endregion

        #region private methods

        private void ShowDialogInternal()
        {

            switch (this.MessageType)
            {
                case DialogType.Info:
                    MessageBox.Show(this.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case DialogType.Warning:
                    MessageBox.Show(this.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case DialogType.Error:
                    MessageBox.Show(this.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show(this.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
            }
        }

        #endregion
    }
}
