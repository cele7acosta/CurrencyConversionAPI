using CurrencyConversionAPI.Resource; //referencia al namespace donde se encuentra la clase ExampleAPI
using Microsoft.AspNetCore.Mvc; // proporciona clases para construir APIs en ASP.NET Core
using Newtonsoft.Json; //permite trabajar con JSON para serializar y deserializar
using System.ComponentModel.DataAnnotations; // contiene atributos de validacion como Required

namespace CurrencyConversionAPI.Controllers
{
    //configuracion de ruta para acceder a esta API; se asocia a 'api/ConverterController'
    [Route("api/[controller]")] //indica que este controlador responde a solicitudes de una API
    [ApiController]
    public class ConverterController : ControllerBase
    {
        //define una accion de tipo GET y le asigna el nombre 'GetQuote'
        [HttpGet(Name ="GetQuote")]

        //Metodo asincronico que realiza una conversion de divisa con un valor proporcionado en la cabecera
        public async Task<string> GetQuote([FromQuery, Required] decimal value)//FromQuery para indicar que el parametro se lo mando en la cabecera
        {
            //crea una instancia de ExampleAPI, que contiene metodos para obtener cotizaciones
            ExampleAPI api = new ExampleAPI(); //instancio la clase

            //llama al metodo GetSpecificQuote() que obtiene la cotizacion actual del dolar
            decimal result = await api.GetSpecificQuote();

            //realiza la conversion  multiplicando el valor obtenido de la cotizacion por el valor proporcionado por el usuario
            return (result * value).ToString();
        }
    }
}
