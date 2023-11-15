using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Shapes
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            SelectedIndex = UserControlsList.FindIndex(x => x.name == "Dashed Rectangle");
        }

        private readonly List<(string name, UserControl control)> UserControlsList = new()
        {
            ("Ellipse", new EllipseUserControl()),
            ("Dashed Ellipse", new DashedEllipseUserControl()),
            ("Dashed Rectangle", new DashedRectangleUserControl())
        };



        public IEnumerable<string> UserControlNames => UserControlsList.Select(x => x.name);

        private UserControl? _selectedUserControl;

        public UserControl? SelectedUserControl
        {
            get => _selectedUserControl;
            set
            {
                _selectedUserControl = value;
                OnPropertyChanged(nameof(SelectedUserControl));
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                SelectedUserControl = UserControlsList[_selectedIndex].control;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
