using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using System.Data.Entity;

namespace MeetingCentreService.Models.Entities
{
    public class Accessory : INotifyPropertyChanged
    {
        [Key]
        public long AccessoryId { get; set; }
        private AccessoriesCategory _category;
        public AccessoriesCategory Category { get { return this._category; } set { this._category = value; this.OnPropertyChanged("Category"); } }
        private string _name;
        public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
        private int _recommendedMinimumStock;
        public int RecommendedMinimumStock { get { return this._recommendedMinimumStock; } set { this._recommendedMinimumStock = value; this.OnPropertyChanged("RecommendedMinimumStock"); this.OnPropertyChanged("IsBelowMinimum"); } }
        private int _stock;
        public int Stock { get { return this._stock; } private set { this._stock = value; this.OnPropertyChanged("Stock"); this.OnPropertyChanged("IsBelowMinimum"); } }
        public bool IsBelowMinimum { get { return this.Stock < this.RecommendedMinimumStock; } }
        private bool _isVisible;
        public bool IsVisible { get { return this._isVisible; } private set { this._isVisible = value; this.OnPropertyChanged("IsVisible"); } }
        public IList<AccessoryHandOutOccurance> AccessoryHandOuts { get; private set; }

        public Accessory()
        {
            this.Stock = 0;
            this.IsVisible = true;
            this.AccessoryHandOuts = new List<AccessoryHandOutOccurance>();
        }

        public void Remove()
        {
            this.IsVisible = false;
        }

        private void HandOut(int handOut, string handOutTo)
        {
            this.Stock -= handOut;
            this.AccessoryHandOuts.Add(new AccessoryHandOutOccurance(this, handOutTo, handOut));
        }

        private void Restock(int restock)
        {
            this.Stock += restock;
        }

        public AccessoryForm GetEditForm()
        {
            return new AccessoryForm(this);
        }

        public static AccessoryForm GetNewForm()
        {
            return new AccessoryForm();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }

        public class AccessoryHandOutOccurance
        {
            [Key]
            public long OccuranceId { get; private set; }
            public DateTime Timestamp { get; private set; }
            public string Customer { get; private set; }
            public int CountHandedOut { get; private set; }
            public Accessory Accessory { get; private set; }
            
            public AccessoryHandOutOccurance(Accessory accessory, string customer, int count)
            {
                this.Timestamp = DateTime.Now;
                this.Customer = customer;
                this.CountHandedOut = count;
                this.Accessory = accessory;
            }
        }

        public class AccessoryForm : INotifyPropertyChanged
        {
            private Accessory Instance;
            private AccessoriesCategory _category;
            public AccessoriesCategory Category { get { return this._category; } set { this._category = value; this.OnPropertyChanged("Category"); } }
            public IEnumerable<AccessoriesCategory> Categories { get {
                    return Entities.MeetingCentreService.Current.AccessoriesContext.CategorySet.Local;
                } }
            private string _name;
            public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
            private int _recommendedMinimumStock;
            public int RecommendedMinimumStock { get { return this._recommendedMinimumStock; } set { this._recommendedMinimumStock = value; this.OnPropertyChanged("RecommendedMinimumStock"); } }
            public int ToFull
            {
                get
                {
                    if (this.Instance != null) return 1000 - this.Instance.Stock;
                    else return 1000;
                }
            }
            private int _restock;
            public int Restock
            {
                get { return this._restock; }
                set
                {
                    if (this.Instance != null)
                    {
                        if (value <= 1000 - this.Instance.Stock) this._restock = value;
                        else this._restock = 1000 - this.Instance.Stock;
                    }
                    else this._restock = 0;
                }
            }
            public int InStock { get { if (this.Instance != null) return this.Instance.Stock; else return 0; } }
            private int _handOut;
            public int HandOut { get { return this._handOut; } set
                {
                    if (this.Instance != null)
                    {
                        if (value <= this.Instance.Stock) this._handOut = value;
                        else this._handOut = this.Instance.Stock;
                    }
                    else this._handOut = 0;
                } }
            private string _handOutTo;
            public string HandOutTo { get { return this._handOutTo; }set { this._handOutTo = value; this.OnPropertyChanged("HandOutTo"); } }
            public AccessoryForm()
            {
                this.RecommendedMinimumStock = 10;
            }
            public AccessoryForm(Accessory accessory)
            {
                this.Category = accessory.Category;
                this.Name = accessory.Name;
                this.RecommendedMinimumStock = accessory.RecommendedMinimumStock;
                this.Instance = accessory;
            }
            public Accessory Save()
            {
                if (this.Instance is null) this.Instance = new Accessory();
                this.Instance.Category = this.Category;
                this.Instance.Name = this.Name;
                this.Instance.RecommendedMinimumStock = this.RecommendedMinimumStock;
                if (this.Restock > 0) this.Instance.Restock(this.Restock);
                if (this.HandOut > 0) this.Instance.HandOut(this.HandOut, this.HandOutTo);
                return this.Instance;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler evt = this.PropertyChanged;
                if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
            }

            public bool HasInstance()
            {
                return this.Instance != null;
            }
        }
    }
}
