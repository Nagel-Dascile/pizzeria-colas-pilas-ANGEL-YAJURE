using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Angel_Yajure_2b_Evaluacion
{
	public partial class MainForm : Form
	{
		private Queue<string> colaPedidos = new Queue<string>();
		private Queue<string> colaPedidosPremium = new Queue<string>();
		private Stack<string> pilaBitacora = new Stack<string>();

		public MainForm()
		{
			InitializeComponent();
			ActualizarUI();
		}

		private void BtnNuevoPedido_Click(object sender, EventArgs e)
		{
			string cliente = txtCliente.Text.Trim();
			
			if (cliente == "")
			{
				lblEstado.Text = string.Format("⚠️ Debe ingresar un nombre de cliente.");
				return;
			}

			colaPedidos.Enqueue(cliente);

			pilaBitacora.Push(string.Format("Pedido: {0}", cliente));
			txtCliente.Clear();
			lblEstado.Text = string.Format("✅ Pedido registrado para {0}", cliente);
			ActualizarUI();
		}
		private void BtnEntregar_Click(object sender, EventArgs e)
		{
			if (colaPedidos.Count == 0)
			{
				lblEstado.Text = string.Format("❌ No hay pedidos pendientes.");
				return;
			}

			string cliente = colaPedidos.Dequeue();

			pilaBitacora.Push(string.Format("ENTREGADO: {0}", cliente));
			lblEstado.Text = string.Format("🍕 Pedido entregado a {0}", cliente);
			ActualizarUI();
		}
		
		private void BtnDeshacer_Click(object sender, EventArgs e)
		{
			if (pilaBitacora.Count == 0)
			{
				lblEstado.Text = string.Format("📭 No hay acciones para deshacer.");
				return;
			}

			string ultimaAccion = pilaBitacora.Pop();

			if (ultimaAccion.StartsWith("PEDIDO:"))
			{
				string nombre = ultimaAccion.Replace("PEDIDO: ", "").Trim();
				string[] temporal = colaPedidos.ToArray();
				colaPedidos.Clear();
				foreach (string p in temporal)
				{
					if (p != nombre)
						colaPedidos.Enqueue(p);
				}
				lblEstado.Text = string.Format("Se deshizo el pedido de {0}", nombre);
			}
			else if (ultimaAccion.StartsWith("ENTREGADO:"))
			{
				string nombre = ultimaAccion.Replace("ENTREGADO: ", "").Trim();
				colaPedidos.Enqueue(nombre);
				lblEstado.Text = string.Format("↩️ Se deshizo la entrega a {0}", nombre);
			}
			else
			{
				lblEstado.Text = string.Format("⚠️ Acción desconocida en bitácora.");
			}

			ActualizarUI();
		}
		
		private void BtnLimpiar_Click(object sender, EventArgs e)
		{
			colaPedidos.Clear();
			pilaBitacora.Clear();
			lblEstado.Text = string.Format("Sistema reiniciado.");
			ActualizarUI();
		}
		
		private void ActualizarUI()
		{
			lstPedidos.Items.Clear();
			lstBitacora.Items.Clear();
			
			foreach (string p in colaPedidos)
				lstPedidos.Items.Add(p);
			if (colaPedidos.Count == 0)
				lstPedidos.Items.Add("(Sin pedidos pendientes)");
			
			foreach (string accion in pilaBitacora)
				lstBitacora.Items.Add(accion);
			if (pilaBitacora.Count == 0)
				lstBitacora.Items.Add("(Sin acciones registradas)");

			lblContador.Text = string.Format("Pedidos: {0} | Bitácora: {1}",
			                                 colaPedidos.Count, pilaBitacora.Count);
		}
		
		void BtnpremiunClick(object sender, EventArgs e)
		{
			string clientePremiun = txtCliente.Text.Trim();
			
			if (clientePremiun == "")
			{
				lblEstado.Text = string.Format("⚠️ Debe ingresar un nombre de cliente.");
				return;
			}
			
			string[] pedidosrecurrentes = colaPedidosPremium.ToArray();
			colaPedidos.Clear();
			colaPedidos.Enqueue(clientePremiun);
			
			foreach (string x in pedidosrecurrentes)
			{
				colaPedidos.Enqueue(x);
			}
			
			pilaBitacora.Push(string.Format("Premiun Pedido: {0}", clientePremiun));
			colaPedidosPremium.Enqueue(clientePremiun);
			txtCliente.Clear();
			lblEstado.Text = string.Format("✅ Pedido registrado para {0}", clientePremiun);
			ActualizarUI();
		}
	}
}