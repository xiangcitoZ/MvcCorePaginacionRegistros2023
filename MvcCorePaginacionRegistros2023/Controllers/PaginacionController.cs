﻿using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros2023.Models;
using MvcCorePaginacionRegistros2023.Repositories;

namespace MvcCorePaginacionRegistros2023.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult>
           PaginarGrupoEmpleados(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int registros = this.repo.GetNumeroEmpleados();
            
            List<Empleado> empleados =
                await this.repo.GetGrupoEmpleadosAsync(posicion.Value);
            ViewData["REGISTROS"] = registros;
            return View(empleados);
        }


        // Metodo para el SP_GRUPO_EMPLEADOS_OFICIO
        public async Task<IActionResult>
            EmpleadosOficio(int? posicion, string oficio)
        {
          if(posicion == null) 
            {
                posicion = 1;
                return View();
            }
          else
            {
                ModelPaginarEmpleados model =
                await this.repo.GetGrupoEmpleadosOficioAsync(posicion.Value, oficio);
                List<Empleado> empleados = model.Empleados;
                int numRegistros = model.NumeroRegistros;
                ViewData["REGISTROS"] = numRegistros;
                ViewData["OFICIO"] = oficio;
                return View(empleados);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult>
            EmpleadosOficio(string oficio)
        {   
            ModelPaginarEmpleados model =
                await this.repo.GetGrupoEmpleadosOficioAsync(1,oficio);
            List<Empleado> empleados = model.Empleados;
               
            int numRegistros = this.repo.GetNumeroEmpleadosOficio(oficio);
            ViewData["REGISTROS"] = numRegistros;
            ViewData["OFICIO"] = oficio;
            return View(empleados);
        }


      


        public async Task<IActionResult>
            PaginarGrupoDepartamentos(int? posicion)
        {
            if(posicion == null) 
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numeroregistros;
            List<Departamento> departamentos =
                await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View (departamentos);
        }


        public async Task<IActionResult>
         PaginarGrupoVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numRegistros;
            // <a href='PaginarGrupo?posicion=1'>Pagina 1</a>
            // <a href='PaginarGrupo?posicion=3'>Pagina 2</a>
            // <a href='PaginarGrupo?posicion=5'>Pagina 3</a>
            int numeroPagina = 1;
            //NECESITAMOS CREAR UN BUCLE QUE VAYA DE N EN N 
            //DEPENDIENDO DEL NUMERO DE REGISTROS A PAGINAR
            //LLEGAREMOS HASTA EL NUMERO DE REGISTROS
            string html = "<div>";
            for (int i = 1; i <= numRegistros; i += 2)
            {
                html +=
                    "<a href='PaginarGrupoVistaDepartamento?posicion="
                    + i + "'>Página " + numeroPagina + "</a> | ";
                numeroPagina += 1;
            }
            html += "</div>";
            ViewData["LINKS"] = html;
            List<VistaDepartamento> departamentos =
                await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }



        public async Task<IActionResult>
            PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            //ESTAMOS EN LA POSICION 1, QUE TENEMOS QUE DEVOLVER A LA VISTA?
            int siguiente = posicion.Value + 1;
            if (siguiente > numregistros)
            {
                //EFECTO OPTICO
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vistaDepartamento =
                await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vistaDepartamento);
        }
    }
}
