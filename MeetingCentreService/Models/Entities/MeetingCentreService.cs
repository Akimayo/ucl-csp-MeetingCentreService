﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Represents current session of working with MeetingCentres
    /// </summary>
    public class MeetingCentreService : INotifyPropertyChanged
    {
        /// <summary cref="MeetingCentreService">
        /// Last created MeetingCentreService
        /// </summary>
        public static MeetingCentreService Current;
        /// <summary>
        /// Save path for current session
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Indicates whether any changes have been made during this session
        /// </summary>
        public bool ServiceChanged { get; private set; }
        /// <summary cref="MeetingCentre">
        /// Collection of MeetingCentres for current session 
        /// </summary>
        public ObservableCollection<MeetingCentre> MeetingCentres { get; }

        /// <summary>
        /// Create a new session
        /// </summary>
        public MeetingCentreService()
        {
            this.MeetingCentres = new ObservableCollection<MeetingCentre>();
            this.MeetingCentres.CollectionChanged += CentresCollectionChanged;
            this.ServiceChanged = false;
            MeetingCentreService.Current = this;
        }

        /// <summary>
        /// Create a session from an imported MeetingCentre collection
        /// </summary>
        /// <param name="import"></param>
        public MeetingCentreService(IList<MeetingCentre> import)
        {
            foreach (MeetingCentre centre in import)
                centre.PropertyChanged += CentreChanged;
            this.MeetingCentres = new ObservableCollection<MeetingCentre>(import);
            this.MeetingCentres.CollectionChanged += CentresCollectionChanged;
            this.ServiceChanged = false;
            MeetingCentreService.Current = this;
        }

        /// <summary>
        /// Event handler for changes made to the MeetingCentres collection and its items
        /// </summary>
        private void CentresCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("MeetingCentres");
            if (e.NewItems != null)
                foreach (MeetingCentre centre in e.NewItems)
                    centre.PropertyChanged += CentreChanged;
            if (e.OldItems != null)
                foreach (MeetingCentre centre in e.OldItems)
                    centre.PropertyChanged -= CentreChanged;
            this.ServiceChanged = true;
        }

        /// <summary>
        /// Event handler for changes made to MeetingCentre items
        /// </summary>
        private void CentreChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("MeetingCentres");
            this.ServiceChanged = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}