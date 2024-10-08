﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoRazorApp.Data;
using TodoRazorApp.Models;
using TodoRazorApp.Pages.Shared;

namespace TodoRazorApp.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly TodoRazorApp.Data.TodoRazorAppContext _context;

        [ViewData]
        public string ErrorMsg { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "メールアドレス")]
        [DataType(DataType.EmailAddress)]
        public string? Mail { get; set; }

        [BindProperty]
        [Display(Name = "パスワード")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public IndexModel(TodoRazorApp.Data.TodoRazorAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 未入力チェック
            if (string.IsNullOrEmpty(Mail) || string.IsNullOrEmpty(Password))
            {
                ErrorMsg = "未入力項目があります";
                return Page();
            }

            // ログインチェック
            var account = await _context.Account.FirstOrDefaultAsync(m => m.Mail == Mail && m.Password == Password);
            if (account == null)
            {
                ErrorMsg = "メールアドレスまたはパスワードが誤っています";
                return Page();
            }

            LoginAccount.Login(account);

            return RedirectToPage("../Todos/Index");
        }
    }
}
