using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang:regex(^tr|en$)}")]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly EmailTemplatesConfig _emailTemplatesConfig;
        private readonly ILogger<ContactController> _logger;
        private readonly EmailSettings _emailSettings;
        protected string Lang => RouteData.Values["lang"]?.ToString() ?? "tr";

        public ContactController(
            IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            IOptions<EmailTemplatesConfig> emailTemplatesConfig,
            ILogger<ContactController> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _emailTemplatesConfig = emailTemplatesConfig.Value;
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        [HttpGet("iletisim")]
        [HttpGet("contact")]
        public IActionResult Index()
        {
            return View(new ContactVM());
        }

        [HttpPost("iletisim")]
        [HttpPost("contact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactVM contactVM)
        {
            var template = _emailTemplatesConfig.GetContactTemplate(Lang);
            if (template == null)
            {
                TempData["error"] = "Email template could not be found.";
                return View(contactVM);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contact = new Contact
                    {
                        Name = contactVM.Name,
                        Email = contactVM.Email,
                        Phone = contactVM.Phone,
                        Message = contactVM.Message,
                        CreatedAt = DateTime.Now,
                        IsEmailSent = false
                    };

                    _unitOfWork.Contact.Add(contact);
                    _unitOfWork.Save();

                    try
                    {
                        var adminEmailBody = template.EmailBody
                            .Replace("{name}", contact.Name)
                            .Replace("{email}", contact.Email)
                            .Replace("{phone}", contact.Phone)
                            .Replace("{message}", contact.Message);

                        await _emailSender.SendEmailAsync(_emailSettings.AdminEmail, template.FormatSubject(template.AdminSubject), adminEmailBody);

                        var userEmailBody = $"{template.FormatGreeting(contact.Name)}<br/><br/>{template.ConfirmationText}<br/><br/>{template.ConfirmationFooter}";
                        await _emailSender.SendEmailAsync(contact.Email, template.ConfirmationSubject, userEmailBody);

                        contact.IsEmailSent = true;
                        _unitOfWork.Contact.Update(contact);
                        _unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send contact form emails for entry {id}", contact.Id);
                    }

                    TempData["success"] = template.SuccessMessage;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving contact form entry.");
                    TempData["error"] = template.ErrorMessage;
                }
            }
            else
            {
                TempData["error"] = template.ValidationMessage;
            }

            return View(contactVM);
        }
    }
}
