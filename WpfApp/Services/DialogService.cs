using System.Windows;

namespace WpfApp.Services;

public class DialogService : IDialogService
{
    public void ShowDialog(string message)
    {
        _ = MessageBox.Show(message);
    }
}