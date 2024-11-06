using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;
using FindFiddo_server.Entities;
using FindFiddo_server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Application
{
    public interface IDigitoVerificadorService
    {
        bool GenerateBackUp();

        void RestoreBackUp();

        IList<User> VerificarDigitoXuser(IList<User> Usuarios);

        void UpdateDVTable(string tableName, IEnumerable<IVerificable> entidades);

        bool VerificarDigitoTable(string tableName, IEnumerable<IVerificable> entidades);

        void RecalcularUserDVTable(string tableName, IEnumerable<IVerificable> entidades);

        bool UpdateDVuser(Guid idUsuario, string dv);

        bool DeleteMirror();

        List<MirrorUser> GetIntegrityIssues();

    }
    public class DVApp: IDigitoVerificadorService
    {
        IDVRepository _repo;
        public DVApp(IDVRepository repo)
        {
            _repo = repo;
        }

        public bool GenerateBackUp()
        {
            return _repo.GenerateBackUp();
        }
        public void RestoreBackUp()
        {
            try
            {
                _repo.RestoreBackUp();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public IList<User> VerificarDigitoXuser(IList<User> Usuarios)
        {
            try
            {
                IList<User> UsuariosModificados = new List<User>();
                foreach (User usuario in Usuarios)
                {
                    var DVnew = DigitoVerificador.CalcularDV(usuario);
                    if (DVnew != usuario.DV)
                    {
                        UsuariosModificados.Add(usuario);
                    }
                }

                return UsuariosModificados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public void UpdateDVTable(string tableName, IEnumerable<IVerificable> entidades)
        {
            try
            {
                string DVTnew = DigitoVerificador.CalcularDVTabla(entidades);
                _repo.UpdateDVtable(tableName, DVTnew);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RecalcularUserDVTable(string tableName,IEnumerable<IVerificable> entidades)
        {
            try
            {
                foreach (User u in entidades)
                {
                    u.DV = DigitoVerificador.CalcularDV(u);
                    UpdateDVuser(u.Id,u.DV);
                }

                string DVTnew = DigitoVerificador.CalcularDVTabla(entidades);
                _repo.UpdateDVtable(tableName, DVTnew);

            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificarDigitoTable(string tableName, IEnumerable<IVerificable> entidades)
        {
            string storedDV = _repo.GetDVbyName(tableName);
            string calculedDV = DigitoVerificador.CalcularDVTabla(entidades);

            if(storedDV.Equals(calculedDV))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateDVuser(Guid idUsuario, string dv)
        {
            return _repo.UpdateDVuser(idUsuario, dv);
        }
        public bool DeleteMirror() {  return _repo.DeleteMirror(); }

        public List<MirrorUser> GetIntegrityIssues()
        {
            return _repo.GetIntegrityIssues(); 
        }
    }
}
