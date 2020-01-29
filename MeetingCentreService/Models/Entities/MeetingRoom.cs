using System.ComponentModel;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Collections.Generic;
using NSwag.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Class representing a Meeting Room in a Meeting Centre
    /// </summary>
    public class MeetingRoom : INotifyPropertyChanged
    {
        [JsonIgnore]
        [XmlIgnore]
        private string _name;
        /// <summary>
        /// Name of the Room
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _code;
        /// <summary>
        /// Identification code of the Room
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Code { get { return this._code; } set { this._code = value; this.OnPropertyChanged("Code"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _description;
        /// <summary>
        /// Room description
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Description { get { return this._description; } set { this._description = value; this.OnPropertyChanged("Description"); } }
        [JsonIgnore]
        [XmlIgnore]
        private int _capacity;
        /// <summary>
        /// Room's capacity for people
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public int Capacity { get { return this._capacity; } set { this._capacity = value; this.OnPropertyChanged("Capacity"); } }
        [JsonIgnore]
        [XmlIgnore]
        private bool _videoConference;
        /// <summary>
        /// Whether the Room contains video-conference equipment
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public bool VideoConference { get { return this._videoConference; } set { this._videoConference = value; this.OnPropertyChanged("VideoConference"); } }
        /// <summary cref="MeetingReservation">
        /// Collection of Meeting Reservations for this Room
        /// </summary>
        [JsonProperty]
        [XmlIgnore]
        public ObservableDictionary<string, ObservableCollection<MeetingReservation>> Reservations { get; private set; }
        /// <summary>
        /// Helper collection for XML serialization and deserialization for Reservations
        /// </summary>
        [JsonIgnore]
        [XmlArray("Reservations")]
        internal IList<ReservationXMLSerializationContainer> ReservationsXMLHelper
        {
            /* 
             * XML Serializer cannot serialize the Observable Dictionary, so I use this helper mapper to work around it.
             * For some reason, though, XML Serializer ignores it, both as an IEnumerable and an IList.
             * I also tried changing the struct to a class, but to no avail.
             * In Debug Results View, it's exactly what one would expect, but XML's just nope-ing out of it.
             * So, screw it, it's in JSON.
             */
            get
            {
                return new List<ReservationXMLSerializationContainer>(this.Reservations.Select(p => new ReservationXMLSerializationContainer() { Date = p.Key, Reservations = p.Value }));
            }
            set
            {
                this.Reservations = new ObservableDictionary<string, ObservableCollection<MeetingReservation>>();
                foreach (ReservationXMLSerializationContainer c in value) this.Reservations.Add(c.Date, new ObservableCollection<MeetingReservation>(c.Reservations));
            }
        }
        /// <summary>
        /// Meeting Room's Meeting Centre
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public MeetingCentre MeetingCentre { get; private set; }

        /// <summary>
        /// Creates a Room in a Meeting Centre
        /// </summary>
        /// <param name="meetingCentre">Parent Meeting Centre</param>
        public MeetingRoom(MeetingCentre meetingCentre)
        {
            this.MeetingCentre = meetingCentre;
            this.Reservations = new ObservableDictionary<string, ObservableCollection<MeetingReservation>>();
            this.Reservations.CollectionChanged += ReservationsDictionaryChanged;
        }

        /// <summary>
        /// Creates a Room. Used for deserialization.
        /// </summary>
        private MeetingRoom()
        {
            this.Reservations = new ObservableDictionary<string, ObservableCollection<MeetingReservation>>();
            this.Reservations.CollectionChanged += ReservationsDictionaryChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event handler for changes made to the dictionary of Reservations
        /// </summary>
        private void ReservationsDictionaryChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("Reservations");
            if (e.NewItems != null)
                foreach (KeyValuePair<string, ObservableCollection<MeetingReservation>> item in e.NewItems)
                    item.Value.CollectionChanged += ReservationsCollectionChanged;
            if (e.OldItems != null)
                foreach (KeyValuePair<string, ObservableCollection<MeetingReservation>> item in e.OldItems)
                    item.Value.CollectionChanged -= ReservationsCollectionChanged;
        }

        /// <summary>
        /// Event handler for changes to the individual collections of Reservations in dictionary
        /// </summary>
        private void ReservationsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("Reservations");
            if (e.NewItems != null)
                foreach (MeetingReservation reservation in e.NewItems)
                    reservation.PropertyChanged += ReservationChanged;
            if (e.OldItems != null)
                foreach (MeetingReservation reservation in e.OldItems)
                    reservation.PropertyChanged -= ReservationChanged;
        }

        /// <summary>
        /// Event handler for changes to the individual Reservations
        /// </summary>
        /// <remarks>
        /// Automaticly reassigns the Reservation under the correct key in the dictionary.
        /// </remarks>
        private void ReservationChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("Reservations");
            if (e.PropertyName == "Date")
            {
                MeetingReservation reservation = sender as MeetingReservation;
                this.Reservations.First(k => k.Value.Contains(reservation)).Value.Remove(reservation);
                this.AddReservation(reservation);
            }
        }

        /// <summary>
        /// Adds a Reservation for this room under the correct key.
        /// </summary>
        /// <param name="reservation">Added Reservation</param>
        public void AddReservation(MeetingReservation reservation)
        {
            if (reservation.MeetingRoom == this)
            {
                string keyDate = reservation.Date.ToShortDateString();
                if (!this.Reservations.ContainsKey(keyDate))
                    this.Reservations.Add(keyDate, new ObservableCollection<MeetingReservation>());
                this.Reservations[keyDate].Add(reservation);
            }
            else throw new System.Exception("The Reservation is not for this Room");
        }

        /// <summary>
        /// Sets the value of MeetingCentre.
        /// Used for importing from JSON and XML.
        /// </summary>
        /// <param name="centre">Assigning MeetingCentre</param>
        public void AssignMeetingCentre(MeetingCentre centre)
        {
            if (this.MeetingCentre is null) this.MeetingCentre = centre;
            else throw new System.Exception("Meeting Centre has already been set");
        }

        /// <summary>
        /// Create a form for this Meeting Room
        /// </summary>
        /// <returns cref="MeetingRoomForm">MeetingRoomForm with properties of this MeetingRoom</returns>
        public MeetingRoomForm GetEditForm()
        {
            return new MeetingRoomForm(this);
        }
        /// <summary>
        /// Create a form for a new Meeting Room in a Meeting Centre
        /// </summary>
        /// <param name="centre">Meeting Room's parent Centre</param>
        /// <returns cref="MeetingCentreForm">Blank MeetingRoomForm</returns>
        public static MeetingRoomForm GetNewForm(MeetingCentre centre)
        {
            return new MeetingRoomForm(centre);
        }
        /// <summary>
        /// Helper class for creating & editing MeetingRooms
        /// </summary>
        /// <remarks>
        /// Allows the user to discard changes and at the same time allows the developer
        /// to use Binding/Property/Event-based model
        /// </remarks>
        public class MeetingRoomForm : INotifyPropertyChanged
        {
            /// <summary>
            /// Empty MeetingCentreForm for clearing forms
            /// </summary>
            public static MeetingRoomForm Empty { get { return new MeetingRoomForm(); } }

            /// <summary>
            /// Instance the form has been created from
            /// </summary>
            private MeetingRoom Instance;
            /// <summary>
            /// MeetingCentre instance for new MeetingRoom
            /// </summary>
            private MeetingCentre Centre;
            private string _name;
            /// <summary>
            /// Name of the Room
            /// </summary>
            public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
            private string _code;
            /// <summary>
            /// Identification code of the Room
            /// </summary>
            public string Code { get { return this._code; } set { this._code = value; this.OnPropertyChanged("Code"); } }
            private string _description;
            /// <summary>
            /// Room description
            /// </summary>
            public string Description { get { return this._description; } set { this._description = value; this.OnPropertyChanged("Description"); } }
            private int _capacity;
            /// <summary>
            /// Room's capacity for people
            /// </summary>
            public int Capacity { get { return this._capacity; } set { this._capacity = value; this.OnPropertyChanged("Capacity"); } }
            private bool _videoConference;
            /// <summary>
            /// Whether the Room contains video-conference equipment
            /// </summary>
            public bool VideoConference { get { return this._videoConference; } set { this._videoConference = value; this.OnPropertyChanged("VideoConference"); } }
            /// <summary>
            /// Create a Form from a Room
            /// </summary>
            /// <param name="room">Source MeetingRoom</param>
            internal MeetingRoomForm(MeetingRoom room)
            {
                this._name = room.Name;
                this._code = room.Code;
                this._description = room.Description;
                this._capacity = room.Capacity;
                this._videoConference = room.VideoConference;
                this.Instance = room;
                this.Centre = null;
                this.PropertyChanged = null;
            }

            /// <summary>
            /// Blank constructor (Artifact from MeetingRoomForm not being a struct)
            /// </summary>
            private MeetingRoomForm() { }

            /// <summary>
            /// Create a new Room form
            /// </summary>
            /// <param name="centre">Centre the new Room is in</param>
            internal MeetingRoomForm(MeetingCentre centre)
            {
                this._name = string.Empty;
                this._code = string.Empty;
                this._description = string.Empty;
                this._capacity = 0;
                this._videoConference = false;
                this.Instance = null;
                this.Centre = centre;
                this.PropertyChanged = null;
            }
            /// <summary>
            /// Saves the form data to it's source MeetingRoom or to a new MeetingRoom
            /// </summary>
            /// <returns>Saved MeetingRoom</returns>
            public MeetingRoom Save()
            {
                if (this.Instance is null) this.Instance = new MeetingRoom(this.Centre);
                this.Instance.Name = this.Name;
                this.Instance.Code = this.Code;
                this.Instance.Description = this.Description;
                this.Instance.Capacity = this.Capacity;
                this.Instance.VideoConference = this.VideoConference;
                return this.Instance;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler evt = this.PropertyChanged;
                if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal struct ReservationXMLSerializationContainer
        {
            [XmlAttribute]
            internal string Date { get; set; }
            [XmlArray]
            internal IEnumerable<MeetingReservation> Reservations { get; set; }
        }
    }
}
