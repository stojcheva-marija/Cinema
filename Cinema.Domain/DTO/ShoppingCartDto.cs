using Cinema.Domain.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Domain.DTO
{ 
    public class ShoppingCartDto
    {
        public List<TicketsInShoppingCart> TicketInShoppingCarts { get; set; }
        public double TotalPrice { get; set; }
    }
}
