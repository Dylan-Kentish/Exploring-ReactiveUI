using ModernWpf.Controls;
using System.Windows;
using System.Linq;

namespace WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : ModernWpf.Controls.NavigationView
    {
        public static readonly DependencyProperty SelectedTagProperty =
            DependencyProperty.Register("SelectedTag", typeof(string), typeof(NavigationView), new PropertyMetadata(default));

        public NavigationView()
        {
            InitializeComponent();
        }

        public string SelectedTag
        {
            get { return (string)GetValue(SelectedTagProperty); }
            set { SetValue(SelectedTagProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            {
                if (e.Property == SelectedItemProperty &&
                    e.NewValue is NavigationViewItem item &&
                    item.Tag is string tag)
                {
                    SelectedTag = tag;
                }
            }
            {
                if (e.Property == SelectedTagProperty &&
                    e.NewValue is string tag)
                {
                    var navItems = MenuItems.Cast<NavigationViewItem>();
                    SelectedItem = navItems.FirstOrDefault(item => item.Tag is string other && other == tag);
                }
            }
        }
    }
}
