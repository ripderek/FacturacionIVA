using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacturacionIVA
{
    public partial class Facturacion : Form
    {
        public Facturacion()
        {
            InitializeComponent();
        }
        private decimal cantidad;
        private decimal precio;
        private void button1_Click(object sender, EventArgs e)
        {
            //si los controles estan llenos
            if (verificar_controles())
            {
                //si los controles contienen numeros
                if (verificar_numeros())
                {
                    //agregar los productos al datagridview
                    dataGridView1.Rows.Add((dataGridView1.Rows.Count + 1).ToString(), txtProducto.Text.ToString(), cantidad.ToString("N2"), precio.ToString("N2"),(cantidad * precio).ToString("N2"));
                    limpiar_controles();
                    Calcular_Subtotal();
                }
                else MessageBox.Show("Ingrese numeros por favor");
            }
            else
                MessageBox.Show("Por favor llene el formulario antes de ingresar un producto ");
        }
        //funcion para limpiar todos los controles
        private void limpiar_controles()
        {
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtProducto.Text = "";
        }
        //Funcion para validar los controles 
        private bool verificar_controles()
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text)) 
              return false;
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtProducto.Text))
                return false;
            return true; 
        }
       
        private bool verificar_numeros()
        {
            string _cantidad = txtCantidad.Text;
            string _precio = txtPrecio.Text;
            decimal numeroDecimal;

            if (decimal.TryParse(_cantidad, out numeroDecimal))
                cantidad = decimal.Parse(_cantidad);
            else
                return false;
            if (decimal.TryParse(_precio, out numeroDecimal))
                precio = decimal.Parse(_precio);
            else
                return false;
            return true;
        }

        private void Facturacion_Load(object sender, EventArgs e)
        {
            cmbIva.SelectedIndex = 0;
        }
        //funcion para calcular el subtotal, es decir, solo la suma de todos los totales
        private void Calcular_Subtotal()
        {
            decimal suma = 0;

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.IsNewRow) continue; // Salta la fila de edición

                // Asegúrate de que la celda en el índice 2 contenga un valor numérico
                if (fila.Cells[4].Value != null && decimal.TryParse(fila.Cells[4].Value.ToString(), out decimal valorCelda))
                {
                    suma += valorCelda;
                }
            }
            txtSubtotal.Text = suma.ToString();
            CalcularIVA(suma);
        }
        private void CalcularIVA(decimal total)
        {
            decimal precioDeVenta = total;
            decimal tasaDeIVA = (cmbIva.SelectedIndex == 0 )? 0m:0.12m ;
            decimal baseImponible = precioDeVenta;
            decimal iva = baseImponible * tasaDeIVA;
            decimal precioTotalConIVA = baseImponible + iva;
            txtTotal.Text = precioTotalConIVA.ToString("N2");
        }

        private void cmbIva_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubtotal.Text))
            { return; }
 
            decimal val = decimal.Parse(txtSubtotal.Text.ToString());
            CalcularIVA(val);
        }
        int fila = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                 fila = e.RowIndex;
            }
        }
        private void Eliminar_Producto()
        {
            if (fila >= 0)
            {
                dataGridView1.Rows.RemoveAt(fila);
                Calcular_Subtotal();
            } 
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar_Producto();
        }
    }
}
