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
    public class CarroController : Controller
    {
        private readonly IServiceCarro _serviceCarro;
        private readonly IHttpUserAgent _httpUserAgent;

        public CarroController(IServiceCarro serviceCarro, IHttpUserAgent httpUserAgent)
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
            CarroDTO carroDTO = new CarroDTO();
            return View("AdicionarEditarCarro", carroDTO);
        }

        [HttpPost]
        public Task<IActionResult> Adicionar(CarroDTO carroDTO)
        {
            ViewData["Title"] = "Novo Carro";
            return SalvarAsync(carroDTO);
        }

        private async Task<IActionResult> SalvarAsync(CarroDTO carroDTO)
        {
            ApiResult apiResult;
            apiResult = await _httpUserAgent.PostAsync<CarroDTO, ApiResult>("api/salvar-carro", carroDTO);           

            if (apiResult.Success)
            {
                TempData["success"] = "Carro salvo com sucesso!";
                return RedirectToAction("Index");
            }

            return View("AdicionarEditarCarro", carroDTO);
        }

        private async Task<IActionResult> AtualizarAsync(CarroDTO carroDTO)
        {
            ApiResult apiResult;
            apiResult = await _httpUserAgent.PutAsync<CarroDTO, ApiResult>("api/atualizar-carro", carroDTO);

            if (apiResult.Success)
            {
                TempData["success"] = "Carro atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View("AdicionarEditarCarro", carroDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int carroId)
        {
            ViewData["Title"] = "Editar Carro";

            Carro carro = _serviceCarro.ObterPorId(carroId);

            CarroDTO carroDTO = new CarroDTO()
            {
                Marca = carro.Marca,
                Modelo = carro.Modelo,
                Placa = carro.Placa,             
            };           

            return View("AdicionarEditarCarro", carroDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CarroDTO carroDTO)
        {
            ViewData["Title"] = "Editar Carro";
            return await AtualizarAsync(carroDTO);
        }


        public async Task<IActionResult> Excluir(int? carroId)
        {
            try
            {               
               var result = await _httpUserAgent.DeleteAsync($"api/deletar-carro/{carroId}");

                if (result.IsSuccessStatusCode)
                {
                    TempData["success"] = "Carro excluído com sucesso!";
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
        public JsonResult ListaCarros(DataTableAjaxPostModel dataTableModel)
        {
            /*
             * consumido por um DataTable serverSide processing ajax POST
             * 
             * o código deste controlador pode ser usado como base para futuras implementações genéricas com DataTable
             */

            string searchTerm = dataTableModel.search.value;
            string firstOrderColumnIdx = dataTableModel.order.Count > 0 ? dataTableModel.order[0].column.ToString() : "";
            string firstOrderDirection = dataTableModel.order.Count > 0 ? dataTableModel.order[0].dir.ToString() : "";

            IEnumerable<Carro> carros = new List<Carro>();

            if (!String.IsNullOrEmpty(dataTableModel.search.value))
            {
                carros = _serviceCarro.Buscar(
                    x => x.Marca.Contains(searchTerm) ||
                         x.Modelo.Contains(searchTerm) 
                );
            }
            else
                carros = _serviceCarro.ObterTodos();

            if (firstOrderColumnIdx.Length > 0)
            {
                Func<Carro, Object> orderByExpr = null;

                switch (firstOrderColumnIdx)
                {
                    case "1":
                        orderByExpr = x => x.Marca;
                        break;
                    case "2":
                        orderByExpr = x => x.Modelo ?? "-";
                        break;
                    case "3":
                        orderByExpr = x => x.Placa ?? "-";
                        break;                  
                }

                if (orderByExpr != null)
                {
                    if (firstOrderDirection.Length > 0 && firstOrderDirection.Equals("desc"))
                        carros = carros.OrderByDescending(orderByExpr);
                    else
                        carros = carros.OrderBy(orderByExpr);
                }
                else
                {
                    carros = carros.OrderBy(x => x.Marca);
                }
            }
            else
            {
                carros = carros.OrderBy(x => x.Marca);
            }

            // pagina a lista
            int totalResultados = carros.Count();
            carros = carros.Skip(dataTableModel.start).Take(dataTableModel.length);

            // monta o resultado final
            List<object> resultData = new List<object>();
            foreach (var ativo in carros)
            {
                List<object> resultItem = new List<object> {
                    ativo.CarroId,
                    ativo.Marca,
                    ativo.Modelo ?? "-",
                    ativo.Placa ?? "-",
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

        public JsonResult PesquisaCarros(string searchTerm, int pageNumber)
        {
            /*
             * consumido por um Select2 ajax
             * 
             * o código deste controlador pode ser usado como base para futuras implementações genéricas com Select2
             */

            const int pageSize = 10;

            var results = new List<Dictionary<string, string>>();

            var carros = !string.IsNullOrEmpty(searchTerm) ? _serviceCarro.Buscar(x => x.Marca.Contains(searchTerm)) : _serviceCarro.ObterTodos();

            var totalResults = carros.Count();
            carros = carros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var manobrista in carros)
            {
                var resultItem = new Dictionary<string, string>
                {
                    {"id", manobrista.CarroId + ""}, {"text", manobrista.Marca}
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
