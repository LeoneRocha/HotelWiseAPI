﻿using FluentValidation;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Validator
{
    public class HotelValidator : AbstractValidator<Hotel>
    {
        public HotelValidator()
        {
            RuleFor(h => h.HotelName).NotEmpty().MaximumLength(100);
            RuleFor(h => h.Description).MaximumLength(500);
            RuleFor(h => h.Stars).InclusiveBetween(1, 5);
            RuleFor(h => h.InitialRoomPrice).GreaterThan(0);
            RuleFor(h => h.ZipCode).MaximumLength(10);
            RuleFor(h => h.Location).MaximumLength(200);
        }
    } 
}
