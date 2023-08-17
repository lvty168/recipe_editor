using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;


namespace NewRecipeViewer.ViewModels
{
    class LocatorViewModle
    {
        Container container;
        public LocatorViewModle()
        {
            //容器初始化
            container = new Container();
            container.Register<MainViewModel>(Reuse.Singleton);
            container.Register<RecipeViewModel>(Reuse.Singleton);
            container.Register<BiasViewModel>(Reuse.Singleton);
            container.Register<CathodesViewModel>(Reuse.Singleton);
            container.Register<BendControlViewModel>(Reuse.Singleton);
            container.Register<PressureControlViewModel>(Reuse.Singleton);
            container.Register<GasChannelsViewModel>(Reuse.Singleton);
            container.Register<DurationViewModel>(Reuse.Singleton);
            container.Register<HeatingViewModle>(Reuse.Singleton);
            container.Register<CompareViewModle>(Reuse.Singleton);
            container.Register<StepViewModel>(Reuse.Singleton);
            container.Register<ProcessSequenceViewModle>(Reuse.Singleton);
            container.Register<Ubm_PlmViewModle>(Reuse.Singleton);


        }
        public MainViewModel main 
        {
            get
            {
                 return container.Resolve<MainViewModel>();
            }
        }
        public RecipeViewModel recipe
        {
            get
            {
                return container.Resolve<RecipeViewModel>();
            }
        }
        
        public BiasViewModel biasViewModel
        {
            get
            {
                return container.Resolve<BiasViewModel>();
            }
        }
        public CathodesViewModel cathodesViewModel
        {
            get
            {
                return container.Resolve<CathodesViewModel>();
            }
        }
        public BendControlViewModel bendControlViewModel
        {
            get
            {
                return container.Resolve<BendControlViewModel>();
            }
        }
        public PressureControlViewModel pressureControlViewModel
        {
            get
            {
                return container.Resolve<PressureControlViewModel>();
            }
        }
        public GasChannelsViewModel gasChannelsViewModel
        {
            get
            {
                return container.Resolve<GasChannelsViewModel>();
            }
        }
        public DurationViewModel durationViewModel
        {
            get
            {
                return container.Resolve<DurationViewModel>();
            }
        }
        public HeatingViewModle heatingViewModle
        {
            get
            {
                return container.Resolve<HeatingViewModle>();
            }
        }
        public CompareViewModle compareViewModle
        {
            get
            {
                return container.Resolve<CompareViewModle>();
            }
        }
        public StepViewModel stepViewModel
        {
            get
            {
                return container.Resolve<StepViewModel>();
            }
        }
        public ProcessSequenceViewModle processSequenceViewModle
        {
            get
            {
                return container.Resolve<ProcessSequenceViewModle>();
            }
        }
        public Ubm_PlmViewModle Ubm_PlmViewModel
        {
            get
            {
                return container.Resolve<Ubm_PlmViewModle>();
            }
        }
    }
}
