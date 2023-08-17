using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace NewRecipeViewer.ViewModels
{
    class Ubm_PlmViewModle:ObservableRecipient, IRecipient<Plasma_Parameter>
    {
        private Plasma_Parameter  plm_parameter = new Plasma_Parameter();

        public Plasma_Parameter Plm_parameter
        {
            get { return plm_parameter; }
            set { plm_parameter = value; OnPropertyChanged(); }
        }
        private bool plm_current_enable;

        public bool Plm_current_enable
        {
            get { return plm_current_enable; }
            set { plm_current_enable = value; OnPropertyChanged(); }
        }
        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }   
        }


        #region Actual
        private float act_plasma_current;

        public float Act_plasma_current
        {
            get { return act_plasma_current; }
            set { act_plasma_current = value; OnPropertyChanged(); }    
        }

        #endregion

        public Ubm_PlmViewModle()
        {
            this.IsActive = true;
            //WeakReferenceMessenger.Default.Register<Plasma_Parameter, string>(this,"Plm_ParameterChanged", Plm_ParameterChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
        }

        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1://加热
                    Plm_current_enable = false;

                    break;
                case 2://glow_discharge
                    Plm_current_enable = true;
                    break;
                case 3://ion_etching
                    Plm_current_enable = false;
                    break;
                case 4://coating
                    Plm_current_enable = false;
                    break;
                default:
                    break;
            }
        }
        private void IDreleaseChanged(object recipient, string idrelease)
        {
            /*
                1	operator	operator
                2	supervisor	supervisor
                3	technician	technician
                4	developer	developer
                5	administrator	administrator
             */
            if (int.Parse(idrelease) == 6)
            {
                Edit_enable = true;
            }
            else
            {
                Edit_enable = false;
            }
        }

        public void Receive(Plasma_Parameter parameter)
        {
            Plm_parameter = parameter;
        }
    }
}
