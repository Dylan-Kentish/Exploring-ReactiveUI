using ModernWpf.Controls;
using System.Windows;

namespace WpfApp.Controls;

/// <summary>
/// Interaction logic for WatermarkTextBox.xaml
/// </summary>
public partial class WatermarkTextBox
{
    public WatermarkTextBox()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(WatermarkTextBox),
        new FrameworkPropertyMetadata(
            string.Empty, 
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
        nameof(Watermark),
        typeof(string),
        typeof(WatermarkTextBox));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(Symbol),
        typeof(WatermarkTextBox));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Watermark
    {
        get => (string)GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public Symbol Icon
    {
        get => (Symbol)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}