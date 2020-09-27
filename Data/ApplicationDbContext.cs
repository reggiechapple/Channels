using System;
using System.Threading;
using System.Threading.Tasks;
using Channels.Data.Entities;
using Channels.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Channels.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public string CurrentUserId { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelSubscriber> ChannelSubscribers { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Cause> Causes { get; set; }
        public DbSet<CampaignVolunteer> CampaignVolunteers { get; set; }
        public DbSet<CauseSupporter> CauseSupporters { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<VolunteerRequest> VolunteerRequests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingAttendee> MeetingAttendees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ChannelSubscriber>(chanMem =>
            {
                chanMem.HasKey(cm => new { cm.SubscriberId, cm.ChannelId });

                chanMem.HasOne(cm => cm.Subscriber)
                    .WithMany(m => m.Subscriptions)
                    .HasForeignKey(cm => cm.SubscriberId)
                    .IsRequired();

                chanMem.HasOne(cm => cm.Channel)
                    .WithMany(c => c.Subscribers)
                    .HasForeignKey(cm => cm.ChannelId)
                    .IsRequired();
            });

            builder.Entity<UserNotification>(notif =>
            {
                notif.HasKey(un => new { un.MemberId, un.NotificationId });

                notif.HasOne(un => un.Member)
                    .WithMany(m => m.Notifications)
                    .HasForeignKey(un => un.MemberId)
                    .IsRequired();

                notif.HasOne(un => un.Notification)
                    .WithMany(n => n.Users)
                    .HasForeignKey(un => un.NotificationId)
                    .IsRequired();
            });

            builder.Entity<CampaignVolunteer>(cv =>
            {
                cv.HasKey(v => new { v.VolunteerId, v.CampaignId });

                cv.HasOne(v => v.Campaign)
                    .WithMany(c => c.Volunteers)
                    .HasForeignKey(v => v.CampaignId)
                    .IsRequired();

                cv.HasOne(v => v.Volunteer)
                    .WithMany(m => m.Work)
                    .HasForeignKey(v => v.VolunteerId)
                    .IsRequired();
            });

            builder.Entity<CauseSupporter>(cs =>
            {
                cs.HasKey(s => new { s.SupporterId, s.CauseId });

                cs.HasOne(s => s.Cause)
                    .WithMany(c => c.Supporters)
                    .HasForeignKey(s => s.CauseId)
                    .IsRequired();

                cs.HasOne(s => s.Supporter)
                    .WithMany(m => m.Causes)
                    .HasForeignKey(s => s.SupporterId)
                    .IsRequired();
            });

            builder.Entity<Follow>(follow =>
            {
                follow.HasKey(f => new { f.FollowedId, f.FollowerId });

                follow.HasOne(f => f.Followed)
                    .WithMany(f => f.Follows)
                    .HasForeignKey(f => f.FollowedId)
                    .IsRequired();

                follow.HasOne(f => f.Follower)
                    .WithMany(f => f.Following)
                    .HasForeignKey(f => f.FollowerId)
                    .IsRequired();
            });

             builder.Entity<MeetingAttendee>(ma =>
            {
                ma.HasKey(m => new { m.AttendeeId, m.MeetingId });

                ma.HasOne(m => m.Meeting)
                    .WithMany(m => m.MeetingAttendees)
                    .HasForeignKey(m => m.MeetingId)
                    .IsRequired();

                ma.HasOne(m => m.Attendee)
                    .WithMany(m => m.MeetingAttendance)
                    .HasForeignKey(m => m.AttendeeId)
                    .IsRequired();
            });
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.Entity is Entity)
                {
                    var entity = changedEntity.Entity as Entity;
                    if (changedEntity.State == EntityState.Added)
                    {
                        entity.Created = DateTime.Now;
                        entity.Updated = DateTime.Now;
                        
                    }
                    else if (changedEntity.State == EntityState.Modified)
                    {
                        entity.Updated = DateTime.Now;
                    }
                }

            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.Entity is Entity)
                {
                    var entity = changedEntity.Entity as Entity;
                    if (changedEntity.State == EntityState.Added)
                    {
                        entity.Created = DateTime.Now;
                        entity.Updated = DateTime.Now;
                        
                    }
                    else if (changedEntity.State == EntityState.Modified)
                    {
                        entity.Updated = DateTime.Now;
                    }
                }
            }
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
        
    }
}