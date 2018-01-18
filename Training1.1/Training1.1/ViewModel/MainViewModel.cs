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
                    com = new Communication(true,GuiUpdate);
                    isConnected = true;
                }, ()=> { return !isConnected; } );

                ConnectBtnClick = new RelayCommand(()=> 
                {
                    com = new Communication(false, GuiUpdate);
                    isConnected = true;
                },()=> { return !isConnected; });               
            }

            ToggleBtnClick = new RelayCommand<ButtonVM>((p)=> 
            {
                string msg = p.ToString() + "@" + !p.State;
                //com.Send(msg);
                GuiUpdate(msg);
            },(p)=> { return !com.IsServer; });


        }

        private void GuiUpdate(string msg)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                string buttonvm = msg.Split('@')[0];
                string newstate = msg.Split('@')[1];

                foreach (var button in Buttons)
                {
                    if (button.ToString().Equals(buttonvm))
                    {
                        button.State = bool.Parse(newstate);
                        Log.Add(button.ToString() + " new State: " + button.State.ToString());
                    }
                } 

            });
        }

    }
}