using Xamarin.Forms;

namespace Shopel
{
    public partial class App : Application
    {
        public static string FilePath;
        public static string ApiUrl;
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
        public App(string filePath, string apiUrl)
        {
            InitializeComponent();
            MainPage = new AppShell();
            FilePath = filePath;
            ApiUrl = apiUrl;
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
