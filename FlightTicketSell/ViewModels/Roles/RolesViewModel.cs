using FlightTicketSell.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using FlightTicketSell.Models.Roles;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// The viewmodel for roles view
    /// </summary>
    public class RolesViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user group list
        /// </summary>
        public ObservableCollection<UserGroupWithCount> UserGroupList { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The load command
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        public RolesViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>(p => true, async p =>
            {
                await LoadData();
            });
        }

        /// <summary>
        /// Loads
        /// </summary>
        private async Task LoadData()
        {
            await LoadUserGroup();
        }

        private async Task LoadUserGroup()
        {
            using (var context = new FlightTicketSellEntities())
            {
                var userGroups = await context.NHOMNGUOIDUNGs
                    .Select(ngd => new UserGroupWithCount()
                    {
                        Code = ngd.MaNhom,
                        Name = ngd.MaNhom
                    })
                    .ToListAsync();

                foreach (var userGroup in userGroups)
                    userGroup.UserCount = context.NGUOIDUNGs.Where(ng => ng.MaNhom == userGroup.Code).Count();

                UserGroupList = new ObservableCollection<UserGroupWithCount>(userGroups);
            }
        }


        #endregion

        #region Helper

        #endregion
    } 
}
