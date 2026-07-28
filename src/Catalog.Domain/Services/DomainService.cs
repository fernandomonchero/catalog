using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Catalog.Domain.Services
{
    public abstract class DomainService
    {
        private readonly INotificationCollector _notificationCollector;

        protected DomainService(INotificationCollector notificationCollector)
        {
            _notificationCollector = notificationCollector;
        }

        protected void Notify(ValidationResult validationResult, NotificationType type)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(type,error.ErrorMessage);
            }
        }

        protected void Notify(NotificationType type, string message)
        {
            _notificationCollector.AddNotification(new Notification { Type = type, Message = message });
        }

        protected bool Validate<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notify(validator, NotificationType.Error);

            return false;
        }
    }
}