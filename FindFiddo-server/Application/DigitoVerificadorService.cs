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
    internal class DigitoVerificadorService
    {
        DVRepository _repo;
        public DigitoVerificadorService(IConfiguration configuration)
        {
            _repo = new DVRepository(configuration);
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
                string DVTnew = DigitoVerificador.CalcularDVTabla(entidades);
                _repo.UpdateDVtable("usuario", tableName);

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
    }
}
