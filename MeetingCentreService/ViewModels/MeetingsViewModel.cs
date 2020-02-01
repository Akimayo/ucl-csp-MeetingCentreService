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
        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static MeetingsViewModel Context { get { if (_context is null) { _context = new MeetingsViewModel(); } return _context; } }

        /// <summary>
        /// Current session's MeetingCentreService
        /// </summary>
        public Models.Entities.MeetingCentreService CurrentService { get { return Models.Entities.MeetingCentreService.Current; } }
        private MeetingRoom _selectedRoom;
        /// <summary>
        /// MeetingRoom currently selected in view
        /// </summary>
        public MeetingRoom SelectedRoom { get { return this._selectedRoom; } set { this._selectedRoom = value; this.OnPropertyChanged("SelectedRoom", "ReservationsForCurrentSelection", "CanCreateReservation"); } }
        private DateTime _selectedDate = DateTime.Now;
        /// <summary>
        /// Date currently selected in view
        /// </summary>
        public DateTime SelectedDate { get { return this._selectedDate; } set { this._selectedDate = value; this.OnPropertyChanged("SelectedDate", "ReservationsForCurrentSelection", "CanCreateReservation"); } }
        /// <summary>
        /// Collection of reservation for the currently selected date
        /// </summary>
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
        /// <summary>
        /// MeetingReservation currently selected in view
        /// </summary>
        public MeetingReservation SelectedReservation { get { return this._selectedReservation; }set { this._selectedReservation = value; this.OnPropertyChanged("SelectedReservation", "CanModifyReservation"); } }
        /// <summary>
        /// Whether the user is allowed to create a reservation based on current selections
        /// </summary>
        public bool CanCreateReservation { get { return this.SelectedRoom != null && this.SelectedDate > DateTime.Now; } }
        /// <summary>
        /// Whether the user is allowed to modify a reservation based on current selections
        /// </summary>
        public bool CanModifyReservation { get { return this.SelectedReservation != null && this.SelectedDate > DateTime.Now; } }

        /// <summary>
        /// Creates the ViewModel
        /// </summary>
        private MeetingsViewModel() { }

        /// <summary>
        /// Call when a new MeetingCentreService has been loaded
        /// </summary>
        internal void ResetService()
        {
            this.OnPropertyChanged("CurrentService");
        }
        /// <summary>
        /// Refreshes the collection of MeetingReservations
        /// </summary>
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
