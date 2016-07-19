using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinggaProject.emgu_support
{
    public class EmguBaseForm : Form
    {
        public virtual void addExplanationText(string text, bool isAppend)
        {

        }

        public void setElementStatus(Control element, bool isEnabled)
        {
            if (InvokeRequired) {
                this.Invoke(new Action<Control, bool>(setElementStatus), new object[] { element, isEnabled });
                return;
            }
            element.Enabled = isEnabled;
        }
    }
}
