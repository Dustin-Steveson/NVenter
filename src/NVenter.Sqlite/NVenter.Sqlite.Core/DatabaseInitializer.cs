using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NVenter.Sqlite.Core {
    public class DatabaseInitializer {
        private readonly ConnectionFactory _factory;

        public DatabaseInitializer(ConnectionFactory factory) {
            _factory = factory;
        }

        public async Task InitializeDatabase() {
            var createTableSql =
    @"CREATE TABLE event(
        Id TEXT NOT NULL,
        StreamId TEXT NOT NULL,
        GlobalPosition INTEGER PRIMARY KEY AUTOINCREMENT,
        StreamPosition INTEGER,
        Data TEXT NOT NULL,
        MetaData TEXT NOT NULL,
        Created DATETIME NOT NULL,

        UNIQUE(StreamId, StreamPosition));

    CREATE INDEX idx_AggregateId ON event(StreamId);";

            using (var connection = _factory.NewConnection()) {
                connection.Open();
                await connection.ExecuteAsync(createTableSql);
            }
        }
    }

    public class ConnectionFactory {
        private readonly string _connectionInfo;

        public ConnectionFactory(string connectionInfo) {
            _connectionInfo = connectionInfo;
        }

        public SqliteConnection NewConnection() => new SqliteConnection(_connectionInfo);
    }
}
