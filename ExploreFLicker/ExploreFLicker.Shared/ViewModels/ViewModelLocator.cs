
using Autofac;
using Autofac.Features.OwnedInstances;
using ExploreFlicker.Helpers;
using ExploreFlickr.Strings;
using ExploreFlicker.DataServices;
using ExploreFlicker.ViewModels;
using ExploreFLicker.ViewModels;
using FlickrExplorer.DataServices.Interfaces;
using FlickrExplorer.DataServices.Requests;


#pragma warning disable 4014

namespace ExploreFlicker.Viewmodels
{
    public class ViewModelLocator
    {
        /// <summary>
        /// This container will be used internaly to resolve registered intances
        /// </summary>
        private static readonly IContainer Container;

        /// <summary>
        /// Register all required types in this static constructor,
        /// This will be executed the first time ViewModelLocator is accessed in code
        /// </summary>
        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            #region DataServices and Helpers

            builder.RegisterType<ViewModelLocator>().SingleInstance();
            builder.RegisterType<Resources>().SingleInstance();

            builder.RegisterType<NetworkHelper>().SingleInstance();
            builder.Register<INetworkHelper>(c => c.Resolve<NetworkHelper>());

            builder.RegisterType<ToastService>().SingleInstance();
            builder.Register<IToastService>(c => c.Resolve<ToastService>());

            //Registeration for Base request and message reslover
            builder.RegisterType<BaseRequest>();

            builder.RegisterType<RequestMessageResolver>();
            builder.Register<IRequestMessageResolver>(c => c.Resolve<RequestMessageResolver>());

            builder.RegisterType<FlickrService>().SingleInstance();
            builder.Register<IFlickrService>(c => c.Resolve<FlickrService>());

            //Register navigation Service
            builder.RegisterType<NavigationService>().SingleInstance().
                OnActivating(service =>
                {
                    //Here before the first request to Navigation Service is completed
                    //We will register the mapping between viewmodels and views
                    //This mapping will be used in navigation

                });
            builder.Register<INavigationService>(c => c.Resolve<NavigationService>());

            //Database Registerations
            #endregion

            #region ViewModels registeration

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<GalleryViewModel>();
            builder.RegisterType<MapViewModel>();
            builder.RegisterType<SearchViewModel>();
            #endregion

            Container = builder.Build();
        }

        #region Data Services and Helpers Properties

        public IToastService ToastService
        {
            get { return Container.Resolve<IToastService>(); }
        }

        public static Resources Resources
        {
            get { return Container.Resolve<Resources>(); }
        }

        public INavigationService NavigationService
        {
            get { return Container.Resolve<INavigationService>(); }
        }
        #endregion

        #region View Models Properties

        public MainViewModel MainViewModel
        {
            get { return Container.Resolve<MainViewModel>(); }
        }

        public SearchViewModel SearchViewModel
        {
            get { return Container.Resolve<SearchViewModel>(); }
        }

        public GalleryViewModel GalleryViewModel
        {
            get { return Container.Resolve<GalleryViewModel>(); }
        }

        public MapViewModel MapViewModel
        {
            get { return Container.Resolve<MapViewModel>(); }
        }

        #endregion

        /// <summary>
        ///Create instance in App.xaml resources so you can use it in data binding in all xaml pages
        ///And don't forget to set this instance from App.xaml.cs, So you can access it from code behind in pages(if required)
        /// </summary>
        public static ViewModelLocator Locator { set; get; }

    }
}
