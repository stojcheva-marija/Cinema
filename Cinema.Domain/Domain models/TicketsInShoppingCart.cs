using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Domain.Domain_models
{
    public class TicketsInShoppingCart : BaseEntity
    {
        public int TicketId { get; set; }
        public int CartId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
        [ForeignKey("CartId")]
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
