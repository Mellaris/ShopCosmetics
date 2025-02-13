using Avalonia.Controls;
using Avalonia.Interactivity;
using Cosmetics.Context;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace Cosmetics
{
    public partial class MainWindow : Window
    {
        private readonly User9Context _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new User9Context(new DbContextOptions<User9Context>());
        }

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Text;

            var user = _context.Users
                .Include(u => u.UserroleNavigation)
                .FirstOrDefault(u => u.Userlogin == login && u.Userpassword == password);

            if (user != null)
            {
                StartWindow startWindow = new StartWindow(user);
                startWindow.Show();
                Close();
            }
            else
            {
                
            }
        }

        private void OnGuestClicked(object sender, RoutedEventArgs e)
        {
            StartWindow startWindow = new StartWindow(null);
            startWindow.Show();
            Close();
        }
    }
}