using System.Collections;
using System.Windows;
using System.Windows.Markup;

namespace WpfApp.Controls;

/// <summary>
/// Interaction logic for UniformItemsScrollViewer.xaml
/// </summary>
[ContentProperty("ItemTemplate")]
public partial class UniformItemsScrollViewer
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        "ItemsSource", typeof(IEnumerable), typeof(UniformItemsScrollViewer));

    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
        "Columns",
        typeof(int),
        typeof(UniformItemsScrollViewer));

        
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
        "Rows",
        typeof(int),
        typeof(UniformItemsScrollViewer));

    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        "ItemTemplate",
        typeof(DataTemplate),
        typeof(UniformItemsScrollViewer));

    public UniformItemsScrollViewer()
    {
        InitializeComponent();
    }

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty) as IEnumerable;
        set => SetValue(ItemsSourceProperty, value);
    }
        
    public int? Columns
    {
        get => GetValue(ColumnsProperty) as int?;
        set => SetValue(ColumnsProperty, value);
    }
        
    public int? Rows
    {
        get => GetValue(RowsProperty) as int?;
        set => SetValue(RowsProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty) as DataTemplate;
        set => SetValue(ItemTemplateProperty, value);
    }
}