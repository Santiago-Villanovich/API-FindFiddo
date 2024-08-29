using FindFiddo_server.DataAccess;
using FindFiddo_server.Entities;

namespace FindFiddo_server.Repository
{
    public class DVRepository
    {
        IDVContext _ctx;
        public DVRepository(IConfiguration configuration, IDVContext ctx) {
        
            _ctx = ctx;
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

        public bool DeleteMirror()
        {
            return _ctx.DeleteMirror(); 
        }

        public List<MirrorUser> GetIntegrityIssues()
        {
            return _ctx.GetIntegrityIssues();
        }
    }
}
