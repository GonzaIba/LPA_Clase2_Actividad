using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CuilValidator
{
    /// <summary>
    /// Descripción breve de WSCuil
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WSCuil : System.Web.Services.WebService
    {

        [WebMethod]
        public string ValidarCuil(int Tipo, int DNI, int DigitoVerificador)
        {
            int sumaTipo = SumaTipo(Tipo);
            if (sumaTipo == 0)
                return "El tipo no es válido";

            int sumaDNI = SumaDNI(DNI);
            int SumaTotal = sumaTipo + sumaDNI;
            string CuilValidado = CalcularDigitoVerificador(SumaTotal,DigitoVerificador);
            return CuilValidado;
        }

        private int SumaTipo(int Tipo)
        {
            int Result = Tipo == 27 ? 38 : 0;
            if (Result == 0)
                Result = Tipo == 20 ? 10 : 0;
            if (Result == 0)
                Result = Tipo == 30 ? 15 : 0;
            if (Result == 0)
                return 0;

            return Result;
        }

        private int SumaDNI(int DNIParametro)
        {
            string DNIString = DNIParametro.ToString();
            if (DNIString.Length < 8)
                DNIString = DNIString.PadLeft(8, '0');

            int Suma = 0;
            int[] Valores = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < DNIString.ToString().Length; i++)
            {
                string ValorPos_i_string = DNIString.ToString()[i].ToString();
                int ValorPos_i_int = Convert.ToInt32(ValorPos_i_string);
                
                Suma += ValorPos_i_int * Valores[i];
            }
            return Suma;
        }
        
        private string CalcularDigitoVerificador(int SumaTotal, int DigitoVerificador)
        {
            int division = SumaTotal / 11;
            int resto = SumaTotal - (division * 11);
            int resultado = 11 - resto;
            if(resultado == DigitoVerificador)
                return "El Cuil es válido";
            else
                return "El Cuil es inválido";

        }

    }
}
