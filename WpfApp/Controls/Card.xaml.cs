using System.Windows;
using System.Windows.Markup;

namespace WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    [ContentProperty("AdditionalContent")]
    public partial class Card
    {
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(Card));


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Card));

        public Card()
        {
            InitializeComponent();
        }
        
        public object AdditionalContent
        {
            get => GetValue(AdditionalContentProperty);
            set => SetValue(AdditionalContentProperty, value);
        }

        public string? Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }
    }
}
