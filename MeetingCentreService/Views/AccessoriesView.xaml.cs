using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetingCentreService.Views
{
    /// <summary>
    /// Interaction logic for AccessoriesView.xaml
    /// </summary>
    public partial class AccessoriesView : Page
    {
        private ViewModels.AccessoriesViewModel ViewModel = ViewModels.AccessoriesViewModel.Context;
        public AccessoriesView()
        {
            this.DataContext = this.ViewModel;
            InitializeComponent();
        }

        private void NewAccessory(object sender, RoutedEventArgs e)
        {
            Views.Forms.AccessoryForm form = new Forms.AccessoryForm();
            form.ShowDialog();
            if (form.ClosedWith == Views.Forms.CloseAction.Save)
                this.ViewModel.Save(form.Accessory);
        }

        private void EditAccessory(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedAccessory != null)
            {
                Views.Forms.AccessoryForm form = new Forms.AccessoryForm(this.ViewModel.SelectedAccessory);
                form.ShowDialog();
                switch(form.ClosedWith)
                {
                    case Forms.CloseAction.Save:
                        this.ViewModel.Save(form.Accessory);
                        break;
                    case Forms.CloseAction.Delete:
                        this.DeleteAccessory(form, e);
                        break;
                }
            }
        }

        private void DeleteAccessory(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedAccessory != null)
            {
                if (sender is Forms.AccessoryForm) (sender as Forms.AccessoryForm).Accessory.Save().Remove();
                else if (MessageBox.Show($"Are you sure you want to remove accessory {this.ViewModel.SelectedAccessory.Name} from listing?", "Remove Accessory from list", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
                    this.ViewModel.RemoveAccessory(this.ViewModel.SelectedAccessory);
            }
        }

        private void RestockAccessory(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedAccessory != null)
            {
                Views.Forms.AccessoryRestockForm form = new Forms.AccessoryRestockForm(this.ViewModel.SelectedAccessory);
                form.ShowDialog();
                if (form.ClosedWith == Forms.CloseAction.Save)
                    form.Accessory.Save();
            }
        }

        private void HandOutAccessory(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedAccessory != null)
            {
                Views.Forms.AccessoryHandOutForm form = new Forms.AccessoryHandOutForm(this.ViewModel.SelectedAccessory);
                form.ShowDialog();
                if (form.ClosedWith == Forms.CloseAction.Save)
                    form.Accessory.Save();
            }
        }
    }
}
