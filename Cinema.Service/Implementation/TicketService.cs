using Cinema.Domain.Domain_models;
using Cinema.Domain.DTO;
using Cinema.Repository.Interface;
using Cinema.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.Service.Implementation
{
    public class TicketService : ITicketService
    {
        public readonly IRepository<Ticket> _ticketRepository;
        public readonly IUserRepository _userRepository;
        public readonly IRepository<TicketsInShoppingCart> _ticketsInShoppingCartRepository;

        public TicketService(IRepository<Ticket> ticketRepository,
            IUserRepository userRepository, IRepository<TicketsInShoppingCart> ticketsInShoppingCartRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketsInShoppingCartRepository = ticketsInShoppingCartRepository;
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;

            if (userShoppingCart != null)
            {
                var ticket = this.GetDetailsForTicket(item.TicketId);

                if (ticket != null)
                {
                    var exists = userShoppingCart.TicketInShoppingCarts.Where(z => z.CartId == userShoppingCart.Id && z.TicketId == item.TicketId).FirstOrDefault();

                    if (exists != null)
                    {
                        exists.Quantity += item.Quantity;
                        _ticketsInShoppingCartRepository.Update(exists);

                    }
                    else
                    {
                        var itemToAdd = new TicketsInShoppingCart
                        {
                            ShoppingCart = userShoppingCart,
                            TicketId = ticket.Id,
                            CartId = userShoppingCart.Id,
                            Quantity = item.Quantity,
                            Ticket = ticket
                        };

                        _ticketsInShoppingCartRepository.Insert(itemToAdd);
                    }

                    return true;
                }
            }

            return false;
        }

        public void CreateNewTicket(Ticket t)
        {
            this._ticketRepository.Insert(t);
        }

        public void DeleteTicket(int id)
        {
            var ticket = _ticketRepository.Get(id);
            this._ticketRepository.Delete(ticket);
        }


        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public List<Ticket> GetAllTicketsByDate(DateTime filterDate)
        {
            return _ticketRepository.GetAll().Where(t => t.Date.Date.Equals(filterDate)).ToList();
        }

        public List<Ticket> GetAllTicketsByGenre(string genre)
        {
            return _ticketRepository.GetAll().Where(t => t.Genre.Equals(genre)).ToList();
        }

        public Ticket GetDetailsForTicket(int id)
        {
            return _ticketRepository.Get(id);
        }

        public ShoppingCartDto GetShoppingCartInfo(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateExistingTicket(Ticket t)
        {
            this._ticketRepository.Update(t);
        }

    }
}
