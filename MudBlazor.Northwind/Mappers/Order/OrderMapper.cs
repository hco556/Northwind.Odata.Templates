using MudBlazor.Northwind.ViewModels;
using OdataClientModels = Northwind.Odata.Api.Client.Models;

namespace MudBlazor.Northwind.Mappers.Order
{
    public static class OrderMapper
    {
        public static List<OrderViewModel> MapOrdersToViewModels(List<OdataClientModels.Order>? orders)
        {
            if (orders is null) return new List<OrderViewModel>();

            var result = new List<OrderViewModel>(orders.Count);
            foreach (var order in orders)
            {
                if (order is null) continue; // tolerate null entries in the list
                OrderViewModel? OrderViewModel = MapOrderToViewModel(order);
                if(OrderViewModel is null) continue;
                result.Add(OrderViewModel);
            }

            return result;
        }
        public static OrderViewModel? MapOrderToViewModel(OdataClientModels.Order? order)
        {
            if (order is null) return null;

            return new OrderViewModel
            {
                OrderId = order.OrderId ?? 0,
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                OrderDate = Convert.ToDateTime(order.OrderDate)
            };
        }
    }
}
