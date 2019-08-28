using System;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net.ConnectionPool;
using Nest;
using Clarifi.RoomMappingLogger.MySql;
using System.Linq;
using System.Collections.Generic;

namespace Clarifi.RoomMappingLogger.ElasticSearch
{
    public class ElasticSearchProvider
    {
        private ConnectionSettings _settings;
        public readonly ElasticClient client;

        public ElasticSearchProvider(string index, List<Uri> urls)
        {
            var connectionPool = new SniffingConnectionPool(urls);
            _settings = new ConnectionSettings(connectionPool).SetPingTimeout(5000).SetDefaultIndex(index).PluralizeTypeNames();
            client = new ElasticClient(_settings);
            var isIndexExist = client.IndexExists(i => i.Index(index));
            if (!isIndexExist.Exists)
            {
                client.CreateIndex(c => c.Index(index));
                _settings.SetDefaultIndex(index);
                client = new ElasticClient(_settings);
            }
            var response =
                client.Map<Hotel>(
                    h =>
                        h.Properties(p => p
                            .Object<PrimaryInfo>(
                                n =>
                                    n.Name(a => a.PrimaryContent).Properties(x => x.GeoPoint(g => g.Name(field => field.GeoPoint).IndexLatLon())))
                            .String(s => s.Name(i => i.SupplierId).Index(FieldIndexOption.NotAnalyzed)))
                        .RoutingField(hotel => hotel.Path(p => p.SupplierFamily).Required(true)));
        }

        public Hotel GetHotelBySupplierIdFamily(string supplierId, string supplierfamily)
        {
            if (string.IsNullOrEmpty(supplierId) || string.IsNullOrEmpty(supplierfamily))
                throw new ArgumentException("supplier hotel id and supplier family cannot be null/empty.", "supplierId");

            var result = client.Search<Hotel>(s => s.From(0)
                .Size(10)
                .Routing(supplierfamily)
                .Query(Q => Q.Bool(b =>
                    b.Must(
                        m => m.Match(a => a.OnField(x => x.SupplierId).Query(supplierId).Operator(Operator.And))
                        , m => m.Match(a => a.OnField(x => x.SupplierFamily).Query(supplierfamily))
                        ))));

            if (result != null && result.Documents != null)
            {
                var hotel = result.Documents.ToList().Find(x => string.Equals(x.SupplierId, supplierId, StringComparison.OrdinalIgnoreCase));
                return hotel;
            }

            return null;
        }

        public Hotel GetHotelByClarifiIdSupplierFamily(string clarifiId, string supplierfamily)
        {
            if (string.IsNullOrEmpty(clarifiId) || string.IsNullOrEmpty(supplierfamily))
                throw new ArgumentException("supplier hotel id and supplier family cannot be null/empty.", "supplierId");

            var result = client.Search<Hotel>(s => s.From(0)
                .Size(1)
                .Routing(supplierfamily)
                .SortAscending(f => f.PrimaryContent.Name.Name)
                .Query(Q => Q.Bool(b =>
                    b.Must(
                        m => m.Match(a => a.OnField("clarifiId").Query(clarifiId).Operator(Operator.And))
                        , m => m.Match(a => a.OnField("supplierFamily").Query(supplierfamily))

                        ))));

            var hotel = result.Documents.SingleOrDefault();
            return (hotel != null && string.Equals(hotel.ClarifiId, clarifiId, StringComparison.OrdinalIgnoreCase))
                ? hotel
                : null;
        }
    }
}
