﻿using LacunaExpanseAPIWrapper;
using LacunaExpress.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using LacunaExpress.Scripts;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.Planets
{
	public class BuildingToolsView : ContentPage
	{
		List<string> damagedBuildingIDs = new List<string>();
		string planetID;
		Label planetNameLabel = new Label();
		Label avgbuildingLevel = new Label();
		Label buildingCount = new Label();
		Label damagedBuildings = new Label();
		Label bleeders = new Label();
		Button repairBuildings = new Button
		{
			Text = "Repair Buildings",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button fillWithSpacePorts = new Button
		{
			Text = "Fill with Spaceports",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button queueUpgrades = new Button
		{
			Text = "Queue Upgrades",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button destroyBleeders = new Button
		{
			Text = "Destroy Bleeders",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		public BuildingToolsView(AccountModel account, string planetName)
		{
			var mainLayout = new StackLayout
			{
				BackgroundColor = Color.FromRgb (0, 0, 128),
				Children = {
					planetNameLabel,
					avgbuildingLevel,
					buildingCount,
					damagedBuildings,
					bleeders,
					repairBuildings,
					fillWithSpacePorts,
					queueUpgrades,
					destroyBleeders
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			this.Appearing += (sender, e) =>
			{
				planetID = (from b in account.Colonies
								where b.Value.Equals(planetName)
								select b.Key).First();
				LoadBuildingsAsync(planetID, account);
			};
			repairBuildings.Clicked += async (sender, e) =>
			{
				var json = Body.RepairList(1, account.SessionID, planetID, damagedBuildingIDs);
				var s = new LacunaExpress.Data.Server();
				var response = await s.GetHttpResultAsync(account.Server, Body.url, json);
				if (response.result != null)
				{
					damagedBuildingIDs = BuildingToolsScripts.GetDamagedBuildingIDs(response.result.buildings);
					damagedBuildings.Text = "Number of damaged buildings: " + damagedBuildingIDs.Count;
					damagedBuildings.TextColor = Color.White;
				}
			};
		}
		async void LoadBuildingsAsync(string bodyID, AccountModel account)
		{
			var json = Body.GetBuildings(1, account.SessionID, bodyID);
			var s = new LacunaExpress.Data.Server();
			var response = await s.GetHttpResultAsync(account.Server, Body.url, json);
			if (response.result != null)
			{
				damagedBuildingIDs = BuildingToolsScripts.GetDamagedBuildingIDs(response.result.buildings);
				damagedBuildings.Text = "Number of damaged buildings: " + damagedBuildingIDs.Count;
				avgbuildingLevel.Text = "Average Level: " + BuildingToolsScripts.GetAvgBuildingLevel(response.result.buildings);
				avgbuildingLevel.TextColor = Color.White;
				buildingCount.Text = "Number of Buildings: " + response.result.buildings.Count;
				buildingCount.TextColor = Color.White;
			}
		}
	}
}
