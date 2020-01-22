using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MeetingCentreService.ViewModels
{
    /// <summary>
    /// ViewModel for CentresView
    /// </summary>
    public class CentresViewModel : INotifyPropertyChanged
    {
        // Singleton implementation
        private static CentresViewModel _context;
        /// <summary>
        /// ViewModel for current app
        /// </summary>
        public static CentresViewModel Context { get { if (_context is null) { _context = new CentresViewModel(); } return _context; } }

        private static readonly Models.CentreFormRoomTupleConverter RoomTupleConverter = new Models.CentreFormRoomTupleConverter();

        /// <summary cref="Models.Entities.MeetingCentreService">
        /// Property reference to the current MeetingCentreService session
        /// </summary>
        public Models.Entities.MeetingCentreService CurrentService { get { return Models.Entities.MeetingCentreService.Current; } }
        private MeetingCentre.MeetingCentreForm _editCentre = MeetingCentre.MeetingCentreForm.Empty;
        /// <summary cref="Models.Entities.MeetingCentre.MeetingCentreForm">
        /// Form of currently selected Meeting Centre
        /// </summary>
        public MeetingCentre.MeetingCentreForm EditCentre { get { return this._editCentre; } set { this._editCentre = value; this.OnPropertyChanged("EditCentre", "EnableRoomControls"); } }
        /// <summary>
        /// Auxiliary collection for visual representation of changes made to currently edited Centre's Rooms
        /// </summary>
        public ObservableCollection<MeetingRoom> EditCentreRooms { get; private set; }

        private MeetingRoom.MeetingRoomForm _editRoom = MeetingRoom.MeetingRoomForm.Empty;
        /// <summary>
        /// Form of currently selected Meeting Room
        /// </summary>
        public MeetingRoom.MeetingRoomForm EditRoom { get { return this._editRoom; } private set { this._editRoom = value; this.OnPropertyChanged("EditRoom"); } }
        private bool _isCentreFormOpen = false;
        /// <summary>
        /// Whether the form for Centre is open
        /// </summary>
        public bool IsCentreFormOpen { get { return this._isCentreFormOpen; } private set { this._isCentreFormOpen = value; this.OnPropertyChanged("IsCentreFormOpen", "IsAnyFormOpen", "EnableRoomControls", "EnableCentreControls"); } }
        private bool _isRoomFormOpen = false;
        /// <summary>
        /// Whether the form for Room is open
        /// </summary>
        public bool IsRoomFormOpen { get { return this._isRoomFormOpen; } private set { this._isRoomFormOpen = value; this.OnPropertyChanged("IsRoomFormOpen", "IsAnyFormOpen", "EnableRoomControls", "EnableCentreControls"); } }
        /// <summary>
        /// Whether the controls for Rooms should be enabled
        /// </summary>
        public bool EnableRoomControls { get { return this.EditCentre.HasInstance() && !this.IsAnyFormOpen; } }
        /// <summary cref="IsCentreFormOpen" cref="IsRoomFormOpen">
        /// Whether any form is open  (from IsCentreFormOpen and IsRoomFormOpen)
        /// </summary>
        public bool IsAnyFormOpen { get { return this.IsCentreFormOpen || this.IsRoomFormOpen; } }
        /// <summary cref="IsAnyFormOpen" cref="ShouldSaveCentre">
        /// Whether the controls for Centres should be enabled
        /// </summary>
        public bool EnableCentreControls { get { return !(this.IsAnyFormOpen || this.ShouldSaveCentre); } }
        /// <summary>
        /// Whether any changes have beed made in the Room collection of currently edited Centre and the Centre should be saved
        /// </summary>
        public bool ShouldSaveCentre
        {
            get
            {
                if (this.EditCentre.RoomsChanged is null) return false;
                return this.EditCentre.RoomsChanged.Any(m => m.action != MeetingCentre.MeetingCentreForm.CollectionAction.Unchanged);
            }
        }

        /// <summary>
        /// Initializes the ViewModel
        /// </summary>
        private CentresViewModel()
        {
            this._loadService();
            this.PropertyChanged += (s, e) => { // Refresh rooms for when selected centre has changed
                if (e.PropertyName == "EditCentre") this.ResetRoomsForCurrentCentre();
            };
        }
        /// <summary cref="Models.Entities.MeetingCentreService">
        /// Asynchronously loads a MeetingCentreService
        /// </summary>
        private async void _loadService()
        {
            new Models.Entities.MeetingCentreService(await Models.Data.CsvImporter.ReadFromFileAsync("D:\\ImportData.csv"));
            this.OnPropertyChanged("CurrentService");
        }

        /// <summary>
        /// Shows form for a new Centre
        /// </summary>
        internal void ShowCentreForm()
        {
            this.EditCentre = MeetingCentre.GetNewForm();
            this.IsCentreFormOpen = true;
        }

        /// <summary>
        /// Shows a form for the given Centre
        /// </summary>
        /// <param name="edit">Source MeetingCentre for the form</param>
        internal void ShowCentreForm(MeetingCentre edit)
        {
            this.EditCentre = edit.GetEditForm(); // Pretty much auxiliary
            this.IsCentreFormOpen = true;
        }

        /// <summary>
        /// Hides the form for Centres
        /// </summary>
        internal void HideCentreForm()
        {
            this.IsCentreFormOpen = false;
        }

        /// <summary>
        /// Saves the Centre for current Centre Form
        /// </summary>
        internal void SaveCentre()
        {
            MeetingCentre saved = this.EditCentre.Save();
            if (!this.CurrentService.MeetingCentres.Contains(saved)) this.CurrentService.MeetingCentres.Add(saved);
            this.EditCentre = saved.GetEditForm();
            this.OnPropertyChanged("ShouldSaveCentre", "EnableCentreControls");
        }

        /// <summary>
        /// Deletes a Centre
        /// </summary>
        /// <param name="meetingCentre">MeetingCentre to be deleted</param>
        internal void DeleteCentre(MeetingCentre meetingCentre)
        {
            this.EditCentre = MeetingCentre.MeetingCentreForm.Empty;
            this.CurrentService.MeetingCentres.Remove(meetingCentre);
        }

        /// <summary>
        /// Refreshes rooms for currently selected Centre and discards any unsaved changes
        /// </summary>
        internal void ResetRoomsForCurrentCentre()
        {
            if (this.EditCentre.RoomsChanged != null)
            {
                for(int i = this.EditCentre.RoomsChanged.Count-1; i >= 0; i--)
                {
                    (MeetingRoom, MeetingCentre.MeetingCentreForm.CollectionAction) tuple = this.EditCentre.RoomsChanged[i];
                    if (tuple.Item2 == MeetingCentre.MeetingCentreForm.CollectionAction.Unchanged) continue;
                    this.EditCentre.RoomsChanged.RemoveAt(i);
                    if(tuple.Item2 == MeetingCentre.MeetingCentreForm.CollectionAction.Removed)
                    {
                        tuple.Item2 = MeetingCentre.MeetingCentreForm.CollectionAction.Unchanged;
                        EditCentre.RoomsChanged.Insert(i, tuple);
                    }
                }
                this.EditCentreRooms = new ObservableCollection<MeetingRoom>(RoomTupleConverter.Convert(this.EditCentre.RoomsChanged, null, null, null) as IEnumerable<MeetingRoom>);
                this.OnPropertyChanged("EditCentreRooms", "ShouldSaveCentre", "EnableCentreControls");
            }
        }

        /// <summary>
        /// Shows a form for a new Room
        /// </summary>
        internal void ShowRoomForm()
        {
            this.EditRoom = MeetingRoom.MeetingRoomForm.Empty;
            this.IsRoomFormOpen = true;
        }

        /// <summary>
        /// Show a form for the given Room
        /// </summary>
        /// <param name="meetingRoom">Source MeetingRoom</param>
        internal void ShowRoomForm(MeetingRoom meetingRoom)
        {
            this.EditRoom = meetingRoom.GetEditForm();
            this.IsRoomFormOpen = true;
        }

        /// <summary>
        /// Hides the form for Rooms
        /// </summary>
        internal void HideRoomForm()
        {
            this.IsRoomFormOpen = false;
        }

        /// <summary>
        /// Saves the currently edited Room
        /// </summary>
        internal void SaveRoom()
        {
            MeetingRoom saved = this.EditRoom.Save();
            if (!this.EditCentre.RoomsChanged.Any(r => r.room == saved))
            {
                this.EditCentre.RoomsChanged.Add((saved, MeetingCentre.MeetingCentreForm.CollectionAction.Added));
                this.EditCentreRooms.Add(saved);
            }
            this.EditRoom = MeetingRoom.MeetingRoomForm.Empty;
            this.OnPropertyChanged("ShouldSaveCentre", "EnableCentreControls");
        }

        /// <summary>
        /// Deletes the given Room with the option to revert
        /// </summary>
        /// <param name="meetingRoom"></param>
        internal void DeleteRoom(MeetingRoom meetingRoom)
        {
            this.EditCentreRooms.Remove(meetingRoom);
            (MeetingRoom, MeetingCentre.MeetingCentreForm.CollectionAction) tuple = this.EditCentre.RoomsChanged.First(n => n.room == meetingRoom);
            int tupleIndex = this.EditCentre.RoomsChanged.IndexOf(tuple);
            this.EditCentre.RoomsChanged.RemoveAt(tupleIndex);
            tuple.Item2 = MeetingCentre.MeetingCentreForm.CollectionAction.Removed;
            this.EditCentre.RoomsChanged.Insert(tupleIndex, tuple);
            this.OnPropertyChanged("ShouldSaveCentre", "EnableCentreControls");
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
