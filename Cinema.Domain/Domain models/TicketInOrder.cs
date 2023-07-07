using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Domain.Domain_models
{
    public class TicketInOrder : BaseEntity
    {
        [ForeignKey("TicketId")]
        public int TicketId { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public Ticket Ticket { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
