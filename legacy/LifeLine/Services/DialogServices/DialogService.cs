using LifeLine.MVVM.View.Windows;
using LifeLine.Utils.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.Services.DialogServices
{
    public class DialogService : IDialogService
    {
        public bool ShowMessage(string Message, string Title = "Предупреждение", MessageButtons messageButtons = MessageButtons.OK)
        {
            NotificationWindow notificationWindow = new(Message, Title, messageButtons);
            notificationWindow.ShowDialog();
            return notificationWindow.Result;
        }

        public MessageButtons ShowMessageButton(string Message, string Title = "Предупреждение", MessageButtons messageButtons = MessageButtons.OK)
        {
            NotificationWindow notificationWindow = new(Message, Title, messageButtons);
            notificationWindow.ShowDialog();
            return notificationWindow.ResultButton;
        }
    }
}
