using System.IO;

namespace WpfApp.ViewModels;

public class HomeVM
{
    public HomeVM()
    {
        Document = File.ReadAllText(@"..\..\..\..\README.md");
    }

    public string Document { get; }
}
