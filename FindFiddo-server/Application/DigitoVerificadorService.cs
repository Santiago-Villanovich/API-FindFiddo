using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;
using FindFiddo_server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Application
{
    public class DigitoVerificadorService
    {
        DVRepository _repo;
        public DigitoVerificadorService(IConfiguration configuration)
        {
            _repo = new DVRepository(configuration);
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


        public void UpdateUserDVTable(string tableName,IEnumerable<IVerificable> entidades)
        {
            try
            {
                foreach (User u in entidades)
                {
                    UpdateDVuser(u.Id,DigitoVerificador.CalcularDV(u));
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
    }
}
