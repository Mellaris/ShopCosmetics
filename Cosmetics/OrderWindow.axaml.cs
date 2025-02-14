using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace Cosmetics;

public partial class OrderWindow : Window
{
    public OrderWindow()
    {
        InitializeComponent();
    }
    public OrderWindow(List<OrderItem> orderItems)
    {
        InitializeComponent();
        OrderList.ItemsSource = orderItems.Select(item => new
        {
            Name = item.Product.Productname,
        }).ToList();
    }

    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}