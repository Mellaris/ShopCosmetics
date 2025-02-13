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
    private List<Product> _allProducts = new List<Product>();
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
        
        ApplyFilters();
    }
    private void ApplyFilters()
    {
        
        var filteredProducts = _allProducts.AsQueryable();

        // Фильтрация по поиску
        if (!string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            string searchText = SearchBox.Text.ToLower();
            filteredProducts = filteredProducts.Where(p => p.ProductName.ToLower().Contains(searchText));
            ProductList.ItemsSource = filteredProducts.ToList();
        }

        // Фильтрация по скидке
        if (DiscountFilter.SelectedIndex > 0)
        {
            filteredProducts = DiscountFilter.SelectedIndex switch
            {
                1 => filteredProducts.Where(p => p.Discount >= 0 && p.Discount < 10),
                2 => filteredProducts.Where(p => p.Discount >= 10 && p.Discount < 15),
                3 => filteredProducts.Where(p => p.Discount >= 15),
                _ => filteredProducts
            };
            ProductList.ItemsSource = filteredProducts.ToList();
        }
        // Сортировка по цене
        if (SortOrder.SelectedIndex == 0)
        {
            filteredProducts = filteredProducts.OrderBy(p => p.Price);
            ProductList.ItemsSource = filteredProducts.ToList();
        }
        else if (SortOrder.SelectedIndex == 1)
        {
            filteredProducts = filteredProducts.OrderByDescending(p => p.Price);
            ProductList.ItemsSource = filteredProducts.ToList();
        }

       
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
