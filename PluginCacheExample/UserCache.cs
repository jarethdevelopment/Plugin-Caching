using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCacheExample
{
    public class UserCache
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static UserCache instance;

        // Using concurrent as multiple instances of plugin could be running
        private ConcurrentDictionary<Guid, Entity> users;

        /// <summary>
        /// Cache constructor
        /// </summary>
        private UserCache()
        {
            users = new ConcurrentDictionary<Guid, Entity>();
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static UserCache Instance
        {
            get
            {
                //If not initialised, set instance
                if (instance == null)
                {
                    instance = new UserCache();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets a user from the cache or CRM
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="service">Organisation service</param>
        /// <returns></returns>
        public Entity GetUser(Guid id, IOrganizationService service)
        {
            // Check for user in cache
            if (users.ContainsKey(id))
            {
                return users[id];
            }

            // Get user from CRM
            var user = service.Retrieve("systemuser", id, new ColumnSet("domainname"));

            // Add to dictionary
            users.TryAdd(id, user);

            return user;
        }

    }
}
