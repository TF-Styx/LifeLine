using LifeLine.Utils.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeLine.Services.DialogServices
{
    public interface IDialogService
    {
        bool ShowMessage(string Message, string Title = "Предупреждение", MessageButtons messageButtons = MessageButtons.OK);
        MessageButtons ShowMessageButton(string Message, string Title = "Предупреждение", MessageButtons messageButtons = MessageButtons.OK);
    }
}
