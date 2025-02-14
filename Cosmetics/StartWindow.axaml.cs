using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Cosmetics.Context;
using Cosmetics.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmetics;

public partial class StartWindow : Window
{
    private User _currentUser;
    private readonly User9Context _context;
    public List<ProductViewModel> _allProducts = new List<ProductViewModel>();
    private List<OrderItem> _currentOrder = new List<OrderItem>();
    public StartWindow()
    {
        InitializeComponent();
    }

    public StartWindow(User user)
    {
        InitializeComponent();
        _currentUser = user;
        _context = new User9Context(new DbContextOptions<User9Context>());
        SetupUI();
        LoadProducts();
    }

    private void SetupUI()
    {
        if (_currentUser != null)
        {
            UserFullName.Text = $"{_currentUser.Usersurname} {_currentUser.Username} {_currentUser.Userpatronymic}";

            ClientPanel.IsVisible = _currentUser.Userrole == 2;
            ManagerPanel.IsVisible = _currentUser.Userrole == 1;
            AdminPanel.IsVisible = _currentUser.Userrole == 3;
        }
        else
        {
            UserFullName.Text = "Гость";
        }
    }
    private void LoadProducts()
    {
        var products = _context.Products
            .Include(p => p.Productmanufacturer) // Подключаем производителя
            .Select(p => new
            {
                ProductName = p.Productname,
                ProductDescription = p.Productdescription,
                ManufacturerName = p.Productmanufacturer.Manufacturername, // Получаем название производителя
                Price = p.Productcost,
                Discount = p.Productdiscountamount,
                DiscountText = p.Productdiscountamount.HasValue ? $"{p.Productdiscountamount}% скидка" : "Без скидки",
                DiscountColor = (p.Productdiscountamount.HasValue && p.Productdiscountamount > 15) ? "#7FFF00" : "Transparent",
                FormattedPrice = p.Productdiscountamount.HasValue && p.Productdiscountamount > 0
                    ? $"{p.Productcost - (p.Productcost * p.Productdiscountamount / 100):0.00} ₽ ( ~{p.Productcost:0.00} ₽~ )"
                    : $"{p.Productcost:0.00} ₽"
            })
            .ToList();

        ProductList.ItemsSource = products;

        _allProducts = _context.Products
        .Include(p => p.Productmanufacturer)
        .Select(p => new ProductViewModel
        {
            ProductName = p.Productname,
            ProductDescription = p.Productdescription,
            ManufacturerName = p.Productmanufacturer.Manufacturername,
            Price = p.Productcost,
            Discount = p.Productdiscountamount,
            FinalPrice = p.Productdiscountamount.HasValue && p.Productdiscountamount > 0
    ? Math.Round(p.Productcost * (1 - (decimal)p.Productdiscountamount / 100), 2) // Округляем до 2 знаков
    : p.Productcost,
            DiscountText = p.Productdiscountamount.HasValue ? $"{p.Productdiscountamount}% скидка" : "Без скидки",
            DiscountColor = (p.Productdiscountamount.HasValue && p.Productdiscountamount > 15) ? "#7FFF00" : "Transparent",
            FormattedPrice = p.Productdiscountamount.HasValue && p.Productdiscountamount > 0
                ? $"{p.Productcost - (p.Productcost * (decimal)p.Productdiscountamount / 100):0.00} ₽ ( ~{p.Productcost:0.00} ₽~ )"
                : $"{p.Productcost:0.00} ₽"
        })
        .ToList();
        
        ApplyFilters();
    }
    private void OnAddToOrderClicked(object sender, RoutedEventArgs e)
    {
        var selectedProduct = ProductList.SelectedItem as Product; // Приводим тип, если возможно

        if (ProductList.SelectedItem != null) // Проверяем, что выбран продукт
        {
            // Проверяем, существует ли уже такой товар в заказе
            var existingItem = _currentOrder.FirstOrDefault(item => item.Product.Productarticlenumber == selectedProduct.Productarticlenumber);

            if (existingItem != null)
            {
                // Если товар уже в заказе, увеличиваем количество
                existingItem.Quantity++;
            }
            else
            {
                // Если товара нет в заказе, добавляем новый товар
                _currentOrder.Add(new OrderItem { Product = selectedProduct, Quantity = 1 });
            }

            // Обновляем видимость кнопки для просмотра заказа
            ViewOrderButton.IsVisible = _currentOrder.Any();
        }
        ViewOrderButton.IsVisible = _currentOrder.Any();
    }
    private async void OnViewOrderClicked(object sender, RoutedEventArgs e)
    {
        var orderWindow = new OrderWindow(_currentOrder);
        await orderWindow.ShowDialog(this);
    }
    private void ApplyFilters()
    {
        var filteredProducts = _allProducts.AsEnumerable();
        int count = _allProducts.Count;
        // Фильтрация по скидке
        if (DiscountFilter.SelectedIndex > 0)
        {
            switch (DiscountFilter.SelectedIndex)
            {
                case 1:
                    filteredProducts = filteredProducts.Where(p => p.Discount is >= 0 and < 10);
                    break;
                case 2:
                    filteredProducts = filteredProducts.Where(p => p.Discount is >= 10 and < 15);
                    break;
                case 3:
                    filteredProducts = filteredProducts.Where(p => p.Discount >= 15);
                    break;
            }
        }

        // Поиск по названию
        if (!string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            string searchText = SearchBox.Text.ToLower();
            filteredProducts = filteredProducts.Where(p => p.ProductName.ToLower().Contains(searchText));
        }

        // Сортировка по цене (учитываем FinalPrice)
        if (SortOrder.SelectedIndex == 0)
        {
            filteredProducts = filteredProducts.OrderBy(p => p.FinalPrice);
        }
        else if (SortOrder.SelectedIndex == 1)
        {
            filteredProducts = filteredProducts.OrderByDescending(p => p.FinalPrice);
        }

        ProductCountText.Text = $"{filteredProducts.Count()} из {count}";
        // Обновляем отображение списка
        ProductList.ItemsSource = filteredProducts.ToList();
    }
    private void OnFilterChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }
    private void OnLogoutClicked(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }
}
public class ProductViewModel
{
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string ManufacturerName { get; set; }
    public decimal Price { get; set; }
    public short? Discount { get; set; }
    public string DiscountText { get; set; }
    public string DiscountColor { get; set; }
    public string FormattedPrice { get; set; }
    public decimal FinalPrice { get; set; } // Итоговая цена после скидки
}
public class OrderItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
