using Bakery.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Bakery.ImportConsole
{
    public class ImportController
    {


        public static IEnumerable<Product> ReadFromCsv()
        {
            string file1 = "OrderItems.csv";
            string file2 = "Products.csv";
            string[][] matrix1 = MyFile.ReadStringMatrixFromCsv(file1, true);
            string[][] matrix2 = MyFile.ReadStringMatrixFromCsv(file2, true);


            var product = matrix2
                  .Select(line => new Product
                  {
                      ProductNr = line[0],
                      Name = line[1],
                      Price = Convert.ToDouble(line[2])
                  }).ToArray();

        
            var customer = matrix1
                .GroupBy(grp => grp[2] + ";" + grp[3] + ";" + grp[4])
                .Select(line => new Customer
                {
                    CustomerNr = line.Key.Split(';')[0],
                    LastName = line.Key.Split(';')[1],
                    FirstName = line.Key.Split(';')[2]


                }).ToArray();

            var order = matrix1
                   .GroupBy(grp => grp[0] + ";" + grp[1] + ";" + grp[2])
                   .Select(line => new Order
                   {
                       OrderNr = line.Key.Split(';')[0],
                       Date = DateTime.Parse(line.Key.Split(';')[1]),
                       Customer = customer.Where(c => c.CustomerNr == line.Key.Split(';')[2]).SingleOrDefault()
                   }).ToArray();


            var orderItem = matrix1
               .Select(line => new OrderItem
               {
                   Amount = Convert.ToInt32(line[6]),
                   Order = order.Where(o => o.OrderNr == line[0]).SingleOrDefault(),
                   Product = product.Where(p => p.ProductNr == line[5]).SingleOrDefault()
               }).ToArray();


            foreach (var products in product)
            {

                products.OrderItems = orderItem.Where(p => p.Product.ProductNr == products.ProductNr).ToArray();

            }
            return product;
        }

    }
}
