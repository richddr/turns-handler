﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TurneroCentroMED;
using TurneroCentroMED.Entidades;

namespace TurneroCentroMED
{
    [Serializable]public class Paciente
    {
        #region Propiedades

        public static int iD_Setter { get; set; }//Para Asignarle el iD a cada usuario
        public static int T_Num { get; set; }
        public int P_Id { get; set; }
        public bool OrdenDuradero { get; set; }
        public ListaEstudios Estudios { get; set; }
        public ListaEstudios Horario { get; set; }
        public DateTime HoraDisponible { get; set; }
        public int Ticket_Number { get; set; }
        public string WhoCreatedMe { get; set; }
        #endregion

        #region Constructores

        public Paciente()
        {
        }

        /// <summary>
        /// Constructor que recibe la lista de estudios seleccionada por el usuario.
        /// </summary>
        /// <param name="_Estudios"></param>
        public Paciente(ListaEstudios _Estudios, bool OrdenSeleccionado, string _WhoCreatedMe)
        {
            this.Ticket_Number = T_Num;
            this.P_Id = FrmPrincipal.Pacientes.Count;
            this.Estudios = _Estudios;
            this.Horario = new ListaEstudios();
            this.OrdenDuradero = OrdenSeleccionado;
            this.WhoCreatedMe = _WhoCreatedMe;

            iD_Setter++;
            T_Num++;
        }
        #endregion

        #region Metodos & Funciones

        public static void SetId()
        {
            try
            {
                T_Num = FrmPrincipal.Pacientes[FrmPrincipal.Pacientes.Count - 1].Ticket_Number;
                iD_Setter = FrmPrincipal.Pacientes[FrmPrincipal.Pacientes.Count - 1].P_Id;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                T_Num = 0;
                iD_Setter = 0;
            }
            finally
            {
                iD_Setter++;
                T_Num++;
            }
        }
        
        #endregion
    }

    [Serializable]public class ListaPaciente:List<Paciente>
    {
        #region Constructores
        
        public ListaPaciente()
        {
            
        }

        private ListaPaciente(ListaPaciente pacientes)
        {
            foreach (Paciente pactiente in pacientes)
            {
                Add(pactiente);
            }
        }
        #endregion
                      
        #region Metodos & Funciones
        
        /// <summary>
        /// Funcion que se encarga de actualizar los pacientes.
        /// </summary>
        public void UpdatePatients()
        {
            ListaPaciente temporal = new ListaPaciente(FrmPrincipal.Pacientes);

            for (int i = 0; i < FrmPrincipal.Pacientes.Count; i++)
            {
                foreach (Estudio estudio1 in FrmPrincipal.Pacientes[i].Horario)
                {
                    if (estudio1.Hora_de_turno.AddMinutes(estudio1.EstudioDuracion) < DateTime.Now)
                    {
                        temporal[i].Horario.Remove(estudio1);
                        break;
                    }
                }

                if (FrmPrincipal.Pacientes[i].Horario.Count == 0)
                    temporal.Remove(FrmPrincipal.Pacientes[i]);
            }

            for (int i = 0; i < temporal.Count; i++)
            {
                temporal[i].P_Id = i;
            }

            FrmPrincipal.Pacientes = temporal;
        }

        #endregion
    }
}
