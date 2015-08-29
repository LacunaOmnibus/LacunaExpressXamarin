﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


using Xamarin.Forms;

namespace LacunaExpress.Pages
{
	public class Splash : ContentPage
	{
		AccountManagement.AccountManager accountManager = new AccountManagement.AccountManager();
		Label welcome = new Label
		{
			Text = "Welcome to Lacuna Express"
		};


		public Splash()
		{

			Content = new StackLayout
			{
				Children = {
					welcome
				}
			};

			LoadAccountsIntoPicker();
		}

		async void LoadAccountsIntoPicker()
		{
			AccountManagement.AccountManager accountManger = new AccountManagement.AccountManager();
			var activeAccount = await accountManager.GetActiveAccountAsync();
			if (activeAccount == null)
			{
				await Navigation.PushModalAsync(new LacunaExpress.Pages.AccountPages.Login());
			}
		}
	}
}