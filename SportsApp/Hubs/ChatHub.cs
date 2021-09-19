using AutoMapper;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SportsApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        public readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ChatHub(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SendPrivate(string receiverName, string message)
        {
            if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
            {
                // Who is the sender;
                var sender = _Connections.Where(u => u.Username == IdentityName).First();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Build the message
                    var messageViewModel = new MessageViewModel()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        From = sender.FullName,
                        Avatar = sender.Avatar,
                        To = "",
                        Timestamp = DateTime.Now.ToLongTimeString()
                    };

                    // Send the message
                    await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }

        public async Task SendToRoom(string roomName, string message)
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
                var room = _context.GameRoomChats.Where(r => r.ChatName == roomName).FirstOrDefault();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Create and save message in database
                    var msg = new Message()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        FromUser = user,
                        ToRoomChat = room,
                        Timestamp = DateTime.Now
                    };
                    _context.Messages.Add(msg);
                    _context.SaveChanges();

                    // Broadcast the message
                    var messageViewModel = _mapper.Map<Message, MessageViewModel>(msg);
                    await Clients.Group(roomName).SendAsync("newMessage", messageViewModel);
                }
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Message not send! Message should be 1-500 characters.");
            }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = _Connections.Where(u => u.Username == IdentityName).FirstOrDefault();
                if (user != null && user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    // Join to new chat room
                    await Leave(user.CurrentRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task CreateRooms(string rooomName,string team1Id,string team2Id,string roomId)
        {
            var team1 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team1Id));
            var team2 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team2Id));
            var teamleader1 = await _context.Users.FirstOrDefaultAsync(x => x.Id == team1.TeamLeaderId.ToString());
            var teamleader2 = await _context.Users.FirstOrDefaultAsync(x => x.Id == team2.TeamLeaderId.ToString());
            var rooms3 = new List<RoomViewModel>();

            try
            {
                // Create and save chat room in database
                var roomteam1 = new GameRoomChat()
                {
                    Id = Guid.NewGuid(),
                    ChatName = team1.Name,
                    Admin = teamleader1,
                    GameRoomId = Guid.Parse(roomId)
                };
                var roomteam2 = new GameRoomChat()
                {
                    Id = Guid.NewGuid(),
                    ChatName = team2.Name,
                    Admin = teamleader2,
                    GameRoomId = Guid.Parse(roomId)
                };
                var roomteamVs = new GameRoomChat()
                {
                    Id = Guid.NewGuid(),
                    ChatName = team1.Name+" vs " +team2.Name,
                    Admin = teamleader2,
                    GameRoomId = Guid.Parse(roomId)
                };
                var roomexists = await _context.GameRoomChats.Where(x => x.GameRoomId == roomteamVs.GameRoomId).ToListAsync();
                if (roomexists.Count == 0)
                {
                    _context.GameRoomChats.Add(roomteam1);
                    _context.GameRoomChats.Add(roomteam2);
                    _context.GameRoomChats.Add(roomteamVs);

                    _context.SaveChanges();

                    if (roomteam1 != null && roomteam2 != null && roomteamVs != null)
                    {
                        // Update room list
                        var roomteam1ViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(roomteam1);
                        var roomteam2ViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(roomteam2);
                        var roomteamVsViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(roomteamVs);
                        _Rooms.Add(roomteam1ViewModel);
                        _Rooms.Add(roomteam2ViewModel);
                        _Rooms.Add(roomteamVsViewModel);

                        rooms3.Add(roomteam1ViewModel);
                        rooms3.Add(roomteam2ViewModel);
                        rooms3.Add(roomteamVsViewModel);
                        foreach (RoomViewModel r in rooms3)
                        {
                            await Clients.All.SendAsync("addChatRoom", r);
                        }
                    }
                }
                // Accept: Letters, numbers and one space between words.
                /*Match match = Regex.Match(roomName, @"^\w+( \w+)*$");
                if (!match.Success)
                {
                    await Clients.Caller.SendAsync("onError", "Invalid room name!\nRoom name must contain only letters and numbers.");
                }
                else if (roomName.Length < 5 || roomName.Length > 100)
                {
                    await Clients.Caller.SendAsync("onError", "Room name must be between 5-100 characters!");
                }
                else if (_context.GameRoomChats.Any(r => r.ChatName == roomName))
                {
                    await Clients.Caller.SendAsync("onError", "Another chat room with this name exists");
                }
                else
                {}*/

            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Couldn't create chat room: " + ex.Message);
            }
        }

        public async Task DeleteRoom(string roomName)
        {
            try
            {
                // Delete from database
                var room = _context.GameRoomChats.Include(r => r.Admin)
                    .Where(r => r.ChatName == roomName && r.Admin.UserName == IdentityName).FirstOrDefault();
                _context.GameRoomChats.Remove(room);
                _context.SaveChanges();

                // Delete from list
                var roomViewModel = _Rooms.First(r => r.Name == roomName);
                _Rooms.Remove(roomViewModel);

                // Move users back to Lobby
                await Clients.Group(roomName).SendAsync("onRoomDeleted", string.Format("Room {0} has been deleted.\nYou are now moved to the Lobby!", roomName));

                // Tell all users to update their room list
                await Clients.All.SendAsync("removeChatRoom", roomViewModel);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Can't delete this chat room. Only owner can delete this room.");
            }
        }

        /*public IEnumerable<RoomViewModel> GetRooms()
        {
            // First run?
            if (_Rooms.Count == 0)
            {
                foreach (var room in _context.GameRoomChats)
                {
                    var roomViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(room);
                    _Rooms.Add(roomViewModel);
                }
            }

            return _Rooms.ToList();
        }*/
        public async Task<IEnumerable<RoomViewModel>> GetRooms()
        {
            var TeamName="";
            var gameroomchats = new List<RoomViewModel>();
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == IdentityName);
            var Team = await _context.Teams.FirstOrDefaultAsync(x=>x.Id == currentUser.TeamId);
            if (Team != null)
            {
                TeamName = Team.Name;
            }
            var GameRoom = await _context.GameRooms.FirstOrDefaultAsync(x => x.Teams.FirstOrDefault(x => x.Id == Team.Id).Id == Team.Id); 
            if (gameroomchats.Count == 0) {
                GameRoomChat roomVs = await _context.GameRoomChats.SingleOrDefaultAsync(x =>x.GameRoomId==GameRoom.Id && x.ChatName.Contains(" vs "));
                var roomViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(roomVs);
                _Rooms.Add(roomViewModel);
                GameRoomChat teamRoom = await _context.GameRoomChats.SingleOrDefaultAsync(x => x.ChatName == TeamName);
                var teamRoomViewModel = _mapper.Map<GameRoomChat, RoomViewModel>(teamRoom);
                _Rooms.Add(teamRoomViewModel);

                gameroomchats.Add(roomViewModel);
                gameroomchats.Add(teamRoomViewModel);
            }
            return gameroomchats.ToList();
        }
        public IEnumerable<UserViewModel> GetUsers(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }
        public IEnumerable<UserViewModel> GetUsersTeams(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName && u.Team == roomName).ToList();
        }
        public IEnumerable<MessageViewModel> GetMessageHistory(string roomName)
        {
            var messageHistory = _context.Messages.Where(m => m.ToRoomChat.ChatName == roomName)
                    .Include(m => m.FromUser)
                    .Include(m => m.ToRoomChat)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(20)
                    .AsEnumerable()
                    .Reverse()
                    .ToList();

            return _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messageHistory);
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
                var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);
                userViewModel.Device = GetDevice();
                userViewModel.CurrentRoom = "";

                if (!_Connections.Any(u => u.Username == IdentityName))
                {
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getProfileInfo", user.Name+" "+user.Surname, user.ProfileImg);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _Connections.Where(u => u.Username == IdentityName).First();
                _Connections.Remove(user);

                // Tell other users to remove you from their list
                Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                // Remove mapping
                _ConnectionsMap.Remove(user.Username);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
    }
}
