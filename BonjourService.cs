using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeroconf;

namespace bonjour_browser
{
    class BonjourService
    {

        public delegate void UpdateServiceListCallback(IReadOnlyList<IZeroconfHost> results);

        public async Task ListAll(UpdateServiceListCallback callback)
        {
            IReadOnlyList<IZeroconfHost> responses = new List<IZeroconfHost>();
            try
            {
                ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync();
                responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
            }
            finally
            {
                callback(responses);
            }
        }
    }
}
