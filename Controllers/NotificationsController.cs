using System.Linq;
using Channels.Data;
using Channels.Data.Identity;
using Channels.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Channels.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(INotificationRepository notificationRepository,
                                        UserManager<ApplicationUser> userManager,
                                        ApplicationDbContext context)
        {
            _context = context;
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public IActionResult GetNotification()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var member = _context.Members.FirstOrDefault(m => m.IdentityId == userId);
            var notification = _notificationRepository.GetUserNotifications(member.Id);
            return Ok(new{UserNotification = notification, Count = notification.Count});
        }

        public IActionResult ReadNotification(long notificationId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var member = _context.Members.FirstOrDefault(m => m.IdentityId == userId);

            _notificationRepository.ReadNotification(notificationId, member.Id);

            return Ok();
        }
    }
}