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
        /// <summary>
        /// Database primary key
        /// </summary>
        [Key]
        public long AccessoryId { get; set; }
        private AccessoriesCategory _category;
        /// <summary>
        /// Category this Accessory belongs to
        /// </summary>
        public AccessoriesCategory Category { get { return this._category; } set { this._category = value; this.OnPropertyChanged("Category"); } }
        private string _name;
        /// <summary>
        /// Name of the Accessory
        /// </summary>
        public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
        private int _recommendedMinimumStock;
        /// <summary>
        /// Recommended minimum amount of units of this Accessory in stock
        /// </summary>
        public int RecommendedMinimumStock { get { return this._recommendedMinimumStock; } set { this._recommendedMinimumStock = value; this.OnPropertyChanged("RecommendedMinimumStock"); this.OnPropertyChanged("IsBelowMinimum"); } }
        private int _stock;
        /// <summary>
        /// Current amount of units of this Accessory in stock
        /// </summary>
        public int Stock { get { return this._stock; } private set { this._stock = value; this.OnPropertyChanged("Stock"); this.OnPropertyChanged("IsBelowMinimum"); } }
        /// <summary>
        /// Whether the stock of this Accessory is below the recommended minimum
        /// </summary>
        public bool IsBelowMinimum { get { return this.Stock < this.RecommendedMinimumStock; } }
        private bool _isVisible;
        /// <summary>
        /// Whether this Accessory is visible or deleted
        /// </summary>
        public bool IsVisible { get { return this._isVisible; } private set { this._isVisible = value; this.OnPropertyChanged("IsVisible"); } }
        /// <summary>
        /// Databse foreign key reference for AccessoryHandOutOccurances
        /// </summary>
        public IList<AccessoryHandOutOccurance> AccessoryHandOuts { get; private set; }

        /// <summary>
        /// Create a new Accessory
        /// </summary>
        public Accessory()
        {
            this.Stock = 0;
            this.IsVisible = true;
            this.AccessoryHandOuts = new List<AccessoryHandOutOccurance>();
        }

        /// <summary>
        /// Delete this accessory
        /// </summary>
        public void Remove()
        {
            this.IsVisible = false;
        }

        /// <summary>
        /// Hand this Accessory out from stock
        /// </summary>
        /// <param name="handOut">Amount of units handed out</param>
        /// <param name="handOutTo">Customer the Accessory is handed out to</param>
        private void HandOut(int handOut, string handOutTo)
        {
            this.Stock -= handOut;
            this.AccessoryHandOuts.Add(new AccessoryHandOutOccurance(this, handOutTo, handOut));
        }

        /// <summary>
        /// Restock this Accessory
        /// </summary>
        /// <param name="restock">Amount of units restocked</param>
        private void Restock(int restock)
        {
            this.Stock += restock;
        }

        /// <summary>
        /// Get an edit form for this Accessory
        /// </summary>
        /// <returns cref="AccessoryForm">Edit form for this Accessory</returns>
        public AccessoryForm GetEditForm()
        {
            return new AccessoryForm(this);
        }

        /// <summary>
        /// Get a form for a new Accessory
        /// </summary>
        /// <returns cref="AccessoryForm">Form for a new Accessory</returns>
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

        /// <summary>
        /// Helper class for tracking Accessory hand outs
        /// </summary>
        public class AccessoryHandOutOccurance
        {
            /// <summary>
            /// Database primary key
            /// </summary>
            [Key]
            public long OccuranceId { get; private set; }
            /// <summary>
            /// Date and time the hand out occured
            /// </summary>
            public DateTime Timestamp { get; private set; }
            /// <summary>
            /// Customer the accessory has been handed out to
            /// </summary>
            public string Customer { get; private set; }
            /// <summary>
            /// Amount of units handed out
            /// </summary>
            public int CountHandedOut { get; private set; }
            /// <summary>
            /// Database foreign key reference for Accessory
            /// </summary>
            public Accessory Accessory { get; private set; }

            /// <summary>
            /// Create a hand out occurance
            /// </summary>
            /// <param name="accessory">Accessory handed out</param>
            /// <param name="customer">Name of the customer</param>
            /// <param name="count">Amount of units handed out</param>
            public AccessoryHandOutOccurance(Accessory accessory, string customer, int count)
            {
                this.Timestamp = DateTime.Now;
                this.Customer = customer;
                this.CountHandedOut = count;
                this.Accessory = accessory;
            }
        }

        /// <summary>
        /// Form for editin Accessories
        /// </summary>
        public class AccessoryForm : INotifyPropertyChanged
        {
            /// <summary>
            /// Edited instance of Accessory
            /// </summary>
            private Accessory Instance;
            private AccessoriesCategory _category;
            /// <summary>
            /// Category this Accessory belongs to
            /// </summary>
            public AccessoriesCategory Category { get { return this._category; } set { this._category = value; this.OnPropertyChanged("Category"); } }
            /// <summary>
            /// Helper list of all Categories
            /// </summary>
            public IEnumerable<AccessoriesCategory> Categories
            {
                get
                {
                    return Entities.MeetingCentreService.Current.AccessoriesContext.CategorySet.Local;
                }
            }
            private string _name;
            /// <summary>
            /// Name of the Accessory
            /// </summary>
            public string Name { get { return this._name; } set { this._name = value; this.OnPropertyChanged("Name"); } }
            private int _recommendedMinimumStock;
            /// <summary>
            /// Recommended minimum amount of units of this Accessory in stock
            /// </summary>
            public int RecommendedMinimumStock { get { return this._recommendedMinimumStock; } set { this._recommendedMinimumStock = value; this.OnPropertyChanged("RecommendedMinimumStock"); } }
            /// <summary>
            /// Maximum amount of units to be restocked until stock capacity is reached
            /// </summary>
            public int ToFull
            {
                get
                {
                    if (this.Instance != null) return 1000 - this.Instance.Stock;
                    else return 1000;
                }
            }
            private int _restock;
            /// <summary>
            /// Amount of units restocked
            /// </summary>
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
            /// <summary>
            /// Amount of units in stock.
            /// Helper for new Accessories.
            /// </summary>
            public int InStock { get { if (this.Instance != null) return this.Instance.Stock; else return 0; } }
            private int _handOut;
            /// <summary>
            /// Amount of units handed out
            /// </summary>
            public int HandOut
            {
                get { return this._handOut; }
                set
                {
                    if (this.Instance != null)
                    {
                        if (value <= this.Instance.Stock) this._handOut = value;
                        else this._handOut = this.Instance.Stock;
                    }
                    else this._handOut = 0;
                }
            }
            private string _handOutTo;
            /// <summary>
            /// Customer being handed out to
            /// </summary>
            public string HandOutTo { get { return this._handOutTo; } set { this._handOutTo = value; this.OnPropertyChanged("HandOutTo"); } }
            /// <summary>
            /// Creates an AccessoryForm for a new Accessory
            /// </summary>
            public AccessoryForm()
            {
                this.RecommendedMinimumStock = 10;
            }
            /// <summary>
            /// Creates an Accessory form for the given Accesory
            /// </summary>
            /// <param name="accessory">Accessory the form is being created for</param>
            public AccessoryForm(Accessory accessory)
            {
                this.Category = accessory.Category;
                this.Name = accessory.Name;
                this.RecommendedMinimumStock = accessory.RecommendedMinimumStock;
                this.Instance = accessory;
            }
            /// <summary>
            /// Saves the edited Accessory or creates a new one
            /// </summary>
            /// <returns>Saved Accessory</returns>
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
            /// <summary>
            /// Returns whether this is a form for an existing Accessory
            /// </summary>
            /// <returns>Whether this is an edit form</returns>
            public bool HasInstance()
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
