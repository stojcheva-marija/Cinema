using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Domain.Domain_models
{
    public class Ticket : BaseEntity
    {

        [Required]
        public string MovieName { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{hh:mm}")]
        [DataType(DataType.Time)]
        public string Time { get; set; }

        [Required]
        public string Genre { get; set; }

        public virtual ICollection<TicketsInShoppingCart> TicketsInShoppingCarts { get; set; }
       // public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
