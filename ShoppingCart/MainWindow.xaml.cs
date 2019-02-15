using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoppingCart
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (ShopContext ShopContext = new ShopContext())
            {               

                foreach (var i in ShopContext.Items)
                {
                    var gridN = new Grid();

                    gridN.VerticalAlignment = VerticalAlignment.Top;
                    gridN.Background = Brushes.White;
                    gridN.Height = 420;
                    gridN.Width = 823;

                    gridN.Effect = new DropShadowEffect
                    {
                        BlurRadius = 20,
                        ShadowDepth = 1
                    };

                    var img = new Image();
                    img.Source = new BitmapImage(new Uri(i.UrlImg));
                    Thickness marginImg = img.Margin;
                    marginImg.Left = 21;
                    img.Margin = marginImg;
                    img.HorizontalAlignment = HorizontalAlignment.Left; 

                    gridN.Children.Add(img);

                    var stackPanel = new StackPanel();
                    Thickness marginStackPanel = stackPanel.Margin;
                    marginStackPanel.Left = 542;
                    marginStackPanel.Top = 70;
                    marginStackPanel.Right = 0;
                    marginStackPanel.Bottom = 70;
                    stackPanel.Margin = marginStackPanel;
                    stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    stackPanel.Width = 263;

                    var textBlockName = new TextBlock();
                    textBlockName.Text = i.Name;
                    textBlockName.FontSize = 18;
                    Thickness marginTextBlockName = textBlockName.Margin;
                    marginTextBlockName.Left = 0;
                    marginTextBlockName.Right = 0;
                    marginTextBlockName.Bottom = 5;
                    marginTextBlockName.Top = 5;
                    textBlockName.Margin = marginTextBlockName;
                    textBlockName.Foreground = Brushes.Black;

                    stackPanel.Children.Add(textBlockName);

                    var textBlockDiscription = new TextBlock();
                    textBlockDiscription.Text = i.Description;
                    textBlockDiscription.FontSize = 14;
                    textBlockDiscription.TextWrapping = TextWrapping.Wrap;
                    textBlockDiscription.Foreground = Brushes.Black;

                    stackPanel.Children.Add(textBlockDiscription);

                    var textBlockPrice = new TextBlock();
                    textBlockPrice.Text = i.Price.ToString();
                    textBlockPrice.FontSize = 20;
                    Thickness marginTextBlockPrice = textBlockPrice.Margin;
                    marginTextBlockPrice.Left = 0;
                    marginTextBlockPrice.Right = 0;
                    marginTextBlockPrice.Top = 15;
                    marginTextBlockPrice.Bottom = 15;
                    textBlockPrice.Margin = marginTextBlockPrice;
                    textBlockPrice.Foreground = Brushes.Silver;

                    stackPanel.Children.Add(textBlockPrice);

                    var buttonForCart = new Button();
                    buttonForCart.Background = Brushes.Blue;
                    buttonForCart.BorderBrush = Brushes.Blue;
                    buttonForCart.Content = "In Cart";

                    stackPanel.Children.Add(buttonForCart);

                    gridN.Children.Add(stackPanel);

                    gridAdd.Items.Add(gridN);
                }

                ShopContext.SaveChanges();
            }

            List<string> KakYgodno = new List<string>();

            for (int i = 0; i < 10; i++) 
            {
                KakYgodno.Add("SLs");
            }

            var grid = new Grid
            {
                Width = 250,
                Height = 100
            };

            
        }

		private void WindowMove(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void ExitButtonClick(object sender, RoutedEventArgs e)
		{

		}

		private void MaximazeClick(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
			}

			else if (WindowState == WindowState.Normal)
			{
				WindowState = WindowState.Maximized;
			}
		}

		private void MinimazeClick(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}            

        private void LoginEnterClick(object sender, RoutedEventArgs e)
        {

            //LOGIN

            mainPanel.Visibility = Visibility.Visible;
            windowLogin.Visibility = Visibility.Collapsed;
        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {


            //REGISTRAT



            regText.Visibility = Visibility.Collapsed;
            regTextBox.Visibility = Visibility.Collapsed;
            regEnter.Visibility = Visibility.Collapsed;
            passwordReg.Visibility = Visibility.Collapsed;

            checkText.Visibility = Visibility.Visible;
            checkTextBox.Visibility = Visibility.Visible;
            checkEnter.Visibility = Visibility.Visible;
        }

        private void SwapRegButton(object sender, RoutedEventArgs e)
        {
            if (regText.Visibility == Visibility.Collapsed && regTextBox.Visibility == Visibility.Collapsed && passwordReg.Visibility == Visibility.Collapsed && regEnter.Visibility == Visibility.Collapsed)
            {
                regText.Visibility = Visibility.Visible;
                regTextBox.Visibility = Visibility.Visible;
                regEnter.Visibility = Visibility.Visible;
                passwordReg.Visibility = Visibility.Visible;

                loginText.Visibility = Visibility.Collapsed;
                loginTextBox.Visibility = Visibility.Collapsed;
                loginEnter.Visibility = Visibility.Collapsed;
                passwordLog.Visibility = Visibility.Collapsed;
            }

            else if (loginText.Visibility == Visibility.Collapsed && loginTextBox.Visibility == Visibility.Collapsed && loginEnter.Visibility == Visibility.Collapsed && passwordLog.Visibility == Visibility.Collapsed)
            {

                loginText.Visibility = Visibility.Visible;
                loginTextBox.Visibility = Visibility.Visible;
                loginEnter.Visibility = Visibility.Visible;
                passwordLog.Visibility = Visibility.Visible;


                regText.Visibility = Visibility.Collapsed;
                regTextBox.Visibility = Visibility.Collapsed;
                regEnter.Visibility = Visibility.Collapsed;
                passwordReg.Visibility = Visibility.Collapsed;
            }


        }

        private void CheckButtonClick(object sender, RoutedEventArgs e)
        {
            //SMS CODE






            loginText.Visibility = Visibility.Visible;
            loginTextBox.Visibility = Visibility.Visible;
            loginEnter.Visibility = Visibility.Visible;
            passwordLog.Visibility = Visibility.Visible;

            checkText.Visibility = Visibility.Collapsed;
            checkTextBox.Visibility = Visibility.Collapsed;
            checkEnter.Visibility = Visibility.Collapsed;
        }
    }
}
