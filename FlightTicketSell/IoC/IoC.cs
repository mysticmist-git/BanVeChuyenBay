using FlightTicketSell.Models;
using FlightTicketSell.ViewModels;
using Ninject;
using System;

namespace FlightTicketSell.IoC
{
    public class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        #endregion

        #region Construction


        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as the application starts up to ensure all
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of ApplicationViewModel
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
            Kernel.Bind<ReportViewModel>().ToConstant(new ReportViewModel());
            Kernel.Bind<BookViewModel>().ToConstant(new BookViewModel());
            Kernel.Bind<BookDetailViewModel>().ToConstant(new BookDetailViewModel());
            Kernel.Bind<SellPayViewModel>().ToConstant(new SellPayViewModel());
            Kernel.Bind<SellViewModel>().ToConstant(new SellViewModel());
            Kernel.Bind<TicketInfoFillingViewModel>().ToConstant(new TicketInfoFillingViewModel());
            Kernel.Bind<ChangeTicketViewModel>().ToConstant(new ChangeTicketViewModel());
            Kernel.Bind<SearchViewModel>().ToConstant(new SearchViewModel());
            Kernel.Bind<FlightDetailViewModel>().ToConstant(new FlightDetailViewModel());
            Kernel.Bind<FlightTicketAndBookViewModel>().ToConstant(new FlightTicketAndBookViewModel());
        }

        #endregion

        /// <summary>
        /// Get a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        /// <summary>
        /// Rebind
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Rebind<T>() where T : class, new()
        {
            Kernel.Rebind<T>().ToConstant(new T());
        }

    }
}
