using Nancy.Json; //se utiliza para serializar objetos en formato JSON
using Newtonsoft.Json; // se utiliza para deserializar objetos desde formato JSON
using System.Net.Http.Headers; //permite la configuracion de encabezados en las solicitudes HTTP
using System.Text; //se utiliza para especificar la codificacion de los datos
using System.Web.Http; //contiene excepciones y utilidades para controladores de API


namespace CurrencyConversionAPI.Resource
{
    public class ExampleAPI
    {
        //Método asincrónico que obtiene una cotizacion especifica de una API externa
        public async Task<decimal> GetSpecificQuote()
        {
            HttpResponseMessage response; //variable para almacenar la respuesta HTTP
            using (var client = new HttpClient()) //crea una instancia de HttpClient para realizar solicitudes HTTP
            {
                //crea una objeto RequestCurrency que contiene el codigo de la moneda que se solicita
                RequestCurrency currency = new RequestCurrency();
                currency.Code = "Bolsa"; //especifica el codigo de la moneda (en este caso, "Bolsa")

                //convierte el objeto currency a un formato JSON  que puede enviarse en la solicitud
                var jsonObject = new JavaScriptSerializer().Serialize(currency);//convierto a json lo que voy a pasar como parametro

                //define el contenido de la solicitud HTTP, especificando el JSON y su codificacion
                var content = new StringContent(jsonObject.ToString(),Encoding.UTF8,"application/json");

                //configura el tipo de contenido en los encabezados de la solicitud
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //realiza una solicitud POST a la API especificada en la URL
                response = await client.PostAsync("https://localhost:7244/api/GetQuote", content);

                //verifica si la respuesta no fue exitosa; en caso negativo, lanza una excepcion
                if(!response.IsSuccessStatusCode)
                {
                    var httpResponse = new HttpResponseMessage(response.StatusCode);

                    //agrega detalles de error a la respuesta HTTP en caso de conexion fallida
                    httpResponse.ReasonPhrase = "error to conect examople api";
                    httpResponse.Content = new StringContent("");

                    //lanza una excepcion de HTTP para manejar el error
                    throw new HttpResponseException(httpResponse);
                }

                //lee el contenido de la respuesta como una cadena JSON
                string str = response.Content.ReadAsStringAsync().Result;

                //deserializa el JSON de la respuesta en un objeto de tipo QuoteCurrencyResponse
                QuoteCurrencyResponse? result = JsonConvert.DeserializeObject<QuoteCurrencyResponse?>(str);

                //retorna el valor de la propiedad 'Venta' de la respuesta
                return result.Venta;

            }
        }
    }
}
