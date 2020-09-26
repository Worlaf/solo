using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solo.Data.Infrastructure;
using Solo.Data.Repositories;
using Solo.Domain.Entities;

namespace Solo.Api.App.Authentication
{
    // Простейшая аутентификация для демонстрации приложения
    // Используем имейл в Authorization header как уникальный ключ пользователя
    // Если пользователя нет - создаем
    // Если нет хедера - значит к нам зашли с админки
    public class SimplestEmailAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SimplestEmailAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, UserRepository userRepository, IUnitOfWork unitOfWork) : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            User user = null;
            const string authorizationHeaderKey = "Authorization";
            if (!Request.Headers.ContainsKey(authorizationHeaderKey))
            {
                user = await _userRepository.SingleAsync(u => u.AuthId == "Admin");
            }
            else
            {
                var email = Request.Headers[authorizationHeaderKey][0];
                user = await _userRepository.SingleOrDefaultAsync(u => u.AuthId == email);
                if (user == null)
                {
                    user = new User
                    {
                        AuthId = email
                    };

                    _userRepository.Save(user);
                    await _unitOfWork.CommitAsync();
                }
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.AuthId),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
