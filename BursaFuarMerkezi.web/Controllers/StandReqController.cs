using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]
    public class StandReqController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly EmailTemplatesConfig _emailTemplatesConfig;
        private readonly ILogger<StandReqController> _logger;
        private readonly EmailSettings _emailSettings;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public StandReqController(IUnitOfWork unitOfWork, IEmailSender emailSender, IOptions<EmailTemplatesConfig> emailTemplatesConfig, ILogger<StandReqController> logger, IOptions<EmailSettings> emailSettings)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _emailTemplatesConfig = emailTemplatesConfig.Value;
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        [HttpGet("stand-talebi")]
        [HttpGet("stand-request")]
        public IActionResult Index()
        {
            var fuarList = _unitOfWork.FuarPages.GetAll().Select(u => new SelectListItem
            {
                Text = Lang == "tr" ? u.TitleTr : u.TitleEn,
                Value = Lang == "tr" ? u.TitleTr : u.TitleEn
            });

            var standRequestVM = new StandRequestVM
            {
                FuarList = fuarList
            };

            return View(standRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost("stand-talebi")]
        [HttpPost("stand-request")]
        public async Task<IActionResult> Index(StandRequestVM standRequestVM)
        {
            var template = _emailTemplatesConfig.GetStandRequestTemplate(Lang);
            if (template == null)
            {
                TempData["error"] = "Email template not found.";
                return View(standRequestVM);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var standRequest = new StandRequest
                    {
                        Exhibition = standRequestVM.Exhibition,
                        Company = standRequestVM.Company,
                        Name = standRequestVM.Name,
                        Surname = standRequestVM.Surname,
                        Email = standRequestVM.Email,
                        Phone = standRequestVM.Phone,
                        Message = standRequestVM.Message,
                        CreatedAt = DateTime.Now,
                        IsEmailSent = false
                    };

                    _unitOfWork.StandRequest.Add(standRequest);
                    _unitOfWork.Save();

                    try
                    {
                        // Admin notification email
                        var adminEmailBody = template.EmailBody
                            .Replace("{exhibition}", standRequest.Exhibition)
                            .Replace("{company}", standRequest.Company)
                            .Replace("{name}", standRequest.Name)
                            .Replace("{surname}", standRequest.Surname)
                            .Replace("{email}", standRequest.Email)
                            .Replace("{phone}", standRequest.Phone)
                            .Replace("{message}", standRequest.Message);

                        await _emailSender.SendEmailAsync(_emailSettings.AdminEmail, template.FormatSubject(template.AdminSubject), adminEmailBody);

                        // User confirmation email
                        var userEmailBody = $"{template.FormatGreeting(standRequest.Name)}<br/><br/>{template.ConfirmationText}<br/><br/>{template.ConfirmationFooter}";
                        await _emailSender.SendEmailAsync(standRequest.Email, template.ConfirmationSubject, userEmailBody);

                        standRequest.IsEmailSent = true;
                        _unitOfWork.StandRequest.Update(standRequest);
                        _unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send emails for stand request {id}", standRequest.Id);
                    }

                    TempData["success"] = template.SuccessMessage;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving stand request.");
                    TempData["error"] = template.ErrorMessage;
                }
            }
            else
            {
                TempData["error"] = template.ValidationMessage;
            }
            var fuarList = _unitOfWork.FuarPages.GetAll().Select(u => new SelectListItem
            {
                Text = Lang == "tr" ? u.TitleTr : u.TitleEn,
                Value = Lang == "tr" ? u.TitleTr : u.TitleEn
            });
            standRequestVM.FuarList = fuarList;
            return View(standRequestVM);
        }
    }
}