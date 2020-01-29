using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MeetingCentreService.Models.Entities;

namespace MeetingCentreService.ViewModels
{
    public class MeetingsViewModel : INotifyPropertyChanged
    {
        private static MeetingsViewModel _context;

        public static MeetingsViewModel Context { get { if (_context is null) { _context = new MeetingsViewModel(); } return _context; } }

        public Models.Entities.MeetingCentreService CurrentService { get { return Models.Entities.MeetingCentreService.Current; } }
        private MeetingRoom _selectedRoom;
        public MeetingRoom SelectedRoom { get { return this._selectedRoom; } set { this._selectedRoom = value; this.OnPropertyChanged("SelectedRoom", "ReservationsForCurrentSelection", "CanCreateReservation"); } }
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate { get { return this._selectedDate; } set { this._selectedDate = value; this.OnPropertyChanged("SelectedDate", "ReservationsForCurrentSelection", "CanCreateReservation"); } }
        public IEnumerable<MeetingReservation> ReservationsForCurrentSelection
        {
            get
            {
                string keyDate = this.SelectedDate.ToShortDateString();
                if (this.SelectedRoom != null && this.SelectedRoom.Reservations.ContainsKey(keyDate)) return this.SelectedRoom.Reservations[keyDate];
                else return null;
            }
        }
        private MeetingReservation _selectedReservation;
        public MeetingReservation SelectedReservation { get { return this._selectedReservation; }set { this._selectedReservation = value; this.OnPropertyChanged("SelectedReservation", "CanModifyReservation"); } }
        public bool CanCreateReservation { get { return this.SelectedRoom != null && this.SelectedDate > DateTime.Now; } }
        public bool CanModifyReservation { get { return this.SelectedReservation != null && this.SelectedDate > DateTime.Now; } }

        private MeetingsViewModel() { }

        internal void ResetService()
        {
            this.OnPropertyChanged("CurrentService");
        }
        internal void RefreshReservations()
        {
            this.OnPropertyChanged("ReservationsForCurrentSelection");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler evt;
            foreach (string propertyName in propertyNames)
            {
                evt = this.PropertyChanged;
                if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
