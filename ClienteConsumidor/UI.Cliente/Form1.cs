using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace UI.Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtCuil_TextChanged(object sender, EventArgs e)
        {           
        }

        private void btnValidar_Click(object sender, EventArgs e)
        { 
            if (txtCuil.Text.Length == 11)
                txtCuil.Text = txtCuil.Text.Substring(0, 2) + "-" + txtCuil.Text.Substring(2, 8) + "-" + txtCuil.Text.Substring(10, 1);
            else if (txtCuil.Text.Length == 10)
                txtCuil.Text = txtCuil.Text.Substring(0, 2) + "-" + txtCuil.Text.Substring(2, 7) + "-" + txtCuil.Text.Substring(9, 1);

            var Resultado = ValidarCuil(txtCuil.Text);
            MessageBox.Show(Resultado);
        }

        private string ValidarCuil(string Cuil)
        {
            try
            {
                bool EsValido = Regex.IsMatch(Cuil, @"^\d{2}-\d{7,8}-\d{1}$");
                if (!EsValido)
                    return "El CUIL/CUIT ingresado no esta escrito correctamente";

                var CuilSplited = Cuil.Split('-');
                int Tipo = int.Parse(CuilSplited[0]);
                int DNI = int.Parse(CuilSplited[1]);
                int Digito = int.Parse(CuilSplited[2]);

                var WebService = new ServiceReference.WSCuilSoapClient();
                var CuilValidado = WebService.ValidarCuil(Tipo, DNI, Digito);
                return CuilValidado;
            }
            catch(TimeoutException)
            {
                return "El Servicio Web de la AFIP no responde. Intentelo nuevamente mas tarde.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
