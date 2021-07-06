﻿using BethanyPieShop.Models;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }
        //public ViewResult List()
        //{
        //    var pieListViewModel = new PieListViewModel
        //    {
        //        CurrentCategory = "Cheese Cakes",
        //        Pies = _pieRepository.AllPies
        //    };
        //    return View(pieListViewModel);
        //}
        public IActionResult Detail(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                return NotFound();
            }
            return View(pie);
        }
        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory;
            if (!string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.PieId);
                currentCategory = category;
            }
            else
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            return View(new PieListViewModel()
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
            
        }
    }
}
