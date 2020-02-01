using System;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Linq;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Class representing a Meeting Reservation
    /// </summary>
    public class MeetingReservation : INotifyPropertyChanged
    {
        /// <summary>
        /// Date of the Reservation
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public DateTime Date { get; }
        [JsonIgnore]
        [XmlIgnore]
        private TimeSpan _timeFrom;
        /// <summary>
        /// Time the Reservation begins
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public TimeSpan TimeFrom { get { return this._timeFrom; } set { this._timeFrom = value; OnPropertyChanged("TimeFrom"); } }
        [JsonIgnore]
        [XmlIgnore]
        private TimeSpan _timeTo;
        /// <summary>
        /// Time the Reservation ends
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public TimeSpan TimeTo { get { return this._timeTo; } set { this._timeTo = value; this.OnPropertyChanged("TimeTo"); } }
        [JsonIgnore]
        [XmlIgnore]
        private int _expectedPersonsCount;
        /// <summary>
        /// Exxpected count of people attending this Reservation
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public int ExpectedPersonsCount { get { return this._expectedPersonsCount; } set { this._expectedPersonsCount = value; this.OnPropertyChanged("ExpectedPersonsCount"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _customer;
        /// <summary>
        /// Name of the customer creating this Reservation
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Customer { get { return this._customer; } set { this._customer = value; this.OnPropertyChanged("Customer"); } }
        [JsonIgnore]
        [XmlIgnore]
        private bool _videoConference;
        /// <summary>
        /// Whether video conference equipment is required for this Reservation
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public bool VideoConference { get { return this._videoConference; } set { this._videoConference = value; this.OnPropertyChanged("VideoConference"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _note;
        /// <summary>
        /// Note to or description of this Reservation
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Note { get { return this._note; } set { this._note = value; this.OnPropertyChanged("Note"); } }
        /// <summary>
        /// Meeting Room the Reservation is for
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public MeetingRoom MeetingRoom { get; private set; }

        /// <summary>
        /// Creates a Reservation for given MeetinRoom
        /// </summary>
        /// <param name="room">MeetingRoom the Reservation is for</param>
        /// <param name="date">Date the Reservation is taking place</param>
        public MeetingReservation(MeetingRoom room, DateTime date)
        {
            this.MeetingRoom = room;
            this.Date = date;
        }

        /// <summary>
        /// Sets the value of MeetingRoom.
        /// Used for importing from JSON and XML.
        /// </summary>
        /// <param name="room">Assigning MeetingRoom</param>
        internal void AssignMeetingRoom(MeetingRoom room)
        {
            if (this.MeetingRoom is null) this.MeetingRoom = room;
            else throw new System.Exception("Meeting Room has already been set");
        }

        /// <summary>
        /// Creates a Form for this MeetingReservation
        /// </summary>
        /// <returns cref="MeetingReservationForm">Form for this Reservation</returns>
        public MeetingReservationForm GetEditForm()
        {
            return new MeetingReservationForm(this);
        }

        /// <summary>
        /// Creates a Form for a new MeetingReservation
        /// </summary>
        /// <param name="room">MeetingRoom the Reservation is for</param>
        /// <param name="date">Date the Reservation is taking place</param>
        /// <returns></returns>
        public static MeetingReservationForm GetNewForm(MeetingRoom room, DateTime date)
        {
            return new MeetingReservationForm(room, date);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Form for editing and creating MeetingReservations
        /// </summary>
        public class MeetingReservationForm : INotifyPropertyChanged
        {
            /// <summary>
            /// Instance the MeetingReservationForm has been created from
            /// </summary>
            private MeetingReservation Instance;
            /// <summary>
            /// MeetingRoom instance for new MeetingReservation
            /// </summary>
            public MeetingRoom Room { get; private set; }
            /// <summary>
            /// Date of the Reservation
            /// </summary>
            public DateTime Date { get; }
            private DateTime _timeFrom;
            /// <summary>
            /// Time the Reservation begins
            /// </summary>
            public DateTime TimeFrom { get { return this._timeFrom; } set { this._timeFrom = value; this.OnPropertyChanged("TimeFrom"); this.OnPropertyChanged("IsTimeAvailable"); } }
            private DateTime _timeTo;
            /// <summary>
            /// Time the Reservation ends
            /// </summary>
            public DateTime TimeTo { get { return this._timeTo; } set { this._timeTo = value; this.OnPropertyChanged("TimeTo"); this.OnPropertyChanged("IsTimeAvailable"); } }
            private int _expectedPersonCount;
            /// <summary>
            /// Expected count of people attending
            /// </summary>
            public int ExpectedPersonCount { get { return this._expectedPersonCount; } set { this._expectedPersonCount = value > this.Room.Capacity ? this.Room.Capacity : value; this.OnPropertyChanged("ExpectedPersonCount"); } }
            private string _customer;
            /// <summary>
            /// Name of the customer creating Reservation
            /// </summary>
            public string Customer { get { return this._customer; } set { this._customer = value; this.OnPropertyChanged("Customer"); } }
            private bool _videoConference;
            /// <summary>
            /// Whether video conference equipment is required for this Reservation
            /// </summary>
            public bool VideoConference { get { return this._videoConference; } set { this._videoConference = value && this.Room.VideoConference; this.OnPropertyChanged("VideoConference"); } }
            private string _note;
            /// <summary>
            /// Note to or description of the Reservation
            /// </summary>
            public string Note { get { return this._note; } set { this._note = value; this.OnPropertyChanged("Note"); } }
            /// <summary>
            /// Calculates whether the reservation is in a time conflict
            /// </summary>
            public bool IsTimeConflicting
            {
                get
                {
                    return this.TimeFrom >= this.TimeTo
                        || this.Room.Reservations.ContainsKey(this.Date.ToShortDateString())
                          && this.Room.Reservations[this.Date.ToShortDateString()].Any(r => r != this.Instance && !(this.TimeTo.TimeOfDay <= r.TimeFrom || this.TimeFrom.TimeOfDay >= r.TimeTo));
                }
            }

            /// <summary>
            /// Creates a Form for an existing Reservation
            /// </summary>
            /// <param name="reservation">Reservation the Form is created for</param>
            public MeetingReservationForm(MeetingReservation reservation)
            {
                this.Customer = reservation.Customer;
                this.Room = reservation.MeetingRoom;
                this.Date = reservation.Date;
                this.TimeFrom = this.TimeFrom.Add(reservation.TimeFrom);
                this.TimeTo = this.TimeTo.Add(reservation.TimeTo);
                this.ExpectedPersonCount = reservation.ExpectedPersonsCount;
                this.VideoConference = reservation.VideoConference;
                this.Note = reservation.Note;
                this.Instance = reservation;
            }

            /// <summary>
            /// Creates a Form for a new Reservation
            /// </summary>
            /// <param name="room">Room the Reservation is for</param>
            /// <param name="date">Date the Reservation is taking place</param>
            public MeetingReservationForm(MeetingRoom room, DateTime date)
            {
                this.Room = room;
                this.Date = date;
            }

            /// <summary>
            /// Saves the form data to it's source MeetingReservation or to a new MeetingReservation
            /// </summary>
            /// <returns cref="MeetingReservation">Saved MeetingReservation</returns>
            public MeetingReservation Save()
            {
                if (this.Instance is null) this.Instance = new MeetingReservation(this.Room, this.Date);
                this.Instance.TimeFrom = this.TimeFrom.TimeOfDay;
                this.Instance.TimeTo = this.TimeTo.TimeOfDay;
                this.Instance.ExpectedPersonsCount = this.ExpectedPersonCount;
                this.Instance.Customer = this.Customer;
                this.Instance.Note = this.Note;
                return this.Instance;
            }

            /// <summary>
            /// Returns whether this is a form for an existing MeetingReservation
            /// </summary>
            /// <returns>Whether this is an existing MeetingReservation</returns>
            internal bool HasInstance()
            {
                return this.Instance != null;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler evt = this.PropertyChanged;
                if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
