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
        public Models.Entities.MeetingReservation.MeetingReservationForm Reservation { get; private set; }
        public CloseAction ClosedWith { get; private set; } = CloseAction.None;
        public bool CanDelete { get { return this.Reservation.HasInstance(); } }
        public string ReservationDateText { get { return this.Reservation.Date.ToLongDateString(); } }
        public ReservationForm(Models.Entities.MeetingRoom room, DateTime date)
        {
            this.Reservation = Models.Entities.MeetingReservation.GetNewForm(room, date);
            this.DataContext = this;
            InitializeComponent();
        }
        public ReservationForm(Models.Entities.MeetingReservation reservation)
        {
            this.Reservation = reservation.GetEditForm();
            this.DataContext = this;
            InitializeComponent();
        }

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

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteReservation(object sender, RoutedEventArgs e)
        {
            this.ClosedWith = CloseAction.Delete;
            this.CloseForm(sender, e);
        }

        private void RefreshTimeAvailableExplicit(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshTimeAvailableExplicit(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(this.TimeUnavailbaleAlert != null)
                this.TimeUnavailbaleAlert.GetBindingExpression(TextBlock.VisibilityProperty).UpdateTarget();
        }
    }
}
