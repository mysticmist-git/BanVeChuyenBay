﻿using FlightTicketSell.Models;
using System;
using System.Globalization;
using System.Linq;

namespace FlightTicketSell.ValueConverters
{
    public class AppViewToBooleanConverter : BaseValueConverter<AppViewToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((AppView)parameter)
            {
                case AppView.Search:
                    return new[] { AppView.Search, AppView.FlightDetail, AppView.TicketSoldOrBooked, AppView.Customer, AppView.FlightInfoEdit, AppView.BookDetail, AppView.BookPay, AppView.TicketInfoFilling, AppView.SellPay}.Any(x => x == (AppView)value);
                case AppView.Customer:
                    return new[] { AppView.Customer }.Any(x => x == (AppView)value);
                case AppView.Book:
                    return new[] { AppView.Book, AppView.BookDetail, AppView.BookPay }.Any(x => x == (AppView)value);
                case AppView.Sell:
                    return new[] { AppView.Sell, AppView.SellPay, AppView.TicketInfoFilling, AppView.ChangeTicket }.Any(x => x == (AppView)value);
                case AppView.Schedule:
                    return new[] { AppView.Schedule}.Any(x => x == (AppView)value);
                case AppView.Report:
                    return new[] { AppView.Report }.Any(x => x == (AppView)value);
                case AppView.Setting:
                    return new[] { AppView.Setting}.Any(x => x == (AppView)value);
                case AppView.Roles:
                    return new[] { AppView.Roles}.Any(x => x == (AppView)value);
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
