﻿using DemoApplication.Contracts;
using DemoApplication.Contracts.Email;

namespace DemoApplication.Services.Abstracts
{
    public interface IEmailService
    {
        public void Send(MessageDto message);
    }
}
