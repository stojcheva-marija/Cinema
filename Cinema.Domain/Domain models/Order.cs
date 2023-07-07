using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Domain.Domain_models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ShopApplicationUser User { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
