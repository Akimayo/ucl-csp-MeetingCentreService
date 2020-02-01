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
        /// <summary>
        /// Action selected in the form
        /// </summary>
        public CloseAction ClosedWith { get; private set; } = CloseAction.None;
        /// <summary>
        /// Accessory of this form
        /// </summary>
        public Models.Entities.Accessory.AccessoryForm Accessory { get; internal set; }
        /// <summary>
        /// Whether this is an edit form
        /// </summary>
        public bool HasInstance { get { return this.Accessory.HasInstance(); } }
        /// <summary>
        /// Title of the window set based on the Accessory
        /// </summary>
        public string ReactiveWindowTitle { get; }
        /// <summary>
        /// Create a form for a new Accessory
        /// </summary>
        public AccessoryForm()
        {
            this.Accessory = Models.Entities.Accessory.GetNewForm();
            this.ReactiveWindowTitle = "New Accessory";
            this.DataContext = this;
            InitializeComponent();
            this.FormCategory.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
        }

        /// <summary>
        /// Create an edit for for an existing Accessory
        /// </summary>
        /// <param name="accessory">Accessory the form is for</param>
        public AccessoryForm(Accessory accessory)
        {
            this.Accessory = accessory.GetEditForm();
            this.ReactiveWindowTitle = "Edit Accessory";
            this.DataContext = this;
            InitializeComponent();
            this.FormCategory.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
        }

        /// <summary>
        /// Click event handler for the Save button. Validates the form, requests a save and closes the form.
        /// </summary>
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

        /// <summary>
        /// Click event handler for the Delete button. Request confirmation from the user, then requests a delete and closes the form.
        /// </summary>
        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to remove accessory {this.Accessory.Name} from listing?", "Remove Accessory from list", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.ClosedWith = CloseAction.Delete;
                this.Close();
            }
        }

        /// <summary>
        /// Click event handler for the Cancel butotn. Just closes the form.
        /// </summary>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
