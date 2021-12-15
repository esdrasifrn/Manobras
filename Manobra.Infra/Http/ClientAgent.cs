using Manobra.Infra.Exceptions;
using Manobra.Infra.Extensions;
using Manobra.Infra.Helpers;
using Manobra.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manobra.Infra.Http
{
    public class ClientAgent : IHttpUserAgent
    {
        private readonly HttpClient _client;

        public TimeSpan Timeout
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        public ClientAgent(string baseUrl, HttpMessageHandler handler)
        {
            ServicePointManager.ServerCertificateValidationCallback
                += (sender, cert, chain, sslPolicyErrors) => true;

            if (!baseUrl.EndsWith("/"))
                baseUrl = string.Concat(baseUrl, "/");

            _client = new HttpClient(handler)
            {
               BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await _client.GetAsync(uri);

            ValidateResponse(response);

            return await response.Content.ReadAsObjectAsync<T>();
        }

       
        public async Task<TResult> PostAsync<T, TResult>(string uri, T obj)
        {
            var response = await _client.PostAsync(uri, obj.ToHttpContent());

            ValidateResponse(response);

            return await response.Content.ReadAsObjectAsync<TResult>();
        }

        public async Task PostAsync<T>(string uri, T obj)
        {
            var response = await _client.PostAsync(uri, obj.ToHttpContent());

            ValidateResponse(response);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T obj)
        {
            var httpContent = obj.ToHttpContent();

            var response = await _client.PutAsync(uri, httpContent);

            await ValidateResponseAsync(response);

            return response;
        }

        public async Task<TResult> PutAsync<T, TResult>(string uri, T obj)
        {
            var response = await _client.PutAsync(uri, obj.ToHttpContent());

            await ValidateResponseAsync(response);

            return await response.Content.ReadAsObjectAsync<TResult>();
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var response = await _client.DeleteAsync(uri);

            await ValidateResponseAsync(response);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync<T>(string uri, T parameters)
        {
            _client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", "DELETE");

            var response = await _client.PostAsync(uri, parameters.ToHttpContent());

            _client.DefaultRequestHeaders.Remove("X-HTTP-Method-Override");

            await ValidateResponseAsync(response);

            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);

            await ValidateResponseAsync(response);

            return response;
        }

        private void ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string responseContent = "";
            if (response.Content != null)
                responseContent = AsyncHelper.RunSync(() => response.Content.ReadAsStringAsync());

            throw new ApiRequestException(response.StatusCode, response.RequestMessage.RequestUri.ToString(), response.ReasonPhrase + "\r\n" + responseContent);
        }

        private async Task ValidateResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string responseContent = "";
            if (response.Content != null)
                responseContent = await response.Content.ReadAsStringAsync();

            throw new ApiRequestException(response.StatusCode, response.RequestMessage.RequestUri.ToString(), response.ReasonPhrase + "\r\n" + responseContent);
        }
    }
}
