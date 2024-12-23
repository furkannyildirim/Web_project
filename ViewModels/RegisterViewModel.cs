using System.ComponentModel.DataAnnotations;

namespace BooksStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        [Display(Name = "Ad")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        [Display(Name = "Soyad")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Kullanıcı adı en az 6 karakter olmalıdır.")]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

         
    }
}
