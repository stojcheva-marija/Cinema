using Cinema.Domain.Domain_models;
using Cinema.Domain.DTO;
using Cinema.Repository.Interface;
using Cinema.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Cinema.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        public readonly IRepository<ShoppingCart> _shoppingCartRepository;
        public readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        public readonly IRepository<Order> _orderRepository;
        public readonly IUserRepository _userRepository;
        public readonly IRepository<EmailMessage>_emailRepository;
        private readonly IRepository<TicketsInShoppingCart> _ticketsInShoppingCartRepository;

        public ShoppingCartService(IRepository<ShoppingCart> _shoppingCartRepositor,
            IRepository<TicketInOrder> _ticketInOrderRepository,
            IRepository<Order> _orderRepository, IUserRepository _userRepository,
            IRepository<TicketsInShoppingCart> _ticketsInShoppingCartRepository,
            IRepository<EmailMessage> _emailRepository)
        {
            this._shoppingCartRepository = _shoppingCartRepositor;
            this._userRepository = _userRepository;
            this._ticketInOrderRepository = _ticketInOrderRepository;
            this._orderRepository = _orderRepository;
            this._ticketsInShoppingCartRepository = _ticketsInShoppingCartRepository;
            this._emailRepository = _emailRepository;
        }
        public bool deleteProductFromShoppingCart(string userId, int ticketId)
        {
            if (!string.IsNullOrEmpty(userId) && ticketId != null)
            {
                var loggInUser = _userRepository.Get(userId);
                var userShoppingCart = loggInUser.UserShoppingCart;
                var itemToDelete = userShoppingCart.TicketInShoppingCarts.Where(z => z.TicketId.Equals(ticketId)).FirstOrDefault();
                userShoppingCart.TicketInShoppingCarts.Remove(itemToDelete);
                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userCart = loggedInUser.UserShoppingCart;

            var ticketsList = userCart.TicketInShoppingCarts.ToList();

            var ticketPrices = ticketsList.Select(z => new
            {
                TicketPrice = z.Ticket.Price,
                Quantity = z.Quantity
            }).ToList();

            double totalPrice = 0.0;

            foreach (var item in ticketPrices)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }

            var result = new ShoppingCartDto
            {
                TicketInShoppingCarts = ticketsList,
                TotalPrice = totalPrice
            };

            return result;
        }

        public bool OrderNow(string userId)
        {
                var loggedInUser = _userRepository.Get(userId);

                var userCard = loggedInUser.UserShoppingCart;

                EmailMessage message = new EmailMessage();

                message.MailTo = loggedInUser.Email;
                message.Subject = "Successfully created order";
                message.Status = false;

                Order order = new Order
                {
                    User = loggedInUser,
                    UserId = userId
                };

                _orderRepository.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userCard.TicketInShoppingCarts.Select(z => new TicketInOrder
                {
                    TicketId = z.Ticket.Id,
                    Ticket = z.Ticket,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                sb.Append("Your order is completed! The order contains: ");

                 var totalPrice = 0.0;

            for (int i=1; i<=result.Count(); i++)
            {
                var item = result[i - 1];
                totalPrice += item.Ticket.Price * item.Quantity;
                sb.AppendLine(i.ToString() + ". " + item.Ticket.MovieName + " with price of: " + item.Ticket.Price + " and quantity: " + item.Quantity);
            }

            sb.AppendLine("Total Price: " + totalPrice.ToString());

            message.Content = sb.ToString();

                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                _ticketInOrderRepository.Insert(item);
                }

                loggedInUser.UserShoppingCart.TicketInShoppingCarts.Clear();

                _userRepository.Update(loggedInUser);

                _emailRepository.Insert(message);

                return true;

            }
        }
    }
