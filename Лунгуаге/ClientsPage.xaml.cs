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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Лунгуаге
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            List<Gender> allGender = TestAuthDBEntities.GetContext().Gender.ToList();
            allGender.Insert(0, new Gender
            {
                Name = "Все типы"
            });
            FiltrComboBox.ItemsSource = allGender;
            FiltrComboBox.DisplayMemberPath = "Name";
            FiltrComboBox.SelectedValuePath = "Code";
            FiltrComboBox.SelectedIndex = 0;

        }
        private void UpdateClients()
        {
            List<Client> currentClients = TestAuthDBEntities.GetContext().Client.ToList();

            if (FiltrComboBox.SelectedIndex > 0)
            {
                string selectedGender = Convert.ToString(FiltrComboBox.SelectedValue);
                currentClients = currentClients.Where(x => x.GenderCode == selectedGender).ToList();
            }
            //сортировка
            switch (SortComboBox.SelectedIndex)
            {
                case 1:
                    {
                        if (UpRadioButton.IsChecked.Value)
                            currentClients = currentClients.OrderBy(x => x.LastName).ToList();
                        if (DownRadioButton.IsChecked.Value)
                            currentClients = currentClients.OrderByDescending(x => x.LastName).ToList();
                        break;
                    }
                case 2:
                    {
                        if (UpRadioButton.IsChecked.Value)
                            currentClients = currentClients.OrderBy(x => x.RegistrationDate).ToList();
                        if (DownRadioButton.IsChecked.Value)
                            currentClients = currentClients.OrderByDescending(x => x.RegistrationDate).ToList();
                        break;
                    }
            }
            //Поиск
            switch (SearchComboBox.SelectedIndex)
            {
                case 0:
                    {
                        currentClients = currentClients.Where(x => x.LastName.ToLower().StartsWith(SearchTextBox.Text.ToLower())).ToList();
                        break;
                    }
                case 1:
                    {
                        currentClients = currentClients.Where(x => x.FirstName.ToLower().StartsWith(SearchTextBox.Text.ToLower())).ToList();
                        break;
                    }
                case 2:
                    {
                        currentClients = currentClients.Where(x => x.Patronymic.ToLower().StartsWith(SearchTextBox.Text.ToLower())).ToList();
                        break;
                    }
                case 3:
                    {
                        currentClients = currentClients.Where(x => x.Email.ToLower().StartsWith(SearchTextBox.Text.ToLower())).ToList();
                        break;
                    }
                case 4:
                    {
                        currentClients = currentClients.Where(x => x.Phone.ToLower().StartsWith(SearchTextBox.Text.ToLower())).ToList();
                        break;
                    }
            }
            MainDataGrid.ItemsSource = currentClients.ToList();
            if (currentClients.Count == 0)
            {
                MessageBox.Show("Запрос не найден");
                SearchTextBox.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(MainDataGrid.SelectedItem as Client));

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данные?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    TestAuthDBEntities.GetContext().Client.Remove(MainDataGrid.SelectedItem as Client);
                    TestAuthDBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    MainDataGrid.ItemsSource = TestAuthDBEntities.GetContext().Client.ToList();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainDataGrid.ItemsSource = TestAuthDBEntities.GetContext().Client.ToList();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DownRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void FiltrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }
    }
}
