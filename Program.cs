using System;
using System.Collections.Generic;

namespace grafos
{
    class Program
    {
        static void Main(string[] args)

        {
            List<Grupo> Listadosgrupos = new List<Grupo>();
            Listadosgrupos = llenarGrupos();
            List<Asignatura> listaAsignaturas = new List<Asignatura>();
            listaAsignaturas = llenarAsignatura();
            List<Matricula> listadosMatriculas = new List<Matricula>();
            Grupo grupoAAMatricular = new Grupo();
            List<Estudiante> Listadosestudiante = new List<Estudiante>();
            Listadosestudiante = LlenarEstudiante();
            bool m;
            Asignatura a = new Asignatura();
            Grupo grupoAMatricular = new Grupo();
            Grupo grupoBuscado = new Grupo();
            String codAsignatura, opc, opcM, opC, codGrupo;
            int identificacion;
            opc = "s";
            Estudiante estudianteBuscado = new Estudiante();



            do
            {
                //1.	El estudiante proporciona su identificación (Numero de documento de identidad)
                Console.WriteLine("Digite su identificacion : ");
                identificacion = Convert.ToInt32(Console.ReadLine());

                estudianteBuscado = BuscarEstudiante(identificacion, Listadosestudiante);

            } while (estudianteBuscado == null);

            Console.WriteLine("******* BIENVENIDO AL SISTEM *****");

            Console.WriteLine(estudianteBuscado.toString());
            Console.WriteLine("------------------------ ");
            Console.WriteLine("");
            Console.WriteLine("Desea Registrar una Materia");
            //opc Controla el ingreso al while "S" ingresa.
            opc = Console.ReadLine().ToLower();

            while (opc.Equals("s"))
            {
                Console.WriteLine(" ");

                do
                {
                    //2.	El estudiante ingresa el código de la asignatura (5 caracteres)
                    Console.WriteLine("Ingresa el codigo de la asignatura : ");
                    Console.WriteLine("------- El codAsignatura es un cod de 5 caracteres ----");

                    codAsignatura = Console.ReadLine();

                } while (codAsignatura.Length < 5 || codAsignatura.Length > 5);
                Console.WriteLine(")");

                //Buscar que la asignatura existe.
                a = BuscarAsignatura(codAsignatura, listaAsignaturas);

                if (a != null)
                {
                    Console.WriteLine("Informacion de la asignatura : ");
                    Console.WriteLine(a.toString());
                    Console.WriteLine("-------------------------------");
                    //3.	El sistema valida que la asignatura no ha sido seleccionada 
                    if (VerificarMatricula(codAsignatura, listadosMatriculas) == null)
                    {
                        Console.WriteLine("Los grupos Disponible para esta asignatura son :");
                        ConsultarGrupo(codAsignatura, Listadosgrupos);

                        Grupo grupoM = new Grupo();
                        do
                        {
                            Console.WriteLine("Ingresa el codigo del grupo que dese matricular :");
                            codGrupo = Console.ReadLine();

                        } while (codGrupo.Length < 2 || codGrupo.Length > 2);

                        //5.El estudiante ingresa el código del grupo (2 dígitos)

                        //Console.WriteLine(grupoM.Horario+"      **************");

                        grupoM = BuscarGrupo(codGrupo, Listadosgrupos);
                        //4.	El sistema valida cruce de horario con otra asignatura y muestra el nombre y la lista de grupos. 
                        m = BuscarMatricula(grupoM.Horario, listadosMatriculas);

                        if (m == false)
                        {
                            // 6.	El sistema verifica disponibilidad del cupo  
                            if (grupoM != null && grupoM.Cupos != 0)
                            {
                                Random random = new Random();
                                int codMatricula = random.Next(1, 201);
                                ///7.	Guarda la asignatura matriculada 
                                Matricula mNueva = new Matricula(codMatricula, identificacion, grupoM.Nombre, a.Nombre, grupoM.Horario, codAsignatura);
                                listadosMatriculas.Add(mNueva);
                                grupoM.DescontarCupos();

                                Console.WriteLine("Se matriculo con exito esta Asignatura");
                                Console.WriteLine("");
                                Console.WriteLine("");

                                //8.	Ingresar opción de continuar (agregar otra asignatura, eliminar asignatura, terminar)
                                Console.WriteLine("Desea Registrar otra Materia");
                                opc = Console.ReadLine().ToLower();
                            }
                            else
                            {
                                Console.WriteLine("El grupo no tiene cupos");
                                Console.WriteLine("Los grupos Disponible para esta asignatura son :");
                                ConsultarGrupo(codAsignatura, Listadosgrupos);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Se cruza el horario");
                            Console.WriteLine("Desea continuar registrando una Materia--- S/N");
                            opc = Console.ReadLine().ToLower();
                        }

                    }
                    else
                    {
                        Console.WriteLine("La asignatura se encuentra matriculada");
                        Console.WriteLine("Desea continuar registrando una Materia--- S/N");
                        opc = Console.ReadLine().ToLower();
                    }
                }
                else
                {
                    Console.WriteLine("La asignatura no existe");
                    Console.WriteLine("Desea continuar registrando una Materia--- S/N");
                    opc = Console.ReadLine().ToLower();
                }
            }

            Console.WriteLine("***** Matricula terminada ******");
            Console.WriteLine("Sus asignaturas matriculadas son : ");
            ConsultarMatricula(listadosMatriculas);
            Console.ReadKey();
        }

        public static List<Grupo> llenarGrupos()
        {

            List<Grupo> grupos = new List<Grupo>();
            grupos.Add(new Grupo("13", "1", 2, "lunes", "12345"));
            grupos.Add(new Grupo("14", "2", 2, "martes", "67890"));
            grupos.Add(new Grupo("15", "3", 1, "miercoles", "34567"));
            grupos.Add(new Grupo("16", "5", 2, "jueves", "12345"));
            grupos.Add(new Grupo("17", "6", 2, "lunes", "67890"));
            grupos.Add(new Grupo("18", "7", 1, "sabado", "34567"));
            grupos.Add(new Grupo("19", "9", 2, "lunes", "12345"));
            return grupos;
        }
        public static List<Estudiante> LlenarEstudiante()
        {

            List<Estudiante> estudiantes = new List<Estudiante>();
            estudiantes.Add(new Estudiante(12345678, "Eva ", "medina", "Quinto semestre"));
            estudiantes.Add(new Estudiante(19023489, "Carlos ", "Sanchez", "cuarto semestre"));
            estudiantes.Add(new Estudiante(1112222, "Laura ", "lopez", "segundo semestre"));
            return estudiantes;
        }

        public static Estudiante BuscarEstudiante(int identificacion, List<Estudiante> listado)
        {

            foreach (Estudiante item in listado)
            {
                if (identificacion == item.Identificacion)
                {
                    return item;
                }
            }
            return null;
        }
        public static List<Asignatura> llenarAsignatura()
        {
            List<Asignatura> asignaturas = new List<Asignatura>();
            asignaturas.Add(new Asignatura("12345", "A"));
            asignaturas.Add(new Asignatura("67890", "B"));
            asignaturas.Add(new Asignatura("34567", "C"));

            return asignaturas;
        }
        public static Matricula VerificarMatricula(String codigo, List<Matricula> listado)
        {

            foreach (Matricula item in listado)
            {
                Console.WriteLine(item.CodigoAsig + "-" + codigo);
                if (codigo.Equals(item.CodigoAsig))
                {
                    return item;
                }
            }
            return null;
        }
        public static Asignatura BuscarAsignatura(String codigo, List<Asignatura> listado)
        {
            foreach (Asignatura item in listado)
            {
                if (codigo.Equals(item.Codigo))
                {
                    return item;
                }
            }
            return null;
        }
        public static Grupo BuscarGrupo(String codigo, List<Grupo> listado)
        {
            foreach (Grupo item in listado)
            {
                if (codigo.Equals(item.CodGrupo))
                {
                    return item;
                }
            }
            return null;
        }
        public static Matricula BuscarAsignaturaMatriculadas(string codAsignatura, List<Matricula> listado)
        {
            foreach (Matricula item in listado)
            {
                if (codAsignatura.Equals(item.CodigoAsig))
                {
                    ;
                    return item;
                }
            }
            return null;
        }
        public static void ConsultarGrupo(String codigo, List<Grupo> listado)
        {
            foreach (Grupo item in listado)
            {
                if (codigo.Equals(item.CodAsignatura))
                {
                    Console.WriteLine(item.toString());
                    Console.WriteLine(" ");
                }
            }
        }
        public static bool BuscarMatricula(String horario, List<Matricula> listado)
        {
            foreach (Matricula item in listado)
            {
                Console.WriteLine(horario + "--" + item.Horario);
                if (horario.Equals(item.Horario))
                {
                    return true;
                }
            }
            return false;
        }
        public static void ConsultarMatricula(List<Matricula> listado)
        {
            foreach (Matricula item in listado)
            {
                Console.WriteLine(item.toString());
            }
        }
    }// fin del program

    public class Asignatura
    {
        public String Codigo;
        public String Nombre;

        public Asignatura()
        {

        }
        public Asignatura(String codigo, String nombre)
        {
            this.Codigo = codigo;
            this.Nombre = nombre;
        }

        public String toString()
        {
            return "Codigo es: " + this.Codigo + " "
                    + "Nombre :" + this.Nombre;
        }
    }
    public class Grupo
    {
        public String CodGrupo;
        public String Nombre;
        public int Cupos;
        public String Horario;
        public String CodAsignatura;

        public Grupo()
        {

        }
        public Grupo(String codGrupo, String nombre, int cupos, String horario, String codAsignatura)
        {
            this.CodGrupo = codGrupo;
            this.Nombre = nombre;
            this.Cupos = cupos;
            this.Horario = horario;
            this.CodAsignatura = codAsignatura;
        }

        public void DescontarCupos()
        {
            this.Cupos = this.Cupos - 1;
        }

        public String toString()
        {
            return "Codigo Grupo es: " + this.CodGrupo + "\n" +
                "Nombr es: " + this.Nombre + "\n"
                + "Cupos disponibeles " + this.Cupos + "\n" +
                " Horarios : " + this.Horario + "\n" +
                " Codigo Asigantura : " + this.CodAsignatura;
        }
    }
    public class Matricula
    {
        public int CodMatricula;
        public int Identificacion;
        public String NombreGrupo;
        public String NombreAsignatura;
        public String Horario;
        public String CodigoAsig;

        public Matricula(int codMatricula, int identificacion, String nombreGrupo, String nombreAsignatura, String horario, String codigoAsig)
        {
            this.CodMatricula = codMatricula;
            this.Identificacion = identificacion;
            this.NombreGrupo = nombreGrupo;
            this.NombreAsignatura = nombreAsignatura;
            this.Horario = horario;
            this.CodigoAsig = codigoAsig;
        }

        public Matricula()
        {

        }
        public String toString()
        {

            return "Codigo Matricula es: " + this.CodMatricula + "\n" +
                "Identificacion es: " + this.Identificacion + "\n"
                + "Nombre GRupos " + this.NombreGrupo + "\n" +
                "Nombre Asignatura : " + this.NombreAsignatura + "\n" +
                " Horario : " + this.Horario + "\n" +
                 " codAsignatura : " + this.CodigoAsig + "\n";
        }
    }

    public class Estudiante
    {
        public int Identificacion;
        public String Nombre;
        public String Apellido;
        public String Nsesmestre;

        public Estudiante(int identificacion, String nombre, String apellido, String nSesmestre)
        {
            this.Identificacion = identificacion;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Nsesmestre = nSesmestre;

        }
        public Estudiante()
        {

        }

        public String toString()
        {

            return "Identificacion del estudiante  es: " + this.Identificacion + "\n" +
                "Nombre : " + this.Nombre + "\n"
                + "Apellido : " + this.Apellido + "\n" +
                "Semestre curzado : " + this.Nsesmestre + "\n";
        }
    }
}






