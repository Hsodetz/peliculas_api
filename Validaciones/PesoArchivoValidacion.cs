using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        private readonly int pesoMaximoEnMegaBytes;

        public PesoArchivoValidacion(int PesoMaximoEnMegaBytes)
        {
            pesoMaximoEnMegaBytes = PesoMaximoEnMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if(formFile == null)
                return ValidationResult.Success;

            if(formFile.Length > pesoMaximoEnMegaBytes * 1024 * 1024)
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoEnMegaBytes}mb"); 

            return ValidationResult.Success;
        }
    }
}