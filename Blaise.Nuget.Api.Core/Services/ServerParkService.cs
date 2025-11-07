namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;

    public class ServerParkService : IServerParkService
    {
        private readonly IConnectedServerFactory _connectionFactory;

        public ServerParkService(IConnectedServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            var serverParks = GetServerParks(connectionModel);

            return serverParks.Select(sp => sp.Name);
        }

        /// <inheritdoc/>
        public bool ServerParkExists(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParkNames = GetServerParkNames(connectionModel);

            return serverParkNames.Any(sp => sp.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <inheritdoc/>
        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParks = GetServerParks(connectionModel);
            var serverPark = serverParks.FirstOrDefault(sp => sp.Name.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));

            if (serverPark == null)
            {
                throw new DataNotFoundException($"Server park '{serverParkName}' not found");
            }

            return serverPark;
        }

        private static readonly ObjectCache _connectionCache = MemoryCache.Default;
        private static readonly object _connectionCacheLock = new object();

        private IConnectedServer GetCachedConnection(ConnectionModel connectionModel)
        {
            string cacheKey = $"Connection_{connectionModel.ServerName}";
            string backupKey = $"{cacheKey}_Backup";
            int expiresInMinutes = connectionModel.ConnectionExpiresInMinutes > 0 ? connectionModel.ConnectionExpiresInMinutes : 5;

            var cachedConnection = _connectionCache.Get(cacheKey) as IConnectedServer;
            if (IsConnectionValid(cachedConnection))
            {
                return cachedConnection;
            }

            lock (_connectionCacheLock)
            {
                cachedConnection = _connectionCache.Get(cacheKey) as IConnectedServer;
                if (IsConnectionValid(cachedConnection))
                {
                    return cachedConnection;
                }

                IConnectedServer freshConnection = null;
                try
                {
                    freshConnection = _connectionFactory.GetConnection(connectionModel);
                }
                catch (Exception ex)
                {
                }

                if (IsConnectionValid(freshConnection))
                {
                    _connectionCache.Set(cacheKey, freshConnection, DateTimeOffset.Now.AddMinutes(expiresInMinutes));
                    _connectionCache.Set(backupKey, freshConnection, ObjectCache.InfiniteAbsoluteExpiration);
                    return freshConnection;
                }

                var backupConnection = _connectionCache.Get(backupKey) as IConnectedServer;
                if (IsConnectionValid(backupConnection))
                {
                    _connectionCache.Set(cacheKey, backupConnection, DateTimeOffset.Now.AddMinutes(expiresInMinutes));
                    return backupConnection;
                }

                throw new DataNotFoundException("Unable to establish a valid connection to the Blaise server.");
            }
        }

        private bool IsConnectionValid(IConnectedServer connection)
        {
            if (connection == null)
            {
                return false;
            }

            try
            {
                var _ = connection.ServerParks?.Count;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <inheritdoc/>
        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            var connection = GetCachedConnection(connectionModel);

            var serverParks = GetServerParks(connection);

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks;
        }

        private static readonly ObjectCache _cache = MemoryCache.Default;
        private static readonly object _cacheLock = new object();

        public static List<IServerPark> GetServerParks(IConnectedServer connection)
        {
            string cacheKey = $"ServerParks";
            string backupKey = $"{cacheKey}_Backup";

            var cachedServerParks = _cache.Get(cacheKey) as List<IServerPark>;
            if (cachedServerParks != null && cachedServerParks.Count > 0)
            {
                return cachedServerParks;
            }

            lock (_cacheLock)
            {
                cachedServerParks = _cache.Get(cacheKey) as List<IServerPark>;
                if (cachedServerParks != null && cachedServerParks.Count > 0)
                {
                    return cachedServerParks;
                }

                List<IServerPark> freshServerParks = null;
                try
                {
                    freshServerParks = connection.ServerParks?.ToList();
                }
                catch (Exception ex)
                {
                }

                if (freshServerParks != null && freshServerParks.Count > 0)
                {
                    _cache.Set(cacheKey, freshServerParks, DateTimeOffset.Now.AddMinutes(5));
                    _cache.Set(backupKey, freshServerParks, ObjectCache.InfiniteAbsoluteExpiration);
                    return freshServerParks;
                }

                var backupServerParks = _cache.Get(backupKey) as List<IServerPark>;
                if (backupServerParks != null && backupServerParks.Count > 0)
                {
                    _cache.Set(cacheKey, backupServerParks, DateTimeOffset.Now.AddMinutes(5));
                    return backupServerParks;
                }

                return new List<IServerPark>();
            }
        }

        public ISurveyCollection GetSurveys(ConnectionModel connectionModel, string serverParkName)
        {
            var connection = GetCachedConnection(connectionModel);
            var surveys = connection.GetSurveys(serverParkName);

            if (!surveys.Any())
            {
                throw new DataNotFoundException("No surveys found");
            }

            return surveys;
        }
    }
}
