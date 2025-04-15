using ShopTracker.Helpers;
namespace ShopTracker
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), 
                        "database_sqlite_shopping.db3");

                    _db = new SQLiteDatabaseHelper(path); 
                }
                return _db;
            }
        }
        public App()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            InitializeComponent();

            MainPage = new NavigationPage(new Views.ItemList());

        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 700;
            window.Height = 750;

            return window;
        }

    }
}
