using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{

    public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenGenerator _jwt;
        private readonly INotificationService _notification;

        public VerifyOtpHandler(IUserRepository userRepo, IJwtTokenGenerator jwt, INotificationService notification)
        {
            _userRepo = userRepo;
            _jwt = jwt;
            _notification = notification;
        }

        public async Task<VerifyOtpResponseDto> Handle(VerifyOtpCommand request, CancellationToken ct)
        {
            var dto = request.VerifyOtpDto;
            var user = await _userRepo.GetByEmailAsync(dto.Email, ct);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (user.EmailOtp != dto.Otp)
                throw new UnauthorizedAccessException("Invalid OTP");

            if (user.OtpExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("OTP expired");

            // Mark Verified
            user.EmailOtp = null;
            user.OtpExpiryTime = null;
            user.IsEmailConfirmed = true;

            await _userRepo.UpdateAsync(user, ct);

            // Generate token
            var token = _jwt.GenerateToken(user, user.Role);

            // Send login successful email
            string successEmail = $@"
            <h2>Kaappaan Login Successful</h2>
            <p>Hi <strong>{user.Name}</strong>,</p>
            <p>Your login was verified successfully on 
            <strong>{DateTime.Now:dddd, MMMM dd, yyyy hh:mm tt}</strong>.</p>
            <p>Stay safe,<br/>Kaappaan Team</p>";

            await _notification.SendEmailAsync(user.Email, "Kaappaan Login Successful", successEmail);

            return new VerifyOtpResponseDto
            {
                Token = token,
                Name = user.Name,
                Role = user.Role,
                Message = "Login successful"
            };
        }
    }

}

