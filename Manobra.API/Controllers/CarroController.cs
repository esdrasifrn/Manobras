using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Services;
using Manobra.Infra.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;

namespace Manobra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarroController : ControllerBase
    {
        private readonly IServiceCarro _serviceCarro;

        /// <summary>
        /// Api para gerenciar carros
        /// </summary>
        /// <param name="serviceCarro"></param>
        public CarroController(IServiceCarro serviceCarro)
        {
            _serviceCarro = serviceCarro;
        }

        private ApiResult Result(bool success, string message, Exception ex = null)
        {
            return ApiResult.New(success, message, ex);
        }

        /// <summary>
        /// Salvar carro no banco de dados
        /// </summary>
        /// <param name="carroDTO"></param>
        /// <returns></returns>
        [Route("~/api/salvar-carro"), HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> SalvarCarro([FromBody]CarroDTO carroDTO)
        {
            try
            {
                if(carroDTO == null)
                {
                    return BadRequest(carroDTO);
                }

                Carro carro = new Carro()
                {
                    Marca = carroDTO.Marca,
                    Modelo = carroDTO.Modelo,
                    Placa = carroDTO.Placa
                };

                if (_serviceCarro.ProcessaAtualizarCarro(carroDTO) != null)
                    return Ok(Result(true, "Carro salvo com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao salvar o carro: Erro:" + ex.Message, null);
            }

            return BadRequest();
        }

        /// <summary>
        /// Atualiza carro no banco de dados
        /// </summary>
        /// <param name="carroDTO"></param>
        /// <returns></returns>
        [Route("~/api/atualizar-carro"), HttpPut]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> AtualizarCarro([FromBody] CarroDTO carroDTO)
        {
            try
            {
                if (carroDTO == null)
                {
                    return BadRequest(carroDTO);
                }              

                if (_serviceCarro.ProcessaAtualizarCarro(carroDTO) != null)
                    return Ok(Result(true, "Carro atualizado com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao atualizar o carro: Erro:" + ex.Message, null);
            }

            return BadRequest();
        }

        /// <summary>
        /// Deleta carro no banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        [Route("~/api/deletar-carro/{id}"), HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> DeletaCarro(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(id);
                }

                _serviceCarro.ProcessaDeletaCarro(id.GetValueOrDefault());
                    return Ok(Result(true, "Carro deletado com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao deletar o carro: Erro:" + ex.Message, null);
            }
        }

        /// <summary>
        /// Lista carros do banco de dados
        /// </summary>     
        [Route("~/api/lista-carros"), HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> ListaCarros()
        {
            try
            {
               var carros = _serviceCarro.ObterTodos();
                return Ok(Result(true, JsonSerializer.Serialize(carros)));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao listar o carro: Erro:" + ex.Message, null);
            }            
        }


        /// <summary>
        /// Salvar uma manobra no banco de dados
        /// </summary>
        /// <param name="manobrasDTO"></param>
        /// <returns></returns>
        [Route("~/api/salvar-manobra-carro"), HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> GerenciarManobra([FromBody] ManobrasDTO manobrasDTO)
        {
            try
            {
                if (manobrasDTO == null)
                {
                    return BadRequest(manobrasDTO);
                }               

                if (_serviceCarro.ProcessaManobra(manobrasDTO) != null)
                    return Ok(Result(true, "Manobra salva com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao salvar a manobra: Erro:" + ex.Message, null);
            }

            return BadRequest();
        }
    }
}
