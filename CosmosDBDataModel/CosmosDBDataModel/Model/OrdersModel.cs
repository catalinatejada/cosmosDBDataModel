using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBDataModel.Model
{
    public interface IDataObject
    {
        string id { get; set; }
        string partitionKey { get; }
        string type { get; }
        string etag { get; set; }
        int ttl { get; set; }
    }

    public partial class Pedido : IDataObject
    {
        public int IdSgaFisico { get; set; }

        public string CodigoPedido { get; set; } = null!;

        public string? CodigoSistemaOrigen { get; set; }

        public TipoPedido tipoPedido { get; set; }

        public short? IdEstadoPedido { get; set; }

        public int? IdCentroDistribucion { get; set; }

        public long? IdCicloReparto { get; set; }

        public short IdCadena { get; set; }

        public DateTime? FechaHoraCreacion { get; set; }

        public DateTime FechaAlta { get; set; }

        public string UsuarioAlta { get; set; } = null!;

        public DateTime? FechaModificacion { get; set; }

        public string? UsuarioModificacion { get; set; }

        public int? IdCampana { get; set; }

        public short? IdTemporada { get; set; }

        public int? IdTipoMovimiento { get; set; }

        public int? IdSubtipoMovimiento { get; set; }

        public string IdPedidoPadre { get; set; }

        public short IdProcedenciaPedido { get; set; }

        public string? CodigoReserva { get; set; }

        public DateTime? FechaHoraLiberacionReserva { get; set; }

        //public IEnumerable<LineaPedido> lineasPedido { get; set; } = new List<LineaPedido>();
        public string id { get; set; }

        public string partitionKey => "order";

        public string type => "order";

        public string etag { get; set; }
        public int ttl { get; set; } = -1;
    }

    public class TipoPedido
    {
        public short IdTipoPedido { get; set; }

        public string Descripcion { get; set; } = null!;
    }

    public class LineaPedido : IDataObject
    {
        public int IdSgaFisico { get; set; }

        public long IdLineaPedido { get; set; }

        public long IdPedido { get; set; }

        public short IdTipoLineaPedido { get; set; }

        public short IdEstadoLineaPedido { get; set; }

        public int Unidades { get; set; }

        public long? IdDestinoTiendaLineaPedido { get; set; }

        public long? CodOrdenTrabajo { get; set; }

        public int? CodOla { get; set; }

        public DateTime? FechaHoraCierreOlaReparto { get; set; }

        public DateTime FechaAlta { get; set; }

        public string UsuarioAlta { get; set; } = null!;

        public DateTime? FechaModificacion { get; set; }

        public string? UsuarioModificacion { get; set; }

        public int? NumeroLiberacion { get; set; }

        public decimal EsRecibidoMarcaAgotar { get; set; }

        public decimal EsSeleccionadoMarcaAgotar { get; set; }

        public decimal EsPrioritario { get; set; }
        public string id { get; set; }

        public string partitionKey => "orderline";

        public string type => "orderline";

        public string etag { get; set; }
        public int ttl { get; set; } = -1;
    }


}
