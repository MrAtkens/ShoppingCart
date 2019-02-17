using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;


namespace ShoppingCart
{

    public partial class MainWindow : Window
    {
        private static int code;
        private List<Item> itemsInCart;
        private List<Item> itemsInCartForUi;
        private int id;
        private Dictionary<int, int> dictionaryCount;
        private double amountPrice;

        public MainWindow()
        {            
            InitializeComponent();

            dictionaryCount = new Dictionary<int, int>();

            itemsInCart = new List<Item>();
            itemsInCartForUi = new List<Item>();

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

                    var img = new System.Windows.Controls.Image();
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
                    buttonForCart.Name = "n" + i.ItemId.ToString();
                    buttonForCart.Click += ButtonClickForAddInCart;

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
            Environment.Exit(0);
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
            using(ShopContext shopContext = new ShopContext())
            {
                foreach(var user in shopContext.Users)
                {
                    if(loginTextBox.Text == user.Phone && passwordLog.Password== user.Password)
                    {
                        mainPanel.Visibility = Visibility.Visible;
                        windowLogin.Visibility = Visibility.Collapsed;
                    }
                }
                loginTextBox.Text = null;
                passwordLog.Password = null;
            }
            //LOGIN
            
        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            
            string resultPage = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.mobizon.kz/service/Message/SendSmsMessage?recipient=" + regTextBox.Text + "&text=" + GenerateRandomCode() + "&apiKey=kz21f62b9387cebabe7cbbafdcf32af0ad83e4d32062d299e9fe19d795fac845f78694");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true))
            {
                resultPage = sr.ReadToEnd();
                sr.Close();
            }
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
            if (checkTextBox.Text == code.ToString())
            {
                using (ShopContext shopContext = new ShopContext())
                {
                    shopContext.Users.Add(new User
                    {
                        FullName="test testулы",
                        Phone = regTextBox.Text,                        
                        Password = passwordReg.Password
                    });
                    shopContext.SaveChanges();
                }

                loginText.Visibility = Visibility.Visible;
                loginTextBox.Visibility = Visibility.Visible;
                loginEnter.Visibility = Visibility.Visible;
                passwordLog.Visibility = Visibility.Visible;

                checkText.Visibility = Visibility.Collapsed;
                checkTextBox.Visibility = Visibility.Collapsed;
                checkEnter.Visibility = Visibility.Collapsed;
            }
            else
            {
                checkTextBox.Text = null;
                

            }
            
        }
        private void DoPayPalPayment(double price)
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            var payout = new Payout
            {
                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = "batch_" + Guid.NewGuid().ToString().Substring(0, 8),
                    email_subject = "You have payment",
                    recipient_type = PayoutRecipientType.EMAIL
                },

                items = new List<PayoutItem>
            {
                new PayoutItem
                {
                    recipient_type = PayoutRecipientType.EMAIL,
                    amount = new Currency { value = price.ToString(), currency="USD" },
                    receiver = "raywom92@gmail.com",
                    note="Thank you",
                    sender_item_id = "item_1"
                }
            }
            };

            var created = payout.Create(apiContext, false);
        }
        private void ButtonClickForPay(object sender, RoutedEventArgs e)
        {
            DoPayPalPayment(amountPrice);
        }
        public static string SignUp(string phone)
        {
            bool isVerifiedNumber = false;
            string message = "";
            int code = 0;
            int codeTwo = 0;
            Random random = new Random();
            while (!isVerifiedNumber)
            {
                code = random.Next(1000, 9999);
                Code(phone, code.ToString());

                int.TryParse(Console.ReadLine(), out codeTwo);
                
                if (code == codeTwo)
                {
                    message = "Вы зарегистрировались";
                    isVerifiedNumber = true;
                    return message;
                }
                else
                {
                    Console.WriteLine("Код не верен");
                }

                message = "Это имя занято";

                message = "Этот телефон используется";
                isVerifiedNumber = true;

            }
            return message;
        }

        private static string GenerateRandomCode()
        {
            Random random = new Random();
            code = random.Next(1000, 9999);
            return code.ToString();
        }

        public static void Code(string phone, string text)
        {
            string resultPage = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.mobizon.kz/service/Message/SendSmsMessage?recipient=" + phone + "&text=" + text + "&apiKey=kz21f62b9387cebabe7cbbafdcf32af0ad83e4d32062d299e9fe19d795fac845f78694");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true))
            {
                resultPage = sr.ReadToEnd();
                sr.Close();
            }

        }

        private void UpdatPage()
        {
            cartMenu.Visibility = Visibility.Visible;
            mainPanel.Visibility = Visibility.Collapsed;

            cartListBox.Items.Clear();

            foreach (var i in itemsInCartForUi)
            {
                Grid gridForItemsInListCart = new Grid
                {
                    Background = Brushes.White,
                    Height = 70,
                    Width = 1060,
                    Effect = new DropShadowEffect
                    {
                        BlurRadius = 20,
                        ShadowDepth = 1
                    }
                };
                gridForItemsInListCart.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(107)
                });
                gridForItemsInListCart.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(302)
                });
                gridForItemsInListCart.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(202)
                });
                gridForItemsInListCart.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(177)
                });
                gridForItemsInListCart.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(209)
                });

                TextBlock textBlockToCount = new TextBlock
                {                    
                    Foreground = Brushes.Black,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 40
                };
                int pido = 0;
                dictionaryCount.TryGetValue(i.ItemId,out pido);
                textBlockToCount.Text = pido.ToString();

                Thickness marginTextBlockToCount = textBlockToCount.Margin;
                marginTextBlockToCount.Bottom = 15;
                marginTextBlockToCount.Left = 0;
                marginTextBlockToCount.Right = 15;
                marginTextBlockToCount.Top = 0;
                textBlockToCount.Margin = marginTextBlockToCount;

                gridForItemsInListCart.Children.Add(textBlockToCount);

                Grid.SetColumn(textBlockToCount, 0);

                TextBlock textBlockToName = new TextBlock
                {
                    Text = i.Name,
                    Foreground = Brushes.Black,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 40
                };
                Thickness marginTextBlockToName = textBlockToName.Margin;
                marginTextBlockToName.Bottom = 15;
                marginTextBlockToName.Left = 0;
                marginTextBlockToName.Right = 15;
                marginTextBlockToName.Top = 0;
                textBlockToName.Margin = marginTextBlockToName;

                gridForItemsInListCart.Children.Add(textBlockToName);

                Grid.SetColumn(textBlockToName, 1);

                TextBlock textBlockToPrice = new TextBlock
                {
                    Text = i.Price.ToString(),
                    Foreground = Brushes.Black,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 40
                };
                Thickness marginTextBlockToPrice = textBlockToPrice.Margin;
                marginTextBlockToPrice.Bottom = 15;
                marginTextBlockToPrice.Left = 0;
                marginTextBlockToPrice.Right = 15;
                marginTextBlockToPrice.Top = 0;
                textBlockToPrice.Margin = marginTextBlockToPrice;

                gridForItemsInListCart.Children.Add(textBlockToPrice);

                Grid.SetColumn(textBlockToPrice, 2);

                double price = 0;

                foreach (var j in itemsInCart)
                {
                    price += j.Price;                    
                }

                amountPrice += price;

                TextBlock textBlockToAmount = new TextBlock
                {
                    Text = price.ToString(),
                    Foreground = Brushes.Black,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 40
                };
                Thickness marginTextBlockToAmount = textBlockToAmount.Margin;
                marginTextBlockToAmount.Bottom = 15;
                marginTextBlockToAmount.Left = 0;
                marginTextBlockToAmount.Right = 15;
                marginTextBlockToAmount.Top = 0;
                textBlockToAmount.Margin = marginTextBlockToAmount;

                gridForItemsInListCart.Children.Add(textBlockToAmount);

                Grid.SetColumn(textBlockToAmount, 3);

                Button buttonForDelete = new Button
                {
                    Background = Brushes.Red,
                    Content = "Удалить",
                    Name = "n" + i.ItemId.ToString()
                };
                Thickness marginButtonForDelete = buttonForDelete.Margin;
                marginButtonForDelete.Bottom = 20;
                marginButtonForDelete.Left = 20;
                marginButtonForDelete.Right = 20;
                marginButtonForDelete.Top = 20;
                buttonForDelete.Margin = marginButtonForDelete;
                buttonForDelete.Click += ButtonClickForDeleteItemToCart;


                gridForItemsInListCart.Children.Add(buttonForDelete);

                Grid.SetColumn(buttonForDelete, 4);

                cartListBox.Items.Add(gridForItemsInListCart);
            }
        }

        private void ButtonForOpenCartMenuClick(object sender, RoutedEventArgs e)
        {
            UpdatPage();
        }

        private void ButtonClickForAddInCart(object sender, RoutedEventArgs e)
        {
            int countS = 0;

            using (ShopContext shopContext = new ShopContext())
            {
                foreach (var item in shopContext.Items)
                {
                    if ("n" + item.ItemId.ToString() == (sender as Button).Name)
                    {
                        itemsInCart.Add(item);

                        foreach (var i in itemsInCart) {
                            if (i.Article == item.Article)
                            {
                                countS++;                                
                            }
                        }
                        dictionaryCount.Remove(item.ItemId);
                        dictionaryCount.Add(item.ItemId, countS);

                        if (countS < 2)
                        {
                            var justId = shopContext.Carts.Add(new Cart {
                                ItemC = itemsInCart
                            });
                            itemsInCartForUi.Add(item);
                        }

                        int count;
                        int.TryParse(countItemsInCart.Text, out count);
                        count++;
                        countItemsInCart.Text = count.ToString();
                    }
                }

                shopContext.SaveChanges();
            }
        }

        private void ButtonClickForReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            mainPanel.Visibility = Visibility.Visible;
            cartMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonClickForDeleteItemToCart(object sender, RoutedEventArgs e)
        {            
            using (ShopContext shopContext = new ShopContext())
            {
                foreach (var i in shopContext.Carts)
                {
                    id = i.Id;
                }

                var buf = (sender as Button).Name;

                Cart cr_edit = shopContext.Carts.First(p => p.Id == id);
                Item item_dd = shopContext.Items.First(p => "n" + p.ItemId.ToString() == buf);

                cr_edit.ItemC.Remove(item_dd);

                foreach (var i in itemsInCart)
                {
                    if ("n" + i.ItemId.ToString() == buf)
                    {
                        itemsInCart.Remove(i);
                        break;
                    }
                }

                int key;
                int countCh;

                string minN = buf.Remove(0,1);                

                int.TryParse(minN, out key);

                dictionaryCount.TryGetValue(key, out countCh);
                dictionaryCount.Remove(key);

                int col = 0;

                foreach (var i in itemsInCart)
                {
                    col++;
                }
                dictionaryCount.Add(key, col);

                int count;
                int.TryParse(countItemsInCart.Text, out count);
                count--;
                countItemsInCart.Text = count.ToString();

                shopContext.SaveChanges();
            }

            UpdatPage();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
