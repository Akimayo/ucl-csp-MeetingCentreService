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
    /// Interakční logika pro RestockForm.xaml
    /// </summary>
    public partial class AccessoryRestockForm : Window
    {
        /// <summary>
        /// Restocked Accessory
        /// </summary>
        public Models.Entities.Accessory.AccessoryForm Accessory { get; }
        /// <summary>
        /// Window title based on the Accessory
        /// </summary>
        public string ReactiveWindowTitle { get; }
        /// <summary>
        /// Action selected in the form
        /// </summary>
        public CloseAction ClosedWith { get; private set; } = CloseAction.None;
        /// <summary>
        /// Creates a erstock form for the given Accessory
        /// </summary>
        /// <param name="accessory">Restocked Accessory</param>
        public AccessoryRestockForm(Models.Entities.Accessory accessory)
        {
            this.Accessory = accessory.GetEditForm();
            this.DataContext = this;
            this.ReactiveWindowTitle = $"Restock {accessory.Name}";
            InitializeComponent();
        }

        /// <summary>
        /// Click event handler for the Save button. Validates the form, requests a save and closes the form.
        /// </summary>
        private void Save(object sender, RoutedEventArgs e)
        {
            if (!this.FormCount.GetBindingExpression(TextBox.TextProperty).HasValidationError)
            {
                this.ClosedWith = CloseAction.Save;
                this.Close();
            }
        }

        /// <summary>
        /// Click event handler for the Cancel button. Just closes the form.
        /// </summary>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
