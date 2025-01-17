﻿using LacunaExpress.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using LacunaExpress.Scripts;
using LacunaExpanseAPIWrapper;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.Planets
{
	public class ShipTools : ContentPage
	{
		string spaceportID;
		string archminID;
		string planetID;
		List<ShipyardsModel> shipyardsList = new List<ShipyardsModel>();
		AccountModel account;
		Button glyphinator = new Button
		{
			Text = "Glyphinator",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		public ShipTools(AccountModel account, string planetName)
		{
			this.account = account;
			var mainLayout = new StackLayout
			{
				BackgroundColor = Color.FromRgb (0, 0, 128),
				Children = {
					glyphinator
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			glyphinator.Clicked += async (sender, e) =>
			{
				var systems = MapScripts.GetAllBodiesInRange30(account, 0, 0);
			};
			this.Appearing += async (sender, e) =>
			{
				planetID = (from b in account.Colonies
							where b.Value.Equals(planetName)
							select b.Key).First();
				LoadBuildingsAsync(account, planetID);
			};
		}
		async void LoadBuildingsAsync(AccountModel account, string bodyID)
		{
			var json = Body.GetBuildings(1, account.SessionID, bodyID);
			var s = new LacunaExpress.Data.Server();
			var response = await s.GetHttpResultAsync(account.Server, Body.url, json);
			if (response.result != null)
			{
				shipyardsList = (from b in response.result.buildings
						where b.Value.url.Equals(Shipyard.URL)
						select new ShipyardsModel
						{
							buildingID = b.Key,
							efficiency = b.Value.efficiency,
							level = b.Value.efficiency
						}).ToList();
				spaceportID = (from b in response.result.buildings
							   where b.Value.Equals(Spaceport.URL) && b.Value.efficiency.Equals("100")
							   select b.Key).First();
				archminID = (from b in response.result.buildings
							   where b.Value.Equals(Archaeology.URL) && b.Value.efficiency.Equals("100")
							   select b.Key).First();
			}
		}
	}

	public class ShipyardsModel
	{
		public string buildingID, level, efficiency;
	}

	class ships
	{
		string shiptype, shipid;
	}
}
