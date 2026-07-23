using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MiPrimeraSolucionJMKK.UI.Helpers
{
    /// <summary>
    /// Genera archivos PDF simples (texto tabulado en una fuente estandar)
    /// sin depender de librerias externas de terceros, para que los
    /// reportes de Contabilidad (CON-003, CON-006) puedan exportarse en
    /// .pdf sin requerir un paquete NuGet adicional que deba restaurarse.
    /// </summary>
    public class SimplePdfBuilder
    {
        private readonly List<string> _lineas = new List<string>();
        private readonly string _titulo;

        public SimplePdfBuilder(string titulo)
        {
            _titulo = titulo;
        }

        public void AgregarLinea(string texto)
        {
            _lineas.Add(texto ?? string.Empty);
        }

        public void AgregarSeparador()
        {
            _lineas.Add(new string('-', 90));
        }

        public byte[] Generar()
        {
            const int margenIzquierdo = 40;
            const int altoPagina = 792; // Carta (8.5x11 in) a 72 dpi
            const int anchoPagina = 612;
            const int alturaLinea = 14;
            int lineasPorPagina = (altoPagina - 100) / alturaLinea;

            var paginas = new List<string>();
            var contenidoPagina = new StringBuilder();

            int contadorLinea = 0;
            contenidoPagina.Append(ComandoTexto(_titulo, margenIzquierdo, altoPagina - 50, true));
            contenidoPagina.Append(ComandoTexto("Generado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), margenIzquierdo, altoPagina - 68, false));

            int y = altoPagina - 95;

            foreach (var linea in _lineas)
            {
                if (contadorLinea >= lineasPorPagina)
                {
                    paginas.Add(contenidoPagina.ToString());
                    contenidoPagina = new StringBuilder();
                    y = altoPagina - 50;
                    contadorLinea = 0;
                }

                contenidoPagina.Append(ComandoTexto(linea, margenIzquierdo, y, false));
                y -= alturaLinea;
                contadorLinea++;
            }

            paginas.Add(contenidoPagina.ToString());

            return ConstruirPdf(paginas, anchoPagina, altoPagina);
        }

        private string ComandoTexto(string texto, int x, int y, bool negrita)
        {
            string escapado = EscaparTexto(texto);
            string fuente = negrita ? "/F2 11 Tf" : "/F1 9 Tf";
            return "BT " + fuente + " " + x + " " + y + " Td (" + escapado + ") Tj ET\n";
        }

        private string EscaparTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            return texto
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private byte[] ConstruirPdf(List<string> paginasContenido, int ancho, int alto)
        {
            var objetos = new List<string>();

            // 1: Catalog
            objetos.Add("<< /Type /Catalog /Pages 2 0 R >>");

            // 2: Pages (se completa el Kids al final)
            int idPages = 2;
            int primerIdPagina = 3;
            int totalPaginas = paginasContenido.Count;
            int primerIdContenido = primerIdPagina + totalPaginas;
            int idFuenteRegular = primerIdContenido + totalPaginas;
            int idFuenteNegrita = idFuenteRegular + 1;

            var kids = new StringBuilder();
            for (int i = 0; i < totalPaginas; i++)
                kids.Append((primerIdPagina + i) + " 0 R ");

            objetos.Add("<< /Type /Pages /Kids [" + kids.ToString().Trim() + "] /Count " + totalPaginas + " >>");

            for (int i = 0; i < totalPaginas; i++)
            {
                objetos.Add(
                    "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 " + ancho + " " + alto + "] " +
                    "/Resources << /Font << /F1 " + idFuenteRegular + " 0 R /F2 " + idFuenteNegrita + " 0 R >> >> " +
                    "/Contents " + (primerIdContenido + i) + " 0 R >>");
            }

            for (int i = 0; i < totalPaginas; i++)
            {
                string flujo = paginasContenido[i];
                objetos.Add("<< /Length " + Encoding.ASCII.GetByteCount(flujo) + " >>\nstream\n" + flujo + "endstream");
            }

            objetos.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
            objetos.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>");

            using (var memoria = new MemoryStream())
            {
                var offsets = new List<int>();

                EscribirTexto(memoria, "%PDF-1.4\n");

                for (int i = 0; i < objetos.Count; i++)
                {
                    offsets.Add((int)memoria.Length);
                    EscribirTexto(memoria, (i + 1) + " 0 obj\n" + objetos[i] + "\nendobj\n");
                }

                int offsetXref = (int)memoria.Length;
                EscribirTexto(memoria, "xref\n0 " + (objetos.Count + 1) + "\n");
                EscribirTexto(memoria, "0000000000 65535 f \n");

                foreach (var offset in offsets)
                    EscribirTexto(memoria, offset.ToString("D10", CultureInfo.InvariantCulture) + " 00000 n \n");

                EscribirTexto(memoria, "trailer\n<< /Size " + (objetos.Count + 1) + " /Root 1 0 R >>\n");
                EscribirTexto(memoria, "startxref\n" + offsetXref + "\n%%EOF");

                return memoria.ToArray();
            }
        }

        private static void EscribirTexto(MemoryStream memoria, string texto)
        {
            var bytes = Encoding.ASCII.GetBytes(texto);
            memoria.Write(bytes, 0, bytes.Length);
        }
    }
}
