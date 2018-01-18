using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training1._1.ViewModel
{
    public class ButtonVM:ViewModelBase
    {
        string name = "toggle";
        bool state = true;

        public string Name { get => name; private set => name = value; }
        public bool State
        {
            get => state; set
            {
                state = value;
                RaisePropertyChanged();
            }
        }
    }
}
