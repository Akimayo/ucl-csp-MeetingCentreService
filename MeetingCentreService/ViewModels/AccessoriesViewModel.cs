using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace MeetingCentreService.ViewModels
{
    public class AccessoriesViewModel : INotifyPropertyChanged
    {
        public static AccessoriesViewModel _context;
        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static AccessoriesViewModel Context
        {
            get
            {
                if (AccessoriesViewModel._context is null) AccessoriesViewModel._context = new AccessoriesViewModel();
                return AccessoriesViewModel._context;
            }
        }
        /// <summary>
        /// Collection of Accessories
        /// </summary>
        private ObservableCollection<Accessory> Accessories
        {
            get
            {
                return Models.Entities.MeetingCentreService.Current.AccessoriesContext.AccessorySet.Local;
            }
        }
        /// <summary>
        /// Collection of non-deleted (visible) Accessories
        /// </summary>
        public IEnumerable<Accessory> AccessoriesList { get { return this.Accessories.Where(a => a.IsVisible); } }
        private Accessory _selectedAccessory;
        /// <summary>
        /// Currently selected Accessory in view
        /// </summary>
        public Accessory SelectedAccessory { get { return this._selectedAccessory; } set { this._selectedAccessory = value; this.OnPropertyChanged("SelectedAccessory"); } }

        /// <summary>
        /// Creates the ViewModel
        /// </summary>
        private AccessoriesViewModel() { }

        /// <summary>
        /// Saves an Accessory
        /// </summary>
        /// <param name="selectedAccessory">Saved Accessory</param>
        public void Save(Accessory.AccessoryForm selectedAccessory)
        {
            Accessory saved = selectedAccessory.Save();
            /*
             * Fun fact: I was debugging this part for about three hours
             *           before I noticed that I missed the negation here:
             */
            if (!this.Accessories.Contains(saved)) this.Accessories.Add(saved);
            this.OnPropertyChanged("Accessories", "AccessoriesList");
        }

        /// <summary>
        /// Removes (marks as invisible) an Accessory
        /// </summary>
        /// <param name="selectedAccessory">Accessory to be removed</param>
        internal void RemoveAccessory(Accessory selectedAccessory)
        {
            selectedAccessory.Remove();
            this.OnPropertyChanged("AccessoriesList");
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