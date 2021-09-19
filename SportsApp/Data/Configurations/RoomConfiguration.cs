using SportsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<GameRoomChat>
    {
        public void Configure(EntityTypeBuilder<GameRoomChat> builder)
        {
            builder.ToTable("GameRoomChats");

            builder.Property(s => s.ChatName).IsRequired().HasMaxLength(100);

            builder.HasOne(s => s.Admin)
                .WithMany(u => u.Room)
                .IsRequired();
        }
    }
}
