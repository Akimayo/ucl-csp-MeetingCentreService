using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetingCentreService.Views
{
    /// <summary>
    /// Interaction logic for CentresView.xaml
    /// </summary>
    public partial class CentresView : Page
    {
        private ViewModels.CentresViewModel ViewModel = ViewModels.CentresViewModel.Context;
        public CentresView()
        {
            this.DataContext = this.ViewModel;
            InitializeComponent();
        }

        private void CentreFormCancel(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ResetRoomsForCurrentCentre();
            this.ViewModel.HideCentreForm();
        }

        private void CentreFormSave(object sender, RoutedEventArgs e)
        {
            if (!(this.CentreFormName.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.CentreFormCode.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.CentreFormDescription.GetBindingExpression(TextBox.TextProperty).HasValidationError))
            {
                this.ViewModel.SaveCentre();
                this.CentreFormCancel(sender, e);
            }
        }

        private void FormNewCentre(object sender, RoutedEventArgs e)
        {
            SelectCentre.SelectedIndex = -1;
            this.ViewModel.ShowCentreForm();
        }

        private void FormEditCentre(object sender, RoutedEventArgs e)
        {
            if (SelectCentre.SelectedItem != null) this.ViewModel.ShowCentreForm(SelectCentre.SelectedItem as Models.Entities.MeetingCentre);
        }

        private void DeleteCentre(object sender, RoutedEventArgs e)
        {
            if (SelectCentre.SelectedItem != null)
                if (MessageBox.Show($"Are you sure you want to remove centre {(SelectCentre.SelectedItem as Models.Entities.MeetingCentre).Name}?", "Remove Meeting Centre", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    this.ViewModel.DeleteCentre(SelectCentre.SelectedItem as Models.Entities.MeetingCentre);
        }

        private void FormNewRoom(object sender, RoutedEventArgs e)
        {
            SelectRoom.SelectedIndex = -1;
            this.ViewModel.ShowRoomForm();
        }

        private void FormEditRoom(object sender, RoutedEventArgs e)
        {
            if (SelectRoom.SelectedItem != null) this.ViewModel.ShowRoomForm(SelectRoom.SelectedItem as Models.Entities.MeetingRoom);
        }

        private void DeleteRoom(object sender, RoutedEventArgs e)
        {
            if (SelectRoom.SelectedItem != null)
                if (MessageBox.Show($"Are you sure you want to remove meeting room {(SelectRoom.SelectedItem as Models.Entities.MeetingRoom).Name} from meeting centre {(SelectRoom.SelectedItem as Models.Entities.MeetingRoom).MeetingCentre.Name}", "Remove Meeting Room", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    this.ViewModel.DeleteRoom(SelectRoom.SelectedItem as Models.Entities.MeetingRoom);
        }

        private void RoomFormCancel(object sender, RoutedEventArgs e)
        {
            this.ViewModel.HideRoomForm();
        }

        private void RoomFormSave(object sender, RoutedEventArgs e)
        {
            if (!(this.RoomFormName.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.RoomFormCode.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.RoomFormDescription.GetBindingExpression(TextBox.TextProperty).HasValidationError
               || this.RoomFormCapacity.GetBindingExpression(TextBox.TextProperty).HasValidationError))
            {
                this.ViewModel.SaveRoom();
                this.RoomFormCancel(sender, e);
            }
        }
    }
}
