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
        public static AccessoriesViewModel Context
        {
            get
            {
                if (AccessoriesViewModel._context is null) AccessoriesViewModel._context = new AccessoriesViewModel();
                return AccessoriesViewModel._context;
            }
        }
        private ObservableCollection<Accessory> Accessories { get {
                return Models.Entities.MeetingCentreService.Current.AccessoriesContext.AccessorySet.Local;
            } }
        public IEnumerable<Accessory> AccessoriesList { get { return this.Accessories.Where(a => a.IsVisible); } }
        private Accessory _selectedAccessory;
        public Accessory SelectedAccessory { get { return this._selectedAccessory; } set { this._selectedAccessory = value; this.OnPropertyChanged("SelectedAccessory"); } }

        private AccessoriesViewModel() { }

        public void Save(Accessory.AccessoryForm accessory)
        {
            Accessory saved = accessory.Save();
            if (!this.Accessories.Contains(saved)) this.Accessories.Add(saved);
            this.OnPropertyChanged("Accessories", "AccessoriesList");
        }

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