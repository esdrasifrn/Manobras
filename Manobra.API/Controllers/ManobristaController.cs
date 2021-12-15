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
    public class ManobristaController : ControllerBase
    {
        private readonly IServiceManobrista _serviceManobrista;

        /// <summary>
        /// Api para gerenciar manobristas
        /// </summary>
        /// <param name="serviceManobrista"></param>
        public ManobristaController(IServiceManobrista serviceManobrista)
        {
            _serviceManobrista = serviceManobrista;
        }

        private ApiResult Result(bool success, string message, Exception ex = null)
        {
            return ApiResult.New(success, message, ex);
        }

        /// <summary>
        /// Salvar manobrista no banco de dados
        /// </summary>
        /// <param name="manobristaDTO"></param>
        /// <returns></returns>
        [Route("~/api/salvar-manobrista"), HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> SalvarManobrista([FromBody] ManobristaDTO manobristaDTO)
        {
            try
            {
                if(manobristaDTO == null)
                {
                    return BadRequest(manobristaDTO);
                }

                Manobrista manobrista = new Manobrista()
                {
                    Nome = manobristaDTO.Nome,
                    CPF = manobristaDTO.CPF,
                    DataNascimento = manobristaDTO.DataNascimento
                };

                if (_serviceManobrista.ProcessaAtualizarManobrista(manobristaDTO) != null)
                    return Ok(Result(true, "Manobrista salvo com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao salvar o manobrista: Erro:" + ex.Message, null);
            }

            return BadRequest();
        }

        /// <summary>
        /// Atualiza manobrista no banco de dados
        /// </summary>
        /// <param name="manobristaDTO"></param>
        /// <returns></returns>
        [Route("~/api/atualizar-manobrista"), HttpPut]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> AtualizarCarro([FromBody] ManobristaDTO manobristaDTO)
        {
            try
            {
                if (manobristaDTO == null)
                {
                    return BadRequest(manobristaDTO);
                }              

                if (_serviceManobrista.ProcessaAtualizarManobrista(manobristaDTO) != null)
                    return Ok(Result(true, "Manobrista atualizado com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao atualizar o manobrista: Erro:" + ex.Message, null);
            }

            return BadRequest();
        }

        /// <summary>
        /// Deleta manobristas do banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        [Route("~/api/deletar-manobrista/{id}"), HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> DeletaManobrista(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(id);
                }

                _serviceManobrista.ProcessaDeletaManobrista(id.GetValueOrDefault());
                    return Ok(Result(true, "Manobrista deletado com sucesso."));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao deletar o manobrista: Erro:" + ex.Message, null);
            }
        }

        /// <summary>
        /// Lista manobristas do banco de dados
        /// </summary>     
        [Route("~/api/lista-manobristas"), HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResult> ListaManobristas()
        {
            try
            {
               var carros = _serviceManobrista.ObterTodos();
                return Ok(Result(true, JsonSerializer.Serialize(carros)));
            }
            catch (Exception ex)
            {

                return Result(false, "Ocorreu um erro ao listar o manobrista: Erro:" + ex.Message, null);
            }            
        }
    }
}
