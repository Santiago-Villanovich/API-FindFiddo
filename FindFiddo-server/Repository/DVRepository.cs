using FindFiddo_server.DataAccess;

namespace FindFiddo_server.Repository
{
    public class DVRepository
    {
        DVContext _ctx;
        public DVRepository(IConfiguration configuration) {
        
            _ctx = new DVContext(configuration);
        }
        public bool GenerateBackUp()
        {
            return _ctx.GenerateBackUp();
        }
        public void RestoreBackUp()
        {
            try
            {
                _ctx.RestoreBackUp();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public bool UpdateDVtable(string tName,string tDV)
        {
            return _ctx.UpdateDVtable(tName, tDV);
        }

        public string GetDVbyName(string tName)
        {
            return _ctx.GetDVbyName(tName);
        }
        public bool UpdateDVuser(Guid idUsuario, string dv)
        {
            return _ctx.UpdateDVuser(idUsuario, dv);
        }
    }
}
