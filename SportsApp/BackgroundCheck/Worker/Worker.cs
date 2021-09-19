using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsApp.BackgroundCheck.Worker
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> logger;
        private readonly ApplicationDbContext _context;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>(); ;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (DateTime.Now.ToLocalTime().Hour == 23)
                {
                    List<Schedule> schedulesToday = _context.Schedules.Where(x => x.StartTime.Date == DateTime.Today.Date).ToList();
                    List<Team> teamsToday = new List<Team>();
                    List<Matches> matchesToday = new List<Matches>();
                    List<GameRoom> GameRoomToday = new List<GameRoom>();

                    foreach (Schedule s in schedulesToday)
                    {
                        var match = _context.Matches.FirstOrDefault(x => x.Id == s.MatchesId);
                        if (match != null)
                        {
                            matchesToday.Add(match);
                        }
                    }
                    foreach (Matches m in matchesToday)
                    {
                        var tToday = _context.Teams.Where(x => x.MatchId == m.Id.ToString()).ToList();
                        if (tToday.Count != 0)
                        {
                            foreach (Team xs in tToday)
                            {
                                teamsToday.Add(xs);
                                var gameroom = _context.GameRooms.FirstOrDefault(x => x.Id == xs.GameRoomId);
                                if (gameroom != null)
                                {
                                    if (!GameRoomToday.Contains(gameroom))
                                    {
                                        GameRoomToday.Add(gameroom);
                                    }
                                }
                            }
                        }
                    }

                    foreach (Team t in teamsToday)
                    {
                        t.MatchId = null;
                        t.GameRoomId = null;
                        _context.Teams.Update(t);
                    }
                    await _context.SaveChangesAsync();

                    foreach (Schedule s in schedulesToday)
                    {
                        _context.Schedules.Remove(s);
                    }
                    await _context.SaveChangesAsync();

                    foreach (GameRoom g in GameRoomToday)
                    {
                        _context.GameRooms.Remove(g);
                    }
                    await _context.SaveChangesAsync();

                    foreach (Matches m in matchesToday)
                    {
                        _context.Matches.Remove(m);
                    }
                    await _context.SaveChangesAsync();

                }
                await Task.Delay(1000 * 1800);
            }
        }
    }
}
