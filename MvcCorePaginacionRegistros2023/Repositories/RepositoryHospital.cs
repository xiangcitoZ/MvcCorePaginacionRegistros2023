using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros2023.Data;
using MvcCorePaginacionRegistros2023.Models;

#region PROCEDURES

//CREATE VIEW V_DEPARTAMENTOS_INDIVIDUAL
//AS
//    SELECT CAST(
//    ROW_NUMBER() OVER(ORDER BY DEPT_NO) AS INT) AS POSICION,
//    ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC 
//    FROM DEPT
//GO


//CREATE PROCEDURE SP_GRUPO_DEPARTAMENTOS
//(@POSICION INT)
//AS
//    SELECT DEPT_NO, DNOMBRE, LOC
//	FROM V_DEPARTAMENTOS_INDIVIDUAL
//	WHERE POSICION >= @POSICION AND POSICION <(@POSICION + 2)
//GO


//CREATE VIEW V_GRUPO_EMPLEADOS
//AS

//	SELECT CAST(
//		ROW_NUMBER() OVER(ORDER BY APELLIDO) AS int) AS POSICION
//        , ISNULL(EMP_NO, 0) AS EMP_NO,
//        APELLIDO, OFICIO, SALARIO, DEPT_NO FROM EMP
//GO
//CREATE PROCEDURE SP_GRUPO_EMPLEADOS
//(@POSICION INT)
//AS
//    SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//	FROM V_GRUPO_EMPLEADOS
//	WHERE POSICION >= @POSICION AND POSICION <(@POSICION +3)
//GO



//CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
//(@POSICION INT, @OFICIO NVARCHAR(20))
//AS
//	SELECT * FROM
//    (SELECT CAST(
//        ROW_NUMBER() OVER(ORDER BY APELLIDO) AS INT) AS POSICION,
//        EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//    FROM EMP
//    WHERE OFICIO = @OFICIO) AS QUERY
//	WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)

//GO

#endregion


namespace MvcCorePaginacionRegistros2023.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }




        #region VISTA DEPARTAMENTOS



        public async Task<List<VistaDepartamento>>
            GetGrupoVistaDepartamentoAsync(int posicion)
        {
            //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL
            //WHERE POSICION >= @POSICION AND POSICION<(@POSICION +2)
            
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        
        }

        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }

        public async Task<VistaDepartamento> 
            GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await
                this.context.VistaDepartamentos
                .FirstOrDefaultAsync(x => x.Posicion == posicion);
            return vista;
        }

        #endregion

        #region DEPARTAMENTOS

        public async Task<List<Departamento>>
            GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta = 
                this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public async Task<List<Departamento>> GetDepartamentos()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<Departamento> FindDepartamento(int iddepartamento)
        {
            Departamento departamento =
                await this.context.Departamentos
                .FirstOrDefaultAsync(x => x.IdDepartamento == iddepartamento);
            return departamento;
        }

        #endregion

        #region EMPLEADOS

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta = 
                this.context.Empleados.FromSqlRaw(sql,pamposicion);
            return await consulta.ToListAsync() ;
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosOficioAsync(int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS @POSICION @OFICIO";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            SqlParameter pamoficio =
               new SqlParameter("@OFICIO", oficio);
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamposicion, pamoficio);
            return await consulta.ToListAsync();
        }


        public int GetNumeroEmpleados()
        {
            return this.context.Empleados.Count();
        }

        public async Task<List<Empleado>> GetEmpleados()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleado(int idempleado)
        {
            Empleado empleado =
                await this.context.Empleados
                .FirstOrDefaultAsync(x => x.IdEmpleado == idempleado);
            return empleado;
        }
        #endregion
    }
}
