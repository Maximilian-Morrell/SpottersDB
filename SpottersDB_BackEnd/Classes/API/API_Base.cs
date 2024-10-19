using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd.Classes.API
{
    public abstract class API_Base
    {
        protected WebApplication app;
        protected SQLController sqlcontroller;

        public API_Base(WebApplication app, SQLController sqlcontroller)
        {
            this.app = app;
            this.sqlcontroller = sqlcontroller;
            MainAPI();
        }

        protected abstract void MainAPI();
    }
}
