﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using LacunaExpress.Pages.Mail;
using LacunaExpress.Pages.AccountPages;
using LacunaExpress.AccountManagement;
using LacunaExpress.Data;
using LacunaExpanseAPIWrapper;
using LacunaExpress.Pages.Bodies;
using LacunaExpress.Pages.Stations;
using LacunaExpress.Pages.Planets;
using LacunaExpress.Pages.EmpireWide;
using LacunaExpress.Pages.Map;
using LacunaExpress.Pages.About;
using LacunaExpress.Pages.Help;

namespace LacunaExpress.Pages
{
	public class Master : MasterDetailPage
	{
		AccountModel activeAccount;
		NavigationPage nav;

		static List<string> menuItems = new List<string>
		{
			"Planets", "Stations", "Mail", "Empire", "Map", "Account", "Help", "About"
		};

		ListView menu = new ListView { ItemsSource = menuItems };

		public Master()
		{
			nav = new NavigationPage(new Splash());
			//nav = new NavigationPage(new blank());
			nav.Title = "Lacuna Express";
			this.Detail = nav;
			this.Master = new ContentPage
			{
				Title = "Lacuna Express",
				Content = new StackLayout
				{
					Children = {
						menu
					}
				}
			};



			nav.BackgroundColor = Color.Black;
			nav.ToolbarItems.Add(new ToolbarItem
			{
				//Text = "Search",
				Icon = "ham.png",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(() => this.IsPresented = true)
			});

			menu.ItemTapped+= async (sender, e) =>
			{
				var text = menu.SelectedItem.ToString();//(menu.SelectedItem as MenuItems).ItemText;
				menu.SelectedItem = null;

				this.IsPresented = false;
				await nav.Navigation.PopToRootAsync();
				switch (text)
				{
					case "Planets":
						await nav.Navigation.PushAsync(new PlanetMenu());
						break;
					case "Stations":
						await nav.Navigation.PushAsync(new StationsMain());
						break;
					case "Mail":
						await nav.Navigation.PushAsync(new MessageList());
						break;
					case "Empire":
						await nav.Navigation.PushAsync(new EmpireWideMain());
						break;
					case "Map":
						await nav.Navigation.PushAsync(new MapMain());
						break;
					case "Account":
						await nav.Navigation.PushAsync(new AccountMain());
						break;
					case "Help":
						await nav.Navigation.PushAsync(new HelpMain());
						break;
					case "About":
						await nav.Navigation.PushAsync(new AboutMain());
						break;
				}
			};
			//GetActiveAccount();
		}

		async void  GetActiveAccount()
		{
			var accm = new AccountManagement.AccountManager();
			 activeAccount = await accm.GetActiveAccountAsync();
			 if (activeAccount == null)
			{
				await Navigation.PushModalAsync(new Login());
				//GetActiveAccount();
			}
		}

	}
}