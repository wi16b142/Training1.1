using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using Training1._1.Com;

namespace Training1._1.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        Communication com;
        bool isConnected = false;
        private RelayCommand _connectBtnClick;
        public ObservableCollection<ButtonVM> Buttons
        {
            get => _buttons; set
            {
                _buttons = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand ConnectBtnClick
        {
            get => _connectBtnClick; set
            {
                _connectBtnClick = value;
                RaisePropertyChanged();
            }
        }
        private RelayCommand listenBtnClick;
        private ObservableCollection<ButtonVM> _buttons;
        public RelayCommand ListenBtnClick
        {
            get { return listenBtnClick; }
            set { listenBtnClick = value; RaisePropertyChanged(); }
        }
        private RelayCommand<ButtonVM> toggleBtnClick;
        public RelayCommand<ButtonVM> ToggleBtnClick
        {
            get { return toggleBtnClick; }
            set { toggleBtnClick = value; }
        }
        private ObservableCollection<string> log;
        bool isServer = false;

        public ObservableCollection<string> Log
        {
            get { return log; }
            set { log = value; }
        }
        
        public MainViewModel()
        {
            Buttons = new ObservableCollection<ButtonVM>();
            Log = new ObservableCollection<string>();

            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());
            Buttons.Add(new ButtonVM());

            if (IsInDesignMode)
            {
                //nothing to do
            }
            else
            {
                ListenBtnClick = new RelayCommand(()=> 
                {
                    isServer = true;
                    com = new Communication(isServer,GuiUpdate);
                    isConnected = true;
                }, ()=> { return !isConnected; } );

                ConnectBtnClick = new RelayCommand(()=> 
                {
                    com = new Communication(isServer, GuiUpdate);
                    isConnected = true;
                },()=> { return !isConnected; });

                ToggleBtnClick = new RelayCommand<ButtonVM>((p) =>
                {
                    string msg = p.Name + "@" + !p.State;
                    com.Send(msg);
                    GuiUpdate(msg);
                }, (p) => { return isServer; });
            }




        }

        private void GuiUpdate(string msg)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                string buttonname = msg.Split('@')[0];
                string newstate = msg.Split('@')[1];

                foreach (var button in Buttons)
                {
                    if (button.Name.Equals(buttonname))
                    {
                        button.State = bool.Parse(newstate);
                        Log.Add(button.Name + " new State: " + button.State.ToString());
                    }
                } 

            });
        }

    }
}