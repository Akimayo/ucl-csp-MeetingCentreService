using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Entity representing a Meeting Centre
    /// </summary>
    public class MeetingCentre : INotifyPropertyChanged
    {
        [JsonIgnore]
        [XmlIgnore]
        private string _name;
        /// <summary>
        /// Name of thee Centre
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _code;
        /// <summary>
        /// Identification code of the Centre
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Code { get { return this._code; } set { this._code = value; this.OnPropertyChanged("Code"); } }
        [JsonIgnore]
        [XmlIgnore]
        private string _description;
        /// <summary>
        /// Centre description
        /// </summary>
        [JsonProperty]
        [XmlAttribute]
        public string Description { get { return this._description; } set { this._description = value; this.OnPropertyChanged("Description"); } }
        /// <summary>
        /// Collection of Meeting Rooms in the Centre
        /// </summary>
        [JsonProperty]
        [XmlArray]
        public ObservableCollection<MeetingRoom> MeetingRooms { get; }

        /// <summary>
        /// Create a blank MeetingCentre
        /// </summary>
        public MeetingCentre()
        {
            this.MeetingRooms = new ObservableCollection<MeetingRoom>();
            this.MeetingRooms.CollectionChanged += (s, e) => this.OnPropertyChanged("MeetingRooms");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Create a form for this Meeting Centre
        /// </summary>
        /// <returns cref="MeetingCentreForm">MeetingCentreForm with properties of this MeetingCentre</returns>
        public MeetingCentreForm GetEditForm()
        {
            return new MeetingCentreForm(this);
        }
        /// <summary>
        /// Create a form for a new Meeting Centre
        /// </summary>
        /// <returns cref="MeetingCentreForm">Blank MeetingCentreForm</returns>
        public static MeetingCentreForm GetNewForm()
        {
            return new MeetingCentreForm(true);
        }

        /// <summary>
        /// Helper class for creating & editing MeetingCentres
        /// </summary>
        /// <remarks>
        /// Allows the user to discard changes and at the same time allows the developer
        /// to use Binding/Property/Event-based model
        /// </remarks>
        public class MeetingCentreForm : INotifyPropertyChanged
        {
            /// <summary>
            /// Empty MeetingCentreForm for clearing forms
            /// </summary>
            public static MeetingCentreForm Empty { get { return new MeetingCentreForm(); } }
            /// <summary>
            /// Instance the form has been created from
            /// </summary>
            private MeetingCentre Instance;
            private string _name;
            /// <summary>
            /// Name of the Centre
            /// </summary>
            public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
            private string _code;
            /// <summary>
            /// Identification code of the Centre
            /// </summary>
            public string Code { get { return this._code; } set { this._code = value; this.OnPropertyChanged("Code"); } }
            private string _description;
            /// <summary>
            /// Centre description
            /// </summary>
            public string Description { get { return this._description; } set { this._description = value; this.OnPropertyChanged("Description"); } }
            /// <summary>
            /// Collection of changes made to Rooms in the Centre
            /// </summary>
            /// <remarks cref="CollectionAction">
            /// Uses the CollectionAction enum to marck changes made to the collection of Rooms
            /// </remarks>
            public ObservableCollection<(MeetingRoom room, CollectionAction action)> RoomsChanged { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Create a Form from a Centre
            /// </summary>
            /// <param name="centre">Source MeetingCentre</param>
            internal MeetingCentreForm(MeetingCentre centre)
            {
                this._name = centre.Name;
                this._code = centre.Code;
                this._description = centre.Description;
                this.RoomsChanged = new ObservableCollection<(MeetingRoom, CollectionAction)>(centre.MeetingRooms.Select(i => (i, CollectionAction.Unchanged)));
                this.Instance = centre;
                this.PropertyChanged = null;
            }
            /// <summary>
            /// Create a new Centre Form
            /// </summary>
            /// <param name="newThing">Unused, acts only to differentiate from ampty constructor (artifact from MeetingCentreForm being a struct)</param>
            public MeetingCentreForm(bool newThing = true)
            {
                this._name = string.Empty;
                this._code = string.Empty;
                this._description = string.Empty;
                this.RoomsChanged = new ObservableCollection<(MeetingRoom room, CollectionAction action)>();
                this.Instance = null;
                this.PropertyChanged = null;
            }
            /// <summary>
            /// Saves the form data to it's source MeetingCentre or to a new MeetingCentre
            /// </summary>
            /// <returns>Saved MeetingCentre</returns>
            public MeetingCentre Save()
            {
                if (this.Instance is null) this.Instance = new MeetingCentre();
                this.Instance.Name = this.Name;
                this.Instance.Code = this.Code;
                this.Instance.Description = this.Description;
                foreach (var delta in this.RoomsChanged)
                {
                    if (delta.action == CollectionAction.Added) this.Instance.MeetingRooms.Add(delta.room);
                    else if (delta.action == CollectionAction.Removed) this.Instance.MeetingRooms.Remove(delta.room);
                }
                return this.Instance;
            }

            /// <summary>
            /// Identifies whether the form is for a new MeetingCentre
            /// </summary>
            /// <returns>MeetingCentreForm has a source MeetingCentre</returns>
            public bool HasInstance() { return this.Instance != null; }
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler evt = this.PropertyChanged;
                if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
            }
            /// <summary>
            /// Helper enum for changes to the collection of Rooms
            /// </summary>
            public enum CollectionAction
            {
                /// <summary>
                /// This room has been removed
                /// </summary>
                Removed,
                /// <summary>
                /// This room has been added
                /// </summary>
                Added,
                /// <summary>
                /// This room is unchanged
                /// </summary>
                Unchanged
            }
        }
    }
}
