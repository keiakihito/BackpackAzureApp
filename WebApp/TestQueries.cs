using System;
using System.Linq;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public static class TestQueries
    {
        public static void Run(AppDbContext db)
        {
            // Console.WriteLine("\n~~ Test Queries on Products ~~\n");
            
            // // Test 1: Read
            // Console.WriteLine("== 撥水加工ありの商品 (5)==");
            // var waterproofProducts = db.Products
            //     .Where(p=> p.waterproof )
            //     .Take(5)
            //     .ToList();
            //
            // foreach (var p in waterproofProducts)
            // {
            //     Console.WriteLine($"{p.id}: {p.brand} ¥{p.price} - {p.size} - 撥水: √");
            // }
            //
            // var mostExpensive = db.Products
            //     .OrderByDescending(p => p.price)
            //     .FirstOrDefault();
            // if (mostExpensive != null)
            // {
            //     Console.WriteLine("\n== 最高価格の商品 ==");
            //     Console.WriteLine($"{mostExpensive.id}: {mostExpensive.brand}¥{mostExpensive.price}");
            //     Console.WriteLine("\n");
            // }
            //
            // // Test2: Create
            // var newProducts = new Product
            // {
            //     brand = "TestBrand",
            //     price = 9999,
            //     weight_capacity_kg = 25.0f,
            //     waterproof = true,
            //     size = "Medium",
            //     color = "Black",
            //     style = "Urban",
            //     compartments = 3
            // };
            // db.Products.Add(newProducts);
            // db.SaveChanges();
            //
            // Console.WriteLine("✅Product 追加完了✅");
            // Console.WriteLine("\n");
            //
            // // Test3:  Update
            // var product = db.Products.FirstOrDefault(p => p.brand == "TestBrand");
            // if (product != null)
            // {
            //     product.color = "Red";
            //     db.SaveChanges();
            //     Console.WriteLine("✅Product 更新完了✅");
            //     Console.WriteLine("\n");
            // }
            //
            //
            // // Test4: Delete
            // product = db.Products.FirstOrDefault(p => p.brand == "TestBrand");
            // if (product != null)
            // {
            //     db.Products.Remove(product);
            //     db.SaveChanges();
            //     Console.WriteLine("✅Product 削除完了✅");
            //     Console.WriteLine("\n");
            // }


        } // end of Run
    } // end of TestQueries
} // end of WebApp



