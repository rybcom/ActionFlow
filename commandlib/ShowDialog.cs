using System;
using System.Windows.Forms;

namespace commandlib
{
    public class ShowDialog : ActionBase
    {
        public enum Type
        {
            Info,
            Warning,
            Error
        }

        #region property

        public string Message { get; set; }

        public Type MessageType { get; set; } = Type.Info;

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
                case Type.Info:
                    MessageBox.Show(this.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    break;

                case Type.Warning:
                    MessageBox.Show(this.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case Type.Error:
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
