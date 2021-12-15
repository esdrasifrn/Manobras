using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manobra.Infra.Interfaces
{
    public interface IHttpUserAgent
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<T> GetAsync<T>(string uri);

        /// <summary>
        /// Método post assíncrono
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser passado como parâmetro</typeparam>
        /// <typeparam name="TResult">Tipo de retorno do post</typeparam>
        /// <param name="uri">url parcial do endpoint</param>
        /// <param name="obj">conteúdo do objeto a ser passado</param>
        /// <returns></returns>
        Task<TResult> PostAsync<T, TResult>(string uri, T obj);
        Task PostAsync<T>(string uri, T obj);
        Task<HttpResponseMessage> PutAsync<T>(string uri, T obj);
        Task<TResult> PutAsync<T, TResult>(string uri, T obj);
        Task<HttpResponseMessage> DeleteAsync(string uri);
        Task<HttpResponseMessage> DeleteAsync<T>(string uri, T parameters);

        TimeSpan Timeout { get; set; }
    }
}
