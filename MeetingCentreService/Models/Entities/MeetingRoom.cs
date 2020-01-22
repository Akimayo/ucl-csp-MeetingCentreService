using System.ComponentModel;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Class representing a Meeting Room in a Meeting Centre
    /// </summary>
    public class MeetingRoom : INotifyPropertyChanged
    {
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
        /// Meeting Room's Meeting Centre
        /// </summary>
        public MeetingCentre MeetingCentre { get; }

        /// <summary>
        /// Creates a Room in a Meeting Centre
        /// </summary>
        /// <param name="meetingCentre">Parent Meeting Centre</param>
        public MeetingRoom(MeetingCentre meetingCentre)
        {
            this.MeetingCentre = meetingCentre;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
