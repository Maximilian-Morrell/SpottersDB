namespace SpottersDB_BackEnd.Classes.API
{
    public abstract class API_Base
    {
        protected WebApplication app;

        public API_Base(WebApplication app)
        {
            this.app = app;
            MainAPI();
        }

        protected abstract void MainAPI();
    }
}
