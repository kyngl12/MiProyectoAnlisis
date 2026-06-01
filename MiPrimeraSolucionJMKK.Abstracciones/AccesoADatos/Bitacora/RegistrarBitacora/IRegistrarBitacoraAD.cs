using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.RegistrarBitacora
{
	public interface IRegistrarBitacoraAD
	{
		void RegistrarEvento(BitacoraEventoDto evento);
	}
}