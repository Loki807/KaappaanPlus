using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using MediatR;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenGenerator _jwt;

        public VerifyOtpHandler(IUserRepository userRepo, IJwtTokenGenerator jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        public async Task<VerifyOtpResponseDto> Handle(VerifyOtpCommand request, CancellationToken ct)
        {
            var dto = request.VerifyOtpDto;

            var user = await _userRepo.GetByEmailAsync(dto.Email, ct);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (user.EmailOtp != dto.Otp)
                throw new UnauthorizedAccessException("Invalid OTP");

            if (user.OtpExpiryTime == null || user.OtpExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("OTP expired");

            // Mark as verified
            user.EmailOtp = null;
            user.OtpExpiryTime = null;
            user.IsEmailConfirmed = true;

            await _userRepo.UpdateAsync(user, ct);

            // Generate token after verification
            var token = _jwt.GenerateToken(user, user.Role);

            return new VerifyOtpResponseDto
            {
                Token = token,
                Name = user.Name,
                Role = user.Role,
                Message = "Email verified successfully"
            };
        }
    }
}
