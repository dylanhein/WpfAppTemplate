
namespace VS2013_WpfTemplate
{
    public class MainWindowViewModel : ViewModel
    {
        private MainWindow mainWindow;

        private string sampleText;
        /// <summary>
        /// Sample Text used to demonstrate binding.
        /// </summary>
        public string SampleText {
            get {
                return sampleText;
            }
            set {
                if (!string.Equals(sampleText, value, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    sampleText = value;
                    RaisePropertyChanged("SampleText");
                }
            }
        }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            SampleText = "Hello, Type Here";
        }
    }
}
