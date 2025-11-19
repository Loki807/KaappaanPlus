using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public enum ServiceType
    {
        Police,             // Crime / harassment alerts
        Fire,               // Fire emergencies
        Ambulance,          // Medical / accident alerts
        DisasterManagement, // Natural disasters, landslides, floods
        CoastGuard,         // Beach & sea emergencies

        University          // University student emergencies handled by staff
    }

}
