using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.BookDTOs;
using FluentValidation;


namespace Books.Application.Validators
{
    public class BookValidation: AbstractValidator<BookCreateDto>
    {
        public BookValidation()
        {
            // Назва: не порожня, максимум 200 символів
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Назва книги обов'язкова")
                .MaximumLength(200).WithMessage("Назва не може бути довшою за 200 символів");

            RuleFor(book => book.GenreId)
                .NotEmpty().WithMessage("Укажите жанр книги");

            // Автор: обов'язкове поле
            RuleFor(book => book.AuthersId)
                .NotEmpty().WithMessage("Вкажіть автора книги");

            // Рік видання: не може бути в майбутньому
            RuleFor(book => book.Year)
                .InclusiveBetween(1500, DateTime.Now.Year)
                .WithMessage($"Рік видання має бути між 1500 та {DateTime.Now.Year}");
        }
    }
}
