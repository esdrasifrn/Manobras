using Manobra.Domain.DTO;
using Manobra.Domain.Entities;
using Manobra.Domain.Interfaces.Services;
using Manobra.Infra.Helpers;
using Manobra.Infra.Interfaces;
using Manobra.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manobra.Web.Controllers
{
    public class ManobrasController : Controller
    {
        private readonly IServiceCarro _serviceCarro;
        private readonly IHttpUserAgent _httpUserAgent;

        public ManobrasController(IServiceCarro serviceCarro, IHttpUserAgent httpUserAgent)
        {
            _serviceCarro = serviceCarro;
            _httpUserAgent = httpUserAgent;
        }

        public IActionResult Index()
        {
            return View();
        }
     

        [HttpGet]
        public IActionResult Adicionar()
        {
            ManobrasDTO manobrasDTO = new ManobrasDTO();
            return View("AdicionarEditarManobra", manobrasDTO);
        }

        [HttpPost]
        public Task<IActionResult> Adicionar(ManobrasDTO manobrasDTO)
        {
            ViewData["Title"] = "Nova Manobra";
            return SalvarAsync(manobrasDTO);
        }

        private async Task<IActionResult> SalvarAsync(ManobrasDTO manobrasDTO)
        {
            ApiResult apiResult;
            apiResult = await _httpUserAgent.PostAsync<ManobrasDTO, ApiResult>("api/salvar-manobra-carro", manobrasDTO);

            if (apiResult.Success)
            {
                TempData["success"] = "Manobra salva com sucesso!";
                return RedirectToAction("Index");
            }

            return View("AdicionarEditarManobra", manobrasDTO);
        }      
       
        [HttpPost]
        public JsonResult ListaManobras(DataTableAjaxPostModel dataTableModel)
        {
            /*
             * consumido por um DataTable serverSide processing ajax POST
             * 
             * o código deste controlador pode ser usado como base para futuras implementações genéricas com DataTable
             */

            string searchTerm = dataTableModel.search.value;
            string firstOrderColumnIdx = dataTableModel.order.Count > 0 ? dataTableModel.order[0].column.ToString() : "";
            string firstOrderDirection = dataTableModel.order.Count > 0 ? dataTableModel.order[0].dir.ToString() : "";

            IEnumerable<ManobrasDTO> carrosManobrados = new List<ManobrasDTO>();
            
            if (!String.IsNullOrEmpty(dataTableModel.search.value))
            {
                carrosManobrados = _serviceCarro.ListaManobrasCarroDTO(searchTerm);
            }
            else
                carrosManobrados = _serviceCarro.ListaManobrasCarroDTOTodos();

            if (firstOrderColumnIdx.Length > 0)
            {
                Func<ManobrasDTO, Object> orderByExpr = null;

                switch (firstOrderColumnIdx)
                {
                    case "1":
                        orderByExpr = x => x.NomeCarro;
                        break;
                    case "2":
                        orderByExpr = x => x.NomeManobrista ?? "-";
                        break;                                    
                }

                if (orderByExpr != null)
                {
                    if (firstOrderDirection.Length > 0 && firstOrderDirection.Equals("desc"))
                        carrosManobrados = carrosManobrados.OrderByDescending(orderByExpr);
                    else
                        carrosManobrados = carrosManobrados.OrderBy(orderByExpr);
                }
                else
                {
                    carrosManobrados = carrosManobrados.OrderBy(x => x.NomeManobrista);
                }
            }
            else
            {
                carrosManobrados = carrosManobrados.OrderBy(x => x.NomeManobrista);
            }

            // pagina a lista
            int totalResultados = carrosManobrados.Count();
            carrosManobrados = carrosManobrados.Skip(dataTableModel.start).Take(dataTableModel.length);

            // monta o resultado final
            List<object> resultData = new List<object>();
            foreach (var carro in carrosManobrados)
            {
                List<object> resultItem = new List<object> {
                    carro.CarroId,
                    carro.NomeCarro,
                    carro.NomeManobrista ?? "-",                    
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
    }
}
