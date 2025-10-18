using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public sealed class Role : AuditableEntity
    {
        public string Name { get; private set; } = default!; // Example: "Citizen", "Police", "Admin"

        private Role() { }  // EF Core needs default constructor

        public Role(string name)
        {
            Name = name;
            SetCreated("system"); // temp — later replace with logged-in user
        }

        public void Rename(string newName)
        {
            Name = newName;
            SetUpdated("system");
        }
    }
}
