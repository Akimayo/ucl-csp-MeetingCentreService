using MeetingCentreService.ViewModels;
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
    /// Interaction logic for MeetingsView.xaml
    /// </summary>
    public partial class MeetingsView : Page
    {
        private static MeetingsViewModel ViewModel = MeetingsViewModel.Context;
        public MeetingsView()
        {
            this.DataContext = ViewModel;
            InitializeComponent();
        }

        private void NewReservation(object sender, RoutedEventArgs e)
        {
            Forms.ReservationForm form = new Forms.ReservationForm(ViewModel.SelectedRoom, ViewModel.SelectedDate);
            form.Closed += CommitFormAction;
            form.ShowDialog();
        }

        private void EditReservation(object sender, RoutedEventArgs e)
        {
            Forms.ReservationForm form = new Forms.ReservationForm(ViewModel.SelectedReservation);
            form.Closed += CommitFormAction;
            form.ShowDialog();
        }

        private void DeleteReservation(object sender, RoutedEventArgs e)
        {
            Models.Entities.MeetingReservation res;
            if (sender is Forms.ReservationForm) res = (sender as Forms.ReservationForm).Reservation.Save();
            else res = ViewModel.SelectedReservation;
            if(MessageBox.Show($"Are you sure you want to delete reservation {res.Customer}?", "Delete Reservation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                res.MeetingRoom.Reservations[res.Date.ToShortDateString()].Remove(res);
        }

        private void CommitFormAction(object sender, EventArgs e)
        {
            Forms.ReservationForm form = sender as Forms.ReservationForm;
            switch (form.ClosedWith)
            {
                case Forms.CloseAction.Save:
                    Models.Entities.MeetingReservation reservation = form.Reservation.Save();
                    if (!form.Reservation.Room.Reservations.ContainsKey(form.Reservation.Date.ToShortDateString()) || !form.Reservation.Room.Reservations[form.Reservation.Date.ToShortDateString()].Contains(reservation)) form.Reservation.Room.AddReservation(reservation);
                    ViewModel.RefreshReservations();
                    break;
                case Forms.CloseAction.Delete:
                    this.DeleteReservation(sender, null);
                    ViewModel.RefreshReservations();
                    break;
            }
        }
    }
}
