using System;
using System.Windows;
using System.Windows.Controls;

namespace MeetingCentreService.Views.Forms
{
    /// <summary>
    /// Interakční logika pro ReservationForm.xaml
    /// </summary>
    public partial class ReservationForm : Window
    {
        /// <summary>
        /// Reservation the form is for
        /// </summary>
        public Models.Entities.MeetingReservation.MeetingReservationForm Reservation { get; private set; }
        /// <summary>
        /// Action selected in the form
        /// </summary>
        public CloseAction ClosedWith { get; private set; } = CloseAction.None;
        /// <summary>
        /// Whether user can delete this Reservation
        /// </summary>
        public bool CanDelete { get { return this.Reservation.HasInstance(); } }
        /// <summary>
        /// Formatted date of this reservation
        /// </summary>
        public string ReservationDateText { get { return this.Reservation.Date.ToLongDateString(); } }
        /// <summary>
        /// Window title for the Reservation
        /// </summary>
        public string ReactiveWindowTitle { get; }
        /// <summary>
        /// Create a form for a new MeetingReservation
        /// </summary>
        /// <param name="room">Reserved room</param>
        /// <param name="date">Date of the Reservation</param>
        public ReservationForm(Models.Entities.MeetingRoom room, DateTime date)
        {
            this.Reservation = Models.Entities.MeetingReservation.GetNewForm(room, date);
            this.DataContext = this;
            this.ReactiveWindowTitle = "New Reservation";
            InitializeComponent();
        }
        /// <summary>
        /// Create an edit form for an exsting MeetingReservation
        /// </summary>
        /// <param name="reservation">Edited MeetingReservation</param>
        public ReservationForm(Models.Entities.MeetingReservation reservation)
        {
            this.Reservation = reservation.GetEditForm();
            this.DataContext = this;
            this.ReactiveWindowTitle = "Edit Reservation";
            InitializeComponent();
        }

        /// <summary>
        /// Click event handler for the Save button. Validates the form, requests a save and closes the form.
        /// </summary>
        private void SaveForm(object sender, RoutedEventArgs e)
        {
            if (!(this.FormCustomer.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.Reservation.IsTimeConflicting
               || this.FormCount.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.FormNote.GetBindingExpression(TextBox.TextProperty).HasValidationError))
            {
                this.ClosedWith = CloseAction.Save;
                this.CloseForm(sender, e);
            }
        }

        /// <summary>
        /// Click event handler for the Cancel button. Just closes the form.
        /// </summary>
        private void CloseForm(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Click event handler for the Delete button. Requests a delete and closes the form.
        /// </summary>
        private void DeleteReservation(object sender, RoutedEventArgs e)
        {
            this.ClosedWith = CloseAction.Delete;
            this.CloseForm(sender, e);
        }

        /// <summary>
        /// ValueChanged event handler for the time selectors. Explicitly updates the validation of currently selected time.
        /// </summary>
        private void RefreshTimeAvailableExplicit(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.TimeUnavailbaleAlert != null)
                this.TimeUnavailbaleAlert.GetBindingExpression(TextBlock.VisibilityProperty).UpdateTarget();
        }
    }
}
