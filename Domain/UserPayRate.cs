using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserPayRate
    {
        public virtual string Id { get { return "userpayrates/" + UserId + "/" + WorkContextId; } }

        public virtual Guid Etag { get; set; }

        public string UserId { get; set; }

        public string WorkContextId { get; set; }

        public string RateRule { get; set; }

        public DateTime EffectiveDate { get; set; }

        public Decimal PayRate { get; set; }
    }


}
