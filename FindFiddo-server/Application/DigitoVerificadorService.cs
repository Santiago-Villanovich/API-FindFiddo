using FindFiddo.DataAccess;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Application
{
    internal class DigitoVerificadorService
    {
        UserContext _ctx;
        public DigitoVerificadorService(IConfiguration configuration)
        {
            _ctx = new UserContext(configuration);
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

        public void ActualizarDigitoVerificadorUser(User usuario)
        {
            try
            {
                string DVnew = DigitoVerificador.CalcularDV(usuario);
                if(DVnew != usuario.DV)
                {
                    usuario.DV = DVnew;
                    _ctx.UpdateDVuser(usuario);
                    
                }     
                
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarDigitoVerificadorTable(IList<User> ususarios)
        {
            try
            {
                string DVTnew = DigitoVerificador.CalcularDVTabla(ususarios);
                _ctx.UpdateDVtable(DVTnew);
               
            }catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public bool VerificarDigitoTable(string DVtable)
        {
            IList<User> usuarios = new List<User>();
            usuarios = _ctx.GetAllUsuarios();
            string DVTnew = DigitoVerificador.CalcularDVTabla(usuarios);
            if(DVtable != DVTnew)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
