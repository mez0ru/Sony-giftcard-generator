using System;
using System.ComponentModel;

namespace Sony_giftcard_generator
{
    public class SampleDialogViewModel : INotifyPropertyChanged
    {
        private string _name;

        public string PPPP
        {
            get { return _name; }
            set
            {
                this.MutateVerbose(ref _name, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}