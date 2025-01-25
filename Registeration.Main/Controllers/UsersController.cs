using Microsoft.AspNetCore.Mvc;
using Registeration.Main.Application.Services;
using Registeration.Main.Domain.Dtos;

namespace Registeration.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, IVerificationService verificationService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IVerificationService _verificationService = verificationService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            return Ok(await _userService.RegisterAsync(dto));
        }

        [HttpPost("{icNumber}/login")]
        public async Task<IActionResult> Login(int icNumber)
        {
            return Ok(await _userService.LoginAsync(icNumber));
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerifyCodeDto dto)
        {
            return Ok(await _verificationService.VerifyCodeAsync(dto));
        }

        [HttpPost("resend-code")]
        public async Task<IActionResult> ResendCode(ResendCodeDto dto)
        {
            return Ok(await _verificationService.ResendCodeAsync(dto));
        }

        [HttpPatch("{icNumber}/accept-policy")]
        public async Task<IActionResult> AcceptPolicy(int icNumber)
        {
            return Ok(await _verificationService.AcceptPolicyAsync(icNumber));
        }

        [HttpPatch("pin")]
        public async Task<IActionResult> AddPin(PinDto dto)
        {
            return Ok(await _verificationService.AddPinAsync(dto));
        }

        [HttpPatch("biometric")]
        public async Task<IActionResult> ToggleBiometric(BiometricDto dto)
        {
            return Ok(await _verificationService.ToggleBiometricAsync(dto));
        }
    }
}
