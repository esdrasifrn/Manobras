using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Services;
using Manobra.Infra.Helpers;
using Manobra.Infra.Interfaces;
using Manobra.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Manobra.Web.Controllers
{
    public class ManobristaController : Controller
    {
        private readonly IServiceManobrista _serviceManobrista;
        private readonly IHttpUserAgent _httpUserAgent;

        public ManobristaController(IServiceManobrista serviceManobrista, IHttpUserAgent httpUserAgent)
        {
            _serviceManobrista = serviceManobrista;
            _httpUserAgent = httpUserAgent;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Adicionar()
        {
            ManobristaDTO manobristaDTO = new ManobristaDTO();
            return View("AdicionarEditarManobrista", manobristaDTO);
        }

        [HttpPost]
        public Task<IActionResult> Adicionar(ManobristaDTO manobristaDTO)
        {
            ViewData["Title"] = "Novo Manobrista";
            return SalvarAsync(manobristaDTO);
        }

        private async Task<IActionResult> SalvarAsync(ManobristaDTO manobristaDTO)
        {
            ApiResult apiResult;
            apiResult = await _httpUserAgent.PostAsync<ManobristaDTO, ApiResult>("api/salvar-manobrista", manobristaDTO);           

            if (apiResult.Success)
            {
                TempData["success"] = "Manobrista salvo com sucesso!";
                return RedirectToAction("Index");
            }
            
            return View("AdicionarEditarManobrista", manobristaDTO);
        }

        private async Task<IActionResult> AtualizarAsync(ManobristaDTO manobristaDTO)
        {
            ApiResult apiResult;
            apiResult = await _httpUserAgent.PutAsync<ManobristaDTO, ApiResult>("api/atualizar-manobrista", manobristaDTO);

            if (apiResult.Success)
            {
                TempData["success"] = "Manobrista atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View("AdicionarEditarManobrista", manobristaDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int manobristaId)
        {
            ViewData["Title"] = "Editar Manobrista";

            Manobrista manobrista = _serviceManobrista.ObterPorId(manobristaId);

            ManobristaDTO manobristaDTO = new ManobristaDTO()
            {
                Nome = manobrista.Nome,
                CPF = manobrista.CPF,
                DataNascimento = manobrista.DataNascimento,             
            };           

            return View("AdicionarEditarManobrista", manobristaDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ManobristaDTO manobristaDTO)
        {
            ViewData["Title"] = "Editar Manobrista";
            return await AtualizarAsync(manobristaDTO);
        }


        public async Task<IActionResult> Excluir(int? manobristaId)
        {
            try
            {               
               var result = await _httpUserAgent.DeleteAsync($"api/deletar-manobrista/{manobristaId}");

                if (result.IsSuccessStatusCode)
                {
                    TempData["success"] = "Manobrista excluído com sucesso!";
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception ex)
            {
                TempData["danger"] = "Erro ao tentar excluir carro!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ListaManobristas(DataTableAjaxPostModel dataTableModel)
        {
            /*
             * consumido por um DataTable serverSide processing ajax POST
             * 
             * o código deste controlador pode ser usado como base para futuras implementações genéricas com DataTable
             */

            string searchTerm = dataTableModel.search.value;
            string firstOrderColumnIdx = dataTableModel.order.Count > 0 ? dataTableModel.order[0].column.ToString() : "";
            string firstOrderDirection = dataTableModel.order.Count > 0 ? dataTableModel.order[0].dir.ToString() : "";

            IEnumerable<Manobrista> manobristas = new List<Manobrista>();

            if (!String.IsNullOrEmpty(dataTableModel.search.value))
            {
                manobristas = _serviceManobrista.Buscar(
                    x => x.Nome.Contains(searchTerm) ||
                         x.CPF.Contains(searchTerm) 
                );
            }
            else
                manobristas = _serviceManobrista.ObterTodos();

            if (firstOrderColumnIdx.Length > 0)
            {
                Func<Manobrista, Object> orderByExpr = null;

                switch (firstOrderColumnIdx)
                {
                    case "1":
                        orderByExpr = x => x.Nome;
                        break;
                    case "2":
                        orderByExpr = x => x.CPF ?? "-";
                        break;                                 
                }

                if (orderByExpr != null)
                {
                    if (firstOrderDirection.Length > 0 && firstOrderDirection.Equals("desc"))
                        manobristas = manobristas.OrderByDescending(orderByExpr);
                    else
                        manobristas = manobristas.OrderBy(orderByExpr);
                }
                else
                {
                    manobristas = manobristas.OrderBy(x => x.Nome);
                }
            }
            else
            {
                manobristas = manobristas.OrderBy(x => x.CPF);
            }

            // pagina a lista
            int totalResultados = manobristas.Count();
            manobristas = manobristas.Skip(dataTableModel.start).Take(dataTableModel.length);

            // monta o resultado final
            List<object> resultData = new List<object>();
            foreach (var manobrista in manobristas)
            {
                List<object> resultItem = new List<object> {
                    manobrista.ManobristaId,
                    manobrista.Nome,
                    manobrista.CPF ?? "-",
                    String.Format(CultureInfo.CreateSpecificCulture("pt-BR"), "{0:dd/MM/yyyy}", manobrista.DataNascimento),
                };
                resultData.Add(resultItem);
            }

            return Json(new
            {
                recordsTotal = totalResultados,
                recordsFiltered = totalResultados,
                data = resultData
            });
        }


        public JsonResult PesquisaManobrista(string searchTerm, int pageNumber)
        {
            /*
             * consumido por um Select2 ajax
             * 
             * o código deste controlador pode ser usado como base para futuras implementações genéricas com Select2
             */

            const int pageSize = 10;

            var results = new List<Dictionary<string, string>>();

            var manobristas = !string.IsNullOrEmpty(searchTerm) ? _serviceManobrista.Buscar(x => x.Nome.Contains(searchTerm)) : _serviceManobrista.ObterTodos();

            var totalResults = manobristas.Count();
            manobristas = manobristas.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var manobrista in manobristas)
            {
                var resultItem = new Dictionary<string, string>
                {
                    {"id", manobrista.ManobristaId + ""}, {"text", manobrista.Nome}
                };
                results.Add(resultItem);
            }

            return Json(new
            {
                pageSize,
                results,
                totalResults
            });
        }

    }
}
