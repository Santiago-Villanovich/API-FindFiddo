using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Services
{
    public static class DigitoVerificador
    {
        public static string CalcularDV(IVerificable entity)

        {
            Type t = entity.GetType();
            string DV = string.Empty;
            var props = t.GetProperties();

            foreach (var item in props)
            {
                var atributos = item.GetCustomAttributes();
                var verificable = atributos.FirstOrDefault(i => i.GetType().Equals(typeof(VerificableProperty)));

                if (verificable != null)
                {
                    DV += item.GetValue(entity).ToString();
                }
            }

            return EncryptService.GenerarMD5(DV);
        }

        public static string CalcularDVTabla(IEnumerable<IVerificable> ususarios)//IEnumerable<IVerificable> List)
        {
            string DVT = string.Empty;
            if (ususarios != null)
            {

                foreach (User i in ususarios)
                {
                    DVT += i.DV;
                }

            }

            return EncryptService.GenerarMD5(DVT);
        }
    }
}
