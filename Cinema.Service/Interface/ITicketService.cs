using Cinema.Domain.Domain_models;
using Cinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(int id);
        void CreateNewTicket(Ticket t);
        void UpdateExistingTicket(Ticket t);
        ShoppingCartDto GetShoppingCartInfo(int id);
        void DeleteTicket(int id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userId);
        List<Ticket> GetAllTicketsByDate(DateTime filterDate);
        List<Ticket> GetAllTicketsByGenre(string genre);
    }
}
