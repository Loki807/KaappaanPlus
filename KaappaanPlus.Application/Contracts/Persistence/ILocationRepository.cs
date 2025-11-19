using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface ILocationRepository
    {
        Task SaveResponderLocationAsync(Guid responderId, double lat, double lng);
        Task SaveCitizenLocationAsync(Guid citizenId, double lat, double lng);
    }

}
