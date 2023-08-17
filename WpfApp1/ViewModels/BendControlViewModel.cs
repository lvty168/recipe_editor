using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
namespace NewRecipeViewer.ViewModels
{
    class BendControlViewModel:ObservableRecipient,IRecipient<GasBlendControlParameters>
    {
        #region 公共属性
        private Dictionary<string, float> gasChannel = new Dictionary<string, float>() { { "N2", 1 }, { "Ar", 2 }, { "H2", 3 }, { "Ar-plm", 4 }, { "Kr-H2", 5 } };
        
        #endregion
        private GasBlendControlParameters blend_control_parameter = new GasBlendControlParameters();

        public GasBlendControlParameters Blend_control_parameter
        {
            get { return blend_control_parameter; }
            set
            {
                blend_control_parameter = value;
                
                Blend_Attribute = blend_control_parameter.gas_blend_control;
                Blend_Maste_Attribute = blend_control_parameter.master_gas;
                
                

                OnPropertyChanged();
            }
        }
        public float Blend_Attribute
        {
            get { return Blend_control_parameter.gas_blend_control; }
            set
            {
                
                if (value == 0 )
                {

                    Visible = false;
                    blend_control_parameter = new GasBlendControlParameters();
                    Blend_Maste_Attribute = -1;
                    Blend_Slave_Attribute = -1;
                }
                else
                {
                    Blend_control_parameter.gas_blend_control = value;
                    Visible = true;
                    
                }
                OnPropertyChanged();
            }
        }
        public float Blend_Maste_Attribute
        {
            get { return Blend_control_parameter.master_gas; }
            set
            {
                Blend_control_parameter.master_gas = value;
                //控制跟随气体选项
                Dictionary<string, float> temp = new Dictionary<string, float>();
                foreach (var item in gasChannel)
                {
                    if(item.Value == value)
                    {
                        continue;
                    }
                    else
                    {
                        temp.Add(item.Key, item.Value);
                    }
                }
                Bld_slave = temp;
                OnPropertyChanged();
            }
        }
        public  float Blend_Slave_Attribute
        {
            get
            {
                return blend_control_parameter.slaver_gas;
            }
            set
            {
                blend_control_parameter.slaver_gas = value;
                OnPropertyChanged();
            }
        }
        #region Select Options
        private Dictionary<string, float> blend = new Dictionary<string, float>() { { "No", 0 }, { "Yes", 1 }};

        public Dictionary<string, float> Blend
        {
            get { return blend; }
            set
            {
                blend = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<string, float> blend_slave = new Dictionary<string, float>();

        public Dictionary<string, float> Bld_slave
        {
            get { return blend_slave; }
            set
            {
                blend_slave = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<string, float> blend_maste;

        public Dictionary<string, float> Bld_master
        {
            get { return blend_maste; }
            set
            {
                blend_maste = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region visible
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region EditEnable
        private bool edit_enable;

        public bool EditEnable
        {
            get { return edit_enable; }
            set
            {
                edit_enable = value;
                OnPropertyChanged();
            }
        }
        private bool id_edit_enable;
        public bool ID_Edit_Enable
        {
            get { return edit_enable; }
            set
            {
                edit_enable = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public BendControlViewModel()
        {
            this.IsActive = true;
            Bld_master = gasChannel;
            Blend_Attribute = 0;
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            //WeakReferenceMessenger.Default.Register<string, string>(this, "BlendMasterChanged", BlendMasterChanged);
            //WeakReferenceMessenger.Default.Register<string, string>(this, "BlendSelectChanged", BlendSelectChanged);
            //WeakReferenceMessenger.Default.Register<GasBlendControlParameters, string>(this, "GasBlendControlParameterChanged", ParameterChanged);
        }

        private void ParameterChanged(object recipient, GasBlendControlParameters parameters)
        {
            
            Blend_control_parameter = parameters;

        }

        private void BlendSelectChanged(object recipient, string selector_value)
        {
            if (float.Parse(selector_value) == 1)
            {
                Visible = true;
            }
            else
            {
                Visible = false;
            }
        }

        private void BlendMasterChanged(object recipient, string bend_master_key)
        {
            var temp = Copy.DeepCopyByReflect(gasChannel);
            temp.Remove(bend_master_key);
            Bld_slave = temp;
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
                EditEnable = true;
            }
            else
            {
                EditEnable = false;
            }
        }

        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1:
                    EditEnable = false;
                    break;
                case 2:
                    EditEnable = true;
                    break;
                case 3:
                    EditEnable = true;
                    break;
                case 4:
                    EditEnable = true;
                    break;
                default:
                    break;
            }

        }

        public void Receive(GasBlendControlParameters parameters)
        {
            Blend_control_parameter = parameters;
        }
    }
}
