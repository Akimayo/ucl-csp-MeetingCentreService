using MeetingCentreService.Models.Entities;
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
using System.Windows.Shapes;

namespace MeetingCentreService.Views.Forms
{
    /// <summary>
    /// Interakční logika pro AccessoryForm.xaml
    /// </summary>
    public partial class AccessoryForm : Window
    {
        public CloseAction ClosedWith { get; private set; } = CloseAction.None;
        public Models.Entities.Accessory.AccessoryForm Accessory { get; internal set; }
        public bool HasInstance { get { return this.Accessory.HasInstance(); } }
        public string ReactiveWindowTitle { get; }
        public AccessoryForm()
        {
            this.Accessory = Models.Entities.Accessory.GetNewForm();
            this.ReactiveWindowTitle = "New Accessory";
            this.DataContext = this;
            InitializeComponent();
            this.FormCategory.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
        }

        public AccessoryForm(Accessory accessory)
        {
            this.Accessory = accessory.GetEditForm();
            this.ReactiveWindowTitle = "Edit Accessory";
            this.DataContext = this;
            InitializeComponent();
            this.FormCategory.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!(this.Accessory.Category is null ||
                  this.FormName.GetBindingExpression(TextBox.TextProperty).HasValidationError ||
                  this.FormMinimuRecommendedStock.GetBindingExpression(TextBox.TextProperty).HasValidationError))
            {
                this.ClosedWith = CloseAction.Save;
                this.Close();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to remove accessory {this.Accessory.Name} from listing?", "Remove Accessory from list", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.ClosedWith = CloseAction.Delete;
                this.Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
