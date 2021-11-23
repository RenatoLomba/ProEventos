using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public string EventDate { get; set; }

        [
            Required(ErrorMessage = "O campo {0} é obrigatório."),
            StringLength(50, MinimumLength = 3,
                ErrorMessage = "O campo {0} deve conter entre 3 a 50 caracteres.")
        ]
        public string Theme { get; set; }

        [Range(1, 120000, ErrorMessage = "O campo {0} deve ser entre 1 e 120.000,.")]
        public int PeopleQty { get; set; }

        [
            RegularExpression(
                @".*\.(gif|jpe?g|bmp|png)$",
                ErrorMessage = "{0} não é uma imagem válida (gif, jpg, jpeg, bmp ou png).")
        ]
        public string ImageUri { get; set; }

        [
            Required(ErrorMessage = "O campo {0} é obrigatório."),
            Phone(ErrorMessage = "O campo {0} está com um número inválido.")
        ]
        public string Phone { get; set; }

        [
            Display(Name = "E-mail"),
            Required(ErrorMessage = "{0} é obrigatório."),
            EmailAddress(ErrorMessage = "{0} inválido.")
        ]
        public string Email { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }

        public IEnumerable<BatchDto> Batches { get; set; }
        public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; }
        public IEnumerable<SpeakerDto> Speakers { get; set; }
    }
}
